using MySql.Data.MySqlClient;

namespace PaymentInfrastructure.Common;

public interface ISqlService
{
    MySqlConnection con { get; }
}