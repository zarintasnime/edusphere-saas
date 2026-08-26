using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface IAssessmentRepository
    : IRepository<Assessment>
{

    Task<Assessment?> GetBySubmissionAsync(
        long institutionId,
        long submissionId);



    Task<Assessment?> GetByIdWithDetailsAsync(
        long institutionId,
        long assessmentId);

    Task<Assessment?> GetByIdWithDetailsAsync(
        long assessmentId);



    Task<IReadOnlyList<Assessment>> GetByTeacherAsync(
        long institutionId,
        long teacherId);



    Task<bool> ExistsBySubmissionAsync(
        long institutionId,
        long submissionId);
}