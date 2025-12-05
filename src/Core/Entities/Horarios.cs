using Core.Common;

namespace Core.Entities;

public class Horarios : Auditoria
{
    public int IdHorario { get; set; }
    public int IdTurno { get; set; }
    public DateTime Entrada { get; set; }
    public DateTime Salida { get; set; }
}
