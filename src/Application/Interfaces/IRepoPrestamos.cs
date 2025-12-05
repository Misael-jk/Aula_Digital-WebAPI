using Core.Entities.Aggregates.Prestamos;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoPrestamos
{
    public void Insert(Prestamos prestamo);
    public void Update(Prestamos prestamo);
    public void Delete(int idPrestamo);
    public IEnumerable<Prestamos> GetAll();
    public Prestamos? GetById(int idPrestamo);
    public IEnumerable<Prestamos> GetByEncargado(int idEncargado);
    public IEnumerable<Prestamos> GetByDocente(int idDocente);
    public void UpdateEstado(int idPrestamo, int idEstadoPrestamo);
    public bool PrestamoActivo();
    public void SetTransaction(IDbTransaction? transaction);
}
