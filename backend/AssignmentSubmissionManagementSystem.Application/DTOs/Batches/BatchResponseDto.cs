namespace AssignmentSubmissionManagementSystem.Application.DTOs.Batches;

public sealed class BatchResponseDto
{
    public long BatchId { get; set; }

    public long InstitutionId { get; set; }

    public long CourseId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public string BatchCode { get; set; } = string.Empty;

    public string BatchName { get; set; } = string.Empty;

    public int StartYear { get; set; }

    public int? EndYear { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}