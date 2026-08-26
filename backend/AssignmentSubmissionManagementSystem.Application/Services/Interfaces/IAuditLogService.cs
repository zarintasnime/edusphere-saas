using AssignmentSubmissionManagementSystem.Application.DTOs.AuditLogs;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IAuditLogService
{
    Task CreateAsync(
        CreateAuditLogDto dto);



    Task<AuditLogResponseDto?> GetByIdAsync(
        long auditLogId);



    Task<IReadOnlyList<AuditLogResponseDto>> GetAllAsync();



    Task<IReadOnlyList<AuditLogResponseDto>> GetByUserAsync(
        long userId);



    Task<IReadOnlyList<AuditLogResponseDto>> GetByEntityAsync(
        string entityName,
        long entityId);
}