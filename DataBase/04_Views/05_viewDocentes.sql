DROP VIEW IF EXISTS View_GetDocenteDTO;

CREATE VIEW View_GetDocenteDTO AS
SELECT 
    d.idDocente,
    d.nombre,
    d.apellido,
    d.dni,
    d.email,
    IFNULL(e.estadoPrestamo, 'Sin Prestamos') AS EstadoPrestamo
FROM Docentes d
LEFT JOIN (
    SELECT 
        p1.idDocente,
        p1.idEstadoPrestamo
    FROM Prestamos p1
    INNER JOIN (
        SELECT idDocente, MAX(fechaPrestamo) AS fechaMax
        FROM Prestamos
        GROUP BY idDocente
    ) p2 ON p1.idDocente = p2.idDocente 
        AND p1.fechaPrestamo = p2.fechaMax
) ult ON d.idDocente = ult.idDocente
LEFT JOIN EstadosPrestamo e ON e.idEstadoPrestamo = ult.idEstadoPrestamo
WHERE d.habilitado = 1;
