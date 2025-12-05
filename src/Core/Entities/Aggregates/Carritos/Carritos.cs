using Core.Common;

namespace Core.Entities.Aggregates.Carritos;

public class Carritos : Auditoria
{
    public int IdCarrito {get; set;}
    public required string EquipoCarrito { get; set; }
    public required string NumeroSerieCarrito { get; set; }
    public int Capacidad { get; set; }
    public int IdEstadoMantenimiento { get; set; }
    public int IdUbicacion { get; set; }
    public int? IdModelo { get; set; }
    public bool Habilitado { get; set; }
    public DateTime? FechaBaja { get; set; }

    private List<int> Notebooks = new();
    public IReadOnlyList<int> NotebooksIds => Notebooks.AsReadOnly();


    public void AsignarNotebook(int idElemento)
    {
        if (Notebooks.Count >= Capacidad)
        {
            throw new InvalidOperationException("Carrito lleno.");
        }

        if (Notebooks.Contains(idElemento))
        {
            return;
        }

        Notebooks.Add(idElemento);
    }

    public void QuitarNotebook(int idElemento)
    {
        Notebooks.Remove(idElemento);
    }
}
