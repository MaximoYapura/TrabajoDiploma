USE [TP_Ing_Soft]
GO
/*****************************************************************************
  Migración 790MY: categoría de producto (RF2 - CUN-005 Registrar Pedido)

  FormRegistrarPedido_790MY muestra el catálogo en pestañas por sección de la
  carta (Entradas, Platos principales, Postres, Bebidas). El valor se guarda
  como texto, igual que Empleados.Puesto, y debe coincidir con el enum
  BE_08YS.CategoriaProducto_790MY.

  Ejecutar DESPUÉS de CompletarRF2_AtencionMesa_790MY.sql, con la aplicación
  cerrada. Es idempotente.
*****************************************************************************/
GO

IF COL_LENGTH(N'dbo.Productos', N'Categoria') IS NULL
BEGIN
    ALTER TABLE [dbo].[Productos]
        ADD [Categoria] [nvarchar](20) NOT NULL
            CONSTRAINT [DF_Productos_Categoria] DEFAULT (N'PlatoPrincipal')
END
GO

IF OBJECT_ID(N'dbo.CK_Productos_Categoria', N'C') IS NULL
    ALTER TABLE [dbo].[Productos] WITH CHECK ADD CONSTRAINT [CK_Productos_Categoria]
        CHECK ([Categoria] IN (N'Entrada', N'PlatoPrincipal', N'Postre', N'Bebida'))
GO

-- Clasificación de los productos semilla de la migración RF2.
UPDATE [dbo].[Productos] SET [Categoria] = N'Entrada'
WHERE [Nombre] IN (N'Provoleta', N'Empanada de carne', N'Ensalada mixta')
UPDATE [dbo].[Productos] SET [Categoria] = N'Postre'
WHERE [Nombre] IN (N'Flan con dulce de leche')
UPDATE [dbo].[Productos] SET [Categoria] = N'Bebida'
WHERE [Nombre] IN (N'Agua mineral')
GO

-- Algunos productos más para que cada pestaña tenga contenido de prueba.
INSERT INTO [dbo].[Productos] ([Nombre], [Precio], [StockPorciones], [Activo], [Categoria])
SELECT v.Nombre, v.Precio, v.Stock, 1, v.Categoria
FROM (VALUES
    (N'Milanesa napolitana',      11200.00, 3,  N'PlatoPrincipal'),
    (N'Sorrentinos de jamón y queso', 9800.00, 18, N'PlatoPrincipal'),
    (N'Risotto de hongos',        10900.00, 0,  N'PlatoPrincipal'),   -- agotado: se muestra deshabilitado
    (N'Tiramisú',                  5200.00, 12, N'Postre'),
    (N'Coca-Cola 500ml',           2500.00, 48, N'Bebida')
) AS v(Nombre, Precio, Stock, Categoria)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Productos] p WHERE p.Nombre = v.Nombre)
GO

-- Cambió la estructura de Productos (RS12 de sp_GetAllTablesParaDV): los DVH
-- guardados ya no corresponden. Se regeneran en el próximo Recalcular().
DELETE FROM [dbo].[DV]
GO

PRINT '--- Migración Categoría de producto completada ---'
GO
