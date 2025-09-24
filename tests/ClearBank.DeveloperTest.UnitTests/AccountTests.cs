using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Models.Entities;
using FluentAssertions;

namespace ClearBank.DeveloperTest.UnitTests;

public class AccountTests
{

    [Fact]
    public void AccountBalance_ShouldReduce_WhenDebit()
    {
        var account = new Account
        {
            Balance = 100m
        };

        account.Debit(10);

        account.Balance.Should().Be(90m);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs, true, null)]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps, false, PaymentFailedType.PaymentSchemeNotAllowed)]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.FasterPayments, false, PaymentFailedType.PaymentSchemeNotAllowed)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Bacs, false, PaymentFailedType.PaymentSchemeNotAllowed)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps, true, null)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments, false, PaymentFailedType.PaymentSchemeNotAllowed)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs, false, PaymentFailedType.PaymentSchemeNotAllowed)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Chaps, false, PaymentFailedType.PaymentSchemeNotAllowed)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments, true, null)]
    public void GivenPaymentScheme_WhenIsPaymentAllowedIsCalled_ShouldReturnExpectedResult(
        AllowedPaymentSchemes allowedPaymentSchemes, 
        PaymentScheme paymentScheme, 
        bool expectedResult,
        PaymentFailedType? expectedPaymentFailedType)
    {
        var account = new Account
        {
            AllowedPaymentSchemes = allowedPaymentSchemes,
            Balance = 100m,
            Status = AccountStatus.Live
        };

        var result = account.IsPaymentAllowed(paymentScheme, 10);
        result.IsPaymentAllowed.Should().Be(expectedResult);
        result.FailedType.Should().Be(expectedPaymentFailedType);
    }

    [Theory]
    [InlineData(AccountStatus.Live, true, null)]
    [InlineData(AccountStatus.Disabled, false, PaymentFailedType.AccountStatusInvalid)]
    [InlineData(AccountStatus.InboundPaymentsOnly, false, PaymentFailedType.AccountStatusInvalid)]
    public void GivenAccountStatus_WhenIsPaymentAllowedIsCalled_ShouldReturnExpectedResult(
        AccountStatus accountStatus, 
        bool expectedResult,
        PaymentFailedType? expectedPaymentFailedType)
    {
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
            Balance = 100m,
            Status = accountStatus
        };

        var result = account.IsPaymentAllowed(PaymentScheme.Bacs, 10);
        result.IsPaymentAllowed.Should().Be(expectedResult);
        result.FailedType.Should().Be(expectedPaymentFailedType);
    }

    [Theory]
    [InlineData(10, true, null)]
    [InlineData(19, true, null)]
    [InlineData(19.99, true, null)]
    [InlineData(20, true, null)]
    [InlineData(20.01, false, PaymentFailedType.InsufficientFunds)]
    [InlineData(21, false, PaymentFailedType.InsufficientFunds)]
    [InlineData(30, false, PaymentFailedType.InsufficientFunds)]
    public void GivenBalance_WhenIsPaymentAllowedIsCalled_ShouldReturnExpectedResult(
        decimal amount, 
        bool expectedResult, 
        PaymentFailedType? expectedPaymentFailedType)
    {
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
            Balance = 20m,
            Status = AccountStatus.Live
        };

        var result = account.IsPaymentAllowed(PaymentScheme.Bacs, amount);

        result.IsPaymentAllowed.Should().Be(expectedResult);
        result.FailedType.Should().Be(expectedPaymentFailedType);
    }
}