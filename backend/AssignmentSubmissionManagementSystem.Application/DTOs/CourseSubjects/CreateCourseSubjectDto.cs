namespace AssignmentSubmissionManagementSystem.Application.DTOs.CourseSubjects;

public sealed class CreateCourseSubjectDto
{
    public long InstitutionId { get; set; }

    public long CourseId { get; set; }

    public long SubjectId { get; set; }
}