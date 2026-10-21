/* =============================================================================
   Pruebas_DeployIdempotente.sql — Deploy_TP_Ing_Soft.sql re-ejecutable (M9 / A9)
   -----------------------------------------------------------------------------
   SOLO sobre la base descartable TP_Ing_Soft_PRUEBA (NUNCA sobre TP_Ing_Soft).
   El Deploy se ejecutó desde una copia en la que "TP_Ing_Soft" se reemplazó por
   "TP_Ing_Soft_PRUEBA" (sqlcmd -S . -E -b -l 30 -f 65001 -i <copia>).
   Servidor: SQL Server 2025 (17.0.1000.7), instancia local ".". Fecha: 21/10/2026.

   Este archivo tiene dos partes:
     A) Datos "del cliente" que simulan una base en uso.
     B) Instantánea: filas + CHECKSUM_AGG(BINARY_CHECKSUM(*)) por tabla, hash
        SHA-256 de la tabla DV completa y cantidad de objetos por tipo. Se toma
        antes y después de cada ejecución del Deploy y se comparan con diff.

   ESCENARIOS Y RESULTADO
   1) Instalación nueva (base inexistente) con el Deploy nuevo vs el anterior
      (rama beta): mismas filas, mismos checksums y MISMO hash de DV en las 23
      tablas comunes. Diferencias esperadas: +1 tabla (CierresCaja, vacía),
      +1 PK, +1 índice y el trigger producto_actualizado nuevo (A5).
   2) Base con datos creada con el Deploy nuevo (+ parte A), Deploy dos veces:
        - 1.ª ejecución: sin errores, DEPLOY_OK. Única diferencia: CierresCaja
          1 -> 2 filas (copia el cierre del 19/10 que solo estaba en la bitácora;
          es la "Actualización de esquema" de A4). Resto de las tablas, DV y
          cantidad de objetos: idénticos.
        - 2.ª ejecución: sin errores, DEPLOY_OK, instantánea idéntica a la 1.ª.
   3) Actualización desde una base v1.1 (Deploy anterior, sin CierresCaja y con
      el trigger viejo) con datos (parte A sin la fila de CierresCaja), Deploy
      dos veces:
        - 1.ª ejecución: crea CierresCaja con el cierre histórico
          (2026-10-19 | 1234.50 | 0 | Maximo) y reemplaza producto_actualizado.
          Las 23 tablas existentes: mismas filas y checksums; DV_HASH idéntico
          (8CBCCC78...297F19CD); 23 FK / 19 CHECK / 16 DEFAULT (sin duplicados).
        - 2.ª ejecución: instantánea idéntica a la 1.ª.
      Después, Pruebas_BitacoraCambios.sql sobre esa base: FALLAS: 0.
   ============================================================================= */
USE [TP_Ing_Soft_PRUEBA];
GO
SET NOCOUNT ON;
GO

/* ─── A) Datos del cliente ─── */
INSERT dbo.Clientes (DNI, Nombre, Apellido, Email, Telefono, Activo, Direccion) VALUES (40111222, N'Ana', N'Pérez', N'ana@x.com', N'1155550000', 1, NULL);
INSERT dbo.EmailsAdicionalesCliente (ClienteDNI, Email) VALUES (40111222, N'ana2@x.com');
INSERT dbo.Mesas (NroMesa, Capacidad, Estado, Activo) VALUES (14, 4, N'Libre', 1);
INSERT dbo.Reservas (ClienteDNI, MesaNumero, Fecha, Hora, CantidadComensales, Estado, CamareroDNI) VALUES (40111222, 2, '2026-10-20', '20:00', 2, N'Finalizada', 30000001);
UPDATE dbo.Productos SET Precio = Precio + 1 WHERE ProductoID = 1;                   -- versión en Productos_C
UPDATE dbo.Productos SET StockPorciones = StockPorciones - 1 WHERE ProductoID = 2;
INSERT dbo.Bitacora (Username, FechaHora, Modulo, Evento, Criticidad, TargetUsername) VALUES
  (N'Maximo', '2026-10-19T23:00:00', 1, 52, 3, N'Caja 19/10/2026 - 1.234,50'),       -- cierre histórico
  (N'Maximo', '2026-10-20T10:00:00', 1, 1, 1, NULL);
-- Solo si la base ya tiene CierresCaja (escenario 2):
IF OBJECT_ID(N'dbo.CierresCaja', N'U') IS NOT NULL
    INSERT dbo.CierresCaja (Fecha, Neta, CantidadCuentas, Usuario) VALUES ('2026-10-20', 500, 1, N'Maximo');
GO

/* ─── B) Instantánea (ejecutar antes y después de cada Deploy y comparar) ─── */
DECLARE @sql nvarchar(max) = N'';
SELECT @sql = @sql + N'SELECT N''' + t.name + N''' AS Tabla, COUNT_BIG(*) AS Filas, ISNULL(CHECKSUM_AGG(BINARY_CHECKSUM(*)),0) AS Chk FROM dbo.' + QUOTENAME(t.name) + N' UNION ALL '
FROM sys.tables t WHERE t.schema_id = SCHEMA_ID('dbo') ORDER BY t.name;
SET @sql = LEFT(@sql, LEN(@sql) - 10) + N' ORDER BY Tabla';
EXEC (@sql);

SELECT 'DV_HASH' AS k,
       CONVERT(varchar(64), HASHBYTES('SHA2_256',
           (SELECT STRING_AGG(CONVERT(nvarchar(max), Tabla + N'|' + DVH + N'|' + DVV), N';') WITHIN GROUP (ORDER BY Tabla) FROM dbo.DV)), 2) AS v;

SELECT type_desc, COUNT(*) AS n FROM sys.objects WHERE is_ms_shipped = 0 GROUP BY type_desc ORDER BY type_desc;
SELECT 'IDX' AS k, COUNT(*) AS n FROM sys.indexes i JOIN sys.tables t ON t.object_id = i.object_id WHERE i.name IS NOT NULL;
SELECT 'TRG_A5' AS k,
       CASE WHEN OBJECT_DEFINITION(OBJECT_ID('dbo.producto_actualizado')) LIKE '%@Versionar%' THEN 'nuevo' ELSE 'viejo' END AS v;
GO
