using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class CourseSubjectRepository
    : Repository<CourseSubject>, ICourseSubjectRepository
{
    private readonly ApplicationDbContext _context;


    public CourseSubjectRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }



    public async Task<CourseSubject?> GetByIdWithDetailsAsync(
        long courseSubjectId)
    {
        return await _context.CourseSubjects
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .FirstOrDefaultAsync(x =>
                x.CourseSubjectId == courseSubjectId);
    }



    public async Task<IReadOnlyList<CourseSubject>> GetAllWithDetailsAsync()
    {
        return await _context.CourseSubjects
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .AsNoTracking()
            .ToListAsync();
    }



    public async Task<bool> ExistsAsync(
        long courseId,
        long subjectId)
    {
        return await _context.CourseSubjects
            .AnyAsync(x =>
                x.CourseId == courseId &&
                x.SubjectId == subjectId);
    }
}