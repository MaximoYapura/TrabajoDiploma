/* =============================================================================
   Seed_TP_Ing_Soft.sql — Base semilla limpia de "El Fogón del Sur"
   -----------------------------------------------------------------------------
   Deja la base TP_Ing_Soft lista para una instalación limpia / demostración:

     1. Vacía las tablas transaccionales y de auditoría (Bitacora, Pagos,
        ItemsFactura, Facturas, ItemsPedido, Pedidos, Reservas) y reajusta sus
        contadores IDENTITY para que la numeración vuelva a empezar en 1.
     2. Deja un único cliente precargado (Cliente Frecuente).
     3. Deja un único empleado de cada tipo (2 camareros, 1 recepcionista, 1 chef).
     4. Deja un único usuario del sistema: Maximo (SuperAdmin).
     5. NO toca: Productos, Productos_C, Mesas, Roles, Permisos, Familias,
        FamiliaPermiso, FamiliaIntegrada, RolFamilia, RolPermiso.
     6. Deja los Dígitos Verificadores (tabla DV) consistentes con los datos
        resultantes, para que el primer login no informe inconsistencia.

   Requisitos: la base TP_Ing_Soft ya debe existir con todas las migraciones
   aplicadas (Migraciones_790MY). El script es idempotente: puede ejecutarse
   más de una vez y siempre deja el mismo estado.

   Ejecución manual (el instalador lo hace con estos mismos parámetros):
       sqlcmd -S . -E -b -f 65001 -i Seed_TP_Ing_Soft.sql
   ============================================================================= */

SET NOCOUNT ON;
SET XACT_ABORT ON;
-- sqlcmd arranca con QUOTED_IDENTIFIER OFF; la base tiene índices filtrados que lo requieren ON.
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
GO

IF DB_ID(N'TP_Ing_Soft') IS NULL
BEGIN
    RAISERROR(N'La base TP_Ing_Soft no existe en esta instancia. Créela y aplique las migraciones antes de ejecutar la semilla.', 16, 1);
    SET NOEXEC ON;
END
GO

USE [TP_Ing_Soft];
GO

IF OBJECT_ID(N'dbo.Clientes') IS NULL OR OBJECT_ID(N'dbo.Empleados') IS NULL
   OR OBJECT_ID(N'dbo.Facturas') IS NULL OR OBJECT_ID(N'dbo.sp_UpsertDVTabla') IS NULL
BEGIN
    RAISERROR(N'TP_Ing_Soft no tiene el esquema completo (faltan tablas o SPs). Aplique las migraciones de Migraciones_790MY antes de la semilla.', 16, 1);
    SET NOEXEC ON;
END
GO

BEGIN TRANSACTION;

/* -----------------------------------------------------------------------------
   1) Vaciar tablas transaccionales y de auditoría
      El orden respeta las claves foráneas (primero los hijos).
   ----------------------------------------------------------------------------- */
DELETE FROM Bitacora;
DELETE FROM ItemsFactura;
DELETE FROM Pagos;
DELETE FROM Facturas;
DELETE FROM ItemsPedido;
DELETE FROM Pedidos;
DELETE FROM Reservas;

/* Reajuste de los contadores IDENTITY.
   ItemsFactura e ItemsPedido no tienen IDENTITY (su clave es NroFactura/NroPedido
   + NroItem), así que no requieren reajuste.
   DBCC CHECKIDENT ... RESEED, 0 hace que el próximo valor sea 1 en una tabla que
   ya tuvo filas; en una tabla que nunca tuvo filas el próximo valor sería 0, por
   eso se reseedea a (seed - incremento) solo si la tabla ya usó su IDENTITY. */
DECLARE @Reseed TABLE (Tabla sysname PRIMARY KEY);
INSERT INTO @Reseed (Tabla) VALUES
    (N'dbo.Bitacora'), (N'dbo.Pagos'), (N'dbo.Facturas'), (N'dbo.Pedidos'), (N'dbo.Reservas'),
    (N'dbo.EmailsAdicionalesCliente'), (N'dbo.TelefonosAdicionalesCliente');

DECLARE @Tabla sysname, @Valor bigint, @Sql nvarchar(400);
WHILE EXISTS (SELECT 1 FROM @Reseed)
BEGIN
    SELECT TOP (1) @Tabla = Tabla FROM @Reseed ORDER BY Tabla;

    SELECT @Valor = CASE WHEN ic.last_value IS NULL
                         THEN CONVERT(bigint, ic.seed_value)
                         ELSE CONVERT(bigint, ic.seed_value) - CONVERT(bigint, ic.increment_value) END
    FROM sys.identity_columns ic
    WHERE ic.object_id = OBJECT_ID(@Tabla);

    SET @Sql = N'DBCC CHECKIDENT (''' + @Tabla + N''', RESEED, ' + CONVERT(nvarchar(20), @Valor) + N') WITH NO_INFOMSGS;';
    EXEC sys.sp_executesql @Sql;

    DELETE FROM @Reseed WHERE Tabla = @Tabla;
END

/* -----------------------------------------------------------------------------
   2) Clientes: un único cliente precargado
      Primero los datos de contacto adicionales (FK a Clientes).
   ----------------------------------------------------------------------------- */
DELETE FROM EmailsAdicionalesCliente;
DELETE FROM TelefonosAdicionalesCliente;
DELETE FROM Clientes;

INSERT INTO Clientes (DNI, Nombre, Apellido, Email, Telefono, Activo, Direccion)
VALUES (30000000, N'Cliente', N'Frecuente', N'cliente@fogon.com', N'1100000000', 1, N'Av. Siempre Viva 123');

/* -----------------------------------------------------------------------------
   3) Empleados: uno de cada tipo
      Solo Maximo Yapura queda vinculado a un usuario del sistema (Users.DNI 12345678,
      Username "Maximo"). Las reservas ya fueron borradas, así que no hay FKs
      (Reservas.CamareroDNI) que impidan reemplazar el plantel.
   ----------------------------------------------------------------------------- */
DELETE FROM Empleados;

INSERT INTO Empleados (DNI, Nombre, Apellido, Puesto, UsuarioDNI, Activo) VALUES
    (12345678, N'Maximo',  N'Yapura', N'Camarero',      12345678, 1),
    (22222222, N'Pedro',   N'Pascal', N'Recepcionista', NULL,     1),
    (30000001, N'Lucas',   N'Gomez',  N'Camarero',      NULL,     1),
    (30000002, N'Martina', N'Ruiz',   N'Chef',          NULL,     1);

/* -----------------------------------------------------------------------------
   4) Users: únicamente "Maximo" (DNI 12345678) con rol SuperAdmin (RolID 1)
      Se eliminan los usuarios de prueba (22222222GestorDeAccesos, Supervisor,
      Gerente, etc.). Las FKs hacia Users (Bitacora, Empleados.UsuarioDNI) ya no
      apuntan a otros usuarios: la bitácora se vació y el plantel se reemplazó.
      La fila de Maximo se fija completa (incluido Hash/Salt de la contraseña de
      demostración, el mismo que ya figura en script.sql) para que su DV sea
      determinístico: la semilla también restablece esa contraseña y lo desbloquea.
   ----------------------------------------------------------------------------- */
DELETE FROM Users WHERE DNI <> 12345678;

IF EXISTS (SELECT 1 FROM Users WHERE DNI = 12345678)
    UPDATE Users
    SET Username = N'Maximo', RolID = 1, Nombre = N'Maximo', Apellido = N'Yapura',
        Email = N'Maximo@gmail.com',
        Hash = N'N4Rf9Gw3hwxmXiqY4tp1IFxaOezlZdSiuYkDZqKOGFQ=', Salt = N'CWu49+d6I6N7m98eZq7sUw==',
        Bloqueado = 0, Activo = 1, Idioma = 'es'
    WHERE DNI = 12345678;
ELSE
    INSERT INTO Users (DNI, Username, RolID, Nombre, Apellido, Email, Hash, Salt, Bloqueado, Activo, Idioma)
    VALUES (12345678, N'Maximo', 1, N'Maximo', N'Yapura', N'Maximo@gmail.com',
            N'N4Rf9Gw3hwxmXiqY4tp1IFxaOezlZdSiuYkDZqKOGFQ=', N'CWu49+d6I6N7m98eZq7sUw==', 0, 1, 'es');

/* -----------------------------------------------------------------------------
   5) Se conservan intactos
      Productos, Productos_C, Mesas y la estructura de permisos (Roles, Permisos,
      Familias, FamiliaPermiso, FamiliaIntegrada, RolFamilia, RolPermiso).
      Sus filas en DV siguen siendo válidas.
   ----------------------------------------------------------------------------- */

/* -----------------------------------------------------------------------------
   6) Dígitos Verificadores (DVH / DVV)
      DvBLL_08YS calcula los DV en C#: concatena las celdas fila por fila (DVH) y
      columna por columna (DVV) y cifra el resultado con AES-256-CBC
      (DvEncriptador_08YS, IV derivado del nombre de la tabla). Ese cálculo no
      puede reproducirse en T-SQL, pero como el contenido de las tablas que
      modifica esta semilla queda totalmente determinado, sus DV también: los
      valores de abajo se generaron con DvBLL_08YS.CalcularYCifrar sobre el
      estado exacto que deja este script.

      Si en el futuro se cambian los datos de la semilla, hay que regenerar estos
      valores; mientras tanto, la aplicación los corrige sola: al iniciar sesión
      detecta la inconsistencia y el administrador puede usar "Recalcular DV" en
      la pantalla de inconsistencia de la base de datos (FormInconsistenciaDB_08YS).
   ----------------------------------------------------------------------------- */
EXEC dbo.sp_UpsertDVTabla @Tabla = N'Users',
    @DVH = N'wSQ6L6QDrv14dS8KCBE8KPeg7vFHZ8SuIy/vcqwvMM7BegE8beOAmBT0A+DOMNDx5dBTSgqrzFn76/58o28tGTxc8CktpgWVtQMhyQKzoJWasnnE1xd0dyHcML/fHpcVL9KZGDXsRyz0gXKppj8F4j+rYIAb97fKdJVTC7WGow0=',
    @DVV = N'xioQuckAuprBSBozkpWWjMbHO0S0sb8mvPhZgo8b/+UqGRCfJHJJ2t9dFP8ZD14TSts/ui66udUY/87CkIfna92zwsgaz6e5zcelEPeEosD4SfLBGwonzxaHmq4duk/yNScguGASVjpPz8ap6+wQ/Syph+Z8NR/OlKe3q4MrHwg=';
EXEC dbo.sp_UpsertDVTabla @Tabla = N'Clientes',
    @DVH = N'FfueLzU+cesSdj5UiBiircXdpdTj5j0N4ypXO03dN6D01sJ6oGRcA5kouJIdVb34qiBNSTuVjHSG8cUVhLzbs5Zw3bF02hndW7rE4FJwfvM=',
    @DVV = N'AT9IHFWLP85lcxCiz0Ib+4eeyZLDC1MCt7l0y2pUSi7qgJ+ZZCCFgYxpTmAlQxEviD+FBA5A/bw+6eXyABkYFOM9+DnlldEm8a0FX20PB+U=';
EXEC dbo.sp_UpsertDVTabla @Tabla = N'Reservas',
    @DVH = N'UZnK9nko0Rk5Hm4urEiuGw==',
    @DVV = N'gC4Lra1Y1YFf4XoAfkCKyA==';
EXEC dbo.sp_UpsertDVTabla @Tabla = N'Empleados',
    @DVH = N'76bUYYAUD637R5lRsWcnFR/IoksAyVChXoFUOuFQsFyN0e7IO/W13losSeAu12HBRjgsGG9UNoZcdwh/YgCy++hA3LPWhl8lO7UgA6Enme/CwVCIVdtIFW09cCwBHK7CvF/LbUUiIR1ncWipPNRkrrt9OZ9cbuSE8NscGRlG5jU=',
    @DVV = N'oo2A994cFMBK6lLG23rJduaUtwrCxx2dZ69BmzrzRtOA4GwOBhDZRz224fB6ZPdrpNChgJunx3RU6gzrRIl9k+0LOXfYkQrBuKxpxS7pnGwvqTM+51IAvFDAkFEvRj+/+i89JdJ+dS8LmLOSby/i969HwKD7dgBKuNCQH3OYYFo=';
EXEC dbo.sp_UpsertDVTabla @Tabla = N'Pedidos',
    @DVH = N'dpAcLn4Rq+yA9YGDve5fow==',
    @DVV = N'dX58yhZ/DvfN+tU39iQuEg==';
EXEC dbo.sp_UpsertDVTabla @Tabla = N'ItemsPedido',
    @DVH = N'oii61PhTuH5EvJODQSz59g==',
    @DVV = N'cRryy1i2VN7QOUJcUSWbIg==';
EXEC dbo.sp_UpsertDVTabla @Tabla = N'Facturas',
    @DVH = N'ptmmpmry2Awc6TsYuuKCHA==',
    @DVV = N'ZUfQd7PynYmY/krYVmYsKg==';
EXEC dbo.sp_UpsertDVTabla @Tabla = N'ItemsFactura',
    @DVH = N'vu221HBsFSFs7jUfe2C+qw==',
    @DVV = N'DGaXUzHNGCgAbivIjGIaeA==';
EXEC dbo.sp_UpsertDVTabla @Tabla = N'Pagos',
    @DVH = N'WkAcY27jlgMAbeZqgwgYVQ==',
    @DVV = N'5DLSMIg0s1ANg6OS2g/2RQ==';

COMMIT TRANSACTION;
GO

PRINT N'Semilla aplicada: TP_Ing_Soft quedó limpia y con los DV consistentes.';
GO

SET NOEXEC OFF;
GO
