namespace AssignmentSubmissionManagementSystem.Application.DTOs.Subjects;

public sealed class CreateSubjectDto
{
    public long InstitutionId { get; set; }

    public string SubjectCode { get; set; } = string.Empty;

    public string SubjectName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}