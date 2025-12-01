using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoUbicacion
{
    public void Insert(Ubicacion ubicacion);
    public void Update(Ubicacion ubicacion);
    public IEnumerable<Ubicacion> GetAll();
    public Ubicacion? GetByUbicacion(string ubicacion);
    public Ubicacion? GetById(int idUbicacion);
    public IEnumerable<Ubicacion> GetByTipo(int tipo);
    public void SetTransaction(IDbTransaction? transaction);
}
