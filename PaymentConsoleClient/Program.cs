using System.Threading.Tasks;
using PaymentApplication.Common;
using PaymentApplication.Controller;
using PaymentConsoleClient.Controller;
using PaymentConsoleClient.Interfaces;
using PaymentCore.Entities;
using PaymentCore.Interfaces;
using PaymentCore.UseCases;

namespace PaymentConsoleClient;

public static class Program
{
    static async Task Main(string[] args)
    {
        //var client = new HttpClient();
        IUser user = new UserEntity();

        ISecurityPolicyInteractor pwCheck = new PasswordSecurityController();
        //IHttpRequestController httpRequestController = new HttpRequestController(client);
        IHttpRequestController httpRequestController = new HttpRequestController();
        IUserAuthenticationInteractor uai = new UserAuthenticationController(pwCheck, httpRequestController);
        ISavingsAccountInteractor sai = new SavingsAccountController(httpRequestController);
        ISelectionValidation selectionValidationController = new SelectionValidationController(user);
        IUserAuthenticationViewController userAuth = new UserAuthenticationViewController(uai, user);
        ISavingsAccountViewController sacAcc = new SavingsAccountViewController(sai, user, selectionValidationController);
        var viewController = new MainViewController(userAuth, selectionValidationController, sacAcc);
        
        await viewController.Start();
    }
}