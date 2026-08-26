namespace AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;

public sealed class UpdateTeacherProfileDto
{

    public long DepartmentId { get; set; }



    public string EmployeeCode { get; set; } = string.Empty;



    public string? Qualification { get; set; }



    public DateOnly? JoiningDate { get; set; }



    public bool IsActive { get; set; }

}