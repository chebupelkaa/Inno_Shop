using Microsoft.EntityFrameworkCore;

namespace UserService.API.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task MigrateDatabaseAsync<TContext>(this WebApplication app, int retries = 10, int delaySeconds = 5)
            where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

            for (var attempt = 1; attempt <= retries; attempt++)
            {
                try
                {
                    if (db.Database.IsRelational())
                    {
                        await db.Database.MigrateAsync();
                    }
                    else
                    {
                        await db.Database.EnsureCreatedAsync();
                    }

                    logger.LogInformation("Database migrations applied successfully");
                    return;
                }
                catch (Exception ex) when (attempt < retries)
                {
                    logger.LogWarning(ex, "Database migration attempt {Attempt}/{Retries} failed. Retrying in {Delay}s...", attempt, retries, delaySeconds);
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }
        }
    }
}
