using System.Data;

namespace Application.IService;

public interface IConnectionFactory
{
    public IDbConnection CreateConnection();
    public IDbConnection CreateConnectionForRole(string role);
    public string GetConnectionStringForRole(string role);
}
