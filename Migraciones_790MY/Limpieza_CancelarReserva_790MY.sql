USE [TP_Ing_Soft]
GO
/*****************************************************************************
  Script de limpieza: elimina la patente/permiso CancelarReserva (ID = 25)
  y sus asignaciones a familias, roles y usuarios.

  Contexto:
    - Refactoring 790MY: se eliminó el módulo GUI "Consultar / Cancelar Reservas".
    - La lógica de negocio (ReservaBLL_790MY.CancelarReserva) se conserva
      internamente; solo se elimina el permiso de la BD.
    - La familia "Gestion de reservas" (FamiliaID = 10) se MANTIENE porque
      aún contiene VerReservas (24) y RegistrarReserva (26).

  Idempotencia:
    Cada bloque verifica la existencia del registro antes de actuar.
    Puede ejecutarse múltiples veces sin efectos secundarios.
*****************************************************************************/

PRINT '--- Inicio de limpieza: CancelarReserva (PermisoID = 25) ---'
GO

-- ── 1. Eliminar asignaciones usuario-permiso directas (si existen) ──────────
IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'UsuarioPermiso'
)
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[UsuarioPermiso] WHERE [PermisoID] = 25)
    BEGIN
        DELETE FROM [dbo].[UsuarioPermiso] WHERE [PermisoID] = 25
        PRINT '  [OK] Filas eliminadas de UsuarioPermiso'
    END
    ELSE
        PRINT '  [--] UsuarioPermiso: no habia asignaciones para PermisoID = 25'
END
ELSE
    PRINT '  [--] Tabla UsuarioPermiso no existe (omitida)'
GO

-- ── 2. Eliminar asignación familia-permiso ───────────────────────────────────
IF EXISTS (SELECT 1 FROM [dbo].[FamiliaPermiso] WHERE [FamiliaID] = 10 AND [PermisoID] = 25)
BEGIN
    DELETE FROM [dbo].[FamiliaPermiso]
    WHERE [FamiliaID] = 10 AND [PermisoID] = 25
    PRINT '  [OK] Fila eliminada de FamiliaPermiso (Familia 10, Permiso 25)'
END
ELSE
    PRINT '  [--] FamiliaPermiso: la asignacion ya no existe'
GO

-- ── 3. Eliminar asignaciones rol-permiso directas (tabla RolPermiso, si existe) ──
IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'RolPermiso'
)
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[RolPermiso] WHERE [PermisoID] = 25)
    BEGIN
        DELETE FROM [dbo].[RolPermiso] WHERE [PermisoID] = 25
        PRINT '  [OK] Filas eliminadas de RolPermiso'
    END
    ELSE
        PRINT '  [--] RolPermiso: no habia asignaciones para PermisoID = 25'
END
ELSE
    PRINT '  [--] Tabla RolPermiso no existe (omitida)'
GO

-- ── 4. Eliminar el permiso de la tabla maestra Permisos ─────────────────────
IF EXISTS (SELECT 1 FROM [dbo].[Permisos] WHERE [PermisoID] = 25 AND [Nombre] = 'CancelarReserva')
BEGIN
    DELETE FROM [dbo].[Permisos] WHERE [PermisoID] = 25
    PRINT '  [OK] Permiso CancelarReserva (ID 25) eliminado de Permisos'
END
ELSE
    PRINT '  [--] Permisos: CancelarReserva (ID 25) ya no existe'
GO

PRINT '--- Limpieza completada ---'
GO
