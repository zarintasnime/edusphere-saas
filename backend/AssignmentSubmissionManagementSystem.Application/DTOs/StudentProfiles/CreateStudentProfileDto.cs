namespace AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;

public sealed class CreateStudentProfileDto
{
    public long InstitutionId { get; set; }

    public long UserId { get; set; }

    public string StudentCode { get; set; } = string.Empty;

    public DateOnly? AdmissionDate { get; set; }
}