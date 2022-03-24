namespace PaymentCore.Interfaces;

public interface IUserSavingsAccount
{
    int Id { get; set; }
    double Savings { get; set; }
    double MaxSpendingPerDay { get; set; }
    double NegativeAllowed { get; set; }
    void DeepCopy(IUserSavingsAccount copyFrom);
}