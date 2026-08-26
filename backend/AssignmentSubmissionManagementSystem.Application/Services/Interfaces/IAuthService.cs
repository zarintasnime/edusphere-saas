using AssignmentSubmissionManagementSystem.Application.DTOs.Auth;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(
        RegisterUserDto request);

    Task<AuthResponseDto> LoginAsync(
        LoginDto request);
}