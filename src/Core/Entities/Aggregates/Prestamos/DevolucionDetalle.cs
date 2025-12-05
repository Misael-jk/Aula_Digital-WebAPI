namespace Core.Entities.Aggregates.Prestamos;

public class DevolucionDetalle
{
    public int IdDevolucion { get; set; }
    public int IdElemento { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public string? Observaciones { get; set; }
}
