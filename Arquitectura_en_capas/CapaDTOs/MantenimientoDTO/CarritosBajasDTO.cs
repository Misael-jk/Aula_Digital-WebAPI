namespace CapaDTOs;

public class CarritosBajasDTO
{
    public int IdCarrito { get; set; }
    public  required string Equipo { get; set; }
    public required string NumeroSerieCarrito { get; set; }
    public required string EstadoMantenimiento { get; set; }
    public required string Ubicacion { get; set; }
    public required string Modelo { get; set; }
    public bool Habilitado { get; set; }
    public DateTime? FechaBaja { get; set; }
}
