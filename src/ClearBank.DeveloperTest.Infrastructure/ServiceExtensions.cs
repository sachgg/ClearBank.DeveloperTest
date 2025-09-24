using ClearBank.DeveloperTest.Application.Abstractions.Persistence;
using ClearBank.DeveloperTest.Infrastructure.Persistence.DataStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.Infrastructure;

public static class ServiceExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataStore(configuration);
    }

    private static void AddDataStore(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["DataStore"] == "Backup")
        {
            services.AddScoped<IAccountDataStore, BackupAccountDataStore>();
        }
        else
        {
            services.AddScoped<IAccountDataStore, AccountDataStore>();
        }

        services.AddScoped<IBankTransactionDataStore, BankTransactionDataStore>();
    }
}
