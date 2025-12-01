namespace CapaDTOs;

public class DocentesBajasDTO
{
    public int IdDocente { get; set; }
    public required string Dni { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Email { get; set; }
    public DateTime? FechaBaja { get; set; }
}
