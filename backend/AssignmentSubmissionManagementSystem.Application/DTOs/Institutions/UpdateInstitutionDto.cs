namespace AssignmentSubmissionManagementSystem.Application.DTOs.Institutions;

public sealed class UpdateInstitutionDto
{
    public string InstitutionCode { get; set; } = string.Empty;

    public string InstitutionName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; }
}