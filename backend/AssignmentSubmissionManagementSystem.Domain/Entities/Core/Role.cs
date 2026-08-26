using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Core;

public class Role : BaseEntity
{
    public long RoleId { get; set; }

    public RoleType RoleName { get; set; }
}