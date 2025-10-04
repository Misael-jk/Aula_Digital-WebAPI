namespace CapaDTOs;

public class InventarioDTO
{
    public required string Equipo { get; set; }
    public required string Modelo { get; set; }
    public int CantidadTotal { get; set; }
    public int CantidadDisponible { get; set; }
    public DateTime FechaCambio { get; set; }
    public required string Observacion { get; set; }
    public required string Usuario { get; set; }
}
