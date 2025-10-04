DROP VIEW IF EXISTS View_Inventario;

CREATE VIEW View_Inventario AS 
SELECT 
    v.subtipo AS Subtipo,
    m.modelo AS Modelo,
    COUNT(e.idElemento) AS CantidadTotal,
    SUM(CASE WHEN em.estadoMantenimiento = 'Disponible' THEN 1 ELSE 0 END) AS CantidadDisponibles,
    (SELECT MAX(hc2.fechaCambio)
     FROM HistorialElemento he2
     JOIN HistorialCambio hc2 ON he2.idHistorialCambio = hc2.idHistorialCambio
     WHERE he2.idElemento IN (
         SELECT e2.idElemento
         FROM Elementos e2
         WHERE e2.idVariante = e.idVariante)) AS UltimaFechaCambio,
    (SELECT hc2.observacion
     FROM HistorialElemento he2
     JOIN HistorialCambio hc2 ON he2.idHistorialCambio = hc2.idHistorialCambio
     WHERE he2.idElemento IN (
         SELECT e2.idElemento
         FROM Elementos e2
         WHERE e2.idVariante = e.idVariante)
     ORDER BY hc2.fechaCambio DESC
     LIMIT 1) AS UltimaObservacion,
    (SELECT u.usuario
     FROM HistorialElemento he3
     JOIN HistorialCambio hc3 ON he3.idHistorialCambio = hc3.idHistorialCambio
     JOIN Usuarios u ON hc3.idUsuario = u.idUsuario
     WHERE he3.idElemento IN (
         SELECT e3.idElemento
         FROM Elementos e3
         WHERE e3.idVariante = e.idVariante)
     ORDER BY hc3.fechaCambio DESC
     LIMIT 1) AS Encargado
FROM Elementos e
JOIN Modelo m using (idModelo)
JOIN TipoElemento te ON m.idTipoElemento = te.idTipoElemento
JOIN VariantesElemento v ON e.idVariante = v.idVariante
JOIN EstadosMantenimiento em ON e.idEstadoMantenimiento = em.idEstadoMantenimiento
LEFT JOIN Notebooks n ON e.idElemento = n.idElemento
WHERE n.idElemento IS NULL
GROUP BY v.subtipo, m.modelo, v.idVariante
ORDER by v.subtipo, m.modelo;
