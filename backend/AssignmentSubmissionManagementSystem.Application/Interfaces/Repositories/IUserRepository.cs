using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(
        string email);


    Task<User?> GetByIdWithRoleAsync(
        long userId);


    Task<IReadOnlyList<User>> GetByInstitutionAsync(
        long institutionId);


    Task<bool> EmailExistsAsync(
        string email);


    Task<long> GetRoleIdAsync(
        RoleType role);

    Task<IReadOnlyList<User>> GetAllWithRoleAsync();
}