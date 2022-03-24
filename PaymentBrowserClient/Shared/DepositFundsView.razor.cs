using Microsoft.AspNetCore.Components;
using PaymentCore.Emuns;
using PaymentCore.Entities;
using PaymentCore.Interfaces;
using PaymentCore.UseCases;
using PaymentWebClient.Models;

namespace PaymentWebClient.Shared;

public partial class DepositFundsView : ComponentBase
{

    [Inject] private ISessionService SessionService { get; set; }
    [Inject] private ISavingsAccountInteractor SavingsAccountInteractor { get; set; }
    private readonly IPayment _paymentModel = new PaymentEntity();
    private PopupDialog _dialog;
    private SavingsAccountView _savingsAccountView;

    public DepositFundsView(ISessionService sessionService, ISavingsAccountInteractor savingsAccountInteractor)
    {
        SessionService = sessionService;
        SavingsAccountInteractor = savingsAccountInteractor;
    }
    public DepositFundsView() { }
    
    

    private async Task DepositFundsClick()
    {
        _paymentModel.ToAccountId = SessionService.User.UserSavingsAccount.Id;
        PaymentState state = await SavingsAccountInteractor.Deposit(_paymentModel);
        await _savingsAccountView.UpdateData();
        _dialog.Show($"Deposit processed with state: {state.ToString()}");
    }
    
}