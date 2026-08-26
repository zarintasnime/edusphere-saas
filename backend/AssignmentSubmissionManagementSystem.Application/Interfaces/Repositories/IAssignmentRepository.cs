using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface IAssignmentRepository
    : IRepository<Assignment>
{

    Task<IReadOnlyList<Assignment>> GetByTeacherAsync(
        long institutionId,
        long teacherId);



    Task<IReadOnlyList<Assignment>> GetStudentAssignmentsAsync(
        long institutionId,
        long academicYearId);



    Task<Assignment?> GetByIdWithDetailsAsync(
        long institutionId,
        long assignmentId);



    Task<IReadOnlyList<Assignment>> GetPublishedAssignmentsAsync(
        long institutionId,
        long academicYearId);



    Task<bool> ExistsAsync(
        long institutionId,
        long teacherId,
        string title);

    Task<Assignment?> GetByIdWithDetailsAsync(
        long id);

    Task<IReadOnlyList<Assignment>> GetAllWithDetailsAsync();
}