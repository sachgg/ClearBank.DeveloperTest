using ClearBank.DeveloperTest.Application.Abstractions.Persistence;
using ClearBank.DeveloperTest.Models.Entities;

namespace ClearBank.DeveloperTest.Infrastructure.Persistence.DataStore;

internal sealed class BankTransactionDataStore : IBankTransactionDataStore
{
    public void LogTransaction(BankTransaction bankTransaction)
    {
        throw new NotImplementedException();
    }
}