using ClearBank.DeveloperTest.Application.Abstractions.Persistence;
using ClearBank.DeveloperTest.Models.Entities;

namespace ClearBank.DeveloperTest.Infrastructure.Persistence.DataStore;

internal sealed class BackupAccountDataStore : IAccountDataStore
{
    public Account GetAccount(string accountNumber)
    {
        throw new NotImplementedException();
    }

    public void UpdateAccount(Account account)
    {
        throw new NotImplementedException();
    }
}