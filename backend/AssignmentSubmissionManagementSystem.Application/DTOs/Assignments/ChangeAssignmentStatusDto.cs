using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Assignments;

public sealed class ChangeAssignmentStatusDto
{
    public AssignmentStatus Status { get; set; }
}