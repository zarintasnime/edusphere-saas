using AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentSubmissionManagementSystem.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{

    private readonly IStudentService _studentService;




    public StudentController(
        IStudentService studentService)
    {
        _studentService = studentService;
    }







    // POST: api/Student

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateStudentDto dto)
    {


        await _studentService
            .CreateAsync(dto);



        return Ok(new
        {
            message = "Student created successfully"
        });


    }









    // GET: api/Student


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {


        var result =
            await _studentService
            .GetAllAsync();



        return Ok(result);


    }









    // GET: api/Student/{id}


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {


        var result =
            await _studentService
            .GetByIdAsync(id);



        if (result == null)

            return NotFound();



        return Ok(result);


    }









    // PUT: api/Student/{id}


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(

        long id,

        UpdateStudentProfileDto dto

    )
    {


        await _studentService
            .UpdateAsync(id, dto);



        return Ok(new
        {
            message = "Student profile updated successfully"
        });


    }

    // GET: api/Student/my-profile

    [Authorize(Roles = "Student")]
    [HttpGet("my-profile")]
    public async Task<IActionResult> GetMyProfile()
    {

        var userIdClaim =
    User.FindFirst(
        System.Security.Claims.ClaimTypes.NameIdentifier
    );


        if (userIdClaim == null)
        {
            return Unauthorized(
                "User id claim not found"
            );
        }


        var userId =
            long.Parse(
                userIdClaim.Value
            );

        var result =
            await _studentService
            .GetMyProfileAsync(
                userId
            );


        if (result == null)

            return NotFound();



        return Ok(result);

    }



}