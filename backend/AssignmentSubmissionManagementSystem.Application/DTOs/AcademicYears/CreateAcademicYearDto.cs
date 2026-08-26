namespace AssignmentSubmissionManagementSystem.Application.DTOs.AcademicYears;

public sealed class CreateAcademicYearDto
{
    public long InstitutionId { get; set; }

    public long BatchId { get; set; }

    public string YearName { get; set; } = string.Empty;

    public int YearOrder { get; set; }

    public bool IsActive { get; set; } = true;
}