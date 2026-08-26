using AssignmentSubmissionManagementSystem.Domain.Entities.System;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface IAuditLogRepository
    : IRepository<AuditLog>
{

    Task<IReadOnlyList<AuditLog>> GetByUserAsync(
        long userId);



    Task<IReadOnlyList<AuditLog>> GetByEntityAsync(
        string entityName,
        long entityId);

    Task<IReadOnlyList<AuditLog>> GetAllWithUserAsync();

}