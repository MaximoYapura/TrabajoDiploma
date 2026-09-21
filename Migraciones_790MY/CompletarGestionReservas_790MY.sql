USE [TP_Ing_Soft]
GO
/****** Migración 790MY: Gestion/Cancelacion de Reservas (permisos + familia) ******/

/* Permisos VerReservas (24) y CancelarReserva (25) */
SET IDENTITY_INSERT [dbo].[Permisos] ON
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (24, N'VerReservas', N'Permite consultar el listado de reservas')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (25, N'CancelarReserva', N'Permite cancelar una reserva existente')
SET IDENTITY_INSERT [dbo].[Permisos] OFF
GO

/* Familia 'Gestion de reservas' (siguiente FamiliaID real: 10) */
SET IDENTITY_INSERT [dbo].[Familias] ON
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (10, N'Gestion de reservas')
SET IDENTITY_INSERT [dbo].[Familias] OFF
GO

INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (10, 24)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (10, 25)
GO

INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 10) -- SuperAdmin
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (4, 10) -- Recepcionista
GO
