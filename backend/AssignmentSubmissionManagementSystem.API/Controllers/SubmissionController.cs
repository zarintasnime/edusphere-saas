using AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
namespace AssignmentSubmissionManagementSystem.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SubmissionController : ControllerBase
{

    private readonly ISubmissionService _submissionService;

    private readonly ICurrentUserService _currentUserService;

    private readonly IStudentService _studentService;

    private readonly IAssignmentService _assignmentService;

    private readonly ITeacherService _teacherService;

    private readonly INotificationDispatcher _notificationDispatcher;



    public SubmissionController(

        ISubmissionService submissionService,

        ICurrentUserService currentUserService,

        IStudentService studentService,

        IAssignmentService assignmentService,

        ITeacherService teacherService,

        INotificationDispatcher notificationDispatcher

    )
    {

        _submissionService = submissionService;

        _currentUserService = currentUserService;

        _studentService = studentService;

        _assignmentService = assignmentService;

        _teacherService = teacherService;

        _notificationDispatcher = notificationDispatcher;

    }









    // POST: api/Submission

    [Authorize(Roles = "Student")]
    [HttpPost]
    public async Task<IActionResult> Create(

        CreateSubmissionDto dto

    )
    {


        dto.InstitutionId =

            _currentUserService
            .InstitutionId;





        var student =

            await _studentService
            .GetMyProfileAsync(

                _currentUserService.UserId

            );




        if (student == null)
        {

            return NotFound(
                "Student profile not found"
            );

        }




        dto.StudentId =

            student.StudentId;






        var submissionId =
    await _submissionService
    .CreateAsync(dto);



        await _notificationDispatcher
            .SubmissionReceivedAsync(
                submissionId
            );



        return Ok(new
        {

            message =
            "Submission created successfully",

            submissionId

        });

    }












    // GET: api/Submission/student/{studentId}

    [Authorize(Roles = "Student")]
    [HttpGet("student/{studentId:long}")]
    public async Task<IActionResult> GetByStudent(

        long studentId

    )
    {


        var institutionId =

            _currentUserService
            .InstitutionId;



        // The role attribute only proves the caller is *a* student. Without
        // this check, changing the id in the URL reads someone else's work.
        var student =
            await _studentService
            .GetMyProfileAsync(
                _currentUserService.UserId
            );


        if (student == null
            || student.StudentId != studentId)
        {
            throw new ForbiddenException(
                "You can only read your own submissions."
            );
        }




        var result =

            await _submissionService
            .GetByStudentAsync(

                institutionId,

                studentId

            );




        return Ok(result);

    }












    // GET: api/Submission/assignment/{assignmentId}

    [Authorize(Roles = "Teacher")]
    [HttpGet("assignment/{assignmentId:long}")]
    public async Task<IActionResult> GetByAssignment(

        long assignmentId

    )
    {

        await EnsureAssignmentOwnedByCurrentTeacherAsync(assignmentId);



        var institutionId =

            _currentUserService
            .InstitutionId;




        var result =

            await _submissionService
            .GetByAssignmentAsync(

                institutionId,

                assignmentId

            );




        return Ok(result);

    }












    // GET: api/Submission/{id}

    [Authorize(Roles = "Student,Teacher")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(

        long id

    )
    {


        var result =

            await _submissionService
            .GetByIdAsync(id);




        if (result == null)

            return NotFound();




        return Ok(result);

    }












    // PATCH: api/Submission/{id}/status

    [Authorize(Roles = "Teacher")]
    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> ChangeStatus(

        long id,

        ChangeSubmissionStatusDto dto

    )
    {


        await _submissionService
            .ChangeStatusAsync(

                id,

                dto

            );




        return Ok(new
        {

            message =
            "Submission status changed successfully"

        });

    }



    // ------------------------------------------------------------------
    // A teacher may only read the submission queue of an assignment they
    // set themselves.
    // ------------------------------------------------------------------

    private async Task EnsureAssignmentOwnedByCurrentTeacherAsync(
        long assignmentId)
    {
        var assignment =
            await _assignmentService
            .GetByIdAsync(
                assignmentId
            );


        if (assignment == null)
        {
            throw new NotFoundException(
                "Assignment",
                assignmentId
            );
        }


        var teacher =
            await _teacherService
            .GetByUserIdAsync(
                _currentUserService.UserId
            );


        if (teacher == null
            || assignment.TeacherId != teacher.TeacherId)
        {
            throw new ForbiddenException(
                "This assignment belongs to another teacher."
            );
        }
    }

}
