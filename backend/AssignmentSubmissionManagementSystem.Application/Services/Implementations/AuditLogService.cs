using System.Text.Json;
using AssignmentSubmissionManagementSystem.Application.DTOs.AuditLogs;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.System;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditLogService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task CreateAsync(CreateAuditLogDto dto)
    {
        var log = new AuditLog
        {
            InstitutionId = dto.InstitutionId,
            UserId = dto.UserId,
            Action = dto.Action,
            EntityName = dto.EntityName,
            EntityId = dto.EntityId,
            OldValues = EnsureValidJson(dto.OldValues),
            NewValues = EnsureValidJson(dto.NewValues),
            CreatedAt = DateTime.Now
        };

        await _auditLogRepository.AddAsync(log);
    }

    public async Task<AuditLogResponseDto?> GetByIdAsync(long auditLogId)
    {
        var log = await _auditLogRepository.GetByIdAsync(auditLogId);
        if (log == null)
            return null;

        return MapToResponse(log);
    }

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetAllAsync()
    {
        var logs = await _auditLogRepository.GetAllWithUserAsync();
        return logs.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetByUserAsync(long userId)
    {
        var logs = await _auditLogRepository.GetByUserAsync(userId);
        return logs.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetByEntityAsync(string entityName, long entityId)
    {
        var logs = await _auditLogRepository.GetByEntityAsync(entityName, entityId);
        return logs.Select(MapToResponse).ToList();
    }

    private static string? EnsureValidJson(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var trimmed = value.Trim();
        if ((trimmed.StartsWith("{") && trimmed.EndsWith("}")) || (trimmed.StartsWith("[") && trimmed.EndsWith("]")))
        {
            return trimmed;
        }

        return JsonSerializer.Serialize(new { message = value });
    }

    private static AuditLogResponseDto MapToResponse(AuditLog log)
    {
        string displayNewValues = log.NewValues ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(log.NewValues))
        {
            try
            {
                using var doc = JsonDocument.Parse(log.NewValues);
                if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("message", out var msgProp))
                {
                    displayNewValues = msgProp.GetString() ?? log.NewValues;
                }
            }
            catch
            {
                displayNewValues = log.NewValues;
            }
        }

        return new AuditLogResponseDto
        {
            AuditLogId = log.AuditLogId,
            InstitutionId = log.InstitutionId,
            UserId = log.UserId,
            UserName = log.User?.FullName ?? string.Empty,
            Action = log.Action,
            EntityName = log.EntityName,
            EntityId = log.EntityId,
            OldValues = log.OldValues,
            NewValues = displayNewValues,
            CreatedAt = log.CreatedAt
        };
    }
}