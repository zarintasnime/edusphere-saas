namespace AssignmentSubmissionManagementSystem.Application.DTOs.AcademicYears;

public sealed class UpdateAcademicYearDto
{
    public long BatchId { get; set; }

    public string YearName { get; set; } = string.Empty;

    public int YearOrder { get; set; }

    public bool IsActive { get; set; }
}