using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface ITeacherSubjectRepository
    : IRepository<TeacherSubject>
{
    Task<IReadOnlyList<TeacherSubject>> GetSubjectsByTeacherAsync(
        long institutionId,
        long teacherId);


    Task<IReadOnlyList<TeacherSubject>> GetTeachersBySubjectAsync(
        long institutionId,
        long subjectId);


    Task<bool> ExistsAsync(
        long institutionId,
        long teacherId,
        long courseSubjectId);
}