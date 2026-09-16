USE [TP_Ing_Soft]
GO
/****** Migración 790MY: Mesas + esqueleto mínimo de Reservas (CUN-003 Seleccionar Mesa) ******/

/* 1) Tabla Mesas */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mesas](
	[NroMesa] [int] NOT NULL,
	[Capacidad] [int] NOT NULL,
	[Estado] [nvarchar](20) NOT NULL, -- 'Libre' | 'Ocupada' | 'Reservada'
 CONSTRAINT [PK_Mesas] PRIMARY KEY CLUSTERED
(
	[NroMesa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* 2) Esqueleto mínimo de Reservas (solo lo indispensable para el NOT EXISTS de disponibilidad) */
CREATE TABLE [dbo].[Reservas](
	[ReservaID] [int] IDENTITY(1,1) NOT NULL,
	[ClienteDNI] [int] NOT NULL,
	[MesaNumero] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[Hora] [time](0) NOT NULL,
	[CantidadComensales] [int] NOT NULL,
	[Estado] [nvarchar](20) NOT NULL, -- 'Confirmada' | 'Cancelada'
 CONSTRAINT [PK_Reservas] PRIMARY KEY CLUSTERED
(
	[ReservaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Reservas] WITH CHECK ADD CONSTRAINT [FK_Reservas_Clientes] FOREIGN KEY([ClienteDNI])
REFERENCES [dbo].[Clientes] ([DNI])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Clientes]
GO

ALTER TABLE [dbo].[Reservas] WITH CHECK ADD CONSTRAINT [FK_Reservas_Mesas] FOREIGN KEY([MesaNumero])
REFERENCES [dbo].[Mesas] ([NroMesa])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Mesas]
GO

/* 3) Datos semilla: 10 mesas con capacidades/estados variados */
INSERT INTO [dbo].[Mesas] ([NroMesa], [Capacidad], [Estado]) VALUES
(1, 2, N'Libre'),
(2, 2, N'Libre'),
(3, 4, N'Libre'),
(4, 4, N'Ocupada'),
(5, 4, N'Libre'),
(6, 6, N'Libre'),
(7, 6, N'Reservada'),
(8, 8, N'Libre'),
(9, 8, N'Libre'),
(10, 10, N'Libre')
GO

/* 4) Cliente de prueba para poder insertar reservas de prueba con FK válida.
      DNI claramente marcado como dato de prueba; se puede borrar junto con
      las reservas de prueba una vez validado el flujo. */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Clientes] WHERE DNI = 99999999)
BEGIN
    INSERT INTO [dbo].[Clientes] ([DNI], [Nombre], [Apellido], [Email], [Telefono], [Activo])
    VALUES (99999999, N'Cliente', N'DePrueba790MY', N'prueba790my@test.com', N'0000000000', 1)
END
GO

/* 5) Reservas de prueba: ambas para la misma fecha, en turnos distintos,
      para poder verificar que el NOT EXISTS filtra por (Fecha, Hora) y no
      bloquea la mesa en un turno donde no tiene reserva. */
DECLARE @FechaPrueba date = DATEADD(DAY, 3, CAST(GETDATE() AS date));

INSERT INTO [dbo].[Reservas] ([ClienteDNI], [MesaNumero], [Fecha], [Hora], [CantidadComensales], [Estado])
VALUES (99999999, 6, @FechaPrueba, '20:00:00', 5, N'Confirmada');

INSERT INTO [dbo].[Reservas] ([ClienteDNI], [MesaNumero], [Fecha], [Hora], [CantidadComensales], [Estado])
VALUES (99999999, 9, @FechaPrueba, '22:00:00', 7, N'Confirmada');
GO

-- Verificación manual sugerida (no se ejecuta como parte del script):
-- Mesa 6 (capacidad 6) NO debería aparecer disponible para @FechaPrueba a las 20:00,
--   pero SÍ debería aparecer para @FechaPrueba a las 18:00 o 22:00.
-- Mesa 9 (capacidad 8) NO debería aparecer disponible para @FechaPrueba a las 22:00,
--   pero SÍ para 18:00 o 20:00.
