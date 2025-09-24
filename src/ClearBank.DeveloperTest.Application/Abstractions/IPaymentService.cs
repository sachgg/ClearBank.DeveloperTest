using ClearBank.DeveloperTest.Models.Requests;
using ClearBank.DeveloperTest.Models.Responses;

namespace ClearBank.DeveloperTest.Application.Abstractions;

public interface IPaymentService
{
    MakePaymentResult MakePayment(MakePaymentRequest request);
}