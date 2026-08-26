using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class Department : BaseEntity
{
    public long DepartmentId { get; set; }

    public long InstitutionId { get; set; }

    public string DepartmentCode { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? UpdatedAt { get; set; }

    public Institution Institution { get; set; } = null!;
}