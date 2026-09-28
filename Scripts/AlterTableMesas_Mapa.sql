-- =====================================================================
-- AlterTableMesas_Mapa.sql  –  Round 5: eliminación física, sin columnas espaciales
-- El Fogón del Sur – TP Ingeniería de Software
-- =====================================================================

-- 1. Eliminar columnas espaciales si aún existen (idempotente)
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Mesas') AND name = 'PosicionX')
    ALTER TABLE Mesas DROP COLUMN PosicionX;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Mesas') AND name = 'PosicionY')
    ALTER TABLE Mesas DROP COLUMN PosicionY;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Mesas') AND name = 'Ancho')
    ALTER TABLE Mesas DROP COLUMN Ancho;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Mesas') AND name = 'Alto')
    ALTER TABLE Mesas DROP COLUMN Alto;

-- 2. Limpiar reservas existentes (entorno de desarrollo / reset)
DELETE FROM Reservas;

-- 3. Limpiar mesas existentes y re-insertar exactamente 10 (IDs 1–10)
DELETE FROM Mesas;

INSERT INTO Mesas (NroMesa, Capacidad, Estado, Activo) VALUES
    (1,  2, 'Libre', 1),
    (2,  2, 'Libre', 1),
    (3,  4, 'Libre', 1),
    (4,  4, 'Libre', 1),
    (5,  4, 'Libre', 1),
    (6,  6, 'Libre', 1),
    (7,  6, 'Libre', 1),
    (8,  6, 'Libre', 1),
    (9,  8, 'Libre', 1),
    (10, 8, 'Libre', 1);

-- 4. Invalidar DV para forzar recálculo en próximo login
DELETE FROM DV;
