USE [TP_Ing_Soft]
GO
/****** Migración 790MY: ABM-C de Mesas (borrado lógico + permiso CrearMesa) ******/

/* 1) Columna Activo en Mesas (bit, NOT NULL, DEFAULT 1 -> backfillea las filas existentes) */
ALTER TABLE [dbo].[Mesas] ADD [Activo] [bit] NOT NULL CONSTRAINT [DF_Mesas_Activo] DEFAULT ((1))
GO

/* 2) Permiso CrearMesa (siguiente PermisoID real: 23), agregado a la familia 'Gestion de mesas' (FamiliaID 9) */
SET IDENTITY_INSERT [dbo].[Permisos] ON
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (23, N'CrearMesa', N'Permite dar de alta, modificar y eliminar mesas')
SET IDENTITY_INSERT [dbo].[Permisos] OFF
GO

INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (9, 23)
GO
