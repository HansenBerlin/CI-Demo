using System.Collections.Generic;
using PaymentConsoleClient.Enums;

namespace PaymentConsoleClient.ValueObjects;

public record MenuOptions()
{
    public Dictionary<object, string> MainMenu = new()
    {
        {MainMenuOptions.UserAccountOptions, "User Account Options"},
        {MainMenuOptions.SavingsAccountOptions, "Savings Account Options"},
        {MainMenuOptions.Exit, "Exit application"}
    };
    
    public Dictionary<object, string> SavingsAccountMenu = new()
    {
        {SavingsAccountOptions.Send, "Send worthless money to another user"},
        {SavingsAccountOptions.Deposit, "Deposit money out of nothing"},
        {SavingsAccountOptions.ChangeDailyLimit, "Change the maximum amount you can spend per day"},
        {SavingsAccountOptions.Back, "back to main menu"}
    };
    
    public Dictionary<object, string> UserAccountMenu = new()
    {
        {UserAccountOptions.Login, "Login with an existing user"},
        {UserAccountOptions.Register, "Register new user with empty savings account"},
        {UserAccountOptions.Logout, "Logout"},
        {UserAccountOptions.Back, "back to main menu"}
    };
};