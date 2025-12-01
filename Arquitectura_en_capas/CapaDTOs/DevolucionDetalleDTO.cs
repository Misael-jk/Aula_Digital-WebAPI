namespace CapaDTOs;

public class DevolucionDetalleDTO
{
    public required int idElemento { get; set; }
    public required string TipoElemento { get; set; }
    public required string Equipo { get; set; }
    public string? PosicionCarrito { get; set; }
    public required string NumeroSerieElemento { get; set; }
    public required string Patrimonio { get; set; }
    public DateTime? FechaDevolucion {  get; set; }
}
