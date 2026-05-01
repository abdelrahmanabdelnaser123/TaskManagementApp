using System.IO.Abstractions;
using TaskManagement.Infrastructure.DataSeed;

namespace TaskManagement.API.Configuration
{
    public static class ExtinctionConfiguration
    {
        public static void ConfigureExtinction(this IServiceCollection services, IConfiguration configuration)
        {

 


            services.AddScoped<IFileSystem, FileSystem>();

            // Register DbInitializer
            services.AddScoped<DbInitializer>();
            // Register HttpContextAccessor for AuthServices
            services.AddHttpContextAccessor();

            // Configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }
    }
}
