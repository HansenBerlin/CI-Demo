namespace PaymentCore.Policies;

public record PasswordSecurityRules
{
    public static int MinLength => 12;
    public static bool LowerUpper => true;
    public static bool Digits => true;
    public static int SpecialChars => 0;
    public static bool NoReservedChars => true;
}