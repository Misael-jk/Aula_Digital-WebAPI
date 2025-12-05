using Core.Entities.Aggregates.Carritos;
using Core.Entities.Aggregates.Notebooks;
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
    public Notebooks? GetNotebookBySerieOrCodigoOrPatrimonio(string? numeroSerie, string? codigoBarra, string? patrimonio);
    public IEnumerable<string> GetSerieBarraPatrimonio(string text, int limit);
    public IEnumerable<string> GetEquiposNotebooks();
    public Carritos? GetCarritoByNotebook(int idNotebook);
    public List<(string Modelo, int Cantidad)> GetCantidadPorModelo();
    public List<(string Estado, int Cantidad)> GetCantidadEstado();
    public List<(string Equipo, int Cantidad)> GetCantidadNotebooksEnCarritos();
    public IEnumerable<int> GetIdNotebooksByCarrito(int idCarrito);
    public bool EstaEnPrestamo(int idPrestamo);
    public void SetTransaction(IDbTransaction? transaction);
    public int? CantidadEstados(int idEstado);
    public int CantidadTotal();
    public IEnumerable<int> GetIdNotebooksPrestadasByCarrito(int idCarrito);
    public IEnumerable<int> GetIdNotebooksDisponiblesByCarrito(int idCarrito);

}
