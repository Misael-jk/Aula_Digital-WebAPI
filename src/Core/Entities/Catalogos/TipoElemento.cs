using Core.Common;

namespace Core.Entities.Catalogos;

public class TipoElemento : Auditoria
{
    public int IdTipoElemento {get; set;}
    public required string ElementoTipo {get; set;}
}
