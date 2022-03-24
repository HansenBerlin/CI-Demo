using System;
using System.Threading.Tasks;
using PaymentApplication.Common;
using PaymentConsoleClient.Enums;
using PaymentConsoleClient.Interfaces;
using PaymentConsoleClient.Views;
using static System.Console;

namespace PaymentConsoleClient.Controller;

public class MainViewController
{
    private readonly IUserAuthenticationViewController _userAuth;
    private readonly ISelectionValidation _limitOptions;
    private readonly ISavingsAccountViewController _savingsAccountController;

    public MainViewController(IUserAuthenticationViewController userAuth, ISelectionValidation limitOptions, ISavingsAccountViewController savingsAccountController)
    {
        _userAuth = userAuth;
        _limitOptions = limitOptions;
        _savingsAccountController = savingsAccountController;
    }
    public async Task Start()
    {
        Title = "Payment App";
        await RunMainMenu();
    }

    private async Task RunMainMenu()
    {
        string prompt = @"

██████╗  █████╗ ██╗   ██╗███╗   ███╗███████╗███╗   ██╗████████╗     █████╗ ██████╗ ██████╗ 
██╔══██╗██╔══██╗╚██╗ ██╔╝████╗ ████║██╔════╝████╗  ██║╚══██╔══╝    ██╔══██╗██╔══██╗██╔══██╗
██████╔╝███████║ ╚████╔╝ ██╔████╔██║█████╗  ██╔██╗ ██║   ██║       ███████║██████╔╝██████╔╝
██╔═══╝ ██╔══██║  ╚██╔╝  ██║╚██╔╝██║██╔══╝  ██║╚██╗██║   ██║       ██╔══██║██╔═══╝ ██╔═══╝ 
██║     ██║  ██║   ██║   ██║ ╚═╝ ██║███████╗██║ ╚████║   ██║       ██║  ██║██║     ██║     
╚═╝     ╚═╝  ╚═╝   ╚═╝   ╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝   ╚═╝       ╚═╝  ╚═╝╚═╝     ╚═╝     
                                                                                           

Welcome to the Payment App. What would you like to do?
(Use the arrow keys to cycle through options and press enter to select an option)
 ";

        var options = _limitOptions.LimitMainMenuOptions();
        var mainMenu = new MasterMenu(prompt, options);
        var selectedIndex = (MainMenuOptions)mainMenu.Run();

        switch (selectedIndex)
        {
            case MainMenuOptions.UserAccountOptions:
                await ShowUserAccountOptions();
                break;
            case MainMenuOptions.SavingsAccountOptions:
                await ShowSavingsAccountOptions();
                break;
            case MainMenuOptions.Exit:
                ExitApplication();
                break;
        }

        await RunMainMenu();
    }

    private void ExitApplication()
    {
        WriteLine("\nBye!");
        Environment.Exit(0);
    }

    private async Task ShowUserAccountOptions()
    {
        var options = _limitOptions.LimitUserAccountMenuOptions();
        string prompt = "USER ACCOUNT OPTIONS: ";
        var userAccountMenu = new MasterMenu(prompt, options);
        var selectedIndex = (UserAccountOptions)userAccountMenu.Run();

        switch (selectedIndex)
        {
            case UserAccountOptions.Back:
                await RunMainMenu();
                break;
            case UserAccountOptions.Login:
            {
                var result = await _userAuth.ValidateLoginInput();
                WriteLine(result);
                Wait();
                break;
            }
            case UserAccountOptions.Register:
            {
                string userName = await _userAuth.ValidateUsernameInput();
                string result = await _userAuth.ValidateUserRegistrationInput(userName);
                WriteLine(result);
                Wait();
                break;
            }
            case UserAccountOptions.Logout:
                WriteLine("logout");
                Wait();
                break;
        }
    }
    private async Task ShowSavingsAccountOptions()
    {
        var options = _limitOptions.LimitSavingsAccountMenuOptions();
        string prompt = "SAVINGS ACCOUNT OPTIONS\n\n";
        string accountData = await _savingsAccountController.ShowAccountData();
        var savingsAccountMenu = new MasterMenu(prompt + accountData, options);
        var selectedIndex = (SavingsAccountOptions)savingsAccountMenu.Run();

        if (selectedIndex == SavingsAccountOptions.Send)
        {
            string result = await _savingsAccountController.SendPayment();
            WriteLine(result);
            Wait();
        }
        else if (selectedIndex == SavingsAccountOptions.Deposit)
        {
            string result = await _savingsAccountController.MakeDeposit();
            WriteLine(result);
            Wait();
        }
        else if (selectedIndex == SavingsAccountOptions.ChangeDailyLimit)
        {
            WriteLine("Change daily limit");
            Wait();
        }
        else if (selectedIndex == SavingsAccountOptions.Back)
        {
            await RunMainMenu();
        }
    }

    private void Wait()
    {
        WriteLine("Press any key to continue...");
        ReadKey(true);
    }
}