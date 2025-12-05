namespace Core.Entities.Aggregates.Notebooks;

public class HistorialNotebooks
{
    public int IdHistorialNotebook { get; set; }
    public int IdTipoAccion { get; set; }
    public int IdUsuario { get; set; }
    public int IdElemento { get; set; }
    public string CampoModificado { get; set; } = null!;
    public string ValorAnterior { get; set; } = null!;
    public string ValorNuevo { get; set; } = null!;
    public DateTime FechaAccion { get; set; }
    public string? Motivo { get; set; }
}
