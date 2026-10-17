/* =====================================================================================
   Reporte Inteligente: Análisis Predictivo de Canasta (790MY)

   - Patente GestionReportePredictivo (debe coincidir con Permisos.GestionReportePredictivo
     en Service_08YS/Entities/Acceso/Permiso_08YS.cs).
   - Familia "Reportes predictivos" que la agrupa.
   - Asignación de la familia al rol SuperAdmin. Los usuarios no tienen patentes
     directas: las heredan de su rol, así que el usuario Maximo (rol SuperAdmin) la
     recibe por esta vía. Se verifica al final.

   Idempotente: puede ejecutarse más de una vez.
   Requiere: CompletarFacturacionCobro_790MY.sql.
   ===================================================================================== */
USE [TP_Ing_Soft]
GO

SET NOCOUNT ON
GO

/* ───────────────────────── 1) Patente ───────────────────────── */
INSERT INTO [dbo].[Permisos] ([Nombre], [Descripcion])
SELECT v.Nombre, v.Descripcion
FROM (VALUES
    (N'GestionReportePredictivo', N'Permite consultar el análisis predictivo de canasta (reglas de asociación y sugerencias)')
) AS v(Nombre, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Permisos] p WHERE p.Nombre = v.Nombre)
GO

/* ───────────────────────── 2) Familia ───────────────────────── */
INSERT INTO [dbo].[Familias] ([Nombre])
SELECT v.Nombre
FROM (VALUES (N'Reportes predictivos')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Familias] f WHERE f.Nombre = v.Nombre)
GO

INSERT INTO [dbo].[FamiliaPermiso] ([FamiliaID], [PermisoID])
SELECT f.FamiliaID, p.PermisoID
FROM [dbo].[Familias] f
INNER JOIN [dbo].[Permisos] p ON p.Nombre = N'GestionReportePredictivo'
WHERE f.Nombre = N'Reportes predictivos'
  AND NOT EXISTS (SELECT 1 FROM [dbo].[FamiliaPermiso] fp WHERE fp.FamiliaID = f.FamiliaID AND fp.PermisoID = p.PermisoID)
GO

/* ───────────────────────── 3) Rol SuperAdmin ───────────────────────── */
INSERT INTO [dbo].[RolFamilia] ([RolID], [FamiliaID])
SELECT r.RolID, f.FamiliaID
FROM [dbo].[Roles] r
INNER JOIN [dbo].[Familias] f ON f.Nombre = N'Reportes predictivos'
WHERE (r.Nombre = N'SuperAdmin' OR r.RolID = 1)
  AND NOT EXISTS (SELECT 1 FROM [dbo].[RolFamilia] rf WHERE rf.RolID = r.RolID AND rf.FamiliaID = f.FamiliaID)
GO

/* ───────────────────────── 4) Usuario Maximo ─────────────────────────
   Si Maximo no tuviera el rol SuperAdmin, la patente no le llegaría: se avisa. */
IF NOT EXISTS (
    SELECT 1
    FROM [dbo].[Users] u
    INNER JOIN [dbo].[RolFamilia] rf ON rf.RolID = u.RolID
    INNER JOIN [dbo].[FamiliaPermiso] fp ON fp.FamiliaID = rf.FamiliaID
    INNER JOIN [dbo].[Permisos] p ON p.PermisoID = fp.PermisoID
    WHERE u.Username = N'Maximo' AND p.Nombre = N'GestionReportePredictivo')
    RAISERROR('ATENCION: el usuario Maximo no recibe GestionReportePredictivo (verifique que tenga el rol SuperAdmin).', 16, 1)
ELSE
    PRINT 'Usuario Maximo: patente GestionReportePredictivo asignada vía rol SuperAdmin.'
GO

/* ───────────────────────── 5) Invalidar DV ─────────────────────────
   Permisos, Familias, FamiliaPermiso y RolFamilia cambiaron: la aplicación vuelve a
   calcular los dígitos verificadores (DVManager_08YS.Recalcular). */
DELETE FROM [dbo].[DV]
GO

PRINT '--- Migración Reporte Predictivo completada ---'
GO
