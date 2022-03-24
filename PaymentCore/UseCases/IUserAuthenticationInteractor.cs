using PaymentCore.Emuns;
using PaymentCore.Interfaces;

namespace PaymentCore.UseCases;

public interface IUserAuthenticationInteractor
{
    Task<IUser> Register(string password, string username);
    Task<bool> UnRegister(string username);
    Task<AuthenticationState> Logout(string userName);
    Task<IUser> Authenticate(string username, string password);
    Task<bool> IsNameAvailable(string username);
}
