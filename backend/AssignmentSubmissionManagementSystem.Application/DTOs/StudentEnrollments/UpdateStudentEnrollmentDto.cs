namespace AssignmentSubmissionManagementSystem.Application.DTOs.StudentEnrollments;

public sealed class UpdateStudentEnrollmentDto
{
    public long AcademicYearId { get; set; }

    public string RollNumber { get; set; } = string.Empty;

    public DateOnly? EnrollmentDate { get; set; }

    public bool IsActive { get; set; }
}