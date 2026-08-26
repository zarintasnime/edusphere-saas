using AssignmentSubmissionManagementSystem.Application.DTOs.Assessments;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
namespace AssignmentSubmissionManagementSystem.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AssessmentController : ControllerBase
{

    private readonly IAssessmentService _assessmentService;

    private readonly ICurrentUserService _currentUserService;

    private readonly INotificationDispatcher _notificationDispatcher;

    private readonly ISubmissionService _submissionService;

    private readonly IAssignmentService _assignmentService;

    private readonly ITeacherService _teacherService;



    public AssessmentController(
        IAssessmentService assessmentService,
        ICurrentUserService currentUserService,
        INotificationDispatcher notificationDispatcher,
        ISubmissionService submissionService,
        IAssignmentService assignmentService,
        ITeacherService teacherService)
    {
        _assessmentService = assessmentService;
        _currentUserService = currentUserService;

        _notificationDispatcher = notificationDispatcher;

        _submissionService = submissionService;

        _assignmentService = assignmentService;

        _teacherService = teacherService;
    }






    // POST api/Assessment

    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAssessmentDto dto)
    {


        dto.InstitutionId =
            _currentUserService.InstitutionId;



        await EnsureSubmissionOwnedByCurrentTeacherAsync(dto.SubmissionId);


        var assessmentId =
            await _assessmentService
            .CreateAsync(dto);



        await _notificationDispatcher
            .AssessmentPublishedAsync(
                assessmentId
            );



        return Ok(new
        {
            message =
            "Assessment created successfully"
        });

    }









    // GET api/Assessment/{id}

    [Authorize(Roles = "Teacher,Student")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {


        var result =
            await _assessmentService
            .GetByIdAsync(id);



        if (result == null)
            return NotFound();



        return Ok(result);

    }









    // GET api/Assessment/submission/{submissionId}


    [Authorize(Roles = "Teacher,Student")]
    [HttpGet("submission/{submissionId:long}")]
    public async Task<IActionResult> GetBySubmission(
        long submissionId)
    {


        var result =
            await _assessmentService
            .GetBySubmissionAsync(

                _currentUserService.InstitutionId,

                submissionId

            );



        if (result == null)
            return NotFound();



        return Ok(result);

    }









    // GET api/Assessment/teacher/{teacherId}


    [Authorize(Roles = "Teacher")]
    [HttpGet("teacher/{teacherId:long}")]
    public async Task<IActionResult> GetByTeacher(
        long teacherId)
    {


        var result =
            await _assessmentService
            .GetByTeacherAsync(

                _currentUserService.InstitutionId,

                teacherId

            );



        return Ok(result);

    }









    // PUT api/Assessment/{id}


    [Authorize(Roles = "Teacher")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        UpdateAssessmentDto dto)
    {


        await _assessmentService
            .UpdateAsync(
                id,
                dto);



        return Ok(new
        {
            message =
            "Assessment updated successfully"
        });

    }



    // ------------------------------------------------------------------
    // Grading is limited to the teacher who set the assignment. Without
    // this, any teacher in the institution could mark - or re-mark - work
    // from a subject they do not teach.
    // ------------------------------------------------------------------

    private async Task EnsureSubmissionOwnedByCurrentTeacherAsync(
        long submissionId)
    {
        var submission =
            await _submissionService
            .GetByIdAsync(
                submissionId
            );


        if (submission == null)
        {
            throw new NotFoundException(
                "Submission",
                submissionId
            );
        }


        var assignment =
            await _assignmentService
            .GetByIdAsync(
                submission.AssignmentId
            );


        var teacher =
            await _teacherService
            .GetByUserIdAsync(
                _currentUserService.UserId
            );


        if (assignment == null
            || teacher == null
            || assignment.TeacherId != teacher.TeacherId)
        {
            throw new ForbiddenException(
                "You can only grade submissions for your own assignments."
            );
        }
    }

}
