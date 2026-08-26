using AssignmentSubmissionManagementSystem.Application.DTOs.SubmissionAttachments;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AssignmentSubmissionManagementSystem.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SubmissionAttachmentController : ControllerBase
{


    private readonly ISubmissionAttachmentService _service;

    private readonly ICurrentUserService _currentUserService;



    public SubmissionAttachmentController(

        ISubmissionAttachmentService service,

        ICurrentUserService currentUserService

    )
    {

        _service = service;

        _currentUserService = currentUserService;

    }









    // POST: api/SubmissionAttachment

    [Authorize(Roles = "Student")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(

        [FromForm] CreateSubmissionAttachmentDto dto

    )
    {


        dto.InstitutionId =

            _currentUserService
                .InstitutionId;




        await _service
            .CreateAsync(dto);




        return Ok(new
        {

            message =
            "Submission attachment uploaded successfully"

        });


    }












    // GET: api/SubmissionAttachment/submission/{submissionId}


    [Authorize(Roles = "Student,Teacher")]
    [HttpGet("submission/{submissionId:long}")]
    public async Task<IActionResult> GetBySubmission(

        long submissionId

    )
    {


        var institutionId =

            _currentUserService
                .InstitutionId;




        var result =

            await _service
                .GetBySubmissionAsync(

                    institutionId,

                    submissionId

                );




        return Ok(result);


    }












    // DELETE: api/SubmissionAttachment/{id}


    [Authorize(Roles = "Student,Teacher")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(

        long id

    )
    {


        await _service
            .DeleteAsync(id);




        return Ok(new
        {

            message =
            "Attachment deleted successfully"

        });


    }



}