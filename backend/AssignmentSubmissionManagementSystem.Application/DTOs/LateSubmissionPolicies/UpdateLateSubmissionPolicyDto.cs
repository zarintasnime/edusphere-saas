namespace AssignmentSubmissionManagementSystem.Application.DTOs.LateSubmissionPolicies;

public sealed class UpdateLateSubmissionPolicyDto
{
    public string PolicyName { get; set; } = string.Empty;

    public int PenaltyPercentage { get; set; }

    public bool IsActive { get; set; }
}