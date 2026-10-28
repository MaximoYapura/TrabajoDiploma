# Informe — Refactorización de limpieza (2026-10-27)

Ejecución de `PROMPT_Refactorizacion_Limpieza.md`: Paso 0 y fases 1, 2, 3, 4 y 6 aplicadas en ramas; fase 5 entregada **como listas para decidir** (el prompt pide mostrarlas antes de borrar).

## Paso 0 — resguardo

- Commit en `beta` (`0562950`): `.resx` de `FormAbrirMesa_790MY` y `FormRegistrarPedido_790MY`, sus `.Designer.cs`, la entrada en `GUI_08YS.csproj` y el prompt. Quedaron fuera `PROMPT_Deuda_Tecnica.md`, `Documentacion_Defensa_Fogon_borrador_previo.md`, `Claude outputs/assets/` y los binarios del instalador.
- Push de `beta` y tag `backup-pre-refactor-2026-10-27` (también pusheado).
- Instalador recompilado desde ese commit (Release, 0 warnings): `Installer\Output\Setup_ElFogonDelSur.exe`. **Es el baseline previo a la refactorización**; ninguna rama de refactor está en el instalador.

## Ramas

**Están apiladas** (cada una sale de la anterior, no de `beta`): las fases tocan los mismos archivos y sin merge a `beta` entre fases no se podían crear todas desde `beta`. Mergear la última (`refactor/fase6-documentacion`) en `beta` trae todo; para revisar de a una, mirá cada rama contra la anterior.

| Orden | Rama | Commits nuevos | Contenido |
|---|---|---|---|
| 1 | `refactor/fase1-codigo-muerto` | 6 | Fase 1 |
| 2 | `refactor/fase2-mover-tipos` | 4 | Fase 2 |
| 3 | `refactor/fase3-tipos-compartidos` | 1 | Fase 3.1 |
| 4 | `refactor/fase3-registrar-pedido` | 1 | 3.2 `ItemComanda_790MY` |
| 5 | `refactor/fase3-comprobante-factura` | 1 | 3.2 `TicketFactura_790MY` |
| 6 | `refactor/fase3-pie-comprobante` | 1 | 3.2 `PieComprobante_790MY` |
| 7 | `refactor/fase4-pago` | 1 | 4.1 |
| 8 | `refactor/fase4-empleado` | 1 | 4.2 |
| 9 | `refactor/fase4-reserva` | 1 | 4.3 |
| 10 | `refactor/fase4-pedido` | 1 | 4.4 |
| 11 | `refactor/fase4-factura` | 1 | 4.5 |
| 12 | `refactor/fase4-productoc` | 1 | 4.6 (opcional, hecha) |
| 13 | `refactor/fase6-documentacion` | 2 | Fase 6 + este informe |

Todas pusheadas a `origin`. Sin merge a `beta` ni a `main`, sin push a `main`. Commits sin línea de co-autoría. Total contra `beta`: 69 archivos, +913 / −1159.

## Verificación hecha (automática)

1. **Compilación** de la solución en Debug (Rebuild) después de cada subpaso: **0 warnings, 0 errores** en todos. Un error intencional no detectado habría aparecido: en la 4.6 un reemplazo fallido rompió la build y MSBuild lo marcó (corregido antes del commit).
2. **csproj ↔ disco** al cierre de cada fase: ningún `.cs` fuera del csproj ni entradas sin archivo, en los 6 proyectos.
3. **Arranque de la app** (`GUI_08YS.exe` Debug) al cierre de cada fase: abre "El Fogón del Sur" y responde.
4. **Fase 3.1:** por reflexión, el ensamblado contiene `ModoEdicion_790MY` y `OpcionCombo_790MY<T>` y ya no los tipos anidados.
5. **Fase 4 — fábrica:** se construyó cada BLL con `BLLFactory_790MY` por reflexión: sin ciclos ni recursión, y el grafo coincide con la estructura objetivo.
6. **Fase 4 — permisos por rol, contra la base local:** harness con sesiones en memoria (usuario + rol con **un solo** permiso, sin contraseñas) que llama métodos de lectura o casos que fallan en una validación **antes** de escribir (reserva -1, cliente -1, capacidad insuficiente, factura -1, versión -1). Escenarios: Camarero (`RegistrarPedido`), usuario sin empleado, Chef (`DespacharPedido`), Recepción (`AbrirMesa`; `RegistrarReserva`), Caja (`GenerarFactura` con camarero; `RegistrarCobro` sin empleado), solo `GestionCaja`, solo `CrearEmpleado`, solo `AuditarProductos` y sin sesión. Se corrió igual sobre el **tag de resguardo** y sobre la rama final: **las 46 líneas de salida son idénticas** (mismos datos, mismos permisos concedidos y denegados, mismos mensajes). No se escribió nada en la base.

No probé la GUI con clics (login, formularios, escalas 125/150 %, idioma en). Eso queda en las checklists de abajo.

## Fase 1 — código muerto

**Cambios**
- Borrados: `BLL/FormatoSerializacion_790MY.cs`, `GUI/Maestros/FormRegistrarClienteModal_790MY.cs`, `MPP/ClienteMapper_790MY.cs` (+ `MPP_08YS.csproj`), `GUI/UserControls/UserControlMesa_790MY.cs` (+ `GUI_08YS.csproj`), `script.sql` de la raíz. Referencias buscadas antes en `.cs`, `.resx`, `.csproj`, `.json`, `.iss` y `.vdproj`.
- `ReservaBLL_790MY`: sin `CancelarReserva` ni `FinalizarReserva`; sin `IPedidoRepository_790MY` (campo, constructor y `BLLFactory_790MY.CreateReservaBLL`). También fuera de `IReservaRepository_790MY` y `SqlReservaRepository_790MY`. `Evento.ReservaCancelada` se conserva.
- Comentario corregido: `FormRegistrarClienteModal_790MY` → `FormRegistrarCliente_790MY`.
- Sin llamadores, borrados: `PedidoBLL.ObtenerPedidosListosDelCamarero`, `PagoBLL.ObtenerPagoDeFactura`, `IMesaRepository.GetDisponibles` (+ `SqlMesaRepository`), `Empleado_790MY.TieneUsuario`, `Reserva_790MY.OcupaMesa` (`EstadosQueOcupanMesa` se queda: lo usa `FiltroOcupacionMesa_790MY`), `TemaFogon_790MY.Fuente*` (las 5; `Familia` se queda), `EstiloRF2_790MY.MostrarError/MostrarExito` (se ajustó el `<summary>` de `MostrarErrorEnBanner`, que las citaba).

**No hecho / observaciones**
- 1.5 respetado: nada `_08YS` tocado. `DAL/TP_INGENIERIA_SOFTWAREDataSet.xss`: no está en el csproj y nada lo referencia. No lo borré.
- `FacturaBLL.ObtenerFactura` se conserva (se usa en la 4.1).

**Checklist manual**
- [ ] Registrar reserva (RF1) y ver el comprobante.
- [ ] Abrir mesa con reserva y con Walk-in.
- [ ] Registrar pedido; KDS: iniciar y marcar listo.
- [ ] Caja: generar cuenta y cobrar (efectivo y tarjeta); arqueo.
- [ ] Maestros de Empleados y Mesas: alta, modificación y baja.

## Fase 2 — un tipo por archivo

**Cambios** (mismo namespace, código sin cambios; archivos nuevos UTF-8 con BOM + CRLF)
- `BLL/CobroDTOs_790MY.cs` → `MesaCobroDTO_790MY.cs`, `ImportesCuenta_790MY.cs`, `DetalleCuentaDTO_790MY.cs`, `ResultadoCobroDTO_790MY.cs`, `ReporteArqueoCaja_790MY.cs` (se borra el original).
- `ReportePredictivoBLL_790MY.cs` → `OrdenReglas_790MY`, `ReglaAsociacion_790MY`, `TipoSugerencia_790MY`, `SugerenciaInteligente_790MY`, `ResumenCanasta_790MY`.
- `ProductoCBLL_790MY.cs` → `DiferenciaVersion_790MY`.
- BE: `ItemFactura_790MY`, `CanastaFactura_790MY` (enums siguen con `Factura_790MY`); `SolicitudCobro_790MY`, `CobroArqueo_790MY`; `ItemPedido_790MY`.
- DAL: `EstadoRegistroPedido_790MY`, `ResultadoRegistroPedido_790MY`, `ResultadoDespacho_790MY` (en `IPedidoRepository_790MY.cs` queda la interfaz).

**No hecho:** lo excluido por el prompt (excepciones `_08YS`, `BitacoraEvento_08YS.cs`, pares enum + clase, interfaces Pago/Factura con su enum).

**Checklist manual:** Caja y cobro completo + Arqueo; Reporte predictivo; Bitácora de cambios de productos (comparar y restaurar una versión).

## Fase 3 — clases anidadas

**3.1** `GUI/Maestros/ModoEdicion_790MY.cs` (`internal enum`) reemplaza los 4 `private enum ModoEdicion`. `GUI/Estilo/OpcionCombo_790MY.cs` (`internal sealed class OpcionCombo_790MY<T>`) reemplaza `Opcion<T>`, `EstadoItem` y `TurnoItem` (`Display` → `Texto`, `Hora` → `Valor`).
- **Desvío del prompt (necesario):** `FormRegistrarReserva_790MY.Designer.cs` tenía `cmbHora_790MY.DisplayMember = "Display"`. Cambié esa única línea a `"Texto"`. Sin el cambio el combo igual mostraría el texto (cae a `ToString()`), pero el `DisplayMember` quedaba apuntando a una propiedad inexistente.

**3.2**
- `ItemComanda` → `GUI/RF2/ItemComanda_790MY.cs` (`internal`).
- `Ticket` → `GUI/RF2/TicketFactura_790MY.cs` (`internal`). Usaba los colores `Tinta` / `TintaSuave` del formulario: pasaron de `private` a `internal` y el ticket los referencia calificados. El dibujo no cambia (diff verificado).
- `PieComprobante` → `BLL/PieComprobante_790MY.cs` (`internal`). Usaba `PdfTileBorde` de `ReservaBLL`: pasó de `private` a `internal`.

**3.3** Sin tocar lo que se queda anidado.

**Checklist manual** (a 100/125/150 % y en es/en)
- [ ] Clientes, Empleados, Mesas, Productos: Reposo → Alta → Guardar; Modificación; Cancelar con cambios sin guardar.
- [ ] Empleados: filtro y combo de cargo. Productos: filtro y combo de categoría.
- [ ] Registrar reserva: el combo de turno muestra "18:00 a 20:00" (o el formato actual) y el resumen lo refleja.
- [ ] Reporte de reservas: combo de estado (Todos/Confirmada/…), buscar y PDF.
- [ ] Registrar pedido: armar comanda en dos mesas y cambiar de mesa sin perderla.
- [ ] Comprobante de factura: pre-ticket y factura pagada (vista previa e impresión).
- [ ] Comprobante PDF de reserva: el pie (línea punteada, emitido, agradecimiento) sigue igual.

## Fase 4 — BLL → BLL

**Estructura final** (verificada por reflexión):
```
Nivel 0: ClienteBLL, ProductoBLL, MesaBLL*, EmpleadoBLL*
ReservaBLL   -> Cliente, Mesa, Empleado
PedidoBLL    -> Reserva, Producto, Empleado
FacturaBLL   -> Reserva, Pedido, Empleado
PagoBLL      -> Factura
ProductoCBLL -> Producto
```
**`internal` agregados** (sin `ValidatePermission`, mismo método de repositorio que antes):
- `EmpleadoBLL`: `ObtenerEmpleadoDeSesion`, `ObtenerCamarerosActivos`, `ObtenerPorDni`. `GetCamarerosActivos` sigue validando `AbrirMesa` y delega.
- `ReservaBLL`: `ObtenerPorId`, `ObtenerAsistidasPorCamarero`, `ObtenerAsistidas`.
- `ProductoBLL`: `ObtenerActivos`, `ObtenerPorId`, `ExisteNombre`.
- `PedidoBLL`: `ContarNoDespachados`.
- `ClienteBLL`: `ExisteActivo`. `MesaBLL`: `ObtenerPorNumero`.

**Reglas respetadas:** cada método público de origen sigue validando su patente al principio; ningún público cambió su permiso; ninguna consulta SQL cambió; la GUI no se tocó; los constructores cambian solo vía `BLLFactory_790MY`. `ObtenerCamareroDeSesion` de Pedido (lanza `msg_rf2_usuario_no_camarero`) y de Factura (devuelve `null`) conservan su regla sobre `EmpleadoBLL.ObtenerEmpleadoDeSesion`. `CamarerosPorDni` de ambas usa `ObtenerCamarerosActivos`. `MesaBLL.EliminarMesa` y `EmpleadoBLL.ContarMesasAbiertas/ValidarSinMesasAbiertas` conservan `IReservaRepository` con comentario `// Excepción: ...`.

**Nota 4.1:** `FacturaBLL.ObtenerFactura` valida "acceso a Caja" (`RegistrarCobro` o `GenerarFactura`). `PagoBLL.RegistrarCobro` ya validó `RegistrarCobro`, así que pasa siempre. Lo confirmó el harness con un rol de solo `RegistrarCobro`.

**Checklist manual por rol**
- [ ] **Camarero:** mesas asignadas, catálogo, registrar pedido, avisos de pedido listo, generar factura (solo sus mesas), cobrar.
- [ ] **Chef:** KDS con nombres de camareros, iniciar preparación, marcar listo.
- [ ] **Recepcionista:** abrir mesa con reserva y Walk-in (combo de camareros y contador de mesas), registrar reserva, reporte de reservas.
- [ ] **SuperAdmin:** todo lo anterior + ABM de empleados (baja de un camarero con mesa abierta → debe rechazar) y de mesas (baja de mesa con historial → debe rechazar) + restaurar versión de producto.
- [ ] **Rol personalizado con un solo permiso** por caso de uso (`RegistrarPedido`, `DespacharPedido`, `AbrirMesa`, `RegistrarReserva`, `GenerarFactura`, `RegistrarCobro`, `AuditarProductos`): cada uno debe poder hacer su caso de uso sin pedir otros permisos.

## Fase 5 — listas para decidir (no apliqué nada)

### Claves de `GUI/idiomas.json`
Búsqueda por texto (palabra completa) en todos los `.cs`/`.resx` y además en `.sql`, otros `.json`, `.config`, `.iss` y `.vdproj`. Las claves armadas en código (`"PREFIJO_" + x`, `$"PREFIJO_{x}"`) se separan aparte. `es` = 1313 claves y `en` = 1314: `en` tiene una clave `"-"` que no existe en `es`.

**A. Sin referencias, candidatas a borrar (72):**
`GR_titulo`, `GR_lblDesde`, `GR_lblHasta`, `GR_lblDni`, `GR_lblEstado`, `GR_colNro`, `GR_colDni`, `GR_colCliente`, `GR_colMesa`, `GR_colFecha`, `GR_colHora`, `GR_colComensales`, `GR_colEstado`, `GR_cmbEstadoTodos`, `GR_cmbConfirmada`, `GR_cmbCancelada`, `msg_gr_dni_invalido`, `msg_gr_error_buscar`, `msg_gr_cancelada_ok`, `FM_colNumero`, `FM_colCapacidad`, `FM_colEstado`, `msg_fm_capacidad_invalida_num`, `msg_fm_mesa_existente`, `SM_titulo`, `SM_comensales`, `SM_lblSinResultados`, `SM_lblPlano`, `LS_btnQuitar`, `UC_mesa_lblMesa`, `UC_mesa_lblPersonas` (eran del `UserControlMesa` borrado), `RR_lblClienteNoEncontrado`, `msg_rr_cliente_no_encontrado`, `msg_reserva_registrada`, `msg_reserva_cancelada`, `msg_seleccionar_reserva`, `msg_reserva_no_cancelable`, `msg_cliente_no_encontrado`, `msg_cliente_registrado`, `msg_cliente_modificado`, `msg_cliente_eliminado`, `msg_confirmar_eliminar_cliente`, `msg_seleccionar_cliente`, `msg_campos_obligatorios`, `msg_mesa_registrada`, `msg_mesa_modificada`, `msg_mesa_eliminada`, `msg_confirmar_eliminar_mesa`, `msg_seleccionar_mesa`, `msg_nro_mesa_duplicado`, `msg_nro_mesa_invalido`, `msg_capacidad_invalida`, `capacidad_invalida_titulo`, `msg_serializar_ok`, `msg_deserializar_ok`, `msg_seleccionar_archivo_serializar`, `msg_seleccionar_archivo_deserializar`, `titulo_datos_invalidos` (la usaba el `MostrarError` borrado), `titulo_confirmar_baja`, `titulo_confirmar_cancelacion`, `titulo_falta_seleccionar`, `titulo_falta_ruta`, `titulo_fc_sin_seleccion`, `REP_pdf_generado`, `KDS_minutos`, `msg_fc_confirmar_cobro`, `CJ_confirmar_titulo`, `CJ_confirmar_tarjeta`, `CJ_pago_rechazado_titulo`, `msg_rf2_reserva_con_pedidos_pendientes`, `msg_rf2_reserva_no_cancelable`, `msg_rf2_reserva_no_finalizable` (estas tres, de lo borrado en 1.2).

**B. Sin referencias, pero parecen de pantallas `_08YS` (roles, familias, contraseña). Por la regla no las propongo (14):**
`lblComposicion`, `error_cargar`, `no_se_puede_eliminar`, `no_se_puede_agregar`, `sin_seleccion`, `col_nombre`, `col_tipo`, `msg_pwd_cambiada`, `msg_error_cargar_datos`, `msg_eliminado_exito`, `msg_guardado_exito`, `msg_nombre_vacio`, `msg_sin_componentes`, `msg_componente_en_uso_error`.

**C. Dinámicas, NO borrar (35):** `estado_reserva_*`, `estado_pedido_*`, `FE_puesto_*`, `RI_num_0..10`, `MT_consejo_1..5_{tipo,titulo,mensaje}`.

**D. Excluidas por regla (21):** `rol_*`, `msg_rol_*`, `msg_familia_*`, `msg_user_*`, `titulo_alta_*`, `titulo_mod_*`, `lbl_Password`, `msg_error_login`.

`acceso_denegado`, `exito` y `error` siguen en uso en otros lugares: no entran.

### `using` probablemente innecesarios (archivos no `_08YS`; heurística por identificadores, aplicalo con Visual Studio)
- `BE/Cliente_790MY.cs`: System.Linq, System.Text, System.Threading.Tasks
- `BLL/ClienteBLL_790MY.cs`: System.Linq, System.Threading.Tasks
- `BLL/Exceptions/ClienteDniDuplicadoException_790MY.cs`: System.Linq, System.Text, System.Threading.Tasks, System.Collections.Generic
- `DAL/Interfaces_Repositories/IClienteRepository_790MY.cs`: System.Linq, System.Text, System.Threading.Tasks
- `DAL/SQL/SqlClienteRepository_790MY.cs`: System.Linq, System.Text, System.Threading.Tasks
- `DAL/SQL/SqlFacturaRepository_790MY.cs`: System.Linq
- `DAL/SQL/SqlMesaRepository_790MY.cs`: System.Data
- `GUI/Config/Program.cs`: System.Linq, System.Threading.Tasks, System.Collections.Generic
- `GUI/UserControls/IconCache.cs`, `MenuColorTable.cs`, `MenuRenderer.cs`: System.Linq, System.Text, System.Threading.Tasks (+ System.Collections.Generic en los dos últimos)
- `Service_08YS/Entities/Comparers/PermisoComparer.cs`: System.Linq, System.Text, System.Threading.Tasks. Está en el proyecto `Service_08YS`; si lo considerás `_08YS`, salteálo.

## Fase 6 — documentación

`Documentacion_Defensa_Fogon.md`: nueva §0.4.1 "Relaciones entre BLL" (grafo, `internal` sin `ValidatePermission`, excepción Mesa/Empleado), flujos por capas 1.1–1.5 con las llamadas BLL→BLL, `CancelarReserva`/`FinalizarReserva` marcados como eliminados (el cierre ocurre en la transacción del cobro), nombres y archivos nuevos (`ModoEdicion_790MY`, `OpcionCombo_790MY`, `TicketFactura_790MY`, `PieComprobante_790MY`, DTOs/BE por archivo, MPP sin `ClienteMapper`), fila A2 de mejoras y respuesta 1 del tribunal. `docs/Informe_Deuda_Tecnica.md` no se tocó.

## Pendiente de tu lado

1. Probar las checklists (sobre todo la Fase 4 por rol y la Fase 3 a distintas escalas).
2. Decidir el borrado de la lista A de claves (y si querés, la B).
3. Limpiar los `using` con Visual Studio.
4. Mergear en `beta` (alcanza con `refactor/fase6-documentacion`, que contiene todo) y recompilar el instalador desde ahí.
