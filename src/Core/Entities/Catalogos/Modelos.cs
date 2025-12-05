using Core.Common;

namespace Core.Entities.Catalogos;

public class Modelos : Auditoria
{
    public int IdModelo { get; set; }
    public int IdTipoElemento { get; set; }
    public required string NombreModelo { get; set; }
}
