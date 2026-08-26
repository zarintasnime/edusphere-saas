using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Auth;

public sealed class AuthResponseDto
{
    public long UserId { get; set; }

    public long? InstitutionId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public RoleType Role { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}