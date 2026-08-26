using AssignmentSubmissionManagementSystem.Application.DTOs.CourseSubjects;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentSubmissionManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseSubjectController : ControllerBase
{
    private readonly ICourseSubjectService _courseSubjectService;


    public CourseSubjectController(
        ICourseSubjectService courseSubjectService)
    {
        _courseSubjectService = courseSubjectService;
    }



    // POST: api/CourseSubject
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCourseSubjectDto dto)
    {
        await _courseSubjectService
            .CreateAsync(dto);


        return Ok(new
        {
            message = "Course subject created successfully"
        });
    }





    // GET: api/CourseSubject
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result =
            await _courseSubjectService
                .GetAllAsync();


        return Ok(result);
    }





    // GET: api/CourseSubject/{id}
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {
        var result =
            await _courseSubjectService
                .GetByIdAsync(id);


        if (result == null)
            return NotFound();



        return Ok(result);
    }
}