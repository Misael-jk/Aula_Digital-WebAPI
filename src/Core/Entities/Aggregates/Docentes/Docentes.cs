using Core.Common;

namespace Core.Entities.Aggregates.Docentes;

public class Docentes : Auditoria
{
    public int IdDocente {get; set;}
    public required string Dni {get; set;}
    public required string Nombre {get; set;}
    public required string Apellido {get; set;}
    public required string Email {get; set;}
    public bool Habilitado { get; set; }
    public DateTime? FechaBaja { get; set; }



    #region Deshabilitar
    public void Disable(int usuarioId)
    {
        if (!Habilitado)
        {
            return;
        }

        Habilitado = false;
        FechaBaja = DateTime.UtcNow;
        FechaModificacion = DateTime.UtcNow;
        ModificadoPor = usuarioId;
    }
    #endregion

    #region Habilitar
    public void Enable(int usuarioId)
    {
        if (Habilitado)
        {
            return;
        }

        Habilitado = true;
        FechaModificacion = DateTime.UtcNow;
        ModificadoPor = usuarioId;
        FechaBaja = null;
    }
    #endregion
}
