using PaymentCore.Policies;

namespace PaymentCore.UseCases;

public interface ISecurityPolicyInteractor
{
    bool IsPasswordCompliantToSecurityRules(string password, PasswordSecurityRules rules);
}