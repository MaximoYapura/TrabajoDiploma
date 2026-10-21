# Informe — Plan de limpieza de Deuda Técnica (4 fases)

Ejecución autónoma del plan `PROMPT_Deuda_Tecnica.md` (21/10/2026). Las cuatro fases se completaron; ninguna se detuvo por errores de compilación.

> **Antes de abrir la app con tu base de desarrollo (`TP_Ing_Soft`), leé el paso 0 de la checklist.** Nunca ejecuté nada contra `TP_Ing_Soft`. El código nuevo necesita la tabla `CierresCaja`, y tu base todavía no la tiene.

## Entorno, compilación y base de prueba

| | |
|---|---|
| Compilador | MSBuild de Visual Studio 18 Community (`C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\amd64\MSBuild.exe`) |
| Comando | `MSBuild.exe TrabajoDiploma.slnx -restore -p:RestorePackagesConfig=true -p:Configuration=Debug -m -v:minimal` |
| Resultado | **0 errores** al terminar cada ítem de código y al cerrar cada fase. Al final también compila en **Release** (misma línea con `-p:Configuration=Release`) |
| Instalador | `ISCC.exe /Q /O<carpeta temporal> Installer_TP_Fogon.iss` → **compila** (Inno Setup 6, en `%LOCALAPPDATA%\Programs\Inno Setup 6`). El setup no se ejecutó |
| SQL Server | `sqlcmd` + instancia local `.` (SQL Server 2025, 17.0.1000.7) |
| Base de prueba | `TP_Ing_Soft_PRUEBA`, creada y borrada varias veces a partir de copias de los scripts con el nombre de la base reemplazado. **Al final quedó borrada.** La base real `TP_Ing_Soft` no se tocó |
| JSON | `GUI/idiomas.json` validado con Node (parseo + claves duplicadas por idioma): es 1294 → 1296, en 1295 → 1297, sin duplicados. La única clave que está en un solo idioma es `"-"` (solo en `en`), y ya estaba antes |

---

## FASE 1: Validaciones y permisos en la BLL (A1, A2, A3) · rama `deuda-tecnica/fase1`

**Qué se hizo**
- Primer commit: `Documentacion_Defensa_Fogon.md`, solo ese archivo.
- **A2.** `ReservaBLL_790MY.CancelarReserva` valida `Permisos.RegistrarReserva` en la primera línea. Se corrigió el comentario, que decía que la validación se había eliminado. El método no tiene llamadores en la GUI.
- **A3.** En `FamiliaBLL_08YS` y `RolBLL_08YS`, `ValidatePermission` pasó a ser lo primero en `Crear`, `Modificar` y `Eliminar`. Los seis métodos ya validaban el permiso; solo se movió la línea. El resto del orden no cambió.
- **A1 (clientes).** En `ClienteBLL_790MY.ValidarDatos`, `dni <= 0` (texto fijo en español) se reemplazó por `^\d{7,8}$`. El error es `MensajesBLL_790MY.Regla("FC_val_dni_formato")`, que se reutiliza. El patrón también rechaza 0 y los negativos.
- **A1 (reservas).** `ReservaBLL_790MY.Buscar` lanza `msg_rep_rango_fechas_invalido` (clave nueva en es y en) cuando las dos fechas vienen informadas y `desde.Date > hasta.Date`. `FormReporteReservas_790MY` ya atrapa `ArgumentException` y la muestra en el banner: *"No se pudieron buscar las reservas: La fecha "Desde" (…) no puede ser posterior…"*.

**Archivos:** `BLL/ReservaBLL_790MY.cs`, `BLL/FamiliaBLL_08YS.cs`, `BLL/RolBLL_08YS.cs`, `BLL/ClienteBLL_790MY.cs`, `GUI/idiomas.json`, `Documentacion_Defensa_Fogon.md`.

**Llamadores revisados (A1).** Validan con `ValidarDatos` solo `RegistrarCliente` y `ActualizarCliente`, que llaman `FormClientes_790MY` y `FormRegistrarCliente_790MY` con el mismo regex. La importación XML (`DeserializarArchivoXML`) **solo lista** los clientes en pantalla y no persiste, así que la regla no la afecta.

**Riesgo.** Si en una base hay clientes cargados antes con DNI de 6 o de 9 o más dígitos, no se van a poder **modificar** (`ActualizarCliente`) hasta corregirlos. El DNI es la PK y la GUI no lo deja editar.

## FASE 2: Modelo de datos (A4, A5, A6) · rama `deuda-tecnica/fase2`

**Qué se hizo**
- **A4: `CierresCaja`** (`Fecha date` PK, `Neta decimal(12,2)`, `CantidadCuentas int`, `Usuario nvarchar(50)`).
  - DAL: `ICierreCajaRepository_790MY` + `SqlCierreCajaRepository_790MY`. `Registrar` devuelve `false` ante el error 2627 (violación de PK) y `Existe(fecha)` consulta la tabla. Está registrado en `DAL_08YS.csproj` y en `BLLFactory_790MY.CreatePagoBLL`, y `PagoBLL_790MY` lo recibe por constructor. En la BLL no hay SQL.
  - `PagoBLL_790MY.CerrarCaja` inserta en la tabla. Si el repositorio devuelve `false` (doble cierre o carrera entre terminales), lanza `msg_ac_caja_ya_cerrada`. Se mantienen la consulta previa, que da el mensaje sin intentar el INSERT, y el evento de bitácora `CierreCajaRealizado` con **el mismo texto** (`"Caja dd/MM/yyyy - importe"`).
  - `EstaCajaCerrada` consulta `CierresCaja`. Se eliminó `PrefijoCierre`; su texto quedó en línea dentro del evento.
  - Migración `Migraciones_790MY/AgregarCierresCaja_790MY.sql` (idempotente). Crea la tabla y copia los cierres históricos de la bitácora (Evento 52, parseo `Caja dd/MM/yyyy - 12.345,67`). La misma definición está en `docs/Deploy_TP_Ing_Soft.sql`.
  - No se agregó a `DVManager_08YS.TablasMonitoreadas` (queda como pendiente).
- **A5: trigger `producto_actualizado`.** En un INSERT siempre crea la versión. En un UPDATE crea una versión nueva solo si cambió `Nombre`, `Precio`, `Categoria`, `Activo` o `ControlaStock`. Si solo cambió `StockPorciones`, o no cambió nada, actualiza el stock de la fila vigente. También versiona si el producto no tiene fila vigente.
  - **Rollback:** durante `ActivarVersion` hay dos filas `Act = 1`. Si la versión restaurada difiere solo en stock, queda vigente la fila de mayor ID con el stock restaurado y la reactivada vuelve a `Act = 0`. Siempre queda **una sola** `Act = 1`.
  - Se entrega en `Migraciones_790MY/VersionarSoloCambiosNegocio_790MY.sql` y en el Deploy.
  - Revisión de código: `FormBitacoraCambios_790MY` sigue coherente. Después de restaurar selecciona la vigente del producto (`Cargar(seleccionarProductoVigente)`) y el diff compara la seleccionada contra la vigente. `ProductoCBLL_790MY.RestaurarVersion` no cambió de lógica; solo se ajustaron los comentarios de ambos.
- **A6.** `BitacoraBLL_08YS.GetAll` y `Filtrar` validan `Permisos.VerBitacora`. Llamadores: el único es `FormBitacora_08YS`, que el MDI abre después de `ValidatePermission(Permisos.VerBitacora)` (`FormMDI_08YS.cs:540`). El otro llamador anterior, `PagoBLL.EstaCajaCerrada`, dejó de usar `Filtrar` en A4. No se tocaron `ContarIntentosFallidos` ni `RegistrarEvento*`.

**Archivos:** `BLL/PagoBLL_790MY.cs`, `BLL/BLLFactory_790MY.cs`, `BLL/BitacoraBLL_08YS.cs`, `BLL/ProductoCBLL_790MY.cs` (comentario), `DAL/DAL_08YS.csproj`, `DAL/Interfaces_Repositories/ICierreCajaRepository_790MY.cs`, `DAL/SQL/SqlCierreCajaRepository_790MY.cs`, `GUI/Admin/FormBitacoraCambios_790MY.cs` (comentario), `Migraciones_790MY/AgregarCierresCaja_790MY.sql`, `Migraciones_790MY/VersionarSoloCambiosNegocio_790MY.sql`, `docs/Deploy_TP_Ing_Soft.sql`, `docs/Pruebas_CierresCaja.sql`, `docs/Pruebas_BitacoraCambios.sql`.

**Pruebas SQL (ejecutadas en `TP_Ing_Soft_PRUEBA`)**

| Script | Resultado |
|---|---|
| `docs/Pruebas_CierresCaja.sql` | La migración corrió **dos veces** y no duplicó filas. Copió los dos cierres válidos e ignoró el duplicado del mismo día, el evento con otro código y el texto con formato inválido. El doble INSERT da el error **2627** |
| `docs/Pruebas_BitacoraCambios.sql` | **10/10 OK** con el trigger nuevo: (f) alta, (a) precio, (b) stock, (b2) varios productos y producto sin vigente, (b3) UPDATE sin cambios, (c) rollback de negocio, (d) rollback solo-stock, (e) borrado, invariante. Con el trigger viejo: **6 FALLAS**, como se esperaba. Corre en una transacción que se revierte |

## FASE 3: Preparación para producción (A7, A8, M3, B1) · rama `deuda-tecnica/fase3`

**Qué se hizo**
- **A7.** En `FormFacturacionCobro_790MY`, `await Task.Delay(LatenciaSimuladaMs)` solo corre si `ModoDemostracionActivo()` es verdadero. Lee `ConfigurationManager.AppSettings["ModoDemostracion"]` con `bool.TryParse` y atrapa `ConfigurationErrorsException`, igual que `PanelAyuda_UC_790MY`. Si la clave falta o es inválida, se toma `false` y no hay demora. `IniciarProcesamiento/FinalizarProcesamiento`, el estado "Procesando" y el bloqueo de navegación no cambiaron. No hubo cambios visuales, así que el `.Designer.cs` no se tocó.
- **A8.** `Installer_TP_Fogon.iss` tiene la tarea `mododemo`, *"Instalar en modo demostración (…)"*, con `Flags: unchecked`. `ConfigurarModoDemostracion` reutiliza el mecanismo PowerShell + `XmlDocument` (`PreserveWhitespace`) de `ConfigurarConnectionString`: escribe `appSettings/ModoDemostracion` = `false` o `true`, y crea la clave si falta. Se llama en `ssPostInstall`, también en reinstalación. En una reinstalación, Inno Setup propone la elección anterior (`UsePreviousTasks`). Quedó documentado en el encabezado del `.iss`. `GUI/Config/App.config` sigue en `true`.
  - El script PowerShell se probó sobre copias de `GUI_08YS.exe.config`: con la clave existente cambia solo el valor; con la clave ausente la agrega.
- **M3.** Los tres formularios ya no usan `MessageBox` salvo en el camino de error fatal, a través de `EstiloRF2_790MY.MostrarErrorEnBanner`. El inventario está más abajo.
  - Se agregaron `Banner_UC_790MY` (y `ErrorProvider` donde corresponde) en los `.Designer.cs`, dentro de `InitializeComponent()`, con `this.components = new Container()`.
  - `FormGestionAcceso` y `FormAccesoAM` usan layout con Dock: el banner va con `Dock = Top` entre el encabezado y el contenido, insertado en `Controls.Add` entre `pnlBottom` y `pnlHeader`, como en `FormClientes_790MY`.
  - `FormBitacora_08YS` usa layout absoluto: el banner tiene posición fija `(865, 30)`, 580×44, en el hueco a la derecha del título. Crece hacia abajo si el mensaje ocupa varias líneas.
  - No cambiaron namespaces, clases parciales ni nombres de controles. `TraducirControles`/`UpdateIdioma` siguen funcionando: el banner no tiene `Tag`.
  - Clave nueva `msg_acceso_en_uso` (es y en).
- **B1.** Se eliminaron `EmpleadoBLL_790MY.ObtenerUsuariosVinculables`, `IMesaRepository_790MY.Delete` y `TieneReservasActivas`, con su implementación en `SqlMesaRepository_790MY`, que era la única. Una búsqueda en toda la solución (.cs, .Designer, docs, README) dejó solo las menciones de `Documentacion_Defensa_Fogon.md`, que se actualizaron (§2.2 y §2.3). `_userRepository` sigue en uso (`Exists`) y no se tocó.

**Archivos:** `GUI/RF2/FormFacturacionCobro_790MY.cs`, `Installer_TP_Fogon.iss`, `GUI/Admin/FormBitacora_08YS(.Designer).cs`, `GUI/Admin/FormGestionAcceso_08YS(.Designer).cs`, `GUI/Admin/FormAccesoAM_08YS(.Designer).cs`, `GUI/idiomas.json`, `BLL/EmpleadoBLL_790MY.cs`, `DAL/Interfaces_Repositories/IMesaRepository_790MY.cs`, `DAL/SQL/SqlMesaRepository_790MY.cs`, `Documentacion_Defensa_Fogon.md`.

### Inventario M3

Los "35 usos" de la guía son **35 líneas que contienen `MessageBox`** (8 + 9 + 18), y corresponden a **25 llamadas a `MessageBox.Show`**.

| # | Formulario / lugar | Antes | Ahora |
|---|---|---|---|
| 1 | Bitácora · `CargarGrid` catch | `MessageBox.Show(ex.Message)` | `MostrarErrorEnBanner` (permiso/regla → banner; inesperado → MessageBox fatal) |
| 2 | Bitácora · Exportar sin datos | MessageBox advertencia | Banner **Aviso** `msg_no_datos_exportar` |
| 3 | Bitácora · `btnFiltrar` catch | `MessageBox.Show(ex.Message)` | `MostrarErrorEnBanner` |
| 4 | Bitácora · `msg_fecha_partida_req` | MessageBox | `ErrorProvider` en `dtpDesde` + banner |
| 5 | Bitácora · `msg_fecha_fin_req` | MessageBox | `ErrorProvider` en `dtpHasta` + banner |
| 6 | Bitácora · `msg_fecha_partida_futura` | MessageBox | `ErrorProvider` en `dtpDesde` + banner |
| 7 | Bitácora · `msg_fecha_fin_futura` | MessageBox | `ErrorProvider` en `dtpHasta` + banner |
| 8 | Bitácora · `msg_fechas_invertidas` | MessageBox | `ErrorProvider` en `dtpDesde` + banner |
| 9 | GestiónAcceso · `CargarGrid` catch | MessageBox + `ex.Message` | `MostrarErrorEnBanner` |
| 10 | GestiónAcceso · confirmar eliminación | MessageBox OK/Cancel | **`ConfirmModal_790MY.Confirmar`** (botón "Eliminar"; el foco arranca en Cancelar) |
| 11 | GestiónAcceso · `ComponenteEnUsoException` | MessageBox advertencia | Banner **Aviso** `msg_acceso_en_uso` (clave nueva, con el nombre) |
| 12 | GestiónAcceso · Eliminar catch | MessageBox + `ex.Message` | `MostrarErrorEnBanner` |
| 13 | AccesoAM · Agregar sin selección | MessageBox info | Banner **Aviso** |
| 14 | AccesoAM · Agregar: componente nulo | MessageBox error | Banner **Error** |
| 15 | AccesoAM · Sugerencia de reemplazo | MessageBox OK/Cancel | **`ConfirmModal`** (botón "Aceptar") |
| 16 | AccesoAM · Conflicto irresoluble | MessageBox error | Banner **Error** (mensaje traducido de la BLL) |
| 17 | AccesoAM · Quitar sin selección | MessageBox info | Banner **Aviso** |
| 18 | AccesoAM · Quitar: componente nulo | MessageBox error | Banner **Error** |
| 19–22 | AccesoAM · Familia creada / modificada, Rol creado / modificado | MessageBox éxito ×4 | **`ToastHost_790MY`** (vía `MostrarExitoToast`) sobre el contenedor principal |
| 23 | AccesoAM · `NombreDuplicadoException` | MessageBox | `ErrorProvider` en `txtNombre` + banner |
| 24 | AccesoAM · `PermisosDuplicadosException` | MessageBox | Banner |
| 25 | AccesoAM · Guardar catch genérico | MessageBox + `ex.Message` | `InvalidOperationException` (validaciones de `AccesoBLL`, ya traducidas) → banner; el resto → `MostrarErrorEnBanner` |

Además, en `AccesoAM` se agregó una prevalidación en la GUI con las mismas claves de la BLL. Nombre vacío: `ErrorProvider` + banner (`bll_val_nombre_vacio`). Sin componentes: banner (`bll_val_sin_componentes`). La BLL las sigue validando.

Quedaron **sin uso** (no se borraron del JSON): `msg_error_cargar_datos`, `sin_seleccion`, `no_se_puede_agregar`, `msg_componente_en_uso_error`, `no_se_puede_eliminar`.

## FASE 4: Reinstalación segura con SQL idempotente (M9, A9) · rama `deuda-tecnica/fase4`

**Qué se hizo**
- **`docs/Deploy_TP_Ing_Soft.sql` idempotente.** Las guardas se generaron con un transformador lote por lote y se verificaron por conteo: 24 tablas, 10 índices, 16 DEFAULT, 19 CHECK, 23 FK, 36 `CHECK/NOCHECK CONSTRAINT`, 18 objetos programables y 12 lotes de semilla.
  - Tablas: `IF OBJECT_ID(N'dbo.X', N'U') IS NULL BEGIN … END`.
  - Tipo `IntList`: `TYPE_ID`.
  - Índices: `sys.indexes`.
  - CHECK: por nombre.
  - **DEFAULT y FK, por columna.** Una columna admite un solo DEFAULT, y varias FK del script no tienen nombre fijo: el sistema les asigna uno distinto en cada base.
  - `CHECK/NOCHECK CONSTRAINT`, solo si la restricción existe.
  - Procedimientos y triggers: `CREATE OR ALTER`. Requiere **SQL Server 2016 SP1+**, documentado en el encabezado.
- **Sin `SET NOEXEC ON`.** Al empezar se decide si la instalación es nueva (no existe `dbo.Users`) y se guarda en `SESSION_CONTEXT('DeployNuevo')`. Los **datos semilla y los DV se insertan solo en una instalación nueva**: en una base con datos no se pisa, duplica ni recalcula nada. Todo sigue en una única transacción y siempre termina en `DEPLOY_OK`. Ya no emite `DEPLOY_OMITIDO`, pero el instalador lo sigue aceptando.
- **Sección "Actualizaciones de esquema"**, con el patrón `IF COL_LENGTH … ADD` y la advertencia sobre los DV. Incluye la copia de los cierres históricos a `CierresCaja` (A4). El trigger A5 viaja por `CREATE OR ALTER`. No hay columnas nuevas en tablas existentes.
- **`Installer_TP_Fogon.iss`:**
  - `DebeDesplegarBase` dejó de exigir `not IsUpgrade`. Ahora es `SqlCmdEncontrado and (not IsUpgrade or BackupOk)` y usa la instancia del registro (`SqlInstance`).
  - Nueva entrada `[Run]` previa (`DebeRespaldarBase`, `ConsultaBackup`, `VerificarBackup`): `BACKUP DATABASE TP_Ing_Soft` a `{app}\Database\Backups\TP_Ing_Soft_<yyyymmdd_hhnnss>.bak` `WITH INIT, CHECKSUM`.
  - Si el backup o el script fallan, se avisa con la ruta del log, no se borra nada y **sin backup el script no se ejecuta**.
  - `[Dirs]`: `{app}\Database\Backups` con `Permissions: service-modify`.
  - Se actualizaron el encabezado, la página de reinstalación, la nota de la página "Base de datos", `UpdateReadyMemo` (incluye el caso de actualización sin `sqlcmd`) y los comentarios.
- **`Documentacion_Defensa_Fogon.md`:** §6.1, §6.2 y §6.3 actualizados, y en la tabla de §8 cada hallazgo del plan quedó marcado ✅ (resuelto) o 🟡 (parcial), con su pendiente.

**Archivos:** `docs/Deploy_TP_Ing_Soft.sql`, `docs/Pruebas_DeployIdempotente.sql`, `Installer_TP_Fogon.iss`, `Documentacion_Defensa_Fogon.md`.

**Pruebas (ejecutadas en `TP_Ing_Soft_PRUEBA`; detalle en `docs/Pruebas_DeployIdempotente.sql`)**

| Escenario | Resultado |
|---|---|
| Instalación nueva: Deploy nuevo vs el anterior | Las 23 tablas comunes tienen las mismas filas, los mismos checksums y **el mismo hash de DV**. Diferencias esperadas: +`CierresCaja`, +1 PK, +1 índice y el trigger nuevo |
| Base con datos, Deploy ×2 | Sin errores y `DEPLOY_OK` las dos veces. 1.ª corrida: solo `CierresCaja` pasa de 1 a 2 filas (copia del cierre histórico); el resto y los DV quedan idénticos. 2.ª corrida: sin diferencias |
| Base v1.1 (Deploy anterior) con datos, Deploy ×2 | 1.ª corrida: crea `CierresCaja` con el cierre histórico y reemplaza el trigger; las 23 tablas y los DV quedan idénticos, sin FK, CHECK ni DEFAULT duplicados. 2.ª corrida: sin diferencias. Después, `Pruebas_BitacoraCambios.sql` sobre esa base: **FALLAS: 0** |
| Comando de backup del instalador vía `cmd /C sqlcmd -Q` (contra la base de prueba) | Sintaxis y comillas OK. **Hallazgo:** un `BACKUP` fallido no corta el lote, así que se imprimía `BACKUP_OK` aunque el archivo no existiera. Se corrigió con `TRY/CATCH` + `THROW` y se volvió a probar: el error queda sin marca y el éxito con `BACKUP_OK`. También se confirmó que el servicio de SQL **no** puede escribir en una carpeta del usuario y **sí** puede con permiso al SID S-1-5-6 (el mismo que da `service-modify`). El `.bak` de prueba se borró |

---

## Decisiones tomadas por mi cuenta
1. **A4:** el repositorio traduce el error 2627 a `false`, siguiendo el patrón de la casa (`AddSiMesaDisponible`), en lugar de que la BLL atrape `SqlException`. Se conservó la consulta previa `EstaCajaCerrada` para el mensaje. No se agregó una FK `CierresCaja.Usuario → Users`. En la carga histórica, `CantidadCuentas` sale de los pagos de facturas Pagadas de ese día, porque la bitácora no la guarda; si el importe no se puede parsear, `Neta` queda en 0, y si una fecha se repite se toma el primer evento.
2. **A5:** `Nombre` y `Categoria` se comparan con `Latin1_General_BIN2`, para detectar también cambios de mayúsculas o acentos. Un producto sin vigente recibe versión. En el rollback solo-stock queda vigente la fila anterior y no se crea una nueva. El trigger se crea con `QUOTED_IDENTIFIER ON` (el original usaba `OFF`).
3. **A8:** el valor se escribe en ambos modos, porque el `.exe.config` se reemplaza siempre. La "elección actual" en una reinstalación es la que muestra el asistente, que propone la anterior.
4. **M3:** el `ErrorProvider` va solo en campos reales (fechas, nombre). Los errores de selección o composición van en el banner: sobre las grillas con `Dock = Fill`, el ícono quedaba recortado. El toast de éxito de `AccesoAM` se ancla al contenedor principal, porque esa pantalla se libera al volver a la grilla. Se agregó una prevalidación de nombre vacío y de falta de componentes.
5. **Fase 4:** la semilla corre "solo al crear la base", en vez de `WHERE NOT EXISTS` fila por fila: insertar una sola fila semilla en una base con datos rompería sus DV. Ya no se emite `DEPLOY_OMITIDO`. El backup va como entrada `[Run]` previa, porque las entradas corren en orden y el `Check` se evalúa al llegar a cada una. Si no hay backup, no se actualiza la base (opción conservadora).
6. No se subió `MyAppVersion` (sigue en 1.1.0): la numeración de la entrega es decisión tuya.
7. Commits sin línea de co-autoría, por la regla de este repositorio.

## Riesgos detectados
- **Tu base de desarrollo `TP_Ing_Soft` no tiene `CierresCaja`.** Con el código nuevo, Arqueo de caja va a fallar hasta que corras la migración (paso 0).
- **Backup del instalador:** solo funciona con una instancia **local**, porque el `.bak` lo escribe el servidor en su propio disco. Con LocalDB depende de que el proceso tenga permiso de escritura en `Program Files`. Si falla, la base no se actualiza (aviso con log). La desinstalación borra `{app}\*`, **backups incluidos**.
- `service-modify` da escritura en `Database\Backups` a **todas** las cuentas de servicio del equipo, y los `.bak` contienen hashes de contraseñas.
- A1: clientes viejos con DNI fuera de 7 u 8 dígitos no se pueden modificar.
- El Deploy ahora se mantiene **a mano**: regenerarlo con SMO perdería las guardas.
- Las pruebas de UI no se ejecutaron. Abrir la app implicaba conectarse a la base real y necesitaba credenciales, así que la UI quedó verificada solo por compilación y revisión de código (ver checklist).

## Pendientes
- `CierresCaja` fuera de los DV (`TablasMonitoreadas` / `sp_GetAllTablesParaDV`). Lo mismo vale para `Bitacora` (A6).
- A5: `producto_c_actualizado` (MERGE del rollback) sigue restaurando el `StockPorciones` de la versión.
- A9: el fallback `InstaladorBD_08YS` sigue usando `script.sql` e ignorando lotes con error. Ahora podría ejecutar el Deploy idempotente.
- M9: no hay tabla `SchemaVersion`.
- A1: falta la regla de DNI de empleado (no estaba en el plan).
- A8: el usuario semilla `Maximo` sigue con su hash en el script.
- Hallazgo: el Deploy inserta `Productos` **antes** de crear los triggers, así que una instalación nueva arranca con `Productos_C` vacía. Con el trigger nuevo, el primer cambio crea la versión. No se tocó.
- Partes de §1–§5 y §7 de la guía siguen describiendo el comportamiento anterior (por ejemplo §7 P13, P16, P23 y P26). El plan pedía actualizar solo §6 y §8.
- Simulación no listada, que se dejó como estaba: `Factura_790MY`, punto de venta fijo del comprobante simulado (Factura B).

## Checklist manual (al volver)
0. **Base de desarrollo:** hacé un backup de `TP_Ing_Soft` y ejecutá, en este orden, `Migraciones_790MY/AgregarCierresCaja_790MY.sql` y `Migraciones_790MY/VersionarSoloCambiosNegocio_790MY.sql`. Como alternativa, el nuevo `docs/Deploy_TP_Ing_Soft.sql` hace lo mismo y no toca datos ni DV.
1. **Clientes:** dar de alta un cliente con DNI `123456` debe mostrar el error de formato. Con `12345678` debe funcionar.
2. **Reporte de reservas:** con Desde = mañana y Hasta = hoy, el banner debe decir *"La fecha "Desde" (…) no puede ser posterior…"*.
3. **Arqueo de caja:** cerrá la caja de un día con cobros y después intentá cerrarla otra vez: debe aparecer *"La caja del … ya se encuentra cerrada"*. Verificá con `SELECT * FROM CierresCaja`. Los días que ya estaban cerrados por la bitácora deben aparecer como cerrados.
4. **Bitácora de cambios:**
   - Cambiá el precio de un producto: debe aparecer una versión nueva.
   - Despachá un pedido de ese producto en cocina: **no** debe aparecer una versión nueva, y la vigente debe mostrar el stock descontado.
   - Restaurá una versión anterior: debe quedar una sola vigente.
5. **Cobro:** con `ModoDemostracion=true` (`GUI\bin\Debug\GUI_08YS.exe.config`) el "Procesando" dura unos 2,5 s. Con `false` el cobro es inmediato. Volvé a dejarlo en `true`.
6. **Bitácora:**
   - Filtrá con solo "Hasta": debe aparecer el ícono rojo en "Desde" y el banner arriba a la derecha.
   - Exportá con la grilla vacía: debe aparecer un aviso amarillo.
   - Revisá que el banner no tape nada y cambiá de idioma.
7. **Gestión de familias/roles:**
   - Eliminar: debe abrir el `ConfirmModal`.
   - Eliminar una familia en uso: banner amarillo con el nombre.
   - Crear con el nombre vacío: ícono en Nombre + banner.
   - Crear o modificar con éxito: el toast debe verse al volver a la grilla.
   - Agregar un componente que engloba otros: `ConfirmModal` de reemplazo.
   - Revisá ES/EN.
8. **Instalador** (idealmente en una VM): compilá en Release y corré `ISCC.exe Installer_TP_Fogon.iss`.
   - Instalación limpia con la casilla de demo desmarcada: `GUI_08YS.exe.config` debe tener `ModoDemostracion=false`.
   - Reinstalación: debe aparecer un `.bak` en `{app}\Database\Backups`, `Backup_TP_Ing_Soft.log` con `BACKUP_OK` y `Deploy_TP_Ing_Soft.log` con `DEPLOY_OK`. Los datos y los DV no deben cambiar: login sin aviso de inconsistencia.

## Ramas creadas
| Rama | Base | Último commit |
|---|---|---|
| `deuda-tecnica/fase1` | `beta` (2c3ed8b) | 7b98e40 |
| `deuda-tecnica/fase2` | `deuda-tecnica/fase1` | 1cb0a5d |
| `deuda-tecnica/fase3` | `deuda-tecnica/fase2` | 5662de4 |
| `deuda-tecnica/fase4` | `deuda-tecnica/fase3` | este informe |

**Confirmación:** **NO se hizo `git push`** a ningún remoto ni rama. Tampoco se hizo ningún merge hacia `beta` o `main`, ni rebase, `reset --hard`, `--force` o `--no-verify`. No se cambió `git config` ni se borraron ramas o tags. No se hicieron commits en `beta`, `main` ni en ramas existentes. `Documentacion_Defensa_Fogon_borrador_previo.md`, `PROMPT_Deuda_Tecnica.md` y `Claude outputs/assets/` siguen sin commitear.
