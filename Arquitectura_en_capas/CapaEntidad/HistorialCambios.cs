namespace CapaEntidad;

public class HistorialCambios
{
    public int IdHistorialCambio { get; set; }
    public int IdTipoAccion { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaCambio { get; set; }
    public required string Descripcion { get; set; }
    public string? Motivo { get; set; }
}
