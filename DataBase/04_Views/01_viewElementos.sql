-- Mostrar los Elementos disponibles en la UI
-- View Elementos DTO

drop view if exists View_GetElementosDTO;

create view View_GetElementosDTO as 
    select 
        e.idElemento,
        v.subtipo,
        e.numeroSerie,
        e.codigoBarra,
        e.patrimonio,
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
