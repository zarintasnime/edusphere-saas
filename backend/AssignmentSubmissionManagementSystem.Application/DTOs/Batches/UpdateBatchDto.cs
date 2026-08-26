namespace AssignmentSubmissionManagementSystem.Application.DTOs.Batches;

public sealed class UpdateBatchDto
{
    public long CourseId { get; set; }

    public string BatchCode { get; set; } = string.Empty;

    public string BatchName { get; set; } = string.Empty;

    public int StartYear { get; set; }

    public int? EndYear { get; set; }

    public bool IsActive { get; set; }
}