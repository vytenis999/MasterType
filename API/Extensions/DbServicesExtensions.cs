using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class DbServicesExtensions
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration,  IWebHostEnvironment environment)
        {
            string connString;
            if (environment.IsDevelopment())
                connString = configuration.GetConnectionString("DefaultConnection");
            else
            {
                // Use connection string provided at runtime by FlyIO.
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl.Replace("postgres://", string.Empty);
                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];
                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];
                var updatedHost = pgHost.Replace("flycast", "internal");

                connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
            }
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseNpgsql(connString);
            });

            return services;
        }
    }
}
