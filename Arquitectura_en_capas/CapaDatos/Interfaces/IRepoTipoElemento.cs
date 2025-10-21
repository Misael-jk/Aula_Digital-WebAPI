using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoTipoElemento
{
    public void Insert(TipoElemento tipoElemento);
    public void Update(TipoElemento tipoElemento);
    public void Delete(int idTipoElemento);
    public IEnumerable<TipoElemento> GetAll();
    public TipoElemento? GetById(int idTipoElemento);
    public TipoElemento? GetByTipo(string elementoTipo);
    public void SetTransaction(IDbTransaction? transaction);
}
