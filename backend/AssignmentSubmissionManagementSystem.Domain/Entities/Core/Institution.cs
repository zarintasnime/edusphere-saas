using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Core;

public class Institution : BaseEntity
{
    public long InstitutionId { get; set; }

    public string InstitutionCode { get; set; } = string.Empty;

    public string InstitutionName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? UpdatedAt { get; set; }
}