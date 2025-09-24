using ClearBank.DeveloperTest.Models.Entities;

namespace ClearBank.DeveloperTest.Application.Abstractions.Persistence;

public interface IBankTransactionDataStore
{
    void LogTransaction(BankTransaction bankTransaction);
}
