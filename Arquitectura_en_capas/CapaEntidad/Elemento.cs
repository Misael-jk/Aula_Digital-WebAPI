namespace CapaEntidad;

public class Elemento
{
    public int IdElemento {get; set;}
    public int IdTipoElemento {get; set;}
    public int IdEstadoMantenimiento { get; set; }
    public int IdUbicacion { get; set; }   
    public int IdModelo { get; set; }
    public required string Equipo { get; set; }
    public required string NumeroSerie {get; set;}
    public required string CodigoBarra {get; set;}
    public required string Patrimonio { get; set; }
    public bool Habilitado {get; set;}
    public DateTime? FechaBaja { get; set; }
}
