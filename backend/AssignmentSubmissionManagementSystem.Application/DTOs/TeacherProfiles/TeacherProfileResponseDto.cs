namespace AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;

public sealed class TeacherProfileResponseDto
{

    public long TeacherId { get; set; }



    public long InstitutionId { get; set; }


    public string InstitutionName { get; set; } = string.Empty;



    public long UserId { get; set; }


    public string TeacherName { get; set; } = string.Empty;


    public string Email { get; set; } = string.Empty;



    public long DepartmentId { get; set; }


    public string DepartmentName { get; set; } = string.Empty;



    public string EmployeeCode { get; set; } = string.Empty;



    public string? Qualification { get; set; }



    public DateOnly? JoiningDate { get; set; }



    public bool IsActive { get; set; }



    public DateTime CreatedAt { get; set; }



    public DateTime? UpdatedAt { get; set; }

}