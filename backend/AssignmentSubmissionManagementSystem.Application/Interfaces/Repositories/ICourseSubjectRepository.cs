using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface ICourseSubjectRepository
    : IRepository<CourseSubject>
{
    Task<CourseSubject?> GetByIdWithDetailsAsync(
        long courseSubjectId);


    Task<IReadOnlyList<CourseSubject>> GetAllWithDetailsAsync();


    Task<bool> ExistsAsync(
        long courseId,
        long subjectId);
}