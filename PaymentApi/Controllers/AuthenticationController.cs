using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PaymentCore.Emuns;
using PaymentCore.Entities;
using PaymentCore.Interfaces;
using PaymentCore.Repositories;

namespace PaymentApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly ISavingsAccountRepository _savingsAccounts;

    public AuthenticationController(IUserRepository users, ISavingsAccountRepository savingsAccounts)
    {
        _users = users;
        _savingsAccounts = savingsAccounts;
    }

    [HttpGet("authenticate/{userName}/{password}")]
    public async Task<IActionResult> AuthenticateUser(string? userName, string password)
    {
        if (await _users.IsNameExisting(userName))
        {
            string pwHashed = GeneratePasswordHash(password);
            if (await _users.IsPasswordHashMatching(userName, pwHashed))
            {
                if (await _users.SetLoginState(userName, true) == 1)
                {
                    IUser user = new UserEntity { Name = userName };
                    user.AuthState = AuthenticationState.LoggedIn;
                    return Ok(user);
                }
                return BadRequest(AuthenticationState.LoggedOut);
            }
            return BadRequest(AuthenticationState.WrongPassword);
        }
        return BadRequest(AuthenticationState.UserNotFound);
    }
    
    [HttpGet("users/{name}")]
    public async Task<IActionResult> GetUser(string? name)
    {
        var result = await _users.IsNameExisting(name);
        return Ok(result.ToString());
    }
    
    [HttpGet("logout/{userName}")]
    public async Task<IActionResult> GetLogoutState(string? userName)
    {
        var result = await _users.SetLoginState(userName, false);
        return Ok(result.ToString());
    }
    
    [HttpGet("register/{userName}/{password}")]
    public async Task<IActionResult> RegisterNewUser(string userName, string password)
    {
        IUser user = new UserEntity { Name = userName };
        try
        {
            var result = await _users.AddNewUser(userName);
            if (result == 1)
            {
                password = GeneratePasswordHash(password);
                await _users.UpdatePasswordHash(userName, password);
                //user = await _users.Login(user);
                await _users.SetLoginState(user.Name, true);
                user.AuthState = AuthenticationState.LoggedIn;
                await _savingsAccounts.AddNewAccount(0, userName);
                return Ok(user);
            }

            return BadRequest(AuthenticationState.UserAlreadyExists);
        }
        catch (Exception e)
        {
            return BadRequest(AuthenticationState.LoggedOut);
        }
    }
    
    private string GeneratePasswordHash(string plainPassword)
    {
        byte[] data = Encoding.UTF8.GetBytes(plainPassword);
        using SHA512 sham = new SHA512Managed();
        return Convert.ToBase64String(sham.ComputeHash(data));
    }
}
