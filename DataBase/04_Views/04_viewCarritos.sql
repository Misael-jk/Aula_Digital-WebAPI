drop view if exists View_GetCarritoDTO;

create view View_GetCarritoDTO as
	select 
		c.idCarrito,
		c.equipo,
		c.numeroSerieCarrito,
		e.estadoMantenimiento as 'EstadoMantenimientoNombre',
		u.ubicacion as 'NombreUbicacion',
		m.modelo as 'NombreModelo'
	from carritos c 
	join estadosmantenimiento e using (idEstadoMantenimiento)
	join Ubicacion u using (idUbicacion)
	join Modelo m using (idModelo)
	where c.habilitado = 1;