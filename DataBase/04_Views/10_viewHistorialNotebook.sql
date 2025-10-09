drop view if exists View_HistorialNotebookDTO;

create view View_HistorialNotebookDTO as
	SELECT 
    hn.idHistorialCambio as 'IdHistorialNotebook',
    n.equipo AS 'Equipo',
    e.numeroSerie,
    c.equipo as 'Carrito',
    n.posicionCarrito,
    m.modelo,
    em.estadoMantenimiento,
    hc.descripcion AS Descripcion,
    hc.motivo as 'Motivo',
    hc.fechaCambio,
    ta.accion AS AccionRealizada,
    concat(u.nombre, ' ', u.apellido) AS Usuario
FROM historialnotebook hn
JOIN HistorialCambio hc ON hn.idHistorialCambio = hc.idHistorialCambio
JOIN TipoAccion ta ON hc.idTipoAccion = ta.idTipoAccion
JOIN Usuarios u ON hc.idUsuario = u.idUsuario
JOIN Elementos e ON hn.idElemento = e.idElemento
join tipoelemento t on t.idTipoElemento = e.idTipoElemento
join notebooks n on e.idElemento = n.idElemento
JOIN Modelo m ON e.idModelo = m.idModelo
JOIN EstadosMantenimiento em ON e.idEstadoMantenimiento = em.idEstadoMantenimiento
join carritos c on n.idCarrito = c.idCarrito
where t.elemento in ("Notebook")
ORDER BY hc.fechaCambio DESC;