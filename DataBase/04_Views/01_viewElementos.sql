-- Mostrar los Elementos disponibles en la UI
-- View Elementos DTO

drop view if exists View_GetElementosDTO;

create view View_GetElementosDTO as 
    select 
        e.idElemento as 'IdElemento',
        e.numeroSerie as 'NumeroSerie',
        e.codigoBarra as 'CodigoBarra',
        e.patrimonio as 'Patrimonio',
        v.subtipo as 'Variante',
        t.elemento as 'ElementoTipo',
        ee.estadoMantenimiento as 'EstadoMantenimientoNombre',
        m.modelo as 'NombreModelo',
        u.ubicacion as 'NombreUbicacion'
    from Elementos e
    join modelo m using (idModelo)
    join varianteselemento v using (idVariante)
    join tipoElemento t on t.idTipoElemento = m.idTipoElemento
    join EstadosMantenimiento ee using(idEstadoMantenimiento)
    join Ubicacion u using (idUbicacion)
    where e.habilitado = 1
    and t.elemento not in ('Notebook');


DROP PROCEDURE IF EXISTS SP_GetElementosDTO;
DELIMITER $$

CREATE PROCEDURE SP_GetElementosDTO(
    IN untext VARCHAR(40),          
    IN unidTipo TINYINT,          
    IN unidModelo TINYINT          
)
BEGIN
    SELECT 
        e.idElemento AS IdElemento,
        v.subtipo AS Equipo,
        e.numeroSerie AS NumeroSerie,
        e.codigoBarra AS CodigoBarra,
        u.ubicacion AS Ubicacion,
        ee.estadoMantenimiento AS Estado,
        t.elemento AS TipoElemento,
        e.patrimonio AS Patrimonio,
        m.modelo AS Modelo
    FROM Elementos e
    JOIN Modelo m USING (idModelo)
    JOIN VariantesElemento v USING (idVariante)
    JOIN TipoElemento t ON t.idTipoElemento = m.idTipoElemento
    JOIN EstadosMantenimiento ee USING (idEstadoMantenimiento)
    JOIN Ubicacion u USING (idUbicacion)
    WHERE e.habilitado = 1
      AND t.elemento NOT IN ('Notebook')
      AND (
            untext IS NULL OR untext = ''
          OR e.numeroSerie LIKE CONCAT(untext, '%')
          OR e.codigoBarra LIKE CONCAT(untext, '%')
          OR e.patrimonio LIKE CONCAT(untext, '%')
      )
      AND (unidTipo IS NULL OR unidTipo = 0 OR m.idTipoElemento = unidTipo)
      AND (unidModelo IS NULL OR unidModelo = 0 OR m.idModelo = unidModelo)
    ;
END$$

DELIMITER ;


drop view if exists View_GetElementosBajasDTO;

create view View_GetElementosBajasDTO as 
    select 
        e.idElemento as 'IdElemento',
        e.numeroSerie as 'NumeroSerie',
        e.codigoBarra as 'CodigoBarra',
        e.patrimonio as 'Patrimonio',
        e.fechaBaja,
        v.subtipo as 'Variante',
        t.elemento as 'ElementoTipo',
        m.modelo as 'NombreModelo',
        u.ubicacion as 'NombreUbicacion',
        ee.estadoMantenimiento as 'EstadoMantenimientoNombre'
    from Elementos e
    join modelo m using (idModelo)
    join varianteselemento v using (idVariante)
    join tipoElemento t on t.idTipoElemento = m.idTipoElemento
    join EstadosMantenimiento ee using(idEstadoMantenimiento)
    join Ubicacion u using (idUbicacion)
    where e.habilitado = 0
    and t.elemento not in ('Notebook')
    and e.idEstadoMantenimiento not in (1, 2);
