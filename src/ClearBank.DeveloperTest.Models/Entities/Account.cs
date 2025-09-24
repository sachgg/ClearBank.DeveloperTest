using ClearBank.DeveloperTest.Models.Responses;

namespace ClearBank.DeveloperTest.Models.Entities;

public class Account
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }

    public void Debit(decimal amount)
    {
        Balance -= amount;
    }

    public PaymentAllowedResponse IsPaymentAllowed(PaymentScheme paymentScheme, decimal paymentAmount)
    {
        if (paymentAmount > Balance)
            return new(IsPaymentAllowed: false, PaymentFailedType.InsufficientFunds);

        if (Status != AccountStatus.Live)
            return new(IsPaymentAllowed: false, PaymentFailedType.AccountStatusInvalid);

        switch (paymentScheme)
        {
            case PaymentScheme.Bacs:
                if (AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    return new(IsPaymentAllowed: true);
                break;
            case PaymentScheme.Chaps:
                if (AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    return new(IsPaymentAllowed: true);
                break;
            case PaymentScheme.FasterPayments:
                if (AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    return new(IsPaymentAllowed: true);
                break;
            default:
                return new(IsPaymentAllowed: false, PaymentFailedType.PaymentSchemeNotAllowed);
        }

        return new(IsPaymentAllowed: false, PaymentFailedType.PaymentSchemeNotAllowed);
    }
}