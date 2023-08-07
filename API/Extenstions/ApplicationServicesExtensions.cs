using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extenstions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<StoreContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
            return ConnectionMultiplexer.Connect(options);
        });
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionsContext =>
            {
                var errors = actionsContext.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();

                var errorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
            });
        });

        return services;
    }
}
