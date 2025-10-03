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
    JOIN Rol r ON u.idRol = r.idRol;