using System.Text;
using System.Text.Json.Serialization;
using AssignmentSubmissionManagementSystem.API.Filters;
using AssignmentSubmissionManagementSystem.API.Middleware;
using AssignmentSubmissionManagementSystem.Application;
using AssignmentSubmissionManagementSystem.Application.Common.Settings;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Infrastructure;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// Enable legacy timestamp behavior for Npgsql/PostgreSQL compatibility across DateTimes
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);


// ---------------------------------------------------------------------------
// CORS - origins come from configuration (Cors:AllowedOrigins) so the same build
// works locally and on a deployed host without recompiling.
// ---------------------------------------------------------------------------
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?? new[] { "http://localhost:5173", "http://localhost:5174" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// ---------------------------------------------------------------------------
// Controllers
//   - JsonStringEnumConverter: without it the API sends enums as integers
//     ("role": 2) while [Authorize(Roles = "Teacher")] compares strings.
//     The client now receives "Teacher", "Published", "UnderReview", ...
//   - ValidationFilter: actually runs the FluentValidation validators, which
//     were registered but never invoked.
// ---------------------------------------------------------------------------
builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<ValidationFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddScoped<ValidationFilter>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();


// ---------------------------------------------------------------------------
// Swagger
// ---------------------------------------------------------------------------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CampusFlow - Assignment & Submission Management API",
        Version = "v1",
        Description =
            "Role based assignment and submission management. "
            + "Log in via /api/Auth/login and paste the returned token below."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste the JWT token only - Swagger adds the 'Bearer ' prefix."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// ---------------------------------------------------------------------------
// Authentication
// ---------------------------------------------------------------------------
var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "The 'Jwt' configuration section is missing.");

if (string.IsNullOrWhiteSpace(jwtSettings.Key) || jwtSettings.Key.Length < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key must be at least 32 characters. Set it with user-secrets or an "
        + "environment variable - see README.md.");
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();


// ---------------------------------------------------------------------------
// Pipeline - the exception handler goes first so it catches everything below.
// ---------------------------------------------------------------------------
app.UseExceptionHandling();

// Swagger is left on outside development too: the deployed demo is the API's
// documentation, and there is no private data behind it that is not already
// protected by [Authorize].
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CampusFlow API v1");
    options.DocumentTitle = "CampusFlow API";
});

if (!app.Environment.IsDevelopment())
{
    // Only redirect outside development: a 307 in the middle of a CORS preflight
    // is a very common source of "blocked by CORS policy" during local dev.
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseCors("AllowReactApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow
}))
.AllowAnonymous();


// ---------------------------------------------------------------------------
// Migrate + seed on startup (idempotent - see DatabaseSeeder)
// ---------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("DatabaseSeeder");

    var passwordHasher = scope.ServiceProvider
        .GetRequiredService<IPasswordHasherService>();

    try
    {
        await DatabaseSeeder.SeedAsync(
            app.Services,
            passwordHasher.HashPassword,
            logger);
    }
    catch (Exception exception)
    {
        logger.LogWarning(
            exception,
            "Migration/seeding encountered an issue. Ensure PostgreSQL is running and your connection string is configured.");
    }
}

app.Run();
