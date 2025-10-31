using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using System.Text.RegularExpressions;

namespace CapaNegocio;

public class VarianteElementoCN
{
    private readonly IRepoVarianteElemento repoVarianteElemento;
    private readonly IRepoTipoElemento repoTipoElemento;
    private readonly IRepoModelo repoModelo;
    private readonly IMapperVarianteElemento mapperVarianteElemento;

    public VarianteElementoCN(IRepoVarianteElemento repoVarianteElemento, IRepoTipoElemento repoTipoElemento, IRepoModelo repoModelo, IMapperVarianteElemento mapperVarianteElemento)
    {
        this.repoVarianteElemento = repoVarianteElemento;
        this.repoTipoElemento = repoTipoElemento;
        this.repoModelo = repoModelo;
        this.mapperVarianteElemento = mapperVarianteElemento;
    }

    public IEnumerable<VarianteElementoDTO> MostrarDatos()
    {
        return mapperVarianteElemento.GetAllDTO();
    }

    #region INSERTAR VARIANTE ELEMENTO
    public void Insert(VariantesElemento variantesElemento)
    {
        ValidarDatos(variantesElemento);

        VariantesElemento? variantes = repoVarianteElemento.GetById(variantesElemento.IdVarianteElemento);

        if(variantes != null)
        {
            throw new Exception("Ya existe una variante con ese Id");
        }

        if (repoTipoElemento.GetById(variantesElemento.IdTipoElemento) == null)
        {
            throw new Exception("Por favor seleccione un tipo elemento para la variante de los elementos");
        }

        Modelos? modelo = repoModelo.GetById(variantesElemento.IdModelo);

        if(variantesElemento.IdTipoElemento != modelo?.IdTipoElemento)
        {
            throw new Exception("No coinciden el tipo Elemento de modelo y la variante");
        }

        repoVarianteElemento.Insert(variantesElemento);
    }
    #endregion

    #region ACTUALIZAR VARIANTE ELEMENTO
    public void Actualizar(VariantesElemento variantes)
    {
        ValidarDatos(variantes);

        VariantesElemento? variantesElemento = repoVarianteElemento.GetById(variantes.IdVarianteElemento);

        if (variantesElemento == null)
        {
            throw new Exception("No existe la variante que desea actualizar");
        }

        if (repoTipoElemento.GetById(variantes.IdTipoElemento) == null)
        {
            throw new Exception("Por favor seleccione un tipo elemento para la variante de los elementos");
        }

        Modelos? modelo = repoModelo.GetById(variantes.IdModelo);

        if (variantes.IdTipoElemento != modelo?.IdTipoElemento)
        {
            throw new Exception("No coinciden el tipo Elemento de modelo y la variante");
        }

        repoVarianteElemento.Update(variantes);
    }
    #endregion


    #region Filtros
    public VariantesElemento? GetById(int idVariante)
    {
        return repoVarianteElemento.GetById(idVariante);
    }
    #endregion

    #region Validaciones
    private void ValidarDatos(VariantesElemento variantesElemento)
    {
        if(string.IsNullOrEmpty(variantesElemento.Variante))
        {
            throw new Exception("El nombre del subtipo de los elementos no puede estar vacia");
        }

        if(variantesElemento.Variante.Length > 40)
        {
            throw new Exception("La nombre del subtipo de los elementos no puede superar los 40 caracteres");
        }

        if (!Regex.IsMatch(variantesElemento.Variante, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s\-]+$"))
        {
            throw new Exception("El nombre del subtipo de los elementos contiene caracteres invalidas");
        }
    }
    #endregion
}
