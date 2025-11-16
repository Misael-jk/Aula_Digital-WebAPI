using CapaDatos.Interfaces;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.Repos;

public class RepoNotebooks : RepoBase, IRepoNotebooks
{
    public RepoNotebooks(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    // CRUD / GESTION

    #region Insert Notebooks
    public void Insert(Notebooks notebooks)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidNotebook", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("unidTipoElemento", notebooks.IdTipoElemento);
        parameters.Add("unidVariante", notebooks.IdVarianteElemento);
        parameters.Add("unidModelo", notebooks.IdModelo);
        parameters.Add("unidUbicacion", notebooks.IdUbicacion);
        parameters.Add("unidEstadoMantenimiento", notebooks.IdEstadoMantenimiento);
        parameters.Add("unnumeroSerie", notebooks.NumeroSerie);
        parameters.Add("uncodigoBarra", notebooks.CodigoBarra);
        parameters.Add("unpatrimonio", notebooks.Patrimonio);
        parameters.Add("unhabilitado", notebooks.Habilitado);
        parameters.Add("unafechaBaja", notebooks.FechaBaja);
        parameters.Add("unequipo", notebooks.Equipo);
        parameters.Add("unidCarrito", notebooks.IdCarrito);
        parameters.Add("unaposicionCarrito", notebooks.PosicionCarrito);

        try
        {
            Conexion.Execute("InsertNotebook", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);
            notebooks.IdElemento = parameters.Get<int>("unidNotebook");
        }
        catch (Exception ex)
        {
            throw new Exception("Hubo un error al insertar una notebook", ex);
        }
    }
    #endregion

    #region Update Notebook
    public void Update(Notebooks notebooks)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidNotebook", notebooks.IdElemento);
        parameters.Add("unidTipoElemento", notebooks.IdTipoElemento);
        parameters.Add("unidVariante", notebooks.IdVarianteElemento);
        parameters.Add("unidModelo", notebooks.IdModelo);
        parameters.Add("unidUbicacion", notebooks.IdUbicacion);
        parameters.Add("unidEstadoMantenimiento", notebooks.IdEstadoMantenimiento);
        parameters.Add("unnumeroSerie", notebooks.NumeroSerie);
        parameters.Add("uncodigoBarra", notebooks.CodigoBarra);
        parameters.Add("unpatrimonio", notebooks.Patrimonio);
        parameters.Add("unhabilitado", notebooks.Habilitado);
        parameters.Add("unafechaBaja", notebooks.FechaBaja);
        parameters.Add("unequipo", notebooks.Equipo);
        parameters.Add("unidCarrito", notebooks.IdCarrito);
        parameters.Add("unaposicionCarrito", notebooks.PosicionCarrito);
        try
        {
            Conexion.Execute("UpdateNotebook", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new Exception("Hubo un error al actualizar una notebook", ex);
        }
    }
    #endregion

    #region Obtener por ID
    public Notebooks? GetById(int idNotebook)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, e.idTipoElemento, n.posicionCarrito, e.idEstadoMantenimiento, e.idUbicacion, e.numeroSerie, e.codigoBarra, e.patrimonio, n.equipo, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.idElemento = @unidNotebook;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidNotebook", idNotebook);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por id");
        }
    }
    #endregion

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


    // VALIDACIONES

    #region OBTENER POR NUMERO DE SERIE
    public Notebooks? GetByNumeroSerie(string numeroSerie)
    {

        string query = @"select e.idElemento, e.idModelo, n.idCarrito, e.idTipoElemento, n.posicionCarrito, e.idEstadoMantenimiento, e.idUbicacion, e.numeroSerie, e.codigoBarra, e.patrimonio, n.equipo, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.numeroSerie = @unnumeroSerie;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unnumeroSerie", numeroSerie);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por numero de serie");
        }
    }
    #endregion

    #region OBTENER POR CODIGO DE BARRAS
    public Notebooks? GetByCodigoBarra(string codigoBarra)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, e.idTipoElemento, n.posicionCarrito, e.idEstadoMantenimiento, e.idUbicacion, e.numeroSerie, e.codigoBarra, e.patrimonio, n.equipo, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.codigoBarra = @uncodigoBarra;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("uncodigoBarra", codigoBarra);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por codigo de barra");
        }
    }
    #endregion

    #region OBTENER POR PATRIMONIO
    public Notebooks? GetByPatrimonio(string patrimonio)
    {
        string query = @"select e.idElemento, e.idModelo, n.idCarrito, e.idTipoElemento, n.posicionCarrito, e.idEstadoMantenimiento, e.idUbicacion, e.numeroSerie, e.codigoBarra, e.patrimonio, n.equipo, e.habilitado, e.fechaBaja
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where e.patrimonio = @unpatrimonio;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unpatrimonio", patrimonio);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por patrimonio");
        }
    }
    #endregion

    #region OBTENER POR EQUIPO 
    public Notebooks? GetByEquipo(string equipo)
    {
        string query = @"select *
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where n.equipo = @unequipo;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unequipo", equipo);

        try
        {
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la notebook por equipo");
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
            return Conexion.Query<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener las notebooks por carrito");
        }
    }
    #endregion

    #region OBTENER NOTEBOOK POR SU POSICION
    public Notebooks? GetNotebookByPosicion(int? idCarrito, int posicionCarrito)
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
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el elemento en la posicion indicada");
        }
    }
    #endregion

    #region VERIFICAR POSICION | true: si existe, false: no existe
    public bool DuplicatePosition(int idCarrito, int posicionCarrito)
    {
        string query = @"select count(*) 
                         from Notebooks
                         where idCarrito = @idCarrito
                         and posicionCarrito = @posicionCarrito;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("idCarrito", idCarrito);
        parameters.Add("posicionCarrito", posicionCarrito);

        int count = Conexion.ExecuteScalar<int>(query, parameters, transaction: Transaction);
        return count > 0;
    }
    #endregion

    #region VERIFICAR DISPONIBILIDAD | true: disponible, false: no disponible
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

        int resultado = Conexion.ExecuteScalar<int>(query, parameters, transaction: Transaction);

        return resultado > 0;
    }
    #endregion

    public void UpdateEstado(int idElemento, int idEstadoMantenimiento)
    {
        string sql = @"update Elementos
                       set idEstadoMantenimiento = @idEstadoMantenimiento
                       where idElemento = @IdElemento";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@IdElemento", idElemento);
        parameters.Add("@idEstadoMantenimiento", idEstadoMantenimiento);

        Conexion.Execute(sql, parameters, transaction: Transaction);
    }

    #region OBTENER LOS NROS DE SERIE DE TODAS LAS NOTEBOOKS
    public IEnumerable<Notebooks> GetNroSerieByNotebook()
    {
        string query = @"select e.numeroSerie
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where habilitado = 1
                         and idEstadoMantenimiento = 1";

        try
        {
            return Conexion.Query<Notebooks>(query, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al traer los numeros de serie de las notebooks");
        }
    }
    #endregion

    #region OBTENER TODOS LOS CODIGOS DE BARRA DE LAS NOTEBOOKS
    public IEnumerable<Notebooks> GetCodBarraByNotebook()
    {
        string query = @"select e.codigoBarra
                         from Elementos e
                         join Notebooks n using (idElemento)
                         where habilitado = 1
                         and idEstadoMantenimiento = 1";

        try
        {
            return Conexion.Query<Notebooks>(query, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al traer los codigos de barra de las notebooks");
        }
    }
    #endregion

    #region OBTENER TODAS LAS NOTEBOOKS DE UN CARRITO
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
            return Conexion.Query<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener notebooks del carrito: " + ex.Message);
        }
    }
    #endregion

    #region OBTENER NOTEBOOK POR CODIGO O SERIE
    public Notebooks? GetNotebookBySerieOrCodigo(string numeroSerie, string codigoBarra)
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
            return Conexion.QueryFirstOrDefault<Notebooks>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("No se encontro el elemento con su numero de serie o codigo de barra");
        }
    }
    #endregion

    #region OBTENER CARRITO POR NOTEBOOK
    public Carritos? GetCarritoByNotebook(int idNotebook)
    {
        string query = @"select c.idCarrito, c.equipo as 'EquipoCarrito', c.capacidad as 'Capacidad', c.idModelo, c.numeroSerieCarrito, c.idEstadoMantenimiento, c.idUbicacion, c.habilitado, c.fechaBaja
                         from Carritos c
                         join Notebooks n on c.idCarrito = n.idCarrito
                         where n.idElemento = @idNotebook;";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("idNotebook", idNotebook);
        try
        {
            return Conexion.QueryFirstOrDefault<Carritos>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener el carrito por notebook");
        }
    }
    #endregion

    #region ESTADISTICAS DE CANTIDAD DE NOTEBOOKS POR MODELO
    public List<(string Modelo, int Cantidad)> GetCantidadPorModelo()
    {
        string query = @"
        SELECT m.modelo AS Modelo, COUNT(*) AS Cantidad
        FROM Notebooks n
        JOIN Elementos e ON n.idElemento = e.idElemento
        JOIN Modelo m ON e.idModelo = m.idModelo
        WHERE e.habilitado = 1
        GROUP BY m.modelo
        ORDER BY Cantidad DESC;";

        try
        {
            var resultado = Conexion.Query<(string Modelo, int Cantidad)>(query, transaction: Transaction);
            return resultado.ToList();
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la cantidad de notebooks por modelo");
        }
    }
    #endregion
    
    #region ESTADISTICAS DE CANTIDAD DE NOTEBOOKS POR ESTADO
    public List<(string Estado, int Cantidad)> GetCantidadEstado()
    {
        string query = @"
        SELECT em.estadoMantenimiento AS Estado, COUNT(*) AS Cantidad
        FROM Notebooks n
        JOIN Elementos e ON n.idElemento = e.idElemento
        JOIN estadosmantenimiento em ON e.idEstadoMantenimiento = em.idEstadoMantenimiento
        GROUP BY em.estadoMantenimiento 
        ORDER BY Cantidad DESC;";

        try
        {
            var resultado = Conexion.Query<(string Estado, int Cantidad)>(query, transaction: Transaction);
            return resultado.ToList();
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la cantidad de notebooks por estado");
        }
    }
    #endregion

    #region ESTADISTICAS DE CANTIDAD DE NOTEBOOKS EN CARRITOS
    public List<(string Equipo, int Cantidad)> GetCantidadNotebooksEnCarritos()
    {
        string query = @"
        SELECT c.equipo AS Equipo, COUNT(n.idElemento) AS Cantidad
        FROM Carritos c
        LEFT JOIN Notebooks n ON c.IdCarrito = n.IdCarrito
        GROUP BY c.idCarrito, c.equipo
        order by c.equipo;";

        try
        {
            var resultado = Conexion.Query<(string Equipo, int Cantidad)>(query, transaction: Transaction);
            return resultado.ToList();
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la cantidad de notebooks en carritos");
        }
    }
    #endregion

    #region OBTENER IDs DE NOTEBOOKS POR EL ID DEL CARRITO
    public IEnumerable<int> GetIdNotebooksByCarrito(int idCarrito)
    {
        string query = @"select e.idElemento
                        from elementos e
                        join Notebooks n using (idElemento)
                        where n.idCarrito = @unIdCarrito
                        Order by posicionCarrito ASC;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@unIdCarrito", idCarrito);

        try
        {
            return Conexion.Query<int>(query, parameters, transaction: Transaction);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener IDs de notebooks del carrito: " + ex.Message);
        }
    }
    #endregion
}