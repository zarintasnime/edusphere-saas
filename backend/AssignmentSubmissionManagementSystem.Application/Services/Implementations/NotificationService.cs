using AssignmentSubmissionManagementSystem.Application.DTOs.Notifications;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Entities.System;
using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IRepository<User> _userRepository;


    public NotificationService(
        INotificationRepository notificationRepository,
        IRepository<User> userRepository)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
    }


    public async Task CreateAsync(
        CreateNotificationDto dto)
    {
        var user =
            await _userRepository
                .GetByIdAsync(dto.UserId);


        if (user == null)
            throw new NotFoundException("User not found");


        var notification = new Notification
        {
            InstitutionId = dto.InstitutionId,

            UserId = dto.UserId,

            Title = dto.Title,

            Message = dto.Message,

            NotificationType = dto.NotificationType,

            Channel = dto.Channel,

            ReferenceId = dto.ReferenceId,

            IsRead = false,

            ReadAt = null
        };


        await _notificationRepository
            .AddAsync(notification);
    }


    public async Task<NotificationResponseDto?> GetByIdAsync(
        long notificationId)
    {
        var notification =
            await _notificationRepository
                .GetByIdWithUserAsync(notificationId);


        if (notification == null)
            return null;


        return MapToResponse(notification);
    }


    public async Task<IReadOnlyList<NotificationResponseDto>> GetByUserAsync(
        long userId)
    {
        var notifications =
            await _notificationRepository
                .GetByUserAsync(userId);


        return notifications
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<IReadOnlyList<NotificationResponseDto>> GetAllAsync()
    {
        var notifications =
            await _notificationRepository
                .GetAllAsync();

        return notifications
            .Select(MapToResponse)
            .ToList();
    }


    public async Task MarkAsReadAsync(
        long notificationId)
    {
        await _notificationRepository
            .MarkAsReadAsync(notificationId);
    }


    private static NotificationResponseDto MapToResponse(
        Notification entity)
    {
        return new NotificationResponseDto
        {
            NotificationId = entity.NotificationId,

            InstitutionId = entity.InstitutionId,

            UserId = entity.UserId,

            Title = entity.Title,

            Message = entity.Message,

            NotificationType = entity.NotificationType,

            Channel = entity.Channel,

            ReferenceId = entity.ReferenceId,

            IsRead = entity.IsRead,

            ReadAt = entity.ReadAt,

            CreatedAt = entity.CreatedAt
        };
    }
}