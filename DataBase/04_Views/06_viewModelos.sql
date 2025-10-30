drop view if exists View_GetModeloDTO;

create view View_GetModeloDTO as
	select
		m.idModelo,
		m.modelo as 'NombreModelo',
		t.elemento as 'ElementoTipo'
	from modelo m 
	join tipoelemento t using (idTipoElemento);


drop view if exists View_GetModeloElementoDTO;

create view View_GetModeloElementoDTO as
	select
		m.idModelo,
		m.modelo as 'NombreModelo',
		t.elemento as 'ElementoTipo'
	from modelo m 
	join tipoelemento t using (idTipoElemento)
	where t.idTipoElemento not in (1, 2);
