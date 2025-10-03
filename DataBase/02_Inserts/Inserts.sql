-- ROL insert
INSERT INTO Rol (rol) VALUES
('Administrador'),
('Usuario'),
('Invitado');

-- USUARIOS inserts 
INSERT INTO Usuarios (usuario, pass, nombre, apellido, idRol, email, FotoPerfil, habilitado) VALUES
('admin', 'hashed_password1', 'Juan', 'Perez', 1, 'juan.perez@mail.com', NULL, TRUE),
('user1', 'hashed_password2', 'Ana', 'Gomez', 2, 'ana.gomez@mail.com', NULL, TRUE),
('guest', 'hashed_password3', 'Luis', 'Ramirez', 3, 'luis.ramirez@mail.com', NULL, TRUE);

-- DOCENTES inserts 
INSERT INTO Docentes (dni, nombre, apellido, email, habilitado) VALUES
(30123456, 'Maria', 'Fernandez', 'maria.fernandez@mail.com', TRUE),
(30123457, 'Carlos', 'Sanchez', 'carlos.sanchez@mail.com', TRUE);

-- UBICACION inserts
INSERT INTO Ubicacion (ubicacion) VALUES
('Depósito A'),
('Depósito B');

-- TIPO ELEMENTOS inserts 
INSERT INTO TipoElemento (elemento) VALUES
('Notebook'),
('Proyector'),
('Tablet');

-- MODELOS inserts
INSERT INTO Modelo (idTipoElemento, modelo) VALUES
(1, 'Dell Latitude 3410'),
(1, 'HP ProBook 450'),
(2, 'Epson X200'),
(3, 'Samsung Tab A');

-- ESTADOS MANTENIMIENTO inserts
INSERT INTO EstadosMantenimiento (estadoMantenimiento) VALUES
('Disponible'),
('En mantenimiento'),
('Prestado');

-- CARRITOS inserts
INSERT INTO Carritos (equipo, numeroSerieCarrito, idEstadoMantenimiento, idUbicacion, idModelo, Habilitado) VALUES
('Carro de guarda 1', 'CARR-001', 1, 1, 1, TRUE),
('Carro de guarda 2', 'CARR-002', 1, 2, 1, TRUE);

-- ELEMENTOS inserts
-- ELEMENTOS extra (Proyectores y Tablets que sí aparecen en la vista)
INSERT INTO Elementos (idTipoElemento, idModelo, idUbicacion, idEstadoMantenimiento, equipo, numeroSerie, codigoBarra, patrimonio, habilitado)
VALUES
-- Proyectores (modelo Epson X200)
(2, 3, 1, 1, 'Proyector A', 'PR002', 'CB005', 'PAT005', TRUE),
(2, 3, 1, 2, 'Proyector B', 'PR003', 'CB006', 'PAT006', TRUE),
(2, 3, 2, 3, 'Proyector C', 'PR004', 'CB007', 'PAT007', TRUE),

-- Tablets (modelo Samsung Tab A)
(3, 4, 2, 1, 'Tablet A', 'TB002', 'CB008', 'PAT008', TRUE),
(3, 4, 2, 1, 'Tablet B', 'TB003', 'CB009', 'PAT009', TRUE),
(3, 4, 1, 2, 'Tablet C', 'TB004', 'CB010', 'PAT010', TRUE);


-- NOTEBOOKS (ligadas a Elementos tipo Notebook)
INSERT INTO Notebooks (idElemento, idCarrito, posicionCarrito) VALUES
(1, 1, 1),
(4, 2, 2);

-- CURSOS inserts
INSERT INTO Cursos (curso) VALUES
('Matemáticas'),
('Lengua'),
('Ciencias');

-- ESTADOS PRESTAMO inserts
INSERT INTO EstadosPrestamo (estadoPrestamo) VALUES
('Activo'),
('Finalizado'),
('Cancelado');

-- PRESTAMOS inserts (ahora requieren idUsuarioRecibio)
INSERT INTO Prestamos (idUsuarioRecibio, idCurso, idDocente, idCarrito, idEstadoPrestamo, fechaPrestamo) VALUES
(1, 1, 1, 1, 1, '2025-08-01 10:00:00'),
(2, 2, 2, 2, 1, '2025-08-02 11:00:00');

-- PRESTAMOS DETALLES inserts
INSERT INTO PrestamoDetalle (idPrestamo, idElemento) VALUES
(1, 1),
(2, 3);

-- DEVOLUCIONES inserts (ajustado a nueva estructura)
INSERT INTO Devoluciones (idPrestamo, idUsuarioDevolvio, fechaDevolucion, observaciones) VALUES
(1, 1, '2025-08-05 15:00:00', 'Sin daños'),
(2, 2, '2025-08-06 16:00:00', 'Con problemas en batería');

-- DEVOLUCION DETALLE inserts 
INSERT INTO DevolucionDetalle (idDevolucion, idElemento, observaciones) VALUES
(1, 1, NULL),
(2, 3, 'Devuelto con anomalías');

-- TIPO ACCION inserts
INSERT INTO TipoAccion (accion) VALUES
('Alta'),
('Modificación'),
('Baja'),
('Préstamo'),
('Devolución');

-- Cambios de proyectores
INSERT INTO HistorialCambio (idTipoAccion, idUsuario, fechaCambio, observacion) VALUES
(1, 1, '2025-09-01 09:00:00', 'Ingreso de Proyector A - disponible'),
(1, 1, '2025-09-01 09:10:00', 'Ingreso de Proyector B - en mantenimiento'),
(1, 2, '2025-09-01 09:20:00', 'Ingreso de Proyector C - prestado'),

-- Cambios de tablets
(1, 1, '2025-09-02 10:00:00', 'Ingreso de Tablet A - disponible'),
(1, 1, '2025-09-02 10:05:00', 'Ingreso de Tablet B - disponible'),
(1, 2, '2025-09-02 10:15:00', 'Ingreso de Tablet C - en mantenimiento');

-- Relacionar con elementos
INSERT INTO HistorialElemento (idHistorialCambio, idElemento) VALUES
(7, 5), -- Proyector A
(8, 6), -- Proyector B
(9, 7), -- Proyector C
(10, 8), -- Tablet A
(11, 9), -- Tablet B
(12, 10); -- Tablet C

