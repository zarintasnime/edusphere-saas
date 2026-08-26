using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;

public sealed class ChangeSubmissionStatusDto
{
    public SubmissionStatus Status { get; set; }
}