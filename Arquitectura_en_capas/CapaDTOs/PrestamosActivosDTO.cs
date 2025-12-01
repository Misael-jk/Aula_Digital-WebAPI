namespace CapaDTOs;

public class PrestamosActivosDTO
{
    public int IdPrestamo { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set;}
    public required string Carrito { get; set; }
    public DateTime Fecha { get; set; }
    public int Prestadas { get; set; }
    public int Devueltas { get; set; }
    public required string Estado { get; set; }
}
