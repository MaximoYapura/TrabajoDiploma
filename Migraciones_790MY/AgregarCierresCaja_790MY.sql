/* =====================================================================================
   Cierres de caja con tabla propia (790MY) — hallazgo A4

   - Tabla CierresCaja: una fila por jornada cerrada. La PK (Fecha) garantiza en la base
     que un día se cierre una sola vez aunque dos terminales cierren a la vez
     (SqlCierreCajaRepository_790MY traduce la violación de PK, error 2627, a
     msg_ac_caja_ya_cerrada). Antes el cierre solo existía como evento
     CierreCajaRealizado de la bitácora y se detectaba por el prefijo de
     Bitacora.TargetUsername.
   - Carga histórica: copia los cierres ya registrados en la bitácora para no reabrir
     días cerrados. El texto tiene la forma "Caja dd/MM/yyyy - 12.345,67" (importe con
     formato es-AR). Si una fecha aparece más de una vez se toma el primer evento.
     CantidadCuentas se recalcula con los pagos de ese día (la bitácora no la guarda).
   - La tabla NO forma parte de los dígitos verificadores (DVManager_08YS.TablasMonitoreadas
     / sp_GetAllTablesParaDV): no hace falta invalidar la tabla DV.

   Idempotente: puede ejecutarse más de una vez.
   ===================================================================================== */
USE [TP_Ing_Soft]
GO

SET NOCOUNT ON
GO

/* ───────────────────────── 1) Tabla ───────────────────────── */
IF OBJECT_ID(N'[dbo].[CierresCaja]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[CierresCaja](
        [Fecha]           [date]           NOT NULL,
        [Neta]            [decimal](12, 2) NOT NULL,
        [CantidadCuentas] [int]            NOT NULL,
        [Usuario]         [nvarchar](50)   NOT NULL,
     CONSTRAINT [PK_CierresCaja] PRIMARY KEY CLUSTERED ([Fecha] ASC)
    ) ON [PRIMARY]
    PRINT 'Tabla CierresCaja creada.'
END
GO

/* ───────────────────────── 2) Cierres históricos de la bitácora ─────────────────────────
   Evento 52 = Evento.CierreCajaRealizado (Service_08YS/Entities/Bitacora/BitacoraEvento_08YS.cs). */
;WITH Cierres AS (
    SELECT TRY_CONVERT(date, SUBSTRING(b.TargetUsername, 6, 10), 103) AS Fecha,
           LTRIM(SUBSTRING(b.TargetUsername, 19, 50))               AS Importe,
           b.Username,
           ROW_NUMBER() OVER (PARTITION BY SUBSTRING(b.TargetUsername, 6, 10) ORDER BY b.Id) AS Orden
    FROM [dbo].[Bitacora] b
    WHERE b.Evento = 52
      AND b.TargetUsername LIKE N'Caja [0-9][0-9]/[0-9][0-9]/[0-9][0-9][0-9][0-9] - %'
)
INSERT INTO [dbo].[CierresCaja] ([Fecha], [Neta], [CantidadCuentas], [Usuario])
SELECT c.Fecha,
       ISNULL(TRY_CONVERT(decimal(12, 2), REPLACE(REPLACE(c.Importe, N'.', N''), N',', N'.')), 0),
       (SELECT COUNT(1)
          FROM [dbo].[Pagos] p
          INNER JOIN [dbo].[Facturas] f ON f.NroFactura = p.NroFactura
         WHERE p.FechaHoraPago >= c.Fecha
           AND p.FechaHoraPago <  DATEADD(DAY, 1, c.Fecha)
           AND f.Estado = N'Pagada'),
       c.Username
FROM Cierres c
WHERE c.Orden = 1
  AND c.Fecha IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM [dbo].[CierresCaja] x WHERE x.Fecha = c.Fecha)
GO

PRINT '--- Migración Cierres de Caja completada ---'
GO
