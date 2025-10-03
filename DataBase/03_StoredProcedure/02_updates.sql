-- =====================================================================
-- UPDATE PARA LA TABLA DE CARRITOS
-- =====================================================================

delimiter $$ 

drop procedure if exists UpdateCarrito $$
create procedure UpdateCarrito(in unidCarrito tinyint, in unequipo varchar(40), in unnumeroSerieCarrito varchar(40), in unidEstadoMantenimiento tinyint, in unidUbicacion tinyint , in unidModelo tinyint, in unhabilitado boolean, in unafechaBaja datetime)
begin
    update Carritos 
    set equipo = unequipo,
        numeroSerieCarrito = unnumeroSerieCarrito,
        idEstadoMantenimiento = unidEstadoMantenimiento,
        idUbicacion = unidUbicacion,
        idModelo = unidModelo,
        habilitado = unhabilitado,
        fechaBaja = unafechaBaja
    where idCarrito = unidCarrito;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE DOCENTES
-- =====================================================================

delimiter $$ 

drop procedure if exists UpdateDocente $$
create procedure UpdateDocente (in unidDocente smallint, in undni int, in unnombre varchar(40), in unapellido varchar(40), in unemail varchar(70), in habilitado boolean, in unafechaBaja datetime)
begin
	update docentes 
	set dni = undni,
		nombre = unnombre,
		apellido = unapellido,
		email = unemail,
        habilitado = unhabilitado,
        fechaBaja = unafechaBaja
	where idDocente = unidDocente;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE ELEMENTOS
-- =====================================================================

delimiter $$ 

drop procedure if exists UpdateElemento $$
create procedure UpdateElemento(in unidElemento tinyint, in unidTipoElemento tinyint ,in unidModelo tinyint, in unidUbicacion tinyint, in unidEstadoMantenimiento tinyint, in unequipo varchar(40), in unnumeroSerie varchar(40), in uncodigoBarra varchar(40), in unpatrimonio varchar(40) , in undisponible boolean, in unafechaBaja datetime)
begin
    update Elementos 
    set idTipoElemento = unidTipoElemento,
        idModelo = unidModelo,
        idUbicacion = unidUbicacion,
        idEstadoMantenimiento = unidEstadoMantenimiento,
        equipo = unequipo,
        numeroSerie = unnumeroSerie,
        codigoBarra = uncodigoBarra,
        patrimonio = unpatrimonio,
        habilitado = unhabilitado,
        fechaBaja = unafechaBaja
    where idElemento = unidElemento;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE NOTEBOOKS
-- =====================================================================

delimiter $$ 
drop procedure if exists UpdateNotebook $$
create procedure UpdateNotebook(in unidElemento smallint, in unidCarrito tinyint, in unaposicionCarrito tinyint
)
begin
    update Notebooks
    set idCarrito = unidCarrito,
        posicionCarrito = unaposicionCarrito
    where idElemento = unidElemento;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE USUARIOS
-- =====================================================================

delimiter $$

drop procedure if exists UpdateUsuario $$
create procedure UpdateUsuario (in unidUsuario tinyint, in unusuario varchar(40), in unpassword varchar(70), in unnombre varchar(40), in unapellido varchar(40), in unrol tinyint, in unemail varchar(70), in unafotoperfil VARCHAR(255), in unhabilitado boolean, in unafechaBaja datetime)
begin
    update usuarios u
    set idUsuario = unidEncargado,
        usuario = unusuario,
        pass = unpassword,
        nombre = unnombre,
        apellido = unapellido,
        idRol = unrol,
        email = unemail,
        FotoPerfil = unafotoperfil,
        habilitado = unhabilitado,
        fechaBaja = unafechaBaja
    where idUsuario = unidUsuario;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE PRESTAMOS
-- =====================================================================

delimiter $$

drop procedure if exists UpdatePrestamo $$
create procedure UpdatePrestamo (in unidPrestamo int, in unidCurso tinyint, in unidDocente smallint, in unidCarrito tinyint, in unidEstadoPrestamo tinyint, in unafechaPrestamo datetime)
begin
    update Prestamos
    set idCurso = unidCurso,
        idDocente = unidDocente,
        idCarrito = unidCarrito,
        idEstadoPrestamo = unidEstadoPrestamo,
        fechaPrestamo = unafechaPrestamo
    where idPrestamo = unidPrestamo;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE DEVOLUCION
-- =====================================================================

delimiter $$

drop procedure if exists UpdateDevolucion $$
create procedure UpdateDevolucion (in unidDevolucion int, in unidPrestamo int, in unidUsuario tinyint, in unidEstadoPrestamo tinyint, in unafechaDevolucion datetime, in unaobservacion varchar(200))
begin
    update Devoluciones
    set idPrestamo = unidPrestamo,
        idUsuario = unidUsuario,
        idEstadoPrestamo = unidEstadoPrestamo,
        fechaDevolucion = unafechaDevolucion,
        observaciones = unaobservacion
    where idDevolucion = unidDevolucion;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE CURSOS
-- =====================================================================

delimiter $$

drop procedure if exists UpdateCurso $$
create procedure UpdateCurso (in unidCurso tinyint, in uncurso varchar(40))
begin
    update Cursos
    set curso = uncurso
    where idCurso = unidCurso;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE TIPO ELEMENTO
-- =====================================================================

delimiter $$

drop procedure if exists UpdateTipoElemento $$
create procedure UpdateTipoElemento (in unidTipoElemento tinyint, in untipoElemento varchar(40))
begin
    update TipoElemento
    set elemento = untipoElemento
    where idTipoElemento = unidTipoElemento;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE ESTADO MANTENIMIENTO
-- =====================================================================

delimiter $$

drop procedure if exists UpdateEstadoMantenimiento $$
create procedure UpdateEstadoMantenimiento (in unidEstadoMantenimiento tinyint, in unestadoMantenimiento varchar(40))
begin
    update EstadosMantenimiento
    set estadoMantenimiento = unestadoMantenimiento
    where idEstadoMantenimiento = unidEstadoMantenimiento;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE ESTADO PRESTAMO
-- =====================================================================

delimiter $$

drop procedure if exists UpdateEstadoPrestamo $$
create procedure UpdateEstadoPrestamo (in unidEstadoPrestamo tinyint, in unestadoPrestamo varchar(40))
begin
    update EstadosPrestamo
    set estadoPrestamo = unestadoPrestamo
    where idEstadoPrestamo = unidEstadoPrestamo;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE ROLES
-- =====================================================================

delimiter $$

drop procedure if exists UpdateRol $$
create procedure UpdateRol (in unidRol tinyint, in unrol varchar(40))
begin
    update Rol 
    set rol = unrol
    where idRol = unidRol;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE UBICACION
-- =====================================================================

delimiter $$

drop procedure if exists UpdateUbicacion $$
create procedure UpdateUbicacion(in unidUbicacion tinyint, in unaubicacion varchar(40))
begin
    update Ubicacion
    set ubicacion = unaubicacion
    where idUbicacion = unidUbicacion;
end $$

delimiter ;



-- =====================================================================
-- UPDATE PARA LA TABLA DE MODELO
-- =====================================================================

delimiter $$

drop procedure if exists UpdateModelo $$
create procedure UpdateModelo(in unidModelo tinyint, in unidTipoElemento tinyint, in unmodelo varchar(40))
begin
    update Modelo
    set idTipoElemento = unidTipoElemento,
        modelo = unmodelo
    where idModelo = unidModelo;
end $$

delimiter ;