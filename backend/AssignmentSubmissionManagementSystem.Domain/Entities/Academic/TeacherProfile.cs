using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class TeacherProfile : BaseEntity
{

    public long TeacherId { get; set; }



    // Institution

    public long InstitutionId { get; set; }



    // Login User

    public long UserId { get; set; }



    // Department

    public long DepartmentId { get; set; }



    // Teacher Information


    public string EmployeeCode { get; set; } = string.Empty;


    public string? Qualification { get; set; }


    public DateOnly? JoiningDate { get; set; }



    public bool IsActive { get; set; } = true;



    public DateTime? UpdatedAt { get; set; }




    // Navigation Properties


    public Institution Institution { get; set; } = null!;



    public User User { get; set; } = null!;



    public Department Department { get; set; } = null!;

}