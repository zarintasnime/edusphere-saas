using AssignmentSubmissionManagementSystem.Application.DTOs.Notifications;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface INotificationService
{
    Task CreateAsync(
        CreateNotificationDto dto);



    Task<NotificationResponseDto?> GetByIdAsync(
        long notificationId);



    Task<IReadOnlyList<NotificationResponseDto>> GetByUserAsync(
        long userId);

    Task<IReadOnlyList<NotificationResponseDto>> GetAllAsync();

    Task MarkAsReadAsync(
        long notificationId);
}