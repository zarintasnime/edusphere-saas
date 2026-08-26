using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class AcademicYear : BaseEntity
{
    public long AcademicYearId { get; set; }

    public long InstitutionId { get; set; }

    public long BatchId { get; set; }

    public string YearName { get; set; } = string.Empty;

    public int YearOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? UpdatedAt { get; set; }

    public Batch Batch { get; set; } = null!;
}