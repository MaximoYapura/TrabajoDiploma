USE [TP_Ing_Soft]
GO
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
/*****************************************************************************
  Migración 790MY: ABM de Empleados y Productos (Maestros, RF2)

    1) Productos.ControlaStock: los productos sin control de porciones no se
       rechazan por stock (CUN-006) ni se descuentan al despachar (CUN-008).
    2) Patente GestionProductos (debe coincidir con el enum Permisos) y familia
       "Gestion de carta", asignada a SuperAdmin.
       El ABM de empleados usa la patente CrearEmpleado ya existente.
    3) Invalidación de DV (cambia la estructura de Productos).

  Ejecutar DESPUÉS de CompletarRF2_AtencionMesa_790MY.sql y
  AgregarCategoriaProducto_790MY.sql, con la aplicación cerrada. Idempotente.
  Desde sqlcmd: sqlcmd -S <servidor> -E -I -f 65001 -i CompletarABM_EmpleadosProductos_790MY.sql
*****************************************************************************/
GO

/* ───────────────────────── 1) Productos.ControlaStock ───────────────────────── */
IF COL_LENGTH(N'dbo.Productos', N'ControlaStock') IS NULL
    ALTER TABLE [dbo].[Productos]
        ADD [ControlaStock] [bit] NOT NULL
            CONSTRAINT [DF_Productos_ControlaStock] DEFAULT ((1))
GO

/* ───────────────────────── 2) Patente, familia y rol ───────────────────────── */
INSERT INTO [dbo].[Permisos] ([Nombre], [Descripcion])
SELECT N'GestionProductos', N'Permite dar de alta, modificar y dar de baja productos de la carta'
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Permisos] WHERE Nombre = N'GestionProductos')
GO

INSERT INTO [dbo].[Familias] ([Nombre])
SELECT N'Gestion de carta'
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Familias] WHERE Nombre = N'Gestion de carta')
GO

INSERT INTO [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID])
SELECT f.FamiliaID, p.PermisoID
FROM [dbo].[Familias] f
CROSS JOIN [dbo].[Permisos] p
WHERE f.Nombre = N'Gestion de carta' AND p.Nombre = N'GestionProductos'
  AND NOT EXISTS (SELECT 1 FROM [dbo].[FamiliaPermiso] fp
                  WHERE fp.FamiliaID = f.FamiliaID AND fp.PermisoID = p.PermisoID)
GO

-- SuperAdmin (RolID 1, o por nombre).
INSERT INTO [dbo].[RolFamilia] ([RolID], [FamiliaID])
SELECT r.RolID, f.FamiliaID
FROM [dbo].[Roles] r
CROSS JOIN [dbo].[Familias] f
WHERE (r.Nombre = N'SuperAdmin' OR r.RolID = 1) AND f.Nombre = N'Gestion de carta'
  AND NOT EXISTS (SELECT 1 FROM [dbo].[RolFamilia] rf WHERE rf.RolID = r.RolID AND rf.FamiliaID = f.FamiliaID)
GO

/* ───────────────────────── 3) Invalidar DV ───────────────────────── */
DELETE FROM [dbo].[DV]
GO

PRINT '--- Migración ABM Empleados / Productos completada ---'
GO
