using API.Data.Repositories;
using API.Interfaces;
using API.Services;

namespace API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<TokenService>();
            services.AddScoped<PaymentService>();

            return services;
        }
    }
}
