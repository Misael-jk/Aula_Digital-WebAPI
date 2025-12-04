namespace CapaDTOs.MantenimientoDTO;

public class NotebooksBajasDTO
{
    public int IdNotebook { get; set; }
    public required string Equipo { get; set; }
    public string? Carrito { get; set; }
    public int? PosicionCarrito { get; set; }
    public required string NumeroSerieNotebook { get; set; }
    public required string CodigoBarra { get; set; }
    public required string Estado { get; set; }
    public required string Patrimonio { get; set; }
    public required string Modelo { get; set; }
    public required string Ubicacion { get; set; }
    public required DateTime FechaBaja { get; set; }
}
