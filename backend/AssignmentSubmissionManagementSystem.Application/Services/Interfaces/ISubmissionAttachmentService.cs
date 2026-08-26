using AssignmentSubmissionManagementSystem.Application.DTOs.SubmissionAttachments;


namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;


public interface ISubmissionAttachmentService
{


    Task CreateAsync(
        CreateSubmissionAttachmentDto dto);



    Task<IReadOnlyList<SubmissionAttachmentResponseDto>>
        GetBySubmissionAsync(
            long institutionId,
            long submissionId);



    Task DeleteAsync(
        long attachmentId);


}