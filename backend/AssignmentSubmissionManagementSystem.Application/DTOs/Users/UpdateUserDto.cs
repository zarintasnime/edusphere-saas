using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Users;

public sealed class UpdateUserDto
{
    public RoleType Role { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }
}