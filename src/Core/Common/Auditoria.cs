namespace Core.Common;

public abstract class Auditoria
{
    public DateTime FechaCreacion { get; set; }

    public int? CreadoPor { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? ModificadoPor { get; set; }
}
