using System.IO;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Playwright;

namespace PaymentEndToEndTests;

public class LoginTest
{
    [Fact]
    public async Task LoginUser_ShouldLogin_WhenExisitingCredentialsAreProvided()
    {
        var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 2000,
                Args = new[] { "--disable-dev-shm-usage" }
            });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        await page.GotoAsync("http://localhost:5001/");
        await File.WriteAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(), "loginTestInitialState.png"),
            await page.ScreenshotAsync());
        await page.Locator("input[type=\"text\"]").ClickAsync();
        await page.Locator("input[type=\"text\"]").FillAsync("robert2");
        await page.Locator("input[type=\"text\"]").PressAsync("Tab");
        await page.Locator("input[type=\"password\"]").FillAsync("1111qqqqQQQQ");
        await page.Locator("button:has-text(\"Login\")").ClickAsync();
        await File.WriteAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(), "loginTestIntermediateState.png"),
            await page.ScreenshotAsync());
        await page.Locator("button:has-text(\"Close\")").ClickAsync();
        await page.Locator("text=Logout >> path").Nth(1).ClickAsync();
        await page.Locator("button:has-text(\"Logout\")").ClickAsync();
        await page.Locator("text=Login").ClickAsync();
        
        var countDisabledTabs = await page.Locator("css=[class=\"mud-tab mud-disabled mud-ripple\"]").CountAsync();

        Assert.Equal(3, countDisabledTabs);

        await File.WriteAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(), "loginTestFinalState.png"),
            await page.ScreenshotAsync());
        
    }
}