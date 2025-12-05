using Core.Common;

namespace Core.Entities.Catalogos;

public class VariantesElemento : Auditoria
{
    public int IdVarianteElemento { get; set; }
    public int IdTipoElemento { get; set; }
    public required string Variante { get; set; }
    public int IdModelo { get; set; }
}
