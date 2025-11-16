namespace CapaDTOs;

public class TransaccionDTO
{
    //Parte del prestamo
    public int IdPrestamo { get; set; }
    public required string ApellidoDocentes { get; set; }
    public string? NombreCurso { get; set; }
    public string? EquipoCarrito { get; set; }
    public required string EstadoPrestamo { get; set; }
    public DateTime FechaPrestamo { get; set; }
    //Parte de devolucion
    public int? IdDevolucion { get; set; }
    public string? ApellidoEncargado { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public string? Observaciones { get; set; }
}
