using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinanceService.API.Extensions;

public static class MigrationBuilderExtensions
{
    /// <summary>
    /// Applies any pending migrations for the given DbContext type (if any exist).
    /// </summary>
    public static void ApplyPendingMigrations<TContext>(this IApplicationBuilder app)
        where TContext : DbContext
    {
        using IServiceScope? scope = app.ApplicationServices.CreateScope();

        try
        {
            TContext? db = scope.ServiceProvider.GetRequiredService<TContext>();

            List<string>? pending = db.Database.GetPendingMigrations().ToList();
            if (pending.Any())
            {
                db.Database.Migrate();
            }

        }
        catch (Exception ex)
        {
            throw new Exception($"Exception: {ex.Message}");
        }
    }
}
