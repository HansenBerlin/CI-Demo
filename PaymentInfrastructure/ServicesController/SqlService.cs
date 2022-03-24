using MySql.Data.MySqlClient;
using PaymentInfrastructure.Common;

namespace PaymentInfrastructure.ServicesController;

public class SqlService : ISqlService
{
    public MySqlConnection con { get; }
    public SqlService()
    {
        string cs = @"server=localhost;port=11111;userid=root;password=my-secret-pw;database=payments;";
        con = new MySqlConnection(cs);
    }
}
