using AssignmentSubmissionManagementSystem.Application.DTOs.Notifications;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentSubmissionManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    private readonly ICurrentUserService _currentUserService;


    public NotificationController(
        INotificationService notificationService,
        ICurrentUserService currentUserService)
    {
        _notificationService = notificationService;

        _currentUserService = currentUserService;
    }






    // POST: api/Notification
    [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateNotificationDto dto)
    {
        await _notificationService
            .CreateAsync(dto);


        return Ok(new
        {
            message = "Notification created successfully"
        });
    }








    // GET: api/Notification/{id}
    [Authorize]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id)
    {
        var result =
            await _notificationService
                .GetByIdAsync(id);


        if (result == null)
            return NotFound();


        return Ok(result);
    }








    // GET: api/Notification/user/{userId}
    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpGet("user/{userId:long}")]
    public async Task<IActionResult> GetByUser(
        long userId)
    {
        var result =
            await _notificationService
                .GetByUserAsync(userId);


        return Ok(result);
    }








    // PATCH: api/Notification/{id}/read
    [Authorize]
    [HttpPatch("{id:long}/read")]
    public async Task<IActionResult> MarkAsRead(
        long id)
    {
        await _notificationService
            .MarkAsReadAsync(id);


        return Ok(new
        {
            message = "Notification marked as read"
        });
    }



    // GET: api/Notification/my
    // Everything addressed to the signed-in user, newest first. The bell in the
    // client polls this rather than the id-based endpoint, which would let one
    // user read another user's notifications.

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMine()
    {
        var role = _currentUserService.Role;
        var notifications = (role == "SuperAdmin" || role == "Admin")
            ? await _notificationService.GetAllAsync()
            : await _notificationService.GetByUserAsync(_currentUserService.UserId);

        var ordered = notifications
            .OrderByDescending(item => item.CreatedAt)
            .ToList();

        return Ok(ordered);
    }

    // GET: api/Notification/my/unread-count
    [Authorize]
    [HttpGet("my/unread-count")]
    public async Task<IActionResult> GetMyUnreadCount()
    {
        var role = _currentUserService.Role;
        var notifications = (role == "SuperAdmin" || role == "Admin")
            ? await _notificationService.GetAllAsync()
            : await _notificationService.GetByUserAsync(_currentUserService.UserId);

        return Ok(new
        {
            unreadCount = notifications.Count(item => !item.IsRead)
        });
    }




    // PATCH: api/Notification/my/read-all

    [Authorize]
    [HttpPatch("my/read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var role = _currentUserService.Role;
        var notifications = (role == "SuperAdmin" || role == "Admin")
            ? await _notificationService.GetAllAsync()
            : await _notificationService.GetByUserAsync(_currentUserService.UserId);

        var unread = notifications
            .Where(item => !item.IsRead)
            .ToList();

        foreach (var notification in unread)
        {
            await _notificationService.MarkAsReadAsync(notification.NotificationId);
        }

        return Ok(new
        {
            message = "All notifications marked as read",
            updated = unread.Count
        });
    }

}
