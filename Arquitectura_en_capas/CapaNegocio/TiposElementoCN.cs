using CapaDatos.Interfaces;
using CapaEntidad;

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
    #endregion

    #region UPDATE TIPO ELEMENTO
    public void ActualizarTipoElemento(TipoElemento tipoElementoNEW)
    {
        if (string.IsNullOrWhiteSpace(tipoElementoNEW.ElementoTipo))
        {
            throw new Exception("El tipo de elemento no puede estar vacío o nulo");
        }

        TipoElemento? tipoElementoOLD = _repoTipoElemento.GetById(tipoElementoNEW.IdTipoElemento);

        if (tipoElementoOLD == null)
        {
            throw new Exception("El tipo de elemento no existe");
        }

        if (tipoElementoOLD.ElementoTipo != tipoElementoNEW.ElementoTipo && _repoTipoElemento.GetByTipo(tipoElementoNEW.ElementoTipo) != null)
        {
            throw new Exception("Ya existe otro tipo de elemento con el mismo nombre");
        }

        _repoTipoElemento.Update(tipoElementoNEW);
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
