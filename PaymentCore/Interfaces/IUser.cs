using PaymentCore.Emuns;

namespace PaymentCore.Interfaces;

public interface IUser
{
    int Id { get; set; }
    string Name { get; set; }
    IUserSavingsAccount UserSavingsAccount { get; set; }
    AuthenticationState AuthState { get; set; }
    string PasswordHash { get; set; }
    void CopyProperties(IUser copyFrom);
}