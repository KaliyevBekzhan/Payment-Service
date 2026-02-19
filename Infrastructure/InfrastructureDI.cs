using Application.Interfaces;
using Application.Repositories;
using Domain.Entity;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EFCore.NamingConventions;

namespace Infrastructure;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString($"DefaultConnection");

            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });
        
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IBaseRepository<Status>, StatusRepository>();
        services.AddScoped<IBaseRepository<Currency>, CurrencyRepository>();
        services.AddScoped<IBaseRepository<Role>, RoleRepository>();
        services.AddScoped<ITopupRepository, TopupRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IGuard, Guard>();
        services.AddScoped<IWalletNumberGenerator, WalletNumberGenerator>();
        
        return services;
    }
}