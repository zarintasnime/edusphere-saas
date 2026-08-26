using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;


public class TeacherProfileRepository
    : Repository<TeacherProfile>,
      ITeacherProfileRepository
{

    private readonly ApplicationDbContext _context;



    public TeacherProfileRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }






    public async Task<IReadOnlyList<TeacherProfile>>
        GetAllWithDetailsAsync()
    {

        return await _context.TeacherProfiles

            .Include(x => x.User)

            .Include(x => x.Institution)

            .Include(x => x.Department)

            .AsNoTracking()

            .ToListAsync();

    }






    public async Task<TeacherProfile?>
        GetByIdWithDetailsAsync(
            long id)
    {

        return await _context.TeacherProfiles

            .Include(x => x.User)

            .Include(x => x.Institution)

            .Include(x => x.Department)

            .FirstOrDefaultAsync(
                x => x.TeacherId == id
            );

    }

    public async Task<TeacherProfile?>
    GetByUserIdWithDetailsAsync(
        long userId)
    {

        return await _context.TeacherProfiles

            .Include(x => x.User)

            .Include(x => x.Institution)

            .Include(x => x.Department)

            .FirstOrDefaultAsync(
                x => x.UserId == userId
            );

    }

}