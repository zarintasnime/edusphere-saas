using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly IStudentProfileRepository _studentRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Institution> _institutionRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IPasswordHasherService _passwordHasher;

    public StudentService(
        IStudentProfileRepository studentRepository,
        IRepository<User> userRepository,
        IRepository<Institution> institutionRepository,
        IRepository<Role> roleRepository,
        IPasswordHasherService passwordHasher)
    {
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _institutionRepository = institutionRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task CreateAsync(CreateStudentDto dto)
    {
        var institution = await _institutionRepository.GetByIdAsync(dto.InstitutionId);
        if (institution == null)
            throw new NotFoundException("Institution not found");

        var roles = await _roleRepository.GetAllAsync();
        var studentRole = roles.FirstOrDefault(x => x.RoleName == RoleType.Student);
        if (studentRole == null)
            throw new NotFoundException("Student role not found");

        var exists = await _studentRepository.ExistsAsync(x => x.StudentCode == dto.StudentCode);
        if (exists)
            throw new ConflictException("Student code already exists");

        var user = new User
        {
            InstitutionId = dto.InstitutionId,
            RoleId = studentRole.RoleId,
            FullName = dto.StudentName,
            Email = dto.Email,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            IsActive = true
        };

        await _userRepository.AddAsync(user);

        var student = new StudentProfile
        {
            InstitutionId = dto.InstitutionId,
            UserId = user.UserId,
            StudentCode = dto.StudentCode,
            AdmissionDate = dto.AdmissionDate,
            IsActive = dto.IsActive
        };

        await _studentRepository.AddAsync(student);
    }

    public async Task<StudentProfileResponseDto?> GetByIdAsync(long studentId)
    {
        var student = await _studentRepository.GetByIdWithDetailsAsync(studentId);
        if (student == null)
            return null;

        return MapToResponse(student);
    }

    public async Task<StudentProfileResponseDto?> GetMyProfileAsync(long userId)
    {
        var student = await _studentRepository.GetByUserIdWithDetailsAsync(userId);
        if (student == null)
            return null;

        return MapToResponse(student);
    }

    public async Task<IReadOnlyList<StudentProfileResponseDto>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllWithDetailsAsync();
        return students.Select(MapToResponse).ToList();
    }

    public async Task UpdateAsync(long studentId, UpdateStudentProfileDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new NotFoundException("Student not found");

        student.StudentCode = dto.StudentCode;
        student.AdmissionDate = dto.AdmissionDate;
        student.IsActive = dto.IsActive;
        student.UpdatedAt = DateTime.Now;

        _studentRepository.Update(student);
        await _studentRepository.SaveChangesAsync();
    }

    private static StudentProfileResponseDto MapToResponse(StudentProfile student)
    {
        return new StudentProfileResponseDto
        {
            StudentId = student.StudentId,
            InstitutionId = student.InstitutionId,
            InstitutionName = student.Institution?.InstitutionName ?? "",
            UserId = student.UserId,
            StudentName = student.User?.FullName ?? "",
            Email = student.User?.Email ?? "",
            StudentCode = student.StudentCode,
            AdmissionDate = student.AdmissionDate,
            IsActive = student.IsActive,
            CreatedAt = student.CreatedAt,
            UpdatedAt = student.UpdatedAt
        };
    }
}