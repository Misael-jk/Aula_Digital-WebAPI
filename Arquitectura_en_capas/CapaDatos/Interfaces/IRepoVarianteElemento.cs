using CapaEntidad;

namespace CapaDatos.Interfaces;

public interface IRepoVarianteElemento
{
    public void Insert(VariantesElemento variante);
    public void Update(VariantesElemento variante);
    public IEnumerable<VariantesElemento> GetAll();
}
