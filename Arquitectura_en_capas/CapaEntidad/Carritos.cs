namespace CapaEntidad;

public class Carritos
{
    public int IdCarrito {get; set;}
    public required string EquipoCarrito { get; set; }
    public required string NumeroSerieCarrito { get; set; }
    public int IdEstadoMantenimiento { get; set; }
    public int IdUbicacion { get; set; }
    public int IdModelo { get; set; }
    public bool Habilitado { get; set; }
    public DateTime? FechaBaja { get; set; }
}
