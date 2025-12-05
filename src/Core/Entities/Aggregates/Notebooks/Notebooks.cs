using Core.Entities.Aggregates.Elementos;

namespace Core.Entities.Aggregates.Notebooks;

public class Notebooks : Elemento
{
    public required string Equipo { get; set; }
    public int? IdCarrito { get; set; }
    public int? PosicionCarrito { get; set; }


    public void AsignarACarrito(int idCarrito, int posicion)
    {
        IdCarrito = idCarrito;
        PosicionCarrito = posicion;
    }

    public void RemoverDeCarrito()
    {
        IdCarrito = null;
        PosicionCarrito = null;
    }
}

