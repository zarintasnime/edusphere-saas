using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AssignmentSubmissionManagementSystem.Application.Common.Settings;
using AssignmentSubmissionManagementSystem.Application.Services.Implementations;
using Microsoft.Extensions.Options;
using Xunit;

namespace AssignmentSubmissionManagementSystem.Tests;

public class JwtServiceTests
{
    private static JwtService CreateService() =>
        new(Options.Create(new JwtSettings
        {
            Key = "test-signing-key-that-is-long-enough-for-hs256",
            Issuer = "CampusFlow",
            Audience = "CampusFlow",
            ExpireMinutes = 60
        }));

    private static JwtSecurityToken Read(string token) =>
        new JwtSecurityTokenHandler().ReadJwtToken(token);

    [Fact]
    public void GenerateToken_carries_the_identity_the_api_authorizes_on()
    {
        var token = Read(CreateService().GenerateToken(42, "teacher@campusflow.dev", 1, "Teacher"));

        Assert.Equal("42", token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
        Assert.Equal("1", token.Claims.First(c => c.Type == "InstitutionId").Value);

        // [Authorize(Roles = "Teacher")] compares against the role name, so the
        // claim must hold the name and not the enum's integer value.
        Assert.Equal("Teacher", token.Claims.First(c => c.Type == ClaimTypes.Role).Value);
    }

    [Fact]
    public void GenerateToken_sets_the_configured_issuer_and_audience()
    {
        var token = Read(CreateService().GenerateToken(1, "admin@campusflow.dev", 1, "Admin"));

        Assert.Equal("CampusFlow", token.Issuer);
        Assert.Contains("CampusFlow", token.Audiences);
    }

    [Fact]
    public void GenerateToken_expires_after_the_configured_window()
    {
        var token = Read(CreateService().GenerateToken(1, "admin@campusflow.dev", 1, "Admin"));

        Assert.True(token.ValidTo > DateTime.UtcNow);
        Assert.True(token.ValidTo <= DateTime.UtcNow.AddMinutes(61));
    }
}
