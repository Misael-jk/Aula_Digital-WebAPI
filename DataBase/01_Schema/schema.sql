drop database if exists aula_digital;
create database aula_digital;
use aula_digital;


create table Rol(
idRol tinyint not null auto_increment,
rol varchar(40) not null,
constraint PK_Rol primary key (idRol)
);


create table Usuarios (
    idUsuario tinyint not null auto_increment,
    usuario varchar(40) not null,
    pass varchar(70) not null,
    nombre varchar(40) not null,
    apellido varchar(40) not null,
    idRol tinyint not null,
    email varchar(70),
    fotoPerfil varchar(255),
    habilitado boolean not null,
    fechaBaja datetime,
    constraint PK_Usuarios primary key (idUsuario),
    constraint UQ_Usuarios unique (usuario),
    constraint FK_Usuarios_Rol foreign key (idRol)
    	references Rol (idRol)
);


create table Docentes (
    idDocente smallint not null auto_increment,
    dni int not null,
    nombre varchar(40) not null,
    apellido varchar(40) not null,
    email varchar(70) not null,
    habilitado boolean not null,
    fechaBaja datetime,
    constraint PK_Docentes primary key (idDocente),
    constraint UQ_Docentes_Dni unique (dni),
    constraint UQ_Docentes_Email unique (email)
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
    constraint PK_EstadosElemento primary key (idEstadoMantenimiento)
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
    Habilitado boolean not null,
    fechaBaja datetime,
    constraint PK_Carritos primary key (idCarrito),
    constraint UQ_Carritos_numeroSerieCarrito unique (equipo,numeroSerieCarrito),
    constraint FK_Carritos_Modelo foreign key (idModelo)
        references Modelo (idModelo),
    constraint FK_Carritos_EstadoMantenimiento foreign key (idEstadoMantenimiento)
    	references EstadosMantenimiento (idEstadoMantenimiento),
    constraint FK_Carritos_Ubicacion foreign key (idUbicacion)
    	references Ubicacion (idUbicacion)
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
    habilitado boolean not null,
    fechaBaja datetime,
    constraint PK_Elementos primary key (idElemento),
    constraint UQ_Elementos_numeroSerie unique (numeroSerie),
    constraint UQ_Elementos_codigoBarra unique (codigoBarra),
    constraint UQ_Elementos_patrimonio unique (patrimonio),
    constraint FK_Elementos_TipoElemento foreign key (idTipoElemento)
        references tipoElemento(idTipoElemento),
    constraint FK_Elementos_VariantesElementos foreign key (idVariante)
        references VariantesElemento (idVariante),
    constraint FK_Elementos_Modelo foreign key (idModelo)
        references Modelo (idModelo),
    constraint FK_Elementos_Ubicacion foreign key (idUbicacion)
        references Ubicacion(idUbicacion),
    constraint FK_Elementos_EstadoMantenimiento foreign key (idEstadoMantenimiento)
        references EstadosMantenimiento (idEstadoMantenimiento)
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
    idCurso tinyint auto_increment,
    curso varchar(40) not null,
    constraint PK_Cursos primary key (idCurso)
);


create table EstadosPrestamo (
    idEstadoPrestamo tinyint not null auto_increment,
    estadoPrestamo varchar(40) not null,
    constraint PK_EstadoPrestamo primary key (idEstadoPrestamo),
    constraint UQ_EstadoPrestamo unique (estadoPrestamo)
);


-- create table TipoAnomalias (
--    idTipoAnomalia tinyint not null auto_increment,
--    idTipoElemento tinyint,
--    nombreAnomalia varchar(70) not null,
--    constraint FK_TipoAnomalia primary key (idTipoAnomalia),
--    constraint UQ_TipoAnomalia_nombreAnomalia unique (nombreAnomalia),
--    constraint FK_TipoAnomalia_TipoElemento foreign key (idTipoElemento)
--        references TipoElemento (idTipoElemento)
-- );


create table Prestamos (
    idPrestamo int auto_increment,
    idUsuarioRecibio tinyint not null,
    idCurso tinyint,
    idDocente smallint not null,
    idCarrito tinyint,
    idEstadoPrestamo tinyint not null,
    fechaPrestamo datetime not null,
    constraint PK_Prestamos primary key (idPrestamo),
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


create table Devoluciones (
    idDevolucion int auto_increment,
    idPrestamo int not null,
    idUsuarioDevolvio tinyint not null,
    fechaDevolucion datetime not null,
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
    observaciones varchar(200),
    constraint PK_DevolucionDetalle primary key (idDevolucion, idElemento),
    constraint FK_DevolucionDetalle_Devoluciones foreign key (idDevolucion)
        references Devoluciones(idDevolucion),
    constraint FK_DevolucionDetalle_Elementos foreign key (idElemento)
        references Elementos(idElemento)
);


-- create table DevolucionAnomalia (
--    idDevolucion int not null,
--    idElemento smallint not null,
--    idTipoAnomalia tinyint not null,
--    descripcion varchar(200), 
--    constraint PK_DevolucionAnomalia primary key (idDevolucion, idElemento, idTipoAnomalia),
--    constraint FK_DevolucionAnomalia_DevolucionDetalle foreign key (idDevolucion, idElemento)
--        references DevolucionDetalle(idDevolucion, idElemento),
--    constraint FK_DevolucionAnomalia_TipoAnomalia foreign key (idTipoAnomalia)
--        references TipoAnomalia(idTipoAnomalia)
-- );



create table TipoAccion(
idTipoAccion tinyint not null auto_increment,
accion varchar(40) not null,
constraint PK_TipoAccion primary key (idTipoAccion)
);


create table HistorialCambio(
idHistorialCambio int not null auto_increment,
idTipoAccion tinyint not null,
idUsuario tinyint not null,
fechaCambio datetime not null,
descripcion varchar(200) not null,
motivo varchar(200),
constraint PK_HistorialCambio primary key (idHistorialCambio),
constraint FK_HistorialCambio_Accion foreign key (idTipoAccion) 
	references TipoAccion (idTipoAccion),
constraint FK_HistorialCambio_idUsuarios foreign key (idUsuario)
	references Usuarios (idUsuario)
);


create table HistorialNotebook(
idHistorialCambio int not null,
idElemento smallint not null,
constraint PK_HistorialNotebook primary key (idHistorialCambio, idElemento),
constraint FK_HistorialNotebook_HistorialCambio foreign key (idHistorialCambio)
	references HistorialCambio (idHistorialCambio),
constraint FK_HistorialNotebook_Notebook foreign key (idElemento)
	references Notebooks (idElemento)
);


create table HistorialElemento(
idHistorialCambio int not null,
idElemento smallint not null,
constraint PK_HistorialElemento primary key (idHistorialCambio, idElemento),
constraint FK_HistorialElemento_Cambio foreign key (idHistorialCambio)
	references HistorialCambio (idHistorialCambio),
constraint FK_HistorialElemento_Elemento foreign key (idElemento)
	references Elementos (idElemento)
);


create table HistorialCarrito(
idHistorialCambio int not null,
idCarrito tinyint not null,
constraint PK_HistorialCarrito primary key (idHistorialCambio, idCarrito),
constraint FK_HistorialCarrito_HistorialCambio foreign key (idHistorialCambio)
	references HistorialCambio (idHistorialCambio),
constraint FK_HistorialCambio_Carrito foreign key (idCarrito)
	references Carritos (idCarrito)
);
