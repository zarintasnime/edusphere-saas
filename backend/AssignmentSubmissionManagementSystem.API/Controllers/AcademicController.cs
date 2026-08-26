using AssignmentSubmissionManagementSystem.Application.DTOs.AcademicYears;
using AssignmentSubmissionManagementSystem.Application.DTOs.Batches;
using AssignmentSubmissionManagementSystem.Application.DTOs.Courses;
using AssignmentSubmissionManagementSystem.Application.DTOs.Departments;
using AssignmentSubmissionManagementSystem.Application.DTOs.Subjects;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentSubmissionManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AcademicController : ControllerBase
{
    private readonly IAcademicService _academicService;


    public AcademicController(
        IAcademicService academicService)
    {
        _academicService = academicService;
    }



    // ==========================
    // Department
    // ==========================

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost("department")]
    public async Task<IActionResult> CreateDepartment(
        CreateDepartmentDto dto)
    {
        await _academicService.CreateDepartmentAsync(dto);

        return Ok(new
        {
            message = "Department created successfully"
        });
    }


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("departments")]
    public async Task<IActionResult> GetDepartments()
    {
        var result =
            await _academicService.GetDepartmentsAsync();

        return Ok(result);
    }


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("department/{id:long}")]
    public async Task<IActionResult> GetDepartment(
        long id)
    {
        var result =
            await _academicService.GetDepartmentByIdAsync(id);


        if (result == null)
            return NotFound();


        return Ok(result);
    }


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("department/{id:long}")]
    public async Task<IActionResult> UpdateDepartment(
        long id,
        UpdateDepartmentDto dto)
    {
        await _academicService
            .UpdateDepartmentAsync(id, dto);


        return Ok(new
        {
            message = "Department updated successfully"
        });
    }





    // ==========================
    // Course
    // ==========================

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost("course")]
    public async Task<IActionResult> CreateCourse(
        CreateCourseDto dto)
    {
        await _academicService.CreateCourseAsync(dto);


        return Ok(new
        {
            message = "Course created successfully"
        });
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses()
    {
        var result =
            await _academicService.GetCoursesAsync();


        return Ok(result);
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("course/{id:long}")]
    public async Task<IActionResult> GetCourse(
        long id)
    {
        var result =
            await _academicService.GetCourseByIdAsync(id);


        if (result == null)
            return NotFound();


        return Ok(result);
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("course/{id:long}")]
    public async Task<IActionResult> UpdateCourse(
        long id,
        UpdateCourseDto dto)
    {
        await _academicService
            .UpdateCourseAsync(id, dto);


        return Ok(new
        {
            message = "Course updated successfully"
        });
    }





    // ==========================
    // Subject
    // ==========================

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost("subject")]
    public async Task<IActionResult> CreateSubject(
        CreateSubjectDto dto)
    {
        await _academicService
            .CreateSubjectAsync(dto);


        return Ok(new
        {
            message = "Subject created successfully"
        });
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("subjects")]
    public async Task<IActionResult> GetSubjects()
    {
        var result =
            await _academicService.GetSubjectsAsync();


        return Ok(result);
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("subject/{id:long}")]
    public async Task<IActionResult> GetSubject(
        long id)
    {
        var result =
            await _academicService.GetSubjectByIdAsync(id);


        if (result == null)
            return NotFound();


        return Ok(result);
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("subject/{id:long}")]
    public async Task<IActionResult> UpdateSubject(
        long id,
        UpdateSubjectDto dto)
    {
        await _academicService
            .UpdateSubjectAsync(id, dto);


        return Ok(new
        {
            message = "Subject updated successfully"
        });
    }





    // ==========================
    // Batch
    // ==========================

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost("batch")]
    public async Task<IActionResult> CreateBatch(
        CreateBatchDto dto)
    {
        await _academicService
            .CreateBatchAsync(dto);


        return Ok(new
        {
            message = "Batch created successfully"
        });
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("batches")]
    public async Task<IActionResult> GetBatches()
    {
        var result =
            await _academicService.GetBatchesAsync();


        return Ok(result);
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("batch/{id:long}")]
    public async Task<IActionResult> GetBatch(
        long id)
    {
        var result =
            await _academicService.GetBatchByIdAsync(id);


        if (result == null)
            return NotFound();


        return Ok(result);
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("batch/{id:long}")]
    public async Task<IActionResult> UpdateBatch(
        long id,
        UpdateBatchDto dto)
    {
        await _academicService
            .UpdateBatchAsync(id, dto);


        return Ok(new
        {
            message = "Batch updated successfully"
        });
    }





    // ==========================
    // Academic Year
    // ==========================

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost("academic-year")]
    public async Task<IActionResult> CreateAcademicYear(
        CreateAcademicYearDto dto)
    {
        await _academicService
            .CreateAcademicYearAsync(dto);


        return Ok(new
        {
            message = "Academic year created successfully"
        });
    }



    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
    [HttpGet("academic-years")]
    public async Task<IActionResult> GetAcademicYears()
    {
        var result =
            await _academicService
                .GetAcademicYearsAsync();


        return Ok(result);
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("academic-year/{id:long}")]
    public async Task<IActionResult> GetAcademicYear(
        long id)
    {
        var result =
            await _academicService
                .GetAcademicYearByIdAsync(id);


        if (result == null)
            return NotFound();


        return Ok(result);
    }



    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("academic-year/{id:long}")]
    public async Task<IActionResult> UpdateAcademicYear(
        long id,
        UpdateAcademicYearDto dto)
    {
        await _academicService
            .UpdateAcademicYearAsync(id, dto);


        return Ok(new
        {
            message = "Academic year updated successfully"
        });
    }
}