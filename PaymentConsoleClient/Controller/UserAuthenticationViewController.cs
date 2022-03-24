using System;
using System.Threading.Tasks;
using PaymentApplication.Common;
using PaymentCore.Emuns;
using PaymentCore.Interfaces;
using PaymentCore.UseCases;

namespace PaymentConsoleClient.Controller;

public class UserAuthenticationViewController : IUserAuthenticationViewController
{
    private readonly IUserAuthenticationInteractor _interactor;
    private readonly IUser _user;
    
    public UserAuthenticationViewController(IUserAuthenticationInteractor interactor, IUser user)
    {
        _interactor = interactor;
        _user = user;
    }
    
    public async Task<string> ValidateUsernameInput()
    {
        Console.WriteLine("\nNEW USER REGISTRATION\ntype x to cancel\nUsername: ");
        while (true)
        {
            string? username = Console.ReadLine();
            if (username == "x")
            {
                return string.Empty;
            }
            var userNameAvailable = await _interactor.IsNameAvailable(username);
            if (userNameAvailable)
            {
                return username;
            }
            Console.WriteLine("Username already in use. Please choose another one.");
        }
    }

    public async Task<string> ValidateLoginInput()
    {
        Console.WriteLine("\nLOGIN\ntype x to cancel \n");
        while (true)
        {
            Console.WriteLine("Username:");
            string? username = Console.ReadLine();
            Console.WriteLine("Password:");
            string? pw = Console.ReadLine();
            if (username == "x" || pw == "x")
            {
                return "LOGIN FAILED";
            }

            IUser user = await _interactor.Authenticate(username, pw);
            _user.CopyProperties(user);
            return "LOGIN SUCCESSFUL";
        }
    }

    public async Task<string> ValidateUserRegistrationInput(string username)
    {
        while (true)
        {
            Console.WriteLine("\nChoose secure password. \ntype x to cancel \nPassword:");
            string pw = Console.ReadLine() ?? string.Empty;
            if (pw == "x")
            {
                return "REGISTRATION CANCELLED BY USER";
            }
            
            var user = await _interactor.Register(pw, username);
            Console.WriteLine(user.AuthState);

            switch (user.AuthState)
            {
                case AuthenticationState.InsecurePassword:
                    return "PASSWORD NOT SECURE OR CONTAINING FORBIDDEN CHARACTERS";
                case AuthenticationState.LoggedIn:
                    _user.CopyProperties(user);
                    return "REGISTRATION SUCCESSFUL";
                default:
                    return $"REGISTRATION FAILED WITH STATUS: {_user.AuthState}";
            }
        }
    }
}