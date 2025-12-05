using Core.Entities.Aggregates.Docentes;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoDocentes
{
    public void Insert(Docentes docente);
    public void Update(Docentes docente);
    public void Deshabilitar(int idDocente, bool habilitado);
    public void Delete(int idDocente);
    public Docentes? GetById(int idDocente);
    public Docentes? GetByDni(string Dni);
    public Docentes? GetByEmail(string Email);
    public Docentes? GetByNombre(string nombre);
    public bool ExistsPrestamo(int idDocente);
    public IEnumerable<string> GetFiltroNombre(string nombre, int limit);
    public Docentes? FiltroGetDocenteByID(string dni);
    public IEnumerable<Docentes> GetAll();
    public void SetTransaction(IDbTransaction? transaction);
}
