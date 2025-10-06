using CapaDatos.Interfaces;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.Repos;

public class RepoVarianteElemento : RepoBase, IRepoVarianteElemento
{
    public RepoVarianteElemento(IDbConnection conexion) : base(conexion)
    {
    }

    public void Insert(VariantesElemento variantesElemento)
    {
        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("unidVarianteElemento", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("unidTipoElemento", variantesElemento.IdTipoElemento);
        parameters.Add("unvariante", variantesElemento.Variante);
        parameters.Add("unidModelo", variantesElemento.IdModelo);

        try
        {
            Conexion.Execute("InsertVarianteElemento", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar una variante de elemento");
        }
    }

    public void Update(VariantesElemento variantesElemento)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidVarianteElemento", variantesElemento.IdVarianteElemento);
        parameters.Add("unidTipoElemento", variantesElemento.IdTipoElemento);
        parameters.Add("unvariante", variantesElemento.Variante);
        parameters.Add("unidModelo", variantesElemento.IdModelo);
        try
        {
            Conexion.Execute("UpdateVarianteElemento", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar una variante de elemento");
        }
    }

    public IEnumerable<VariantesElemento> GetAll()
    {
        string query = "select * from VariantesElemento";
        return Conexion.Query<VariantesElemento>(query);
    }
}
