namespace AssignmentSubmissionManagementSystem.Application.DTOs.CourseSubjects;

public sealed class CourseSubjectResponseDto
{
    public long CourseSubjectId { get; set; }

    public long InstitutionId { get; set; }

    public long CourseId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public long SubjectId { get; set; }

    public string SubjectCode { get; set; } = string.Empty;

    public string SubjectName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}