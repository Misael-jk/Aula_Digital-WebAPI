drop view if exists View_GetModeloDTO;

create view View_GetModeloDTO as
	select
		m.idModelo,
		m.modelo as 'NombreModelo',
		t.elemento as 'ElementoTipo'
	from modelo m 
	join tipoelemento t using (idTipoElemento);