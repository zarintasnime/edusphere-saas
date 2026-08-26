namespace AssignmentSubmissionManagementSystem.Application.DTOs.Courses;

public sealed class CourseResponseDto
{
    public long CourseId { get; set; }

    public long InstitutionId { get; set; }

    public long DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string CourseCode { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}