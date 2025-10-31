-- ======================
-- ROLES
-- ======================
INSERT INTO Rol (rol) VALUES
('Administrador'),
('Usuario'),
('Invitado');

-- ======================
-- USUARIOS
-- ======================
INSERT INTO Usuarios (usuario, pass, nombre, apellido, idRol, email, FotoPerfil, habilitado) VALUES
('admin', '12345678', 'Juan', 'Perez', 1, 'juan.perez@mail.com', NULL, TRUE),
('user1', 'password123', 'Ana', 'Gomez', 2, 'ana.gomez@mail.com', NULL, TRUE),
('guest', 'et12d1', 'Luis', 'Ramirez', 3, 'luis.ramirez@mail.com', NULL, TRUE);

-- ======================
-- DOCENTES
-- ======================
INSERT INTO Docentes (dni, nombre, apellido, email, habilitado) VALUES
(30123456, 'Maria', 'Fernandez', 'maria.fernandez@mail.com', TRUE),
(30123457, 'Carlos', 'Sanchez', 'carlos.sanchez@mail.com', TRUE),
(30123458, 'Pedro', 'Lopez', 'pedro.lopez@mail.com', TRUE);

-- ======================
-- UBICACION
-- ======================
INSERT INTO Ubicacion (ubicacion) VALUES
('Deposito A'),
('Deposito B'),
('Aula Digital');

-- ======================
-- TIPO ELEMENTOS
-- ======================
INSERT INTO TipoElemento (elemento) VALUES
('Notebook'),
('Carritos'),
('Proyector'),
('Tablet');

-- ======================
-- MODELOS
-- ======================
INSERT INTO Modelo (idTipoElemento, modelo) VALUES
(1, 'Dell Latitude 3410'),
(1, 'HP ProBook 450'),
(2, 'ENOVA'),
(3, 'Samsung Tab A'),
(4, 'Epson X10');

-- ======================
-- VARIANTES ELEMENTO
-- ======================
INSERT INTO VariantesElemento (idTipoElemento, subtipo, idModelo) VALUES
(3, 'Proyector Epson estandar', 5),
(4, 'Tablet Samsung 10"', 4);

-- ======================
-- ESTADOS MANTENIMIENTO
-- ======================
INSERT INTO EstadosMantenimiento (estadoMantenimiento) VALUES
('Disponible'),
('Prestado'),
('En mantenimiento');

-- ======================
-- CARRITOS
-- ======================
INSERT INTO Carritos (equipo, numeroSerieCarrito, capacidad, idEstadoMantenimiento, idUbicacion, idModelo, Habilitado) VALUES
('Carro de guarda 1', 'CARR-001', 32, 1, 3, 3, TRUE),
('Carro de guarda 2', 'CARR-002', 32, 1, 3, 3, TRUE);

-- ======================
-- ELEMENTOS
-- ======================
INSERT INTO Elementos (idTipoElemento, idVariante, idModelo, idUbicacion, idEstadoMantenimiento, numeroSerie, codigoBarra, patrimonio, habilitado, fechaBaja)
VALUES
-- Notebooks Dell Latitude 3410
(1, null, 1, 3, 1, 'NB001', 'CB001', 'PAT001', TRUE, null),
(1, null, 1, 3, 1, 'NB002', 'CB002', 'PAT002', TRUE, null),

-- Notebooks HP ProBook 450
(1, null, 1, 3, 1, 'NB003', 'CB003', 'PAT003', TRUE, null),
(1, null, 1, 3, 3, 'NB004', 'CB004', 'PAT004', false, now()),

-- Proyectores (Epson X200)
(3, 1, 3, 2, 1, 'PR001', 'CB011', 'PAT011', TRUE, null),
(3, 1, 3, 2, 1, 'PR002', 'CB012', 'PAT012', TRUE, null),
(3, 1, 3, 2, 3, 'PR003', 'CB013', 'PAT013', false, now()),

-- Tablets (Samsung Tab A)
(4, 2, 4, 1, 1, 'TB001', 'CB014', 'PAT014', TRUE, null),
(4, 2, 4, 2, 1, 'TB002', 'CB015', 'PAT015', TRUE, null),
(4, 2, 4, 2, 3, 'TB003', 'CB016', 'PAT016', TRUE, null);

-- ======================
-- NOTEBOOKS (asociadas a carritos)
-- ======================
INSERT INTO Notebooks (idElemento, idCarrito, posicionCarrito, equipo) VALUES
(1, 1, 1, 'Notebook 1'),
(2, 1, 2, 'Notebook 2'),
(3, 2, 1, 'Notebook 3'),
(4, 2, 2, 'Notebook 4');

-- ======================
-- CURSOS
-- ======================
INSERT INTO Cursos (curso) VALUES
('1°3'),
('2°3'),
('3°3'),
('4°8');

-- ======================
-- ESTADOS PRESTAMO
-- ======================
INSERT INTO EstadosPrestamo (estadoPrestamo) VALUES
('En Prestamo'),
('Devuelto'),
('Cancelado'),
('En Parcial');


-- ======================
-- TIPO ANOMALIAS
-- ======================
-- INSERT INTO TipoAnomalias (idTipoElemento, nombreAnomalia) VALUES
-- Notebooks
-- (1, 'Pantalla rota'),
-- (1, 'Batería dañada'),
-- (1, 'Teclas faltantes'),
-- (1, 'No enciende'),
-- Proyectores
-- (2, 'Lámpara quemada'),
-- (2, 'Problemas de enfoque'),
-- (2, 'Sin señal HDMI'),
-- Tablets
-- (3, 'Pantalla táctil no responde'),
-- (3, 'Puerto de carga dañado'),
-- (3, 'Carcasa rota');


-- ======================
-- PRESTAMOS
-- ======================
INSERT INTO Prestamos (idUsuarioRecibio, idCurso, idDocente, idCarrito, idEstadoPrestamo, fechaPrestamo) VALUES
(1, 1, 1, 1, 1, '2025-08-01 10:00:00'),
(2, 2, 2, 2, 1, '2025-08-02 11:00:00');

-- ======================
-- PRESTAMO DETALLE
-- ======================
INSERT INTO PrestamoDetalle (idPrestamo, idElemento) VALUES
(1, 1),
(2, 5);

-- ======================
-- DEVOLUCIONES
-- ======================
INSERT INTO Devoluciones (idPrestamo, idUsuarioDevolvio, fechaDevolucion, observaciones) VALUES
(1, 1, '2025-08-05 15:00:00', 'Sin daños'),
(2, 2, '2025-08-06 16:00:00', 'Con problemas en batería');

-- ======================
-- DEVOLUCION DETALLE
-- ======================
INSERT INTO DevolucionDetalle (idDevolucion, idElemento, observaciones) VALUES
(1, 1, NULL),
(2, 5, 'Devuelto con anomalías');


-- ======================
-- DEVOLUCION ANOMALIA
-- ======================
-- INSERT INTO DevolucionAnomalia (idDevolucion, idElemento, idTipoAnomalia, descripcion) VALUES
--  Devolución 1 → Sin anomalías, no se agrega nada
--  Devolución 2 → Problemas con la Notebook 3 (de idElemento 5 es un proyector en tu caso)
-- (2, 5, 5, 'El proyector devuelve con la lámpara quemada y necesita reemplazo'),
--  Simulamos que otro elemento del mismo préstamo también tuvo un problema
-- (2, 5, 7, 'Además, no detecta entrada HDMI correctamente');


-- ======================
-- TIPO ACCION
-- ======================
INSERT INTO TipoAccion (accion) VALUES
('Alta'),
('Modificación'),
('Baja'),
('Préstamo'),
('Devolución');

-- ======================
-- HISTORIAL CAMBIO
-- ======================
INSERT INTO HistorialCambio (idTipoAccion, idUsuario, fechaCambio, descripcion, motivo) VALUES
-- Notebooks
(1, 1, '2025-09-01 08:00:00', 'Ingreso Notebook Dell A - disponible', null),   -- ID 1
(1, 1, '2025-09-01 08:05:00', 'Ingreso Notebook Dell A - disponible', null),   -- ID 2
(1, 2, '2025-09-01 08:10:00', 'Ingreso Notebook HP B - disponible', null),     -- ID 3
(1, 2, '2025-09-01 08:15:00', 'Ingreso Notebook HP B - en mantenimiento', null), -- ID 4
(2, 1, '2025-09-10 09:00:00', 'Notebook 1 enviada a mantenimiento', null),
(2, 2, '2025-09-10 09:10:00', 'Notebook 2 con actualización de sistema', null),
(2, 2, '2025-09-10 09:20:00', 'Notebook 3 con limpieza interna', null),

-- Baja de un carrito
(3, 1, '2025-09-12 10:00:00', 'Carrito 1 dado de baja por rotura', null),
(3, 2, '2025-09-12 13:00:00', 'Carrito 2 dado de baja por Algooo', null),

-- Proyectores
(1, 1, '2025-09-01 09:00:00', 'Ingreso Proyector A - disponible', null),       -- ID 5
(1, 1, '2025-09-01 09:05:00', 'Ingreso Proyector A - disponible', null),       -- ID 6
(2, 2, '2025-09-01 09:10:00', 'Ingreso Proyector B - en mantenimiento', null), -- ID 7

-- Tablets
(1, 1, '2025-09-01 10:00:00', 'Ingreso Tablet A - disponible', null),          -- ID 8
(1, 1, '2025-09-01 10:05:00', 'Ingreso Tablet A - disponible', null),          -- ID 9
(2, 2, '2025-09-01 10:10:00', 'Ingreso Tablet B - en mantenimiento', null);    -- ID 10

-- ======================
-- HISTORIAL ELEMENTO
-- ======================
INSERT INTO HistorialElemento (idHistorialCambio, idElemento) VALUES
-- Notebooks
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 1),
(6, 2),
(7, 3),

-- Proyectores
(10, 5),
(11, 6),
(12, 7),

-- Tablets
(13, 8),
(14, 9),
(15, 10);

INSERT INTO HistorialCarrito (idHistorialCambio, idCarrito) VALUES
-- Carritos
(8, 1),
(9, 2);

Insert into HistorialNotebook (idHistorialCambio, idElemento) values
-- Notebooks
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 1),
(6, 2),
(7, 3);