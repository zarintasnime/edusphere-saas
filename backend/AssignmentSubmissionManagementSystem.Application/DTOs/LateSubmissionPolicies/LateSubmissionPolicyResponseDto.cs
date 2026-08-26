namespace AssignmentSubmissionManagementSystem.Application.DTOs.LateSubmissionPolicies;

public sealed class LateSubmissionPolicyResponseDto
{
    public long PolicyId { get; set; }

    public long InstitutionId { get; set; }

    public string PolicyName { get; set; } = string.Empty;

    public int PenaltyPercentage { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}