using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class TeacherSubject : BaseEntity
{
    public long TeacherSubjectId { get; set; }

    public long InstitutionId { get; set; }

    public long TeacherId { get; set; }

    public long CourseSubjectId { get; set; }

    public TeacherProfile Teacher { get; set; } = null!;

    public CourseSubject CourseSubject { get; set; } = null!;
}