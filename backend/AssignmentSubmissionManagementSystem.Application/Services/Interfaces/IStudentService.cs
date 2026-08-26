using AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IStudentService
{

    Task CreateAsync(
        CreateStudentDto dto);



    Task<StudentProfileResponseDto?> GetByIdAsync(
        long studentId);



    Task<IReadOnlyList<StudentProfileResponseDto>> GetAllAsync();



    Task UpdateAsync(
        long studentId,
        UpdateStudentProfileDto dto);

    Task<StudentProfileResponseDto?>
GetMyProfileAsync(
    long userId);

}