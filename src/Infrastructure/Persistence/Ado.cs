using System.Data;

namespace Infrastructure.Persistence;

public class Ado
{
    private readonly IConnectionFactory factory;
    private readonly IGetRolActualService rolService;

    public Ado(IConnectionFactory factory, IGetRolActualService rolService)
    {
        this.factory = factory;
        this.rolService = rolService;
    }

    public IDbConnection GetDbConnection()
    {
        string rol = rolService.GetRolActual();
        return factory.CreateConnectionForRole(rol);
    }
}
