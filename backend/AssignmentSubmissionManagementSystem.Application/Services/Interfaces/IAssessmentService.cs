using AssignmentSubmissionManagementSystem.Application.DTOs.Assessments;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IAssessmentService
{
    Task<long> CreateAsync(
        CreateAssessmentDto dto);



    Task<AssessmentResponseDto?> GetByIdAsync(
        long assessmentId);



    Task<AssessmentResponseDto?> GetBySubmissionAsync(
        long institutionId,
        long submissionId);



    Task<IReadOnlyList<AssessmentResponseDto>> GetByTeacherAsync(
        long institutionId,
        long teacherId);



    Task UpdateAsync(
        long assessmentId,
        UpdateAssessmentDto dto);
}