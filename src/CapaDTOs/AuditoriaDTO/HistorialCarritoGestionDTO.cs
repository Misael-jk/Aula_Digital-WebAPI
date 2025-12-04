namespace CapaDTOs.AuditoriaDTO;

public class HistorialCarritoGestionDTO
{
    public int IdHistorialCarrito { get; set; }
    public required string Usuario { get; set; }
    public required string AccionRealizada { get; set; }
    public DateTime FechaCambio { get; set; }
}
