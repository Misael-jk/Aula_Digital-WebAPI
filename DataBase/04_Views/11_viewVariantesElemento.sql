drop view if exists View_GetVariantesElementoDTO;

create view View_GetVariantesElementoDTO as
	select
		v.idVariante as 'IdVarianteElemento',
		v.subtipo as 'Variante',
		te.elemento as 'ElementoTipo',
		m.modelo as 'NombreModelo'
	from VariantesElemento v
	join TipoElemento te using(idTipoElemento)
	left join Modelo m on v.idModelo = m.idModelo;

