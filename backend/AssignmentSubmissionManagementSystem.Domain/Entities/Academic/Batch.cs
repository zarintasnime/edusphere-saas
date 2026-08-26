using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class Batch : BaseEntity
{
    public long BatchId { get; set; }

    public long InstitutionId { get; set; }

    public long CourseId { get; set; }

    public string BatchCode { get; set; } = string.Empty;

    public string BatchName { get; set; } = string.Empty;

    public int StartYear { get; set; }

    public int? EndYear { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? UpdatedAt { get; set; }

    public Course Course { get; set; } = null!;
}