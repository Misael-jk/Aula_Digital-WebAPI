using CapaEntidad;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using System.Transactions;
using CapaDatos.InterfaceUoW;

namespace CapaNegocio;

public class PrestamosCN
{
    private readonly IUowPrestamos uow;
    private readonly IMapperPrestamos mapperPrestamos;

    public PrestamosCN(IMapperPrestamos mapperPrestamos, IUowPrestamos uow)
    {
        this.uow = uow;
        this.mapperPrestamos = mapperPrestamos;
    }

    public IEnumerable<PrestamosDTO> ObtenerTodo()
    {
        return mapperPrestamos.GetAllDTO();
    }

    public void CrearPrestamo(Prestamos prestamo, IEnumerable<int> idsElementos, int? idCarrito)
    {
        using (TransactionScope scope = new TransactionScope())
        {

            if(uow.RepoDocentes.GetById(prestamo.IdDocente) == null)
            {
                throw new Exception("El docente no existe");
            }

            if (uow.RepoUsuarios.GetById(prestamo.IdUsuario) == null)
            {
                throw new Exception("El usuario no existe");
            }

            if (idsElementos == null || !idsElementos.Any())
            {
                throw new Exception("Debe prestar al menos un elemento.");
            }


            if (idCarrito.HasValue)
            {
                if(uow.RepoCarritos.GetById(idCarrito.Value) == null)
                {
                    throw new Exception("El carrito no existe.");
                }

                if(uow.RepoCarritos.GetCountByCarrito(idCarrito.Value) < 25)
                {
                    throw new Exception("El carrito debe tener al menos 25 elementos para ser prestado.");
                }

                if (!uow.RepoCarritos.GetDisponible(idCarrito.Value))
                {
                    throw new Exception("El carrito no esta disponible.");
                }

                prestamo.IdCarrito = idCarrito.Value;

                uow.RepoCarritos.UpdateDisponible(idCarrito.Value, 2);
            }

            foreach (int idElemento in idsElementos)
            {
                if (!uow.RepoElemento.GetDisponible(idElemento))
                {
                    throw new Exception($"El elemento {idElemento} no esta disponible.");
                }
            }

            uow.RepoPrestamos.Insert(prestamo);

            foreach (int idElemento in idsElementos)
            {
                uow.RepoPrestamoDetalle.Insert(new PrestamoDetalle
                {
                    IdPrestamo = prestamo.IdPrestamo,
                    IdElemento = idElemento
                });

                uow.RepoElemento.UpdateEstado(idElemento, 2);
            }



            scope.Complete();
        }
    }

    public void ActualizarPrestamo(Prestamos prestamo, IEnumerable<int> nuevosIdsElementos, int? nuevoIdCarrito)
    {
        using (TransactionScope scope = new TransactionScope())
        {
            if (uow.RepoDocentes.GetById(prestamo.IdDocente) == null)
            {
                throw new Exception("El docente no existe");
            }

            if (uow.RepoUsuarios.GetById(prestamo.IdUsuario) == null)
            {
                throw new Exception("El usuario no existe");
            }

            if (nuevosIdsElementos == null || !nuevosIdsElementos.Any())
            {
                throw new Exception("Debe prestar al menos un elemento.");
            }

            if (nuevoIdCarrito.HasValue)
            {
                if (uow.RepoCarritos.GetById(nuevoIdCarrito.Value) == null)
                {
                    throw new Exception("El carrito no existe");
                }

                if (uow.RepoCarritos.GetCountByCarrito(nuevoIdCarrito.Value) < 25)
                {
                    throw new Exception("El carrito debe tener al menos 25 elementos para ser prestado");
                }

                if (!uow.RepoCarritos.GetDisponible(nuevoIdCarrito.Value))
                {
                    throw new Exception("El carrito no esta disponible");
                }
                prestamo.IdCarrito = nuevoIdCarrito.Value;

                uow.RepoCarritos.UpdateDisponible(nuevoIdCarrito.Value, 2);
            }


            foreach (int idElemento in nuevosIdsElementos)
            {
                if (!uow.RepoElemento.GetDisponible(idElemento))
                {
                    throw new Exception($"El elemento {idElemento} no esta disponible");
                }
            }

            uow.RepoPrestamos.Update(prestamo);

            uow.RepoPrestamoDetalle.Delete(prestamo.IdPrestamo);

            foreach (int idElemento in nuevosIdsElementos)
            {
                uow.RepoPrestamoDetalle.Insert(new PrestamoDetalle
                {
                    IdPrestamo = prestamo.IdPrestamo,
                    IdElemento = idElemento,
                });

                uow.RepoElemento.UpdateEstado(idElemento, 2);

            }

            scope.Complete();
        }
    }

    public void EliminarPrestamo(int idPrestamo)
    {
        using (TransactionScope scope = new TransactionScope())
        {
            Prestamos? prestamo = uow.RepoPrestamos.GetById(idPrestamo);

            if (prestamo == null)
            {
                throw new Exception("El prestamo no existe.");
            }

            if (prestamo.IdCarrito.HasValue)
            {
                uow.RepoCarritos.UpdateDisponible(prestamo.IdCarrito.Value, 1);
            }

            IEnumerable<PrestamoDetalle> detalles = uow.RepoPrestamoDetalle.GetByPrestamo(idPrestamo);

            foreach (var detalle in detalles)
            {
                uow.RepoElemento.UpdateEstado(detalle.IdElemento, 1);

            }

            uow.RepoPrestamoDetalle.Delete(idPrestamo);

            uow.RepoPrestamos.Delete(idPrestamo);

            scope.Complete();
        }
    }

}


