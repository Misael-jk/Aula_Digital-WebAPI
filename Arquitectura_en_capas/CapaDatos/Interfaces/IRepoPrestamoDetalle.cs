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
    public void SetTransaction(IDbTransaction? transaction);

}
