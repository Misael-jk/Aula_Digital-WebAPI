using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace AulaDigital.Test;

public class TestBase : IDisposable
{
    public IDbConnection Conexion { get; private set; }

    public TestBase()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
            .Build();

        string connectionString = config.GetConnectionString("Conexion");

        Conexion = new MySqlConnection(connectionString);
        Conexion.Open(); 
    }

    public void Dispose()
    {
        if (Conexion.State != ConnectionState.Closed)
            Conexion.Close();

        Conexion.Dispose();
    }
}
