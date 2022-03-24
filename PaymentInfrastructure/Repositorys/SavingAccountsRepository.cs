using System.Globalization;
using MySql.Data.MySqlClient;
using PaymentCore.Entities;
using PaymentCore.Interfaces;
using PaymentCore.Repositories;
using PaymentInfrastructure.Common;

namespace PaymentInfrastructure.Repositorys;

public class SavingAccountsRepository : ISavingsAccountRepository
{
    private readonly MySqlConnection _connection;
    
    public SavingAccountsRepository(ISqlService service)
    {
        _connection = service.con;
    }
    
    public async Task<int> AddFunds(double amount, int toId)
    {
        await _connection.OpenAsync();
        string sql = $"UPDATE savings_account SET savings_amount = savings_amount + {amount.ToString(CultureInfo.InvariantCulture)} WHERE id = {toId};";
        await using var cmd = new MySqlCommand(sql, _connection);
        var value = await cmd.ExecuteNonQueryAsync();
        await _connection.CloseAsync();
        return value;
    }

    public async Task<int> SubstractFunds(double amount, int fromId)
    {
        await _connection.OpenAsync();
        string sql = $"UPDATE savings_account SET savings_amount = savings_amount - {amount.ToString(CultureInfo.InvariantCulture)} WHERE id = {fromId};";
        await using var cmd = new MySqlCommand(sql, _connection);
        var value = await cmd.ExecuteNonQueryAsync();
        await _connection.CloseAsync();
        return value;
    }

    public Task<int> ChangeDailyLimit(int amount)
    {
        throw new NotImplementedException();
    }

    public Task<int> ChangeMaxNegativeLimit(int amount)
    {
        throw new NotImplementedException();
    }

    public async Task<IUserSavingsAccount> AddNewAccount(int initialAmount, string userName)
    {
        await _connection.OpenAsync();
        string sql = $"INSERT INTO savings_account (savings_amount, max_negative, max_spending_per_day, fk_user) " +
                     $"VALUES ({initialAmount}, {initialAmount*2}, 0, '{userName}');" + 
                     $"SELECT * FROM savings_account WHERE id = LAST_INSERT_ID();";
        await using var cmd = new MySqlCommand(sql, _connection);
        await using var reader = await cmd.ExecuteReaderAsync();
        var account = new SavingsAccountEntity();
        while (await reader.ReadAsync())
        {
            account.Id = reader.GetInt32(0);
            account.Savings = reader.GetDouble(1);
            account.NegativeAllowed = reader.GetDouble(2);
            account.MaxSpendingPerDay = reader.GetDouble(3);
        }

        return account;
    }

    public async Task<bool> IsAccountAvailable(int iD)
    {
        await _connection.OpenAsync();
        string sql = $"SELECT id FROM savings_account WHERE id = '{iD}';";
        await using var cmd = new MySqlCommand(sql, _connection);
        await using var rdr = await cmd.ExecuteReaderAsync();
        int idFound = -1;

        while (await rdr.ReadAsync())
        {
            idFound = rdr.GetInt32(0);
        }
        await _connection.CloseAsync();

        return idFound != -1;
    }

    public Task<IUserSavingsAccount> DeleteAccount(int accountId)
    {
        throw new NotImplementedException();
    }

    public async Task<IUserSavingsAccount> GetUserSavingsAccount(string userName)
    {
        await _connection.OpenAsync();
        string sql = $"SELECT id, savings_amount, max_negative, max_spending_per_day FROM savings_account WHERE fk_user = '{userName}';";
        await using var cmd = new MySqlCommand(sql, _connection);
        await using var rdr = await cmd.ExecuteReaderAsync();
        IUserSavingsAccount account = new SavingsAccountEntity {Id = -1};

        while (await rdr.ReadAsync())
        {
            account.Id = rdr.GetInt32(0);
            account.Savings = rdr.GetDouble(1);
            account.NegativeAllowed = rdr.GetDouble(2);
            account.MaxSpendingPerDay = rdr.GetDouble(3);
        }
        await _connection.CloseAsync();
        return account;
    }
}