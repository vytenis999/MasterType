using API.Data.Repositories;
using API.Interfaces;
using API.Services;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPaymentsRepository, PaymentsRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ILovedRepository, LovedRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            //Stripe services
            services.AddScoped<TokenService>();
            services.AddScoped<PaymentService>();

            return services;
        }
    }
}
