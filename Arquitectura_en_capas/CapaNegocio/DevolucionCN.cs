using CapaDatos.Repos;
using CapaNegocio;
using CapaEntidad;
using System.Data;
using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using System.Transactions;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using CapaDatos.InterfaceUoW;

namespace CapaNegocio;

public class DevolucionCN
{
    private readonly IUowDevolucion uow;
    private readonly IMapperDevoluciones mapperDevolucion;

    public DevolucionCN(IMapperDevoluciones mapperDevolucion, IUowDevolucion uow)
    {
        this.mapperDevolucion = mapperDevolucion;
        this.uow = uow;
    }

    public IEnumerable<DevolucionesDTO> ObtenerElementos()
    {
        return mapperDevolucion.GetAllDTO();
    }

    #region INSERT DEVOLUCION
    public void CrearDevolucion(Devolucion devolucionNEW, IEnumerable<int> idsElementos, IEnumerable<int> idsEstadosElemento, IEnumerable<string>? Observaciones)
    {
        using (TransactionScope scope = new TransactionScope())
        {

            Prestamos? prestamo = uow.RepoPrestamos.GetById(devolucionNEW.IdPrestamo);

            if (prestamo == null)
            {
                throw new Exception("El prestamo no existe");
            }

            if (uow.RepoDevolucion.GetByPrestamo(devolucionNEW.IdPrestamo) != null)
            {
                throw new Exception("El prestamo ya fue devuelto");
            }

            //if (repoDocentes.GetById(devolucionNEW.IdDocente) == null)
            //{
            //    throw new Exception("El docente no existe");
            //}

            if (uow.RepoUsuarios.GetById(devolucionNEW.IdUsuario) == null)
            {
                throw new Exception("El usuario no existe");
            }

            foreach (int idElemento in idsElementos)
            {
                if (uow.RepoElementos.GetDisponible(idElemento))
                {
                    throw new Exception($"El elemento {idElemento} no debe estar disponible.");
                }
            }

            if (idsElementos.Count() != idsEstadosElemento.Count())
            {
                throw new Exception("Error: el número de elementos y estados no coincide.");
            }

            if (idsElementos.Any())
            {
                throw new Exception("Debe seleccionar al menos un elemento para devolver.");
            }

            if (prestamo.IdCarrito != null)
            {
                uow.RepoCarritos.UpdateDisponible(prestamo.IdCarrito.Value, 1);
            }

            uow.RepoDevolucion.Insert(devolucionNEW);

            int cont = 0;
            foreach (int idElemento in idsElementos)
            {
                int estadoElemento = idsEstadosElemento.ElementAt(cont);
                string? obs = Observaciones?.ElementAtOrDefault(cont);

                uow.RepoDevolucionDetalle.Insert(new DevolucionDetalle
                {
                    IdDevolucion = devolucionNEW.IdDevolucion,
                    IdElemento = idElemento,
                    Observaciones = obs
                });

                uow.RepoElementos.UpdateEstado(idElemento, estadoElemento);

                //repoHistorialElemento.Insert(new HistorialElemento
                //{
                //    IdElemento = idElemento,
                //    IdCarrito = prestamo.IdCarrito,
                //    idUsuario = devolucionNEW.IdUsuario,
                //    IdEstadoMantenimiento = estadoElemento,
                //    FechaHora = DateTime.Now,
                //    Observacion = obs ?? "Devolución realizada"
                //});

                cont++;
            }


            scope.Complete();
        }
    }
    #endregion
}
