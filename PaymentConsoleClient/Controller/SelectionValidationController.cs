using System;
using System.Collections.Generic;
using PaymentConsoleClient.Enums;
using PaymentConsoleClient.Interfaces;
using PaymentConsoleClient.ValueObjects;
using PaymentCore.Emuns;
using PaymentCore.Interfaces;

namespace PaymentConsoleClient.Controller;

public class SelectionValidationController : ISelectionValidation
{
    private readonly IUser _user;
    
    public SelectionValidationController(IUser user)
    {
        _user = user;
    }

    public Dictionary<object, string> LimitMainMenuOptions()
    {
        var options = new MenuOptions();
        var allValues = options.MainMenu;

        if (_user.AuthState != AuthenticationState.LoggedIn)
        {
            allValues.Remove(MainMenuOptions.SavingsAccountOptions);
        }
        return allValues;
    }

    public Dictionary<object, string> LimitUserAccountMenuOptions()
    {
        var options = new MenuOptions();
        var allValues = options.UserAccountMenu;

        if (_user.AuthState == AuthenticationState.LoggedIn)
        {
            allValues.Remove(UserAccountOptions.Login);
            allValues.Remove(UserAccountOptions.Register);
        }
        else
        {
            allValues.Remove(UserAccountOptions.Logout);
        }
        return allValues;
    }

    public Dictionary<object, string> LimitSavingsAccountMenuOptions()
    {
        var options = new MenuOptions();
        var allValues = options.SavingsAccountMenu;
        return allValues;
    }
    
    public int RestrictInputToPositiveInt()
    {
        while (true)
        {
            bool isValid = int.TryParse(Console.ReadLine(), out int input);
            if (isValid && input > 0)
            {
                return input;
            }
            Console.WriteLine("Invalid input. No integer or negative.");
        }
    }
    
    public double RestrictInputToDouble()
    {
        while (true)
        {
            bool isValid = double.TryParse(Console.ReadLine(), out double input);
            if (isValid)
            {
                return input;
            }
            Console.WriteLine("Invalid input. No decimal or integer.");
        }
    }
}