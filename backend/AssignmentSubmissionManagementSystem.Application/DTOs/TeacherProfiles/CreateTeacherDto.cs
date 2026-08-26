namespace AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;

public sealed class CreateTeacherDto
{

    // User Information

    public string TeacherName { get; set; } = string.Empty;


    public string Email { get; set; } = string.Empty;


    public string Password { get; set; } = string.Empty;



    // Institution

    public long InstitutionId { get; set; }



    // Department

    public long DepartmentId { get; set; }



    // Teacher Profile

    public string EmployeeCode { get; set; } = string.Empty;


    public string? Qualification { get; set; }


    public DateOnly? JoiningDate { get; set; }


    public bool IsActive { get; set; } = true;

}