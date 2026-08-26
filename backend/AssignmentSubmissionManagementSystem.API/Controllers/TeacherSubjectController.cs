using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherSubjects;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AssignmentSubmissionManagementSystem.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TeacherSubjectController : ControllerBase
{

    private readonly ITeacherSubjectService _teacherSubjectService;

    private readonly ITeacherService _teacherService;

    private readonly ICurrentUserService _currentUserService;



    public TeacherSubjectController(

        ITeacherSubjectService teacherSubjectService,

        ITeacherService teacherService,

        ICurrentUserService currentUserService

    )
    {

        _teacherSubjectService = teacherSubjectService;

        _teacherService = teacherService;

        _currentUserService = currentUserService;

    }








    // POST: api/TeacherSubject

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTeacherSubjectDto dto)
    {

        await _teacherSubjectService
            .CreateAsync(dto);



        return Ok(new
        {
            message =
            "Teacher subject assigned successfully"
        });

    }









    // GET: api/TeacherSubject/teacher/{teacherId}

    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
    [HttpGet("teacher/{teacherId:long}")]
    public async Task<IActionResult> GetSubjectsByTeacher(
        long teacherId)
    {


        var teacher =
            await _teacherService
            .GetByIdAsync(teacherId);



        if (teacher == null)

            return NotFound(
                "Teacher not found"
            );






        var result =
            await _teacherSubjectService
            .GetSubjectsByTeacherAsync(

                teacher.InstitutionId,

                teacherId

            );



        return Ok(result);

    }












    // GET: api/TeacherSubject/subject/{subjectId}


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("subject/{subjectId:long}")]
    public async Task<IActionResult> GetTeachersBySubject(
        long subjectId)
    {


        


        var result =
            await _teacherSubjectService
            .GetTeachersBySubjectAsync(

                _currentUserService.InstitutionId,

                subjectId

            );



        return Ok(result);

    }



}