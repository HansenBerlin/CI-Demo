using Microsoft.AspNetCore.Mvc;
using PaymentCore.Emuns;
using PaymentCore.Entities;
using PaymentCore.Repositories;

namespace PaymentApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SavingsAccountController : ControllerBase
    {
        private readonly ISavingsAccountRepository _savings;

        public SavingsAccountController(ISavingsAccountRepository savings)
        {
            _savings = savings;
        }

        [HttpGet("newaccount/{userName}/{initialAmount}")]
        public async Task<IActionResult> AddNewAccount(string userName, int initialAmount)
        {
            var result = await _savings.AddNewAccount(initialAmount, userName);
            return Ok(result);
        }
        
        [HttpGet("accountbyid/{iD}")]
        public async Task<IActionResult> CheckForAccount(int iD)
        {
            var result = await _savings.IsAccountAvailable(iD);
            return Ok(result);
        }
        
        [HttpPost("transferfunds")]
        public async Task<IActionResult> SendFunds([FromBody] PaymentEntity payment)
        {
            var result = await _savings.SubstractFunds(payment.Amount, payment.FromAccountId);
            if (result == 1)
                result = await _savings.AddFunds(payment.Amount, payment.ToAccountId);
            if (result == 1)
                return Ok(PaymentState.Success);
            return Ok(PaymentState.Failed);
        }
        
        [HttpPost("depositfunds")]
        public async Task<IActionResult> AddFunds([FromBody] PaymentEntity payment)
        {
            var result = await _savings.AddFunds(payment.Amount, payment.ToAccountId);
            if (result == 1)
                return Ok(PaymentState.Success);
            return Ok(PaymentState.Failed);
        }
        
        [HttpGet("accountbyuser/{userName}")]
        public async Task<IActionResult> GetAccountByUsername(string userName)
        {
            var result = await _savings.GetUserSavingsAccount(userName);
            if (result.Id != -1)
                return Ok(result);
            return BadRequest("No account found");
        }
    }
}