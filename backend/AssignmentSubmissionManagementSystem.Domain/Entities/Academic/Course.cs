using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class Course : BaseEntity
{
    public long CourseId { get; set; }

    public long InstitutionId { get; set; }

    public long DepartmentId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? UpdatedAt { get; set; }

    public Department Department { get; set; } = null!;
}