drop view if exists View_GetNotebookDTO;

create view View_GetNotebookDTO as 
    select 
        n.idElemento,
        n.equipo as "Equipo",
        n.posicionCarrito,
        e.numeroSerie,
        e.codigoBarra,
        e.patrimonio,
        c.equipo as "EquipoCarrito",
        ee.estadoMantenimiento as 'Estado',
        m.modelo,
        u.ubicacion
    from Elementos e
    join notebooks n using (idElemento)
    left join carritos c on n.idCarrito = c.idCarrito
    join modelo m on e.idModelo = m.idModelo
    join EstadosMantenimiento ee on e.idEstadoMantenimiento = ee.idEstadoMantenimiento
    join Ubicacion u on u.idUbicacion = e.idUbicacion
    where e.habilitado = 1;




drop view if exists View_GetNotebookBajasDTO;

create view View_GetNotebookBajasDTO as 
    select 
        n.idElemento,
        n.equipo as "Equipo",
        n.posicionCarrito,
        e.numeroSerie,
        e.codigoBarra,
        e.patrimonio,
        e.fechaBaja,
        c.equipo as "EquipoCarrito",
        ee.estadoMantenimiento as 'Estado',
        m.modelo,
        u.ubicacion
    from Elementos e
    join notebooks n using (idElemento)
    left join carritos c on n.idCarrito = c.idCarrito
    join modelo m on e.idModelo = m.idModelo
    join EstadosMantenimiento ee on e.idEstadoMantenimiento = ee.idEstadoMantenimiento
    join Ubicacion u on u.idUbicacion = e.idUbicacion
    where e.habilitado = 0;




DELIMITER $$   
DROP PROCEDURE IF EXISTS SP_GetNotebooksDTO $$
CREATE PROCEDURE SP_GetNotebooksDTO(IN untext VARCHAR(40), IN unidCarrito TINYINT, IN unequipo varchar(40))
BEGIN
       select 
        n.idElemento,
        n.equipo as "Equipo",
        n.posicionCarrito,
        e.numeroSerie,
        e.codigoBarra,
        e.patrimonio,
        c.equipo as "EquipoCarrito",
        ee.estadoMantenimiento as 'Estado',
        m.modelo,
        u.ubicacion
    from Elementos e
    join notebooks n using (idElemento)
    left join carritos c on n.idCarrito = c.idCarrito
    join modelo m on e.idModelo = m.idModelo
    join EstadosMantenimiento ee on e.idEstadoMantenimiento = ee.idEstadoMantenimiento
    join Ubicacion u on u.idUbicacion = e.idUbicacion
    join tipoelemento t on t.idTipoElemento = e.idTipoElemento 
    where e.habilitado = 1
      AND t.elemento IN ('Notebook')
      AND (
            untext IS NULL OR untext = ''
          OR e.numeroSerie LIKE CONCAT(untext, '%')
          OR e.codigoBarra LIKE CONCAT(untext, '%')
          OR e.patrimonio LIKE CONCAT(untext, '%')
      )
      AND (unidCarrito IS NULL OR unidCarrito = 0 OR c.idCarrito = unidCarrito)
      AND (unequipo IS NULL OR unequipo = '' OR n.equipo = unequipo);
     
END$$

DELIMITER ;