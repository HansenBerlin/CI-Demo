using System.IO;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Playwright;

namespace PaymentEndToEndTests;

public class LoginFakeTest
{
    [Fact]
    public async Task LoginUser_ShouldLogin_WhenExisitingCredentialsAreProvided()
    {
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
        await File.WriteAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(), "loginTestInitialState2.png"),
            await page.ScreenshotAsync());
        await page.Locator("input[type=\"text\"]").ClickAsync();
        await page.Locator("input[type=\"text\"]").FillAsync("robert2");
        await page.Locator("input[type=\"text\"]").PressAsync("Tab");
        await page.Locator("input[type=\"password\"]").FillAsync("1111qqqqQQQQ");
        await File.WriteAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(), "loginTestIntermediateState2.png"),
            await page.ScreenshotAsync());
        var countDisabledTabs = await page.Locator("css=[class=\"mud-tab mud-disabled mud-ripple\"]").CountAsync();

        Assert.Equal(3, countDisabledTabs);

        await File.WriteAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(), "loginTestFinalState2.png"),
            await page.ScreenshotAsync());
        
    }
}