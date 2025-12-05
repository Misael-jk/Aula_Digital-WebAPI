namespace Core.Entities.Aggregates.Prestamos;

public class Prestamos
{
    public int IdPrestamo {get; set;}
    public int? IdCurso {get; set;}
    public int IdUsuario {get; set;}
    public int IdDocente {get; set;}
    public int IdEstadoPrestamo { get; set; }
    public int? IdCarrito {get; set;} 
    public DateTime FechaPrestamo {get; set;}

    public List<PrestamoDetalle> Detalles { get; set; } = new();
    public List<PrestamoDetalleCambio> Cambios { get; set; } = new();
    public Devolucion? Devolucion { get; set; } = new();


    #region AGREGAR ELEMENTO AL PRESTAMO
    public void AgregarDetalle(int idElemento)
    {
        if (Detalles.Any(d => d.IdElemento == idElemento))
        {
            throw new InvalidOperationException("El elemento ya está en el prestamo");
        }

        Detalles.Add(new PrestamoDetalle
        {
            IdPrestamo = this.IdPrestamo,
            IdElemento = idElemento
        });
    }
    #endregion

    #region QUITAR ELEMENTO DEL PRESTAMO
    public void QuitarDetalle(int idElemento)
    {
        PrestamoDetalle? detalle = Detalles.FirstOrDefault(d => d.IdElemento == idElemento);

        if (detalle == null)
        {
            throw new InvalidOperationException("Elemento no encontrado en el prestamo");
        }

        Detalles.Remove(detalle);
    }
    #endregion

    #region REEMPLAZO DE ELEMENTO EN UN PRESTAMO
    public void ReemplazarElemento(int elementoOriginal, int elementoReemplazo, int idTipoAnomalia, int usuarioRegistro, DateTime fechaCambio, string? motivo = null)
    {
        if (elementoOriginal == elementoReemplazo)
        {
            throw new ArgumentException("elemento original y reemplazo iguales");
        }

        PrestamoDetalle? det = Detalles.FirstOrDefault(d => d.IdElemento == elementoOriginal);
        if (det == null)
        {
            throw new InvalidOperationException("Elemento original no pertenece al prestamo");
        }

        var cambio = new PrestamoDetalleCambio
        {
            IdPrestamo = this.IdPrestamo,
            IdTipoAnomalia = idTipoAnomalia,
            ElementoOriginal = elementoOriginal,
            ElementoReemplazo = elementoReemplazo,
            IdUsuarioRegistro = usuarioRegistro,
            FechaCambio = fechaCambio,
            Motivo = motivo
        };
        Cambios.Add(cambio);

        det.IdElemento = elementoReemplazo;
    }
    #endregion

    #region REEMPLAZO DE CARRITO EN UN PRESTAMO
    public void ReemplazarCarrito(int carritoOriginal, int carritoReemplazo)
    {
        if(carritoOriginal == carritoReemplazo)
        {
            throw new ArgumentException("El carrito original y el de reemplazo son iguales");
        }
        IdCarrito = carritoReemplazo;
    }
    #endregion

    public void RegistrarDevolucion(int usuarioDevolvio, DateTime fechaDevolucion, bool carritoDevuelto, IEnumerable<(int idElemento, DateTime fecha, string? observaciones)> detalles, IEnumerable<(int idElemento, int idTipoAnomalia, string? descripcion)>? anomalias = null)
    {
        if (Devolucion != null)
        {
            throw new InvalidOperationException("Préstamo ya registró una devolución.");
        }

        var dev = new Devolucion
        {
            IdPrestamo = this.IdPrestamo,
            IdUsuario = usuarioDevolvio,
            FechaDevolucion = fechaDevolucion,
            CarritoDevuelto = carritoDevuelto
        };

        foreach (var d in detalles)
        {
            if (!Detalles.Any(x => x.IdElemento == d.idElemento))
            {
                throw new InvalidOperationException($"Elemento {d.idElemento} no pertenece al préstamo.");
            }

            dev.AddDetalle(d.idElemento, d.fecha, d.observaciones);
        }


        if (anomalias != null)
        {
            foreach (var a in anomalias)
            {
                dev.AddAnomalia(a.idElemento, a.idTipoAnomalia, a.descripcion);
            }
        }

        Devolucion = dev;

        if (dev.IsFull(Detalles.Count))
            IdEstadoPrestamo = 3;
        else if (dev.IsPartial(Detalles.Count))
            IdEstadoPrestamo = 4; 

    }
}
