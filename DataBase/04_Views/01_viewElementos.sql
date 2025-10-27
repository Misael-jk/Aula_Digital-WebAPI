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
