using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.System;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class NotificationRepository
    : Repository<Notification>, INotificationRepository
{
    private readonly ApplicationDbContext _context;


    public NotificationRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }






    public async Task<IReadOnlyList<Notification>> GetByUserAsync(
        long userId)
    {
        return await _context.Notifications

            .Where(x =>
                x.UserId == userId)

            .OrderByDescending(x =>
                x.CreatedAt)

            .AsNoTracking()

            .ToListAsync();
    }








    public async Task<Notification?> GetByIdWithUserAsync(
        long notificationId)
    {
        return await _context.Notifications

            .Include(x => x.User)

            .FirstOrDefaultAsync(x =>
                x.NotificationId == notificationId);
    }








    public async Task MarkAsReadAsync(
        long notificationId)
    {
        var notification =
            await _context.Notifications
                .FirstOrDefaultAsync(x =>
                    x.NotificationId == notificationId);



        if (notification == null)
            throw new NotFoundException("Notification not found");



        notification.IsRead = true;

        notification.ReadAt =
            DateTime.Now;


        await _context.SaveChangesAsync();
    }
}