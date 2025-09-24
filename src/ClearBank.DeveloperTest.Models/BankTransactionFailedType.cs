namespace ClearBank.DeveloperTest.Models;

public enum BankTransactionFailedType
{
    AccountNotFound,
    InsufficientFunds,
    AccountStatusInvalid,
    PaymentSchemeNotAllowed,
    UnknownError
}