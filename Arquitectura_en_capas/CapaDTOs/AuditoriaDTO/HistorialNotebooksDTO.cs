namespace CapaDTOs.AuditoriaDTO;

public class HistorialNotebooksDTO
{
    public int IdHistorialNotebook { get; set; }
    public required string Equipo { get; set; }
    public required string NumeroSerie { get; set; }
    public string? Carrito { get; set; }
    public int? PosicionCarrito { get; set; }
    public required string Modelo { get; set; }
    public required string EstadoMantenimiento { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaCambio { get; set; }
    public required string AccionRealizada { get; set; }
    public required string Usuario { get; set; }
}
