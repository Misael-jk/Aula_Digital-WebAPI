-- ======================
-- ROLES
-- ======================
INSERT INTO Rol (rol) VALUES
('Administrador'),
('Encargado'),
('Invitado');

-- ======================
-- USUARIOS
-- ======================
INSERT INTO Usuarios (usuario, pass, nombre, apellido, idRol, email, FotoPerfil, habilitado) VALUES
('admin', '12345678', 'Juan', 'Perez', 1, 'juan.perez@mail.com', NULL, TRUE),
('user1', 'password123', 'Ana', 'Gomez', 2, 'ana.gomez@mail.com', NULL, TRUE),
('guest', 'et12d1', 'Luis', 'Ramirez', 3, 'luis.ramirez@mail.com', NULL, TRUE),
('operador1', 'claveOp1', 'Maria', 'Lopez', 2, 'maria.lopez@mail.com', NULL, TRUE),
('operador2', 'op2024', 'Carlos', 'Sanchez', 2, 'carlos.sanchez@mail.com', NULL, TRUE),
('superadmin', 'root1234', 'Roberto', 'Fernandez', 1, 'roberto.fernandez@mail.com', NULL, TRUE),
('tecnico1', 'tecno99', 'Sofia', 'Martinez', 2, 'sofia.martinez@mail.com', NULL, TRUE),
('soporte', 'helpme2024', 'Pedro', 'Silva', 2, 'pedro.silva@mail.com', NULL, TRUE),
('auditor', 'auditx1', 'Nicolas', 'Vega', 3, 'nicolas.vega@mail.com', NULL, TRUE),
('visitante1', 'visit123', 'Julieta', 'Mendez', 3, 'julieta.mendez@mail.com', NULL, TRUE),
('backupadmin', 'adminBK99', 'Facundo', 'Mollo', 1, 'facundo.ibarra@mail.com', NULL, TRUE),
('encargado_lab', 'lab2024', 'Lorena', 'Castillo', 2, 'lorena.castillo@mail.com', NULL, TRUE),
('nuevo_usuario', 'pass2025', 'Agustin', 'Rojas', 2, 'agustin.rojas@mail.com', NULL, TRUE);

-- ======================
-- DOCENTES
-- ======================
INSERT INTO Docentes (dni, nombre, apellido, email, habilitado) VALUES
(30123456, 'Maria', 'Fernandez', 'maria.fernandez@mail.com', TRUE),
(30123457, 'Carlos', 'Sanchez', 'carlos.sanchez@mail.com', TRUE),
(30123458, 'Pedro', 'Lopez', 'pedro.lopez@mail.com', TRUE),
(30123459, 'Laura', 'Gomez', 'laura.gomez@mail.com', TRUE),
(30123460, 'Roberto', 'Paz', 'roberto.paz@mail.com', TRUE),
(30123461, 'Sofia', 'Martinez', 'sofia.martinez2@mail.com', TRUE),
(30123462, 'Hector', 'Benitez', 'hector.benitez@mail.com', TRUE),
(30123463, 'Lucia', 'Silva', 'lucia.silva@mail.com', TRUE),
(30123464, 'Martina', 'Herrera', 'martina.herrera@mail.com', TRUE),
(30123465, 'Jorge', 'Arce', 'jorge.arce@mail.com', TRUE),
(30123466, 'Camila', 'Rivas', 'camila.rivas@mail.com', TRUE),
(30123467, 'Daniel', 'Torres', 'daniel.torres@mail.com', TRUE),
(30123468, 'Florencia', 'Medina', 'florencia.medina@mail.com', TRUE);

-- ======================
-- UBICACION
-- ======================
INSERT INTO Ubicacion (ubicacion) VALUES
('Deposito A'),
('Deposito B'),
('Aula Digital'),
('Aula de Informatica'),
('Laboratorio'),
('Sala de Mantenimiento'),
('Sector de Carga'),
('Deposito C'),
('Aula Maker'),
('Sala de Proyeccion'),
('Regencia');

-- ======================
-- TIPO ELEMENTOS
-- ======================
INSERT INTO TipoElemento (elemento) VALUES
('Notebook'),
('Carritos'),
('Proyector'),
('Tablet'),
('Teclado'),
('Mouse'),
('Cable HDMI'),
('Impresora');

-- ======================
-- MODELOS
-- ======================
INSERT INTO Modelo (idTipoElemento, modelo) VALUES
-- NOTEBOOKS
(1, 'Dell Latitude 3410'),
(1, 'HP ProBook 450'),
(1, 'Lenovo ThinkPad L14'),
(1, 'Acer Aspire 5'),

-- CARRITOS
(2, 'ENOVA'),
(2, 'WACE Carrito 32'),
(2, 'Clamshell 24 Slots'),

-- PROYECTOR
(3, 'Epson X10'),
(3, 'Epson X5'),

-- TABLETS
(4, 'Lenovo Tab M10'),
(4, 'Apple iPad 9th Gen'),
(4, 'Samsung Tab A'),

-- TECLADO
(5, 'Logitech K120'),
(5, 'Redragon K552'),

-- MAUSE
(6, 'Logitech M110'),
(6, 'Redragon Storm M808'),

-- CABLES
(7, 'HDMI 1.4 Basico'),
(7, 'HDMI 2.0 Premium'),

-- IMPRESORAS
(8, 'HP LaserJet Pro M404'),
(8, 'Epson EcoTank L3250');



-- ======================
-- VARIANTES ELEMENTO
-- ======================
INSERT INTO VariantesElemento (idTipoElemento, subtipo, idModelo) VALUES
-- Notebook
(1, 'Notebook 14" Educativa', 1),
(1, 'Notebook 15" Administrativa', 2),

-- Proyector
(3, 'Proyector Epson estandar', null),
(3, 'Proyector HD', 9),
(3, 'Proyector Portátil', NULL),

-- tablet
(4, 'Tablet Samsung 10"', 12),
(4, 'Tablet 10" WiFi', 10),
(4, 'Tablet 8" Ligera', NULL),

-- TECLADOS
(5, 'Teclado USB estandar', 13),
(5, 'Teclado mecánico compacto', 14),

-- MAUSE 
(6, 'Mouse optico basico', 15),
(6, 'Mouse gamer RGB', 16),

-- CABLE
(7, 'HDMI 1.4 1.5m', 17),
(7, 'HDMI 2.0 3m', 18),

-- IMpresora
(8, 'Impresora Láser', 19),
(8, 'Impresora Multifunción', 20);


-- ======================
-- ESTADOS MANTENIMIENTO
-- ======================
INSERT INTO EstadosMantenimiento (estadoMantenimiento) VALUES
('Disponible'),
('Prestado'),
('En reparacion'),
('Roto'),
('Faltantes'),
('Dado de Baja');

-- ======================
-- CARRITOS
-- ======================
INSERT INTO Carritos (equipo, numeroSerieCarrito, capacidad, idEstadoMantenimiento, idUbicacion, idModelo, Habilitado) VALUES
('Carro de guarda 1',           'CARR-001', 32, 1, 3, 5, TRUE), -- ENOVA
('Carro de guarda 2',           'CARR-002', 32, 1, 3, 5, TRUE), -- WACE 32
('Carro ENOVA de reserva',      'CARR-003', 32, 1, 3, 5, TRUE),
('Carro WACE 32 auxiliar',      'CARR-004', 32, 1, 3, 6, TRUE),
('Carro Clamshell 24',          'CARR-005', 24, 1, 3, 7, TRUE);

-- ======================
-- ELEMENTOS
-- ======================
INSERT INTO Elementos (idTipoElemento, idVariante, idModelo, idUbicacion, idEstadoMantenimiento, numeroSerie, codigoBarra, patrimonio, habilitado, fechaBaja)
VALUES
-- NOTEBOOKS (TipoElemento = 1)
(1, 1, 1, 3, 1, 'NB001', 'CB001', 'PAT001', TRUE, NULL),   -- Dell Latitude 3410 (variante 1)
(1, 2, 2, 3, 1, 'NB002', 'CB002', 'PAT002', TRUE, NULL),   -- HP ProBook 450 (variante 2)
(1, NULL, 3, 3, 1, 'NB003', 'CB003', 'PAT003', TRUE, NULL), -- Lenovo ThinkPad L14 (sin variante)
(1, NULL, 4, 3, 6, 'NB004', 'CB004', 'PAT004', FALSE, NOW()), -- Acer Aspire 5 (dado de baja)

-- PROYECTORES (TipoElemento = 3)
(3, 3, 8, 5, 1, 'PR001', 'CB011', 'PAT011', TRUE, NULL),   -- Proyector Epson X10 (variante estandar)
(3, 4, 9, 5, 1, 'PR002', 'CB012', 'PAT012', TRUE, NULL),   -- Proyector Epson X5 (variante HD)
(3, 5, 8, 5, 3, 'PR003', 'CB013', 'PAT013', FALSE, NOW()), -- Proyector portátil (fuera de servicio / en reparacion)

-- TABLETS (TipoElemento = 4)
(4, 6, 12, 1, 1, 'TB001', 'CB014', 'PAT014', TRUE, NULL),  -- Samsung Tab A (variante 6)
(4, 7, 10, 2, 1, 'TB002', 'CB015', 'PAT015', TRUE, NULL),  -- Lenovo Tab M10 (variante 7)
(4, 8, 11, 2, 1, 'TB003', 'CB016', 'PAT016', TRUE, NULL),  -- Apple iPad (variante genérica)

-- TECLADOS (TipoElemento = 5)
(5, 9, 13, 4, 1, 'KB001', 'CB021', 'PAT021', TRUE, NULL),   -- Logitech K120
(5, 10, 14, 4, 1, 'KB002', 'CB022', 'PAT022', TRUE, NULL),  -- Redragon K552

-- MOUSE (TipoElemento = 6)
(6, 11, 15, 4, 1, 'MS001', 'CB031', 'PAT031', TRUE, NULL),  -- Logitech M110
(6, 12, 16, 4, 1, 'MS002', 'CB032', 'PAT032', TRUE, NULL),  -- Redragon Storm

-- CABLES HDMI (TipoElemento = 7)
(7, 13, 17, 8, 1, 'HC001', 'CB041', 'PAT041', TRUE, NULL),  -- HDMI 1.4 1.5m
(7, 14, 18, 8, 1, 'HC002', 'CB042', 'PAT042', TRUE, NULL),  -- HDMI 2.0 3m

-- IMPRESORAS (TipoElemento = 8)
(8, 15, 19, 5, 1, 'PRN001', 'CB051', 'PAT051', TRUE, NULL), -- HP LaserJet
(8, 16, 20, 5, 1, 'PRN002', 'CB052', 'PAT052', TRUE, NULL); -- Epson EcoTank

-- ======================
-- NOTEBOOKS (tabla Notebooks) — asociadas a los 4 primeros elementos (idElemento 1..4)
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
('1-3'),
('2-3'),
('3-3'),
('4-8'),
('5-8'),
('6-8');

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
(1, 1, 1, 1, 2, '2025-05-01 10:00:00'),
(2, 2, 2, 2, 2, '2025-05-02 11:00:00'),
(3, 3, 4, 1, 1, '2025-07-03 09:15:00'),   -- En Préstamo
(4, 4, 5, 2, 1, '2025-07-04 08:50:00'),
(5, 5, 6, 3, 2, '2025-07-05 10:10:00'),   -- Devuelto
(6, 6, 7, 4, 2, '2025-08-06 14:40:00'),
(7, 1, 8, 5, 3, '2025-08-07 12:00:00'),   -- Cancelado
(8, 2, 9, 1, 4, '2025-08-08 11:30:00'),   -- En parcial
(9, 3, 10, 2, 1, '2025-08-09 09:20:00'),
(10, 4, 11, 3, 1, '2025-09-10 10:45:00'),
(11, 5, 12, 4, 2, '2025-01-11 13:10:00'),
(12, 6, 13, 5, 1, '2025-03-12 15:25:00');

-- ======================
-- PRESTAMO DETALLE
-- ======================
INSERT INTO PrestamoDetalle (idPrestamo, idElemento) VALUES
-- PRestamo 1
(1, 1),

-- Prestamo 2
(2, 5),

-- Prestamo 3
(3, 2),
(3, 3),

-- Prestamo 4
(4, 4),

-- Prestamo 5 (devuelto)
(5, 1),
(5, 7),

-- Prestamo 6 (devuelto)
(6, 8),
(6, 9),

-- Prestamo 7 (cancelado)
(7, 11),

-- Prestamo 8 (en parcial)
(8, 12),
(8, 13),

-- Prestamo 9
(9, 14),

-- Prestamo 10
(10, 5),
(10, 6),

-- Prestamo 11
(11, 10),

-- Prestamo 12
(12, 15),
(12, 16),
(12, 17);

-- ======================
-- DEVOLUCIONES
-- ======================
INSERT INTO Devoluciones (idPrestamo, idUsuarioDevolvio, fechaDevolucion, observaciones) VALUES
-- Ya existentes:
(1, 1, '2025-08-05 15:00:00', 'Sin daños'),
(2, 2, '2025-08-06 16:00:00', 'Con problemas en bateria'),

-- Prestamo 5 (Devuelto)
(5, 4, '2025-08-07 17:20:00', 'Notebook revisada y en buen estado'),

-- Prestamo 6 (Devuelto)
(6, 3, '2025-08-08 09:40:00', 'Equipo limpiado, sin fallas'),

-- Prestamo 8 (En parcial)
(8, 6, '2025-010-09 14:30:00', 'Devolución parcial — faltan elementos'),

-- Prestamo 11 (Devuelto)
(11, 8, '2025-11-12 11:15:00', 'Devuelto completo y funcional');

-- ======================
-- DEVOLUCION DETALLE
-- ======================
INSERT INTO DevolucionDetalle (idDevolucion, idElemento, observaciones) VALUES
-- Devolución 1 (prestamo 1)
(1, 1, NULL),

-- Devolución 2 (prestamo 2)
(2, 5, 'Devuelto con anomalias'),

-- Devolución 3 (prestamo 5)
(3, 1, NULL),
(3, 7, 'Correcto'),

-- Devolución 4 (prestamo 6)
(4, 8, NULL),
(4, 9, 'Limpiado y revisado'),

-- Devolución 5 (prestamo 8) - parcial
(5, 12, 'Devuelto — faltante el segundo elemento'),
-- El elemento 13 queda pendiente

-- Devolución 6 (prestamo 11)
(6, 10, 'Sin novedades');


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
('Modificacion'),
('Baja'),
('Habilitar'),
('Prestamo'),
('Devolución');

-- ======================
-- HISTORIAL CAMBIO (completo, con idHistorialCambio explícito)
-- ======================
INSERT INTO HistorialCambio (idHistorialCambio, idTipoAccion, idUsuario, fechaCambio, descripcion, motivo) VALUES
-- Notebooks (altas iniciales y cambios)
(1, 1, 1, '2025-09-01 08:00:00', 'Ingreso Notebook Dell Latitude 3410 - PAT001', NULL),
(2, 1, 1, '2025-09-01 08:05:00', 'Ingreso Notebook HP ProBook 450 - PAT002', NULL),
(3, 1, 2, '2025-09-01 08:10:00', 'Ingreso Notebook Lenovo ThinkPad L14 - PAT003', NULL),
(4, 1, 2, '2025-09-01 08:15:00', 'Ingreso Notebook Acer Aspire 5 - PAT004', NULL),

-- Mantenimientos y actualizaciones
(5, 2, 1, '2025-09-10 09:00:00', 'Notebook PAT001 enviada a mantenimiento preventivo', 'Mantención semestral'),
(6, 2, 2, '2025-09-10 09:10:00', 'Notebook PAT002 actualizacion BIOS y SO', 'Actualización requerida'),
(7, 2, 2, '2025-09-10 09:20:00', 'Notebook PAT003 limpieza interna', NULL),

-- Bajas / Habilitaciones de carritos
(8, 3, 1, '2025-09-12 10:00:00', 'Carrito CARR-001 dado de baja por daño estructural', 'Rota estructura'),
(9, 4, 2, '2025-09-12 13:00:00', 'Carrito CARR-002 habilitado tras reparación', 'Reparación completada'),

-- Proyectores (altas y mantenimiento)
(10, 1, 1, '2025-09-01 09:00:00', 'Ingreso Proyector Epson X10 - PAT011', NULL),
(11, 1, 1, '2025-09-01 09:05:00', 'Ingreso Proyector Epson X5 - PAT012', NULL),
(12, 2, 2, '2025-09-01 09:10:00', 'Proyector PAT013 en reparación (lámpara)', 'Lámpara quemada'),

-- Tablets (altas y reparaciones)
(13, 1, 1, '2025-09-01 10:00:00', 'Ingreso Tablet Samsung Tab A - PAT014', NULL),
(14, 1, 1, '2025-09-01 10:05:00', 'Ingreso Tablet Lenovo Tab M10 - PAT015', NULL),
(15, 2, 2, '2025-09-01 10:10:00', 'Tablet PAT016 en reparación (puerto carga)', 'Puerto dañado'),

-- Acciones relacionadas a prestamos / devoluciones para generar historial operativo
(16, 5, 3, '2025-08-03 09:17:00', 'Prestamo generado (idPrestamo=3) — Carrito 1 entregado a docente', 'Prestamo a curso 3'),
(17, 6, 4, '2025-08-05 17:25:00', 'Devolución registrada (idPrestamo=5) — elementos verificados', NULL),
(18, 5, 5, '2025-08-12 15:27:00', 'Prestamo generado (idPrestamo=12) — Carrito 5 entregado', 'Prestamo a curso 6');

-- ======================
-- HISTORIAL ELEMENTO (mapea cambios a elementos concretos)
-- ======================
INSERT INTO HistorialElemento (idHistorialCambio, idElemento) VALUES
-- Notebooks: enlazo los cambios 1..7 con elementos 1..7
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 1),
(6, 2),
(7, 3),

-- Proyectores: enlazo registros 10..12 con elementos 5..7
(10, 5),
(11, 6),
(12, 7),

-- Tablets: enlazo registros 13..15 con elementos 8..10
(13, 8),
(14, 9),
(15, 10),

-- Historial extra ligado a impresora / cables si necesitas (ejemplo)
(16, 11),
(17, 12);

-- ======================
-- HISTORIAL CARRITO (acciones referidas a carritos)
-- ======================
INSERT INTO HistorialCarrito (idHistorialCambio, idCarrito) VALUES
(8, 1),  -- Carrito CARR-001 baja
(9, 2),  -- Carrito CARR-002 habilitado
(18, 5); -- Prestamo que involucró el carrito 5 (registro 18)

-- ======================
-- HISTORIAL NOTEBOOK (registro específico para notebooks)
-- ======================
INSERT INTO HistorialNotebook (idHistorialCambio, idElemento) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 1),
(6, 2),
(7, 3);
