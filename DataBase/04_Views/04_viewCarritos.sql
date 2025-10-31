drop view if exists View_GetCarritoDTO;

create view View_GetCarritoDTO as
	select 
		c.idCarrito,
		c.equipo as 'EquipoCarrito',
		c.numeroSerieCarrito,
		c.capacidad,
		e.estadoMantenimiento as 'EstadoMantenimientoNombre',
		u.ubicacion as 'NombreUbicacion',
		m.modelo as 'NombreModelo'
	from carritos c 
	join estadosmantenimiento e using (idEstadoMantenimiento)
	join Ubicacion u using (idUbicacion)
	join Modelo m using (idModelo)
	where c.habilitado = 1
	and c.idEstadoMantenimiento in (1, 2);



drop view if exists View_GetCarritoBajasDTO;

create view View_GetCarritoBajasDTO as
	select 
		c.idCarrito,
		c.equipo as 'EquipoCarrito',
		c.numeroSerieCarrito,
		c.capacidad,
		c.fechaBaja,
		u.ubicacion as 'NombreUbicacion',
		m.modelo as 'NombreModelo',
		e.estadoMantenimiento as 'EstadoMantenimientoNombre'
	from carritos c 
	join estadosmantenimiento e using (idEstadoMantenimiento)
	join Ubicacion u using (idUbicacion)
	join Modelo m using (idModelo)
	where c.habilitado = 0
	and c.idEstadoMantenimiento not in (1, 2);