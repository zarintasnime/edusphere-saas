using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.DTOs.Auth;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentSubmissionManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly IUserService _userService;

    private readonly ICurrentUserService _currentUserService;


    public AuthController(
        IAuthService authService,
        IUserService userService,
        ICurrentUserService currentUserService)
    {
        _authService = authService;
        _userService = userService;
        _currentUserService = currentUserService;
    }


    /// <summary>
    /// Public self-registration. The role is forced to Student.
    ///
    /// Previously this endpoint accepted whatever RoleType the caller sent, which meant an
    /// anonymous user could register themselves as SuperAdmin. Teacher, Admin and SuperAdmin
    /// accounts are now created only through the admin-protected endpoints
    /// (POST /api/Teacher, POST /api/User).
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto request)
    {
        request.Role = RoleType.Student;

        await _authService.RegisterAsync(request);

        return Ok(new
        {
            message = "User registered successfully"
        });
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }


    /// <summary>
    /// Returns the currently authenticated user.
    /// The React client calls this on page refresh to rehydrate its auth state from
    /// the stored token instead of trusting a cached user object.
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var user = await _userService.GetByIdAsync(_currentUserService.UserId);

        if (user is null)
        {
            throw new NotFoundException("The signed-in user no longer exists.");
        }

        return Ok(user);
    }
}
