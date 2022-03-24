using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PaymentWebClient;
using MudBlazor.Services;
using PaymentApplication.Common;
using PaymentApplication.Controller;
using PaymentCore.UseCases;
using PaymentWebClient.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<ISessionService, SessionService>();
builder.Services.AddScoped<ISecurityPolicyInteractor, PasswordSecurityController>();
builder.Services.AddScoped<IHttpRequestController, HttpRequestController>();
builder.Services.AddScoped<IUserAuthenticationInteractor, UserAuthenticationController>();
builder.Services.AddScoped<ISavingsAccountInteractor, SavingsAccountController>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
