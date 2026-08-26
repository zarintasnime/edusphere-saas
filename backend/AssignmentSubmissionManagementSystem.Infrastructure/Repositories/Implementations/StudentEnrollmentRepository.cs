using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;



namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;


public class StudentEnrollmentRepository
    : Repository<StudentEnrollment>,
      IStudentEnrollmentRepository
{


    private readonly ApplicationDbContext _context;




    public StudentEnrollmentRepository(
        ApplicationDbContext context)
        : base(context)
    {

        _context = context;

    }







    public async Task<IReadOnlyList<StudentEnrollment>>
    GetAllWithDetailsAsync()
    {


        return await _context.StudentEnrollments


            .Include(x => x.Student)

                .ThenInclude(x => x.User)



            .Include(x => x.AcademicYear)



            .AsNoTracking()



            .ToListAsync();


    }










    public async Task<StudentEnrollment?>
    GetByIdWithDetailsAsync(
        long id)
    {


        return await _context.StudentEnrollments


            .Include(x => x.Student)

                .ThenInclude(x => x.User)



            .Include(x => x.AcademicYear)



            .FirstOrDefaultAsync(

                x => x.EnrollmentId == id

            );


    }









    public async Task<IReadOnlyList<StudentEnrollment>>
    GetStudentsByAcademicYearAsync(

        long institutionId,

        long academicYearId

    )
    {


        return await _context.StudentEnrollments


            .Include(x => x.Student)

                .ThenInclude(x => x.User)



            .Include(x => x.AcademicYear)



            .Where(x =>

                (institutionId <= 0 || x.InstitutionId == institutionId) &&

                x.AcademicYearId == academicYearId

            )



            .AsNoTracking()



            .ToListAsync();


    }









    public async Task<bool> ExistsAsync(

        long institutionId,

        long studentId,

        long academicYearId

    )
    {


        return await _context.StudentEnrollments


            .AnyAsync(x =>


                x.InstitutionId == institutionId &&

                x.StudentId == studentId &&

                x.AcademicYearId == academicYearId


            );


    }


}