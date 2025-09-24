using ClearBank.DeveloperTest.Application.Abstractions;
using ClearBank.DeveloperTest.Application.Abstractions.Persistence;
using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Models.Entities;
using ClearBank.DeveloperTest.Models.Requests;
using ClearBank.DeveloperTest.Models.Responses;

namespace ClearBank.DeveloperTest.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IAccountDataStore _accountDataStore;
    private readonly IBankTransactionDataStore _bankTransactionDataStore;

    public PaymentService(
        IAccountDataStore accountDataStore, 
        IBankTransactionDataStore bankTransactionDataStore)
    {
        _accountDataStore = accountDataStore;
        _bankTransactionDataStore = bankTransactionDataStore;
    }

    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {
        var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

        var result = new MakePaymentResult
        {
            Success = true
        };

        if (account == null)
        {
            result.Success = false;
            result.ErrorMessage = "Account not found";
            LogTransaction(request, request.DebtorAccountNumber, success: false, BankTransactionFailedType.AccountNotFound);
            return result;
        }

        var paymentAllowedResponse = account.IsPaymentAllowed(request.PaymentScheme, request.Amount);

        if (!paymentAllowedResponse.IsPaymentAllowed)
        {
            result.Success = false;
            result.ErrorMessage = "Payment is not allowed for this account";

            LogTransaction(request, account.AccountNumber, success: false, 
                MapPaymentFailedTypeToBankTransactionFailedType(paymentAllowedResponse.FailedType.Value));

            return result;
        }

        account.Debit(request.Amount);

        _accountDataStore.UpdateAccount(account);
        LogTransaction(request, account.AccountNumber, success: true, null);

        return result;
    }

    private void LogTransaction(
        MakePaymentRequest request, 
        string accountNumber, 
        bool success, 
        BankTransactionFailedType? failedType)
    {
        var transaction = new BankTransaction
        {
            Id = Guid.NewGuid().ToString(),
            AccountNumber = accountNumber,
            CreditorAccountNumber = request.CreditorAccountNumber,
            Amount = request.Amount,
            PaymentScheme = request.PaymentScheme,
            Created = DateTime.UtcNow,
            Success = success,
            FailedType = failedType,
        };

        _bankTransactionDataStore.LogTransaction(transaction);
    }

    private static BankTransactionFailedType? MapPaymentFailedTypeToBankTransactionFailedType(
        PaymentFailedType paymentFailedType) => 
            paymentFailedType switch
            {
                PaymentFailedType.InsufficientFunds => BankTransactionFailedType.InsufficientFunds,
                PaymentFailedType.AccountStatusInvalid => BankTransactionFailedType.AccountStatusInvalid,
                PaymentFailedType.PaymentSchemeNotAllowed => BankTransactionFailedType.PaymentSchemeNotAllowed,
                _ => BankTransactionFailedType.UnknownError,
            };
}