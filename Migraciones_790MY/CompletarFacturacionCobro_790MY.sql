USE [TP_Ing_Soft]
GO
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
/*****************************************************************************
  Migración 790MY: RF2 Facturación y Cobro
    CUN-006 Generar Factura, CUN-007 Registrar Pago y Arqueo / Cierre de Caja.

  Contenido:
    1) Tablas Facturas, ItemsFactura (detalle con precios congelados) y Pagos.
    2) sp_GetAllTablesParaDV: RS15..RS17 (sincronizado con
       DvBLL_08YS.TablasMonitoreadas).
    3) Patentes GenerarFactura, RegistrarCobro y GestionCaja (deben coincidir
       con el enum Permisos), familias "Facturacion y cobro" y "Gestion de caja".
       SuperAdmin recibe ambas; el rol Camarero recibe "Facturacion y cobro".
    4) Invalidación de DV (se agregan tablas monitoreadas).

  Ejecutar DESPUÉS de CompletarABM_EmpleadosProductos_790MY.sql, con la
  aplicación cerrada. Idempotente.
  Desde sqlcmd: sqlcmd -S <servidor> -E -I -f 65001 -i CompletarFacturacionCobro_790MY.sql
*****************************************************************************/
GO

/* ───────────────────────── 1) Facturas / ItemsFactura / Pagos ───────────────────────── */
IF OBJECT_ID(N'dbo.Facturas', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Facturas](
        [NroFactura]          [int] IDENTITY(1,1) NOT NULL,
        [NroReserva]          [int]               NOT NULL,   -- FK Reservas.ReservaID
        [FechaEmision]        [datetime2](0)      NOT NULL,
        [Subtotal]            [decimal](12, 2)    NOT NULL,
        [PorcentajeDescuento] [decimal](5, 2)     NOT NULL CONSTRAINT [DF_Facturas_PorcDesc] DEFAULT ((0)),
        [MontoDescuento]      [decimal](12, 2)    NOT NULL CONSTRAINT [DF_Facturas_MontoDesc] DEFAULT ((0)),
        [SubtotalNeto]        [decimal](12, 2)    NOT NULL,
        [IVA]                 [decimal](12, 2)    NOT NULL,   -- 21 % sobre SubtotalNeto
        [MontoTotal]          [decimal](12, 2)    NOT NULL,
        [Estado]              [nvarchar](20)      NOT NULL,   -- 'Abierta' | 'Pagada' | 'Anulada'
     CONSTRAINT [PK_Facturas] PRIMARY KEY CLUSTERED ([NroFactura] ASC),
     CONSTRAINT [CK_Facturas_Estado] CHECK ([Estado] IN (N'Abierta', N'Pagada', N'Anulada')),
     CONSTRAINT [CK_Facturas_Descuento] CHECK ([PorcentajeDescuento] BETWEEN 0 AND 100
                                              AND [MontoDescuento] >= 0 AND [MontoDescuento] <= [Subtotal]),
     CONSTRAINT [CK_Facturas_Importes] CHECK ([Subtotal] > 0 AND [IVA] >= 0
                                             AND [SubtotalNeto] = [Subtotal] - [MontoDescuento]
                                             AND [MontoTotal] = [SubtotalNeto] + [IVA])
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[Facturas] WITH CHECK ADD CONSTRAINT [FK_Facturas_Reservas]
        FOREIGN KEY([NroReserva]) REFERENCES [dbo].[Reservas] ([ReservaID])

    -- Como máximo una factura Abierta y una Pagada por reserva (las Anuladas se conservan).
    CREATE UNIQUE NONCLUSTERED INDEX [UX_Facturas_Reserva_Abierta]
        ON [dbo].[Facturas]([NroReserva]) WHERE [Estado] = N'Abierta'
    CREATE UNIQUE NONCLUSTERED INDEX [UX_Facturas_Reserva_Pagada]
        ON [dbo].[Facturas]([NroReserva]) WHERE [Estado] = N'Pagada'
END
GO

IF OBJECT_ID(N'dbo.ItemsFactura', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ItemsFactura](
        [NroFactura]     [int]            NOT NULL,
        [NroItem]        [int]            NOT NULL,
        [ProductoID]     [int]            NOT NULL,
        [Descripcion]    [nvarchar](100)  NOT NULL,   -- nombre congelado al emitir
        [Cantidad]       [int]            NOT NULL,
        [PrecioUnitario] [decimal](10, 2) NOT NULL,   -- precio congelado al emitir
     CONSTRAINT [PK_ItemsFactura] PRIMARY KEY CLUSTERED ([NroFactura] ASC, [NroItem] ASC),
     CONSTRAINT [CK_ItemsFactura_Cantidad] CHECK ([Cantidad] > 0)
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[ItemsFactura] WITH CHECK ADD CONSTRAINT [FK_ItemsFactura_Facturas]
        FOREIGN KEY([NroFactura]) REFERENCES [dbo].[Facturas] ([NroFactura])
    ALTER TABLE [dbo].[ItemsFactura] WITH CHECK ADD CONSTRAINT [FK_ItemsFactura_Productos]
        FOREIGN KEY([ProductoID]) REFERENCES [dbo].[Productos] ([ProductoID])
END
GO

IF OBJECT_ID(N'dbo.Pagos', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Pagos](
        [IdPago]          [int] IDENTITY(1,1) NOT NULL,
        [NroFactura]      [int]               NOT NULL,
        [FechaHoraPago]   [datetime2](0)      NOT NULL,
        [MetodoPago]      [nvarchar](20)      NOT NULL,   -- 'Efectivo' | 'Tarjeta'
        [MontoAbonado]    [decimal](12, 2)    NOT NULL,   -- total factura + propina
        [MontoRecibido]   [decimal](12, 2)    NULL,       -- solo efectivo
        [Cambio]          [decimal](12, 2)    NULL,       -- solo efectivo
        [MontoPropina]    [decimal](12, 2)    NOT NULL CONSTRAINT [DF_Pagos_Propina] DEFAULT ((0)),
        [NroTarjeta]      [nvarchar](25)      NULL,       -- SIEMPRE enmascarado (**** **** **** 1111)
        [MarcaTarjeta]    [nvarchar](20)      NULL,
        [CantidadCuotas]  [int]               NULL,
        [NroAutorizacion] [nvarchar](20)      NULL,
     CONSTRAINT [PK_Pagos] PRIMARY KEY CLUSTERED ([IdPago] ASC),
     CONSTRAINT [CK_Pagos_Metodo] CHECK ([MetodoPago] IN (N'Efectivo', N'Tarjeta')),
     CONSTRAINT [CK_Pagos_Importes] CHECK ([MontoAbonado] > 0 AND [MontoPropina] >= 0),
     CONSTRAINT [CK_Pagos_Cuotas] CHECK ([CantidadCuotas] IS NULL OR [CantidadCuotas] IN (1, 3, 6, 12)),
     CONSTRAINT [CK_Pagos_Efectivo] CHECK ([MetodoPago] <> N'Efectivo'
                                          OR ([MontoRecibido] >= [MontoAbonado] AND [Cambio] = [MontoRecibido] - [MontoAbonado])),
     CONSTRAINT [CK_Pagos_Tarjeta] CHECK ([MetodoPago] <> N'Tarjeta'
                                         OR ([NroTarjeta] IS NOT NULL AND [CantidadCuotas] IS NOT NULL AND [NroAutorizacion] IS NOT NULL))
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[Pagos] WITH CHECK ADD CONSTRAINT [FK_Pagos_Facturas]
        FOREIGN KEY([NroFactura]) REFERENCES [dbo].[Facturas] ([NroFactura])

    -- Un único pago confirmado por factura.
    CREATE UNIQUE NONCLUSTERED INDEX [UX_Pagos_Factura] ON [dbo].[Pagos]([NroFactura])
    -- Arqueo por fecha.
    CREATE NONCLUSTERED INDEX [IX_Pagos_Fecha] ON [dbo].[Pagos]([FechaHoraPago]) INCLUDE ([MetodoPago], [MontoPropina])
END
GO

/* ───────────────────────── 2) SP de integridad DVH/DVV ─────────────────────────
   El orden de los result-sets DEBE coincidir con DvBLL_08YS.TablasMonitoreadas. */
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
    SELECT * FROM Empleados    ORDER BY DNI;               -- RS11
    SELECT * FROM Productos    ORDER BY ProductoID;        -- RS12
    SELECT * FROM Pedidos      ORDER BY NroPedido;         -- RS13
    SELECT * FROM ItemsPedido  ORDER BY NroPedido, NroItem;-- RS14
    SELECT * FROM Facturas     ORDER BY NroFactura;        -- RS15
    SELECT * FROM ItemsFactura ORDER BY NroFactura, NroItem;-- RS16
    SELECT * FROM Pagos        ORDER BY IdPago;            -- RS17
END
GO

/* ───────────────────────── 3) Patentes, familias y roles ───────────────────────── */
INSERT INTO [dbo].[Permisos] ([Nombre], [Descripcion])
SELECT v.Nombre, v.Descripcion
FROM (VALUES
    (N'GenerarFactura', N'Permite generar la cuenta / factura de una mesa (CUN-006)'),
    (N'RegistrarCobro', N'Permite registrar el pago de una factura y liberar la mesa (CUN-007)'),
    (N'GestionCaja',    N'Permite consultar el arqueo y realizar el cierre de caja diario')
) AS v(Nombre, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Permisos] p WHERE p.Nombre = v.Nombre)
GO

INSERT INTO [dbo].[Familias] ([Nombre])
SELECT v.Nombre
FROM (VALUES (N'Facturacion y cobro'), (N'Gestion de caja')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Familias] f WHERE f.Nombre = v.Nombre)
GO

INSERT INTO [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID])
SELECT f.FamiliaID, p.PermisoID
FROM (VALUES
    (N'Facturacion y cobro', N'GenerarFactura'),
    (N'Facturacion y cobro', N'RegistrarCobro'),
    (N'Gestion de caja',     N'GestionCaja')
) AS v(Familia, Permiso)
INNER JOIN [dbo].[Familias] f ON f.Nombre = v.Familia
INNER JOIN [dbo].[Permisos] p ON p.Nombre = v.Permiso
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[FamiliaPermiso] fp WHERE fp.FamiliaID = f.FamiliaID AND fp.PermisoID = p.PermisoID)
GO

INSERT INTO [dbo].[RolFamilia] ([RolID], [FamiliaID])
SELECT r.RolID, f.FamiliaID
FROM (VALUES
    (N'SuperAdmin', N'Facturacion y cobro'),
    (N'SuperAdmin', N'Gestion de caja'),
    (N'Camarero',   N'Facturacion y cobro')
) AS v(Rol, Familia)
INNER JOIN [dbo].[Roles]    r ON r.Nombre = v.Rol
                              OR (v.Rol = N'SuperAdmin' AND r.RolID = 1)
INNER JOIN [dbo].[Familias] f ON f.Nombre = v.Familia
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[RolFamilia] rf WHERE rf.RolID = r.RolID AND rf.FamiliaID = f.FamiliaID)
GO

/* ───────────────────────── 4) Invalidar DV ───────────────────────── */
DELETE FROM [dbo].[DV]
GO

PRINT '--- Migración Facturación y Cobro completada ---'
GO
