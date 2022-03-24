using PaymentCore.Interfaces;

namespace PaymentWebClient.Models;

public interface ISessionService
{
    IUser User { get; set; }
    IUserSavingsAccount SavingsAccount { get; set; }

}