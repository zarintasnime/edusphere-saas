using System.Linq.Expressions;

namespace AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;

public interface IRepository<T>
where T : class
{
    Task<T?> GetByIdAsync(long id);

    Task<IReadOnlyList<T>> GetAllAsync();


    Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate);


    Task AddAsync(T entity);


    void Update(T entity);


    void Remove(T entity);


    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate);


    Task SaveChangesAsync();
}