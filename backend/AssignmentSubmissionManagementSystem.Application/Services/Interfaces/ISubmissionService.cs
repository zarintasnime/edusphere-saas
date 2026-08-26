using AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface ISubmissionService
{
    Task<long> CreateAsync(
     CreateSubmissionDto dto);


    Task CreateResubmissionAsync(
        CreateResubmissionDto dto);


    Task<SubmissionResponseDto?> GetByIdAsync(
        long submissionId);


    Task<IReadOnlyList<SubmissionResponseDto>> GetByAssignmentAsync(
        long institutionId,
        long assignmentId);


    Task<IReadOnlyList<SubmissionResponseDto>> GetByStudentAsync(
        long institutionId,
        long studentId);


    Task ChangeStatusAsync(
        long submissionId,
        ChangeSubmissionStatusDto dto);
}