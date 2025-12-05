using Core.Entities.Catalogos;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoModelo
{
    public void Insert(Modelos modelo);
    public void Update(Modelos modelo);
    public IEnumerable<Modelos> GetAll();
    public Modelos? GetById(int idModelo);
    public IEnumerable<Modelos> GetByTipo(int idTipoElemento);
    public Modelos? GetByNombre(string nombreModelo);
    public Modelos? GetByTipoYNombre(int idTipoElemento, string nombreModelo);
    public IEnumerable<string> GetNombresModelosByNombreTipo(string Tipo);
    public IEnumerable<string> ObtenerModelo();
    public void SetTransaction(IDbTransaction? transaction);
}
