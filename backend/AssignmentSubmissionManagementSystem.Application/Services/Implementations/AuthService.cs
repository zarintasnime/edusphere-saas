using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.Common.Settings;
using AssignmentSubmissionManagementSystem.Application.DTOs.Auth;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using Microsoft.Extensions.Options;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasherService passwordHasher,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task RegisterAsync(RegisterUserDto request)
    {
        var emailExists = await _userRepository.EmailExistsAsync(request.Email);
        if (emailExists)
            throw new ConflictException("Email already exists");

        var roleId = await _userRepository.GetRoleIdAsync(request.Role);

        var user = new User
        {
            InstitutionId = request.InstitutionId,
            RoleId = roleId,
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            IsDeleted = false
        };

        await _userRepository.AddAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedException("Invalid email or password");

        var passwordValid = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
            throw new UnauthorizedException("Invalid email or password");

        var token = _jwtService.GenerateToken(
            user.UserId,
            user.Email,
            user.InstitutionId,
            user.Role.RoleName.ToString());

        return new AuthResponseDto
        {
            UserId = user.UserId,
            InstitutionId = user.InstitutionId,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.RoleName,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes)
        };
    }
}