using AssignmentSubmissionManagementSystem.Application.DTOs.StudentEnrollments;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AssignmentSubmissionManagementSystem.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StudentEnrollmentController : ControllerBase
{

    private readonly IStudentEnrollmentService _enrollmentService;

    private readonly IStudentService _studentService;

    private readonly ICurrentUserService _currentUserService;



    public StudentEnrollmentController(
        IStudentEnrollmentService enrollmentService,
        IStudentService studentService,
        ICurrentUserService currentUserService)
    {

        _enrollmentService = enrollmentService;

        _studentService = studentService;

        _currentUserService = currentUserService;

    }









    // POST: api/StudentEnrollment

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateStudentEnrollmentDto dto)
    {


        await _enrollmentService
            .CreateAsync(dto);



        return Ok(new
        {
            message =
            "Student enrolled successfully"
        });


    }









    // GET: api/StudentEnrollment

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {


        var result =
            await _enrollmentService
            .GetAllAsync();



        return Ok(result);


    }









    // GET: api/StudentEnrollment/{id}

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {


        var result =
            await _enrollmentService
            .GetByIdAsync(id);



        if (result == null)

            return NotFound();



        return Ok(result);


    }









    // PUT: api/StudentEnrollment/{id}

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(

        long id,

        UpdateStudentEnrollmentDto dto

    )
    {


        await _enrollmentService
            .UpdateAsync(
                id,
                dto
            );



        return Ok(new
        {
            message =
            "Student enrollment updated successfully"
        });


    }









    // GET: api/StudentEnrollment/academic-year/{academicYearId}


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("academic-year/{academicYearId:long}")]
    public async Task<IActionResult> GetByAcademicYear(

        long academicYearId

    )
    {


        /*
          Temporary InstitutionId
          will be replaced with
          logged-in user's institution
        */


        var result =
            await _enrollmentService
            .GetStudentsByAcademicYearAsync(

                _currentUserService.InstitutionId,

                academicYearId

            );



        return Ok(result);


    }









    // GET: api/StudentEnrollment/batch/{batchId}


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("batch/{batchId:long}")]
    public async Task<IActionResult> GetByBatch(

        long batchId

    )
    {


        var result =
            await _enrollmentService
            .GetStudentsByBatchAsync(

                _currentUserService.InstitutionId,

                batchId

            );



        return Ok(result);


    }



    // GET: api/StudentEnrollment/my
    // Students cannot read the admin-only enrolment endpoints, but the client needs
    // the student's own academic year before it can list assignments. This returns
    // only the rows belonging to the signed-in student.

    [Authorize(Roles = "Student")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyEnrollments()
    {

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


        var all =
            await _enrollmentService
            .GetAllAsync();


        var mine =
            all
            .Where(enrollment =>
                enrollment.StudentId == student.StudentId)
            .OrderByDescending(enrollment =>
                enrollment.IsActive)
            .ToList();


        return Ok(mine);

    }

}
