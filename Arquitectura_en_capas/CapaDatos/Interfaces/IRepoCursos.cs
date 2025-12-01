using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoCursos
{
    public Curso? GetById(int idCurso);
    public IEnumerable<Curso> GetAll();
    public void SetTransaction(IDbTransaction? transaction);
}
