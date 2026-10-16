USE [TP_Ing_Soft]
GO
-- Empleados tiene un índice filtrado (UsuarioDNI único): exige QUOTED_IDENTIFIER ON.
-- Desde sqlcmd ejecutar con: sqlcmd -S <servidor> -E -I -f 65001 -i DatosPrueba_AtencionMesa_790MY.sql
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
/*****************************************************************************
  Datos de PRUEBA 790MY: RF2 Atención en mesa (no ejecutar en producción)

  1) Productos de ejemplo por categoría para FormRegistrarPedido_790MY.
     La categoría se guarda como texto en Productos.Categoria (Entrada,
     PlatoPrincipal, Postre, Bebida), igual que el enum CategoriaProducto_790MY;
     no existe una tabla de categorías con Id.
  2) Vincula un usuario del sistema a un empleado con puesto Camarero.
     Sin este vínculo PedidoBLL_790MY rechaza Registrar Pedido con
     "Su usuario no está vinculado a un camarero activo".
     Cambiar @UsuarioCamarero por el Username con el que se va a probar.

  Requiere haber ejecutado antes CompletarRF2_AtencionMesa_790MY.sql y
  AgregarCategoriaProducto_790MY.sql. Es idempotente.
*****************************************************************************/
GO

/* ───────────────────────── 1) Productos ───────────────────────── */
INSERT INTO [dbo].[Productos] ([Nombre], [Precio], [StockPorciones], [Activo], [Categoria])
SELECT v.Nombre, v.Precio, v.Stock, 1, v.Categoria
FROM (VALUES
    -- Entradas
    (N'Provoleta',                  7800.00, 15, N'Entrada'),
    (N'Empanada de carne',          2200.00, 60, N'Entrada'),
    (N'Rabas a la provenzal',       9400.00,  8, N'Entrada'),
    -- Platos principales
    (N'Bife de lomo con papas',    14500.00,  9, N'PlatoPrincipal'),
    (N'Ravioles de ricota',         9200.00, 14, N'PlatoPrincipal'),
    (N'Milanesa napolitana',       11200.00,  3, N'PlatoPrincipal'),   -- bajo stock
    (N'Risotto de hongos',         10900.00,  0, N'PlatoPrincipal'),   -- agotado
    -- Postres
    (N'Flan con dulce de leche',    4500.00,  3, N'Postre'),
    (N'Tiramisú',                   5200.00, 12, N'Postre'),
    -- Bebidas
    (N'Coca-Cola 500ml',            2500.00, 48, N'Bebida'),
    (N'Agua mineral',               2000.00, 80, N'Bebida'),
    (N'Copa de Malbec',             4800.00, 30, N'Bebida')
) AS v(Nombre, Precio, Stock, Categoria)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Productos] p WHERE p.Nombre = v.Nombre)
GO

/* ───────────────────────── 2) Usuario -> Camarero ───────────────────────── */
DECLARE @UsuarioCamarero nvarchar(50) = N'Maximo';
DECLARE @Dni int = (SELECT DNI FROM [dbo].[Users] WHERE Username = @UsuarioCamarero);

IF @Dni IS NULL
    PRINT 'No existe el usuario ' + @UsuarioCamarero + '; no se vinculó ningún camarero.'
ELSE IF EXISTS (SELECT 1 FROM [dbo].[Empleados] WHERE UsuarioDNI = @Dni)
    PRINT 'El usuario ' + @UsuarioCamarero + ' ya está vinculado a un empleado.'
ELSE IF EXISTS (SELECT 1 FROM [dbo].[Empleados] WHERE DNI = @Dni)
    UPDATE [dbo].[Empleados]
       SET UsuarioDNI = @Dni, Puesto = N'Camarero', Activo = 1
     WHERE DNI = @Dni
ELSE
    INSERT INTO [dbo].[Empleados] ([DNI], [Nombre], [Apellido], [Puesto], [UsuarioDNI], [Activo])
    SELECT u.DNI, u.Nombre, u.Apellido, N'Camarero', u.DNI, 1
    FROM [dbo].[Users] u WHERE u.DNI = @Dni
GO

/* Se modificaron tablas monitoreadas (Productos, Empleados): los DV guardados ya
   no corresponden. Se regeneran en el próximo Recalcular().                    */
DELETE FROM [dbo].[DV]
GO

PRINT '--- Datos de prueba RF2 cargados ---'
GO
