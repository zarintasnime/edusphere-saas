using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class TeacherSubjectRepository
    : Repository<TeacherSubject>, ITeacherSubjectRepository
{
    private readonly ApplicationDbContext _context;


    public TeacherSubjectRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }





    public async Task<IReadOnlyList<TeacherSubject>> GetSubjectsByTeacherAsync(
        long institutionId,
        long teacherId)
    {
        return await _context.TeacherSubjects

            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Course)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Subject)

            .Where(x =>
                x.InstitutionId == institutionId &&
                x.TeacherId == teacherId)

            .AsNoTracking()

            .ToListAsync();
    }







    public async Task<IReadOnlyList<TeacherSubject>> GetTeachersBySubjectAsync(
        long institutionId,
        long subjectId)
    {
        return await _context.TeacherSubjects

            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Course)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Subject)

            .Where(x =>
                x.InstitutionId == institutionId &&
                x.CourseSubject.SubjectId == subjectId)

            .AsNoTracking()

            .ToListAsync();
    }







    public async Task<bool> ExistsAsync(
        long institutionId,
        long teacherId,
        long courseSubjectId)
    {
        return await _context.TeacherSubjects

            .AnyAsync(x =>
                x.InstitutionId == institutionId &&
                x.TeacherId == teacherId &&
                x.CourseSubjectId == courseSubjectId);
    }
}