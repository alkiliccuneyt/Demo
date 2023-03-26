using System.Data.SqlClient;

namespace Demo.Data.Configurations.Abstract;

public interface IConnectionFactory
{
    SqlConnection CreateConnection();
}