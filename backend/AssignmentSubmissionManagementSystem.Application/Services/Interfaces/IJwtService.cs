namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(
        long userId,
        string email,
        long? institutionId,
        string role);
}