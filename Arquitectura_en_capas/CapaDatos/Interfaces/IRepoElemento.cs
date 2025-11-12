using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoElemento
{
    public void Insert(Elemento elemento);
    public void Update(Elemento elemento);
    public void Delete(int idElemento);
    public void UpdateEstado(int idElemento, int idEstadoMantenimiento);
    public Elemento? GetByNumeroSerie(string numeroSerie);
    public Elemento? GetByCodigoBarra(string codigoBarra);
    public Elemento? GetByPatrimonio(string patrimonio);
    public Elemento? GetById(int idElemento);
    public bool GetDisponible(int idElemento);
    public Elemento? GetNotebookBySerieOrCodigo(string nroSerie, string codigoBarra);
    public IEnumerable<string> GetSerieBarraPatrimonio(string texto, int limite);
    public List<(string Nombre, int Cantidad)> GetElementosPorTipo();
    public int? CantidadEstados(int idEstado);
    public int CantidadTotal();
    public void SetTransaction(IDbTransaction? transaction);
}
