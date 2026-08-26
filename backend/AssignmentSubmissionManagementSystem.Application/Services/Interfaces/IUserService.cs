using AssignmentSubmissionManagementSystem.Application.DTOs.Users;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IUserService
{
    Task CreateAsync(
        CreateUserDto dto);


    Task<UserResponseDto?> GetByIdAsync(
        long userId);


    Task<IReadOnlyList<UserResponseDto>> GetAllAsync();


    Task<IReadOnlyList<UserResponseDto>> GetByInstitutionAsync(
        long institutionId);


    Task UpdateAsync(
        long userId,
        UpdateUserDto dto);


    Task DeleteAsync(
        long userId);
}