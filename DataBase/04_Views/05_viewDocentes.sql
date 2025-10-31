drop view if exists View_GetDocenteDTO;

create view View_GetDocenteDTO as
	select 
		d.idDocente,
		d.nombre,
		d.apellido,
		d.dni,
		d.email,
		ifnull(e.estadoPrestamo, 'Sin Prestamos') as 'EstadoPrestamo'
	from docentes d
	left join prestamos p using (idDocente)
	left join estadosprestamo e on e.idEstadoPrestamo = p.idEstadoPrestamo
	where d.habilitado = 1;