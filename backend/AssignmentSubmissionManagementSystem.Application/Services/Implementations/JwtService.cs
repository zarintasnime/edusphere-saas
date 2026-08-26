using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AssignmentSubmissionManagementSystem.Application.Common.Settings;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;


    public JwtService(
        IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }


    public string GenerateToken(
        long userId,
        string email,
        long? institutionId,
        string role)
    {
        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                userId.ToString()),

            new(
                JwtRegisteredClaimNames.Email,
                email),

            new(
                "InstitutionId",
                institutionId?.ToString() ?? string.Empty),

            new(
                ClaimTypes.Role,
                role)
        };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.Key));


        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                _settings.ExpireMinutes),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}