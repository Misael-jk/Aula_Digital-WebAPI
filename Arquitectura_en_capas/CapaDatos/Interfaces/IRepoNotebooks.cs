using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoNotebooks
{
    public void Insert(Notebooks notebook);
    public void Update(Notebooks notebook);
    public IEnumerable<Notebooks> GetAll();
    public IEnumerable<Notebooks> GetByCarrito(int idCarrito);
    public Notebooks? GetById(int idNotebook);
    public Notebooks? GetByNumeroSerie(string numeroSerie);
    public Notebooks? GetByCodigoBarra(string codigoBarra);
    public Notebooks? GetByPatrimonio(string patrimonio);
    public Notebooks? GetByEquipo(string equipo);
    public Notebooks? GetNotebookByPosicion(int? idCarrito, int posicionCarrito);
    public bool DuplicatePosition(int idCarrito, int posicionCarrito);
    public bool GetDisponible(int idElemento);
    public void UpdateEstado(int idElemento, int idEstadoMantenimiento);
    public IEnumerable<Notebooks> GetNroSerieByNotebook();
    public IEnumerable<Notebooks> GetCodBarraByNotebook();
    public IEnumerable<Notebooks> GetNotebookByCarrito(int idCarrito);
    public Notebooks? GetNotebookBySerieOrCodigo(string numeroSerie, string codigoBarra);
    public Carritos? GetCarritoByNotebook(int idNotebook);
    public void SetTransaction(IDbTransaction? transaction);
}
