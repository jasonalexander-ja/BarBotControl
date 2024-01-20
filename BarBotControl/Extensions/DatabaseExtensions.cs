using BarBotControl.Data;
using Microsoft.EntityFrameworkCore;

namespace BarBotControl.Extensions;

public static class DatabaseExtensions
{
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var serverVersion = new MySqlServerVersion(new Version(11, 2, 2));

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, serverVersion)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );
        return builder;
    }
}
