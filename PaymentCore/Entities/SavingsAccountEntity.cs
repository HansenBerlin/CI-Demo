using PaymentCore.Interfaces;

namespace PaymentCore.Entities;

public class SavingsAccountEntity : IUserSavingsAccount
{
    public int Id { get; set; }
    public double Savings { get; set; }
    public double MaxSpendingPerDay { get; set; }
    public double NegativeAllowed { get; set; }
    public void DeepCopy(IUserSavingsAccount copyFrom)
    {
        Id = copyFrom.Id;
        Savings = copyFrom.Savings;
        NegativeAllowed = copyFrom.NegativeAllowed;
        MaxSpendingPerDay = copyFrom.MaxSpendingPerDay;
    }
}