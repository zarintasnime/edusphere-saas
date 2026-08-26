namespace AssignmentSubmissionManagementSystem.Application.DTOs.StudentEnrollments;

public sealed class CreateStudentEnrollmentDto
{
    public long InstitutionId { get; set; }

    public long StudentId { get; set; }

    public long AcademicYearId { get; set; }

    public string RollNumber { get; set; } = string.Empty;

    public DateOnly? EnrollmentDate { get; set; }

    public bool IsActive { get; set; } = true;
}