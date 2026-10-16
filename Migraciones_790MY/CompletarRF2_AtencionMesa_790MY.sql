USE [TP_Ing_Soft]
GO
/*****************************************************************************
  Migración 790MY: RF2 Atención en mesa
    CUN-004 Abrir Mesa (con reserva y Walk-in), CUN-005 Registrar Pedido,
    CUN-008 Despachar Pedido.

  Contenido:
    1) Tabla Empleados (desacoplada de Users; UsuarioDNI opcional y único).
    2) Reservas.CamareroDNI + FK + índice de ocupación por turno.
    3) Productos, Pedidos e ItemsPedido (stock por porciones).
    4) sp_GetAllTablesParaDV: RS11..RS14 (sincronizado con
       DvBLL_08YS.TablasMonitoreadas).
    5) Patentes, familias y roles (Composite) para los nuevos casos de uso.
    6) Datos semilla mínimos para probar (productos y empleados sin usuario).
    7) Invalidación de DV (mismo criterio que Scripts/AlterTableMesas_Mapa.sql).

  Idempotencia: cada bloque verifica la existencia antes de crear/insertar.
  Ejecutar UNA vez sobre la base TP_Ing_Soft con la aplicación cerrada.
*****************************************************************************/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ───────────────────────── 1) Empleados ───────────────────────── */
IF OBJECT_ID(N'dbo.Empleados', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Empleados](
        [DNI]        [int]           NOT NULL,
        [Nombre]     [nvarchar](100) NOT NULL,
        [Apellido]   [nvarchar](100) NOT NULL,
        [Puesto]     [nvarchar](20)  NOT NULL,   -- 'Camarero' | 'Chef' | 'Recepcionista'
        [UsuarioDNI] [int]           NULL,       -- FK opcional a Users.DNI
        [Activo]     [bit]           NOT NULL CONSTRAINT [DF_Empleados_Activo] DEFAULT ((1)),
     CONSTRAINT [PK_Empleados] PRIMARY KEY CLUSTERED ([DNI] ASC),
     CONSTRAINT [CK_Empleados_Puesto] CHECK ([Puesto] IN (N'Camarero', N'Chef', N'Recepcionista'))
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[Empleados] WITH CHECK ADD CONSTRAINT [FK_Empleados_Users]
        FOREIGN KEY([UsuarioDNI]) REFERENCES [dbo].[Users] ([DNI])

    -- Un usuario del sistema puede estar vinculado, como máximo, a un empleado.
    CREATE UNIQUE NONCLUSTERED INDEX [UX_Empleados_UsuarioDNI]
        ON [dbo].[Empleados]([UsuarioDNI]) WHERE [UsuarioDNI] IS NOT NULL
END
GO

/* ───────────────────────── 2) Reservas.CamareroDNI ───────────────────────── */
IF COL_LENGTH(N'dbo.Reservas', N'CamareroDNI') IS NULL
    ALTER TABLE [dbo].[Reservas] ADD [CamareroDNI] [int] NULL
GO
IF OBJECT_ID(N'dbo.FK_Reservas_Empleados', N'F') IS NULL
    ALTER TABLE [dbo].[Reservas] WITH CHECK ADD CONSTRAINT [FK_Reservas_Empleados]
        FOREIGN KEY([CamareroDNI]) REFERENCES [dbo].[Empleados] ([DNI])
GO
-- Estados válidos (se persisten como texto). WITH NOCHECK no valida filas viejas
-- por si existiera algún estado heredado; las nuevas escrituras sí se validan.
IF OBJECT_ID(N'dbo.CK_Reservas_Estado', N'C') IS NULL
    ALTER TABLE [dbo].[Reservas] WITH NOCHECK ADD CONSTRAINT [CK_Reservas_Estado]
        CHECK ([Estado] IN (N'Confirmada', N'Cancelada', N'Asistida', N'Finalizada'))
GO
-- Índice de la regla de ocupación (MesaNumero, Fecha, Hora): hace eficientes las
-- consultas de disponibilidad y acota los bloqueos de rango (UPDLOCK/HOLDLOCK)
-- del alta atómica a la mesa/turno involucrados.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Reservas_Mesa_Turno' AND object_id = OBJECT_ID(N'dbo.Reservas'))
    CREATE NONCLUSTERED INDEX [IX_Reservas_Mesa_Turno]
        ON [dbo].[Reservas]([MesaNumero], [Fecha], [Hora]) INCLUDE ([Estado])
GO

/* ───────────────────────── 3) Productos / Pedidos / ItemsPedido ───────────────────────── */
IF OBJECT_ID(N'dbo.Productos', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Productos](
        [ProductoID]     [int] IDENTITY(1,1) NOT NULL,
        [Nombre]         [nvarchar](100)     NOT NULL,
        [Precio]         [decimal](10, 2)    NOT NULL,
        [StockPorciones] [int]               NOT NULL,
        [Activo]         [bit]               NOT NULL CONSTRAINT [DF_Productos_Activo] DEFAULT ((1)),
     CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED ([ProductoID] ASC),
     CONSTRAINT [CK_Productos_Stock]  CHECK ([StockPorciones] >= 0),   -- nunca stock negativo
     CONSTRAINT [CK_Productos_Precio] CHECK ([Precio] >= 0)
    ) ON [PRIMARY]
END
GO

IF OBJECT_ID(N'dbo.Pedidos', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Pedidos](
        [NroPedido]       [int] IDENTITY(1,1) NOT NULL,
        [ReservaID]       [int]               NOT NULL,
        [Fecha]           [date]              NOT NULL,
        [Hora]            [time](0)           NOT NULL,
        [Estado]          [nvarchar](20)      NOT NULL,   -- 'Pendiente' | 'EnPreparacion' | 'Listo'
        [FechaFin]        [date]              NULL,
        [HoraFin]         [time](0)           NULL,
        [StockDescontado] [bit]               NOT NULL CONSTRAINT [DF_Pedidos_StockDescontado] DEFAULT ((0)),
     CONSTRAINT [PK_Pedidos] PRIMARY KEY CLUSTERED ([NroPedido] ASC),
     CONSTRAINT [CK_Pedidos_Estado] CHECK ([Estado] IN (N'Pendiente', N'EnPreparacion', N'Listo')),
     -- Coherencia de la marca de idempotencia: solo un pedido Listo tiene stock descontado.
     CONSTRAINT [CK_Pedidos_StockDescontado] CHECK (([Estado] = N'Listo' AND [StockDescontado] = 1)
                                                 OR ([Estado] <> N'Listo' AND [StockDescontado] = 0))
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[Pedidos] WITH CHECK ADD CONSTRAINT [FK_Pedidos_Reservas]
        FOREIGN KEY([ReservaID]) REFERENCES [dbo].[Reservas] ([ReservaID])

    CREATE NONCLUSTERED INDEX [IX_Pedidos_Estado] ON [dbo].[Pedidos]([Estado], [StockDescontado]) INCLUDE ([ReservaID])
END
GO

IF OBJECT_ID(N'dbo.ItemsPedido', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ItemsPedido](
        [NroPedido]      [int]            NOT NULL,
        [NroItem]        [int]            NOT NULL,
        [ProductoID]     [int]            NOT NULL,
        [Cantidad]       [int]            NOT NULL,
        [Observaciones]  [nvarchar](200)  NULL,
        [PrecioUnitario] [decimal](10, 2) NOT NULL,
     CONSTRAINT [PK_ItemsPedido] PRIMARY KEY CLUSTERED ([NroPedido] ASC, [NroItem] ASC),
     CONSTRAINT [CK_ItemsPedido_Cantidad] CHECK ([Cantidad] > 0)
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[ItemsPedido] WITH CHECK ADD CONSTRAINT [FK_ItemsPedido_Pedidos]
        FOREIGN KEY([NroPedido]) REFERENCES [dbo].[Pedidos] ([NroPedido])
    ALTER TABLE [dbo].[ItemsPedido] WITH CHECK ADD CONSTRAINT [FK_ItemsPedido_Productos]
        FOREIGN KEY([ProductoID]) REFERENCES [dbo].[Productos] ([ProductoID])

    CREATE NONCLUSTERED INDEX [IX_ItemsPedido_Producto] ON [dbo].[ItemsPedido]([ProductoID]) INCLUDE ([Cantidad])
END
GO

/* ───────────────────────── 4) SP de integridad DVH/DVV ─────────────────────────
   El orden de los result-sets DEBE coincidir con DvBLL_08YS.TablasMonitoreadas.
   RS9/RS10 (Mesas/Reservas) ya estaban en la base pero no en los scripts del repo;
   se dejan explícitos acá para que el SP quede versionado completo.            */
ALTER PROCEDURE [dbo].[sp_GetAllTablesParaDV]
AS
BEGIN
    SELECT * FROM Users;                                   -- RS0
    SELECT * FROM Roles;                                   -- RS1
    SELECT * FROM Familias;                                -- RS2
    SELECT * FROM Permisos;                                -- RS3
    SELECT * FROM FamiliaPermiso;                          -- RS4
    SELECT * FROM FamiliaIntegrada;                        -- RS5
    SELECT * FROM RolFamilia;                              -- RS6
    SELECT * FROM RolPermiso;                              -- RS7
    SELECT * FROM Clientes;                                -- RS8
    SELECT * FROM Mesas;                                   -- RS9
    SELECT * FROM Reservas;                                -- RS10
    SELECT * FROM Empleados   ORDER BY DNI;                -- RS11
    SELECT * FROM Productos   ORDER BY ProductoID;         -- RS12
    SELECT * FROM Pedidos     ORDER BY NroPedido;          -- RS13
    SELECT * FROM ItemsPedido ORDER BY NroPedido, NroItem; -- RS14
END
GO

/* ───────────────────────── 5) Patentes, familias y roles ───────────────────────── */
-- Patentes (el Nombre debe coincidir con el enum Permisos de Service_08YS).
INSERT INTO [dbo].[Permisos] ([Nombre], [Descripcion])
SELECT v.Nombre, v.Descripcion
FROM (VALUES
    (N'AbrirMesa',       N'Permite abrir mesas (con reserva o Walk-in) y asignar el camarero responsable'),
    (N'CerrarMesa',      N'Permite finalizar la atención de una mesa y liberarla'),
    (N'RegistrarPedido', N'Permite registrar pedidos de las mesas asignadas'),
    (N'DespacharPedido', N'Permite gestionar comandas de cocina y marcar pedidos como listos'),
    (N'CrearEmpleado',   N'Permite dar de alta y modificar empleados')
) AS v(Nombre, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Permisos] p WHERE p.Nombre = v.Nombre)
GO

-- Familias
INSERT INTO [dbo].[Familias] ([Nombre])
SELECT v.Nombre
FROM (VALUES (N'Atencion en salon'), (N'Gestion de pedidos'), (N'Cocina'), (N'Gestion de empleados')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Familias] f WHERE f.Nombre = v.Nombre)
GO

-- Familia -> Patentes
INSERT INTO [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID])
SELECT f.FamiliaID, p.PermisoID
FROM (VALUES
    (N'Atencion en salon',    N'AbrirMesa'),
    (N'Gestion de pedidos',   N'RegistrarPedido'),
    (N'Gestion de pedidos',   N'CerrarMesa'),
    (N'Cocina',               N'DespacharPedido'),
    (N'Gestion de empleados', N'CrearEmpleado')
) AS v(Familia, Permiso)
INNER JOIN [dbo].[Familias] f ON f.Nombre = v.Familia
INNER JOIN [dbo].[Permisos] p ON p.Nombre = v.Permiso
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[FamiliaPermiso] fp WHERE fp.FamiliaID = f.FamiliaID AND fp.PermisoID = p.PermisoID)
GO

-- Roles nuevos
INSERT INTO [dbo].[Roles] ([Nombre])
SELECT v.Nombre
FROM (VALUES (N'Camarero'), (N'Chef')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Roles] r WHERE r.Nombre = v.Nombre)
GO

-- Rol -> Familias
--   SuperAdmin (RolID 1): todas las familias nuevas.
--   Recepcionista (RolID 4): Atencion en salon (CUN-004). Ya tiene Gestion de mesas (VerMesas),
--     necesario para el mapa de FormSeleccionarMesa_790MY en el Walk-in.
--   Camarero: Gestion de pedidos.   Chef: Cocina.
INSERT INTO [dbo].[RolFamilia] ([RolID], [FamiliaID])
SELECT r.RolID, f.FamiliaID
FROM (VALUES
    (N'SuperAdmin',    N'Atencion en salon'),
    (N'SuperAdmin',    N'Gestion de pedidos'),
    (N'SuperAdmin',    N'Cocina'),
    (N'SuperAdmin',    N'Gestion de empleados'),
    (N'Recepcionista', N'Atencion en salon'),
    (N'Camarero',      N'Gestion de pedidos'),
    (N'Chef',          N'Cocina')
) AS v(Rol, Familia)
INNER JOIN [dbo].[Roles]    r ON r.Nombre = v.Rol
                              OR (v.Rol = N'SuperAdmin' AND r.RolID = 1)   -- por si el nombre real difiere
INNER JOIN [dbo].[Familias] f ON f.Nombre = v.Familia
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[RolFamilia] rf WHERE rf.RolID = r.RolID AND rf.FamiliaID = f.FamiliaID)
GO

/* ───────────────────────── 6) Datos semilla de prueba ───────────────────────── */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Productos])
BEGIN
    INSERT INTO [dbo].[Productos] ([Nombre], [Precio], [StockPorciones], [Activo]) VALUES
        (N'Bife de chorizo',       18500.00, 20, 1),
        (N'Provoleta',              7800.00, 15, 1),
        (N'Empanada de carne',      2200.00, 60, 1),
        (N'Ensalada mixta',         5200.00, 25, 1),
        (N'Flan con dulce de leche',4500.00,  3, 1),   -- stock bajo: escenario de rechazo
        (N'Agua mineral',           2000.00, 80, 1)
END
GO

-- Empleados SIN usuario asociado (escenario "empleado sin usuario").
IF NOT EXISTS (SELECT 1 FROM [dbo].[Empleados] WHERE DNI = 30000001)
    INSERT INTO [dbo].[Empleados] ([DNI], [Nombre], [Apellido], [Puesto], [UsuarioDNI], [Activo])
    VALUES (30000001, N'Lucas', N'Gomez', N'Camarero', NULL, 1)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Empleados] WHERE DNI = 30000002)
    INSERT INTO [dbo].[Empleados] ([DNI], [Nombre], [Apellido], [Puesto], [UsuarioDNI], [Activo])
    VALUES (30000002, N'Martina', N'Ruiz', N'Chef', NULL, 1)
GO

-- Empleado CON usuario asociado: reemplazar <DNI_USUARIO> por el DNI de un registro
-- existente en Users (por ejemplo el usuario con el que se probará CUN-005).
-- INSERT INTO [dbo].[Empleados] ([DNI], [Nombre], [Apellido], [Puesto], [UsuarioDNI], [Activo])
-- SELECT u.DNI, u.Nombre, u.Apellido, N'Camarero', u.DNI, 1 FROM [dbo].[Users] u WHERE u.DNI = <DNI_USUARIO>
-- GO

/* ───────────────────────── 7) Invalidar DV ─────────────────────────
   Cambió la estructura de Reservas y se agregaron tablas monitoreadas: los DV
   guardados ya no corresponden. Se borran para que el próximo Recalcular()
   (cualquier alta/modificación, o el botón Recalcular de FormInconsistenciaDB)
   los regenere con las 15 tablas.                                            */
DELETE FROM [dbo].[DV]
GO

PRINT '--- Migración RF2 Atención en mesa completada ---'
GO
