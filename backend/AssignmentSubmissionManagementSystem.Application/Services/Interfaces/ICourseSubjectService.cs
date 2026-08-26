using AssignmentSubmissionManagementSystem.Application.DTOs.CourseSubjects;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface ICourseSubjectService
{
    Task CreateAsync(
        CreateCourseSubjectDto dto);


    Task<CourseSubjectResponseDto?> GetByIdAsync(
        long courseSubjectId);


    Task<IReadOnlyList<CourseSubjectResponseDto>> GetAllAsync();
}