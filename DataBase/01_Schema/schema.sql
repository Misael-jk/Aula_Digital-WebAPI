drop database if exists WebAPI_Aula_Digital;
create database WebAPI_Aula_Digital;
use WebAPI_Aula_Digital;


create table Rol(
    idRol tinyint not null auto_increment,
    rol varchar(40) not null,
    constraint PK_Rol primary key (idRol)
);


create table Turnos (
    idTurno tinyint not null auto_increment,
    turno varchar(40) not null,
    constraint PK_Turnos primary key (idTurno)
);


create table DiasLaborales (
    idDiaLaboral tinyint not null auto_increment,
    diaSemana varchar(40) not null,
    constraint PK_DiasLaborales primary key (idDiaLaboral)
);


create table Horarios(
    idHorario tinyint not null auto_increment,
    idTurno tinyint not null,
    entrada time not null,
    salida time not null,
    constraint PK_Horarios primary key (idHorario),
    constraint FK_Horarios_Turnos foreign key (idTurno)
        references Turnos(idTurno),
    constraint CHK_Horarios_Times check (entrada < salida)
);


create table HorariosDias (
    idHorario tinyint not null,
    idDiaLaboral tinyint not null,
    constraint PK_HorariosDias primary key (idHorario, idDiaLaboral),
    constraint FK_HorariosDias_Horarios foreign key (idHorario)
    	references Horarios (idHorario),
    constraint FK_HorariosDias_DiasLaborales foreign key (idDiaLaboral)
    	references DiasLaborales (idDiaLaboral)
);


create table Usuarios (
    idUsuario smallint not null auto_increment,
    usuario varchar(40) not null,
    pass varchar(255) not null,
    nombre varchar(40) not null,
    apellido varchar(40) not null,
    idRol tinyint not null,
    email varchar(70) not null,
    fotoPerfil varchar(255),
    fechaRegistro datetime not null,
    fechaAlta datetime not null,
    fechaModificacion datetime,
    creadorPor tinyint,
    modificadoPor tinyint,
    habilitado boolean not null,
    fechaBaja datetime,
    constraint PK_Usuarios primary key (idUsuario),
    constraint UQ_Usuarios unique (usuario),
    constraint UQ_Usuarios_Email unique (email),
    constraint FK_Usuarios_Rol foreign key (idRol)
        references Rol (idRol),
    constraint FK_Usuarios_CreadorPor foreign key (creadorPor) 
        references Usuarios(idUsuario) ON DELETE SET NULL ON UPDATE CASCADE,
    constraint FK_Usuarios_ModificadoPor foreign key (modificadoPor) 
        references Usuarios(idUsuario) ON DELETE SET NULL ON UPDATE CASCADE
);


create table UsuariosHorarios (
    idUsuario smallint not null,
    idHorario tinyint not null,
    constraint PK_UsuariosHorarios primary key (idUsuario, idHorario),
    constraint FK_UsuariosHorarios_Usuarios foreign key (idUsuario)
    	references Usuarios (idUsuario),
    constraint FK_UsuariosHorarios_Horarios foreign key (idHorario)
    	references Horarios (idHorario) ON DELETE CASCADE ON UPDATE CASCADE
);


create table RefreshTokens (
    idRefreshToken bigint not null auto_increment,
    idUsuario smallint not null,
    refreshToken varchar(200) not null,
    emitido datetime not null,
    expiracion datetime not null,
    revocado boolean not null,
    fechaRevocado datetime,
    reemplazadoPorToken varchar(200),
    constraint UQ_RefreshTokens_uniqueToken unique (refreshToken),
    constraint PK_RefreshTokens primary key (idRefreshToken),
    constraint FK_RefreshTokens_Usuario foreign key (idUsuario)
        references Usuarios(idUsuario) ON DELETE CASCADE ON UPDATE CASCADE
);


create table Docentes (
    idDocente smallint not null auto_increment,
    dni int not null,
    nombre varchar(40) not null,
    apellido varchar(40) not null,
    email varchar(70) not null,
    fechaAlta datetime not null,
    fechaModificacion datetime,
    creadorPor smallint,
    modificadoPor smallint,
    habilitado boolean not null,
    fechaBaja datetime,
    constraint PK_Docentes primary key (idDocente),
    constraint UQ_Docentes_Dni unique (dni),
    constraint UQ_Docentes_Email unique (email),
    constraint FK_Docentes_CreadorPor foreign key (creadorPor) 
        references Usuarios(idUsuario) ON DELETE SET NULL ON UPDATE CASCADE,
    constraint FK_Docentes_ModificadoPor foreign key (modificadoPor) 
        references Usuarios(idUsuario) ON DELETE SET NULL ON UPDATE CASCADE
);


create table Ubicacion(
	idUbicacion tinyint not null auto_increment,
	ubicacion varchar(40) not null,
	constraint PK_Ubicacion primary key (idUbicacion)
);


create table TipoElemento (
    idTipoElemento tinyint not null auto_increment,
    elemento varchar(40) not null,
    constraint PK_TipoElemento primary key (idTipoElemento)
);


create table Modelo (
idModelo tinyint not null auto_increment,
idTipoElemento tinyint not null,
modelo varchar(40) not null,
constraint PK_Modelo primary key (idModelo),
constraint UQ_Modelo unique (idTipoElemento, modelo),
constraint FK_Modelo_TipoElemento foreign key (idTipoElemento)
	references TipoElemento (idTipoElemento)
);


create table EstadosMantenimiento (
    idEstadoMantenimiento tinyint not null auto_increment,
    estadoMantenimiento varchar(40) not null,
    constraint PK_EstadosMantenimiento primary key (idEstadoMantenimiento)
);


create table VariantesElemento (
    idVariante smallint not null auto_increment,
    idTipoElemento tinyint not null,
    subtipo varchar(40) not null,        
    idModelo tinyint,                
    constraint PK_VariantesElemento primary key (idVariante),
    constraint UQ_VariantesElemento unique (idTipoElemento, subtipo),
    constraint FK_VariantesElemento_TipoElemento foreign key (idTipoElemento)
        references TipoElemento (idTipoElemento),
    constraint FK_VariantesElemento_Modelo foreign key (idModelo)
        references Modelo (idModelo)
);


create table Carritos (
    idCarrito tinyint not null auto_increment,
    equipo varchar(40) not null,
    capacidad tinyint not null,
    idModelo tinyint,
    numeroSerieCarrito varchar(40) not null,
    idEstadoMantenimiento tinyint not null,
    idUbicacion tinyint not null,
    fechaAlta datetime not null,
    fechaModificacion datetime,
    creadorPor smallint,
    modificadoPor smallint,
    habilitado boolean not null,
    fechaBaja datetime,
    constraint PK_Carritos primary key (idCarrito),
    constraint UQ_Carritos_numeroSerieCarrito unique (numeroSerieCarrito),
    constraint FK_Carritos_Modelo foreign key (idModelo)
        references Modelo (idModelo),
    constraint FK_Carritos_EstadoMantenimiento foreign key (idEstadoMantenimiento)
        references EstadosMantenimiento (idEstadoMantenimiento),
    constraint FK_Carritos_Ubicacion foreign key (idUbicacion)
        references Ubicacion (idUbicacion),
    constraint FK_Carritos_CreadorPor foreign key (creadorPor) 
        references Usuarios(idUsuario) ON DELETE SET NULL ON UPDATE CASCADE,
    constraint FK_Carritos_ModificadoPor foreign key (modificadoPor) 
        references Usuarios(idUsuario) ON DELETE SET NULL ON UPDATE CASCADE
); 
    
    
create table Elementos (
    idElemento smallint not null auto_increment,
    idTipoElemento tinyint not null,
    idVariante smallint,
    idModelo tinyint,
    idUbicacion tinyint not null,
    idEstadoMantenimiento tinyint not null,
    numeroSerie varchar(40) not null,
    codigoBarra varchar(40) not null,
    patrimonio varchar(60) not null,
    fechaAlta datetime not null,
    fechaModificacion datetime,
    creadorPor smallint,
    modificadoPor smallint,
    habilitado boolean not null,
    fechaBaja datetime,
    constraint PK_Elementos primary key (idElemento),
    constraint UQ_Elementos_numeroSerie unique (numeroSerie),
    constraint UQ_Elementos_codigoBarra unique (codigoBarra),
    constraint UQ_Elementos_patrimonio unique (patrimonio),
    constraint FK_Elementos_TipoElemento foreign key (idTipoElemento)
        references TipoElemento(idTipoElemento),
    constraint FK_Elementos_VariantesElementos foreign key (idVariante)
        references VariantesElemento (idVariante),
    constraint FK_Elementos_Modelo foreign key (idModelo)
        references Modelo (idModelo),
    constraint FK_Elementos_Ubicacion foreign key (idUbicacion)
        references Ubicacion(idUbicacion),
    constraint FK_Elementos_EstadoMantenimiento foreign key (idEstadoMantenimiento)
        references EstadosMantenimiento (idEstadoMantenimiento),
    constraint FK_Elementos_CreadorPor foreign key (creadorPor) 
        references Usuarios(idUsuario) ON DELETE SET NULL ON UPDATE CASCADE,
    constraint FK_Elementos_ModificadoPor foreign key (modificadoPor) 
        references Usuarios(idUsuario) ON DELETE SET NULL ON UPDATE CASCADE
);


create table Notebooks (
    idElemento smallint not null,
    equipo varchar(40) not null,
    idCarrito tinyint,
    posicionCarrito tinyint,
    constraint PK_Notebooks primary key (idElemento),
    constraint UQ_Notebooks unique (equipo),
    constraint FK_Notebooks_Elementos foreign key (idElemento)
        references Elementos (idElemento),
    constraint FK_Notebooks_Carrito foreign key (idCarrito) 
        references Carritos(idCarrito)
);   


create table Cursos (
    idCurso tinyint not null auto_increment,
    curso varchar(40) not null,
    constraint PK_Cursos primary key (idCurso)
);


create table EstadosPrestamo (
    idEstadoPrestamo tinyint not null auto_increment,
    estadoPrestamo varchar(40) not null,
    constraint PK_EstadoPrestamo primary key (idEstadoPrestamo),
    constraint UQ_EstadoPrestamo unique (estadoPrestamo)
);


create table TipoAnomalias (
    idTipoAnomalia tinyint not null auto_increment,
    idTipoElemento tinyint,
    nombreAnomalia varchar(70) not null,
    constraint PK_TipoAnomalia primary key (idTipoAnomalia),
    constraint UQ_TipoAnomalia_nombreAnomalia unique (nombreAnomalia),
    constraint FK_TipoAnomalia_TipoElemento foreign key (idTipoElemento)
        references TipoElemento (idTipoElemento)
);


create table EstadoReservas (
    idEstadoReserva tinyint not null auto_increment,
    estadoReserva varchar(40) not null,
    constraint PK_EstadoReservas primary key (idEstadoReserva)
);


create table Reservas (
    idReserva int not null auto_increment,
    idTurno tinyint not null,
    idUsuario smallint not null,
    idDocente smallint not null,
    idCarrito tinyint,
    fechaSolicitud datetime not null,
    fechaInicio datetime not null,
    fechaFin datetime not null,
    idEstadoReserva tinyint not null,
    motivo varchar(255),
    constraint PK_Reservas primary key (idReserva),
    constraint FK_Reservas_Turnos foreign key (idTurno) 
        references Turnos(idTurno),
    constraint FK_Reservas_Usuarios foreign key (idUsuario) 
        references Usuarios(idUsuario),
    constraint FK_Reservas_EstadoReserva foreign key (idEstadoReserva) 
        references EstadoReservas(idEstadoReserva),
    constraint FK_Reservas_Docentes foreign key (idDocente) 
        references Docentes(idDocente),
    constraint FK_Reservas_Carritos foreign key (idCarrito)
        references Carritos(idCarrito)
);


create table ReservaDetalle (
    idReserva int not null,
    idElemento smallint not null,
    constraint PK_ReservaDetalle primary key (idReserva, idElemento),
    constraint FK_ReservaDetalle_Reservas foreign key (idReserva) 
        references Reservas(idReserva),
    constraint FK_ReservaDetalle_Elementos foreign key (idElemento) 
        references Elementos(idElemento)
);


create table Prestamos (
    idPrestamo int not null auto_increment,
    idTurno tinyint not null,
    idUsuarioRecibio smallint not null,
    idCurso tinyint,
    idDocente smallint not null,
    idCarrito tinyint,
    idEstadoPrestamo tinyint not null,
    fechaPrestamo datetime not null,
    constraint PK_Prestamos primary key (idPrestamo),
    constraint FK_Prestamos_Turnos foreign key (idTurno)
    	references Turnos(idTurno),
    constraint FK_Prestamos_UsuarioRecibio foreign key (idUsuarioRecibio)
    	references Usuarios(idUsuario),
    constraint FK_Prestamos_Docentes foreign key (idDocente)
        references Docentes(idDocente),
    constraint FK_Prestamos_Cursos foreign key (idCurso)
        references Cursos(idCurso),
    constraint FK_Prestamos_Carritos foreign key (idCarrito) 
        references Carritos(idCarrito),
    constraint FK_Prestamos_Estado foreign key (idEstadoPrestamo)
    	references EstadosPrestamo (idEstadoPrestamo)
);


create table PrestamoDetalle (
    idPrestamo int not null,
    idElemento smallint not null,
    constraint PK_PrestamoDetalle primary key (idPrestamo, idElemento),
    constraint FK_PrestamoDetalle_Prestamos foreign key (idPrestamo)
        references Prestamos(idPrestamo),
    constraint FK_PrestamoDetalle_Elementos foreign key (idElemento)
        references Elementos(idElemento)
);


create table PrestamoDetalleCambio (
    idCambio int not null auto_increment,
    idPrestamo int not null,
    idTipoAnomalia tinyint not null,
    elementoOriginal smallint not null,
    elementoReemplazo smallint not null,
    idUsuarioRegistro smallint not null,
    fechaCambio datetime not null,
    motivo varchar(255),
    constraint PK_PrestamoDetalleCambio primary key (idCambio),
    constraint FK_Cambio_Prestamo foreign key (idPrestamo) 
        references Prestamos(idPrestamo),
    constraint FK_Cambio_TipoAnomalia foreign key (idTipoAnomalia) 
        references TipoAnomalias(idTipoAnomalia),
    constraint FK_Cambio_ElementoOrig foreign key (elementoOriginal) 
        references Elementos(idElemento),
    constraint FK_Cambio_ElementoRepl foreign key (elementoReemplazo) 
        references Elementos(idElemento),
    constraint FK_Cambio_Usuario foreign key (idUsuarioRegistro) 
        references Usuarios(idUsuario),
    constraint CHK_Cambio_Distintos check (elementoOriginal != elementoReemplazo)
);


create table Devoluciones (
    idDevolucion int not null auto_increment,
    idPrestamo int not null,
    idUsuarioDevolvio smallint not null,
    fechaDevolucion datetime not null,
    carritoDevuelto boolean not null,
    observaciones varchar(200),
    constraint PK_Devoluciones primary key (idDevolucion),
    constraint FK_Devoluciones_Prestamo foreign key (idPrestamo)
        references Prestamos(idPrestamo),
    constraint FK_Devoluciones_Usuarios foreign key (idUsuarioDevolvio)
        references Usuarios(idUsuario),
    constraint UQ_Devoluciones unique (idPrestamo)
);


create table DevolucionDetalle (
    idDevolucion int not null,
    idElemento smallint not null,
    fechaDevolucion datetime not null,
    observaciones varchar(200),
    constraint PK_DevolucionDetalle primary key (idDevolucion, idElemento),
    constraint FK_DevolucionDetalle_Devoluciones foreign key (idDevolucion)
        references Devoluciones(idDevolucion),
    constraint FK_DevolucionDetalle_Elementos foreign key (idElemento)
        references Elementos(idElemento),
    constraint CHK_DevolucionDetalle_Fecha 
        check (fechaDevolucion >= (select fechaDevolucion from Devoluciones where idDevolucion = DevolucionDetalle.idDevolucion))
);


create table DevolucionAnomalia (
    idDevolucion int not null,
    idElemento smallint not null,
    idTipoAnomalia tinyint not null,
    descripcion varchar(200), 
    constraint PK_DevolucionAnomalia primary key (idDevolucion, idElemento, idTipoAnomalia),
    constraint FK_DevolucionAnomalia_DevolucionDetalle foreign key (idDevolucion, idElemento)
        references DevolucionDetalle(idDevolucion, idElemento),
    constraint FK_DevolucionAnomalia_TipoAnomalia foreign key (idTipoAnomalia)
        references TipoAnomalias(idTipoAnomalia)
);



create table TipoAccion(
idTipoAccion tinyint not null auto_increment,
accion varchar(40) not null,
constraint PK_TipoAccion primary key (idTipoAccion)
);



create table HistorialElemento(
    idHistorialElemento BIGINT not null auto_increment,
    idTipoAccion tinyint not null,
    idUsuario smallint not null,
    idElemento SMALLINT not null,
    campoModificado varchar(100) not null,
    valorAnterior varchar(255) not null,
    valorNuevo varchar(255) not null,
    fechaAccion datetime not null,
    motivo varchar(255),
    constraint PK_HistorialElemento primary key (idHistorialElemento),
    constraint FK_HistorialElemento_TipoAccion foreign key (idTipoAccion)
        references TipoAccion (idTipoAccion),
    constraint FK_HistorialElemento_Usuarios foreign key (idUsuario)
        references Usuarios (idUsuario),
    constraint FK_HistorialElemento_Elemento foreign key (idElemento)
        references Elementos (idElemento)
);


create table HistorialNotebook(
    idHistorialNotebook BIGINT not null auto_increment,
    idTipoAccion tinyint not null,
    idUsuario smallint not null,
    idElemento smallint not null,
    campoModificado varchar(100) not null,
    valorAnterior varchar(255) not null,
    valorNuevo varchar(255) not null,
    fechaAccion datetime not null,
    motivo varchar(255),
    constraint PK_HistorialNotebook primary key (idHistorialNotebook),
    constraint FK_HistorialNotebook_TipoAccion foreign key (idTipoAccion)
        references TipoAccion (idTipoAccion),
    constraint FK_HistorialNotebook_Usuarios foreign key (idUsuario)
        references Usuarios (idUsuario),
    constraint FK_HistorialNotebook_Notebook foreign key (idElemento)
        references Notebooks (idElemento)
);



create table HistorialCarrito(
    idHistorialCarrito BIGINT not null auto_increment,
    idTipoAccion tinyint not null,
    idUsuario smallint not null,
    idCarrito tinyint not null,
    campoModificado varchar(100) not null,
    valorAnterior varchar(255) not null,
    valorNuevo varchar(255) not null,
    fechaAccion datetime not null,
    motivo varchar(255),
    constraint PK_HistorialCarrito primary key (idHistorialCarrito),
    constraint FK_HistorialCarrito_TipoAccion foreign key (idTipoAccion)
        references TipoAccion (idTipoAccion),
    constraint FK_HistorialCarrito_Usuarios foreign key (idUsuario)
        references Usuarios (idUsuario),
    constraint FK_HistorialCarrito_Carrito foreign key (idCarrito)
        references Carritos (idCarrito)
);

CREATE INDEX IDX_Prestamos_FechaPrestamo ON Prestamos(fechaPrestamo);
CREATE INDEX IDX_Devoluciones_FechaDevolucion ON Devoluciones(fechaDevolucion);