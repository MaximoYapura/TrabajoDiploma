USE [TP_Ing_Soft]
GO
/****** Migración 790MY: ABM-C de Clientes (Direccion, borrado lógico, contactos adicionales) + permiso VerMesas ******/

/* 1) Columna Direccion en Clientes */
ALTER TABLE [dbo].[Clientes] ADD [Direccion] [nvarchar](200) NULL
GO

/* 2) Tablas hijas para emails/telefonos adicionales (el Email/Telefono de Clientes
      sigue siendo el "principal", obligatorio; esto es solo para los extras). */
CREATE TABLE [dbo].[EmailsAdicionalesCliente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClienteDNI] [int] NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_EmailsAdicionalesCliente] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailsAdicionalesCliente] WITH CHECK ADD CONSTRAINT [FK_EmailsAdicionalesCliente_Clientes]
FOREIGN KEY([ClienteDNI]) REFERENCES [dbo].[Clientes] ([DNI])
GO
ALTER TABLE [dbo].[EmailsAdicionalesCliente] CHECK CONSTRAINT [FK_EmailsAdicionalesCliente_Clientes]
GO

CREATE TABLE [dbo].[TelefonosAdicionalesCliente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClienteDNI] [int] NOT NULL,
	[Telefono] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_TelefonosAdicionalesCliente] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TelefonosAdicionalesCliente] WITH CHECK ADD CONSTRAINT [FK_TelefonosAdicionalesCliente_Clientes]
FOREIGN KEY([ClienteDNI]) REFERENCES [dbo].[Clientes] ([DNI])
GO
ALTER TABLE [dbo].[TelefonosAdicionalesCliente] CHECK CONSTRAINT [FK_TelefonosAdicionalesCliente_Clientes]
GO

/* 3) Permiso VerMesas + familia + asignación (siguiente PermisoID real: 22, FamiliaID: 9) */
SET IDENTITY_INSERT [dbo].[Permisos] ON
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (22, N'VerMesas', N'Permite acceder al listado de mesas')
SET IDENTITY_INSERT [dbo].[Permisos] OFF
GO

SET IDENTITY_INSERT [dbo].[Familias] ON
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (9, N'Gestion de mesas')
SET IDENTITY_INSERT [dbo].[Familias] OFF
GO

INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (9, 22)
GO

INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 9) -- SuperAdmin
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (4, 9) -- Recepcionista
GO
