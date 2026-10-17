/* =====================================================================================
   Bitácora de Cambios: Auditoría de Productos (790MY)

   - Tabla espejo Productos_C: una fila por versión de cada producto. Act = 1 marca la
     versión vigente (a lo sumo una por ProductoID); el resto es historial y nunca se borra.
   - Triggers:
       producto_actualizado   (AFTER INSERT, UPDATE ON Productos)  -> nueva versión vigente.
       producto_eliminado     (AFTER DELETE ON Productos)          -> historial a Act = 0.
       producto_c_actualizado (AFTER UPDATE ON Productos_C)        -> rollback: al pasar una
         versión de Act = 0 a Act = 1 copia sus datos a Productos. Eso dispara a su vez
         producto_actualizado, que apaga todas las versiones e inserta la restaurada como
         nueva fila vigente: siempre queda un único Act = 1 por producto y el rollback
         también se puede deshacer.
     Los triggers trabajan por conjuntos (inserted/deleted pueden traer varias filas, p. ej.
     el descuento de stock de un pedido con varios ítems).
     El rollback necesita la opción de servidor "nested triggers" habilitada (default = 1).
   - Patente AuditarProductos (debe coincidir con Permisos.AuditarProductos en
     Service_08YS/Entities/Acceso/Permiso_08YS.cs), familia "Auditoría de productos"
     y asignación al rol SuperAdmin.

   Idempotente: puede ejecutarse más de una vez.
   Requiere: CompletarABM_EmpleadosProductos_790MY.sql (columnas Categoria y ControlaStock).
   ===================================================================================== */
USE [TP_Ing_Soft]
GO

SET NOCOUNT ON
GO

/* ───────────────────────── 1) Tabla espejo Productos_C ─────────────────────────
   Nombre y Categoria con el mismo largo que Productos (nvarchar(100) / nvarchar(20)
   ampliado a 50) para no truncar ninguna versión. */
IF OBJECT_ID(N'[dbo].[Productos_C]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Productos_C](
        [ID]             [int] IDENTITY(1,1) NOT NULL,
        [ProductoID]     [int]               NOT NULL,
        [Nombre]         [nvarchar](100)     NOT NULL,
        [Categoria]      [nvarchar](50)      NOT NULL,
        [Precio]         [decimal](10, 2)    NOT NULL,
        [StockPorciones] [int]               NOT NULL,
        [ControlaStock]  [bit]               NOT NULL,
        [Activo]         [bit]               NOT NULL,   -- negocio (baja lógica)
        [Fecha]          [datetime]          NOT NULL CONSTRAINT [DF_Productos_C_Fecha] DEFAULT (GETDATE()),
        [Act]            [bit]               NOT NULL CONSTRAINT [DF_Productos_C_Act] DEFAULT ((1)),   -- auditoría: versión vigente
     CONSTRAINT [PK_Productos_C] PRIMARY KEY CLUSTERED ([ID] ASC)
    ) ON [PRIMARY]

    -- La FK documenta la relación pero queda deshabilitada (NOCHECK): si estuviera
    -- activa, el borrado físico de un producto con historial fallaría antes de llegar
    -- a producto_eliminado, que justamente debe preservar ese historial.
    ALTER TABLE [dbo].[Productos_C] WITH NOCHECK ADD CONSTRAINT [FK_Productos_C_Productos]
        FOREIGN KEY([ProductoID]) REFERENCES [dbo].[Productos] ([ProductoID])
    ALTER TABLE [dbo].[Productos_C] NOCHECK CONSTRAINT [FK_Productos_C_Productos]

    CREATE NONCLUSTERED INDEX [IX_Productos_C_ProductoID_Act]
        ON [dbo].[Productos_C] ([ProductoID], [Act]) INCLUDE ([Fecha])

    CREATE NONCLUSTERED INDEX [IX_Productos_C_Fecha]
        ON [dbo].[Productos_C] ([Fecha] DESC)
END
GO

/* Carga inicial: una versión vigente por cada producto existente (solo si todavía no
   tiene ninguna, así la reejecución no duplica). */
INSERT INTO [dbo].[Productos_C]
    ([ProductoID], [Nombre], [Categoria], [Precio], [StockPorciones], [ControlaStock], [Activo], [Fecha], [Act])
SELECT p.ProductoID, p.Nombre, p.Categoria, p.Precio, p.StockPorciones, p.ControlaStock, p.Activo, GETDATE(), 1
FROM [dbo].[Productos] p
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Productos_C] c WHERE c.ProductoID = p.ProductoID)
GO

/* ───────────────────────── 2) Triggers ───────────────────────── */

/* Alta o modificación en Productos: apaga las versiones previas e inserta la nueva. */
IF OBJECT_ID(N'[dbo].[producto_actualizado]', N'TR') IS NOT NULL
    DROP TRIGGER [dbo].[producto_actualizado]
GO

CREATE TRIGGER [dbo].[producto_actualizado]
ON [dbo].[Productos]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM inserted) RETURN;

    UPDATE c
    SET c.Act = 0
    FROM [dbo].[Productos_C] c
    WHERE c.Act = 1
      AND c.ProductoID IN (SELECT i.ProductoID FROM inserted i);

    INSERT INTO [dbo].[Productos_C]
        ([ProductoID], [Nombre], [Categoria], [Precio], [StockPorciones], [ControlaStock], [Activo], [Fecha], [Act])
    SELECT i.ProductoID, i.Nombre, i.Categoria, i.Precio, i.StockPorciones, i.ControlaStock, i.Activo, GETDATE(), 1
    FROM inserted i;
END
GO

/* Borrado físico en Productos: el historial se conserva, solo deja de haber vigente. */
IF OBJECT_ID(N'[dbo].[producto_eliminado]', N'TR') IS NOT NULL
    DROP TRIGGER [dbo].[producto_eliminado]
GO

CREATE TRIGGER [dbo].[producto_eliminado]
ON [dbo].[Productos]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    UPDATE c
    SET c.Act = 0
    FROM [dbo].[Productos_C] c
    WHERE c.ProductoID IN (SELECT d.ProductoID FROM deleted d);
END
GO

/* Rollback: una versión histórica que pasa de Act = 0 a Act = 1 restaura sus datos de
   negocio en Productos. Solo reacciona a esa transición; cuando producto_actualizado
   vuelve a apagar versiones (1 -> 0) este trigger no hace nada, así que la recursión
   indirecta termina en el segundo nivel. Si en la misma sentencia se activaran varias
   versiones de un mismo producto, gana la de mayor ID. */
IF OBJECT_ID(N'[dbo].[producto_c_actualizado]', N'TR') IS NOT NULL
    DROP TRIGGER [dbo].[producto_c_actualizado]
GO

CREATE TRIGGER [dbo].[producto_c_actualizado]
ON [dbo].[Productos_C]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT UPDATE([Act]) RETURN;

    ;WITH Restauradas AS (
        SELECT i.*,
               ROW_NUMBER() OVER (PARTITION BY i.ProductoID ORDER BY i.ID DESC) AS Orden
        FROM inserted i
        INNER JOIN deleted d ON d.ID = i.ID
        WHERE i.Act = 1 AND d.Act = 0
    )
    MERGE [dbo].[Productos] AS destino
    USING (SELECT * FROM Restauradas WHERE Orden = 1) AS origen
       ON destino.ProductoID = origen.ProductoID
    WHEN MATCHED THEN
        UPDATE SET destino.Nombre         = origen.Nombre,
                   destino.Categoria      = origen.Categoria,
                   destino.Precio         = origen.Precio,
                   destino.StockPorciones = origen.StockPorciones,
                   destino.ControlaStock  = origen.ControlaStock,
                   destino.Activo         = origen.Activo;
END
GO

/* ───────────────────────── 3) Patente ───────────────────────── */
INSERT INTO [dbo].[Permisos] ([Nombre], [Descripcion])
SELECT v.Nombre, v.Descripcion
FROM (VALUES
    (N'AuditarProductos', N'Permite consultar la bitácora de cambios de productos y restaurar versiones anteriores (rollback)')
) AS v(Nombre, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Permisos] p WHERE p.Nombre = v.Nombre)
GO

/* ───────────────────────── 4) Familia ───────────────────────── */
INSERT INTO [dbo].[Familias] ([Nombre])
SELECT v.Nombre
FROM (VALUES (N'Auditoría de productos')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Familias] f WHERE f.Nombre = v.Nombre)
GO

INSERT INTO [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID])
SELECT f.FamiliaID, p.PermisoID
FROM [dbo].[Familias] f
INNER JOIN [dbo].[Permisos] p ON p.Nombre = N'AuditarProductos'
WHERE f.Nombre = N'Auditoría de productos'
  AND NOT EXISTS (SELECT 1 FROM [dbo].[FamiliaPermiso] fp WHERE fp.FamiliaID = f.FamiliaID AND fp.PermisoID = p.PermisoID)
GO

/* ───────────────────────── 5) Rol SuperAdmin ───────────────────────── */
INSERT INTO [dbo].[RolFamilia] ([RolID], [FamiliaID])
SELECT r.RolID, f.FamiliaID
FROM [dbo].[Roles] r
INNER JOIN [dbo].[Familias] f ON f.Nombre = N'Auditoría de productos'
WHERE (r.Nombre = N'SuperAdmin' OR r.RolID = 1)
  AND NOT EXISTS (SELECT 1 FROM [dbo].[RolFamilia] rf WHERE rf.RolID = r.RolID AND rf.FamiliaID = f.FamiliaID)
GO

IF NOT EXISTS (
    SELECT 1
    FROM [dbo].[Users] u
    INNER JOIN [dbo].[RolFamilia] rf ON rf.RolID = u.RolID
    INNER JOIN [dbo].[FamiliaPermiso] fp ON fp.FamiliaID = rf.FamiliaID
    INNER JOIN [dbo].[Permisos] p ON p.PermisoID = fp.PermisoID
    WHERE u.Username = N'Maximo' AND p.Nombre = N'AuditarProductos')
    RAISERROR('ATENCION: el usuario Maximo no recibe AuditarProductos (verifique que tenga el rol SuperAdmin).', 16, 1)
ELSE
    PRINT 'Usuario Maximo: patente AuditarProductos asignada vía rol SuperAdmin.'
GO

/* ───────────────────────── 6) Invalidar DV ─────────────────────────
   Permisos, Familias, FamiliaPermiso y RolFamilia cambiaron: la aplicación vuelve a
   calcular los dígitos verificadores (DVManager_08YS.Recalcular). */
DELETE FROM [dbo].[DV]
GO

PRINT '--- Migración Bitácora de Cambios de Productos completada ---'
GO
