using AssignmentSubmissionManagementSystem.Application.DTOs.Assignments;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AssignmentSubmissionManagementSystem.Domain.Enums;

using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
namespace AssignmentSubmissionManagementSystem.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AssignmentController : ControllerBase
{

    private readonly IAssignmentService _assignmentService;

    private readonly ITeacherProfileRepository _teacherRepository;

    private readonly ICurrentUserService _currentUserService;

    private readonly INotificationDispatcher _notificationDispatcher;



    public AssignmentController(

        IAssignmentService assignmentService,

        ITeacherProfileRepository teacherRepository,

        ICurrentUserService currentUserService,

        INotificationDispatcher notificationDispatcher

    )
    {

        _assignmentService = assignmentService;

        _teacherRepository = teacherRepository;

        _currentUserService = currentUserService;

        _notificationDispatcher = notificationDispatcher;

    }







    // POST api/Assignment

    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAssignmentDto dto)
    {

        var assignmentId =
            await _assignmentService
            .CreateAsync(dto);



        // A draft is deliberately silent: students cannot see it yet.
        if (dto.AssignmentStatus == AssignmentStatus.Published)
        {
            await _notificationDispatcher
                .AssignmentPublishedAsync(
                    assignmentId
                );
        }



        return Ok(new
        {
            message =
            "Assignment created successfully"
        });

    }







    // GET api/Assignment

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        var result =
            await _assignmentService
            .GetAllAsync();



        return Ok(result);

    }








    // GET api/Assignment/{id}

    [Authorize(Roles = "Teacher,Student,Admin,SuperAdmin")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {

        var result =
            await _assignmentService
            .GetByIdAsync(id);



        if (result == null)
            return NotFound();



        return Ok(result);

    }








    // GET api/Assignment/my
    // Current logged teacher assignments


    [Authorize(Roles = "Teacher")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyAssignments()
    {


        var userId =
            _currentUserService.UserId;



        var teacher =
            await _teacherRepository
            .GetByUserIdWithDetailsAsync(
                userId
            );



        if (teacher == null)
        {
            return NotFound(
                "Teacher profile not found"
            );
        }




        var result =
            await _assignmentService
            .GetByTeacherAsync(

                teacher.InstitutionId,

                teacher.TeacherId

            );



        return Ok(result);

    }








    // GET api/Assignment/teacher/{teacherId}

    [Authorize(Roles = "Teacher")]
    [HttpGet("teacher/{teacherId:long}")]
    public async Task<IActionResult> GetByTeacher(
        long teacherId)
    {

        await EnsureIsCurrentTeacherAsync(teacherId);



        // The institution now comes from the caller's JWT claim instead of the
        // hard-coded '1', which broke as soon as a second institution existed.
        var result =
            await _assignmentService
            .GetByTeacherAsync(

                _currentUserService.InstitutionId,

                teacherId

            );


        return Ok(result);

    }









    // PUT api/Assignment/{id}

    [Authorize(Roles = "Teacher")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(

        long id,

        UpdateAssignmentDto dto

    )
    {

        await EnsureOwnedByCurrentTeacherAsync(id);


        await _assignmentService
            .UpdateAsync(
                id,
                dto
            );



        return Ok(new
        {
            message =
            "Assignment updated successfully"
        });

    }









    // PATCH api/Assignment/{id}/status

    [Authorize(Roles = "Teacher")]
    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> ChangeStatus(

        long id,

        ChangeAssignmentStatusDto dto

    )
    {


        await EnsureOwnedByCurrentTeacherAsync(id);


        await _assignmentService
            .ChangeStatusAsync(
                id,
                dto
            );



        if (dto.Status == AssignmentStatus.Published)
        {
            await _notificationDispatcher
                .AssignmentPublishedAsync(
                    id
                );
        }



        return Ok(new
        {
            message =
            "Assignment status changed successfully"
        });

    }








    // GET api/Assignment/student/{academicYearId}

    [Authorize(Roles = "Student")]
    [HttpGet("student/{academicYearId:long}")]
    public async Task<IActionResult> GetStudentAssignments(

     long academicYearId

 )
    {

        var institutionId =
            _currentUserService.InstitutionId;



        var result =
            await _assignmentService
            .GetStudentAssignmentsAsync(

                institutionId,

                academicYearId

            );


        return Ok(result);

    }



    // ------------------------------------------------------------------
    // Ownership guards
    //
    // [Authorize(Roles = "Teacher")] only proves the caller is *a* teacher.
    // Without these checks any teacher could edit, publish or read another
    // teacher's assignment just by changing the id in the URL.
    // ------------------------------------------------------------------

    private async Task<long> GetCurrentTeacherIdAsync()
    {
        var teacher =
            await _teacherRepository
            .GetByUserIdWithDetailsAsync(
                _currentUserService.UserId
            );


        if (teacher == null)
        {
            throw new ForbiddenException(
                "No teacher profile is linked to this account."
            );
        }


        return teacher.TeacherId;
    }


    private async Task EnsureIsCurrentTeacherAsync(
        long teacherId)
    {
        var currentTeacherId =
            await GetCurrentTeacherIdAsync();


        if (currentTeacherId != teacherId)
        {
            throw new ForbiddenException(
                "You can only read your own assignments."
            );
        }
    }


    private async Task EnsureOwnedByCurrentTeacherAsync(
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


        var currentTeacherId =
            await GetCurrentTeacherIdAsync();


        if (assignment.TeacherId != currentTeacherId)
        {
            throw new ForbiddenException(
                "This assignment belongs to another teacher."
            );
        }
    }

}
