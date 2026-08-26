using AssignmentSubmissionManagementSystem.Application.DTOs.Users;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;

public sealed class CreateTeacherProfileDto
{

    // Institution

    public long InstitutionId { get; set; }



    // Existing User (if account already created)

    public long UserId { get; set; }



    // Department

    public long DepartmentId { get; set; }




    // Teacher Profile Information


    public string EmployeeCode { get; set; } = string.Empty;


    public string? Qualification { get; set; }


    public DateOnly? JoiningDate { get; set; }


    public bool IsActive { get; set; } = true;

}