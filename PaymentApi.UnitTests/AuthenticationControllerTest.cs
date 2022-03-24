using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Moq;
using NuGet.Protocol;
using PaymentApi.Controllers;
using PaymentCore.Emuns;
using PaymentCore.Repositories;
using Xunit;

namespace PaymentApi.UnitTests;

public class AuthenticationControllerTest
{
    private readonly AuthenticationController _cut;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<ISavingsAccountRepository> _mockSavingsAccountsRepo;

    public AuthenticationControllerTest()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockSavingsAccountsRepo = new Mock<ISavingsAccountRepository>();
        _cut = new AuthenticationController(_mockUserRepo.Object, _mockSavingsAccountsRepo.Object);
    }
    
    [Fact]
    public async Task AuthenticateUser_ReturnAuthenticationStateUserNotFound_WhenUserIsUnknown()
    {
        _mockUserRepo.Setup(m => m.IsNameExisting(It.IsAny<string?>())).ReturnsAsync(false);
        
        IActionResult result = await _cut.AuthenticateUser("username", "password");
        string responseMessage = result.ToJson();
        
        Assert.Contains(AuthenticationState.UserNotFound.GetDisplayName(), responseMessage);
    }
    
    [Fact]
    public async Task AuthenticateUser_ReturnAuthenticationStateWrongPassword_WhenPasswordIsNotMatching()
    {
        _mockUserRepo.Setup(m => m.IsNameExisting(It.IsAny<string?>())).ReturnsAsync(true);
        _mockUserRepo.Setup(m => m.IsPasswordHashMatching(It.IsAny<string?>(), It.IsAny<string>())).ReturnsAsync(false);
        
        IActionResult result = await _cut.AuthenticateUser("username", "password");
        string responseMessage = result.ToJson();
        
        Assert.Contains(AuthenticationState.WrongPassword.GetDisplayName(), responseMessage);
    }
    
    [Fact]
    public async Task AuthenticateUser_ReturnAuthenticatedUser_WhenPasswordAndUserNameAreMatching()
    {
        _mockUserRepo.Setup(m => m.IsNameExisting(It.IsAny<string?>())).ReturnsAsync(true);
        _mockUserRepo.Setup(m => m.IsPasswordHashMatching(It.IsAny<string?>(), It.IsAny<string>())).ReturnsAsync(true);
        _mockUserRepo.Setup(m => m.SetLoginState(It.IsAny<string?>(), It.IsAny<bool>())).ReturnsAsync(1);
        
        IActionResult result = await _cut.AuthenticateUser("username", "password");
        string responseMessage = result.ToJson();

        Assert.Contains(AuthenticationState.LoggedIn.GetDisplayName(), responseMessage);
    }
}