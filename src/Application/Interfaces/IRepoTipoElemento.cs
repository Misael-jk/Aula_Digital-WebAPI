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
    public TipoElemento? GetByNombreTipo(string elementoTipo);
    public IEnumerable<TipoElemento> GetByIdTipo(int idTipo);
    public IEnumerable<TipoElemento> GetTiposByElemento();
    public IEnumerable<string> GetNombreTipos();
    public string? GetNombreTipoById(int idTipoElemento);
    public void SetTransaction(IDbTransaction? transaction);
}
