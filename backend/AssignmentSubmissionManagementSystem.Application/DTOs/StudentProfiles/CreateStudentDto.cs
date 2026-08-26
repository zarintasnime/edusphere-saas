namespace AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;

public sealed class CreateStudentDto
{

    // User Information

    public string StudentName { get; set; } = string.Empty;



    public string Email { get; set; } = string.Empty;



    public string Password { get; set; } = string.Empty;





    // Institution

    public long InstitutionId { get; set; }





    // Student Profile Information

    public string StudentCode { get; set; } = string.Empty;



    public DateOnly? AdmissionDate { get; set; }



    public bool IsActive { get; set; } = true;

}