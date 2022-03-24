using PaymentCore.Repositories;
using MySql.Data.MySqlClient;
using PaymentInfrastructure.Common;

namespace PaymentInfrastructure.Repositorys;

public class UserRepository : IUserRepository
{
    private readonly MySqlConnection _connection;
    
    public UserRepository(ISqlService service)
    {
        _connection = service.con;
    }
    
    public async Task<string> GetById(int iD)
    {
        await _connection.OpenAsync();
        string sql = $"USE payments; SELECT * FROM user WHERE id = {iD};";
        await using var cmd = new MySqlCommand(sql, _connection);
        await using var reader = await cmd.ExecuteReaderAsync();
        string returnValue = "";
        while (await reader.ReadAsync())
        {
            returnValue = reader.GetString(0);
        }

        await _connection.CloseAsync();
        return returnValue;
    }
    
    public async Task<int> UpdatePasswordHash(string name, string pwHash)
    {
        await _connection.OpenAsync();
        string sql = $"UPDATE user SET pw_hash = '{pwHash}' WHERE name = '{name}';";
        await using var cmd = new MySqlCommand(sql, _connection);
        var value = await cmd.ExecuteNonQueryAsync();
        await _connection.CloseAsync();
        return value;
    }
    
    public async Task<bool> IsPasswordHashMatching(string? name, string pwHash)
    {
        await _connection.OpenAsync();
        string sql = $"SELECT pw_hash FROM user WHERE name = '{name}';";
        await using var cmd = new MySqlCommand(sql, _connection);
        await using var rdr = await cmd.ExecuteReaderAsync();
        string hash = "";

        while (await rdr.ReadAsync())
        {
            hash = rdr.GetString(0);
        }
        await _connection.CloseAsync();
        return hash == pwHash;
    }
    
    public async Task<bool> IsNameExisting(string? name)
    {
        await _connection.OpenAsync();
        string sql = $"SELECT name FROM user WHERE name = '{name}';";
        await using var cmd = new MySqlCommand(sql, _connection);
        await using var rdr = await cmd.ExecuteReaderAsync();
        string userName = "";

        while (await rdr.ReadAsync())
        {
            userName = rdr.GetString(0);
        }
        await _connection.CloseAsync();

        return userName == name;
    }
    
    public async Task<int> SetLoginState(string? name, bool isLoggedIn)
    {
        await _connection.OpenAsync();
        string sql = $"UPDATE user SET is_logged_in = {(isLoggedIn ? 1 : 0)} WHERE name = '{name}';";
        await using var cmd = new MySqlCommand(sql, _connection);
        var value = await cmd.ExecuteNonQueryAsync();
        await _connection.CloseAsync();
        return value;
    }

    public async Task<int> AddNewUser(string userName)
    {
        await _connection.OpenAsync();
        string sql = $"INSERT INTO user (name) VALUES ('{userName}');";
        await using var cmd = new MySqlCommand(sql, _connection);
        var value = await cmd.ExecuteNonQueryAsync();
        await _connection.CloseAsync();
        return value;
    }
}