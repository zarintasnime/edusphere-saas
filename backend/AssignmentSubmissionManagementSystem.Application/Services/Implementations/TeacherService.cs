using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class TeacherService : ITeacherService
{
    private readonly ITeacherProfileRepository _teacherRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Institution> _institutionRepository;
    private readonly IRepository<Department> _departmentRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IPasswordHasherService _passwordHasher;

    public TeacherService(
        ITeacherProfileRepository teacherRepository,
        IRepository<User> userRepository,
        IRepository<Institution> institutionRepository,
        IRepository<Department> departmentRepository,
        IRepository<Role> roleRepository,
        IPasswordHasherService passwordHasher)
    {
        _teacherRepository = teacherRepository;
        _userRepository = userRepository;
        _institutionRepository = institutionRepository;
        _departmentRepository = departmentRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task CreateAsync(CreateTeacherDto dto)
    {
        var institution = await _institutionRepository.GetByIdAsync(dto.InstitutionId);
        if (institution == null)
            throw new NotFoundException("Institution not found");

        var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
        if (department == null)
            throw new NotFoundException("Department not found");

        var roles = await _roleRepository.GetAllAsync();
        var teacherRole = roles.FirstOrDefault(x => x.RoleName == RoleType.Teacher);
        if (teacherRole == null)
            throw new NotFoundException("Teacher role not found");

        var user = new User
        {
            InstitutionId = dto.InstitutionId,
            RoleId = teacherRole.RoleId,
            FullName = dto.TeacherName,
            Email = dto.Email,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            IsActive = true
        };

        await _userRepository.AddAsync(user);

        var teacher = new TeacherProfile
        {
            InstitutionId = dto.InstitutionId,
            UserId = user.UserId,
            DepartmentId = dto.DepartmentId,
            EmployeeCode = dto.EmployeeCode,
            Qualification = dto.Qualification,
            JoiningDate = dto.JoiningDate,
            IsActive = dto.IsActive
        };

        await _teacherRepository.AddAsync(teacher);
    }

    public async Task<TeacherProfileResponseDto?> GetByIdAsync(long teacherId)
    {
        var teacher = await _teacherRepository.GetByIdWithDetailsAsync(teacherId);
        if (teacher == null)
            return null;

        return MapToResponse(teacher);
    }

    public async Task<IReadOnlyList<TeacherProfileResponseDto>> GetAllAsync()
    {
        var teachers = await _teacherRepository.GetAllWithDetailsAsync();
        return teachers.Select(MapToResponse).ToList();
    }

    public async Task UpdateAsync(long teacherId, UpdateTeacherProfileDto dto)
    {
        var teacher = await _teacherRepository.GetByIdAsync(teacherId);
        if (teacher == null)
            throw new NotFoundException("Teacher not found");

        var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
        if (department == null)
            throw new NotFoundException("Department not found");

        teacher.DepartmentId = dto.DepartmentId;
        teacher.EmployeeCode = dto.EmployeeCode;
        teacher.Qualification = dto.Qualification;
        teacher.JoiningDate = dto.JoiningDate;
        teacher.IsActive = dto.IsActive;
        teacher.UpdatedAt = DateTime.Now;

        _teacherRepository.Update(teacher);
        await _teacherRepository.SaveChangesAsync();
    }

    public async Task<TeacherProfileResponseDto?> GetByUserIdAsync(long userId)
    {
        var teacher = await _teacherRepository.GetByUserIdWithDetailsAsync(userId);
        if (teacher == null)
            return null;

        return MapToResponse(teacher);
    }

    private static TeacherProfileResponseDto MapToResponse(TeacherProfile teacher)
    {
        return new TeacherProfileResponseDto
        {
            TeacherId = teacher.TeacherId,
            InstitutionId = teacher.InstitutionId,
            InstitutionName = teacher.Institution?.InstitutionName ?? "",
            UserId = teacher.UserId,
            TeacherName = teacher.User?.FullName ?? "",
            Email = teacher.User?.Email ?? "",
            DepartmentId = teacher.DepartmentId,
            DepartmentName = teacher.Department?.DepartmentName ?? "",
            EmployeeCode = teacher.EmployeeCode,
            Qualification = teacher.Qualification,
            JoiningDate = teacher.JoiningDate,
            IsActive = teacher.IsActive,
            CreatedAt = teacher.CreatedAt,
            UpdatedAt = teacher.UpdatedAt
        };
    }
}