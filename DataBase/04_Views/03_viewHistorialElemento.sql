drop view if exists View_HistorialElementoDTO;

create view View_HistorialElementoDTO as
	SELECT 
    he.idHistorialCambio as 'IdHistorialElemento',
    v.subtipo AS 'Equipo',
    e.numeroSerie,
    m.modelo,
    em.estadoMantenimiento,
    u2.ubicacion AS UbicacionActual,
    hc.observacion AS Descripcion,
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
