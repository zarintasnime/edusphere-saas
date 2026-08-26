using AssignmentSubmissionManagementSystem.Application.DTOs.Institutions;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentSubmissionManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstitutionController : ControllerBase
{
    private readonly IInstitutionService _institutionService;


    public InstitutionController(
        IInstitutionService institutionService)
    {
        _institutionService = institutionService;
    }


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateInstitutionDto dto)
    {
        await _institutionService.CreateAsync(dto);

        return Ok(new
        {
            message = "Institution created successfully"
        });
    }


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var institutions =
            await _institutionService.GetAllAsync();

        return Ok(institutions);
    }


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {
        var institution =
            await _institutionService.GetByIdAsync(id);


        if (institution == null)
            return NotFound(new
            {
                message = "Institution not found"
            });


        return Ok(institution);
    }


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateInstitutionDto dto)
    {
        await _institutionService.UpdateAsync(id, dto);


        return Ok(new
        {
            message = "Institution updated successfully"
        });
    }
}