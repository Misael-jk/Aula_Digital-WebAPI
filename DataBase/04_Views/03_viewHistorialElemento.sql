drop view if exists View_HistorialElementoDTO;

create view View_HistorialElementoDTO as
	SELECT 
    he.idHistorialCambio as 'IdHistorialElemento',
    v.subtipo AS 'Equipo',
    e.numeroSerie,
    m.modelo,
    em.estadoMantenimiento,
    u2.ubicacion AS UbicacionActual,
    hc.descripcion AS Descripcion,
    hc.motivo as Motivo,
    hc.fechaCambio,
    ta.accion AS AccionRealizada,
    concat(u.nombre, ' ', u.apellido) AS Usuario
FROM HistorialElemento he
JOIN HistorialCambio hc ON he.idHistorialCambio = hc.idHistorialCambio
JOIN TipoAccion ta ON hc.idTipoAccion = ta.idTipoAccion
JOIN Usuarios u ON hc.idUsuario = u.idUsuario
JOIN Elementos e ON he.idElemento = e.idElemento
JOIN VariantesElemento v ON e.idVariante = v.idVariante
JOIN Modelo m ON e.idModelo = m.idModelo
JOIN EstadosMantenimiento em ON e.idEstadoMantenimiento = em.idEstadoMantenimiento
JOIN Ubicacion u2 ON e.idUbicacion = u2.idUbicacion
left join notebooks n on n.idElemento = e.idElemento
where n.idElemento is null
ORDER BY hc.fechaCambio DESC;



-- _______________________________________________________________________________________
	SELECT 
    he.idHistorialCambio as 'IdHistorialElemento',
    hc.fechaCambio,
    ta.accion AS AccionRealizada,
    concat(u.nombre, ' ', u.apellido) AS Usuario
FROM HistorialElemento he
JOIN HistorialCambio hc ON he.idHistorialCambio = hc.idHistorialCambio
JOIN TipoAccion ta ON hc.idTipoAccion = ta.idTipoAccion
JOIN Usuarios u ON hc.idUsuario = u.idUsuario
JOIN Elementos e ON he.idElemento = e.idElemento
left join notebooks n on n.idElemento = e.idElemento
where n.idElemento is null
and e.idElemento = 11
ORDER BY hc.fechaCambio DESC;



-- _______________________________________________________________________________________
	SELECT 
	hc.descripcion as Descripcion
FROM HistorialElemento he
JOIN HistorialCambio hc ON he.idHistorialCambio = hc.idHistorialCambio
JOIN Elementos e ON he.idElemento = e.idElemento
left join notebooks n on n.idElemento = e.idElemento
where n.idElemento is null
and e.idElemento = 2
and he.idHistorialCambio = 2
ORDER BY hc.fechaCambio DESC;



-- _______________________________________________________________________________________
	SELECT 
	hc.motivo as Motivo
FROM HistorialElemento he
JOIN HistorialCambio hc ON he.idHistorialCambio = hc.idHistorialCambio
JOIN Elementos e ON he.idElemento = e.idElemento
left join notebooks n on n.idElemento = e.idElemento
where n.idElemento is null
and e.idElemento = 2
and he.idHistorialCambio = 2
ORDER BY hc.fechaCambio DESC;
