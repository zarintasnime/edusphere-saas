using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface ISubmissionAttachmentRepository
    : IRepository<SubmissionAttachment>
{


    Task<IReadOnlyList<SubmissionAttachment>>
        GetBySubmissionAsync(
            long institutionId,
            long submissionId);



    Task<bool>
        ExistsAsync(
            long institutionId,
            long submissionId,
            string fileName);


}