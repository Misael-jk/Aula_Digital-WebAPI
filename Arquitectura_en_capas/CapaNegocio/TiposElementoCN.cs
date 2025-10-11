using CapaDatos.Interfaces;
using CapaEntidad;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CapaNegocio;

public class TiposElementoCN
{
    private readonly IRepoTipoElemento _repoTipoElemento;

    public TiposElementoCN(IRepoTipoElemento repoTipoElemento)
    {
        _repoTipoElemento = repoTipoElemento;
    }

    #region CREATE TIPO ELEMENTO
    public void InsertarTipoElemento(TipoElemento tipoElemento)
    {
        if (string.IsNullOrWhiteSpace(tipoElemento.ElementoTipo))
        {
            throw new Exception("El tipo de elemento no puede estar vacío o nulo");
        }

        if (tipoElemento.ElementoTipo.Length >= 40)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(tipoElemento.ElementoTipo, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        if (_repoTipoElemento.GetByTipo(tipoElemento.ElementoTipo) != null)
        {
            throw new Exception("Ya existe un tipo de elemento con ese nombre, por favor elija uno nuevo");
        }

        _repoTipoElemento.Insert(tipoElemento);
    }
    #endregion

    #region READ TIPO ELEMENTO
    public IEnumerable<TipoElemento> GetAllTipo()
    {
        return _repoTipoElemento.GetAll();
    }

    public TipoElemento? GetById(int idTipoElemento)
    {
        return _repoTipoElemento.GetById(idTipoElemento);
    }
    #endregion

    #region UPDATE TIPO ELEMENTO
    public void ActualizarTipoElemento(TipoElemento tipoElemento)
    {
        if (string.IsNullOrWhiteSpace(tipoElemento.ElementoTipo))
        {
            throw new Exception("El tipo de elemento no puede estar vacío o nulo");
        }

        if (tipoElemento.ElementoTipo.Length >= 40)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(tipoElemento.ElementoTipo, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        TipoElemento? tipoElementoOLD = _repoTipoElemento.GetById(tipoElemento.IdTipoElemento);

        if (tipoElementoOLD == null)
        {
            throw new Exception("El tipo de elemento no existe");
        }

        if (!tipoElementoOLD.ElementoTipo.Equals(tipoElemento.ElementoTipo, StringComparison.OrdinalIgnoreCase))
        {
            TipoElemento? existente = _repoTipoElemento.GetByTipo(tipoElemento.ElementoTipo);

            if (existente != null)
            {
                throw new Exception("Ya existe otro tipo de elemento con el mismo nombre.");
            }
        }

        _repoTipoElemento.Update(tipoElemento);
    }
    #endregion

    #region DELETE TIPO ELEMENTO
    public void EliminarTipoElemento(int idTipoElemento)
    {
        TipoElemento? tipoElemento = _repoTipoElemento.GetById(idTipoElemento);

        if (tipoElemento == null)
        {
            throw new Exception("El tipo de elemento no existe");
        }

        _repoTipoElemento.Delete(idTipoElemento);
    }
    #endregion

}
