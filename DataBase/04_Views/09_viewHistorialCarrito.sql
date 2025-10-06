drop view if exists View_HistorialCarritoDTO;

create view View_HistorialCarritoDTO as
	select
		hc.idHistorialCambio as 'IdHistorialCarrito',
		c.equipo,
		c.numeroSerieCarrito,
		u2.ubicacion as 'UbicacionActual',
		m.modelo,
		em.estadoMantenimiento,
		hc.observacion as 'Descripcion',
		hc.fechaCambio,
		ta.accion as 'AccionRealizada',
		concat(u.nombre, ' ', u.apellido) AS Usuario
		from historialcarrito h 
		JOIN HistorialCambio hc ON h.idHistorialCambio = hc.idHistorialCambio
		JOIN TipoAccion ta ON hc.idTipoAccion = ta.idTipoAccion
		JOIN Usuarios u ON hc.idUsuario = u.idUsuario
		join carritos c using (idCarrito)
		join modelo m on c.idModelo = m.idModelo
		join estadosmantenimiento em on c.idEstadoMantenimiento = em.idEstadoMantenimiento
		join ubicacion u2 on c.idUbicacion = u2.idUbicacion
		ORDER BY hc.fechaCambio DESC;