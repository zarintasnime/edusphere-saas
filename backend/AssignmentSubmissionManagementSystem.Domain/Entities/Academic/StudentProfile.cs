using AssignmentSubmissionManagementSystem.Domain.Entities.Core;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class StudentProfile : BaseEntity
{

    public long StudentId { get; set; }



    // Institution

    public long InstitutionId { get; set; }



    // Login User

    public long UserId { get; set; }



    // Student Information

    public string StudentCode { get; set; } = string.Empty;



    public DateOnly? AdmissionDate { get; set; }



    public bool IsActive { get; set; } = true;



    public DateTime? UpdatedAt { get; set; }







    // Navigation


    public Institution Institution { get; set; } = null!;



    public User User { get; set; } = null!;

}