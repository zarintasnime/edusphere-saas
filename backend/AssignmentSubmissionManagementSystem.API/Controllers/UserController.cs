using AssignmentSubmissionManagementSystem.Application.DTOs.Users;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentSubmissionManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;


    public UserController(
        IUserService userService)
    {
        _userService = userService;
    }


    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserDto dto)
    {
        await _userService.CreateAsync(dto);


        return Ok(new
        {
            message = "User created successfully"
        });
    }




    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users =
            await _userService.GetAllAsync();


        return Ok(users);
    }




    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {
        var user =
            await _userService.GetByIdAsync(id);


        if (user == null)
            return NotFound(new
            {
                message = "User not found"
            });


        return Ok(user);
    }




    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpGet("institution/{institutionId:long}")]
    public async Task<IActionResult> GetByInstitution(
        long institutionId)
    {
        var users =
            await _userService
                .GetByInstitutionAsync(institutionId);


        return Ok(users);
    }




    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateUserDto dto)
    {
        await _userService.UpdateAsync(id, dto);


        return Ok(new
        {
            message = "User updated successfully"
        });
    }




    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(
        long id)
    {
        await _userService.DeleteAsync(id);


        return Ok(new
        {
            message = "User deleted successfully"
        });
    }
}