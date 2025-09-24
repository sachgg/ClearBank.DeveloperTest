using ClearBank.DeveloperTest.Application.Abstractions.Persistence;
using ClearBank.DeveloperTest.Application.Services;
using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Models.Entities;
using ClearBank.DeveloperTest.Models.Requests;
using FluentAssertions;
using NSubstitute;

namespace ClearBank.DeveloperTest.UnitTests;

public class PaymentServiceTests
{
    private readonly IAccountDataStore _accountDataStore;
    private readonly PaymentService _paymentService;
    private readonly IBankTransactionDataStore _bankTransactionDataStore;

    public PaymentServiceTests()
    {
        _accountDataStore = Substitute.For<IAccountDataStore>();
        _bankTransactionDataStore = Substitute.For<IBankTransactionDataStore>();
        _paymentService = new PaymentService(_accountDataStore, _bankTransactionDataStore);
    }

    [Fact]
    public void MakePayment_ReturnsFailure_WhenAccountNotFound()
    {
        var request = new MakePaymentRequest
        {
            DebtorAccountNumber = "123",
            Amount = 100,
            PaymentScheme = PaymentScheme.FasterPayments
        };

        _accountDataStore.GetAccount("123").Returns((Account)null); 

        var result = _paymentService.MakePayment(request);

        result.Success.Should().Be(false);
        result.ErrorMessage.Should().NotBeNull();
        result.ErrorMessage.Should().Be("Account not found");
    }

    [Fact]
    public void MakePayment_ReturnsFailure_WhenPaymentSchemeNotAllowed()
    {
        var account = new Account
        {
            AccountNumber = "456",
            Balance = 100,
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Status = AccountStatus.Live
        };

        var request = new MakePaymentRequest
        {
            DebtorAccountNumber = "456",
            Amount = 50,
            PaymentScheme = PaymentScheme.Bacs
        };

        _accountDataStore.GetAccount("456").Returns(account);
        var result = _paymentService.MakePayment(request);

        result.Success.Should().Be(false);
        result.ErrorMessage.Should().NotBeNull();
        result.ErrorMessage.Should().Be("Payment is not allowed for this account");

        account.Balance.Should().Be(100);
    }

    [Fact]
    public void MakePayment_DebitsAccountAndUpdates_WhenValid()
    {
        var account = new Account
        {
            AccountNumber = "789",
            Balance = 100,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
            Status = AccountStatus.Live
        };

        var request = new MakePaymentRequest
        {
            DebtorAccountNumber = "789",
            Amount = 10,
            PaymentScheme = PaymentScheme.Chaps
        };

        _accountDataStore.GetAccount("789").Returns(account);

        var result = _paymentService.MakePayment(request);

        result.Success.Should().Be(true);
        result.ErrorMessage.Should().BeNull();
        
        account.Balance.Should().Be(90);
    }
}
