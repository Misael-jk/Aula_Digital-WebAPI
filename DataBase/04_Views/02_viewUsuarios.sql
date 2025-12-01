-- Mostrar Usuario disponibles en la UI

drop view if exists View_GetUsuarioDTO;

create view View_GetUsuarioDTO as
    SELECT 
        u.idUsuario,
        u.usuario,
        u.pass as Password,
        u.nombre,
        u.apellido,
        u.email as Email,
        r.rol
    FROM Usuarios u
    JOIN Rol r ON u.idRol = r.idRol
    where habilitado = 1;


    
drop view if exists View_GetUsuarioBajasDTO;

create view View_GetUsuarioBajasDTO as
    SELECT 
        u.idUsuario,
        u.usuario,
        u.pass as Password,
        u.nombre,
        u.apellido,
        u.email as Email,
        u.fechaBaja,
        r.rol as Rol
    FROM Usuarios u
    JOIN Rol r ON u.idRol = r.idRol
    where habilitado = 0;