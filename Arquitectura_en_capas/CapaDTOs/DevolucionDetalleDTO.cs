namespace CapaDTOs;

public class DevolucionDetalleDTO
{
    public required string TipoElemento { get; set; }
    public required string NumeroSerieElemento { get; set; }
    public string? NumeroSerieCarrito { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public required string EstadoMantenimiento { get; set; }
    public required string Observacion { get; set; }
}
