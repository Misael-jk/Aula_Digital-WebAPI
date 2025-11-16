using CapaDatos.Interfaces;
using CapaEntidad;
using Dapper;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CapaDatos.Repos;

public class RepoDocentes : RepoBase, IRepoDocentes
{
    public RepoDocentes(IDbConnection conexion, IDbTransaction? transaction = null)
   : base(conexion, transaction)
    {
    }

    #region Alta Docente
    public void Insert(Docentes docentes)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidDocente", dbType: DbType.Int32, direction: ParameterDirection.Output);

        parametros.Add("undni", docentes.Dni);
        parametros.Add("unnombre", docentes.Nombre);
        parametros.Add("unapellido", docentes.Apellido);
        parametros.Add("unemail", docentes.Email);
        parametros.Add("unhabilitado", docentes.Habilitado);
        parametros.Add("unafechabaja", docentes.FechaBaja);

        try
        {
            Conexion.Execute("InsertDocente", parametros, commandType: CommandType.StoredProcedure);
            docentes.IdDocente = parametros.Get<int>("unidDocente");
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar un docente");
        }
    }
    #endregion

    #region Actualizar Docente
    public void Update(Docentes docentes)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidDocente", docentes.IdDocente);
        parametros.Add("undni", docentes.Dni);
        parametros.Add("unnombre", docentes.Nombre);
        parametros.Add("unapellido", docentes.Apellido);
        parametros.Add("unemail", docentes.Email);
        parametros.Add("unhabilitado", docentes.Habilitado);
        parametros.Add("unafechaBaja", docentes.FechaBaja);

        try
        {
            Conexion.Execute("UpdateDocente", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar un docente");
        }
    }
    #endregion

    #region Deshabilitar Docente
    public void Deshabilitar(int idDocente, bool habilitado)
    {
        string query = "update Docentes set habilitado = @unhabilitado, fechaBaja = @unafechaBaja where idDocente = @unidDocente";

        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidDocente", idDocente);
        parametros.Add("unhabilitado", habilitado);
        parametros.Add("unafechaBaja", !habilitado ? DateTime.Now : null);

        try
        {
            Conexion.Execute(query, parametros);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al deshabilitar un docente");
        }
    }
    #endregion 

    #region Eliminar Docente
    public void Delete(int idDocente)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidDocente", idDocente);

        try
        {
            Conexion.Execute("DeleteDocente", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al eliminar un docente");
        }
    }
    #endregion

    #region Obtener por Id
    public Docentes? GetById(int idDocente)
    {
        string query = "select * from Docentes where idDocente = @unidDocente";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("unidDocente", idDocente);
            return Conexion.QueryFirstOrDefault<Docentes>(query, parametros);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el id del docente");
        }
    }
    #endregion

    #region Obtener por Dni
    public Docentes? GetByDni(string Dni)
    {
        string query = "select * from Docentes where dni = @dni";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("@dni", Dni);
            return Conexion.QueryFirstOrDefault<Docentes>(query, parametros);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el dni del docente");
        }
    }
    #endregion

    #region Obteber por Email
    public Docentes? GetByEmail(string Email)
    {
        string query = "select * from Docentes where email = @email";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("@email", Email);
            return Conexion.QueryFirstOrDefault<Docentes>(query, parametros);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el email del docente");
        }
    }
    #endregion

    #region Obtener por Nombre
    public Docentes? GetByNombre(string nombre)
    {
        string query = "select * from Docentes where nombre = @nombre";
        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("@nombre", nombre);
            return Conexion.QueryFirstOrDefault<Docentes>(query, parametros);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el nombre del docente");
        }
    }
    #endregion

    #region Obtener Todos
    public IEnumerable<Docentes> GetAll()
    {
        string query = "select * from Docentes";
        try
        {
            return Conexion.Query<Docentes>(query);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener todos los docentes");
        }
    }
    #endregion

    #region Esta en un prestamo
    public bool ExistsPrestamo(int idDocente)
    {
        string query = @"
        SELECT EXISTS(
            SELECT 1
            FROM Prestamos p
            JOIN EstadosPrestamo e ON e.idEstadoPrestamo = p.idEstadoPrestamo
            WHERE p.idDocente = @unidDocente
              AND e.estadoPrestamo IN ('En Prestamo', 'En Parcial')
        )";

        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("unidDocente", idDocente);
            int count = Conexion.ExecuteScalar<int>(query, parametros);
            return count == 1;
        }
        catch (Exception)
        {
            throw new Exception("Error al verificar si el docente tiene prestamos activos");
        }
    }
    #endregion

    #region Filtrar por nombre
    public IEnumerable<string> GetFiltroNombre(string nombre, int limit)
    {
        string query = @"
        SELECT *
        from docentes d 
        where d.nombre LIKE CONCAT(@filtroNombre, '%')
        LIMIT @limite";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("filtroNombre", nombre);
            parametros.Add("limite", limit);
            return Conexion.Query<string>(query, parametros);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los nombres de los docentes");
        }
    }
    #endregion

    #region Filtrar por DNI
    public Docentes? FiltroGetDocenteByID(string dni)
    {
        string query = @"SELECT *
                         FROM Docentes
                         WHERE (@dni IS NULL OR @dni = '' OR dni = @dni)
                         ";
        DynamicParameters parametros = new DynamicParameters();
        parametros.Add("dni", dni);

        try
        {
            return Conexion.QueryFirstOrDefault<Docentes>(query, parametros);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al buscar el docente por DNI" + ex);
        }
    }
    #endregion
}
