using System.Net;
using PaymentApplication.Common;
using PaymentApplication.ValueObjects;
using PaymentCore.Emuns;
using PaymentCore.Entities;
using PaymentCore.Interfaces;
using PaymentCore.Policies;
using PaymentCore.UseCases;

namespace PaymentApplication.Controller;

public class UserAuthenticationController : IUserAuthenticationInteractor
{
    private readonly ISecurityPolicyInteractor _pwCheck;
    private readonly IHttpRequestController _requestController;

    public UserAuthenticationController(ISecurityPolicyInteractor pwCheck, IHttpRequestController requestController)
    {
        _pwCheck = pwCheck;
        _requestController = requestController;
    }
    
    public async Task<bool> IsNameAvailable(string username)
    {
        string url = $"{ApiStrings.NameCheck}/{username.ToLower()}";
        HttpResponseMessage response = await _requestController.GetAsync(url);
        bool isNameInUse = false;
        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            isNameInUse = bool.Parse(result);
        }
        return !isNameInUse;
    }

    public async Task<IUser> Authenticate(string username, string password)
    {
        string url = $"{ApiStrings.UserAuthenticate}/{username.ToLower()}/{password}";
        HttpResponseMessage response = await _requestController.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<UserEntity>();
        }

        var state = await response.Content.ReadAsAsync<AuthenticationState>();
        return new UserEntity { AuthState = state };
    }

    public async Task<IUser> Register(string password, string username)
    {
        IUser user = new UserEntity { AuthState = AuthenticationState.Unregistered, Name = username.ToLower() };
        bool isPasswordSecure = _pwCheck.IsPasswordCompliantToSecurityRules(password, new PasswordSecurityRules());
        if (isPasswordSecure)
        {
            string uri = $"{ApiStrings.UserRegister}/{user.Name}/{password}";
            HttpResponseMessage response = await _requestController.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<UserEntity>();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                user.AuthState = await response.Content.ReadAsAsync<AuthenticationState>();
            }
            
            return user;
        }

        user.AuthState = AuthenticationState.InsecurePassword;
        return user;
    }

    public Task<bool> UnRegister(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthenticationState> Logout(string userName)
    {
        string url = $"{ApiStrings.UserLogout}/{userName}";
        HttpResponseMessage response = await _requestController.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<AuthenticationState>();
        }
        return AuthenticationState.LoggedIn;
    }
}