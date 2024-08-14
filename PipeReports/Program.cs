
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PipeReports.API.Database;

namespace PipeReports.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

        var assembly = typeof(Program).Assembly;

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        builder.Services.AddCarter();

        builder.Services.AddValidatorsFromAssembly(assembly);

        var app = builder.Build();

        ApplyMigrations(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapCarter();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.Run();
    }

    private static void ApplyMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}
