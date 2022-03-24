using PasswordCheckerLibrary;
using PaymentCore.Policies;
using PaymentCore.UseCases;

namespace PaymentApplication.Controller;

public class PasswordSecurityController : ISecurityPolicyInteractor
{
    public bool IsPasswordCompliantToSecurityRules(string password, PasswordSecurityRules rules)
    {
        var passwordChecker = new PasswordSecurityChecker(PasswordSecurityRules.MinLength, PasswordSecurityRules.SpecialChars, PasswordSecurityRules.NoReservedChars,
            PasswordSecurityRules.LowerUpper, PasswordSecurityRules.Digits);
        return passwordChecker.IsPasswordSecure(password);
    }
}