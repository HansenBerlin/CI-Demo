using System.Threading.Tasks;
using PaymentApplication.Common;
using PaymentConsoleClient.Interfaces;
using PaymentCore.Emuns;
using PaymentCore.Entities;
using PaymentCore.Interfaces;
using PaymentCore.UseCases;
using static System.Console;

namespace PaymentConsoleClient.Controller;

public class SavingsAccountViewController : ISavingsAccountViewController
{
    private readonly ISavingsAccountInteractor _interactor;
    private readonly IUserSavingsAccount _account;
    private readonly ISelectionValidation _validator;
    private readonly IUser _user;
    public SavingsAccountViewController(ISavingsAccountInteractor interactor, IUser user, ISelectionValidation validator)
    {
        _interactor = interactor;
        _user = user;
        _account = user.UserSavingsAccount;
        _validator = validator;
    }

    public async Task AddAccount(string username)
    {
        var acc = await _interactor.AddAccount(username);
        _account.DeepCopy(acc);
    }

    public async Task<string> SendPayment()
    {
        WriteLine("Type in amount to send without decimals");
        double amount = _validator.RestrictInputToPositiveInt();
        WriteLine("Type in receiver account id");
        int receiverId = _validator.RestrictInputToPositiveInt();
        bool isReceiverKnownUser = await _interactor.IsAccountAvailable(receiverId);
        if (isReceiverKnownUser)
        {
            IPayment payment = new PaymentEntity()
            {
                Amount = amount,
                FromAccountId = _account.Id,
                ToAccountId = receiverId
            };

            PaymentState paymentState = await _interactor.MakePayment(payment, _account);
            return $"Payment done with state: {paymentState}";
        }

        return "USER UNKNOWN";
    }

    public async Task<string> MakeDeposit()
    {
        WriteLine("Type in amount to deposit without decimals");
        double amount = _validator.RestrictInputToPositiveInt();
        IPayment payment = new PaymentEntity()
        {
            Amount = amount,
            ToAccountId = _account.Id
        };
        PaymentState paymentState = await _interactor.Deposit(payment);
        return $"Deposit done with state: {paymentState}";
    }

    public async Task<string> ShowAccountData()
    {
        var account = await _interactor.GetUserAccount(_user.Name);
        _account.DeepCopy(account);
        return   ($"---------------------------------------\n" +
                  $"Account Id:            {_account.Id}\n" +
                  $"Available Funds:       {_account.Savings}\n" +
                  $"Max. negative allowed: {_account.NegativeAllowed}\n" +
                  $"Max. spending per day: {_account.MaxSpendingPerDay}\n" +
                  $"---------------------------------------");
    }
}