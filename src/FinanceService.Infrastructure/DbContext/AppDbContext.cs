using Microsoft.EntityFrameworkCore;
using System.Text;
using FinanceService.Domain.Models;
using FinanceService.Application.Contract.IService;

namespace FinanceService.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly IUserContext _userContext;
    public AppDbContext(DbContextOptions<AppDbContext> options, IUserContext userContext) : base(options)
    {
        _userContext = userContext;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserLock> UserLocks => Set<UserLock>();
    public DbSet<AuthDetail> AuthDetails => Set<AuthDetail>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<ProviderDetail> ProviderDetails => Set<ProviderDetail>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<BankAccountDetail> BankAccountDetails => Set<BankAccountDetail>();
    public DbSet<FinancialAccount> FinancialAccounts => Set<FinancialAccount>();
    public DbSet<FinancialStatement> FinancialStatements => Set<FinancialStatement>();
    public DbSet<FinancialStatementAccountMapping> FinancialStatementAccountMappings => Set<FinancialStatementAccountMapping>();
    public DbSet<UserBankMapping> UserBankMappings => Set<UserBankMapping>();
    public DbSet<UserFinancialStatementMapping> UserFinancialStatementMappings => Set<UserFinancialStatementMapping>();
    public DbSet<TaxYear> TaxYears => Set<TaxYear>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        ApplySnakeCaseNames(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var createdAt = entity.FindProperty("CreatedAt");
            if (createdAt != null)
            {
                modelBuilder.Entity(entity.ClrType)
                    .HasIndex("CreatedAt")
                    .HasDatabaseName($"ix_{ToSnakeCase(entity.GetTableName()!)}_created_at");
            }
        }
    }

    private static void ApplySnakeCaseNames(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()!));

            foreach (var prop in entity.GetProperties())
                prop.SetColumnName(ToSnakeCase(prop.GetColumnName()!));

            foreach (var key in entity.GetKeys())
                key.SetName(ToSnakeCase(key.GetName()!));

            foreach (var fk in entity.GetForeignKeys())
                fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()!));

            foreach (var ix in entity.GetIndexes())
                ix.SetDatabaseName(ToSnakeCase(ix.GetDatabaseName()!));
        }
    }

    private static string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        var sb = new StringBuilder();
        for (int i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c)) { if (i > 0) sb.Append('_'); sb.Append(char.ToLowerInvariant(c)); }
            else sb.Append(c);
        }
        return sb.ToString();
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditFields();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditFields()
    {
        var now = DateTime.UtcNow;
        var userId = _userContext?.UserId ?? Guid.Empty;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not BaseModel model)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    if (model.CreatedAt == default)
                        model.CreatedAt = now;

                    model.UpdatedAt = now;

                    if (model.CreatedBy == default)
                        model.CreatedBy = userId;

                    model.UpdatedBy = userId;

                    model.IsActive = true;

                    // Prevent overriding CreatedAt/CreatedBy
                    entry.Property(nameof(BaseModel.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                    break;

                case EntityState.Modified:
                    // Only change update info
                    model.UpdatedAt = now;
                    model.UpdatedBy = userId;

                    // Prevent modifying CreatedAt and CreatedBy
                    entry.Property(nameof(BaseModel.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                    break;

                case EntityState.Deleted:
                    // Soft delete instead of real delete
                    entry.State = EntityState.Modified;

                    model.IsActive = false;
                    model.UpdatedAt = now;
                    model.UpdatedBy = userId;

                    entry.Property(nameof(BaseModel.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                    break;
            }
        }
    }

}
