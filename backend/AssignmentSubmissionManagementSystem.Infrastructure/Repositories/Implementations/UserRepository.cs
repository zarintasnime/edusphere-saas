using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetByIdWithRoleAsync(long userId)
    {
        return await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<IReadOnlyList<User>> GetByInstitutionAsync(long institutionId)
    {
        return await _context.Users
            .Include(x => x.Role)
            .Where(x => x.InstitutionId == institutionId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<User>> GetAllWithRoleAsync()
    {
        return await _context.Users
            .Include(x => x.Role)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email);
    }

    public async Task<long> GetRoleIdAsync(RoleType role)
    {
        var roleEntity = await _context.Roles
            .FirstOrDefaultAsync(x => x.RoleName == role);

        if (roleEntity == null)
            throw new NotFoundException("Role not found");

        return roleEntity.RoleId;
    }
}