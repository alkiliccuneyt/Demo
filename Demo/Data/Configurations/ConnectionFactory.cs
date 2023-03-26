using System.Data.SqlClient;
using Demo.Data.Configurations.Abstract;

namespace Demo.Data.Configurations;

public class ConnectionFactory: IConnectionFactory
{
    private readonly IConnectionConfiguration _configuration;

    public ConnectionFactory(IConnectionConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlConnection CreateConnection()
    {
        var csb = new SqlConnectionStringBuilder(_configuration.DefaultConnection)
        {
            ApplicationName = _configuration.ApplicationName
        };
        return new SqlConnection(csb.ToString());
    }
}