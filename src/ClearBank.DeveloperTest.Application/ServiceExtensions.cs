using ClearBank.DeveloperTest.Application.Abstractions;
using ClearBank.DeveloperTest.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.Application;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, PaymentService>();
    }   
}