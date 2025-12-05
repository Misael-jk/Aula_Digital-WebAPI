using Core.Common;

namespace Core.Entities.Aggregates.Elementos;

public class Elemento : Auditoria
{
    public int IdElemento { get; set; }
    public int IdTipoElemento { get; set; }
    public int? IdVarianteElemento { get; set; }
    public int IdEstadoMantenimiento { get; set; }
    public int IdUbicacion { get; set; }
    public int IdModelo { get; set; }
    public required string NumeroSerie { get; set; }
    public required string CodigoBarra { get; set; }
    public required string Patrimonio { get; set; }
    public bool Habilitado { get; set; }
    public DateTime? FechaBaja { get; set; }


    #region Deshabilitar
    public void Disable()
    {
        if (!Habilitado)
        {
            return;
        }
        Habilitado = false;
        FechaBaja = DateTime.UtcNow;
    }
    #endregion

    #region Habilitar
    public void Enable()
    {
        if (Habilitado)
        {
            return;
        }

        Habilitado = true;
        FechaBaja = null;
    }
    #endregion
}
