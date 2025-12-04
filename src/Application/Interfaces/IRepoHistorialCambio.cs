using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoHistorialCambio
{
    public void Insert(HistorialCambios historialCambio);
    public IEnumerable<HistorialCambios> GetAll();
    public HistorialCambios? GetById(int idHistorialCambio);
    public IEnumerable<HistorialCambios> GetByAccion(int idTipoAccion);
    public HistorialCambios? GetUltimateDateByIdNotebook(int idNotebook);
    public HistorialCambios? GetUltimateDateByIdCarrito(int idCarrito);
    public string? GetLastDateByUser(int idUsuario);
    public int CantidadAccionByUser(int  idUsuario, int idAccion);
    public int CantidadPrestamosByUser(int idUsuario);
    public int CantidadDevolucionByUser(int idUsuario);
    public void SetTransaction(IDbTransaction? transaction);
}
