using System.Diagnostics;
using FinanceService.Domain.Dto;
using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.API.Extensions;


public static class DbContextExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection InvokeAppDbContext(this IServiceCollection service, IConfiguration configuration)
    {
         service.Configure<PasswordHashingOptions>(configuration.GetSection("PasswordHashing"));
        string provider = configuration.GetConnectionString("Provider") ?? "";

        string sqliteConn = configuration.GetConnectionString("Sqlite") ?? "";
        string postgresConn = configuration.GetConnectionString("Postgres") ?? "";
        string sqlServerConn = configuration.GetConnectionString("SqlServer") ?? "";

        service.AddDbContext<AppDbContext>(options =>
            {
                switch (provider.ToLower())
                {
                    case "sqlite":
                        if (string.IsNullOrEmpty(sqliteConn))
                            throw new Exception("Connection string not found");
                        options.UseSqlite(sqliteConn);
                        break;
                    case "postgres":
                    case "postgresql":
                        if (String.IsNullOrEmpty(postgresConn))
                            throw new Exception("Connection string not found");

                        options.UseNpgsql(postgresConn);
                        break;
                    default:
                        throw new Exception("Db connection provider not found");
                }
            }
        );

        return service;
    }
}