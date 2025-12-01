namespace CapaDTOs;

public class ElementoBajasDTO
{
    public int IdElemento { get; set; }
    public required string NumeroSerie { get; set; }
    public required string CodigoBarra { get; set; }
    public required string Patrimonio { get; set; }
    public required string Equipo { get; set; } 
    public required string TipoElemento { get; set; }
    public required string Estado { get; set; } 
    public required string Modelo { get; set; }
    public required string Ubicacion { get; set; }
    public DateTime? FechaBaja { get; set; }
}
