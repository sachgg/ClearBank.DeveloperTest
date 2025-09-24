namespace ClearBank.DeveloperTest.Models.Entities;

public sealed class BankTransaction
{
    public string Id { get; set; }
    public string AccountNumber { get; set; }
    public string CreditorAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public PaymentScheme PaymentScheme { get; set; }
    public bool Success { get; set; }
    public BankTransactionFailedType? FailedType { get; set; }
    public DateTimeOffset Created { get; set; }
}
