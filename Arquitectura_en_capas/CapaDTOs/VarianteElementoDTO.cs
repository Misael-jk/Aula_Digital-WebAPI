namespace CapaDTOs;

public class VarianteElementoDTO
{
    public int IdVarianteElemento { get; set; }
    public required string Equipo { get; set; }
    public required string TipoElemento { get; set; }
    public string? Modelo { get; set; }
}
