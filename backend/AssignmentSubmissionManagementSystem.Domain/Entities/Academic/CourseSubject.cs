using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class CourseSubject : BaseEntity
{
    public long CourseSubjectId { get; set; }

    public long InstitutionId { get; set; }

    public long CourseId { get; set; }

    public long SubjectId { get; set; }

    public Course Course { get; set; } = null!;

    public Subject Subject { get; set; } = null!;
}