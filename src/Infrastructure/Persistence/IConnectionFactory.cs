using System.Data;

namespace Infrastructure.Persistence;

public interface IConnectionFactory
{
    public IDbConnection CreateConnection();
    public IDbConnection CreateConnectionForRole(string role);
    public string GetConnectionStringForRole(string role);
}
