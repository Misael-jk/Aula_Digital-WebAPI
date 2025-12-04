using Microsoft.Extensions.Options;
using System.Data;
using MySqlConnector;

namespace Infrastructure.Persistence;

public class ConnectionFactory : IConnectionFactory
{
    private readonly DbSettings opcion;

    public ConnectionFactory(IOptions<DbSettings> opcion)
    {
        this.opcion = opcion.Value;
    }

    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(opcion.DefaultConnection);
    }

    public IDbConnection CreateConnectionForRole(string rol)
    {
        return new MySqlConnection(GetConnectionStringForRole(rol));
    }

    public string GetConnectionStringForRole(string role)
    {
        if (opcion.Users != null && opcion.Users.TryGetValue(role, out string? cs) && !string.IsNullOrEmpty(cs))
        {
            return cs;
        }

        return opcion.DefaultConnection ?? throw new InvalidOperationException("No connection configured");
    }
}
