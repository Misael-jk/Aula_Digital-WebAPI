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
('admin', 'hashed_password1', 'Juan', 'Perez', 1, 'juan.perez@mail.com', NULL, TRUE),
('user1', 'hashed_password2', 'Ana', 'Gomez', 2, 'ana.gomez@mail.com', NULL, TRUE),
('guest', 'hashed_password3', 'Luis', 'Ramirez', 3, 'luis.ramirez@mail.com', NULL, TRUE);

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
('Depósito A'),
('Depósito B'),
('Sala de Profesores');

-- ======================
-- TIPO ELEMENTOS
-- ======================
INSERT INTO TipoElemento (elemento) VALUES
('Notebook'),
('Proyector'),
('Tablet');

-- ======================
-- MODELOS
-- ======================
INSERT INTO Modelo (idTipoElemento, modelo) VALUES
(1, 'Dell Latitude 3410'),
(1, 'HP ProBook 450'),
(2, 'Epson X200'),
(3, 'Samsung Tab A');

-- ======================
-- VARIANTES ELEMENTO
-- ======================
INSERT INTO VariantesElemento (idTipoElemento, subtipo, idModelo) VALUES
(1, 'Notebook Dell 14"', 1),
(1, 'Notebook HP 15"', 2),
(2, 'Proyector Epson estándar', 3),
(3, 'Tablet Samsung 10"', 4);

-- ======================
-- ESTADOS MANTENIMIENTO
-- ======================
INSERT INTO EstadosMantenimiento (estadoMantenimiento) VALUES
('Disponible'),
('En mantenimiento'),
('Prestado');

-- ======================
-- CARRITOS
-- ======================
INSERT INTO Carritos (equipo, numeroSerieCarrito, idEstadoMantenimiento, idUbicacion, idModelo, Habilitado) VALUES
('Carro de guarda 1', 'CARR-001', 1, 1, 1, TRUE),
('Carro de guarda 2', 'CARR-002', 1, 2, 1, TRUE);

-- ======================
-- ELEMENTOS
-- ======================
INSERT INTO Elementos (idTipoElemento, idVariante, idModelo, idUbicacion, idEstadoMantenimiento, numeroSerie, codigoBarra, patrimonio, habilitado)
VALUES
-- Notebooks Dell Latitude 3410
(1, 1, 1, 1, 1, 'NB001', 'CB001', 'PAT001', TRUE),
(1, 1, 1, 1, 1, 'NB002', 'CB002', 'PAT002', TRUE),

-- Notebooks HP ProBook 450
(1, 2, 2, 2, 1, 'NB003', 'CB003', 'PAT003', TRUE),
(1, 2, 2, 2, 2, 'NB004', 'CB004', 'PAT004', TRUE),

-- Proyectores (Epson X200)
(2, 3, 3, 1, 1, 'PR001', 'CB011', 'PAT011', TRUE),
(2, 3, 3, 1, 1, 'PR002', 'CB012', 'PAT012', TRUE),
(2, 3, 3, 2, 2, 'PR003', 'CB013', 'PAT013', TRUE),

-- Tablets (Samsung Tab A)
(3, 4, 4, 1, 1, 'TB001', 'CB014', 'PAT014', TRUE),
(3, 4, 4, 2, 1, 'TB002', 'CB015', 'PAT015', TRUE),
(3, 4, 4, 2, 2, 'TB003', 'CB016', 'PAT016', TRUE);

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
('Matemáticas'),
('Lengua'),
('Ciencias'),
('Historia');

-- ======================
-- ESTADOS PRESTAMO
-- ======================
INSERT INTO EstadosPrestamo (estadoPrestamo) VALUES
('Activo'),
('Finalizado'),
('Cancelado');

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
INSERT INTO HistorialCambio (idTipoAccion, idUsuario, fechaCambio, observacion) VALUES
-- Notebooks
(1, 1, '2025-09-01 08:00:00', 'Ingreso Notebook Dell A - disponible'),   -- ID 1
(1, 1, '2025-09-01 08:05:00', 'Ingreso Notebook Dell A - disponible'),   -- ID 2
(1, 2, '2025-09-01 08:10:00', 'Ingreso Notebook HP B - disponible'),     -- ID 3
(1, 2, '2025-09-01 08:15:00', 'Ingreso Notebook HP B - en mantenimiento'), -- ID 4

-- Proyectores
(1, 1, '2025-09-01 09:00:00', 'Ingreso Proyector A - disponible'),       -- ID 5
(1, 1, '2025-09-01 09:05:00', 'Ingreso Proyector A - disponible'),       -- ID 6
(1, 2, '2025-09-01 09:10:00', 'Ingreso Proyector B - en mantenimiento'), -- ID 7

-- Tablets
(1, 1, '2025-09-01 10:00:00', 'Ingreso Tablet A - disponible'),          -- ID 8
(1, 1, '2025-09-01 10:05:00', 'Ingreso Tablet A - disponible'),          -- ID 9
(1, 2, '2025-09-01 10:10:00', 'Ingreso Tablet B - en mantenimiento');    -- ID 10

-- ======================
-- HISTORIAL ELEMENTO
-- ======================
INSERT INTO HistorialElemento (idHistorialCambio, idElemento) VALUES
-- Notebooks
(1, 1),
(2, 2),
(3, 3),
(4, 4),

-- Proyectores
(5, 5),
(6, 6),
(7, 7),

-- Tablets
(8, 8),
(9, 9),
(10, 10);
