namespace ClearBank.DeveloperTest.Models.Responses;

public record PaymentAllowedResponse(
    bool IsPaymentAllowed, 
    PaymentFailedType? FailedType = null)
{
}