using System.Data.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinanceService.Application.Contract.IRepository;

public interface IRepositoryBase
{
    IQueryable<T> Query<T>(bool asNoTracking = true) where T : class;

    Task<T?> GetByIdAsync<T, TKey>(TKey id, CancellationToken ct) where T : class;
    Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate,  CancellationToken ct, bool asNoTracking) where T : class;

    IQueryable<T> FindByCondition<T>(Expression<Func<T, bool>> predicate, bool asNoTracking = true) where T : class;
    // CUD
    Task AddAsync<T>(T entity, CancellationToken ct) where T : class;
    Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken ct) where T : class;
    void Update<T>(T entity) where T : class;
    void UpdateRange<T>(IEnumerable<T> entities) where T : class;
    void Remove<T>(T entity) where T : class;
    void RemoveRange<T>(IEnumerable<T> entities) where T : class;

    Task<int> SaveChangesAsync(CancellationToken ct);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken ct);
    Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken ct);

    Task<List<TModel>> QuerySqlAsync<TModel>(string sql, IEnumerable<DbParameter>? parameters, CancellationToken ct);

}