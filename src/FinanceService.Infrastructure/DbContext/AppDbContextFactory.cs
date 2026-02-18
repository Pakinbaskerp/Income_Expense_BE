// using System;
// using System.IO;
// using System.Linq;
// using FinanceService.Application.Contract.IService;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;

// namespace FinanceService.Infrastructure.Persistence;

// public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
// {
//     public AppDbContext CreateDbContext(string[] args)
//     {
//         var provider = args.FirstOrDefault(a => a.StartsWith("--provider=", StringComparison.OrdinalIgnoreCase))?
//                            .Split('=')[1]
//                        ?? Environment.GetEnvironmentVariable("DB_PROVIDER")
//                        ?? "Postgres";

//         var config = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json", optional: true)
//             .AddJsonFile("appsettings.Development.json", optional: true)
//             .AddEnvironmentVariables()
//             .Build();

//         var opts = new DbContextOptionsBuilder<AppDbContext>();

//         if (provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
//         {
//             var cs = config.GetConnectionString("Postgres")
//                   ?? "Host=localhost;Port=5432;Database=finance_db;Username=finance_user;Password=finance_pass";

//             opts.UseNpgsql(cs, npg =>
//             {
//                 npg.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
//                 npg.MigrationsHistoryTable("__EFMigrationsHistory", "public");
//             });
//         }
//         else if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
//         {
//             var cs = config.GetConnectionString("Sqlite") ?? "Data Source=./data/finance.sqlite";
//             opts.UseSqlite(cs, lite => lite.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
//         }
//         else
//         {
//             throw new InvalidOperationException($"Unknown provider: {provider}");
//         }

//         return new AppDbContext(opts.Options, new DesignTimeUserContext());

//     }
// }

// public sealed class DesignTimeUserContext : IUserContext
// {
//     public bool IsAuthenticated => false;
//     public Guid UserId => Guid.Empty;
// }
