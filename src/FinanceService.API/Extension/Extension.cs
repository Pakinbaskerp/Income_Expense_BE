using System.Diagnostics;
using System.Reflection;
using FinanceService.Application.Contract.IRepository;
using FinanceService.Application.Contract.IService;
using FinanceService.Application.Contract.Services;
using FinanceService.Application.Extension;
using FinanceService.Application.Services;
using FinanceService.Domain.Dto;
using FinanceService.Infrastructure.Contract.Repository;
using FinanceService.Infrastructure.Contract.Service;
using FluentValidation;
using libraries.logging.Contract;
using libraries.logging.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FinanceService.API.Extensions;

public static class Extension
{
    // ---------------- Swagger (middleware) ----------------
    public static void InvokeSwaggerExtension(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Finance Service API v1");
            options.DocumentTitle = "Finance Service API Docs";
            options.DocExpansion(DocExpansion.None);
            options.EnableFilter();
        });
    }

    // ---------------- Swagger (services with JWT) ----------------
    public static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Finance Service API",
                Version = "v1",
                Description = "API documentation with JWT Auth"
            });

            var jwt = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                Description = "Enter: **Bearer {your JWT token}**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            };

            options.AddSecurityDefinition("Bearer", jwt);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwt, Array.Empty<string>() } });
        });

        return services;
    }

    // ---------------- CORS (named policy) ----------------
    public static IServiceCollection InvokeCorsException(this IServiceCollection services, IConfiguration configuration)
    {
        string[] cors = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularDev", policy =>
            {
                policy.WithOrigins("http://localhost:4200")  // your Angular dev URL
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // only if you actually use cookies
            });
        });
        return services;
    }

    // ---------------- CQRS Registry (API-only) ----------------
    public static IServiceCollection InvokeServiceRegistryExtension(this IServiceCollection services, IConfiguration _)
    {
        RegisterAssemblyReference(services);
        RegisterServices(services);
        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped(typeof(ILoggerManager<>), typeof(LoggerManager<>));
        services.AddScoped<IUserContext, UserContext>();

        _ = services.AddScoped<IRepositoryBase, RepositoryBase>();
        _ = services.AddScoped<IAuthService, AuthService>();
        _ = services.AddScoped<IAccountService, AccountService>();
        _ = services.AddScoped<IFinanceStatementService, FinanceStatementService>();
        _ = services.AddScoped<ITaxYearService, TaxYearService>();
    }

    private static void RegisterAssemblyReference(IServiceCollection services)
    {
       services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),               
            typeof(AssemblyMarker).Assembly                
        ));

        services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);
        // MediatR pipeline behaviors (order: top runs first)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
    }

}

// ---------------- Behaviors ----------------
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (_validators.Any())
        {
            var ctx = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(ctx, ct)));
            var failures = results.SelectMany(r => r.Errors).Where(e => e is not null).ToList();
            if (failures.Count != 0) throw new ValidationException(failures);
        }
        return await next();
    }
}

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>  where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        _logger.LogInformation("Handling {Request}", typeof(TRequest).Name);
        var response = await next();
        _logger.LogInformation("Handled {Request}", typeof(TRequest).Name);
        return response;
    }
}

public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>  where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        if (sw.ElapsedMilliseconds > 200)
            _logger.LogWarning("Slow request {Request} took {Elapsed} ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);

        return response;
    }
}
