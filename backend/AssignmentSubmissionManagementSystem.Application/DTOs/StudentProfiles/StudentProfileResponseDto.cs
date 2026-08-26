namespace AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;

public sealed class StudentProfileResponseDto
{

    public long StudentId { get; set; }



    public long InstitutionId { get; set; }


    public string InstitutionName { get; set; } = string.Empty;




    public long UserId { get; set; }



    public string StudentName { get; set; } = string.Empty;



    public string Email { get; set; } = string.Empty;




    public string StudentCode { get; set; } = string.Empty;



    public DateOnly? AdmissionDate { get; set; }



    public bool IsActive { get; set; }



    public DateTime CreatedAt { get; set; }



    public DateTime? UpdatedAt { get; set; }

}