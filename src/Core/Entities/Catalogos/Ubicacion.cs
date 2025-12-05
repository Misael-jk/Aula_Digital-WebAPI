using Core.Common;

namespace Core.Entities.Catalogos;

public class Ubicacion : Auditoria
{
    public int IdUbicacion { get; set; }
    public required string NombreUbicacion { get; set; }
}
