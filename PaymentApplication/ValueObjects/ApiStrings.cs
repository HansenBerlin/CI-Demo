namespace PaymentApplication.ValueObjects;

public static class ApiStrings
{
    public const string BaseUrl = "https://localhost:7250";
    public const string UserAuthenticate = "Authentication/authenticate";
    public const string UserLogout = "Authentication/logout";
    public const string UserRegister = "Authentication/register";
    public const string NameCheck = "Authentication/users";
    public const string MakePayment = "SavingsAccount/transferfunds";
    public const string Deposit = "SavingsAccount/depositfunds";
    public const string AddNewSavingsAccount = "SavingsAccount/newaccount";
    public const string CheckAccountAvailabilityById = "SavingsAccount/accountbyid";
    public const string CheckAccountAvailabilityByUser = "SavingsAccount/accountbyuser";
}
