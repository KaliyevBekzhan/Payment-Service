using Application.Repositories;
using Application.UseCases;
using Application.UseCases.Admin;
using Application.UseCases.Interfaces;
using Domain.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationDI
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Админ
        // Работа с кор частью приложения
        services.AddScoped<IAcceptPaymentUseCase, AcceptPaymentUseCase>();
        services.AddScoped<IDeclinePaymentUseCase, DeclinePaymentUseCase>();
        services.AddScoped<IGetPaymentsForAdminUseCase, GetPaymentsForAdminUseCase>();
        services.AddScoped<IGetPaymentInfoForAdminUseCase, GetPaymentInfoForAdminUseCase>();
        
        // Работа с валютами
        services.AddScoped<IAddCurrencyUseCase, AddCurrencyUseCase>();
        services.AddScoped<IDeleteCurrencyUseCase, DeleteCurrencyUseCase>();
        services.AddScoped<IUpdateCurrencyUseCase, UpdateCurrencyUseCase>();
        services.AddScoped<IGetCurrenciesForAdminUseCase, GetCurrenciesForAdminUseCse>();
        
        // Работа с ролями
        services.AddScoped<IAddRoleUseCase, AddRoleUseCase>();
        services.AddScoped<IUpdateRoleUseCase, UpdateRoleUseCase>();
        services.AddScoped<IDeleteRoleUseCase, DeleteRoleUseCase>();
        services.AddScoped<IGetRolesUseCase, GetRolesUseCase>();
        
        // Работа с пользователями
        services.AddScoped<IGetUserDetailsUseCase, GetUserDetailsUseCase>();
        services.AddScoped<IGetUsersUseCase, GetUsersUseCase>();
        services.AddScoped<IUpdateUserRoleUseCase, UpdateUserRoleUseCase>();
        
        // Клиент
        services.AddScoped<IAddPaymentUseCase, AddPaymentUseCase>();
        services.AddScoped<IGetInfoForPaymentUseCase, GetInfoForPaymentUseCase>();
        services.AddScoped<IGetMyPaymentsUseCase, GetMyPaymentsUseCase>();
        services.AddScoped<ITopUpUseCase, TopUpUseCase>();
        services.AddScoped<IMyCabinetUseCase, MyCabinetUseCase>();
        services.AddScoped<IGetCurrenciesUseCase, GetCurrenciesUseCase>();
        
        services.AddScoped<IAddTopupUseCase, AddTopupUseCase>();
        services.AddScoped<IGetMyTopupsUseCase, GetMyTopupsUseCase>();
        services.AddScoped<IMyActionsHistoryUseCase, MyActionsHistoryUseCase>();
        
        // Регистрация
        services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        
        return services;
    }
    
}
