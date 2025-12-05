namespace Core.Entities.Aggregates.Prestamos;

public class PrestamoDetalleCambio
{
    public int IdCambio { get; set; }
    public int IdPrestamo { get; set; }
    public int IdTipoAnomalia { get; set; }
    public int ElementoOriginal { get; set; }
    public int ElementoReemplazo { get; set; }
    public int IdUsuarioRegistro { get; set; }
    public DateTime FechaCambio { get; set; }
    public string? Motivo { get; set; }
}
