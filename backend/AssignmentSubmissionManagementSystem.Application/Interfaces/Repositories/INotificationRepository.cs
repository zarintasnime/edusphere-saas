using AssignmentSubmissionManagementSystem.Domain.Entities.System;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface INotificationRepository
    : IRepository<Notification>
{

    Task<IReadOnlyList<Notification>> GetByUserAsync(
        long userId);



    Task<Notification?> GetByIdWithUserAsync(
        long notificationId);



    Task MarkAsReadAsync(
        long notificationId);
}