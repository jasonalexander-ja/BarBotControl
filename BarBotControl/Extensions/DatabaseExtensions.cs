using BarBotControl.Data;
using Microsoft.EntityFrameworkCore;

namespace BarBotControl.Extensions;

public static class DatabaseExtensions
{
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        var logToConsole = builder.Configuration.GetSection("EnableConsoleDBLogging")
            .Get<bool>();

        var serverVersion = new MySqlServerVersion(new Version(11, 2, 2));

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            var dbBuilder = options.UseMySql(connectionString, serverVersion);
            if (logToConsole)
            {
                dbBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });
        return builder;
    }
}
