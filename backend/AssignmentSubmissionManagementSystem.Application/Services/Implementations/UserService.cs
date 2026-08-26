using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.DTOs.Users;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<User> _repository;
    private readonly IPasswordHasherService _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IRepository<User> repository,
        IPasswordHasherService passwordHasher)
    {
        _userRepository = userRepository;
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task CreateAsync(CreateUserDto dto)
    {
        var emailExists = await _userRepository.EmailExistsAsync(dto.Email);
        if (emailExists)
            throw new ConflictException("Email already exists");

        var roleId = await _userRepository.GetRoleIdAsync(dto.Role);

        var user = new User
        {
            InstitutionId = dto.InstitutionId,
            RoleId = roleId,
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            PhoneNumber = dto.PhoneNumber,
            IsActive = dto.IsActive,
            IsDeleted = false
        };

        await _repository.AddAsync(user);
    }

    public async Task<UserResponseDto?> GetByIdAsync(long userId)
    {
        var user = await _userRepository.GetByIdWithRoleAsync(userId);
        if (user == null)
            return null;

        return MapToResponse(user);
    }

    public async Task<IReadOnlyList<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllWithRoleAsync();
        return users.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<UserResponseDto>> GetByInstitutionAsync(long institutionId)
    {
        var users = await _userRepository.GetByInstitutionAsync(institutionId);
        return users.Select(MapToResponse).ToList();
    }

    public async Task UpdateAsync(long userId, UpdateUserDto dto)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        if (user.Email != dto.Email)
        {
            var emailExists = await _userRepository.EmailExistsAsync(dto.Email);
            if (emailExists)
                throw new ConflictException("Email already exists");
        }

        var roleId = await _userRepository.GetRoleIdAsync(dto.Role);

        user.RoleId = roleId;
        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.Now;

        _repository.Update(user);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(long userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        user.IsDeleted = true;
        user.IsActive = false;

        _repository.Update(user);
        await _repository.SaveChangesAsync();
    }

    private static UserResponseDto MapToResponse(User user)
    {
        return new UserResponseDto
        {
            UserId = user.UserId,
            InstitutionId = user.InstitutionId,
            RoleId = user.RoleId,
            Role = user.Role != null ? user.Role.RoleName : default,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}