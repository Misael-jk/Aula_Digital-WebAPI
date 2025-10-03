drop view if exists View_GetDocenteDTO;

create view View_GetDocenteDTO as
	select 
		d.idDocente,
		d.nombre,
		d.apellido,
		d.dni,
		d.email
	from docentes d 
	where d.habilitado = 1;