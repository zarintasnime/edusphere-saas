using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class AssignmentRepository
    : Repository<Assignment>, IAssignmentRepository
{
    private readonly ApplicationDbContext _context;


    public AssignmentRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }





    public async Task<IReadOnlyList<Assignment>> GetByTeacherAsync(
        long institutionId,
        long teacherId)
    {
        return await _context.Assignments

            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Course)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Subject)

            .Include(x => x.AcademicYear)

            .Where(x =>
                x.InstitutionId == institutionId &&
                x.TeacherId == teacherId)

            .AsNoTracking()

            .ToListAsync();
    }







    public async Task<IReadOnlyList<Assignment>> GetStudentAssignmentsAsync(
        long institutionId,
        long academicYearId)
    {
        return await _context.Assignments

            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Course)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Subject)

            .Include(x => x.AcademicYear)

            .Where(x =>
                x.InstitutionId == institutionId &&
                x.AcademicYearId == academicYearId &&
                x.AssignmentStatus ==
                    Domain.Enums.AssignmentStatus.Published &&
                x.IsActive)

            .AsNoTracking()

            .ToListAsync();
    }








    public async Task<Assignment?> GetByIdWithDetailsAsync(
        long institutionId,
        long assignmentId)
    {
        return await _context.Assignments

            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)

            .Include(x => x.TeacherSubject)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Course)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Subject)

            .Include(x => x.AcademicYear)

            .FirstOrDefaultAsync(x =>
                x.InstitutionId == institutionId &&
                x.AssignmentId == assignmentId);
    }








    public async Task<IReadOnlyList<Assignment>> GetPublishedAssignmentsAsync(
        long institutionId,
        long academicYearId)
    {
        return await _context.Assignments

            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Course)

            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Subject)

            .Include(x => x.AcademicYear)

            .Where(x =>
                x.InstitutionId == institutionId &&
                x.AcademicYearId == academicYearId &&
                x.AssignmentStatus ==
                    Domain.Enums.AssignmentStatus.Published &&
                x.IsActive)

            .AsNoTracking()

            .ToListAsync();
    }








    public async Task<bool> ExistsAsync(
        long institutionId,
        long teacherId,
        string title)
    {
        return await _context.Assignments

            .AnyAsync(x =>
                x.InstitutionId == institutionId &&
                x.TeacherId == teacherId &&
                x.Title == title);
    }

    public async Task<Assignment?> GetByIdWithDetailsAsync(long id)
    {
        return await _context.Assignments
            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)
            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Course)
            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Subject)
            .Include(x => x.AcademicYear)
            .FirstOrDefaultAsync(
                x => x.AssignmentId == id
            );
    }

    public async Task<IReadOnlyList<Assignment>> GetAllWithDetailsAsync()
    {
        return await _context.Assignments
            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)
            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Course)
            .Include(x => x.CourseSubject)
                .ThenInclude(x => x.Subject)
            .Include(x => x.AcademicYear)
            .AsNoTracking()
            .ToListAsync();
    }
}