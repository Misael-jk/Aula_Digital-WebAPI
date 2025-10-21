using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoVarianteElemento
{
    public void Insert(VariantesElemento variante);
    public void Update(VariantesElemento variante);
    public IEnumerable<VariantesElemento> GetAll();
    public VariantesElemento? GetById(int idVarianteElemento);
    public IEnumerable<VariantesElemento> GetByModelo(int idModelo);
    public IEnumerable<VariantesElemento> GetByTipo(int idTipoElemento);
    public void SetTransaction(IDbTransaction? transaction);
}
