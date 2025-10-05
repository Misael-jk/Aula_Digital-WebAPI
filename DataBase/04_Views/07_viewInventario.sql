DROP VIEW IF EXISTS View_InventarioDTO;

CREATE VIEW View_InventarioDTO AS 
SELECT 
    v.subtipo as 'Equipo',
    m.modelo,
    COUNT(e.idElemento) AS CantidadTotal,
    SUM(CASE WHEN em.estadoMantenimiento = 'Disponible' THEN 1 ELSE 0 END) AS 'CantidadDisponible',

    -- Última fecha de cambio del subtipo
    (SELECT MAX(hc2.fechaCambio)
     FROM HistorialElemento he2
     JOIN HistorialCambio hc2 ON he2.idHistorialCambio = hc2.idHistorialCambio
     WHERE he2.idElemento IN (
         SELECT e2.idElemento
         FROM Elementos e2
         WHERE e2.idVariante = e.idVariante
     )
    ) AS 'FechaCambio',

    -- Última observación del subtipo
    (SELECT hc2.observacion
     FROM HistorialElemento he2
     JOIN HistorialCambio hc2 ON he2.idHistorialCambio = hc2.idHistorialCambio
     WHERE he2.idElemento IN (
         SELECT e2.idElemento
         FROM Elementos e2
         WHERE e2.idVariante = e.idVariante
     )
     ORDER BY hc2.fechaCambio DESC
     LIMIT 1
    ) AS 'Observacion',

    -- Último usuario que hizo el cambio
    (SELECT u.usuario
     FROM HistorialElemento he3
     JOIN HistorialCambio hc3 ON he3.idHistorialCambio = hc3.idHistorialCambio
     JOIN Usuarios u ON hc3.idUsuario = u.idUsuario
     WHERE he3.idElemento IN (
         SELECT e3.idElemento
         FROM Elementos e3
         WHERE e3.idVariante = e.idVariante)
     ORDER BY hc3.fechaCambio DESC
     LIMIT 1) AS 'Usuario'
FROM Elementos e
JOIN Modelo m using (idModelo)
JOIN VariantesElemento v ON e.idVariante = v.idVariante
JOIN EstadosMantenimiento em ON e.idEstadoMantenimiento = em.idEstadoMantenimiento
LEFT JOIN Notebooks n ON e.idElemento = n.idElemento
WHERE n.idElemento IS NULL
GROUP BY v.subtipo, m.modelo, v.idVariante
ORDER by v.subtipo, m.modelo;
