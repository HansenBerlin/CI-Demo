using Microsoft.AspNetCore.Components;
using MudBlazor;
using PaymentCore.Emuns;
using PaymentCore.Entities;
using PaymentCore.Interfaces;
using PaymentCore.UseCases;
using PaymentWebClient.Models;

namespace PaymentWebClient.Shared;

public partial class MainView : ComponentBase
{
    [Inject] 
    private IUserAuthenticationInteractor Authentication { get; set; }
    [Inject] 
    private ISessionService SessionService { get; set; }
    private MudTabs _tabs;
    private bool isAuthenticated;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //_user = SessionService.User;
            UpdateTabVisibilityState();
            StateHasChanged();
        }
    }

    private void UpdateTabVisibilityState()
    {
        if (SessionService.User.AuthState == AuthenticationState.LoggedIn)
        {
            isAuthenticated = true;
            _tabs.ActivatePanel(2);
        }
        else
        {
            isAuthenticated = false;
            _tabs.ActivatePanel(0);
        }
    }

    private void LoginButtonClick(RegisterAccountForm model)
    {
        UpdateTabVisibilityState();
        StateHasChanged();
    }
    
    private async Task RegisterButtonClick(RegisterAccountForm model)
    {
        var result = await Authentication.Register(model.Password, model.Username);
        SessionService.User.CopyProperties(result);
        UpdateTabVisibilityState();
        StateHasChanged();
    }

    private void Logout()
    {
        SessionService.User.AuthState = AuthenticationState.LoggedOut;
        UpdateTabVisibilityState();
        StateHasChanged();
    }
}