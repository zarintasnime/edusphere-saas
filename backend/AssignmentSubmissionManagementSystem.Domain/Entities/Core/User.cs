using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Core;

public class User : BaseEntity
{
    public long UserId { get; set; }

    public long? InstitutionId { get; set; }

    public long RoleId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Institution? Institution { get; set; }

    public Role Role { get; set; } = null!;
}