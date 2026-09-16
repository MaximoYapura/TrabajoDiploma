USE [TP_Ing_Soft]
GO
/****** Migración 790MY: completa lo que el script.sql original no incluye ******/

/* 1) Tabla Clientes */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[DNI] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Telefono] [nvarchar](30) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Clientes] ADD CONSTRAINT [DF_Clientes_Activo] DEFAULT ((1)) FOR [Activo]
GO

/* 2) Extensión del SP de integridad DVH/DVV: agrega Clientes como RS8,
      en la misma posición que se agrega en DvBLL_08YS.TablasMonitoreadas */
ALTER PROCEDURE [dbo].[sp_GetAllTablesParaDV]
AS
BEGIN
    SELECT * FROM Users;            -- RS0
    SELECT * FROM Roles;            -- RS1
    SELECT * FROM Familias;         -- RS2
    SELECT * FROM Permisos;         -- RS3
    SELECT * FROM FamiliaPermiso;   -- RS4
    SELECT * FROM FamiliaIntegrada; -- RS5
    SELECT * FROM RolFamilia;       -- RS6
    SELECT * FROM RolPermiso;       -- RS7
    SELECT * FROM Clientes;         -- RS8
END
GO

/* 3) Patentes nuevas (siguen el máximo real: PermisoID llega a 19) */
SET IDENTITY_INSERT [dbo].[Permisos] ON
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (20, N'CrearCliente', N'Permite registrar un nuevo cliente en el sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (21, N'VerClientes', N'Permite acceder al listado de clientes registrados')
SET IDENTITY_INSERT [dbo].[Permisos] OFF
GO

/* 4) Familia 'Gestion de clientes' (siguiente FamiliaID real: llega a 7) */
SET IDENTITY_INSERT [dbo].[Familias] ON
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (8, N'Gestion de clientes')
SET IDENTITY_INSERT [dbo].[Familias] OFF
GO

INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (8, 20)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (8, 21)
GO

/* 5) Rol 'Recepcionista' (siguiente RolID real: llega a 3) */
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([RolID], [Nombre]) VALUES (4, N'Recepcionista')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO

INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (4, 8)
GO
-- Nota: no se agrega fila en RolPermiso para el Rol 4, siguiendo el mismo patrón
-- que el Rol 3 ('Gestor de accesos'), que obtiene todos sus permisos vía RolFamilia.
