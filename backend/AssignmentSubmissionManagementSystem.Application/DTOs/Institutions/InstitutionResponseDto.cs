namespace AssignmentSubmissionManagementSystem.Application.DTOs.Institutions;

public sealed class InstitutionResponseDto
{
    public long InstitutionId { get; set; }

    public string InstitutionCode { get; set; } = string.Empty;

    public string InstitutionName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}