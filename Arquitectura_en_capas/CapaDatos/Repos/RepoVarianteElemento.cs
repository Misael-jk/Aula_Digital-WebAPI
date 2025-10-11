using CapaDatos.Interfaces;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.Repos;

public class RepoVarianteElemento : RepoBase, IRepoVarianteElemento
{
    public RepoVarianteElemento(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    // CRUD/ GESTION
    #region INSERTAR VARIANTE DE ELEMENTO
    public void Insert(VariantesElemento variantesElemento)
    {
        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("unidVarianteElemento", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("unidTipoElemento", variantesElemento.IdTipoElemento);
        parameters.Add("unvariante", variantesElemento.Variante);
        parameters.Add("unidModelo", variantesElemento.IdModelo);

        try
        {
            Conexion.Execute("InsertVarianteElemento", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar una variante de elemento");
        }
    }
    #endregion

    #region UPDATE VARIANTE DE ELEMENTO
    public void Update(VariantesElemento variantesElemento)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidVarianteElemento", variantesElemento.IdVarianteElemento);
        parameters.Add("unidTipoElemento", variantesElemento.IdTipoElemento);
        parameters.Add("unvariante", variantesElemento.Variante);
        parameters.Add("unidModelo", variantesElemento.IdModelo);
        try
        {
            Conexion.Execute("UpdateVarianteElemento", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar una variante de elemento");
        }
    }
    #endregion

    #region OBTENER TODOS
    public IEnumerable<VariantesElemento> GetAll()
    {
        string query = "select * from VariantesElemento";
        return Conexion.Query<VariantesElemento>(query);
    }
    #endregion

    #region OBTENER POR ID
    public VariantesElemento? GetById(int idVariante)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidVariante", idVariante);

        string query = "select * from VariantesElemento where idVariante = @unidVariante";

        try
        {
            return Conexion.QueryFirstOrDefault<VariantesElemento>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener una variante de elemento por id");
        }
    }
    #endregion




    // VALIDACION / CAPA DE NEGOCIO

    #region OBTENER VARIANTES POR MODELO
    public IEnumerable<VariantesElemento> GetByModelo(int idModelo)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidModelo", idModelo);

        string query = "select * from VariantesElemento where idModelo = @unidModelo";
        try
        {
            return Conexion.Query<VariantesElemento>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener las variantes de elemento por modelo");
        }
    }
    #endregion

    #region OBTENER POR TIPO
    public IEnumerable<VariantesElemento> GetByTipo(int idTipoElemento)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidTipoElemento", idTipoElemento);

        string query = "select * from VariantesElemento where idTipoElemento = @unidTipoElemento";
        try
        {
            return Conexion.Query<VariantesElemento>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener las variantes de elemento por tipo de elemento");
        }
    }
    #endregion
}
