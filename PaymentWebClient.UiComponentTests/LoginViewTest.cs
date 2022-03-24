using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using PaymentCore.Emuns;
using PaymentCore.Interfaces;
using PaymentCore.UseCases;
using PaymentWebClient.Models;
using PaymentWebClient.Shared;
using Xunit;

namespace PaymentWebClient.UiComponentTests;

public class LoginViewTest
{
    private readonly IRenderedComponent<LoginView> _cut;
    private readonly Mock<ISessionService> _mockSessionService;
    private readonly Mock<ISavingsAccountInteractor> _mockSavingsAccountInteractor;
    private readonly Mock<IUserAuthenticationInteractor> _mockUserAuthenticationInteractor;
    private readonly Mock<IUser> _mockUser;
    private readonly Mock<IUser> _mockLoggedInUser;

    public LoginViewTest()
    {
        var ctx = new TestContext();
        _mockSessionService = new Mock<ISessionService>();
        _mockSavingsAccountInteractor = new Mock<ISavingsAccountInteractor>();
        _mockUserAuthenticationInteractor = new Mock<IUserAuthenticationInteractor>();
        _mockUser = new Mock<IUser>();
        _mockLoggedInUser = new Mock<IUser>();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(_mockSessionService.Object);
        ctx.Services.AddSingleton(_mockSavingsAccountInteractor.Object);
        ctx.Services.AddSingleton(_mockUserAuthenticationInteractor.Object);
        ctx.Services.AddMudServices();
        _cut = ctx.RenderComponent<LoginView>();
    }
    
    [Fact]
    public void LoginUser_ShouldCallAuthenticationMethodAndDisplaySuccessfulPopup_WithProvidedCredentials()
    {
        _mockUser.Setup(m => m.AuthState).Returns(AuthenticationState.LoggedOut);
        _mockSessionService.Setup(m => m.User).Returns(_mockUser.Object);
        _mockLoggedInUser.Setup(m => m.AuthState).Returns(AuthenticationState.LoggedIn);
        _mockUserAuthenticationInteractor.Setup(m=>m.Authenticate(
                It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(_mockLoggedInUser.Object);

        _cut.FindAll("input")[0].Change("username");
        _cut.FindAll("input")[1].Change("password");
        _cut.FindAll("button")[0].Click();
        
        _mockUserAuthenticationInteractor.Verify(m => m.Authenticate("username", "password"), Times.Once);
    }
}