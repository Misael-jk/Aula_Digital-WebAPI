namespace Core.Entities.Aggregates.Prestamos;

public class Devolucion
{
    public int IdDevolucion {get; set;}
    public int IdPrestamo {get; set;}
    public int IdUsuario {get; set;}
    public DateTime FechaDevolucion {get; set;}
    public bool CarritoDevuelto { get; set; }
    public string? Observaciones {get; set;}


    public List<DevolucionDetalle> Detalle { get; set; } = new();
    public List<DevolucionAnomalias> Anomalias { get; set; } = new();


    #region AGREGAR ELEMENTO DEVUELTO
    public void AddDetalle(int idElemento, DateTime fechaDevolucion, string? observaciones = null)
    {
        if (Detalle.Any(d => d.IdElemento == idElemento))
            throw new InvalidOperationException("Elemento ya registrado en la devolución.");

        Detalle.Add(new DevolucionDetalle
        {
            IdElemento = idElemento,
            FechaDevolucion = fechaDevolucion,
            Observaciones = observaciones
        });
    }
    #endregion

    #region AGREGAR ANOMALIAS
    public void AddAnomalia(int idElemento, int idTipoAnomalia, string? descripcion)
    {
        if (!Detalle.Any(d => d.IdElemento == idElemento))
        {
            throw new InvalidOperationException("No se puede asociar anomalia a un elemento que no fue devuelto.");
        }

        if (Anomalias.Any(a => a.IdElemento == idElemento && a.IdTipoAnomalia == idTipoAnomalia))
        {
            return;
        }

        Anomalias.Add(new DevolucionAnomalias
        {
            IdElemento = idElemento,
            IdTipoAnomalia = idTipoAnomalia,
            Descripcion = descripcion
        });
    }
    #endregion

    #region ESTA EN PARCIAL?
    public bool IsPartial(int totalElementosPrestamo)
    {
        return Detalle.Count > 0 && Detalle.Count < totalElementosPrestamo;
    }
    #endregion

    public bool IsFull(int totalElementosPrestamo) => Detalle.Count == totalElementosPrestamo;

}
