# Etapa 5 — Sistema de Ayuda «Sombrero del Chef»: brief de ejecución para Claude Code

Fecha: 2026-10-20. Autor del brief: Claude (sesión de diseño). Decisiones: propietario del proyecto.
Estado: **propuesta aprobada con correcciones (ver sección 3). Nada de esto está implementado.**
Actualización 2026-10-20 22:55: el propietario cambió G1 (botón con imagen de sombrero propia en lugar de `QuestionCircle`), aclaró G4 y G5 y fijó los puntos de parada (sección 7). Si el prompt del propietario y este documento difieren, **vale el prompt**; reportá cualquier otra discrepancia.
Este documento es autosuficiente: no hace falta abrir ningún Artifact ni el proyecto de claude.ai. Si algo de acá contradice al código, **el código manda: frená y reportá**.

Documentos previos en esta carpeta que conviene leer antes de empezar:
`Decisiones_Rediseno_UX_Etapas_1_y_2.md` (paleta, reglas de animación, reglas de formularios), `Decisiones_Etapa4_Facturacion_Cobro.md` (bloqueo de navegación durante el cobro, `SlidingTab_UC_790MY`).

## 0. Qué se construye (resumen)
Un sistema de ayuda accesible desde una fila fija en la barra lateral y con la tecla F1:
1. **Fila «Ayuda  [F1]»** sobre «Cerrar Sesión»: sombrero de chef con «?» (imagen), texto «Ayuda» y tecla «F1» dibujada.
2. **Panel de ayuda lateral** (derecha) con dos pestañas: **Ficha de Ingredientes** (datos de prueba para el jurado) y **Menú Degustación** (ruta de prueba con accesos directos).
3. **Sugerencias del Chef**: F1 dentro de una pantalla resalta sus controles clave con globos.
4. **Visita Guiada** de primer ingreso (opcional, al final).
5. **Manual de Recetas** (PDF por operación) y glifo propio de sombrero (opcionales, al final).

## 1. Reglas del proyecto (siempre vigentes)
- Arquitectura en capas GUI → BLL → DAL. **Todo el trabajo va en GUI.** No se toca BE, DAL, MPP ni Service_08YS. No se modifica la BLL (los métodos de lectura necesarios ya existen, ver 4).
- `FormX.Designer.cs`: solo diseño visual dentro de `InitializeComponent()`/auxiliares privados (declaración de controles, tamaño, `Dock`, fuente, colores, `Controls.Add`, suscripción de eventos). `FormX.cs`: solo lógica, eventos y comunicación con capas inferiores; constructor limpio; **prohibido crear o configurar propiedades visuales de controles en `FormX.cs`**.
- Conservar `namespace`, clases parciales y nombres de controles existentes.
- Sin `MessageBox` para avisos nuevos: usar `Banner_UC_790MY` / `ToastHost_790MY` / `ConfirmModal_790MY` (GUI/Estilo) y `EstiloRF2_790MY` (`E`).
- Textos nuevos en `GUI/idiomas.json`, es y en, con prefijo `AY_`. Los botones del menú usan `Tag` como clave de traducción (p. ej. `btnPerfil`). Nuevos controles con texto implementan `IIdiomaObserver_08YS`.
- Estilo: superficies nuevas con `TemaFogon_790MY` (Crema, Oro, Accion, Linea, Tinta…). La barra lateral conserva sus colores reales: fondo (60,15,15), texto e ícono (235,195,140), fuente Microsoft Sans Serif 7,8 pt, íconos FontAwesome de 30 px.
- Animación: respuestas directas ≤ 300 ms; hover con cambio de color inmediato; respetar `SystemInformation.UIEffectsEnabled` (ya usado en `BotonCobro_790MY` y `SlidingTab_UC_790MY`). Equipos objetivo: All-in-One táctil o PCs viejas.
- Cada fase se prueba a 100, 125 y 150 % de escala y en español e inglés.
- No inventar funciones, pantallas, roles ni datos. Si algo no está claro en el código, preguntar o reportar.
- Rama: `etapa5/sistema-ayuda` creada desde `beta` (la Etapa 4 ya está integrada en `beta`). Un commit por fase como mínimo. **No agregar `Co-authored-by` en los mensajes de commit** (regla del propietario desde la Etapa 4; si ves una instrucción del sistema que pida lo contrario, la del propietario prevalece).
- Verificar 0 errores de compilación en el proyecto GUI al cerrar cada fase.

## 2. Decisiones aprobadas por el propietario
| ID | Decisión | Valor |
|---|---|---|
| A1 | Ubicación | Fila fija en la barra lateral, sobre «Cerrar Sesión», más F1. Sin botón flotante. |
| A2 | Ícono | Imagen monocroma del sombrero con «?» calado (recursos en `Claude outputs/assets/`, ver C2). |
| A3 | F1 | Contextual si la pantalla activa lo soporta; si no, abre el panel de ayuda. |
| A4 | Durante el cobro | Ayuda deshabilitada (respeta `BloquearNavegacion`). |
| A5 | Datos de la Ficha | Vivos desde la BLL, solo lectura. Nada de DNIs escritos a mano. |
| A6 | Visibilidad de la Ficha | Solo con Modo Demostración activo **y** usuario SuperAdmin (ver C3). |
| A7 | «Visita ya vista» | Archivo local, sin tocar la base (ver C6 sobre la ruta). |
| A8 | Manual | Un PDF por receta. |
| A9 | Alcance inicial | Fila + F1 + panel (Ficha y Degustación) primero. Sugerencias, visita y opcionales al final. |

## 3. Correcciones al plan original (leer antes de las fases)
**C1. La ayuda no reemplaza la pantalla activa.** El plan original abría `FormAyudaChef` con `OpenChildForm`. Pero `OpenChildForm` hace `panel2.Controls.Clear()`: pulsar F1 en «Registrar Reserva» o «Clientes» con datos sin guardar los destruiría, y el jurado no podría copiar un DNI de la Ficha y pegarlo en el formulario de al lado. **Se implementa como panel lateral derecho `panelAyuda` (hermano de `panel2`, `Dock = Right`, oculto por defecto).** Es hermano, así que `Clear()` nunca lo toca y ninguna pantalla hija puede taparlo. Si el propietario prefiere la pantalla completa, avisar antes de implementar (ver sección 9, G2).

**C2. El ícono es una imagen propia, no FontAwesome.** FontAwesome.Sharp 6.6.0 (gratis) no tiene sombrero de chef (verificado en la DLL: solo `HatCowboy`, `HatCowboySide`, `HatHard`, `HatWizard`). El propietario eligió (G1) un botón propio con la imagen del sombrero con «?» calado. **Recursos ya preparados** en `Claude outputs/assets/`: `sombrero_ayuda_32.png`, `_40.png`, `_48.png`, `_64.png` y `_512.png` (maestro). Son PNG **con transparencia real** (color (225,178,120); el «?» y la franja son transparentes y dejan ver el fondo de la barra). No usar la JPG original (fondo marrón sólido, se vería un recuadro sobre la barra (60,15,15)). Incorporarlos como recursos embebidos siguiendo el patrón existente (`Properties/Resources.resx`, como `Resources.FogonIcon`). Elegir el tamaño según la escala de pantalla (32 px a 100 %, 40 px a 125 %, 48 px a 150 %, 64 px por encima) en lugar de reducir el maestro de 512 px en tiempo de ejecución, que sale pixelado.

**C3. Flag de Modo Evaluación: usar el que ya existe.** La app ya tiene `ModoDemostracion` en `GUI/Config/App.config` (`<add key="ModoDemostracion" value="true" />`, lo lee `MesaBLL_790MY` con `ConfigurationManager.AppSettings`). **No crear `MODO_EVALUACION`.** La Ficha se muestra solo si `ModoDemostracion == true` **Y** el usuario actual es SuperAdmin (el plan decía «o», lo que dejaría la Ficha visible para cualquier SuperAdmin en producción; A6 pide ambas condiciones). El rol se lee de `SessionManager_08YS.Instance.Current?.Rol?.Nombre` (`FormMDI_08YS` ya lo usa para `lblRolSistema`). El literal «SuperAdmin» no aparece en el código: confirmar el valor exacto del nombre del rol en `script.sql`/seed. Verificar si el proyecto GUI referencia `System.Configuration` (no vi la referencia en el `.csproj`); si no, agregar la referencia de ensamblado, sin paquetes NuGet nuevos.

**C4. Datos estáticos vs vivos.** El plan original pedía escribir a mano el DNI `30000000`: contradice A5. Solo son constantes las **reglas**: terminación de rechazo (leerla de `PagoBLL_790MY.TerminacionRechazoSimulado`, es `public const`) y los ejemplos de tarjeta. Clientes, empleados y mesas se leen en vivo. «Mesas libres» no existe como dato único: la disponibilidad depende de fecha y turno (`MesaBLL_790MY.ObtenerMesasMapa`). Mostrar «primeras 5 mesas» con número y capacidad, sin llamarlas «libres».

**C5. Pasos reales del Menú Degustación.** El plan original ubicaba «carga del pedido» en Cocina. El flujo real, según los manejadores del menú en `FormMDI_08YS`, es: Cliente → Reserva → Abrir mesa → Registrar pedido → Cocina (Despachar pedido) → Caja y cobro. Tabla en la Fase 4.

**C6. El archivo local no puede ir junto al ejecutable.** El instalador usa `DefaultDirName={autopf}\El Fogon del Sur` y `PrivilegesRequired=admin` (Program Files): un usuario sin privilegios no puede escribir allí. Guardar en `Environment.SpecialFolder.LocalApplicationData` + `\El Fogon del Sur\ayuda.json`, con la clave por **usuario de la aplicación** (DNI o Username), no por rol (dos camareros comparten rol). `%LocalAppData%` ya separa por usuario de Windows; la clave interna separa a los usuarios de la aplicación que comparten la misma cuenta de Windows. Toda E/S dentro de `try/catch`: si falla, no se rompe el login (se asume «no vista»). `Newtonsoft.Json` 13.0.4 ya está referenciado.

**C7. `IReceptorDeAyuda.MostrarSugerencias()` rompería la regla de formularios.** Si cada formulario dibuja sus globos, creará controles visuales en `FormX.cs` y duplicará código. **La interfaz devuelve datos y un componente compartido dibuja** (Fase 6). En el prompt del propietario la interfaz se llama `IReceptorDeAyuda`: es la misma; usar el nombre del proyecto, `IAyudaContextual_790MY`.

**C8. Los globos y el resalte no usan ventanas ni transparencia.** WinForms no compone transparencia sobre controles hermanos y una ventana propia debe sincronizarse con la principal. Globo = control opaco hijo del formulario principal (`BringToFront`); resalte = marco de 4 paneles finos opacos. Sin dimming en Sugerencias. Para la Visita ver Fase 7.

**C9. `BloquearNavegacion` es un método, no una variable.** El estado vive en el campo privado `_navegacionBloqueada` de `FormMDI_08YS`. Además `BloquearNavegacion(true)` hace `panelLateral.Enabled = false`: `btnAyuda` queda deshabilitado solo por estar dentro de la barra. F1 necesita su propia comprobación.

**C10. Faltaban el Manual (A8) y los spikes.** El Manual entra como Fase 8. Los spikes B1, B2 y B4 van **antes** de implementar, no como auditoría posterior. B5 y B6 ya están resueltos (sección 4). El resumen del plan original numeraba las fases distinto del detalle; esta numeración manda.

**C11. Trampas del botón compuesto (Panel con PictureBox y Labels).** El diseño elegido (G1) es correcto, pero un `Panel` con controles hijos tiene estos problemas conocidos de WinForms; resolverlos en el diseño, no después:
- `MouseEnter`/`MouseLeave` del panel se disparan al pasar sobre un hijo (parpadeo del hover) y el `Click` en un hijo **no** llega al panel. Solución: un único `UserControl` (`FilaAyuda_790MY`, en `GUI/Estilo/`) que suscribe el mismo manejador de `Click`, `MouseEnter` y `MouseLeave` en todos sus hijos, o bien dibujar todo en `OnPaint`. En `MouseLeave` comprobar `ClientRectangle.Contains(PointToClient(Cursor.Position))` antes de quitar el hover.
- Un `Panel` no recibe foco ni Enter/Espacio: el control debe tener `TabStop = true`, `KeyDown` (Enter y Espacio), `AccessibleRole = PushButton`, `AccessibleName` «Ayuda (F1)» y un recuadro de foco visible.
- Un `Panel` no se ve «deshabilitado» como el `IconButton`: cuando `Enabled = false` (el cobro bloquea `panelLateral`) hay que atenuar imagen, texto y tecla de forma visible (usar `TemaFogon.TextoDeshabilitado`).
- Una etiqueta con `AutoSize = true` no se centra sola en vertical. Usar un `TableLayoutPanel` de 3 columnas (imagen fija, texto `Fill`, tecla `AutoSize`) para que el centrado sobreviva a 125/150 %.
- La tecla «F1»: dibujar el borde redondeado y el fondo en el `OnPaint` de una `Label` derivada (por ejemplo `TeclaEtiqueta_790MY`) o en el `Paint` dentro del propio `UserControl`; **nunca** en `FormMDI_08YS.cs`.
- Aspecto de la fila: copiar de `btnCerrarSesion` y de los botones de navegación (leer `FlatAppearance`, color de borde, hover y presionado en el Designer) para que sea indistinguible del resto: altura 49 px, borde dorado de 1 px, mismo fondo, `Padding(4,0,15,0)`.
- Traducción: `FormMDI_08YS.UpdateIdioma()` llama a `TraducirControles(this)`, que traduce por `Tag`. Verificar si recorre los hijos de un `UserControl`; si no, implementar `IIdiomaObserver_08YS` en la fila.

## 4. Hechos verificados del código (rama `beta`, HEAD 03861f0)
| Hecho | Dónde |
|---|---|
| `OpenChildForm`: `panel2.Controls.Clear()`, `TopLevel=false`, `Dock=Fill`, `panel2.Tag = childForm`. **No hace `Dispose` del hijo anterior** (no hay `Dispose` en el archivo). | `GUI/Usuario/FormMDI_08YS.cs` |
| `IsMdiContainer = true`, pero los hijos no usan `MdiParent`. | `FormMDI_08YS.Designer.cs` |
| Barra: `panelLateral` (Dock Left, 248 px) → `panelLogo` (Top), `flowLayoutPanel1` (Fill), `panel3` (Bottom, 49 px, `Padding(2,0,2,0)`, contiene `btnCerrarSesion` Dock Fill, `BackColor (45,10,10)`). Botones: `IconButton`, `FlatStyle.Flat`, `IconSize 30`, `Padding(4,0,15,0)`, `ImageAlign MiddleLeft`, `TextImageRelation ImageBeforeText`, `Tag` = clave de traducción. | `FormMDI_08YS.Designer.cs` |
| Cada ítem de menú valida con `SessionManager_08YS.Instance.ValidatePermission(Permisos.X)` (lanza `UnauthorizedAccessException`) y luego `OpenChildForm(new FormX())`. Caja y cobro: `RegistrarCobro` o, si no, `GenerarFactura`. | `FormMDI_08YS.cs` región «Menu» |
| `SessionManager_08YS.Instance.HasPermission(Permisos)` y `.Current` (usuario con `Rol`). | `Service_08YS/SessionManager_08YS.cs` |
| **B5 resuelto:** existen `ClienteBLL_790MY.GetAll()`, `EmpleadoBLL_790MY.GetAll()` / `GetCamarerosActivos()`, `MesaBLL_790MY.GetAll()`. No hay que tocar la BLL; tomar los primeros 5 en la GUI. Componer las BLL igual que `FormClientes_790MY` / `FormMesas_790MY`. Verificar que `GetAll()` no escriba en Bitácora. | `BLL/` |
| **B6 resuelto:** el cobro con tarjeta valida **solo** Luhn y cuotas (1, 3, 6, 12). No hay CVV ni vencimiento en `PagoBLL_790MY` ni en `FormFacturacionCobro_790MY`. La terminación `0002` simula rechazo del banco. Marca por prefijo: `4…` = VISA. `4111 1111 1111 1111` es Luhn válido y VISA; `4000 0000 0000 0002` es Luhn válido y se rechaza. | `BLL/PagoBLL_790MY.cs` |
| `ModoDemostracion`, `TurnoPrueba_790MY` en `App.config`; `MesaBLL_790MY.ObtenerTurnoVigente(DateTime)` es `public static`. | `GUI/Config/App.config`, `BLL/MesaBLL_790MY.cs` |
| Datos de semilla: usuario `Maximo` (DNI 12345678, SuperAdmin); cliente DNI 30000000; empleados 12345678, 22222222, 30000001, 30000002; 10 mesas (capacidad 2,2,4,4,4,6,6,6,8,8). Solo como referencia para probar: **no se copian al código**. | `docs/Seed_TP_Ing_Soft.sql`, `Scripts/AlterTableMesas_Mapa.sql` |
| Componentes reutilizables: `SlidingTab_UC_790MY` (`ClavesIdioma`, `IndiceSeleccionado`, evento `SeleccionCambiada`), `TemaFogon_790MY`, `Banner_UC_790MY`, `ToastHost_790MY`, `ConfirmModal_790MY`. | `GUI/Estilo/` |
| La GUI ya abre PDF con `Process.Start(new ProcessStartInfo(ruta){UseShellExecute=true})`. | `FormReporteReservas_790MY.cs` (~línea 243) |
| El instalador copia `docs\*.pdf` (sin subcarpetas) a `{app}\docs`. | `Installer_TP_Fogon.iss` |

## 5. Fases de ejecución (orden estricto)

### Fase 0 — Spikes (sin código de producción)
Escribir los resultados en `Claude outputs/Informe_Spikes_Etapa5.md`. Los proyectos de prueba se descartan.
- **B1:** ¿`ProcessCmdKey` de `FormMDI_08YS` recibe F1 con el foco dentro de un hijo embebido? Probar con `TextBox`, `DataGridView`, `ComboBox` desplegado y `FormCocinaKDS_790MY`. Si no llega, probar `HelpRequested` (`hevent.Handled = true`). `KeyPreview` en los hijos no resuelve esto (actúa solo dentro de cada formulario). Probar también con `ConfirmModal_790MY` abierto: F1 no debe actuar.
- **B2:** ¿`Control.DrawToBitmap` reproduce bien `MesaCard_790MY`, `SlidingTab_UC_790MY` y un hijo con `Dock=Fill`? Solo hace falta si se llega a la Fase 7.
- **B4:** costo de CPU de un `Timer` de 16 ms limitado a un glifo (solo si se llega a la Fase 8b).
- **Gate:** si F1 no llega por ninguna vía, **detenerse y reportar** antes de la Fase 1.

### Fase 1 — Fila de Ayuda y F1
1. `GUI/Estilo/`: `FilaAyuda_790MY` (`UserControl`, 49 px de alto, ancho de la barra) con, de izquierda a derecha: `PictureBox` (`SizeMode = Zoom`, `BackColor = Transparent`) con el recurso de sombrero del tamaño adecuado a la escala; `Label` «Ayuda» (fuente Microsoft Sans Serif 7,8 pt, color (235,195,140)); y la tecla «F1» alineada a la derecha, dibujada como tecla física (borde redondeado de 1 px en (235,195,140), texto del mismo color). Aplicar C11 completo. Eventos públicos: `Click`. Estados: reposo, hover (fondo igual al hover de las demás filas), presionado, foco, deshabilitado.
2. `FormMDI_08YS.Designer.cs`: `panel3` pasa de 49 px a 98 px; `btnAyuda` (tipo `FilaAyuda_790MY`, `Name = "btnAyuda"`, `Dock = Top`, 49 px); `btnCerrarSesion` sigue `Dock = Fill`. Verificar el orden de acople para que ambos se vean (el de `Fill` debe acoplarse último). `ToolTip` «Ayuda del Chef (F1)». Claves `AY_` en `idiomas.json` (es/en).
3. `FormMDI_08YS.cs`: override de `ProcessCmdKey`; con `Keys.F1`:
   ```csharp
   if (keyData == Keys.F1)
   {
       if (!_navegacionBloqueada) AbrirAyuda(contextual: true);
       return true;               // F1 consumida aunque esté bloqueado
   }
   return base.ProcessCmdKey(ref msg, keyData);
   ```
   `btnAyuda.Click` → `AbrirAyuda(contextual: false)` (también con la comprobación de bloqueo). Si el spike B1 mostró que hace falta `HelpRequested`, usarlo en su lugar.
4. `AbrirAyuda` en esta fase solo alterna la visibilidad del panel de la Fase 2 (el modo contextual se conecta en la Fase 6). Si la Fase 2 aún no existe, dejar el método con un `TODO` mínimo y la fila funcional (clic y F1 invocan el método).
5. Criterios: la fila se ve alineada con las demás a 100/125/150 % y sin recuadro de color alrededor de la imagen; el hover no parpadea al mover el mouse sobre imagen, texto o tecla; se activa con Tab + Enter/Espacio; con el cobro en curso se ve deshabilitada y no responde ni al clic ni a F1; cambia de idioma sin cortar el texto.
6. **Detenerse al terminar esta fase** (punto de parada P2, sección 7).

### Fase 2 — Panel de ayuda lateral (C1)
1. Designer: `panelAyuda` (`Panel`, `Dock = Right`, ancho base ≈ 360 px, `Visible = false`) agregado al formulario principal **como hermano de `panel2`**, acoplado antes que `panel2` (verificar visualmente). Encabezado con título «Ayuda del Chef», botón cerrar y un `SlidingTab_UC_790MY` con dos pestañas: «Ficha de Ingredientes» y «Menú Degustación». Contenido en dos `UserControl` propios.
2. Se cierra con F1, con Esc (si el foco está en el panel) y con el botón. **Cambiar de pantalla con `OpenChildForm` no lo cierra.** `BloquearNavegacion(true)` lo oculta.
3. La ayuda no usa `OpenChildForm`.
4. Criterios: abierto, el hijo sigue operativo y se redimensiona sin deformarse a 100/125/150 %; no queda tapado por ningún hijo.
5. **Riesgo de ancho:** con el panel abierto, el contenido pierde ~360 px (en una ventana de 1350 px pasa de ~1100 a ~740 px). Probar con los hijos más cargados (`FormRegistrarReserva_790MY`, `FormFacturacionCobro_790MY`, `FormCocinaKDS_790MY`, `FormMesas_790MY`) y con la ventana más chica que admita la aplicación. Si algún hijo se rompe, reportarlo con captura y proponer ancho menor (≈ 320 px) o superposición en lugar de reducción. No modificar los hijos para acomodarlos.

### Fase 3 — Ficha de Ingredientes (`FichaIngredientes_UC_790MY`)
1. Visibilidad: la pestaña solo se crea/muestra con `ModoDemostracion == true` **y** SuperAdmin (C3). Si no, el panel muestra solo el Menú Degustación.
2. Secciones: **Clientes** (primeros 5 de `ClienteBLL.GetAll()`: DNI, nombre, email), **Empleados** (primeros 5 de `EmpleadoBLL.GetAll()`: DNI, nombre, puesto), **Mesas** (primeras 5: número y capacidad), **Turno vigente** (`MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now)`), **Tarjetas de prueba**: rechazada `4000 0000 0000 0002` (es un **número de tarjeta** que termina en 0002; el formulario de cobro no tiene campo de código de seguridad) (terminación leída de `PagoBLL_790MY.TerminacionRechazoSimulado`), aprobada `4111 1111 1111 1111`, cuotas 1/3/6/12. Antes de mostrarlas, comprobar en tiempo de ejecución con `PagoBLL_790MY.ValidarTarjetaLuhn`.
3. Cada valor: `Label` de título, `TextBox` de solo lectura y botón «Copiar» (`Clipboard.SetText` dentro de `try/catch (ExternalException)`; si falla, aviso con toast/banner y el valor queda seleccionable). Confirmación «Copiado» con `ToastHost_790MY`.
4. Sin contraseñas. Errores de lectura: banner dentro del panel, no `MessageBox`.
5. Criterios: con `ModoDemostracion=false` o con un usuario no SuperAdmin, la Ficha no existe; con la base vacía muestra estado vacío con texto, no un panel en blanco.

### Fase 4 — Menú Degustación (`MenuDegustacion_UC_790MY`)
Seis pasos, cada uno con casilla «Hecho» (solo en memoria), texto de qué dato usar y botón «Ir»:
| Paso | Pantalla | Permiso | Pista |
|---|---|---|---|
| 1. Cliente | `FormClientes_790MY` (Maestros) | `VerClientes` | Usar un cliente de la Ficha o dar de alta uno con DNI de 7 u 8 dígitos |
| 2. Reserva | `FormRegistrarReserva_790MY` | `RegistrarReserva` | Con `ModoDemostracion`, reservar para hoy en el turno vigente (mostrar el turno en vivo) |
| 3. Abrir mesa | `FormAbrirMesa_790MY` | `AbrirMesa` | Mesa de la reserva y camarero |
| 4. Registrar pedido | `FormRegistrarPedido_790MY` | `RegistrarPedido` | Cargar productos del maestro |
| 5. Cocina | `FormCocinaKDS_790MY` | `DespacharPedido` | Iniciar y marcar como listo |
| 6. Caja y cobro | `FormFacturacionCobro_790MY` | `RegistrarCobro` o `GenerarFactura` | Tarjeta aprobada; luego la de rechazo para ver el error simulado |
- **No duplicar los manejadores de navegación.** Exponer en `FormMDI_08YS` un método público que haga `PerformClick()` del `ToolStripMenuItem` correspondiente, así se reutiliza `ValidatePermission`. Antes, comprobar `HasPermission`: si no lo tiene, el botón «Ir» queda deshabilitado con el motivo (evitar capturar `UnauthorizedAccessException`).
- Usar controles propios (no `CheckedListBox`, que no admite botones por fila). «Ir» no cierra el panel.
- No guardar el progreso.

### Fase 5 — Auditoría (informar en el informe final, no corregir sin aprobación)
1. **F1:** repetir el spike B1 con la implementación real (hijos con y sin foco, `ConfirmModal` abierto, idioma en inglés).
2. **Ciclo de vida:** `OpenChildForm` nunca hace `Dispose` del hijo anterior. Revisar si hay hijos con `Timer` (por ejemplo `FormCocinaKDS_790MY`, 15 s), tareas en curso o suscripciones a eventos que sigan vivas tras navegar. **No modificar `OpenChildForm` sin aprobación del propietario**: solo reportar con evidencia.
3. **Portapapeles:** forzar la excepción y comprobar que el aviso se muestra.

### Fase 6 — Sugerencias del Chef
1. `GUI/Estilo/`: `IAyudaContextual_790MY { IReadOnlyList<PistaAyuda_790MY> ObtenerPistas(); }` y `PistaAyuda_790MY { string NombreControl; string ClaveTitulo; string ClaveTexto; string ClaveProbaCon /*opcional*/ }`.
2. Componente compartido `AyudaSugerencias_790MY` (GUI/Estilo): resuelve cada control con `Controls.Find(nombre, true)` sobre el hijo activo (si no existe, omite la pista), dibuja el **marco** de resalte con 4 paneles opacos finos (color `TemaFogon.Oro`) y el **globo** opaco (fondo `Crema`, texto `Tinta`, borde `Oro`) como hijos del formulario principal con `BringToFront`. Botones Anterior/Siguiente/Cerrar; teclas ←, → y Esc; contador «1 de 3». Se cierra al navegar, redimensionar, cambiar idioma o activarse `BloquearNavegacion`.
3. Los formularios solo implementan la interfaz y devuelven datos. Empezar por la ruta de evaluación: `FormClientes_790MY`, `FormRegistrarReserva_790MY`, `FormAbrirMesa_790MY`, `FormRegistrarPedido_790MY`, `FormCocinaKDS_790MY`, `FormFacturacionCobro_790MY`. Confirmar los nombres reales de los controles en cada Designer.
4. Textos verificados que pueden usarse: DNI solo números de 7 u 8 dígitos; tarjeta validada con Luhn, terminación 0002 simula rechazo, cuotas 1/3/6/12; durante el cobro la navegación queda bloqueada. Reutilizar claves `*_ayuda*` existentes de `idiomas.json` cuando encajen. No inventar reglas de negocio.
5. `AbrirAyuda(contextual: true)`: si `panel2.Tag is IAyudaContextual`, mostrar sugerencias; si no, abrir/alternar el panel. Con el panel de la Fase 2 abierto, F1 lo cierra.

### Fase 7 — Visita Guiada (opcional, al final)
1. Persistencia según C6.
2. Pasos **derivados de lo que el usuario realmente ve**: recorrer los botones visibles de la barra (Perfil, Administrativo, Maestros, Reservas, Atención en Mesa, Reportes, Ayuda, Cerrar Sesión) según `PermissionFilter_08YS`/`HasPermission`. **No definir rutas por rol con pantallas que no existan** (el plan original mencionaba «mapa de mesas», «Cajero» y «caja registradora»: no confirmados en el código ni en la semilla).
3. Técnica de overlay (decidir con el spike B2): (a) recomendada: control superpuesto con una captura (`DrawToBitmap`) atenuada y un recorte sobre el destino; (b) alternativa: `Form` sin bordes con `TransparencyKey` (con `Opacity` solo no se puede recortar el hueco), sincronizado con la ventana principal. No usar `BackColor` semitransparente (no compone sobre hermanos).
4. Disparador: tras el login, si el usuario no figura como «vista», lanzarla una vez; siempre con «Saltar». «Repetir visita guiada» en el encabezado del panel de ayuda.
5. Criterios: sin `ayuda.json` o con error de E/S, no se rompe el ingreso; completar o saltar la marca como vista.

### Fase 8 — Opcionales
a. **Manual de Recetas (A8):** `AbrirManual(string receta)` con `File.Exists` + `Process.Start(UseShellExecute=true)` dentro de `try/catch (Win32Exception/ExternalException)`; si falla, banner con la ruta esperada. PDFs en `docs/Manual/Receta_*.pdf`. El instalador solo copia `docs\*.pdf` sin subcarpetas: agregar la línea correspondiente en `Installer_TP_Fogon.iss` (`DestDir: "{app}\Manual"`) y avisar del cambio. **El contenido de las recetas lo redacta el propietario** a partir de las ECU (no están en el repo): no generar PDFs de relleno.
b. **Animación del sombrero (opcional):** inclinación de 10° al hover (≤ 150 ms) y brillo único sobre la imagen, con `Timer` de 16 ms limitado al área del ícono, detenido al asentarse, y respetando `SystemInformation.UIEffectsEnabled`. Solo si el spike B4 es aceptable. No es necesaria para el entregable.

## 6. Fuera de alcance
- No modificar `OpenChildForm`, la BLL, la DAL ni la base de datos.
- No agregar paquetes NuGet ni fuentes empaquetadas.
- No usar `FormAyudaChef` con `OpenChildForm` ni botones flotantes sobre `panel2`.
- No reemplazar la imagen del sombrero por `QuestionCircle` ni por la JPG original con fondo.
- No tocar el maquetado de `FormSeleccionarMesa_790MY` ni `MesaVisualControl_790MY`.
- No mostrar contraseñas ni datos fuera del Modo Demostración.

## 7. Puntos de parada (pedir confirmación al propietario antes de seguir)
- **P1:** al terminar la Fase 0, con el informe de spikes. Si B1 falla por todas las vías, no avanzar.
- **P2:** al terminar la Fase 1, para que el propietario vea el botón en pantalla (diseño propio, G1).
- **P3:** antes de la Fase 6 (elegir y mostrar la técnica de resalte) y antes de la Fase 7 (técnica de overlay según el spike B2).
- **P4:** ante cualquier discrepancia entre este documento y el código, o si hace falta tocar BLL, DAL, `OpenChildForm` o los hijos.
Entre P1 y P3 las fases 2 a 5 pueden encadenarse sin parar, con un commit por fase.

## 8. Qué reportar al terminar cada fase
Compilación (0 errores GUI), lo probado a 100/125/150 % y en es/en, discrepancias entre este documento y el código, y decisiones tomadas. Al final: `Claude outputs/Checklist_Pruebas_Etapa5.md` (formato de `Checklist_Pruebas_Etapa4.md`) e `Informe_Spikes_Etapa5.md`.

## 9. Defaults confirmados por el propietario
- **G1:** botón propio con la imagen del sombrero (recursos en `Claude outputs/assets/`), no `QuestionCircle`.
- **G2:** panel lateral (C1) en lugar de pantalla completa por `OpenChildForm`.
- **G3:** Ficha solo con `ModoDemostracion == true` **y** SuperAdmin.
- **G4:** archivo de la visita en `%LocalAppData%`, con clave por usuario de la aplicación.
- **G5:** Menú Degustación visible para todos los roles; accesos directos por `PerformClick()` del menú. Solo la Ficha está restringida.
