namespace CapaEntidad;

public class Prestamos
{
    public int IdPrestamo {get; set;}
    public int? IdCurso {get; set;}
    public int IdUsuario {get; set;}
    public int IdDocente {get; set;}
    public int IdEstadoPrestamo { get; set; }
    public int? IdCarrito {get; set;} 
    public DateTime FechaPrestamo {get; set;}
}
