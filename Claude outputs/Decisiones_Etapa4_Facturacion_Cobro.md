# Etapa 4 — Facturación y cobro: decisiones, plan de ejecución y verificaciones

Fuente: documento de análisis de la Etapa 4 (riesgos, decisiones E4-D1 a E4-D8 e hipótesis E4-H1 a E4-H7) y las instrucciones del propietario del 2026-10-19.
Formularios: `GUI/RF2/FormFacturacionCobro_790MY` y `GUI/RF2/FormComprobanteFactura_790MY`.
Rama: `etapa4/facturacion-y-cobro`, creada desde `beta`.
Base verificada: commit `30038dd` (merge de `etapa3/kds-opcion-c` en `beta`). Todo lo de abajo se leyó en el código; no se ejecutó la aplicación.

Si algo de este documento contradice el código real, se detiene el trabajo y se consulta antes de decidir. Los resultados de las mediciones y de las pruebas se agregan al final de este archivo (secciones «Mediciones» y «Resultado»), como en `Decisiones_Etapa3_KDS.md`.

## 1. Qué se pide (instrucciones del propietario, vigentes)

1. Crear la rama `etapa4/facturacion-y-cobro` desde `beta`. Antes: verificar que la rama actual sea `beta` y que no haya cambios sin commitear. Los archivos sin seguimiento de `Claude outputs/` (PDF y otros) NO se agregan al commit.
2. Medición inicial con `Stopwatch` (E4-H1), registrada con `Trace.WriteLine` en la categoría `Cobro`, de:
   - `GenerarFactura`
   - la transacción de cobro (`RegistrarCobro`)
   - `DVManager.Recalcular`
   - el registro en Bitácora
3. Componentes nuevos:
   - `SlidingTab_UC`: selector deslizante de medio de pago (Efectivo / Tarjeta), con escalado DPI estándar y foco por teclado.
   - Botón de cobro con spinner de carga: indicador circular giratorio (`Timer` liviano o GDI+). Al iniciar el cobro se deshabilita la interacción y se muestra «Procesando»; al terminar o fallar se restaura el estado original.
4. Migración UX/UI:
   - Eliminar el camino legacy de `MessageBox` en `FormFacturacionCobro_790MY` y `FormComprobanteFactura_790MY`.
   - Usar `Banner_UC_790MY`, `ToastHost_790MY`, `ErrorProvider` y `ConfirmModal_790MY`.
   - Confirmación del cobro con `ConfirmModal_790MY`: «¿Confirmar el cobro de $X en [Medio de Pago]?».
   - Rechazo simulado (tarjetas terminadas en `0002`): error en `Banner_UC_790MY` y foco en el campo de tarjeta.
5. Asincronismo:
   - Envolver `GenerarFactura` + `RegistrarCobro` en un único `await Task.Run(...)`.
   - Mientras dure: spinner activo, inputs deshabilitados, interacción con el contenedor principal bloqueada, y `FormClosing` interceptado para no cerrar a mitad del cobro.
   - Las excepciones vuelven al hilo de la interfaz y se muestran en el banner.
6. i18n y compilación:
   - Textos nuevos en `idiomas.json` (es/en).
   - 0 errores de compilación en el proyecto GUI.
   - **En los mensajes de commit NO se agrega `Co-authored-by` (ni ninguna otra línea de coautoría).**

## 2. Decisiones vigentes

| ID | Decisión | Estado |
|---|---|---|
| E4-D1 | Solo capa GUI (4a). Sin cambios de lógica en BLL ni DAL. Única excepción: la instrumentación de medición del paso 2 (ver §3.1). | Aprobado |
| E4-D2 | Confirmación del cobro con `ConfirmModal_790MY` (modal temático), no con dos clics sobre el botón. El botón queda con los estados Disponible, Procesando y, tras el resultado, vuelve al estado original. | Aprobado |
| E4-D3 | `DVManager.Recalcular` no se modifica: solo se mide. | Aprobado (implícito) |
| E4-D4 | No hay estado «pago pendiente» ni «sin respuesta del banco»: el banco es simulado e instantáneo. | Aprobado (por omisión en el pedido) |
| E4-D5 | Rechazo de tarjeta: banner con el mensaje existente, foco y selección en `txtNroTarjeta_790MY`; se puede cambiar a efectivo con el selector. | Aprobado |
| E4-D6 | Durante el cobro: formulario deshabilitado, cierre bloqueado, navegación del contenedor principal bloqueada. | Aprobado |
| E4-D7 | `SlidingTab_UC` se incluye en esta etapa. | Aprobado |
| E4-D8 | Criterio de éxito: mismos datos guardados que la versión síncrona, ningún doble cobro, la ventana responde durante el cobro. Matriz de pruebas en §7. | Aprobado |

## 3. Hallazgos del código que condicionan la implementación

Verificados leyendo el commit `30038dd`.

### 3.1 La medición toca BLL
`Recalcular`, la bitácora y la transacción se ejecutan DENTRO de `PagoBLL_790MY.RegistrarCobro`. Para medirlos por separado hay que agregar `Stopwatch` y `Trace.WriteLine("...", "Cobro")` en `PagoBLL_790MY` (alrededor de `_pagoRepository.RegistrarCobro`, `DVManager_08YS.Recalcular()` y de los tres `RegistrarEventoPostCommit`). El propietario lo pidió expresamente, así que está permitido con estas condiciones:
- Solo cronometraje y `Trace`. Sin cambiar lógica, orden de llamadas, excepciones ni firmas.
- En un commit propio y separado, titulado como medición, para poder revertirlo sin tocar el resto.
- `GenerarFactura` y el total de `RegistrarCobro` se pueden medir también desde la GUI.
- Al terminar, informar al propietario si la instrumentación se deja o se revierte. Por defecto queda en su commit separado, sin mezclar.

Si el entorno de Claude Code no puede ejecutar la aplicación contra la base, NO se inventan números: se deja la instrumentación lista, se escribe «pendiente de ejecución» en la sección «Mediciones» y se indica cómo leer la salida (ventana Output de Visual Studio o DebugView). Registrar también cuántas filas tienen las 18 tablas de `DvBLL_08YS.TablasMonitoreadas`, porque el costo de `Recalcular` depende del tamaño de la base.

### 3.2 Qué hace hoy el cobro (todo síncrono, en el hilo de la interfaz)
`btnRegistrarCobro_790MY_Click` hace: `MessageBox` Sí/No → `Cursor = WaitCursor` → `ObtenerFacturaParaCobrar()` (que puede llamar `_facturaBll.GenerarFactura`, una escritura) → arma la `SolicitudCobro` → `_pagoBll.RegistrarCobro`.
Dentro de `RegistrarCobro`: lectura de la factura, validaciones, banco simulado, transacción `Serializable` (`UPDLOCK, HOLDLOCK`), `DVManager_08YS.Recalcular()` (lee las 18 tablas completas y guarda los DV), tres eventos de bitácora (con un reintento cada uno) y una lectura final de la factura.

### 3.3 Acceso a controles desde otro hilo (el error más probable)
El click actual lee controles de la interfaz DENTRO del bloque que pasaría a `Task.Run`: `txtPropina_790MY`, `txtDescuento_790MY`, `TipoDescuentoSeleccionado`, `txtMontoRecibido_790MY`, `txtNroTarjeta_790MY`, `CuotasSeleccionadas`, `_metodo` y `_detalle`. Leer una propiedad de un control desde otro hilo lanza `InvalidOperationException`.
**Regla:** todo valor de pantalla se lee y se guarda en variables locales ANTES del `await Task.Run`. Dentro del `Task.Run` solo van `GenerarFactura` y `RegistrarCobro` con datos ya capturados. Nada de `E.T(...)`, `MessageBox`, banners ni acceso a controles dentro del hilo secundario: los textos se arman después del `await`.
`ObtenerFacturaParaCobrar()` mezcla lectura de controles con la llamada al BLL: hay que separarla en dos partes (captura en la interfaz, llamada en el hilo secundario).

### 3.4 `FormComprobanteFactura_790MY` no tiene `MessageBox`
La búsqueda no encontró ninguna llamada a `MessageBox` en ese archivo (solo `preview.ShowDialog(this)` para la vista previa). Antes de modificarlo: confirmar si usa `E.MostrarError` o `E.MostrarExito` (que son MessageBox legacy) y si sus manejadores de impresión o PDF tienen errores sin tratar. Si no hay nada que migrar, se deja sin cambios y se documenta. No se cambia por cambiar.
`FormFacturacionCobro_790MY.cs` sí tiene `MessageBox` directos (confirmar y rechazo) y usa los envoltorios legacy `E.MostrarError` y `E.MostrarExito` en varios puntos, incluido `btnPreTicket_790MY_Click` y la carga. Hacer inventario de todos con el mismo criterio que `Inventario_MessageBox_Etapa2.md`.

### 3.5 El contenedor principal no es un MDI clásico
`FormMDI_08YS` aloja los formularios hijos con `panel2.Controls.Add(childForm)`, no con `MdiParent`. Por lo tanto:
- `Enabled = false` en `FormMDI_08YS` deshabilitaría también al formulario de cobro, incluido el spinner.
- **Propuesta:** exponer en `FormMDI_08YS` un método de bloqueo de navegación (por ejemplo `BloquearNavegacion(bool)`) que deshabilite solo la barra, el menú o el panel lateral, sin tocar `panel2`. Verificar en el código real cómo está armada la navegación.
- Ya existe `FormMDI_FormClosing`: debe cancelar el cierre de la aplicación mientras haya un cobro en curso, con un aviso claro (no un MessageBox nativo).
- Mientras haya un cobro en curso, también se bloquea cerrar sesión y cualquier otra acción que cierre el formulario.

### 3.6 `PermissionFilter` y los colores del botón
- `PermissionFilter_08YS.Aplicar` busca `btnRegistrarCobro_790MY` por nombre y modifica solo `Visible`. El control nuevo debe conservar ESE nombre.
- `ActualizarCobro()` hoy cambia `Enabled`, `BackColor`, `ForeColor` e `IconColor` del botón (`FontAwesome.Sharp.IconButton`). El botón nuevo debe exponer una API equivalente (por ejemplo un estado `Disponible / Deshabilitado / Procesando`) y esa lógica debe pasar al control. La regla del proyecto prohíbe configurar propiedades visuales de controles directamente en `FormX.cs`.
- `btnEfectivo_790MY` y `btnTarjeta_790MY` son `IconButton` recoloreados desde `SeleccionarMetodo`. Al reemplazarlos por `SlidingTab_UC`, conservar el método `SeleccionarMetodo(MetodoPago_790MY)` y su lógica; si se cambian los nombres de controles, buscar y corregir TODAS las referencias (colores, `Visible`, permisos, i18n por `Tag`).

### 3.7 Concurrencia en la base (ya resuelta; no se toca)
La DAL abre una conexión por operación y la transacción `Serializable` con `UPDLOCK, HOLDLOCK` impide el doble cobro de la misma factura: la segunda caja recibe `FacturaNoAbierta`. `SessionManager_08YS` no tiene afinidad de hilo. Nada de esto requiere cambios.

### 3.8 Hardware
Según las respuestas de la Etapa 3, el KDS corre en All-in-One táctiles de gama baja o media o en PC viejas. Aplica también al cobro: animación liviana, `Timer` activo solo mientras procesa, objetivos táctiles de 40 a 56 px.

## 4. Plan de ejecución

1. **Rama y base.** Verificar que se parte de `beta` limpia (sin cambios sin commitear en el código). Crear `etapa4/facturacion-y-cobro`.
2. **Inventario.** Listar los `MessageBox` y envoltorios legacy de ambos formularios, clasificados (confirmación, rechazo, error de regla, error inesperado, éxito). Guardar el resultado como `Inventario_MessageBox_Etapa4.md` en esta carpeta.
3. **Medición (commit propio).** Instrumentar según §3.1. Ejecutar escenarios si el entorno lo permite: efectivo, tarjeta aprobada y tarjeta `0002`, con varias repeticiones. Registrar en la sección «Mediciones» de este archivo con la tabla de tiempos por tramo y el tamaño de la base.
4. **`SlidingTab_UC`** (commit propio). `UserControl` en `GUI/Estilo` con `.Designer.cs` separado, como `Banner_UC_790MY`. Requisitos:
   - colores y fuentes de `TemaFogon`;
   - evento de cambio de selección;
   - navegación con flechas y foco visible, con orden de tabulación coherente;
   - escalado DPI (el formulario usa `AutoScaleMode.Dpi`);
   - texto traducido por `idiomas.json`;
   - deslizamiento del indicador de unos 200 ms, que se desactiva con `SystemInformation.UIEffectsEnabled == false`;
   - sin fugas GDI+: liberar pinceles, plumas y timers en `Dispose`.
5. **Botón de cobro con spinner** (commit propio). Control con estados Disponible, Deshabilitado y Procesando:
   - el indicador circular se dibuja con GDI+ y un `Timer` de unos 40 ms que corre solo en «Procesando»;
   - se detiene y se libera al volver al estado original, al fallar y al cerrar el formulario;
   - con «reducir animaciones», el indicador es estático;
   - no cambia el nombre `btnRegistrarCobro_790MY` (ver §3.6);
   - el texto del estado «Procesando» sale de `idiomas.json`.
6. **Migración de `FormFacturacionCobro_790MY`** (separar `.Designer.cs` y `.cs`):
   - cambiar los dos `IconButton` de medio de pago por `SlidingTab_UC`;
   - cambiar el botón de cobro por el control nuevo;
   - agregar `Banner_UC_790MY` al formulario (diseño en el Designer);
   - reemplazar `MessageBox` y envoltorios legacy por `ConfirmModal_790MY.Confirmar`, `E.MostrarErrorEnBanner` y `E.MostrarExitoToast`, como se hizo en `FormCocinaKDS_790MY`;
   - validación en línea con `ErrorProvider` donde corresponda, con las reglas actuales (no se agregan reglas);
   - rechazo `0002`: banner con el mensaje de `PagoRechazadoException_790MY` y foco con `SelectAll` en `txtNroTarjeta_790MY`;
   - quitar `Cursor = WaitCursor`.
7. **Asincronismo** (en el mismo formulario):
   - Flujo del click: capturar valores de pantalla (§3.3) → `ConfirmModal_790MY` → si confirma: marcar «procesando» → `await Task.Run(() => { factura = GenerarFactura(...); resultado = RegistrarCobro(...); })` → restaurar el estado en `finally`.
   - La bandera de «procesando» se activa ANTES del primer `await` y bloquea doble clic y reentradas.
   - Deshabilitar inputs, listado de mesas, selector de medio y descuento, y botón «pre-ticket» mientras dure. La navegación del contenedor se bloquea según §3.5.
   - `FormClosing` del formulario: cancelar mientras haya cobro en curso.
   - Excepciones: capturar fuera del `Task.Run` (con `await` llegan al hilo de la interfaz). `PagoRechazadoException_790MY` va al banner; reglas (`ArgumentException`) y permisos, al banner; fallas inesperadas, al `MessageBox` nativo de error fatal, que es el único permitido por E2-D1.
   - Éxito: toast, abrir `FormComprobanteFactura_790MY` como hoy y recargar mesas.
8. **`FormComprobanteFactura_790MY`:** según el resultado de §3.4.
9. **i18n.** Agregar las claves nuevas a `idiomas.json` en es y en, siguiendo el criterio de claridad de E2-D3 (qué pasó y cómo corregirlo). Verificar que los controles nuevos respondan al cambio de idioma como los de las otras pantallas (`IIdiomaObserver_08YS`, `UpdateIdioma`).
10. **Compilación y pruebas.** Compilar el proyecto GUI con 0 errores. Ejecutar el checklist de §7. Escribir `Checklist_Pruebas_Etapa4.md` en esta carpeta, siguiendo el formato de `Checklist_Pruebas_Etapa2.md`.
11. **Cierre.** Completar «Resultado» en este archivo, commitear en la rama y avisar al propietario.

## 5. Riesgos y tratamiento

| Riesgo | Tratamiento |
|---|---|
| `InvalidOperationException` por leer controles dentro del `Task.Run` | Capturar todo antes (§3.3). Revisión manual del lambda: debe usar solo variables locales. |
| Doble clic, reintento o doble `GenerarFactura` | Bandera `procesando` antes del primer `await`. La base ya frena el doble cobro de la misma factura. |
| Cierre del formulario o de la aplicación a mitad del cobro | `FormClosing` del formulario y de `FormMDI_08YS` cancelan el cierre mientras procesa. |
| El control nuevo rompe `PermissionFilter` o `ActualizarCobro` | Conservar el nombre `btnRegistrarCobro_790MY` y exponer un estado propio (§3.6). |
| Bloquear el contenedor deshabilita también el cobro | No usar `Enabled = false` en todo `FormMDI_08YS` (§3.5). |
| Timer del spinner que sigue vivo | Iniciar solo en «Procesando». Detener en `finally` y en `Dispose`. |
| Falla entre `GenerarFactura` y `RegistrarCobro` | Es el comportamiento de hoy (dos operaciones separadas). No se unifican: no hay cambios de BLL. Documentar. |
| Recalcular de DV simultáneo entre dos cajas | Riesgo existente, no introducido. Se registra en la documentación; no se corrige acá. |
| Tarjeta rechazada deja inputs bloqueados | Restaurar el estado en un `finally`. Probar el caso `0002`. |

## 6. Pendiente de confirmación (NO implementar sin respuesta del propietario)

1. **Verificar antes de reintentar.** Propuesta del análisis: si tras un intento propio llega `FacturaNoAbierta`, consultar `PagoBLL_790MY.ObtenerPagoDeFactura` y mostrar el pago ya registrado como confirmado, en vez de un error confuso. No está en las instrucciones, así que no se implementa hasta que se apruebe.
2. **Instrumentación de BLL.** Si queda o se revierte después de medir (§3.1).
3. **`GenerarFactura`.** Verificar leyendo su código que sea idempotente (devuelve la factura abierta vigente) y que no acceda a la interfaz antes de moverla a un hilo secundario. Si no cumple alguna de las dos cosas, detenerse y consultar.
4. **Botón «pre-ticket»** (`btnPreTicket_790MY_Click`, que también llama a `GenerarFactura`): no se hace asíncrono porque no se pidió. Solo se deshabilita mientras haya un cobro en curso y se migran sus avisos legacy.

## 7. Criterios de salida y pruebas

Mismos datos guardados que la versión síncrona, ningún doble cobro y la ventana responde durante el cobro.

- Efectivo con vuelto y efectivo insuficiente.
- Tarjeta aprobada con cuotas 1, 3, 6 y 12.
- Tarjeta rechazada (terminación `0002`): banner, foco en el campo y el cobro sigue disponible tras el rechazo.
- Doble clic rápido sobre el botón y sobre «Confirmar» del modal.
- Intento de cerrar el formulario y la aplicación durante el cobro.
- Dos instancias de la aplicación cobrando la misma factura (esperado: la segunda recibe «factura no abierta»).
- Cobro sin la patente `GenerarFactura` (usa la factura ya emitida) y sin la patente `RegistrarCobro`.
- Escala 100, 125 y 150 %.
- Español e inglés (incluye el cambio de idioma con el formulario abierto).
- «Reducir animaciones» activado: spinner y deslizamiento sin movimiento.
- Verificar que los eventos de bitácora (`PagoRegistrado`, `ReservaFinalizada`, `MesaEstadoActualizado`, `PagoRechazado`) salgan igual que antes y en el mismo orden.
- Compilación del proyecto GUI sin errores ni advertencias nuevas.

## 8. Reglas de entrega

- Una rama y varios commits pequeños (medición, `SlidingTab_UC`, botón, migración del formulario, i18n, documentación).
- `FormX.Designer.cs`: solo diseño visual dentro de `InitializeComponent()` o auxiliares privados. `FormX.cs`: solo lógica y eventos. Constructor limpio. Se conservan namespace, clases parciales y nombres de controles.
- Sin `Co-authored-by` en los commits.
- No se tocan: lógica de BLL y DAL (salvo la medición de §3.1), `FormSeleccionarMesa`, KDS, ni formularios fuera de este alcance.

## 9. Qué informar al terminar
Rama y commits, tabla de mediciones (o «pendiente de ejecución» con el motivo), lista de `MessageBox` migrados y los que se conservan con su razón, resultado del checklist, estado de la compilación, y cada punto de §6 con lo que se decidió o quedó abierto.

## Mediciones

**Tiempos: pendiente de ejecución.** Claude Code no puede operar la aplicación de escritorio (requiere iniciar sesión e interactuar con la pantalla), así que no se inventan números. La instrumentación quedó lista en el commit `9698f11`.

Cómo leerlos: ejecutar en Debug desde Visual Studio (ventana Output) o con DebugView, y filtrar por `Cobro:`. Cada cobro escribe, en orden: `GenerarFactura`, `Transaccion RegistrarCobro`, `DVManager.Recalcular`, `Bitacora (3 eventos)` y `RegistrarCobro (total)`. Tras la migración, las dos líneas de la GUI se escriben desde el `Task.Run`.

| Escenario | Rep. | GenerarFactura | Transacción | Recalcular | Bitácora | Total RegistrarCobro |
|---|---:|---:|---:|---:|---:|---:|
| Efectivo | 5 | _pend._ | _pend._ | _pend._ | _pend._ | _pend._ |
| Tarjeta aprobada | 5 | _pend._ | _pend._ | _pend._ | _pend._ | _pend._ |
| Tarjeta `0002` | 3 | _pend._ | — | — | — | — |

Tamaño de la base `TP_Ing_Soft` en `PC_Maximo` el 2026-10-19 (filas según `sys.partitions`, consulta de solo lectura), tablas de `DvBLL_08YS.TablasMonitoreadas`:

| Tabla | Filas | Tabla | Filas | Tabla | Filas |
|---|---:|---|---:|---|---:|
| Users | 6 | RolPermiso | 12 | Productos | 15 |
| Roles | 7 | Clientes | 15 | Pedidos | 14 |
| Familias | 17 | Mesas | 12 | ItemsPedido | 19 |
| Permisos | 37 | Reservas | 42 | Facturas | 7 |
| FamiliaPermiso | 37 | Empleados | 4 | ItemsFactura | 19 |
| FamiliaIntegrada | 0 | RolFamilia | 23 | Pagos | 6 |

Total: 292 filas. Con una base tan chica, `Recalcular` debería ser corto; el valor de la medición está en ver cómo crece con datos reales.

## Resultado

Rama `etapa4/facturacion-y-cobro`, commits (sin líneas de coautoría):

1. `9698f11` Medición con `Stopwatch` (E4-H1). Toca `PagoBLL_790MY` solo con cronometraje y `Trace` (§3.1). **Queda en su commit separado; se puede revertir sin afectar el resto.** Decidir si se deja.
2. `56a9d29` `SlidingTab_UC_790MY` (`GUI/Estilo`, con `.Designer.cs`). Se nombró con el sufijo `_790MY` como `Banner_UC_790MY`.
3. `a1de4dd` `BotonCobro_790MY` (deriva de `Button`; conserva clic, Enter/Espacio, `Tag` de idioma y el nombre `btnRegistrarCobro_790MY`).
4. `22bb9d4` Migración de `FormFacturacionCobro_790MY`, `FormComprobanteFactura_790MY` y bloqueo en `FormMDI_08YS`.
5. `d7fff2d` Textos es/en.
6. Documentación: `Inventario_MessageBox_Etapa4.md` y `Checklist_Pruebas_Etapa4.md`.
7. Consolidación final (instrucciones del propietario del 2026-10-19):
   - `FormFacturacionCobro_790MY`: `await Task.Delay(2500)` antes del `Task.Run` (constante `LatenciaSimuladaMs`) para simular la pasarela bancaria. Queda dentro del bloque protegido: el spinner y los bloqueos duran también esa espera. No entra en las mediciones de `Trace`.
   - `FormAbrirMesa_790MY`: los botones «Con reserva» / «Walk-in (sin reserva)» (`pnlModo_790MY`, `tlpModo_790MY`, `btnModoReserva_790MY`, `btnModoWalkIn_790MY`) se reemplazaron por `sldModo_790MY` (`SlidingTab_UC_790MY`, claves `AM_modo_reserva` y `AM_modo_walkin`). `AplicarModo` se conserva; si el cambio viene del selector, el foco queda en él para seguir con el teclado.
   - `FormEmpleados_790MY`: el vínculo empleado-usuario queda de solo lectura (ver «Vinculación empleado-usuario»).

## Vinculación empleado-usuario (relevamiento)

- Esquema real: la columna es `Empleados.UsuarioDNI` (`int NULL`), no `UsuarioID`. FK `FK_Empleados_Users` a `Users.DNI` e índice único filtrado `UX_Empleados_UsuarioDNI` (un usuario, como máximo, vinculado a un empleado).
- Datos al 2026-10-19: «Maximo» (DNI 12345678) vinculado a sí mismo; **también** Pedro Pascal (22222222) vinculado a `22222222GestorDeAccesos`; Lucas Gómez y Martina Ruiz sin usuario.
- `EmpleadoBLL_790MY.ValidarUsuarioAsociado` acepta `UsuarioDNI = null` sin validar nada: un empleado sin usuario se carga y se guarda sin excepción. La BLL no se modificó.
- GUI: `cmbUsuario_790MY` queda siempre deshabilitado (solo muestra el vínculo vigente, incluso si el usuario no figura entre los vinculables). Al guardar, el alta va sin usuario y la modificación reenvía el `UsuarioDNI` que el empleado ya tenía, tomado de los datos cargados y no del combo. Así se preserva el vínculo de «Maximo» y ningún vínculo se puede asignar, cambiar ni borrar desde el maestro. El vínculo existente de Pedro Pascal también se conserva tal cual.
- Textos: `FE_lblUsuario` pasa a «Usuario del sistema (solo lectura)» y `FE_lblUsuarioAyuda` explica que el vínculo no se gestiona desde este maestro.

## Cierre

**Etapa 4 TERMINADA.** Pruebas de integración informadas como **exitosas por el propietario** (2026-10-19); Claude Code no ejecutó la app (verificó compilación y código). Las mediciones de tiempos de la tabla de arriba siguen sin cargar.

Compilación del proyecto GUI (MSBuild): **0 errores en Debug y en Release**, 0 advertencias de C#.

Cómo quedó cada punto:
- §3.3: el clic captura todos los valores de pantalla en variables locales antes del `await Task.Run`; el lambda usa solo esas variables y los BLL. `ObtenerFacturaParaCobrar` se eliminó: el chequeo de «sin patente y sin cuenta emitida» se hace antes del modal, en la interfaz.
- §3.5: `FormMDI_08YS.BloquearNavegacion(bool)` deshabilita `panelLateral` (menú y cerrar sesión) y `panelIdioma`, sin tocar `panel2`. `FormMDI_FormClosing` cancela el cierre (salvo apagado de Windows) y dispara `CierreBloqueado`, que el formulario de cobro muestra como banner ámbar. Hallazgo: `OpenChildForm` hace `panel2.Controls.Clear()` sin cerrar el hijo, por eso el bloqueo de navegación es necesario además del `FormClosing`.
- §3.6: la lógica de colores del botón pasó al control (`Estado`). `btnEfectivo_790MY`/`btnTarjeta_790MY` se reemplazaron por `sldMetodo_790MY`; no había otras referencias. `SeleccionarMetodo` se conserva.
- §3.4: `FormComprobanteFactura_790MY` no tenía `MessageBox` directos, pero sí un `E.MostrarError` en la impresión; pasó a un banner propio.
- Los colores de los componentes nuevos salen de `TemaFogon_790MY` (bordó `#6B1D2F`), apenas distinto del `E.Bordo` del resto de la pantalla.

Pendientes de §6:
1. **Verificar antes de reintentar** (`FacturaNoAbierta` → `ObtenerPagoDeFactura`): no se implementó; sigue a la espera de aprobación.
2. **Instrumentación de BLL**: queda en `9698f11` y se fusiona con el resto en `beta` (instrucción de consolidar todo). Se puede revertir ese commit si se decide quitarla.
3. **`GenerarFactura`**: verificado leyendo el código. Es idempotente (si la factura abierta tiene el mismo detalle e importes, la devuelve sin escribir) y no accede a la interfaz. Lee textos de `TraductorManager_08YS` solo para armar mensajes de excepción, una lectura del diccionario; el cambio de idioma está bloqueado durante el cobro.
4. **Pre-ticket**: sigue síncrono; se deshabilita durante el cobro (está dentro de `tlpCuenta_790MY`) y sus avisos se migraron (toast y banner).
