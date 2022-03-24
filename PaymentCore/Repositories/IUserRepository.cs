namespace PaymentCore.Repositories;

public interface IUserRepository
{
    Task<string> GetById(int iD);
    Task<int> UpdatePasswordHash(string name, string pwHash);
    Task<int> AddNewUser(string userName);
    Task<bool> IsPasswordHashMatching(string? name, string pwHash);
    Task<bool> IsNameExisting(string? name);
    Task<int> SetLoginState(string? name, bool isLoggedIn);
}