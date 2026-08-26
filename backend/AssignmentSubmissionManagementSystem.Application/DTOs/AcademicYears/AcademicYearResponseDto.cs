namespace AssignmentSubmissionManagementSystem.Application.DTOs.AcademicYears;

public sealed class AcademicYearResponseDto
{
    public long AcademicYearId { get; set; }

    public long InstitutionId { get; set; }

    public long BatchId { get; set; }

    public string BatchCode { get; set; } = string.Empty;

    public string BatchName { get; set; } = string.Empty;

    public string YearName { get; set; } = string.Empty;

    public int YearOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}