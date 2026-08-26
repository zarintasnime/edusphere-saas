using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class AssessmentRepository
    : Repository<Assessment>, IAssessmentRepository
{
    private readonly ApplicationDbContext _context;


    public AssessmentRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }






    public async Task<Assessment?> GetBySubmissionAsync(
        long institutionId,
        long submissionId)
    {
        return await _context.Assessments

            .Include(x => x.Submission)

            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)

            .Include(x => x.Policy)

            .FirstOrDefaultAsync(x =>
                x.InstitutionId == institutionId &&
                x.SubmissionId == submissionId);
    }









    public async Task<Assessment?> GetByIdWithDetailsAsync(
        long institutionId,
        long assessmentId)
    {
        return await _context.Assessments
            .Include(x => x.Submission)
                .ThenInclude(x => x.Student)
                    .ThenInclude(x => x.User)
            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)
            .Include(x => x.Policy)
            .FirstOrDefaultAsync(x =>
                x.InstitutionId == institutionId &&
                x.AssessmentId == assessmentId);
    }

    public async Task<Assessment?> GetByIdWithDetailsAsync(
        long assessmentId)
    {
        return await _context.Assessments
            .Include(x => x.Submission)
                .ThenInclude(x => x.Student)
                    .ThenInclude(x => x.User)
            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)
            .Include(x => x.Policy)
            .FirstOrDefaultAsync(x => x.AssessmentId == assessmentId);
    }









    public async Task<IReadOnlyList<Assessment>> GetByTeacherAsync(
        long institutionId,
        long teacherId)
    {
        return await _context.Assessments

            .Include(x => x.Submission).ThenInclude(x => x.Assignment)

            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)

            .Where(x =>
                x.InstitutionId == institutionId &&
                x.TeacherId == teacherId)

            .OrderByDescending(x => x.ReviewedAt)

            .AsNoTracking()

            .ToListAsync();
    }









    public async Task<bool> ExistsBySubmissionAsync(
        long institutionId,
        long submissionId)
    {
        return await _context.Assessments

            .AnyAsync(x =>
                x.InstitutionId == institutionId &&
                x.SubmissionId == submissionId);
    }
}