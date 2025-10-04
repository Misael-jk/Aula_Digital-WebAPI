drop view if exists View_GetNotebookDTO;

create view View_GetNotebookDTO as 
    select 
        e.idElemento,
        n.equipo as "Equipo",
        c.equipo as "Carrito",
        e.numeroSerie,
        e.codigoBarra,
        e.patrimonio,
        ee.estadoMantenimiento as 'EstadoMantenimientoNombre',
        m.modelo as 'NombreModelo'
    from Elementos e
    join notebooks n using (idElemento)
    left join carritos c on n.idCarrito = c.idCarrito
    join modelo m on e.idModelo = m.idModelo
    join EstadosMantenimiento ee on e.idEstadoMantenimiento = ee.idEstadoMantenimiento
    where e.habilitado = 1;