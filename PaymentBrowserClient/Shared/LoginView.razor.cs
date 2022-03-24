using Microsoft.AspNetCore.Components;
using PaymentCore.Emuns;
using PaymentCore.UseCases;
using PaymentWebClient.Models;

namespace PaymentWebClient.Shared;

public partial class LoginView : ComponentBase
{
    private readonly RegisterAccountForm _model = new();
    [Inject] private IUserAuthenticationInteractor Authentication { get; set; }
    [Inject] private ISessionService SessionService { get; set; }
    private PopupDialog _popup;
    [Parameter] public EventCallback<RegisterAccountForm> OnClickCallback { get; set; }

    private bool loggedin;

    private async Task LoginButtonClick()
    {
        var result = await Authentication.Authenticate(_model.Username, _model.Password);
        SessionService.User.CopyProperties(result);
        await OnClickCallback.InvokeAsync(_model);
        loggedin = result.AuthState == AuthenticationState.LoggedIn;
        StateHasChanged();
        _popup.Show($"Authentication processed with state: {result.AuthState}");
    }
    
}