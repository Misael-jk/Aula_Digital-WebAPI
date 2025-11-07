drop view if exists View_HistorialNotebookDTO;

create view View_HistorialNotebookDTO as
	SELECT 
    hn.idHistorialCambio as 'IdHistorialNotebook',
    n.equipo AS 'Equipo',
    e.numeroSerie,
    ifnull(c.equipo, 'Sin Carrito') as 'Carrito',
    ifnull(n.posicionCarrito, 0),
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
left join carritos c on n.idCarrito = c.idCarrito
where t.elemento in ("Notebook")
ORDER BY hc.fechaCambio DESC;



-- _______________________________________________________________________________________
	SELECT 
    hn.idHistorialCambio as 'IdHistorialNotebook',
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
where t.elemento in ("Notebook")
and n.idElemento = 2
ORDER BY hc.fechaCambio DESC;



-- ___________________________________________________________________________________________
	SELECT 
    hc.descripcion AS Descripcion
FROM historialnotebook hn
JOIN HistorialCambio hc ON hn.idHistorialCambio = hc.idHistorialCambio
JOIN Elementos e ON hn.idElemento = e.idElemento
join tipoelemento t on t.idTipoElemento = e.idTipoElemento
join notebooks n on e.idElemento = n.idElemento
where t.elemento in ("Notebook")
and n.idElemento = 2
and hn.idHistorialCambio = 2;



-- ________________________________________________________________________________________________
	SELECT 
    hc.motivo AS Motivo
FROM historialnotebook hn
JOIN HistorialCambio hc ON hn.idHistorialCambio = hc.idHistorialCambio
JOIN Elementos e ON hn.idElemento = e.idElemento
join tipoelemento t on t.idTipoElemento = e.idTipoElemento
join notebooks n on e.idElemento = n.idElemento
where t.elemento in ("Notebook")
and n.idElemento = 2
and hn.idHistorialCambio = 2;