using FinanceService.API.Extensions;
using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi()
                .AddSwaggerGen()
                .AddControllers();
builder.Services.InvokeCorsException(builder.Configuration);
builder.Services.InvokeAppDbContext(builder.Configuration);
builder.Services.AddSwaggerWithAuth();
builder.Services.AddEndpointsApiExplorer();
builder.Services.InvokeServiceRegistryExtension(builder.Configuration);
builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
});


WebApplication? app = builder.Build();


if (app.Environment.IsDevelopment())
{

    app.ApplyPendingMigrations<AppDbContext>();
}

app.UseCors("AllowAngularDev");

app.InvokeSwaggerExtension();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
