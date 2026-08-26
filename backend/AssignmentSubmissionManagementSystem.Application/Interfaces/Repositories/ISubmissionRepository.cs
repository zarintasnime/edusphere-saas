using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface ISubmissionRepository
    : IRepository<Submission>
{

    Task<IReadOnlyList<Submission>> GetByAssignmentAsync(
        long institutionId,
        long assignmentId);



    Task<IReadOnlyList<Submission>> GetByStudentAsync(
        long institutionId,
        long studentId);



    Task<Submission?> GetLatestSubmissionAsync(
        long institutionId,
        long assignmentId,
        long studentId);



    Task<bool> HasSubmittedAsync(
        long institutionId,
        long assignmentId,
        long studentId);



    Task<int> GetNextVersionAsync(
        long institutionId,
        long assignmentId,
        long studentId);

    Task<Submission?> GetByIdWithDetailsAsync(
    long submissionId);


}