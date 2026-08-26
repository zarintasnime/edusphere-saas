using AssignmentSubmissionManagementSystem.Application.DTOs.AuditLogs;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentSubmissionManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditLogController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;


    public AuditLogController(
        IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }






    // POST: api/AuditLog
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAuditLogDto dto)
    {
        await _auditLogService
            .CreateAsync(dto);


        return Ok(new
        {
            message = "Audit log created successfully"
        });
    }








    // GET: api/AuditLog/{id}
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {
        var result =
            await _auditLogService
                .GetByIdAsync(id);


        if (result == null)
            return NotFound();


        return Ok(result);
    }








    // GET: api/AuditLog
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result =
            await _auditLogService
                .GetAllAsync();


        return Ok(result);
    }




    // GET: api/AuditLog/user/{userId}
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("user/{userId:long}")]
    public async Task<IActionResult> GetByUser(
        long userId)
    {
        var result =
            await _auditLogService
                .GetByUserAsync(userId);


        return Ok(result);
    }




    // GET: api/AuditLog/entity/{entityName}/{entityId}
    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("entity/{entityName}/{entityId:long}")]
    public async Task<IActionResult> GetByEntity(
        string entityName,
        long entityId)
    {
        var result =
            await _auditLogService
                .GetByEntityAsync(
                    entityName,
                    entityId);


        return Ok(result);
    }
}