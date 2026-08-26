using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;


public class SubmissionAttachmentRepository
    : Repository<SubmissionAttachment>,
      ISubmissionAttachmentRepository
{

    private readonly ApplicationDbContext _context;



    public SubmissionAttachmentRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }








    public async Task<IReadOnlyList<SubmissionAttachment>>
        GetBySubmissionAsync(
            long institutionId,
            long submissionId)
    {

        return await _context.SubmissionAttachments


            .Where(x =>

                x.InstitutionId == institutionId &&

                x.SubmissionId == submissionId

            )


            .OrderByDescending(x => x.CreatedAt)


            .AsNoTracking()


            .ToListAsync();

    }









    public async Task<bool>
        ExistsAsync(
            long institutionId,
            long submissionId,
            string fileName)
    {


        return await _context.SubmissionAttachments


            .AnyAsync(x =>

                x.InstitutionId == institutionId &&

                x.SubmissionId == submissionId &&

                x.FileName == fileName

            );


    }


}