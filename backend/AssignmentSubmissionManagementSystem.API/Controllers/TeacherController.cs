using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentSubmissionManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{

    private readonly ITeacherService _teacherService;



    public TeacherController(
        ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }







    // POST: api/Teacher
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTeacherDto dto)
    {

        await _teacherService
            .CreateAsync(dto);



        return Ok(new
        {
            message = "Teacher created successfully"
        });

    }








    // GET: api/Teacher
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        var result =
            await _teacherService
            .GetAllAsync();



        return Ok(result);

    }








    // GET: api/Teacher/{id}
    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {

        var result =
            await _teacherService
            .GetByIdAsync(id);



        if (result == null)
            return NotFound();



        return Ok(result);

    }









    // PUT: api/Teacher/{id}
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        UpdateTeacherProfileDto dto)
    {

        await _teacherService
            .UpdateAsync(id, dto);



        return Ok(new
        {
            message = "Teacher profile updated successfully"
        });

    }

    // GET: api/Teacher/user/{userId}

    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
    [HttpGet("user/{userId:long}")]
    public async Task<IActionResult> GetByUserId(
        long userId)
    {

        var result =
            await _teacherService
            .GetByUserIdAsync(
                userId
            );


        if (result == null)

            return NotFound();


        return Ok(result);

    }



}