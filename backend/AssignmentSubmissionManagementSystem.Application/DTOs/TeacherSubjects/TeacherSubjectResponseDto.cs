namespace AssignmentSubmissionManagementSystem.Application.DTOs.TeacherSubjects;

public sealed class TeacherSubjectResponseDto
{
    public long TeacherSubjectId { get; set; }

    public long InstitutionId { get; set; }

    public long TeacherId { get; set; }

    public string TeacherName { get; set; } = string.Empty;

    public long CourseSubjectId { get; set; }

    public long CourseId { get; set; }

    public string CourseName { get; set; } = string.Empty;

    public long SubjectId { get; set; }

    public string SubjectName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}