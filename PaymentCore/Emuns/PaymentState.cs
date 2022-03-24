namespace PaymentCore.Emuns;

public enum PaymentState
{
    Success,
    InvalidFunds,
    UserNotFound,
    Pending,
    DailyLimitReached,
    Failed
}