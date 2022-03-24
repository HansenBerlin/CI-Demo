using System.Text;
using Newtonsoft.Json;
using PaymentApplication.Common;
using PaymentApplication.ValueObjects;
using PaymentCore.Emuns;
using PaymentCore.Entities;
using PaymentCore.Interfaces;
using PaymentCore.UseCases;

namespace PaymentApplication.Controller;

public class SavingsAccountController : ISavingsAccountInteractor
{
    private readonly IHttpRequestController _httpRequestController;
    public SavingsAccountController(IHttpRequestController httpRequestController)
    {
        _httpRequestController = httpRequestController;
    }

    public async Task<IUserSavingsAccount> AddAccount(string username)
    {
        string url = $"{ApiStrings.AddNewSavingsAccount}/{username.ToLower()}/0";
        HttpResponseMessage response = await _httpRequestController.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<SavingsAccountEntity>();
        }

        return new SavingsAccountEntity();
    }

    public async Task <PaymentState> MakePayment(IPayment paymentData, IUserSavingsAccount account)
    {
        bool areFundsInsufficient = paymentData.Amount > account.Savings + account.NegativeAllowed * -1;
        if (areFundsInsufficient)
        {
            return PaymentState.InvalidFunds;
        }
        PaymentState result = PaymentState.Pending;
        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(paymentData), Encoding.UTF8);
        HttpResponseMessage response = await _httpRequestController.PostAsync($"{ApiStrings.MakePayment}", httpContent);

        if (response.IsSuccessStatusCode)
        {
            string responseMessage = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<PaymentState>(responseMessage);

        }
        return result;
    }

    public async Task<PaymentState> Deposit(IPayment paymentData)
    {
        PaymentState result = PaymentState.Pending;
        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(paymentData), Encoding.UTF8);
        HttpResponseMessage response = await _httpRequestController.PostAsync($"{ApiStrings.Deposit}", httpContent);

        if (response.IsSuccessStatusCode)
        {
            string responseMessage = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<PaymentState>(responseMessage);

        }
        return result;
    }

    public async Task<bool> IsAccountAvailable(int iD)
    {
        string url = $"{ApiStrings.CheckAccountAvailabilityById}/{iD}";
        HttpResponseMessage response = await _httpRequestController.GetAsync(url);

        bool isAccountAvailable = false;
        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            isAccountAvailable = bool.Parse(result);
        }
        return isAccountAvailable;
    }

    public async Task<IUserSavingsAccount> GetUserAccount(string? username)
    {
        string url = $"{ApiStrings.CheckAccountAvailabilityByUser}/{username}";
        HttpResponseMessage response = await _httpRequestController.GetAsync(url);

        IUserSavingsAccount savingsAccount = new SavingsAccountEntity();
        if (response.IsSuccessStatusCode)
        {
            savingsAccount = await response.Content.ReadAsAsync<SavingsAccountEntity>();
        }
        return savingsAccount;
    }
}