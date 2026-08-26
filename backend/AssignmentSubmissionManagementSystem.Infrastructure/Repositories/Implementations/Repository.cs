using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;

public class Repository<T> : IRepository<T>
where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;


    public Repository(
        ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }



    public async Task<T?> GetByIdAsync(
        long id)
    {
        return await _dbSet.FindAsync(id);
    }




    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }




    public async Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
    }




    public async Task AddAsync(
        T entity)
    {
        await _dbSet.AddAsync(entity);

        await _context.SaveChangesAsync();
    }




    public void Update(
        T entity)
    {
        _dbSet.Update(entity);
    }




    public void Remove(
        T entity)
    {
        _dbSet.Remove(entity);
    }




    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .AnyAsync(predicate);
    }




    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}