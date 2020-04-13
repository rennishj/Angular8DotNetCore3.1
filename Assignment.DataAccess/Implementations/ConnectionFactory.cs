using Assignment.DataAccess.Abstractions;
using System.Data;
using System.Data.SqlClient;

namespace Assignment.DataAccess.Implementations
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
