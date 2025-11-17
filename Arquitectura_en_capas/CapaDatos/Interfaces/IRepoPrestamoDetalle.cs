using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoPrestamoDetalle
{
    public void Insert(PrestamoDetalle prestamoDetalle);
    public void Update(PrestamoDetalle prestamoDetalle);
    public void Delete(int idPrestamo);
    public IEnumerable<PrestamoDetalle> GetByPrestamo(int idPrestamo);
    public IEnumerable<PrestamoDetalle> GetAll();
    public Elemento? GetByElemento(int idElemento);
    public bool PerteneceAlPrestamo(int idPrestamo, int idElemento);
    public int GetCountByPrestamo(int idPrestamo);
    public IEnumerable<Elemento> GetElementosPendientes(int idPrestamo);
    public List<int> GetIdsElementosByIdPrestamo(int idPrestamo);
    public void SetTransaction(IDbTransaction? transaction);

}
