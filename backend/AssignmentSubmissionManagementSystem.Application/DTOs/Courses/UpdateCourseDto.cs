namespace AssignmentSubmissionManagementSystem.Application.DTOs.Courses;

public sealed class UpdateCourseDto
{
    public long DepartmentId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}