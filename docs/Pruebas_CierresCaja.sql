/* =============================================================================
   Pruebas_CierresCaja.sql — Prueba de la migración AgregarCierresCaja_790MY (A4)
   -----------------------------------------------------------------------------
   SOLO sobre la base descartable TP_Ing_Soft_PRUEBA (NUNCA sobre TP_Ing_Soft).

   Preparación (desde la raíz del repo, con una copia de los scripts en la que
   TP_Ing_Soft se reemplazó por TP_Ing_Soft_PRUEBA):
     1) Deploy de la versión anterior (sin CierresCaja) -> TP_Ing_Soft_PRUEBA.
     2) Este script, sección A (eventos de cierre simulados en la bitácora).
     3) AgregarCierresCaja_790MY.sql  (dos veces: debe ser idempotente).
     4) Este script, sección B (verificación).

   Resultado de la ejecución (21/10/2026, SQL Server 2025 local):
     Fecha      | Neta      | CantidadCuentas | Usuario
     2026-10-01 | 125430.50 | 0               | Maximo
     2026-10-02 |    980.00 | 0               | Maximo     <- primer evento del día
   La segunda ejecución de la migración no agregó ni duplicó filas. El evento con
   otro código (10) y el texto que no respeta el formato se ignoran.
   ============================================================================= */
USE [TP_Ing_Soft_PRUEBA];
GO
SET NOCOUNT ON;
GO

/* ─── A) Datos: cierres históricos en la bitácora (Evento 52 = CierreCajaRealizado) ─── */
INSERT [dbo].[Bitacora] ([Username], [FechaHora], [Modulo], [Evento], [Criticidad], [TargetUsername]) VALUES
    (N'Maximo', '2026-10-01T23:00:00', 1, 52, 3, N'Caja 01/10/2026 - 125.430,50'),
    (N'Maximo', '2026-10-02T23:00:00', 1, 52, 3, N'Caja 02/10/2026 - 980,00'),
    (N'Maximo', '2026-10-02T23:05:00', 1, 52, 3, N'Caja 02/10/2026 - 999,99'),  -- duplicado del día: se ignora
    (N'Maximo', '2026-10-03T10:00:00', 1, 10, 1, N'Caja 03/10/2026 - 5,00'),    -- otro evento: se ignora
    (N'Maximo', '2026-10-04T10:00:00', 1, 52, 3, N'texto raro');                -- formato inválido: se ignora
GO

/* ─── B) Verificación (ejecutar después de la migración) ─── */
SELECT * FROM [dbo].[CierresCaja] ORDER BY [Fecha];

-- Doble cierre: la PK debe rechazarlo (error 2627, que la DAL traduce a msg_ac_caja_ya_cerrada).
BEGIN TRY
    INSERT [dbo].[CierresCaja] ([Fecha], [Neta], [CantidadCuentas], [Usuario])
    VALUES ('2026-10-01', 1, 1, N'Maximo');
    PRINT 'ERROR: se permitió un doble cierre';
END TRY
BEGIN CATCH
    PRINT CONCAT('OK doble cierre rechazado, error ', ERROR_NUMBER());
END CATCH
GO
