using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using FinanceService.Application.Contract.IRepository;
using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinanceService.Infrastructure.Contract.Repository;

public sealed class RepositoryBase : IRepositoryBase
{
    private readonly AppDbContext _db;
    public RepositoryBase(AppDbContext db) => _db = db;

    // ---------------- Query ----------------
    public IQueryable<T> Query<T>(bool asNoTracking = true) where T : class
        => asNoTracking ? _db.Set<T>().AsNoTracking() : _db.Set<T>();

    public async Task<T?> GetByIdAsync<T, TKey>(TKey id, CancellationToken ct) where T : class
        => await _db.Set<T>().FindAsync([id!], ct);

    public IQueryable<T> FindByCondition<T>(Expression<Func<T, bool>> predicate, bool asNoTracking = true) where T : class
        => (asNoTracking ? _db.Set<T>().AsNoTracking() : _db.Set<T>()).Where(predicate);


    public async Task<T?> FirstOrDefaultAsync<T>(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct,
        bool asNoTracking=true) where T : class
        => asNoTracking
            ? await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, ct)
            : await _db.Set<T>().FirstOrDefaultAsync(predicate, ct);

    // ---------------- CUD ----------------
    // EF Core: AddAsync returns ValueTask<EntityEntry<T>> → await it to expose Task
    public async Task AddAsync<T>(T entity, CancellationToken ct) where T : class
        => await _db.Set<T>().AddAsync(entity, ct);

    // DbSet<T>.AddRangeAsync(IEnumerable<T>, ct) returns Task — no AsTask() needed
    public Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken ct) where T : class
        => _db.Set<T>().AddRangeAsync(entities, ct);

    public void Update<T>(T entity) where T : class
        => _db.Set<T>().Update(entity);

    public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        => _db.Set<T>().UpdateRange(entities);

    public void Remove<T>(T entity) where T : class
        => _db.Set<T>().Remove(entity);

    public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        => _db.Set<T>().RemoveRange(entities);

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);

    // ---------------- Manual Transaction Control ----------------
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct)
        => _db.Database.BeginTransactionAsync(ct);

    public Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken ct)
        => transaction.CommitAsync(ct);

    public Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken ct)
        => transaction.RollbackAsync(ct);

    // ---------------- Raw SQL → typed model/DTO ----------------
    public async Task<List<TModel>> QuerySqlAsync<TModel>(
        string sql,
        IEnumerable<DbParameter>? parameters,
        CancellationToken ct)
    {
        var args = parameters is null ? Array.Empty<object>() : parameters.Cast<object>().ToArray();
        var query = _db.Database.SqlQueryRaw<TModel>(sql, args);
        return await query.ToListAsync(ct);
    }
}
