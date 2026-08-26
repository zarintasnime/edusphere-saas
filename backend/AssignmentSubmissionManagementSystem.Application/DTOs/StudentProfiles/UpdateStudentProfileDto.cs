namespace AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;

public sealed class UpdateStudentProfileDto
{

    public string StudentCode { get; set; } = string.Empty;



    public DateOnly? AdmissionDate { get; set; }



    public bool IsActive { get; set; }

}