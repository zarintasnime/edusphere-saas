using AssignmentSubmissionManagementSystem.Application.Common.Settings;
using AssignmentSubmissionManagementSystem.Application.Services.Implementations;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentSubmissionManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);


        services.AddScoped<
            IInstitutionService,
            InstitutionService>();


        services.AddScoped<
            IPasswordHasherService,
            PasswordHasherService>();


        services.AddScoped<
            IJwtService,
            JwtService>();

        services.AddScoped<
    ICurrentUserService,
    CurrentUserService>();


        services.Configure<JwtSettings>(
            configuration.GetSection("Jwt"));

        services.AddScoped<
    IAuthService,
    AuthService>();

        services.AddScoped<
    IUserService,
    UserService>();

        services.AddScoped<
    IAcademicService,
    AcademicService>();

        services.AddScoped<ICourseSubjectService, CourseSubjectService>();

        services.AddScoped<
    ITeacherService,
    TeacherService>();

        services.AddScoped<
    ITeacherSubjectService,
    TeacherSubjectService>();

        services.AddScoped<
    IStudentService,
    StudentService>();

        services.AddScoped<
    IStudentEnrollmentService,
    StudentEnrollmentService>();

        services.AddScoped<
    IAssignmentService,
    AssignmentService>();

        services.AddScoped<
    ISubmissionService,
    SubmissionService>();


        services.AddScoped<
    IAssessmentService,
    AssessmentService>();

        services.AddScoped<
    INotificationService,
    NotificationService>();

        services.AddScoped<
    IAuditLogService,
    AuditLogService>();


        services.AddScoped<
    ISubmissionAttachmentService,
    SubmissionAttachmentService>();



        return services;
    }
}