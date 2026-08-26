using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Users;

public sealed class UserResponseDto
{
    public long UserId { get; set; }

    public long? InstitutionId { get; set; }

    public long RoleId { get; set; }

    public RoleType Role { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}