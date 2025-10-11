namespace CapaDTOs.AuditoriaDTO;

public class HistoralElementoDTO
{
    public int IdHistorialElemento { get; set; }
    public required string Equipo { get; set; }
    public required string NumeroSerie { get; set; }
    public required string Modelo { get; set; }
    public required string EstadoMantenimiento { get; set; }
    public required string UbicacionActual { get; set; }
    public required string Descripcion { get; set; }
    public string? Motivo { get; set; }
    public DateTime FechaCambio { get; set; }
    public required string AccionRealizada { get; set; }
    public required string Usuario { get; set; }
}
