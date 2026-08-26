using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Auth;

public sealed class RegisterUserDto
{
    public long? InstitutionId { get; set; }

    public RoleType Role { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
}