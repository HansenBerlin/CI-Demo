namespace PaymentCore.Interfaces;

public interface IPayment
{
    int ToAccountId { get; set; }
    int FromAccountId { get; set; }
    double Amount { get; set; }
}