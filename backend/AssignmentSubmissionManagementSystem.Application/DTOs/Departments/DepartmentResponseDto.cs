namespace AssignmentSubmissionManagementSystem.Application.DTOs.Departments;

public sealed class DepartmentResponseDto
{
    public long DepartmentId { get; set; }

    public long InstitutionId { get; set; }

    public string InstitutionName { get; set; } = string.Empty;

    public string DepartmentCode { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}