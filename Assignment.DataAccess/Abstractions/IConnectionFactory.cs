using System.Data;

namespace Assignment.DataAccess.Abstractions
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection(string connectionString);
    }
}
