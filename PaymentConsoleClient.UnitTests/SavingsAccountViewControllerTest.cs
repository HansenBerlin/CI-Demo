using System.Threading.Tasks;
using Moq;
using PaymentConsoleClient.Controller;
using PaymentConsoleClient.Interfaces;
using PaymentCore.Emuns;
using PaymentCore.Interfaces;
using PaymentCore.UseCases;
using Xunit;

namespace PaymentConsoleClient.UnitTests;

public class SavingsAccountViewControllerTest
{
    private SavingsAccountViewController _cut;
    private readonly Mock<ISavingsAccountInteractor> _mockSavingsAccountInteractor;
    private readonly Mock<IUser> _mockUser;
    private readonly Mock<IUserSavingsAccount> _mockUserSavingsAccount;
    private readonly Mock<ISelectionValidation> _mockSelectionValidation;

    public SavingsAccountViewControllerTest()
    {
        _mockSelectionValidation = new Mock<ISelectionValidation>();
        _mockSavingsAccountInteractor = new Mock<ISavingsAccountInteractor>();
        _mockUser = new Mock<IUser>();
        _mockUserSavingsAccount = new Mock<IUserSavingsAccount>();
        _mockUser.Setup(m => m.UserSavingsAccount).Returns(_mockUserSavingsAccount.Object);
        _cut = new SavingsAccountViewController(_mockSavingsAccountInteractor.Object, _mockUser.Object, _mockSelectionValidation.Object);
    }
    
    [Fact]
    public async Task MakeDeposit_ShouldReturnSuccessString_WhenPaymentWasSuccessful()
    {
        _mockUserSavingsAccount.Setup(m => m.Id).Returns(1);
        _mockSelectionValidation.Setup(m => m.RestrictInputToPositiveInt()).Returns(100);
        _mockSavingsAccountInteractor.Setup(m => m.Deposit(It.IsAny<IPayment>())).ReturnsAsync(PaymentState.Success);
        
        string result = await _cut.MakeDeposit();
        
        Assert.Equal("Deposit done with state: Success", result);
    }
}