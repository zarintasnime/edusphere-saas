namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface ICurrentUserService
{

    long UserId { get; }


    long InstitutionId { get; }


    string Role { get; }

}