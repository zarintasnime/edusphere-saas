using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;


public class StudentProfileRepository
    : Repository<StudentProfile>,
      IStudentProfileRepository
{


    private readonly ApplicationDbContext _context;



    public StudentProfileRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }





    public async Task<IReadOnlyList<StudentProfile>>
        GetAllWithDetailsAsync()
    {


        return await _context.StudentProfiles


            .Include(x => x.User)


            .Include(x => x.Institution)


            .AsNoTracking()


            .ToListAsync();

    }






    public async Task<StudentProfile?>
        GetByIdWithDetailsAsync(
            long id)
    {


        return await _context.StudentProfiles


            .Include(x => x.User)


            .Include(x => x.Institution)


            .FirstOrDefaultAsync(
                x => x.StudentId == id);

    }

    public async Task<StudentProfile?>
     GetByUserIdWithDetailsAsync(
         long userId)
    {

        return await _context.StudentProfiles

            .Include(x => x.User)

            .Include(x => x.Institution)

            .FirstOrDefaultAsync(

                x => x.UserId == userId

            );

    }

}