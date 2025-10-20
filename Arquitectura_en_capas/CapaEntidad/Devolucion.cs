namespace CapaEntidad;

public class Devolucion
{
    public int IdDevolucion {get; set;}
    public int IdPrestamo {get; set;}
    public int IdUsuario {get; set;}
    public DateTime FechaDevolucion {get; set;}
    public string? Observaciones {get; set;}
}
