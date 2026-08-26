using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class Subject : BaseEntity
{
    public long SubjectId { get; set; }

    public long InstitutionId { get; set; }

    public string SubjectCode { get; set; } = string.Empty;

    public string SubjectName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? UpdatedAt { get; set; }

    public Institution Institution { get; set; } = null!;
}