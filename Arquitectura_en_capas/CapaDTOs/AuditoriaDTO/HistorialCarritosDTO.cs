namespace CapaDTOs.AuditoriaDTO;

public class HistorialCarritosDTO
{
    public int IdHistorialCarrito { get; set; }
    public required string Equipo { get; set; }
    public required string NumeroSerieCarrito { get; set; }
    public required string UbicacionActual { get; set; }
    public required string Modelo { get; set; }
    public required string EstadoMantenimiento { get; set; }
    public required string? Descripcion { get; set; }
    public DateTime FechaCambio { get; set; }
    public required string AccionRealizada { get; set; }
    public required string Usuario { get; set; }
}
