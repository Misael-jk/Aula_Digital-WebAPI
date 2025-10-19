-- =====================================================================
-- INSERT PARA LA TABLA DE CARRITOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertCarrito $$
create procedure InsertCarrito(out unidCarrito tinyint, in unequipo varchar(40), in unnumeroSerieCarrito varchar(40), in unidEstadoMantenimiento tinyint, in unidUbicacion tinyint, in unidModelo tinyint, in unhabilitado boolean, in unafechaBaja datetime)
begin
    insert into Carritos (equipo, numeroSerieCarrito, idEstadoMantenimiento, idUbicacion, idModelo, habilitado, fechaBaja)
    values (unequipo, unnumeroSerieCarrito, unidEstadoMantenimiento, unidUbicacion, unidModelo, unhabilitado, unafechaBaja);

    set unidCarrito = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE VARIANTE DE ELEMENTOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertVarianteElemento $$
create procedure InsertVarianteElemento(out unidVariante smallint, in unidTipoElemento tinyint, in unsubtipo varchar(40), in unidModelo tinyint)
begin
    insert into VariantesElemento (idTipoElemento, subtipo, idModelo)
    values (unidTipoElemento, unsubtipo, unidModelo);

    set unidVariante = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE DOCENTES
-- =====================================================================

delimiter $$

drop procedure if exists InsertDocente $$
create procedure InsertDocente (out unidDocente smallint, in undni int, in unnombre varchar(40), in unapellido varchar(40), in unemail varchar(70), in unhabilitado boolean, in unafechaBaja datetime)
begin
    insert into Docentes(dni, nombre, apellido, email, habilitado, fechaBaja)
    values (undni, unnombre, unapellido, unemail, unhabilitado, unafechaBaja);

    set unidDocente = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE ELEMENTOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertElemento $$
create procedure InsertElemento (out unidElemento smallint, in unidTipoElemento tinyint,  in unidVariante varchar(80), in unidModelo tinyint, in unidUbicacion tinyint, in unidEstadoMantenimiento tinyint, in unequipo varchar(40), in unnumeroSerie varchar(40), in uncodigoBarra varchar(40), in unpatrimonio varchar(40), in unhabilitado boolean, in unafechaBaja datetime)
begin
    insert into Elementos (idTipoElemento, idVariante, idModelo, idUbicacion, idEstadoMantenimiento, numeroSerie, codigoBarra, patrimonio, habilitado, fechaBaja)
    values (unidTipoElemento, unidVariante, unidModelo, unidUbicacion, unidEstadoMantenimiento, unnumeroSerie, uncodigoBarra, unpatrimonio, unhabilitado, unafechaBaja);

    set unidElemento = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE NOTEBOOKS
-- =====================================================================

delimiter $$

drop procedure if exists InsertNotebook $$
create procedure InsertNotebook (out unidNotebook smallint, in unidTipoElemento tinyint, in unidVariante smallint, in unidModelo tinyint, in unidUbicacion tinyint, in unidEstadoMantenimiento tinyint, in unnumeroSerie varchar(40), in uncodigoBarra varchar(40), in unpatrimonio varchar(40), in unhabilitado boolean, in unafechaBaja datetime, in unequipo varchar(40), in unidCarrito tinyint, in unaposicionCarrito tinyint)
begin
declare unidElemento smallint;

call InsertElemento(unidElemento, unidTipoElemento, unidVariante, unidModelo, unidUbicacion, unidEstadoMantenimiento, unnumeroSerie, uncodigoBarra, unpatrimonio, unhabilitado, unafechaBaja);

    insert into Notebooks (idElemento, equipo, idCarrito, posicionCarrito)
    values (unidElemento, unequipo, unidCarrito, unaposicionCarrito);

    set unidNotebook = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE USUARIOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertUsuario $$
create procedure InsertUsuario (out unidUsuario tinyint, in unusuario varchar(40), in unpassword varchar(70), in unnombre varchar(40), in unapellido varchar(40), in unidRol tinyint, in unemail varchar(70), in unafotoperfil VARCHAR(255), in unhabilitado boolean, in unafechaBaja datetime)
begin
    insert into usuarios  (usuario, pass, nombre, apellido, idRol, email, fotoPerfil, habilitado, fechaBaja)
    values (unusuario, unpassword, unnombre, unapellido, unidRol, unemail, unafotoperfil, unhabilitado, unafechaBaja);

    set unidUsuario = last_insert_id(); 
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE TIPO ANOMALIAS
-- =====================================================================

-- delimiter $$

-- create procedure InsertTipoAnomalia (out unidTipoAnomalia tinyint, in unidTipoElemento smallint, in unnombreAnomalia varchar(70))
-- begin
--     insert into TipoAnomalia (idTipoElemento, nombreAnomalia)
--     values (unidTipoElemento, unnombreAnomalia);
--     set unidTipoAnomalia = last_insert_id();
-- end$$

-- delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE PRESTAMOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertPrestamo $$
create procedure InsertPrestamo (out unidPrestamo int, in unidUsuario tinyint, in unidCurso tinyint, in unidDocente smallint ,in unidCarrito tinyint ,in unidEstadoPrestamo tinyint ,in unafechaPrestamo datetime)
begin
    insert into Prestamos(idUsuarioRecibio, idCurso, idDocente, idCarrito, idEstadoPrestamo, fechaPrestamo)
    values (unidUsuario, unidCurso, unidDocente, unidCarrito, unidEstadoPrestamo, unafechaPrestamo);

    set unidPrestamo = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE PRESTAMOS DETALLES
-- =====================================================================

delimiter $$

drop procedure if exists InsertPrestamoDetalle $$
create procedure InsertPrestamoDetalle (in unidPrestamo int, in unidElemento smallint)
begin
    insert into PrestamoDetalle (idPrestamo, idElemento)
    values (unidPrestamo, unidElemento);
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE DEVOLUCION
-- =====================================================================

delimiter $$

drop procedure if exists InsertDevolucion $$
create procedure InsertDevolucion (out unidDevolucion int, in unidPrestamo int, in unidDocente smallint, in unidUsuario tinyint, in unidEstadoPrestamo tinyint, in unafechaDevolucion datetime, in unaobservacion varchar(200))
begin
    insert into Devoluciones (idPrestamo, idDocente, idUsuario, idEstadoPrestamo, fechaDevolucion, observaciones)
    values (unidPrestamo, unidUsuario, unidEstadoPrestamo, unafechaDevolucion, unaobservacion);

    set unidDevolucion = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE DEVOLUCION DETALLE
-- =====================================================================

delimiter $$

drop procedure if exists InsertDevolucionDetalle $$
create procedure InsertDevolucionDetalle (in unidDevolucion int ,in unidElemento smallint ,in unaobservacion varchar(200))
begin
    insert into DevolucionDetalle (idDevolucion, idElemento, observaciones)
    values (unidDevolucion, unidElemento, unaobservacion);
end $$

delimiter ;




-- =====================================================================
-- INSERT PARA LA TABLA DE DEVOLUCION ANOMALIAS
-- =====================================================================

-- delimiter $$

-- create procedure InsertrDevolucionAnomalia (in unidDevolucion int, in unidElemento smallint, in unidTipoAnomalia tinyint, in unadescripcion(200))
-- begin
--     insert into DevolucionAnomalia (idDevolucion, idElemento, idTipoAnomalia, descripcion)
--     values (unidDevolucion, unidElemento, unidTipoAnomalia, unadescripcion);
-- end$$

-- delimiter ;




-- =====================================================================
-- INSERT PARA LA TABLA DE CURSOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertCurso $$
create procedure InsertCurso (out unidCurso tinyint, in uncurso varchar(40))
begin
    insert into Cursos (curso)
    values (uncurso);

    set unidCurso = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE TIPO ELEMENTO
-- =====================================================================

delimiter $$

drop procedure if exists InsertTipoElemento $$
create procedure InsertTipoElemento (out unidTipoElemento tinyint, in untipoElemento varchar(40))
begin
    insert into TipoElemento (elemento)
    values (untipoElemento);

    set unidTipoElemento = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE ESTADO ELEMENTO
-- =====================================================================

delimiter $$

drop procedure if exists InsertEstadoMantenimiento $$
create procedure InsertEstadoMantenimiento (out unidEstadoMantenimiento tinyint, in unestadoMantenimiento varchar(40))
begin
    insert into EstadosElemento (estadoMantenimiento)
    values (unestadoMantenimiento);

    set unidEstadoMantenimiento = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE ESTADO PRESTAMO
-- =====================================================================

delimiter $$

drop procedure if exists InsertEstadoPrestamo $$
create procedure InsertEstadoPrestamo (out unidEstadoPrestamo tinyint, in unestadoPrestamo varchar(40))
begin
    insert into EstadosPrestamo(estadoPrestamo)
    values (unestadoPrestamo);

    set unidEstadoPrestamo = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA DE ROLES
-- =====================================================================

delimiter $$

drop procedure if exists InsertRol $$
create procedure InsertRol (out unidRol tinyint, in unrol varchar(40))
begin
    insert into Rol (rol)
    values (unrol);

    set unidRol = last_insert_id();
end $$



-- =====================================================================
-- INSERT PARA LA TABLA UBICACION
-- =====================================================================

delimiter $$

drop procedure if exists InsertUbicacion $$
create procedure InsertUbicacion (out unidUbicacion tinyint, in unaubicacion varchar(40))
begin
    insert into Ubicacion (ubicacion)
    values (unaubicacion);

    set unidUbicacion = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA MODELOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertModelo $$
create procedure InsertModelo (out unidModelo smallint, in unmodelo varchar(40), in unidTipoElemento tinyint)
begin
    insert into Modelo (idTipoElemento, modelo)
    values (unidTipoElemento, unmodelo);

    set unidModelo = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA TIPOS DE ACCION
-- =====================================================================

delimiter $$

drop procedure if exists InsertTipoAccion $$
create procedure InsertTipoAccion (out unidTipoAccion tinyint, in unnombreAccion varchar(50))
begin
    insert into TipoAccion (nombreAccion)
    values (unnombreAccion);

    set unidTipoAccion = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA HISTORIAL DE CAMBIOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertHistorialCambio $$
create procedure InsertHistorialCambio (out unidHistorialCambio int, in unidTipoAccion tinyint, in unidUsuario tinyint, in unfechaCambio datetime, in unadescripcion varchar(200), in unmotivo varchar(200))
begin
    insert into HistorialCambio (idTipoAccion, idUsuario, fechaCambio, descripcion)
    values (unidTipoAccion, unidUsuario, unfechaCambio, unadescripcion, unmotivo);

    set unidHistorialCambio = last_insert_id();
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA HISTORIAL DE NOTEBOOKS
-- =====================================================================

delimiter $$

drop procedure if exists InsertHistorialNotebook $$
create procedure InsertHistorialNotebook (
    in unidHistorialCambio int,
    in unidElemento smallint
)
begin
    insert into HistorialNotebook (idHistorialCambio, idElemento)
    values (unidHistorialCambio, unidElemento);
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA HISTORIAL DE ELEMENTOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertHistorialElemento $$
create procedure InsertHistorialElemento (in unidHistorialCambio int, in unidElemento smallint)
begin
    insert into HistorialElemento (idHistorialCambio, idElemento)
    values (unidHistorialCambio, unidElemento);
end $$

delimiter ;



-- =====================================================================
-- INSERT PARA LA TABLA HISTORIAL DE CARRITOS
-- =====================================================================

delimiter $$

drop procedure if exists InsertHistorialCarrito $$
create procedure InsertHistorialCarrito (in unidHistorialCambio int, in unidCarrito tinyint)
begin
    insert into HistorialCarrito (idHistorialCambio, idCarrito)
    values (unidHistorialCambio, unidCarrito);
end $$

delimiter ;
