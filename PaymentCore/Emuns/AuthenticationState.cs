namespace PaymentCore.Emuns;

public enum AuthenticationState
{
    LoggedOut,
    LoggedIn,
    WrongPassword,
    InsecurePassword,
    UserNotFound,
    UserAlreadyExists,
    Unregistered,
    Registered
}