namespace CapaDTOs;

public class DevolucionesDTO
{
    public int? IdDevolucion { get; set; }
    public string? ApellidoDocente { get; set; }
    public string? ApellidoEncargado { get; set; }
    public DateTime? FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public string? EstadoPrestamo { get; set; }
    public string? Observaciones { get; set; }
}

