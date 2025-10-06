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
        m.modelo
    from Elementos e
    join notebooks n using (idElemento)
    left join carritos c on n.idCarrito = c.idCarrito
    join modelo m on e.idModelo = m.idModelo
    join EstadosMantenimiento ee on e.idEstadoMantenimiento = ee.idEstadoMantenimiento
    where e.habilitado = 1;