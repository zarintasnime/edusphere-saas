namespace AssignmentSubmissionManagementSystem.Application.DTOs.LateSubmissionPolicies;

public sealed class CreateLateSubmissionPolicyDto
{
    public long InstitutionId { get; set; }

    public string PolicyName { get; set; } = string.Empty;

    public int PenaltyPercentage { get; set; } = 25;

    public bool IsActive { get; set; } = true;
}