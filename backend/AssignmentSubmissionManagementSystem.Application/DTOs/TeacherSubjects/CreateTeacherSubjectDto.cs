namespace AssignmentSubmissionManagementSystem.Application.DTOs.TeacherSubjects;

public sealed class CreateTeacherSubjectDto
{
    public long InstitutionId { get; set; }

    public long TeacherId { get; set; }

    public long CourseSubjectId { get; set; }
}