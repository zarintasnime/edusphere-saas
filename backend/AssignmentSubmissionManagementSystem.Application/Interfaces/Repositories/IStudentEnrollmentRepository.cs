using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;


namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;


public interface IStudentEnrollmentRepository
    : IRepository<StudentEnrollment>
{


    Task<IReadOnlyList<StudentEnrollment>>
    GetAllWithDetailsAsync();




    Task<StudentEnrollment?>
    GetByIdWithDetailsAsync(
        long id);




    Task<IReadOnlyList<StudentEnrollment>>
    GetStudentsByAcademicYearAsync(

        long institutionId,

        long academicYearId

    );





    Task<bool> ExistsAsync(

        long institutionId,

        long studentId,

        long academicYearId

    );

}