using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

public class LateSubmissionPolicy : BaseEntity
{
    public long PolicyId { get; set; }

    public long InstitutionId { get; set; }

    public string PolicyName { get; set; } = string.Empty;

    public int PenaltyPercentage { get; set; } = 25;

    public bool IsActive { get; set; } = true;

    public DateTime? UpdatedAt { get; set; }

    public Institution Institution { get; set; } = null!;
}