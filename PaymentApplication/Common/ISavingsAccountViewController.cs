namespace PaymentApplication.Common;

public interface ISavingsAccountViewController
{
    Task<string> SendPayment();
    Task<string> MakeDeposit();
    Task <string> ShowAccountData();
}