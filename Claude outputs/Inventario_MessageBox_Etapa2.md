# Inventario de `MessageBox` — Etapa 2 (para aprobación)

Rama base: `beta` (commit 2ec5134). Fecha: 2026-10-18. No se migró nada todavía (E2-D2).

## Resumen

- **139 llamadas directas** a `MessageBox.Show` en 28 archivos de la GUI (el análisis previo contaba 145; Clientes bajó de 21 a 15 con la Etapa 1).
- **41 llamadas indirectas** a través de `EstiloRF2_790MY.MostrarError` (33) y `MostrarExito` (8), en 10 formularios. Ese ayudante contiene 4 `MessageBox`: si se migra el ayudante, esas 41 llamadas cambian de una sola vez.
- `TemaFogon_790MY` hoy lo usan solo Clientes y Mesas. No existe ningún diálogo de confirmación, banner ni toast reutilizable: los modales más cercanos (`FormListaSimple_790MY`, `FormRegistrarCliente_790MY`) son `FixedDialog` + `CenterParent` sin el tema. Habrá que crear los tres componentes (ver propuesta al final).

Clases: **V** validación · **C** confirmación · **X** éxito · **R** error recuperable (regla de negocio, dato duplicado, permiso, etc.) · **F** error fatal (excepción inesperada, base de datos).

| Clase | Directas | Destino según E2-D1 / E2-D4 |
|---|---:|---|
| V validación | 33 | `ErrorProvider` al salir del campo + resumen en banner al guardar |
| C confirmación | 19 | Bajas/eliminaciones/irreversibles: diálogo modal con `TemaFogon` |
| X éxito | 23 | `ToastHost`, se cierra solo a los 4 s |
| R error recuperable | 35 | Banner del formulario, permanece hasta corregir |
| F error fatal | 29 | Se mantiene `MessageBox` nativo (texto reescrito) |
| **Total** | **139** | |

## Detalle por formulario

Orden propuesto según E2-D2 (maestros → … → RF1 y RF2) y, dentro de cada grupo, por cantidad de llamadas (E2-H2).

### Grupo 1 — Maestros

| Formulario | Total | V | C | X | R | F | Líneas |
|---|---:|---:|---:|---:|---:|---:|---|
| `FormClientes_790MY` | 15 | 3 | 1 | 4 | 3 | 4 | V 848 sin selección p/ exportar, 856 y 884 falta ruta · C 412 baja · X 422 baja, 701 alta, 714 modificación, 866 serialización · R 428 no se puede eliminar (msj BLL), 725 DNI duplicado, 730 validación BLL · F 433, 735, 872, 910 |
| `FormMesas_790MY` | 9 | 1 | 1 | 3 | 2 | 2 | V 318 nro inválido · C 286 baja · X 295, 331, 338 · R 302 no se puede eliminar, 351 datos inválidos (msj BLL) · F 307, 356 |
| `FormEmpleados_790MY` | 2 (+4) | 1 | 1 | (+1) | (+3 mixtos) | | V 330 DNI · C 364 baja · ayudante RF2: 1 éxito, 3 errores |
| `FormListaSimple_790MY` | 2 | 2 | | | | | V 47 duplicado, 62 sin selección |
| `FormProductos_790MY` | 1 (+4) | | 1 | (+1) | (+3 mixtos) | | C 358 baja · ayudante RF2: 1 éxito, 3 errores |

### Grupo 2 — RF1 (Reservas)

| Formulario | Total | V | C | X | R | F | Líneas |
|---|---:|---:|---:|---:|---:|---:|---|
| `FormRegistrarReserva_790MY` | 12 | 5 | 1 | | 4 | 2 | V 252 DNI, 310 y 379 falta turno, 361 falta cliente, 370 falta mesa · C 280 «cliente no encontrado, ¿registrarlo?» · R 271 cliente inactivo, 330 sin disponibilidad, 339 y 397 msj BLL · F 295, 404 |
| `FormRegistrarCliente_790MY` | 4 | 1* | | | 2 | 1 | V 172 (`Avisar`, usado por 5 validaciones) · R 155 DNI duplicado, 160 validación BLL · F 165 |
| `FormSeleccionarMesa_790MY` | 2 | 1 | | | 1 | | V 239 falta seleccionar mesa · R 133 regla BLL. **Sin tocar maquetado**: el banner se superpone o reutiliza un espacio existente (ver pregunta 6) |
| `FormComprobanteReserva_790MY` | 2 | | | 1 | 1 | | X 223 «guardado, ¿abrir?» (Sí/No) · R 214 no se pudo guardar el archivo |
| `FormPruebaMapa_08YS` | 1 | | | 1 | | | Prototipo sin referencias en el proyecto (ver pregunta 5) |

### Grupo 3 — RF2 (Salón y cocina), sin `FormFacturacionCobro`

| Formulario | Total | V | C | X | R | F | Líneas |
|---|---:|---:|---:|---:|---:|---:|---|
| `EstiloRF2_790MY` (ayudante) | 4 | | | 1 | 2 | 1 | `MostrarExito` → toast · `MostrarError`: `ArgumentException` y `UnauthorizedAccessException` → banner, resto → nativo |
| `FormAbrirMesa_790MY` | 4 (+5) | 1 | 1 | (+1) | 2 (+4) | | V 388 DNI · C 406 «¿registrar cliente?» · R 400 inactivo, 448 sin disponibilidad |
| `FormRegistrarPedido_790MY` | 1 (+6) | 1 | | (+1) | (+5) | | V 447 falta mesa |
| `FormCocinaKDS_790MY` | 0 (+5) | | | (+1) | (+4) | | Solo vía ayudante |
| `FormComprobanteFactura_790MY` | 0 (+1) | | | | (+1) | | Solo vía ayudante |
| ~~`FormFacturacionCobro_790MY`~~ | 2 (+6) | | 1 | | 1 | | **Fuera de alcance (Etapa 4).** Ojo: si se migra el ayudante, sus 6 llamadas indirectas cambian igual (ver pregunta 7) |

### Grupo 4 — Reportes

| Formulario | Total | V | C | X | R | F | Líneas |
|---|---:|---:|---:|---:|---:|---:|---|
| `FormReporteReservas_790MY` | 5 | 2 | | 1 | 1 | 1 | V 136 DNI, 194 sin datos para PDF · X 211 PDF guardado · R 218 error al guardar PDF · F 162 |
| `FormArqueoCaja_790MY` | 1 (+3) | | 1 | (+1) | (+2) | | C 169 cierre de caja (irreversible → modal) |
| `FormReportePredictivo_790MY` | 1 (+3) | | 1 | (+1) | (+2) | | C 383 confirmar acción del informe |

### Grupo 5 — Administración y sesión (formularios `_08YS`, ver pregunta 4)

| Formulario | Total | V | C | X | R | F |
|---|---:|---:|---:|---:|---:|---:|
| `FormAccesoAM_08YS` | 13 | 2 | 1 | 4 | 3 | 3 |
| `FormGestionUsuarios_08YS` | 13 | 2 | 2 | 3 | 3 | 3 |
| `FormLogin_08YS` | 11 | 1 | | | 7† | 3 |
| `FormBitacora_08YS` | 8 | 6 | | | | 2 |
| `FormInconsistenciaDB_08YS` | 7 | | 2 | 2 | | 3 |
| `FormCambiarContraseña_08YS` | 6 | 3 | | 1 | 1 | 1 |
| `FormGestionAcceso_08YS` | 4 | | 1 | | 1 | 2 |
| `FormGestionRespaldos_08YS` | 3 | | 1 | 2 | | |
| `FormMDI_08YS` | 3 | | 2 | | 1† | |
| `FormBitacoraCambios_790MY` | 2 (+4) | 1 | 1 | (+1) | (+3) | |
| `Program.cs` | 1 | | | | | 1 |

† Avisos informativos que interrumpen el flujo (sesión invalidada, cambio de contraseña obligatorio, sesión reiniciada). Propongo mantenerlos modales con el diálogo propio, no como banner. `Program.cs` (base de datos no preparada al arrancar) queda con `MessageBox` nativo: es exactamente el caso de error fatal de E2-D1.

## Verificaciones pedidas en el documento de decisiones

1. **Celular (contradicción 1).** `FormClientes_790MY` ya lo exige (coherente con la respuesta más reciente y con `ClienteBLL_790MY.ValidarDatos`). **Incoherencia:** `FormRegistrarCliente_790MY` (registro rápido, línea 126) lo trata como opcional; si se deja vacío, la BLL lo rechaza y el usuario ve el mensaje genérico de «validación BLL». Propuesta: exigirlo también en la GUI del registro rápido.
2. **Dirección (contradicción 2).** La BLL **no** la exige (`ValidarDatos` solo controla DNI, nombre, apellido, teléfono y email). La exige la GUI en dos lugares: `FormClientes_790MY:607` y `FormRegistrarCliente_790MY:133`. Según la regla del documento, no la cambio sin tu confirmación (pregunta 1).
3. **Estados de mesa (contradicción 3).** El maestro solo ofrece «Disponible» y «Fuera de servicio»; Ocupada y Reservada no se ofrecen (se conservan al guardar). **Pero** `FormMesas_790MY:202` habilita `rdoFueraServicio_790MY` en modo Modificación, es decir, el maestro **sí** permite pasar una mesa a «Fuera de servicio», lo que contradice P14 (pregunta 2).
4. **DNI de 7 u 8 dígitos.** Ni la GUI ni la BLL lo controlan hoy: solo verifican que sea un entero positivo. La GUI podría agregarlo (E2-H3), pero quedaría más estricta que la BLL (pregunta 3).
5. **`FormSeleccionarMesa_790MY`.** 2 llamadas: 1 validación (falta elegir mesa) y 1 error recuperable de la BLL. Ninguna confirmación.
6. **`FormEmpleados` y `FormProductos`.** Ya usan `EstiloRF2_790MY` (`VentanaPropietaria`, `E.T`, `MostrarError/MostrarExito`), no `TemaFogon`. Tienen pocas llamadas directas; la mayor parte de su migración es el ayudante.
7. **Diálogo modal base.** No hay ninguno reutilizable con el tema. Además, Clientes ya valida en línea con etiquetas `lblErrorX` debajo de cada campo (Etapa 1), no con `ErrorProvider` (pregunta 8).

## Componentes nuevos propuestos (solo GUI, carpeta `GUI/Estilo`)

- `DialogoConfirmacion_790MY`: modal con `TemaFogon`, título, mensaje, botón primario bordó (acción) y secundario (cancelar). Cancelar con el foco por defecto en bajas. Devuelve `DialogResult` para reemplazar `MessageBox.Show(...) == DialogResult.Yes` sin cambiar la lógica.
- `BannerAviso_790MY`: `UserControl` acoplado arriba del formulario, variantes error / aviso / info (colores `ErrorFondo`, `AvisoSuave`…), con lista de errores y que se oculta cuando se corrigen.
- `ToastHost_790MY`: aviso no bloqueante en la esquina del formulario, `Timer` de 4 s, sin sombra (P09).
- Migrar `EstiloRF2_790MY.MostrarError/MostrarExito` para que usen banner y toast (cubre 41 llamadas).

## Preguntas para aprobar antes de migrar

1. **Dirección:** ¿la hago opcional en `FormClientes_790MY` y `FormRegistrarCliente_790MY` (alineado con la BLL y tu respuesta), o la mantengo obligatoria en la GUI?
2. **Fuera de servicio en el maestro de mesas:** ¿se quita la opción (como dice P14) o se deja como está?
3. **DNI 7–8 dígitos:** ¿se agrega esa validación en la GUI de Clientes, registro rápido, Reserva y Abrir Mesa, sabiendo que la BLL no lo controla?
4. **Formularios `_08YS`** (Administración, Login, MDI, Bitácora, Respaldos, Contraseña: 69 llamadas): ¿entran en la Etapa 2 o quedan fuera por ser de otro módulo?
5. **`FormPruebaMapa_08YS`:** no tiene referencias. ¿Se elimina como código muerto o se deja?
6. **`FormSeleccionarMesa_790MY`:** sin tocar el maquetado no hay lugar fijo para un banner. ¿Vale que el banner aparezca superpuesto arriba del plano (sin mover controles), o preferís otra solución?
7. **Ayudante `EstiloRF2_790MY`:** si se migra, `FormFacturacionCobro_790MY` también cambia (6 llamadas indirectas). ¿Se acepta, o se deja un camino aparte para que Facturación siga con `MessageBox` hasta la Etapa 4?
8. **Clientes:** ¿se conservan las etiquetas de error en línea de la Etapa 1, o se reemplazan por `ErrorProvider` para unificar con el resto?
9. **Confirmaciones que no son bajas** (`¿registrar cliente nuevo?` en Reserva y Abrir Mesa, `¿abrir el PDF?`, cerrar sesión, reemplazo sugerido en permisos): ¿usan el mismo diálogo propio o se quedan con `MessageBox` nativo? E2-D1 solo habla de bajas y eliminaciones.
