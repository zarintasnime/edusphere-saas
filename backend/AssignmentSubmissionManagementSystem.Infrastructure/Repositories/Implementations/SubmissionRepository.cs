using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class SubmissionRepository
    : Repository<Submission>, ISubmissionRepository
{
    private readonly ApplicationDbContext _context;


    public SubmissionRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }







    public async Task<IReadOnlyList<Submission>> GetByAssignmentAsync(
        long institutionId,
        long assignmentId)
    {
        return await _context.Submissions
            .Include(x => x.Student)
                .ThenInclude(x => x.User)
            .Include(x => x.Assignment)
            .Include(x => x.SubmissionAttachments)
            .Where(x =>
                x.InstitutionId == institutionId &&
                x.AssignmentId == assignmentId)
            .OrderByDescending(x => x.SubmittedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Submission>> GetByStudentAsync(
        long institutionId,
        long studentId)
    {
        return await _context.Submissions
            .Include(x => x.Student)
                .ThenInclude(x => x.User)
            .Include(x => x.Assignment)
            .Include(x => x.SubmissionAttachments)
            .Where(x =>
                x.InstitutionId == institutionId &&
                x.StudentId == studentId)
            .OrderByDescending(x => x.SubmittedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Submission?> GetLatestSubmissionAsync(
        long institutionId,
        long assignmentId,
        long studentId)
    {
        return await _context.Submissions
            .Include(x => x.Student)
                .ThenInclude(x => x.User)
            .Include(x => x.Assignment)
            .Include(x => x.SubmissionAttachments)
            .Where(x =>
                x.InstitutionId == institutionId &&
                x.AssignmentId == assignmentId &&
                x.StudentId == studentId &&
                x.IsLatestSubmission)
            .OrderByDescending(x => x.SubmissionVersion)
            .FirstOrDefaultAsync();
    }









    public async Task<bool> HasSubmittedAsync(
        long institutionId,
        long assignmentId,
        long studentId)
    {
        return await _context.Submissions

            .AnyAsync(x =>
                x.InstitutionId == institutionId &&
                x.AssignmentId == assignmentId &&
                x.StudentId == studentId);
    }









    public async Task<int> GetNextVersionAsync(
        long institutionId,
        long assignmentId,
        long studentId)
    {
        var lastVersion =
            await _context.Submissions

                .Where(x =>
                    x.InstitutionId == institutionId &&
                    x.AssignmentId == assignmentId &&
                    x.StudentId == studentId)

                .MaxAsync(
                    x => (int?)x.SubmissionVersion)
                ?? 0;


        return lastVersion + 1;
    }

    public async Task<Submission?> GetByIdWithDetailsAsync(
    long submissionId)
    {
        return await _context.Submissions

            .Include(x => x.Assignment)

            .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Include(x => x.SubmissionAttachments)

            .FirstOrDefaultAsync(
                x => x.SubmissionId == submissionId
            );
    }
}