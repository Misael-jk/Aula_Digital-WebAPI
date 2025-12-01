drop view if exists View_GetPrestamosActivosDTO;

create view View_GetPrestamosActivosDTO as
SELECT 
    p.idPrestamo,
    d.nombre AS Nombre,
    d.apellido AS Apellido,
    ifnull(c.equipo, 'Sin Carrito') as Carrito,
    p.fechaPrestamo as Fecha,
    COUNT(DISTINCT pd.idElemento) AS Prestadas,
    COUNT(DISTINCT dd.idElemento) AS Devueltas,
    e.estadoPrestamo as Estado
FROM Prestamos p
INNER JOIN Docentes d ON p.idDocente = d.idDocente
INNER JOIN PrestamoDetalle pd ON p.idPrestamo = pd.idPrestamo
join estadosprestamo e on e.idEstadoPrestamo = p.idEstadoPrestamo 
left join carritos c using (idCarrito)
LEFT JOIN Devoluciones dv ON p.idPrestamo = dv.idPrestamo
LEFT JOIN DevolucionDetalle dd ON dv.idDevolucion = dd.idDevolucion
GROUP BY p.idPrestamo, d.nombre, d.apellido, p.fechaPrestamo
HAVING COUNT(DISTINCT dd.idElemento) < COUNT(DISTINCT pd.idElemento) 
ORDER BY p.fechaPrestamo DESC;