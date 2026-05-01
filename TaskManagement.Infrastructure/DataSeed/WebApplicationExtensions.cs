using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;
namespace TaskManagement.Infrastructure.DataSeed
{
    public static class WebApplicationExtensions
    {
        public static void InitializeDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();

            dbInitializer.Initialize();
            dbInitializer.SeedData();
        }
    }
}