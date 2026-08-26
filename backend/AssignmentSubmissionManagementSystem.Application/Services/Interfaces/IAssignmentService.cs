using AssignmentSubmissionManagementSystem.Application.DTOs.Assignments;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IAssignmentService
{
    Task<long> CreateAsync(
        CreateAssignmentDto dto);



    Task<AssignmentResponseDto?> GetByIdAsync(
        long assignmentId);



    Task<IReadOnlyList<AssignmentResponseDto>> GetAllAsync();



    Task<IReadOnlyList<AssignmentResponseDto>> GetByTeacherAsync(
        long institutionId,
        long teacherId);



    Task<IReadOnlyList<AssignmentResponseDto>> GetStudentAssignmentsAsync(
        long institutionId,
        long academicYearId);



    Task UpdateAsync(
        long assignmentId,
        UpdateAssignmentDto dto);



    Task ChangeStatusAsync(
        long assignmentId,
        ChangeAssignmentStatusDto dto);
}