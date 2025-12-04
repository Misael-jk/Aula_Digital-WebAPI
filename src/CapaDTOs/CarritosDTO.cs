namespace CapaDTOs;

public class CarritosDTO
{
    public int IdCarrito { get; set; }
    public required string Equipo { get; set; }
    public required string NumeroSerieCarrito { get; set; }
    public int Capacidad { get; set; }
    public required string EstadoMantenimiento { get; set; }
    public required string UbicacionActual { get; set; }
    public required string Modelo { get; set; }
}
