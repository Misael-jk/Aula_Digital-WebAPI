using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoElemento : RepoBase, IRepoElemento
{
    public RepoElemento(IDbConnection conexion, IDbTransaction? transaction = null)
   : base(conexion, transaction)
    {
    }

    #region Alta Elemento 
    public void Insert(Elemento elemento)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidElemento", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("unidTipoElemento", elemento.IdTipoElemento);
        parametros.Add("unidVariante", elemento.IdVarianteElemento);
        parametros.Add("unidEstadoMantenimiento", elemento.IdEstadoMantenimiento);
        parametros.Add("unidUbicacion", elemento.IdUbicacion);
        parametros.Add("unidModelo", elemento.IdModelo);
        parametros.Add("unnumeroSerie", elemento.NumeroSerie);
        parametros.Add("uncodigoBarra", elemento.CodigoBarra);
        parametros.Add("unpatrimonio", elemento.Patrimonio);
        parametros.Add("unhabilitado", elemento.Habilitado);
        parametros.Add("unafechaBaja", elemento.FechaBaja);

        try
        {
            Conexion.Execute("InsertElemento", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);
            elemento.IdElemento = parametros.Get<int>("unidElemento");
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar un elemento");
        }
    }
    #endregion

    #region Actualizar Elemento
    public void Update(Elemento elemento)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidElemento", elemento.IdElemento);
        parametros.Add("unidTipoElemento", elemento.IdTipoElemento);
        parametros.Add("unidVariante", elemento.IdVarianteElemento);
        parametros.Add("unidEstadoMantenimiento", elemento.IdEstadoMantenimiento);
        parametros.Add("unidubicacion", elemento.IdUbicacion);
        parametros.Add("unidmodelo", elemento.IdModelo);
        parametros.Add("unnumeroSerie", elemento.NumeroSerie);
        parametros.Add("uncodigoBarra", elemento.CodigoBarra);
        parametros.Add("unPatrimonio", elemento.Patrimonio);
        parametros.Add("unhabilitado", elemento.Habilitado);
        parametros.Add("unafechaBaja", elemento.FechaBaja);

        try
        {
            Conexion.Execute("UpdateElemento", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar un Elemento");
        }
    }
    #endregion

    #region Eliminar Elemento
    public void Delete(int idElemento)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidElemento", idElemento);

        try
        {
            Conexion.Execute("DeleteElemento", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al eliminar un elemento");
        }
    }
    #endregion

    #region Obtener por numero de serie
    public Elemento? GetByNumeroSerie(string numeroSerieElemento)
    {
        string query = "select * from Elementos where numeroSerie = @numeroSerieElemento";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("@numeroSerieElemento", numeroSerieElemento);
            return Conexion.QueryFirstOrDefault<Elemento>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el elemento con su numero de serie");
        }
    }
    #endregion

    #region Obtener por codigo de Barra
    public Elemento? GetByCodigoBarra(string codigoBarra)
    {
        string query = "select * from Elementos where codigoBarra = @codigoBarra";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("codigoBarra", codigoBarra);
            return Conexion.QueryFirstOrDefault<Elemento>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el elemento con su codigo de barra");
        }
    }
    #endregion

    #region Obtener por Id elemento
    public Elemento? GetById(int idElemento)
    {
        string query = "select idElemento, idTipoElemento, idVariante AS 'idVarianteElemento', idEstadoMantenimiento, idUbicacion, idModelo, numeroSerie, codigoBarra, patrimonio, habilitado, fechaBaja from Elementos where idElemento = @idElemento";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("@idElemento", idElemento);
            return Conexion.QueryFirstOrDefault<Elemento>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el id del elemento");
        }
    }
    #endregion

    #region Obtener Elementos Disponibles
    public bool GetDisponible(int idElemento)
    {

        string sql = "select * from Elementos where IdElemento = @IdElemento and IdEstadoMantenimiento = 1 and habilitado = true limit 1;";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("IdElemento", idElemento);

        int disponible = Conexion.ExecuteScalar<int>(sql, parameters, transaction: Transaction);

        return disponible > 0;
    }
    #endregion

    #region Cambiar Estado de Elemento
    public void UpdateEstado(int idElemento, int idEstadoElemento)
    {
        string sql = @"update Elementos
                       set idEstadoElemento = @idEstadoElemento
                       where idElemento = @IdElemento";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("@IdElemento", idElemento);
        parameters.Add("@IdEstadoElemento", idEstadoElemento);

        Conexion.Execute(sql, parameters, transaction: Transaction);
    }
    #endregion

    #region OBTENER NOTEBOOK POR NRO SERIE O COD BARRA
    public Elemento? GetNotebookBySerieOrCodigo(string numeroSerie, string codigoBarra)
    {
        string query = @"select idEstadoMantenimiento, numeroSerie, codigoBarra
                         from Elementos
                         where (numeroSerie = @numeroSerie or codigoBarra = @codigoBarra)
                         and idCarrito is null
                         and idEstadoMantenimiento = 1 
                         and disponible = 1
                         limit 1;";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("numeroSerie", numeroSerie);
        parameters.Add("codigoBarra", codigoBarra);

        try
        {
            return Conexion.QueryFirstOrDefault<Elemento>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el elemento con su numero de serie o codigo de barra");
        }
    }
    #endregion

    #region OBTENER PATRIMONIO
    public Elemento? GetByPatrimonio(string patrimonio)
    {
        string query = "select * from Elementos where patrimonio = @unpatrimonio";
        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("unpatrimonio", patrimonio);
            return Conexion.QueryFirstOrDefault<Elemento>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el elemento con su patrimonio");
        }
    }
    #endregion

}
