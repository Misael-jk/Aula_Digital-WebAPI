using CapaDatos.Interfaces;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.Repos;

public class RepoNotebooks : RepoBase, IRepoNotebooks
{
    public RepoNotebooks(IDbConnection conexion) : base(conexion)
    {
    }

    #region Insert Notebooks
    public void Insert(Notebooks notebooks)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidNotebook", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("unidModelo", notebooks.IdModelo);
        parameters.Add("unidCarrito", notebooks.IdCarrito);
        parameters.Add("unaposicionCarrito", notebooks.PosicionCarrito);
        parameters.Add("unidEstado", notebooks.IdEstadoMantenimiento);
        parameters.Add("unnumeroSerie", notebooks.NumeroSerie);
        parameters.Add("uncodigoBarra", notebooks.CodigoBarra);
        parameters.Add("unpatrimonio", notebooks.Patrimonio);
        parameters.Add("unhabilitado", notebooks.Habilitado);
        parameters.Add("unfechaBaja", notebooks.FechaBaja);

        try
        {
            Conexion.Execute("InsertNotebook", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar una notebook");
        }
    }
    #endregion

    #region Update Notebook
    public void Update(Notebooks notebooks)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidNotebook", notebooks.IdElemento);
        parameters.Add("unnumeroSerie", notebooks.NumeroSerie);
        parameters.Add("uncodigoBarra", notebooks.CodigoBarra);
        parameters.Add("unidModelo", notebooks.IdModelo);
        parameters.Add("unidCarrito", notebooks.IdCarrito);
        parameters.Add("unaposicionCarrito", notebooks.PosicionCarrito);
        parameters.Add("unidEstado", notebooks.IdEstadoMantenimiento);
        parameters.Add("unpatrimonio", notebooks.Patrimonio);
        parameters.Add("unhabilitado", notebooks.Habilitado);
        parameters.Add("unfechaBaja", notebooks.FechaBaja);
        try
        {
            Conexion.Execute("UpdateNotebook", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar una notebook");
        }
    }
    #endregion

    #region Obtener por ID
    public Notebooks? GetById(int idNotebook)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, n.posicionCarrito, e.idEstadoMantenimiento, e.numeroSerie, e.codigoBarra, e.patrimonio, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.idElemento = @unidNotebook;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidNotebook", idNotebook);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por id");
        }
    }
    #endregion

    public Notebooks? GetByNumeroSerie(string numeroSerie)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, n.posicionCarrito, e.idEstadoMantenimiento, e.numeroSerie, e.codigoBarra, e.patrimonio, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.numeroSerie = @unnumeroSerie;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unnumeroSerie", numeroSerie);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por numero de serie");
        }
    }

    public Notebooks? GetByCodigoBarra(string codigoBarra)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, n.posicionCarrito, e.idEstadoMantenimiento, e.numeroSerie, e.codigoBarra, e.patrimonio, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.codigoBarra = @uncodigoBarra;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("uncodigoBarra", codigoBarra);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por codigo de barra");
        }
    }

    public Notebooks? GetByPatrimonio(string patrimonio)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, n.posicionCarrito, e.idEstadoMantenimiento, e.numeroSerie, e.codigoBarra, e.patrimonio, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.patrimonio = @unpatrimonio;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unpatrimonio", patrimonio);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por patrimonio");
        }
    }

    #region Obtener Todas las Notebooks
    public IEnumerable<Notebooks> GetAll()
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, n.posicionCarrito, e.idEstadoMantenimiento, e.numeroSerie, e.codigoBarra, e.patrimonio, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento);
                         where habilitado = 1;";

        try
        {
            return Conexion.Query<Notebooks>(query);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al traer las notebooks");
        }
    }
    #endregion

    #region Obtener por carrito
    public IEnumerable<Notebooks> GetByCarrito(int idCarrito)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, n.posicionCarrito, e.idEstadoMantenimiento, e.numeroSerie, e.codigoBarra, e.patrimonio, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where n.idCarrito = @unidCarrito;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidCarrito", idCarrito);

        try
        {
            return Conexion.Query<Notebooks>(query, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener las notebooks por carrito");
        }
    }
    #endregion

    public Notebooks? GetNotebookByPosicion(int idCarrito, int posicionCarrito)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, n.posicionCarrito, e.idEstadoMantenimiento, e.numeroSerie, e.codigoBarra, e.patrimonio, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where idCarrito = @idCarrito
                         and posicionCarrito = @posicionCarrito
                         and idEstadoMantenimiento = 1 
                         and habilitado = true
                         limit 1;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("idCarrito", idCarrito);
        parameters.Add("posicionCarrito", posicionCarrito);
        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el elemento en la posicion indicada");
        }
    }
    public bool DuplicatePosition(int idCarrito, int posicionCarrito)
    {
        string query = @"select count(*) 
                         from Notebooks
                         where idCarrito = @idCarrito
                         and posicionCarrito = @posicionCarrito;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("idCarrito", idCarrito);
        parameters.Add("posicionCarrito", posicionCarrito);

        int count = Conexion.ExecuteScalar<int>(query, parameters);
        return count > 0;
    }
    public bool GetDisponible(int idElemento)
    {
        string query = @"select e.idElemento
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.idElemento = @idElemento
                         and e.idEstadoMantenimiento = 1 
                         and e.habilitado = true;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("idElemento", idElemento);

        int resultado = Conexion.ExecuteScalar<int>(query, parameters);

        return resultado > 0;
    }
    public void UpdateEstado(int idElemento, int idEstadoMantenimiento)
    {
        string sql = @"update Elementos
                       set idEstadoMantenimiento = @idEstadoMantenimiento
                       where idElemento = @IdElemento";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@IdElemento", idElemento);
        parameters.Add("@idEstadoMantenimiento", idEstadoMantenimiento);

        Conexion.Execute(sql, parameters);
    }

    public IEnumerable<Notebooks> GetNroSerieByNotebook()
    {
        string query = @"select e.numeroSerie
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where habilitado = 1
                         and idEstadoMantenimiento = 1";

        try
        {
            return Conexion.Query<Notebooks>(query);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al traer los numeros de serie de las notebooks");
        }
    }

    public IEnumerable<Notebooks> GetCodBarraByNotebook()
    {
        string query = @"select e.codigoBarra
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where habilitado = 1
                         and idEstadoMantenimiento = 1";

        try
        {
            return Conexion.Query<Notebooks>(query);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al traer los codigos de barra de las notebooks");
        }
    }

    public IEnumerable<Notebooks> GetNotebookByCarrito(int idCarrito)
    {
        string query = @"select posicionCarrito, idEstadoMantenimiento
                        from elementos e
                        join Notebooks n using (idElemento)
                        where n.idCarrito = @unIdCarrito
                        and (idEstadoMantenimiento = 1
                        or idEstadoMantenimiento = 3);";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@unIdCarrito", idCarrito);

        try
        {
            return Conexion.Query<Notebooks>(query, parameters);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener notebooks del carrito: " + ex.Message);
        }
    }

    public Elemento? GetNotebookBySerieOrCodigo(string numeroSerie, string codigoBarra)
    {
        string query = @"select idEstadoMantenimiento, numeroSerie, codigoBarra
                         from Elementos e
                         LEFT JOIN Notebooks n ON e.idElemento = n.idElemento
                         where (numeroSerie = @numeroSerie or codigoBarra = @codigoBarra)
                         and n.idCarrito is null
                         and idEstadoMantenimiento = 1 
                         and habilitado = 1
                         limit 1;";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("numeroSerie", numeroSerie);
        parameters.Add("codigoBarra", codigoBarra);

        try
        {
            return Conexion.QueryFirstOrDefault<Elemento>(query, parameters);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el elemento con su numero de serie o codigo de barra");
        }
    }
}
