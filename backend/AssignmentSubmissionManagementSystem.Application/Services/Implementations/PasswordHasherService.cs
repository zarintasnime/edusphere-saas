using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class PasswordHasherService : IPasswordHasherService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }


    public bool VerifyPassword(
        string password,
        string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(
            password,
            passwordHash);
    }
}