using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface ITeacherService
{

    Task CreateAsync(
        CreateTeacherDto dto);



    Task<TeacherProfileResponseDto?> GetByIdAsync(
        long teacherId);



    Task<IReadOnlyList<TeacherProfileResponseDto>> GetAllAsync();



    Task UpdateAsync(
        long teacherId,
        UpdateTeacherProfileDto dto);


    Task<TeacherProfileResponseDto?> GetByUserIdAsync(
    long userId
);
}