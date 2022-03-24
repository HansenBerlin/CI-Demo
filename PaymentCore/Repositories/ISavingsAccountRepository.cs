using PaymentCore.Interfaces;

namespace PaymentCore.Repositories;

public interface ISavingsAccountRepository
{
    Task<int> AddFunds(double amount, int toId);
    Task<int> SubstractFunds(double amount, int fromId);
    Task<int> ChangeDailyLimit(int amount);
    Task<int> ChangeMaxNegativeLimit(int amount);
    Task<IUserSavingsAccount> AddNewAccount(int initialAmount, string userName);
    Task<bool> IsAccountAvailable(int iD);
    Task<IUserSavingsAccount> DeleteAccount(int accountId);
    Task<IUserSavingsAccount> GetUserSavingsAccount(string userName);
}