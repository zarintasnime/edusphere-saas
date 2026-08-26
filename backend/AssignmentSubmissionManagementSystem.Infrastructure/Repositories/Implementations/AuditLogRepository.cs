using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.System;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class AuditLogRepository
    : Repository<AuditLog>, IAuditLogRepository
{
    private readonly ApplicationDbContext _context;


    public AuditLogRepository(
        ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AuditLog>> GetByUserAsync(
        long userId)
    {
        return await _context.AuditLogs

            .Include(x => x.User)

            .Where(x =>
                x.UserId == userId)

            .OrderByDescending(x =>
                x.CreatedAt)

            .AsNoTracking()

            .ToListAsync();
    }
    public async Task<IReadOnlyList<AuditLog>> GetByEntityAsync(
        string entityName,
        long entityId)
    {
        return await _context.AuditLogs

            .Include(x => x.User)

            .Where(x =>
                x.EntityName == entityName &&
                x.EntityId == entityId)

            .OrderByDescending(x =>
                x.CreatedAt)

            .AsNoTracking()

            .ToListAsync();
    }

    public async Task<IReadOnlyList<AuditLog>> GetAllWithUserAsync()
    {
        return await _context.AuditLogs
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }
}