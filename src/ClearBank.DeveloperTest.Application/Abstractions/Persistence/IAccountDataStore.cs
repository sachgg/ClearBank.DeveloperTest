using ClearBank.DeveloperTest.Models.Entities;

namespace ClearBank.DeveloperTest.Application.Abstractions.Persistence;

public interface IAccountDataStore
{
    Account GetAccount(string accountNumber);
    void UpdateAccount(Account account);
}
