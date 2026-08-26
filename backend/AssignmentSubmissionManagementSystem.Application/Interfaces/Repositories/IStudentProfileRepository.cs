using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface IStudentProfileRepository
    : IRepository<StudentProfile>
{

    Task<IReadOnlyList<StudentProfile>>
        GetAllWithDetailsAsync();



    Task<StudentProfile?>
        GetByIdWithDetailsAsync(
            long id);



    Task<StudentProfile?>
        GetByUserIdWithDetailsAsync(
            long userId);

}