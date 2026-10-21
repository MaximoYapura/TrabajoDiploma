/* =====================================================================================
   Bitácora de Cambios: versionar solo cambios de negocio (790MY) — hallazgo A5

   Antes, producto_actualizado creaba una versión nueva en Productos_C con CADA UPDATE de
   Productos, incluido cada descuento de stock de cocina: el historial se llenaba de
   versiones que solo diferían en StockPorciones.

   Nuevo comportamiento de producto_actualizado (AFTER INSERT, UPDATE ON Productos):
     - INSERT: siempre crea la versión vigente (igual que antes).
     - UPDATE con cambio en Nombre, Precio, Categoria, Activo o ControlaStock: apaga las
       versiones vigentes y crea una nueva (igual que antes).
     - UPDATE que solo cambia StockPorciones (o no cambia nada): NO crea versión; actualiza
       StockPorciones de la fila vigente (Act = 1).
     - Si el producto no tiene fila vigente (p. ej. productos de la semilla anteriores a
       los triggers) se crea una versión, para que siempre exista una.

   Rollback (ProductoCBLL_790MY.RestaurarVersion -> ActivarVersion): la versión histórica
   pasa de Act = 0 a Act = 1, producto_c_actualizado hace MERGE sobre Productos y eso
   vuelve a disparar producto_actualizado mientras hay DOS filas Act = 1 (la restaurada y
   la vigente anterior):
     - si la versión restaurada difiere en datos de negocio, se crea una versión nueva y
       las dos anteriores quedan en Act = 0 (igual que antes);
     - si difiere solo en stock (o en nada), no se crea versión: queda vigente la fila de
       mayor ID (la que ya era vigente) con el stock restaurado, y la restaurada vuelve a
       Act = 0.
   En todos los casos queda UNA sola fila Act = 1 por producto.

   Nombre y Categoria se comparan con intercalación binaria para detectar también cambios
   de mayúsculas/acentos (la base es Modern_Spanish_CI_AS).

   Requiere: AgregarBitacoraCambiosProductos_790MY.sql. Idempotente.
   ===================================================================================== */
USE [TP_Ing_Soft]
GO

SET NOCOUNT ON
GO

IF OBJECT_ID(N'[dbo].[producto_actualizado]', N'TR') IS NOT NULL
    DROP TRIGGER [dbo].[producto_actualizado]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[producto_actualizado]
ON [dbo].[Productos]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM inserted) RETURN;

    -- Productos que necesitan una versión nueva: altas, cambios de negocio o sin vigente.
    DECLARE @Versionar TABLE ([ProductoID] int NOT NULL PRIMARY KEY);

    INSERT INTO @Versionar ([ProductoID])
    SELECT i.ProductoID
    FROM inserted i
    LEFT JOIN deleted d ON d.ProductoID = i.ProductoID
    WHERE d.ProductoID IS NULL
       OR i.Nombre    COLLATE Latin1_General_BIN2 <> d.Nombre    COLLATE Latin1_General_BIN2
       OR i.Categoria COLLATE Latin1_General_BIN2 <> d.Categoria COLLATE Latin1_General_BIN2
       OR i.Precio        <> d.Precio
       OR i.Activo        <> d.Activo
       OR i.ControlaStock <> d.ControlaStock
       OR NOT EXISTS (SELECT 1 FROM [dbo].[Productos_C] c
                      WHERE c.ProductoID = i.ProductoID AND c.Act = 1);

    -- 1) Versión nueva: apaga las vigentes e inserta la nueva.
    UPDATE c
    SET c.Act = 0
    FROM [dbo].[Productos_C] c
    INNER JOIN @Versionar v ON v.ProductoID = c.ProductoID
    WHERE c.Act = 1;

    INSERT INTO [dbo].[Productos_C]
        ([ProductoID], [Nombre], [Categoria], [Precio], [StockPorciones], [ControlaStock], [Activo], [Fecha], [Act])
    SELECT i.ProductoID, i.Nombre, i.Categoria, i.Precio, i.StockPorciones, i.ControlaStock, i.Activo, GETDATE(), 1
    FROM inserted i
    INNER JOIN @Versionar v ON v.ProductoID = i.ProductoID;

    -- 2) Solo stock (o sin cambios): una única vigente, la de mayor ID. Durante un
    --    rollback la versión recién reactivada también tiene Act = 1 y vuelve a 0.
    ;WITH Vigentes AS (
        SELECT c.ID,
               ROW_NUMBER() OVER (PARTITION BY c.ProductoID ORDER BY c.ID DESC) AS Orden
        FROM [dbo].[Productos_C] c
        INNER JOIN inserted i ON i.ProductoID = c.ProductoID
        WHERE c.Act = 1
          AND NOT EXISTS (SELECT 1 FROM @Versionar v WHERE v.ProductoID = c.ProductoID)
    )
    UPDATE c
    SET c.Act = 0
    FROM [dbo].[Productos_C] c
    INNER JOIN Vigentes v ON v.ID = c.ID
    WHERE v.Orden > 1;

    UPDATE c
    SET c.StockPorciones = i.StockPorciones
    FROM [dbo].[Productos_C] c
    INNER JOIN inserted i ON i.ProductoID = c.ProductoID
    WHERE c.Act = 1
      AND c.StockPorciones <> i.StockPorciones
      AND NOT EXISTS (SELECT 1 FROM @Versionar v WHERE v.ProductoID = c.ProductoID);
END
GO

ALTER TABLE [dbo].[Productos] ENABLE TRIGGER [producto_actualizado]
GO

PRINT '--- Migración: producto_actualizado versiona solo cambios de negocio ---'
GO
