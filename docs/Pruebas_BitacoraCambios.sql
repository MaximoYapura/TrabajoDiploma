/* =============================================================================
   Pruebas_BitacoraCambios.sql — Trigger producto_actualizado (A5)
   -----------------------------------------------------------------------------
   SOLO sobre la base descartable TP_Ing_Soft_PRUEBA (NUNCA sobre TP_Ing_Soft).
   Requiere la base de prueba con los triggers de la Bitácora de Cambios y la
   migración VersionarSoloCambiosNegocio_790MY.sql aplicada (o el Deploy nuevo).

   Escenarios (cada uno verifica cantidad de versiones, valores de la vigente y que
   quede UNA sola fila Act = 1 por producto):
     (f) alta de producto                      -> 1 versión vigente
     (a) editar precio                         -> +1 versión
     (b) descontar stock                       -> sin versión nueva, stock de la vigente actualizado
     (b2) descuento de stock de varios productos en una sentencia y producto sin vigente
     (b3) UPDATE sin cambios                   -> sin versión nueva
     (c) rollback a versión con cambios de negocio -> +1 versión con los datos restaurados
     (d) rollback a versión que difiere solo en stock -> sin versión nueva, stock restaurado
     (e) borrado físico del producto           -> historial completo en Act = 0
   Todo corre dentro de una transacción que se revierte al final: la base de prueba
   queda como estaba.

   Resultado esperado: todas las líneas "OK" y "FALLAS: 0".
   ============================================================================= */
USE [TP_Ing_Soft_PRUEBA];
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;

DECLARE @Fallas int = 0, @P int, @P2 int, @Q int, @Versiones int, @IdOriginal int, @IdV int, @IdZ int, @Stock int;

/* Comprueba: versiones del producto, filas Act = 1 y valores de la vigente. */
DECLARE @Chk TABLE (Caso nvarchar(60), Ok bit);

/* ─── (f) Alta ─── */
INSERT INTO dbo.Productos (Nombre, Precio, StockPorciones, Activo, Categoria, ControlaStock)
VALUES (N'Prueba A5 Locro', 1000, 20, 1, N'PlatoPrincipal', 1);
SET @P = SCOPE_IDENTITY();
INSERT @Chk SELECT N'(f) alta crea 1 versión vigente',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P) = 1
          AND (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 1 THEN 1 ELSE 0 END;
SELECT @IdOriginal = ID FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1;

/* ─── (a) Editar precio ─── */
UPDATE dbo.Productos SET Precio = 1500 WHERE ProductoID = @P;
INSERT @Chk SELECT N'(a) editar precio crea versión',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P) = 2
          AND (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 1
          AND (SELECT Precio FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 1500 THEN 1 ELSE 0 END;

/* ─── (b) Descontar stock ─── */
UPDATE dbo.Productos SET StockPorciones = StockPorciones - 2 WHERE ProductoID = @P;
INSERT @Chk SELECT N'(b) descontar stock no versiona',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P) = 2
          AND (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 1
          AND (SELECT StockPorciones FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 18
          AND (SELECT Precio FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 1500 THEN 1 ELSE 0 END;

/* ─── (b2) Varios productos en una sentencia + producto sin vigente ─── */
INSERT INTO dbo.Productos (Nombre, Precio, StockPorciones, Activo, Categoria, ControlaStock)
VALUES (N'Prueba A5 Empanada', 300, 50, 1, N'Entrada', 1);
SET @P2 = SCOPE_IDENTITY();
-- Simula un producto sin versión vigente (p. ej. de la semilla, anterior a los triggers).
INSERT INTO dbo.Productos (Nombre, Precio, StockPorciones, Activo, Categoria, ControlaStock)
VALUES (N'Prueba A5 Sin vigente', 100, 10, 1, N'Postre', 1);
SET @Q = SCOPE_IDENTITY();
DELETE FROM dbo.Productos_C WHERE ProductoID = @Q;

UPDATE dbo.Productos SET StockPorciones = StockPorciones - 1 WHERE ProductoID IN (@P, @P2, @Q);
INSERT @Chk SELECT N'(b2) descuento múltiple sin versiones nuevas',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P)  = 2
          AND (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P2) = 1
          AND (SELECT StockPorciones FROM dbo.Productos_C WHERE ProductoID = @P  AND Act = 1) = 17
          AND (SELECT StockPorciones FROM dbo.Productos_C WHERE ProductoID = @P2 AND Act = 1) = 49 THEN 1 ELSE 0 END;
INSERT @Chk SELECT N'(b2) producto sin vigente recibe una versión',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @Q AND Act = 1) = 1
          AND (SELECT StockPorciones FROM dbo.Productos_C WHERE ProductoID = @Q AND Act = 1) = 9 THEN 1 ELSE 0 END;

/* ─── (b3) UPDATE sin cambios (p. ej. guardar el ABM sin modificar nada) ─── */
UPDATE dbo.Productos SET Nombre = Nombre WHERE ProductoID = @P;
INSERT @Chk SELECT N'(b3) UPDATE sin cambios no versiona',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P) = 2 THEN 1 ELSE 0 END;

/* ─── (c) Rollback a versión con cambios de negocio (precio 1000 original) ─── */
UPDATE dbo.Productos_C SET Act = 1 WHERE ID = @IdOriginal AND Act = 0;   -- = SqlProductoCRepository_790MY.ActivarVersion
INSERT @Chk SELECT N'(c) rollback de negocio crea versión restaurada',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P) = 3
          AND (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 1
          AND (SELECT Precio FROM dbo.Productos WHERE ProductoID = @P) = 1000
          AND (SELECT Precio FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 1000
          AND (SELECT Act FROM dbo.Productos_C WHERE ID = @IdOriginal) = 0 THEN 1 ELSE 0 END;
-- Comportamiento heredado (no cambiado): el MERGE del rollback también restaura el stock
-- de la versión (20), ver informe.

/* ─── (d) Rollback a versión que difiere solo en stock ─── */
SELECT @IdV = ID, @Stock = StockPorciones FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1;   -- precio 1000, stock 20
UPDATE dbo.Productos SET Precio = 1001 WHERE ProductoID = @P;                  -- versión W
UPDATE dbo.Productos SET StockPorciones = StockPorciones - 3 WHERE ProductoID = @P;  -- W: stock 17
UPDATE dbo.Productos SET Precio = 1000 WHERE ProductoID = @P;                  -- versión Z: 1000 / 17
SELECT @IdZ = ID FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1;
SELECT @Versiones = COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P;       -- 5

UPDATE dbo.Productos_C SET Act = 1 WHERE ID = @IdV AND Act = 0;                -- V difiere de Z solo en stock
INSERT @Chk SELECT N'(d) rollback solo-stock no versiona y deja 1 vigente',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P) = @Versiones
          AND (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 1
          AND (SELECT ID FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = @IdZ
          AND (SELECT StockPorciones FROM dbo.Productos_C WHERE ID = @IdZ) = @Stock
          AND (SELECT StockPorciones FROM dbo.Productos WHERE ProductoID = @P) = @Stock
          AND (SELECT Act FROM dbo.Productos_C WHERE ID = @IdV) = 0 THEN 1 ELSE 0 END;

/* ─── (e) Borrado físico ─── */
DELETE FROM dbo.Productos WHERE ProductoID = @P;
INSERT @Chk SELECT N'(e) borrado deja el historial en Act = 0',
    CASE WHEN (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P) = @Versiones
          AND (SELECT COUNT(*) FROM dbo.Productos_C WHERE ProductoID = @P AND Act = 1) = 0 THEN 1 ELSE 0 END;

/* ─── Invariante global ─── */
INSERT @Chk SELECT N'Invariante: a lo sumo 1 Act = 1 por producto',
    CASE WHEN NOT EXISTS (SELECT ProductoID FROM dbo.Productos_C WHERE Act = 1
                          GROUP BY ProductoID HAVING COUNT(*) > 1) THEN 1 ELSE 0 END;

SELECT CASE WHEN Ok = 1 THEN N'OK    ' ELSE N'FALLA ' END + Caso AS Resultado FROM @Chk;
SELECT @Fallas = COUNT(*) FROM @Chk WHERE Ok = 0;
PRINT CONCAT(N'FALLAS: ', @Fallas);

ROLLBACK TRANSACTION;
GO
