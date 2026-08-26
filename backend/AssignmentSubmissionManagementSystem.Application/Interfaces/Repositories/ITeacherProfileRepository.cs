using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface ITeacherProfileRepository
    : IRepository<TeacherProfile>
{

    Task<IReadOnlyList<TeacherProfile>>
        GetAllWithDetailsAsync();



    Task<TeacherProfile?>
        GetByIdWithDetailsAsync(
            long id);

    Task<TeacherProfile?> GetByUserIdWithDetailsAsync(
    long userId
);

}