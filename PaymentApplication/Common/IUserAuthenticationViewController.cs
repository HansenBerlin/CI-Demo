namespace PaymentApplication.Common;

public interface IUserAuthenticationViewController
{
    Task<string> ValidateUsernameInput();
    Task<string> ValidateUserRegistrationInput(string username);
    Task<string> ValidateLoginInput();
}