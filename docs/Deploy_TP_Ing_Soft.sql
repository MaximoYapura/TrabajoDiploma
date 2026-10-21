/* =============================================================================
   Deploy_TP_Ing_Soft.sql — Despliegue completo de "El Fogón del Sur"
   -----------------------------------------------------------------------------
   Script ÚNICO para una PC limpia. Reemplaza a script.sql + Migraciones_790MY +
   Seed_TP_Ing_Soft.sql: contiene el esquema final unificado (incluido RF2:
   pedidos, facturación y cobro) y los datos iniciales.

     1. Crea la base TP_Ing_Soft si no existe (intercalación Modern_Spanish_CI_AS).
     2. Crea tipos, tablas, restricciones, índices, claves foráneas, triggers y
        procedimientos almacenados con su estructura final.
     3. Inserta los datos semilla limpios:
          - Seguridad: Roles, Permisos, Familias y sus relaciones.
          - Usuario Maximo (SuperAdmin).
          - 1 cliente de prueba, 4 empleados (uno de cada puesto), Mesas y el
            catálogo de Productos.
        Sin reservas, pedidos, facturas, pagos ni bitácora.
     4. Inserta los Dígitos Verificadores (DVH/DVV) correctos para esos datos:
        el primer login no informa inconsistencias.

   SEGURO DE RE-EJECUTAR
     - Si TP_Ing_Soft ya existe con su esquema (dbo.Users), NO se modifica nada.
     - Si existe pero está vacía (p. ej. un despliegue anterior falló), se crea
       todo dentro de ella.
     - El esquema y los datos se crean en una única transacción: ante cualquier
       error no queda una base a medio crear.

   El instalador busca en la salida las marcas DEPLOY_OK / DEPLOY_OMITIDO.

   Ejecución manual:
       sqlcmd -S . -E -b -f 65001 -i Deploy_TP_Ing_Soft.sql

   Generado desde una base semillada con SMO (esquema y datos reales); para
   regenerarlo, aplicar Seed_TP_Ing_Soft.sql sobre una copia y volver a scriptear.
   ============================================================================= */

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
SET QUOTED_IDENTIFIER ON;
GO

/* ─── 1) Base de datos ───────────────────────────────────────────────────── */
USE [master];
GO
IF DB_ID(N'TP_Ing_Soft') IS NULL
BEGIN
    CREATE DATABASE [TP_Ing_Soft] COLLATE Modern_Spanish_CI_AS;
    PRINT N'Base TP_Ing_Soft creada.';
END
GO

USE [TP_Ing_Soft];
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
BEGIN
    PRINT N'TP_Ing_Soft ya tiene su esquema: no se modifica (se conservan sus datos).';
    PRINT N'DEPLOY_OMITIDO';
    SET NOEXEC ON;
END
GO

BEGIN TRANSACTION;
GO

/* ─── 2) Tipos, tablas, restricciones e índices ──────────────────────────── */
CREATE TYPE [dbo].[IntList] AS TABLE(
	[ID] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bitacora](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[Modulo] [int] NOT NULL,
	[Evento] [int] NOT NULL,
	[Criticidad] [int] NOT NULL,
	[TargetUsername] [nvarchar](50) NULL,
 CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
ALTER TABLE [dbo].[Bitacora] ADD  CONSTRAINT [DF_Bitacora_Fecha]  DEFAULT (getdate()) FOR [FechaHora]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Cierres de caja (A4): una fila por jornada; la PK impide el doble cierre.
-- No forma parte de los dígitos verificadores (TablasMonitoreadas).
CREATE TABLE [dbo].[CierresCaja](
	[Fecha] [date] NOT NULL,
	[Neta] [decimal](12, 2) NOT NULL,
	[CantidadCuentas] [int] NOT NULL,
	[Usuario] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CierresCaja] PRIMARY KEY CLUSTERED
(
	[Fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
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
	[Direccion] [nvarchar](200) NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
ALTER TABLE [dbo].[Clientes] ADD  CONSTRAINT [DF_Clientes_Activo]  DEFAULT ((1)) FOR [Activo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DV](
	[Tabla] [nvarchar](100) NOT NULL,
	[DVH] [nvarchar](max) NOT NULL,
	[DVV] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Tabla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailsAdicionalesCliente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClienteDNI] [int] NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_EmailsAdicionalesCliente] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleados](
	[DNI] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[Puesto] [nvarchar](20) NOT NULL,
	[UsuarioDNI] [int] NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Empleados] PRIMARY KEY CLUSTERED 
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Empleados_UsuarioDNI] ON [dbo].[Empleados]
(
	[UsuarioDNI] ASC
)
WHERE ([UsuarioDNI] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
ALTER TABLE [dbo].[Empleados] ADD  CONSTRAINT [DF_Empleados_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Empleados]  WITH CHECK ADD  CONSTRAINT [CK_Empleados_Puesto] CHECK  (([Puesto]=N'Recepcionista' OR [Puesto]=N'Chef' OR [Puesto]=N'Camarero'))
GO
ALTER TABLE [dbo].[Empleados] CHECK CONSTRAINT [CK_Empleados_Puesto]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facturas](
	[NroFactura] [int] IDENTITY(1,1) NOT NULL,
	[NroReserva] [int] NOT NULL,
	[FechaEmision] [datetime2](0) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
	[PorcentajeDescuento] [decimal](5, 2) NOT NULL,
	[MontoDescuento] [decimal](12, 2) NOT NULL,
	[SubtotalNeto] [decimal](12, 2) NOT NULL,
	[IVA] [decimal](12, 2) NOT NULL,
	[MontoTotal] [decimal](12, 2) NOT NULL,
	[Estado] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Facturas] PRIMARY KEY CLUSTERED 
(
	[NroFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Facturas_Reserva_Abierta] ON [dbo].[Facturas]
(
	[NroReserva] ASC
)
WHERE ([Estado]=N'Abierta')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Facturas_Reserva_Pagada] ON [dbo].[Facturas]
(
	[NroReserva] ASC
)
WHERE ([Estado]=N'Pagada')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
ALTER TABLE [dbo].[Facturas] ADD  CONSTRAINT [DF_Facturas_PorcDesc]  DEFAULT ((0)) FOR [PorcentajeDescuento]
GO
ALTER TABLE [dbo].[Facturas] ADD  CONSTRAINT [DF_Facturas_MontoDesc]  DEFAULT ((0)) FOR [MontoDescuento]
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD  CONSTRAINT [CK_Facturas_Descuento] CHECK  (([PorcentajeDescuento]>=(0) AND [PorcentajeDescuento]<=(100) AND [MontoDescuento]>=(0) AND [MontoDescuento]<=[Subtotal]))
GO
ALTER TABLE [dbo].[Facturas] CHECK CONSTRAINT [CK_Facturas_Descuento]
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD  CONSTRAINT [CK_Facturas_Estado] CHECK  (([Estado]=N'Anulada' OR [Estado]=N'Pagada' OR [Estado]=N'Abierta'))
GO
ALTER TABLE [dbo].[Facturas] CHECK CONSTRAINT [CK_Facturas_Estado]
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD  CONSTRAINT [CK_Facturas_Importes] CHECK  (([Subtotal]>(0) AND [IVA]>=(0) AND [SubtotalNeto]=([Subtotal]-[MontoDescuento]) AND [MontoTotal]=([SubtotalNeto]+[IVA])))
GO
ALTER TABLE [dbo].[Facturas] CHECK CONSTRAINT [CK_Facturas_Importes]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FamiliaIntegrada](
	[FamiliaPadreID] [int] NOT NULL,
	[FamiliaID] [int] NOT NULL,
 CONSTRAINT [PK_FamiliaIntegrada] PRIMARY KEY CLUSTERED 
(
	[FamiliaID] ASC,
	[FamiliaPadreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
ALTER TABLE [dbo].[FamiliaIntegrada]  WITH CHECK ADD  CONSTRAINT [CHK_NoAutorreferencia] CHECK  (([FamiliaID]<>[FamiliaPadreID]))
GO
ALTER TABLE [dbo].[FamiliaIntegrada] CHECK CONSTRAINT [CHK_NoAutorreferencia]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FamiliaPermiso](
	[FamiliaID] [int] NOT NULL,
	[PermisoID] [int] NOT NULL,
 CONSTRAINT [PK_FamiliaPermiso] PRIMARY KEY CLUSTERED 
(
	[FamiliaID] ASC,
	[PermisoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familias](
	[FamiliaID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FamiliaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemsFactura](
	[NroFactura] [int] NOT NULL,
	[NroItem] [int] NOT NULL,
	[ProductoID] [int] NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_ItemsFactura] PRIMARY KEY CLUSTERED 
(
	[NroFactura] ASC,
	[NroItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
ALTER TABLE [dbo].[ItemsFactura]  WITH CHECK ADD  CONSTRAINT [CK_ItemsFactura_Cantidad] CHECK  (([Cantidad]>(0)))
GO
ALTER TABLE [dbo].[ItemsFactura] CHECK CONSTRAINT [CK_ItemsFactura_Cantidad]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemsPedido](
	[NroPedido] [int] NOT NULL,
	[NroItem] [int] NOT NULL,
	[ProductoID] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Observaciones] [nvarchar](200) NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_ItemsPedido] PRIMARY KEY CLUSTERED 
(
	[NroPedido] ASC,
	[NroItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_ItemsPedido_Producto] ON [dbo].[ItemsPedido]
(
	[ProductoID] ASC
)
INCLUDE([Cantidad]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
ALTER TABLE [dbo].[ItemsPedido]  WITH CHECK ADD  CONSTRAINT [CK_ItemsPedido_Cantidad] CHECK  (([Cantidad]>(0)))
GO
ALTER TABLE [dbo].[ItemsPedido] CHECK CONSTRAINT [CK_ItemsPedido_Cantidad]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mesas](
	[NroMesa] [int] NOT NULL,
	[Capacidad] [int] NOT NULL,
	[Estado] [nvarchar](20) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Mesas] PRIMARY KEY CLUSTERED 
(
	[NroMesa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
ALTER TABLE [dbo].[Mesas] ADD  CONSTRAINT [DF_Mesas_Activo]  DEFAULT ((1)) FOR [Activo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pagos](
	[IdPago] [int] IDENTITY(1,1) NOT NULL,
	[NroFactura] [int] NOT NULL,
	[FechaHoraPago] [datetime2](0) NOT NULL,
	[MetodoPago] [nvarchar](20) NOT NULL,
	[MontoAbonado] [decimal](12, 2) NOT NULL,
	[MontoRecibido] [decimal](12, 2) NULL,
	[Cambio] [decimal](12, 2) NULL,
	[MontoPropina] [decimal](12, 2) NOT NULL,
	[NroTarjeta] [nvarchar](25) NULL,
	[MarcaTarjeta] [nvarchar](20) NULL,
	[CantidadCuotas] [int] NULL,
	[NroAutorizacion] [nvarchar](20) NULL,
 CONSTRAINT [PK_Pagos] PRIMARY KEY CLUSTERED 
(
	[IdPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_Pagos_Fecha] ON [dbo].[Pagos]
(
	[FechaHoraPago] ASC
)
INCLUDE([MetodoPago],[MontoPropina]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Pagos_Factura] ON [dbo].[Pagos]
(
	[NroFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
ALTER TABLE [dbo].[Pagos] ADD  CONSTRAINT [DF_Pagos_Propina]  DEFAULT ((0)) FOR [MontoPropina]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Cuotas] CHECK  (([CantidadCuotas] IS NULL OR ([CantidadCuotas]=(12) OR [CantidadCuotas]=(6) OR [CantidadCuotas]=(3) OR [CantidadCuotas]=(1))))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Cuotas]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Efectivo] CHECK  (([MetodoPago]<>N'Efectivo' OR [MontoRecibido]>=[MontoAbonado] AND [Cambio]=([MontoRecibido]-[MontoAbonado])))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Efectivo]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Importes] CHECK  (([MontoAbonado]>(0) AND [MontoPropina]>=(0)))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Importes]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Metodo] CHECK  (([MetodoPago]=N'Tarjeta' OR [MetodoPago]=N'Efectivo'))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Metodo]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Tarjeta] CHECK  (([MetodoPago]<>N'Tarjeta' OR [NroTarjeta] IS NOT NULL AND [CantidadCuotas] IS NOT NULL AND [NroAutorizacion] IS NOT NULL))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Tarjeta]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedidos](
	[NroPedido] [int] IDENTITY(1,1) NOT NULL,
	[ReservaID] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[Hora] [time](0) NOT NULL,
	[Estado] [nvarchar](20) NOT NULL,
	[FechaFin] [date] NULL,
	[HoraFin] [time](0) NULL,
	[StockDescontado] [bit] NOT NULL,
 CONSTRAINT [PK_Pedidos] PRIMARY KEY CLUSTERED 
(
	[NroPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_Pedidos_Estado] ON [dbo].[Pedidos]
(
	[Estado] ASC,
	[StockDescontado] ASC
)
INCLUDE([ReservaID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
ALTER TABLE [dbo].[Pedidos] ADD  CONSTRAINT [DF_Pedidos_StockDescontado]  DEFAULT ((0)) FOR [StockDescontado]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [CK_Pedidos_Estado] CHECK  (([Estado]=N'Listo' OR [Estado]=N'EnPreparacion' OR [Estado]=N'Pendiente'))
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [CK_Pedidos_Estado]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [CK_Pedidos_StockDescontado] CHECK  (([Estado]=N'Listo' AND [StockDescontado]=(1) OR [Estado]<>N'Listo' AND [StockDescontado]=(0)))
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [CK_Pedidos_StockDescontado]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permisos](
	[PermisoID] [int] IDENTITY(0,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](200) NULL,
 CONSTRAINT [PK__Permisos__96E0C703D1806354] PRIMARY KEY CLUSTERED 
(
	[PermisoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos](
	[ProductoID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[StockPorciones] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Categoria] [nvarchar](20) NOT NULL,
	[ControlaStock] [bit] NOT NULL,
 CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED 
(
	[ProductoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
ALTER TABLE [dbo].[Productos] ADD  CONSTRAINT [DF_Productos_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Productos] ADD  CONSTRAINT [DF_Productos_Categoria]  DEFAULT (N'PlatoPrincipal') FOR [Categoria]
GO
ALTER TABLE [dbo].[Productos] ADD  CONSTRAINT [DF_Productos_ControlaStock]  DEFAULT ((1)) FOR [ControlaStock]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [CK_Productos_Categoria] CHECK  (([Categoria]=N'Bebida' OR [Categoria]=N'Postre' OR [Categoria]=N'PlatoPrincipal' OR [Categoria]=N'Entrada'))
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [CK_Productos_Categoria]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [CK_Productos_Precio] CHECK  (([Precio]>=(0)))
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [CK_Productos_Precio]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [CK_Productos_Stock] CHECK  (([StockPorciones]>=(0)))
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [CK_Productos_Stock]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos_C](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductoID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Categoria] [nvarchar](50) NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[StockPorciones] [int] NOT NULL,
	[ControlaStock] [bit] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Act] [bit] NOT NULL,
 CONSTRAINT [PK_Productos_C] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_Productos_C_Fecha] ON [dbo].[Productos_C]
(
	[Fecha] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_Productos_C_ProductoID_Act] ON [dbo].[Productos_C]
(
	[ProductoID] ASC,
	[Act] ASC
)
INCLUDE([Fecha]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
ALTER TABLE [dbo].[Productos_C] ADD  CONSTRAINT [DF_Productos_C_Fecha]  DEFAULT (getdate()) FOR [Fecha]
GO
ALTER TABLE [dbo].[Productos_C] ADD  CONSTRAINT [DF_Productos_C_Act]  DEFAULT ((1)) FOR [Act]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservas](
	[ReservaID] [int] IDENTITY(1,1) NOT NULL,
	[ClienteDNI] [int] NOT NULL,
	[MesaNumero] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[Hora] [time](0) NOT NULL,
	[CantidadComensales] [int] NOT NULL,
	[Estado] [nvarchar](20) NOT NULL,
	[CamareroDNI] [int] NULL,
 CONSTRAINT [PK_Reservas] PRIMARY KEY CLUSTERED 
(
	[ReservaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_Reservas_Mesa_Turno] ON [dbo].[Reservas]
(
	[MesaNumero] ASC,
	[Fecha] ASC,
	[Hora] ASC
)
INCLUDE([Estado]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO
ALTER TABLE [dbo].[Reservas]  WITH NOCHECK ADD  CONSTRAINT [CK_Reservas_Estado] CHECK  (([Estado]=N'Finalizada' OR [Estado]=N'Asistida' OR [Estado]=N'Cancelada' OR [Estado]=N'Confirmada'))
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [CK_Reservas_Estado]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RolID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolFamilia](
	[RolID] [int] NOT NULL,
	[FamiliaID] [int] NOT NULL,
 CONSTRAINT [PK_RolFamilia] PRIMARY KEY CLUSTERED 
(
	[RolID] ASC,
	[FamiliaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolPermiso](
	[RolID] [int] NOT NULL,
	[PermisoID] [int] NOT NULL,
 CONSTRAINT [PK_RolPermiso] PRIMARY KEY CLUSTERED 
(
	[RolID] ASC,
	[PermisoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TelefonosAdicionalesCliente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClienteDNI] [int] NOT NULL,
	[Telefono] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_TelefonosAdicionalesCliente] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[DNI] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[RolID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Hash] [nvarchar](255) NOT NULL,
	[Salt] [nvarchar](255) NOT NULL,
	[Bloqueado] [bit] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Idioma] [varchar](5) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF),
 CONSTRAINT [UQ_Users_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Bloqueado]  DEFAULT ((0)) FOR [Bloqueado]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Idioma]  DEFAULT ('es') FOR [Idioma]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [CHK_Users_Idioma] CHECK  (([Idioma]='en' OR [Idioma]='es'))
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [CHK_Users_Idioma]
GO


/* ─── 3) Datos semilla y 4) Dígitos Verificadores (tabla DV) ──────────────
   Las filas de DV corresponden exactamente a estos datos (DvBLL_08YS). */
-- Clientes
INSERT [dbo].[Clientes] ([DNI], [Nombre], [Apellido], [Email], [Telefono], [Activo], [Direccion]) VALUES (30000000, N'Cliente', N'Frecuente', N'cliente@fogon.com', N'1100000000', 1, N'Av. Siempre Viva 123')
GO
-- DV
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Clientes', N'FfueLzU+cesSdj5UiBiircXdpdTj5j0N4ypXO03dN6D01sJ6oGRcA5kouJIdVb34qiBNSTuVjHSG8cUVhLzbs5Zw3bF02hndW7rE4FJwfvM=', N'AT9IHFWLP85lcxCiz0Ib+4eeyZLDC1MCt7l0y2pUSi7qgJ+ZZCCFgYxpTmAlQxEviD+FBA5A/bw+6eXyABkYFOM9+DnlldEm8a0FX20PB+U=')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Empleados', N'76bUYYAUD637R5lRsWcnFR/IoksAyVChXoFUOuFQsFyN0e7IO/W13losSeAu12HBRjgsGG9UNoZcdwh/YgCy++hA3LPWhl8lO7UgA6Enme/CwVCIVdtIFW09cCwBHK7CvF/LbUUiIR1ncWipPNRkrrt9OZ9cbuSE8NscGRlG5jU=', N'oo2A994cFMBK6lLG23rJduaUtwrCxx2dZ69BmzrzRtOA4GwOBhDZRz224fB6ZPdrpNChgJunx3RU6gzrRIl9k+0LOXfYkQrBuKxpxS7pnGwvqTM+51IAvFDAkFEvRj+/+i89JdJ+dS8LmLOSby/i969HwKD7dgBKuNCQH3OYYFo=')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Facturas', N'ptmmpmry2Awc6TsYuuKCHA==', N'ZUfQd7PynYmY/krYVmYsKg==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'FamiliaIntegrada', N'5IosYTXNpmSR1HPORAmeNQ==', N'1iPTQXhgIkDeNjdii7cfOQ==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'FamiliaPermiso', N'SS5amWewvtVrB+MWBQq1noI5AuM+NBqZ4sdK5bopfQdwVoAq8CSQgRzPvFEjJYy4gTt6wn4IHFabfH0TF/q1eqSNIDMMjdopUWpWLfW0pwo7Op6TYVUr8trhId+ycYJBuLGb5rQQcQ+4bYc4rjeldfz+xBxXIT/LQSVOLgLiObg=', N'XM45C3pJvHb1T7S2eKv4ytYAjKn9VSkDvCIM6HahORpgqg8iu97OQb7Dh/XPrxM1LYE5t0Z+U0b7CBBFGaDWQFrhYfgt6PDf2MVUCGGaoRy/DFCCuM7Hm6CZqcOSEZBAPZIA/7I+EcJNKvc1yA6ESY8dl3ObvOhJzORpg4XYQT4=')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Familias', N'4ABMalsu8ryZjyVlUyC4n/7xIHifXgW/mLnna0tFJkl7dgbn3+5atdMIjv4qTV7do6Vlfc6VH5nhhUf0VogxYl5AZtKRL9dv9nFhf6CjFUdxg2zlwc4BzKgCnBVRE5K72kz7Xboc/OYChHXvOkpixiKKB9UssDWfjjtWKWks8JI0oYYsHtkcirWB/VXeL92atJJmbvfqlHhSaegsnAUo+C7w6b5R262oc8SEQSLRi51vgOXty3xJY5zvqMBeG2fRhCMruqi1toaMcYmJQJzu/D54XamQHz8J66WXMiT5gJBGnaWGblNCus8zJ1HVJVAJ7p9UVNpJOFEtmUzTV3ZsTIK0GQMmcn5jAxcy+goCAojvxT4pOdgDyKznACOTQY6u4Bii7+FcvDc6/PToXdkjO3+KbIuwgPVn8/qNQ4woZ8+Xia4aKevwDQzb1rmdvLxH', N'JJ3S3gbZI3J6JUSV30kAlkOHRhxCswExfv/hZ5SwOFCZvmJVZEFPpmpMKmjc69GgLdlpv8M2pYWqRtaB0BeRjbQDknXs5QGnG32wHEsmtMuxQJZnMPPPWO0UOfZ16GIHg1CDUG1ZJ3vsfV1LRv4f0ZFImFKJURNu4hfZY6MLCg1bk4ZhWEYYMrB3mBu8j67SFP6nOOfjZ7j6F1INT4Hy0h8Ps8nSkowYrUMd6WvTkxjt+gk47wA0EsWYJjm7nHXuKTzbPO4qNMKMEZ6w6xWpGjoQceKjx3OARW1xxeZHc9R6ZBFyJmJlQre6wXqGUG1vj2sIq5BoMbLikJpSEevRjh4bvrdtngD2DoR5QU6bT37dllr9SEE9/nSvH68vg8hUnvFLCp/qUuLAlEiTnHCDYZeDPzLBZ0Yu4fVCrJ9cmwNLqKPysStL4AIpA5M+pSJm')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'ItemsFactura', N'vu221HBsFSFs7jUfe2C+qw==', N'DGaXUzHNGCgAbivIjGIaeA==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'ItemsPedido', N'oii61PhTuH5EvJODQSz59g==', N'cRryy1i2VN7QOUJcUSWbIg==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Mesas', N'Wvwjr91FrA5EyI9gxcKKC4y4yGkO6QvjjrdSURI7L1ZZaRtZiClVcLIJugY37ZPLGiTg8UU0sgenzhqUyIg1icBMDjCIVuMZSrD70BPypRgFyfFOXJqRZ4UzzSkr5J4GPxtppV7IH1nMVn+1dA7Y5Q==', N'rK1ocdKY7uxT47hLVLlYBNDmrrdsf06gLe2zesGxvnur2Vo4GzRlU+OM7LI9c4gcuXm8ixvzCiQEBKmTidGPIBjShv3mdvxpv3c+X667FTmKEnuWYnv2UQwUTBIHVZW6EImmLPVqv2+h3qyvn+jPFw==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Pagos', N'WkAcY27jlgMAbeZqgwgYVQ==', N'5DLSMIg0s1ANg6OS2g/2RQ==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Pedidos', N'dpAcLn4Rq+yA9YGDve5fow==', N'dX58yhZ/DvfN+tU39iQuEg==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Permisos', N'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', N'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')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Productos', N'huaTDGf8l4IfxMJsv/OF71k0+m4c9Uu91AaxJ+eBpjOT5+O2q1LhW3qMvUiNzWZggMEcKC7yXhfdXHodVMRhyfk6pdy4wlVK8HtSPzhSeZQERDumIWmTqH4uiyKr9Iul74PG96gFHNPXCTfWPyhTot1HWwbvBCKcB/fburd+EUAmjiRjj3V6VxN8dOoA5EadhIedZ4rUNWYwpDfRMJO8ki8TVMcDt1C5YUsy5CbHirAUOMLK+/pqhJotR4tHBHA7BFHnnWP/aFTUC/6wHFkDgInbyLXB4TPQ8UC9S1BQ7XOwcg7Cr/T6EGIpVD+8X+hU6X98rLwor1IBfmW37yrho5jxHU1OuvVlC1XeOLhaO1T6qoUNBp0T2D6fq2bW8mGQ4Cq8Dyr2GKkGpS1lVzpb56GB19dV92/HxtFZU6X6+/1J4oG1WdhgrqiPPFJrd8P1T6PTiBe4yeQ3P+ie8TrYtlzU7Ks+PErNqKmODlIG8Tglfz0lPF7Gls5sSGpMVS9vwJo3Xe9WWQvcC1e6lGRkSnrTjDHJQdnJkrUxFztiNWVy1DwTsfmnCT8+RJ6pOEE9YZbrPZ4PbmE/Va61jfY6/Yq4FJXMDJ8umVd6GrXNfT0Fxo5QAeUwacs8EkUnNM8B8S8F1RG65gczClnKigPNaM5Ruf6ySbelmK40F0yq6rRhXA0KrldhHY+Hctj7ihnNJ+QDMuDDJVZR50+S7Z8plFc6lwO0if2vrZ/d5pobXbWrfEVJgR3CVlnF5uXAB61e7N5iH2wqaxk5nvKR1yKxcA==', N'QoMPmumxKZuOe+7bxzPmwozZbMBNkt6LKDsdWHtuG/m7usg9ZGtZ6wlow6LxZTXSuZx5+5AQZ4xKvyCCRm7uw3fzM4e9XLPQJvqptBBxA1SZUJLo50IBqGId430aEIHTuWf2KpQRqXcClQQTAlKJhb+dmZaiXCebeDHwA1iaNKA+8AvzF71OrRohYtEmLnx9uhI+RJEw60m+E6b6oH/P/DKAIDCk5ucIMuy4iwdeF4aOZJZ0cK68knP/aVP1iHiBZoD+M+wVJiiXqDd43clwddaLEnUxItERCZzK/KOfAiDxtJoo47UiOPpLshM9EOs7xpbUeVE51c/T28hnT/6JhdIaTjrbdoI76qVjXbOB9AVf11pj4ZeIRMNE43QJGKRJwsck5JVsXKCP11fKvpMMphDjhDmakTkfH9fVsWAiB6vTkvbFmCjKrT7UX9wfusqC2lJ6iT3T2/THPwzDUDRV7IMFUE+BS8AEgUd1bflJJ75trp74bL1Vyb6egM3E9xd+Zyno82PizG2dBwI1JRgw2KoSBMwLDz8U9UTl/LFm6/3c1MEVTFYV7sIKY2mHrqoqDbSPcUUmt83SzDBJxD502Y5bPzLScixIrTyGdbGKKTr3e9rJtvYIQ5OA2qyCfgySLGNS7ASZu7gjDW7w7QPR/TU56k4E0EgrjA4FH6RRV0gAS9c7ExgEGsg4e7ZqTNOZhcPnXsN9OujIcdjJQNhif6k7K/Cj3T2FR5H56JYvhlatsghBlmsZvB+rFEpRg8RlHYbZ/xfO43FU0ifFRoAO4A==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Reservas', N'UZnK9nko0Rk5Hm4urEiuGw==', N'gC4Lra1Y1YFf4XoAfkCKyA==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Roles', N'cougKoLpSK6LuvvoaHF+6jC+rAkkNWqKSU+b9oQY4X2g7H4BLfSoPdfZRqMCh3Wc7flHUIx4J0huXnXieOwenhtuzbR/zzBPUqEH475Qz8M=', N'Y1dFEhqr5LRbxa81Mh7Zv9hf8fWSwxJXDhwnWVEwXlQToEHqzzuGdqRXtHhAFMrfa0zGZrmb/GAI2wq0mD1wtGvM5z+MzS+bkQwJ87gpwkA=')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'RolFamilia', N'O/TplqOE+7KQugh/DwAcc2tW2CeMAufDo2O9gY+rSTE3tiy4OzaChO8Ev44waz6reQoh8aSxNYayzDgqm0cb+A==', N'pBIJXLVaPTAmpkXr6fjrUNBnO4IJ3K/5XSh6Li8qpy+K6G0DSyrEs37nu4dnmxipwARYRmnRFAA05Trhds/n2A==')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'RolPermiso', N'1ZHbaibjUMIvq/i9x7uEdxHYXfwep/Tkw2313z4yOQPufXZWqaeH0itb2pfW+Bb8', N'idoJuuYcLt7tPVf0Zvce2kyGY649urSUB0cru/wmG+FIzkIBRCKN/8GfYacFIR2b')
INSERT [dbo].[DV] ([Tabla], [DVH], [DVV]) VALUES (N'Users', N'wSQ6L6QDrv14dS8KCBE8KPeg7vFHZ8SuIy/vcqwvMM7BegE8beOAmBT0A+DOMNDx5dBTSgqrzFn76/58o28tGTxc8CktpgWVtQMhyQKzoJWasnnE1xd0dyHcML/fHpcVL9KZGDXsRyz0gXKppj8F4j+rYIAb97fKdJVTC7WGow0=', N'xioQuckAuprBSBozkpWWjMbHO0S0sb8mvPhZgo8b/+UqGRCfJHJJ2t9dFP8ZD14TSts/ui66udUY/87CkIfna92zwsgaz6e5zcelEPeEosD4SfLBGwonzxaHmq4duk/yNScguGASVjpPz8ap6+wQ/Syph+Z8NR/OlKe3q4MrHwg=')
GO
-- Empleados
INSERT [dbo].[Empleados] ([DNI], [Nombre], [Apellido], [Puesto], [UsuarioDNI], [Activo]) VALUES (12345678, N'Maximo', N'Yapura', N'Camarero', 12345678, 1)
INSERT [dbo].[Empleados] ([DNI], [Nombre], [Apellido], [Puesto], [UsuarioDNI], [Activo]) VALUES (22222222, N'Pedro', N'Pascal', N'Recepcionista', NULL, 1)
INSERT [dbo].[Empleados] ([DNI], [Nombre], [Apellido], [Puesto], [UsuarioDNI], [Activo]) VALUES (30000001, N'Lucas', N'Gomez', N'Camarero', NULL, 1)
INSERT [dbo].[Empleados] ([DNI], [Nombre], [Apellido], [Puesto], [UsuarioDNI], [Activo]) VALUES (30000002, N'Martina', N'Ruiz', N'Chef', NULL, 1)
GO
-- FamiliaPermiso
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (1, 2)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (1, 3)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (1, 4)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (1, 5)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (1, 6)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (2, 11)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (2, 12)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (2, 13)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (2, 14)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (3, 7)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (3, 8)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (3, 9)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (3, 10)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (4, 1)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (4, 2)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (4, 7)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (4, 11)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (7, 17)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (7, 18)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (7, 19)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (8, 20)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (8, 21)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (9, 22)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (9, 23)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (10, 24)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (10, 26)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (11, 29)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (12, 30)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (12, 31)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (13, 32)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (14, 33)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (15, 34)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (16, 35)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (16, 36)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (17, 37)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (18, 38)
INSERT [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID]) VALUES (19, 39)
GO
-- Familias
SET IDENTITY_INSERT [dbo].[Familias] ON
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (1, N'Gestion de usuarios')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (2, N'Gestion de familias')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (3, N'Gestion de roles')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (4, N'Consulta general')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (7, N'Gestion de respaldos')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (8, N'Gestion de clientes')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (9, N'Gestion de mesas')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (10, N'Gestion de reservas')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (11, N'Atencion en salon')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (12, N'Gestion de pedidos')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (13, N'Cocina')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (14, N'Gestion de empleados')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (15, N'Gestion de carta')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (16, N'Facturacion y cobro')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (17, N'Gestion de caja')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (18, N'Reportes predictivos')
INSERT [dbo].[Familias] ([FamiliaID], [Nombre]) VALUES (19, N'Auditoría de productos')
SET IDENTITY_INSERT [dbo].[Familias] OFF
GO
-- Mesas
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (1, 2, N'Ocupada', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (2, 2, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (4, 4, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (5, 4, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (6, 6, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (7, 8, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (8, 6, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (9, 7, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (10, 2, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (11, 8, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (12, 4, N'Libre', 1)
INSERT [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado], [Activo]) VALUES (13, 7, N'Libre', 1)
GO
-- Permisos
SET IDENTITY_INSERT [dbo].[Permisos] ON
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (1, N'VerBitacora', N'Consultar eventos del sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (2, N'VerUsuarios', N'Acceder a la pantalla gestion de usuarios con la lista de usuarios en el sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (3, N'CrearUsuario', N'Crear usuarios nuevos dentro del sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (4, N'ModificarUsuario', N'Modificar usuarios existentes')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (5, N'DesActivarUsuario', N'Activar o desactivar usarios existentes')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (6, N'DesbloquearUsuario', N'Desbloquear usuarios que se encuentren bloqueados')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (7, N'VerRoles', N'Acceder a la pantalla gestion de roles con la lista de roles en el sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (8, N'CrearRoles', N'Crear roles nuevos dentro del sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (9, N'ModificarRoles', N'Modificar roles existentes')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (10, N'EliminarRoles', N'Eliminar roles existentes')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (11, N'VerFamilias', N'Acceder a la pantalla gestion de familias con la lista de familias en el sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (12, N'CrearFamilias', N'Crear familias nuevas dentro del sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (13, N'ModificarFamilias', N'Modificar familias existentes')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (14, N'EliminarFamilias', N'Eliminar Familias existentes')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (16, N'ManejarInconsistencias', N'Permite tomar decisiones en caso de producirse una inconsistencia de base de datos en el sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (17, N'VerRespaldos', N'Permite acceder al formulario Gestion de Respaldos en el menu principal')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (18, N'RealizarBackup', N'Permite realizar un backup de la base de datos actual y guardar el archivo .bak')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (19, N'RealizarRestore', N'Permite restaurar toda la base de datos actual del sistema a una version anterior guardada en un backup seleccionando el archivo .bak')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (20, N'CrearCliente', N'Permite registrar un nuevo cliente en el sistema')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (21, N'VerClientes', N'Permite acceder al listado de clientes registrados')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (22, N'VerMesas', N'Permite acceder al listado de mesas')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (23, N'CrearMesa', N'Permite dar de alta, modificar y eliminar mesas')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (24, N'VerReservas', N'Permite consultar el listado de reservas')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (26, N'RegistrarReserva', N'Permite registrar una nueva reserva')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (27, N'VerReportes', N'Permite acceder al modulo de reportes gerenciales en la barra lateral')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (28, N'VerMaestros', N'Permite acceder a la seccion de Maestros en la barra lateral')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (29, N'AbrirMesa', N'Permite abrir mesas (con reserva o Walk-in) y asignar el camarero responsable')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (30, N'CerrarMesa', N'Permite finalizar la atención de una mesa y liberarla')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (31, N'RegistrarPedido', N'Permite registrar pedidos de las mesas asignadas')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (32, N'DespacharPedido', N'Permite gestionar comandas de cocina y marcar pedidos como listos')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (33, N'CrearEmpleado', N'Permite dar de alta y modificar empleados')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (34, N'GestionProductos', N'Permite dar de alta, modificar y dar de baja productos de la carta')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (35, N'GenerarFactura', N'Permite generar la cuenta / factura de una mesa (CUN-006)')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (36, N'RegistrarCobro', N'Permite registrar el pago de una factura y liberar la mesa (CUN-007)')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (37, N'GestionCaja', N'Permite consultar el arqueo y realizar el cierre de caja diario')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (38, N'GestionReportePredictivo', N'Permite consultar el análisis predictivo de canasta (reglas de asociación y sugerencias)')
INSERT [dbo].[Permisos] ([PermisoID], [Nombre], [Descripcion]) VALUES (39, N'AuditarProductos', N'Permite consultar la bitácora de cambios de productos y restaurar versiones anteriores (rollback)')
SET IDENTITY_INSERT [dbo].[Permisos] OFF
GO
-- Productos
SET IDENTITY_INSERT [dbo].[Productos] ON
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (1, N'Bife de chorizo', CAST(18500.00 AS Decimal(10, 2)), 13, 1, N'PlatoPrincipal', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (2, N'Provoleta', CAST(7800.00 AS Decimal(10, 2)), 15, 1, N'Entrada', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (3, N'Empanada de carne', CAST(2200.00 AS Decimal(10, 2)), 52, 1, N'Entrada', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (4, N'Ensalada mixta', CAST(5200.00 AS Decimal(10, 2)), 25, 1, N'Entrada', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (5, N'Flan con dulce de leche', CAST(4500.00 AS Decimal(10, 2)), 3, 1, N'Postre', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (6, N'Agua mineral', CAST(2000.00 AS Decimal(10, 2)), 75, 1, N'Bebida', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (7, N'Milanesa napolitana', CAST(11200.00 AS Decimal(10, 2)), 0, 1, N'PlatoPrincipal', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (8, N'Sorrentinos de jamón y queso', CAST(9800.00 AS Decimal(10, 2)), 17, 1, N'PlatoPrincipal', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (9, N'Risotto de hongos', CAST(10900.00 AS Decimal(10, 2)), 0, 1, N'PlatoPrincipal', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (10, N'Tiramisú', CAST(5200.00 AS Decimal(10, 2)), 9, 1, N'Postre', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (11, N'Coca-Cola 500ml', CAST(2500.00 AS Decimal(10, 2)), 42, 1, N'Bebida', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (12, N'Rabas a la provenzal', CAST(9400.00 AS Decimal(10, 2)), 8, 1, N'Entrada', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (13, N'Bife de lomo con papas', CAST(14500.00 AS Decimal(10, 2)), 0, 1, N'PlatoPrincipal', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (14, N'Ravioles de ricota', CAST(9200.00 AS Decimal(10, 2)), 14, 1, N'PlatoPrincipal', 1)
INSERT [dbo].[Productos] ([ProductoID], [Nombre], [Precio], [StockPorciones], [Activo], [Categoria], [ControlaStock]) VALUES (16, N'Copa de Malbec', CAST(4800.00 AS Decimal(10, 2)), 30, 1, N'Bebida', 1)
SET IDENTITY_INSERT [dbo].[Productos] OFF
GO
-- Roles
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([RolID], [Nombre]) VALUES (1, N'SuperAdmin')
INSERT [dbo].[Roles] ([RolID], [Nombre]) VALUES (2, N'Supervisor')
INSERT [dbo].[Roles] ([RolID], [Nombre]) VALUES (3, N'Gestor de accesos')
INSERT [dbo].[Roles] ([RolID], [Nombre]) VALUES (4, N'Recepcionista')
INSERT [dbo].[Roles] ([RolID], [Nombre]) VALUES (5, N'Gerente')
INSERT [dbo].[Roles] ([RolID], [Nombre]) VALUES (6, N'Camarero')
INSERT [dbo].[Roles] ([RolID], [Nombre]) VALUES (7, N'Chef')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
-- RolFamilia
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 1)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 2)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 3)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 7)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 8)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 9)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 10)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 11)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 12)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 13)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 14)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 15)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 16)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 17)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 18)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (1, 19)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (2, 4)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (3, 2)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (3, 3)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (4, 11)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (6, 12)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (6, 16)
INSERT [dbo].[RolFamilia] ([RolID], [FamiliaID]) VALUES (7, 13)
GO
-- RolPermiso
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (1, 1)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (1, 16)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (1, 27)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (1, 28)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (2, 6)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (4, 20)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (4, 21)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (4, 22)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (4, 24)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (4, 26)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (5, 24)
INSERT [dbo].[RolPermiso] ([RolID], [PermisoID]) VALUES (5, 27)
GO
-- Users
INSERT [dbo].[Users] ([DNI], [Username], [RolID], [Nombre], [Apellido], [Email], [Hash], [Salt], [Bloqueado], [Activo], [Idioma]) VALUES (12345678, N'Maximo', 1, N'Maximo', N'Yapura', N'Maximo@gmail.com', N'N4Rf9Gw3hwxmXiqY4tp1IFxaOezlZdSiuYkDZqKOGFQ=', N'CWu49+d6I6N7m98eZq7sUw==', 0, 1, N'es')
GO


/* ─── Claves foráneas (después de los datos) ──────────────────────────── */
ALTER TABLE [dbo].[Bitacora]  WITH CHECK ADD  CONSTRAINT [FK_Bitacora_Users] FOREIGN KEY([Username])
REFERENCES [dbo].[Users] ([Username])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Bitacora] CHECK CONSTRAINT [FK_Bitacora_Users]
GO
ALTER TABLE [dbo].[EmailsAdicionalesCliente]  WITH CHECK ADD  CONSTRAINT [FK_EmailsAdicionalesCliente_Clientes] FOREIGN KEY([ClienteDNI])
REFERENCES [dbo].[Clientes] ([DNI])
GO
ALTER TABLE [dbo].[EmailsAdicionalesCliente] CHECK CONSTRAINT [FK_EmailsAdicionalesCliente_Clientes]
GO
ALTER TABLE [dbo].[Empleados]  WITH CHECK ADD  CONSTRAINT [FK_Empleados_Users] FOREIGN KEY([UsuarioDNI])
REFERENCES [dbo].[Users] ([DNI])
GO
ALTER TABLE [dbo].[Empleados] CHECK CONSTRAINT [FK_Empleados_Users]
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD  CONSTRAINT [FK_Facturas_Reservas] FOREIGN KEY([NroReserva])
REFERENCES [dbo].[Reservas] ([ReservaID])
GO
ALTER TABLE [dbo].[Facturas] CHECK CONSTRAINT [FK_Facturas_Reservas]
GO
ALTER TABLE [dbo].[FamiliaIntegrada]  WITH CHECK ADD FOREIGN KEY([FamiliaPadreID])
REFERENCES [dbo].[Familias] ([FamiliaID])
GO
ALTER TABLE [dbo].[FamiliaIntegrada]  WITH CHECK ADD FOREIGN KEY([FamiliaID])
REFERENCES [dbo].[Familias] ([FamiliaID])
GO
ALTER TABLE [dbo].[FamiliaPermiso]  WITH CHECK ADD FOREIGN KEY([FamiliaID])
REFERENCES [dbo].[Familias] ([FamiliaID])
GO
ALTER TABLE [dbo].[FamiliaPermiso]  WITH CHECK ADD  CONSTRAINT [FK__FamiliaPe__Permi__5165187F] FOREIGN KEY([PermisoID])
REFERENCES [dbo].[Permisos] ([PermisoID])
GO
ALTER TABLE [dbo].[FamiliaPermiso] CHECK CONSTRAINT [FK__FamiliaPe__Permi__5165187F]
GO
ALTER TABLE [dbo].[ItemsFactura]  WITH CHECK ADD  CONSTRAINT [FK_ItemsFactura_Facturas] FOREIGN KEY([NroFactura])
REFERENCES [dbo].[Facturas] ([NroFactura])
GO
ALTER TABLE [dbo].[ItemsFactura] CHECK CONSTRAINT [FK_ItemsFactura_Facturas]
GO
ALTER TABLE [dbo].[ItemsFactura]  WITH CHECK ADD  CONSTRAINT [FK_ItemsFactura_Productos] FOREIGN KEY([ProductoID])
REFERENCES [dbo].[Productos] ([ProductoID])
GO
ALTER TABLE [dbo].[ItemsFactura] CHECK CONSTRAINT [FK_ItemsFactura_Productos]
GO
ALTER TABLE [dbo].[ItemsPedido]  WITH CHECK ADD  CONSTRAINT [FK_ItemsPedido_Pedidos] FOREIGN KEY([NroPedido])
REFERENCES [dbo].[Pedidos] ([NroPedido])
GO
ALTER TABLE [dbo].[ItemsPedido] CHECK CONSTRAINT [FK_ItemsPedido_Pedidos]
GO
ALTER TABLE [dbo].[ItemsPedido]  WITH CHECK ADD  CONSTRAINT [FK_ItemsPedido_Productos] FOREIGN KEY([ProductoID])
REFERENCES [dbo].[Productos] ([ProductoID])
GO
ALTER TABLE [dbo].[ItemsPedido] CHECK CONSTRAINT [FK_ItemsPedido_Productos]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [FK_Pagos_Facturas] FOREIGN KEY([NroFactura])
REFERENCES [dbo].[Facturas] ([NroFactura])
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [FK_Pagos_Facturas]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Reservas] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[Reservas] ([ReservaID])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Reservas]
GO
ALTER TABLE [dbo].[Productos_C]  WITH NOCHECK ADD  CONSTRAINT [FK_Productos_C_Productos] FOREIGN KEY([ProductoID])
REFERENCES [dbo].[Productos] ([ProductoID])
GO
ALTER TABLE [dbo].[Productos_C] NOCHECK CONSTRAINT [FK_Productos_C_Productos]
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Clientes] FOREIGN KEY([ClienteDNI])
REFERENCES [dbo].[Clientes] ([DNI])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Clientes]
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Empleados] FOREIGN KEY([CamareroDNI])
REFERENCES [dbo].[Empleados] ([DNI])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Empleados]
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Mesas] FOREIGN KEY([MesaNumero])
REFERENCES [dbo].[Mesas] ([NroMesa])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Mesas]
GO
ALTER TABLE [dbo].[RolFamilia]  WITH CHECK ADD FOREIGN KEY([FamiliaID])
REFERENCES [dbo].[Familias] ([FamiliaID])
GO
ALTER TABLE [dbo].[RolFamilia]  WITH CHECK ADD FOREIGN KEY([RolID])
REFERENCES [dbo].[Roles] ([RolID])
GO
ALTER TABLE [dbo].[RolPermiso]  WITH CHECK ADD  CONSTRAINT [FK__RolPermis__Permi__5441852A] FOREIGN KEY([PermisoID])
REFERENCES [dbo].[Permisos] ([PermisoID])
GO
ALTER TABLE [dbo].[RolPermiso] CHECK CONSTRAINT [FK__RolPermis__Permi__5441852A]
GO
ALTER TABLE [dbo].[RolPermiso]  WITH CHECK ADD FOREIGN KEY([RolID])
REFERENCES [dbo].[Roles] ([RolID])
GO
ALTER TABLE [dbo].[TelefonosAdicionalesCliente]  WITH CHECK ADD  CONSTRAINT [FK_TelefonosAdicionalesCliente_Clientes] FOREIGN KEY([ClienteDNI])
REFERENCES [dbo].[Clientes] ([DNI])
GO
ALTER TABLE [dbo].[TelefonosAdicionalesCliente] CHECK CONSTRAINT [FK_TelefonosAdicionalesCliente_Clientes]
GO


/* ─── Triggers ─────────────────────────────────────────────────────────── */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE TRIGGER [dbo].[producto_actualizado]
ON [dbo].[Productos]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM inserted) RETURN;

    UPDATE c
    SET c.Act = 0
    FROM [dbo].[Productos_C] c
    WHERE c.Act = 1
      AND c.ProductoID IN (SELECT i.ProductoID FROM inserted i);

    INSERT INTO [dbo].[Productos_C]
        ([ProductoID], [Nombre], [Categoria], [Precio], [StockPorciones], [ControlaStock], [Activo], [Fecha], [Act])
    SELECT i.ProductoID, i.Nombre, i.Categoria, i.Precio, i.StockPorciones, i.ControlaStock, i.Activo, GETDATE(), 1
    FROM inserted i;
END
GO
ALTER TABLE [dbo].[Productos] ENABLE TRIGGER [producto_actualizado]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE TRIGGER [dbo].[producto_eliminado]
ON [dbo].[Productos]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    UPDATE c
    SET c.Act = 0
    FROM [dbo].[Productos_C] c
    WHERE c.ProductoID IN (SELECT d.ProductoID FROM deleted d);
END
GO
ALTER TABLE [dbo].[Productos] ENABLE TRIGGER [producto_eliminado]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE TRIGGER [dbo].[producto_c_actualizado]
ON [dbo].[Productos_C]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT UPDATE([Act]) RETURN;

    ;WITH Restauradas AS (
        SELECT i.*,
               ROW_NUMBER() OVER (PARTITION BY i.ProductoID ORDER BY i.ID DESC) AS Orden
        FROM inserted i
        INNER JOIN deleted d ON d.ID = i.ID
        WHERE i.Act = 1 AND d.Act = 0
    )
    MERGE [dbo].[Productos] AS destino
    USING (SELECT * FROM Restauradas WHERE Orden = 1) AS origen
       ON destino.ProductoID = origen.ProductoID
    WHEN MATCHED THEN
        UPDATE SET destino.Nombre         = origen.Nombre,
                   destino.Categoria      = origen.Categoria,
                   destino.Precio         = origen.Precio,
                   destino.StockPorciones = origen.StockPorciones,
                   destino.ControlaStock  = origen.ControlaStock,
                   destino.Activo         = origen.Activo;
END
GO
ALTER TABLE [dbo].[Productos_C] ENABLE TRIGGER [producto_c_actualizado]
GO


/* ─── Procedimientos almacenados ──────────────────────────────────────── */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateFamilia]
    @Nombre         NVARCHAR(100),
    @Permisos       dbo.IntList READONLY,
    @SubFamilias    dbo.IntList READONLY,
    @NuevoFamiliaID INT OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO Familias (Nombre) VALUES (@Nombre);
        SET @NuevoFamiliaID = SCOPE_IDENTITY();

        INSERT INTO FamiliaPermiso (FamiliaID, PermisoID)
        SELECT @NuevoFamiliaID, ID FROM @Permisos;

        INSERT INTO FamiliaIntegrada (FamiliaID, FamiliaPadreID)
        SELECT ID, @NuevoFamiliaID FROM @SubFamilias;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK; THROW;
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateRol]
    @Nombre     NVARCHAR(100),
    @Familias   dbo.IntList READONLY,
    @Permisos   dbo.IntList READONLY,
    @NuevoRolID INT OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO Roles (Nombre) VALUES (@Nombre);
        SET @NuevoRolID = SCOPE_IDENTITY();

        INSERT INTO RolFamilia (RolID, FamiliaID)
        SELECT @NuevoRolID, ID FROM @Familias;

        INSERT INTO RolPermiso (RolID, PermisoID)
        SELECT @NuevoRolID, ID FROM @Permisos;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK; THROW;
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteFamilia] @FamiliaID INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM RolFamilia       WHERE FamiliaID = @FamiliaID)
    OR EXISTS (SELECT 1 FROM FamiliaIntegrada WHERE FamiliaID = @FamiliaID)
        RAISERROR('La familia está siendo utilizada.', 16, 1);
    ELSE
    BEGIN
        DELETE FROM FamiliaPermiso   WHERE FamiliaID = @FamiliaID;
        DELETE FROM FamiliaIntegrada WHERE FamiliaPadreID = @FamiliaID;
        DELETE FROM Familias         WHERE FamiliaID = @FamiliaID;
    END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteRol] @RolID INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE RolID = @RolID)
        RAISERROR('El rol está asignado a usuarios.', 16, 1);
    ELSE
    BEGIN
        DELETE FROM RolPermiso WHERE RolID = @RolID;
        DELETE FROM RolFamilia WHERE RolID = @RolID;
        DELETE FROM Roles      WHERE RolID = @RolID;
    END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_EjecutarBackup]
    @RutaArchivo NVARCHAR(260),
    @NombreBaseDatos NVARCHAR(100)
AS
    BACKUP DATABASE @NombreBaseDatos
    TO DISK = @RutaArchivo
    WITH FORMAT, INIT,
         NAME = 'Horizon Backup',
         SKIP, NOREWIND, NOUNLOAD, CHECKSUM, STATS = 10;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllFamilias]
AS
BEGIN
    -- RS0: todas las familias con su padre (NULL si es raíz)
    -- LEFT JOIN porque una familia raíz no tiene fila en FamiliaIntegrada
    SELECT f.FamiliaID, f.Nombre, fi.FamiliaPadreID
    FROM Familias f
    LEFT JOIN FamiliaIntegrada fi ON f.FamiliaID = fi.FamiliaID;

    -- RS1: permisos directos por familia
    SELECT fp.FamiliaID, p.PermisoID, p.Nombre AS PermisoNombre, p.Descripcion
    FROM FamiliaPermiso fp
    INNER JOIN Permisos p ON fp.PermisoID = p.PermisoID;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllPermisos]
AS
    SELECT PermisoID, Nombre AS PermisoNombre, Descripcion FROM Permisos;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllRolesConEstructura]
AS
BEGIN
    SELECT RolID, Nombre FROM Roles;                               -- RS0

    SELECT RolID, FamiliaID FROM RolFamilia;                       -- RS1

    SELECT rp.RolID, p.PermisoID, p.Nombre AS PermisoNombre,       -- RS2
           p.Descripcion
    FROM RolPermiso rp
    INNER JOIN Permisos p ON rp.PermisoID = p.PermisoID;

    -- RS3: familias con FamiliaPadreID — mismo formato que EnsamblarFamilias espera
    SELECT f.FamiliaID, f.Nombre, fi.FamiliaPadreID
    FROM Familias f
    LEFT JOIN FamiliaIntegrada fi ON f.FamiliaID = fi.FamiliaID;

    -- RS4: permisos de familias — mismo formato que EnsamblarFamilias espera
    SELECT fp.FamiliaID, p.PermisoID, p.Nombre AS PermisoNombre,
           p.Descripcion
    FROM FamiliaPermiso fp
    INNER JOIN Permisos p ON fp.PermisoID = p.PermisoID;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ───────────────────────── 2) SP de integridad DVH/DVV ─────────────────────────
   El orden de los result-sets DEBE coincidir con DvBLL_08YS.TablasMonitoreadas. */
CREATE PROCEDURE [dbo].[sp_GetAllTablesParaDV]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAncestrosFamilia]
    @FamiliaID INT
AS
BEGIN
    -- RS0: familias que contienen a @FamiliaID, directa o transitivamente
    ;WITH FamiliasAncestros AS (
        SELECT FamiliaPadreID AS FamiliaID
        FROM FamiliaIntegrada
        WHERE FamiliaID = @FamiliaID

        UNION ALL

        SELECT fi.FamiliaPadreID
        FROM FamiliaIntegrada fi
        INNER JOIN FamiliasAncestros fa ON fi.FamiliaID = fa.FamiliaID
    )
    SELECT DISTINCT FamiliaID FROM FamiliasAncestros;

    -- RS1: roles que contienen a @FamiliaID, directa o transitivamente
    -- (incluye el caso en que el rol la contiene directamente)
    ;WITH FamiliasIncluyendoSelf AS (
        SELECT @FamiliaID AS FamiliaID
        UNION ALL
        SELECT fi.FamiliaPadreID
        FROM FamiliaIntegrada fi
        INNER JOIN FamiliasIncluyendoSelf f ON fi.FamiliaID = f.FamiliaID
    )
    SELECT DISTINCT rf.RolID
    FROM RolFamilia rf
    INNER JOIN FamiliasIncluyendoSelf f ON rf.FamiliaID = f.FamiliaID;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetDVTodo]
AS SELECT Tabla, DVH, DVV FROM DV ORDER BY Tabla;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetRolByID]
    @RolID INT
AS
BEGIN
    -- RS0: el rol
    SELECT RolID, Nombre FROM Roles WHERE RolID = @RolID;

    -- RS1: permisos directos del rol
    SELECT p.PermisoID, p.Nombre AS PermisoNombre, p.Descripcion
    FROM RolPermiso rp
    INNER JOIN Permisos p ON rp.PermisoID = p.PermisoID
    WHERE rp.RolID = @RolID;

    -- Tabla temporal: CTE una sola vez
    ;WITH FamiliasDelRol AS (
        SELECT f.FamiliaID, f.Nombre, CAST(NULL AS INT) AS FamiliaPadreID
        FROM RolFamilia rf
        INNER JOIN Familias f ON rf.FamiliaID = f.FamiliaID
        WHERE rf.RolID = @RolID

        UNION ALL

        SELECT f.FamiliaID, f.Nombre, fi.FamiliaPadreID
        FROM FamiliaIntegrada fi
        INNER JOIN Familias f         ON fi.FamiliaID      = f.FamiliaID
        INNER JOIN FamiliasDelRol fdr ON fi.FamiliaPadreID = fdr.FamiliaID
    )
    SELECT FamiliaID, Nombre, FamiliaPadreID
    INTO #FamiliasTemp
    FROM FamiliasDelRol;

    -- RS2: familias con su relación padre-hijo
    SELECT FamiliaID, Nombre, FamiliaPadreID FROM #FamiliasTemp;

    -- RS3: permisos de esas familias — reutiliza #FamiliasTemp sin repetir CTE
    SELECT fp.FamiliaID, p.PermisoID, p.Nombre AS PermisoNombre, p.Descripcion
    FROM FamiliaPermiso fp
    INNER JOIN Permisos p       ON fp.PermisoID = p.PermisoID
    INNER JOIN #FamiliasTemp ft ON fp.FamiliaID = ft.FamiliaID;

    DROP TABLE #FamiliasTemp;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ModifyFamilia]
    @FamiliaID   INT,
    @Nombre      NVARCHAR(100),
    @Permisos    dbo.IntList READONLY,
    @SubFamilias dbo.IntList READONLY
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE Familias SET Nombre = @Nombre WHERE FamiliaID = @FamiliaID;

        DELETE FROM FamiliaPermiso   WHERE FamiliaID    = @FamiliaID;
        DELETE FROM FamiliaIntegrada WHERE FamiliaPadreID = @FamiliaID;

        INSERT INTO FamiliaPermiso (FamiliaID, PermisoID)
            SELECT @FamiliaID, ID FROM @Permisos;

        INSERT INTO FamiliaIntegrada (FamiliaID, FamiliaPadreID)
            SELECT ID, @FamiliaID FROM @SubFamilias;

        COMMIT;
    END TRY
    BEGIN CATCH ROLLBACK; THROW; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ModifyRol]
    @RolID    INT,
    @Nombre   NVARCHAR(100),
    @Familias dbo.IntList READONLY,
    @Permisos dbo.IntList READONLY
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE Roles SET Nombre = @Nombre WHERE RolID = @RolID;

        DELETE FROM RolFamilia WHERE RolID = @RolID;
        DELETE FROM RolPermiso  WHERE RolID = @RolID;

        INSERT INTO RolFamilia (RolID, FamiliaID)
            SELECT @RolID, ID FROM @Familias;

        INSERT INTO RolPermiso (RolID, PermisoID)
            SELECT @RolID, ID FROM @Permisos;

        COMMIT;
    END TRY
    BEGIN CATCH ROLLBACK; THROW; END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_UpsertDVTabla]
    @Tabla NVARCHAR(100),
    @DVH   NVARCHAR(MAX),
    @DVV   NVARCHAR(MAX)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM DV WHERE Tabla = @Tabla)
        UPDATE DV SET DVH = @DVH, DVV = @DVV WHERE Tabla = @Tabla;
    ELSE
        INSERT INTO DV (Tabla, DVH, DVV) VALUES (@Tabla, @DVH, @DVV);
END
GO


SET QUOTED_IDENTIFIER ON;
GO

COMMIT TRANSACTION;
GO

PRINT N'TP_Ing_Soft desplegada: esquema, datos iniciales y DV consistentes.';
PRINT N'DEPLOY_OK';
GO

SET NOEXEC OFF;
GO
