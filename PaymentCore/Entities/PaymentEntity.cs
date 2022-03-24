using PaymentCore.Interfaces;

namespace PaymentCore.Entities;

public class PaymentEntity : IPayment
{
    public int ToAccountId { get; set; }
    public int FromAccountId { get; set; }
    public double Amount { get; set; }
}