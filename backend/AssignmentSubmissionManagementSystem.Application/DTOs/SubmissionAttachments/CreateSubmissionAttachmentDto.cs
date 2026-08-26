using Microsoft.AspNetCore.Http;


namespace AssignmentSubmissionManagementSystem.Application.DTOs.SubmissionAttachments;


public sealed class CreateSubmissionAttachmentDto
{


    public long InstitutionId { get; set; }



    public long SubmissionId { get; set; }




    // Actual uploaded file

    public IFormFile File { get; set; } = null!;



    // These will be generated from uploaded file

    public string FileName { get; set; } = string.Empty;



    public string FilePath { get; set; } = string.Empty;



    public string? FileType { get; set; }



    public long? FileSize { get; set; }


}