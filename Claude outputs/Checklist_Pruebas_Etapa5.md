# Checklist de pruebas — Etapa 5 (Sistema de Ayuda «Sombrero del Chef»)

Rama: `etapa5/sistema-ayuda` (desde `beta`). Se actualiza al cerrar cada fase.

Claude Code verifica compilación (0 errores) y, cuando es posible, ejecuta pruebas automatizadas fuera de la app. Las casillas sin marcar son para quien prueba la aplicación real.
Marcá cada ítem con [x]. Si algo falla, anotá acción, escala, idioma y pantalla.

## 0. Antes de empezar

- [ ] Repetir los casos clave a **100 %, 125 % y 150 %** de escala de Windows (la app es DPI-unaware: Windows la estira; ver `Informe_Spikes_Etapa5.md`).
- [ ] Repetir en **español e inglés**. Ningún texto aparece como clave cruda (ej. `[AY_titulo]`).
- [ ] «Reducir animaciones»: Windows → Accesibilidad → Efectos visuales → desactivar «Efectos de animación».

## Fase 0 — Spikes (hecho por Claude Code, ver `Informe_Spikes_Etapa5.md`)

- [x] B1: F1 llega a `ProcessCmdKey` de la MDI con foco en TextBox, DataGridView (celda y edición), ComboBox desplegado y `FormCocinaKDS_790MY`.
- [x] B1: con `ConfirmModal_790MY` abierto, `ProcessCmdKey` de la MDI **no** recibe F1.
- [x] B1: `HelpRequested` llega siempre (incluso bajo el modal) → no se usa.
- [x] B2: `DrawToBitmap` reproduce bien un hijo `Dock = Fill`, `SlidingTab_UC_790MY` y `MesaCard_790MY`; **no** el formulario MDI completo.
- [x] B4: timer de 16 ms sobre un glifo de 40×40 ≈ 1,6 % de un núcleo.

## Fase 1 — Fila «Ayuda [F1]» (`FilaAyuda_790MY`)

Verificado por Claude Code con el `FormMDI_08YS` real (sesión en memoria, sin base), mouse y teclado reales, al 100 %:
- [x] La fila aparece sobre «Cerrar Sesión», 49 px, borde dorado de 1 px, mismo fondo que las demás filas; sombrero sin recuadro de color.
- [x] Texto alineado con el resto de las filas (x = 44 px).
- [x] Hover (72,18,18) igual al de los IconButton; **sin parpadeo** al pasar por imagen, texto, hueco, tecla y borde.
- [x] Presionado (189,73,73) igual al de los IconButton; al soltar vuelve a hover.
- [x] Clic en imagen, texto, tecla, hueco y borde: 5 de 5 disparan `Click`.
- [x] Shift+Tab desde «Cerrar Sesión» llega a la fila; recuadro de foco dorado visible; Enter y Espacio: 2 de 2.
- [x] Con `BloquearNavegacion(true)`: imagen, texto y tecla en gris (`TextoDeshabilitado`); el clic no hace nada.
- [x] Inglés: «Help», nombre accesible «Help (F1)»; vuelve a «Ayuda» en español.

Para el propietario:
- [ ] Ver la fila en la app real (login normal) y aprobar el diseño (punto P2).
- [ ] ToolTip «Ayuda del Chef (F1)» al dejar el mouse sobre cualquier parte de la fila.
- [ ] Repetir a 125 % y 150 %.
- [ ] Narrador / lector de pantalla: anuncia «Ayuda (F1), botón».
- [ ] Durante un cobro real (Caja y cobro): la fila se ve gris y F1 no hace nada.

## Fase 2 — Panel de ayuda lateral (`PanelAyuda_UC_790MY`, `panelAyuda`)

Verificado por Claude Code (FormMDI real, base local en solo lectura, ventana de 1350×784 y maximizada a 1920×1080):
- [x] `panelAyuda` es hermano de `panel2`, `Dock = Right`, 360 px, oculto al iniciar.
- [x] F1 con foco en un TextBox de Clientes: abre el panel y el foco pasa a las pestañas. Esc: lo cierra y el foco **vuelve al TextBox**.
- [x] Clic en la fila: abre; F1 de nuevo: cierra (alterna).
- [x] Navegar a Registrar Reserva, Caja y cobro, Cocina y Mesas con el panel abierto: el panel **sigue abierto**; el hijo ocupa 742 px.
- [x] `BloquearNavegacion(true)`: el panel se oculta y F1 no lo reabre.
- [x] Registrar Reserva, Caja y cobro y Maestro de Mesas se ven bien a 742 px.
- [ ] **KDS a 742 px: la columna del título se parte** («Co / ma / nd / as…»). A 320 px de panel queda apretado pero legible. **Decisión pendiente del propietario** (ver informe).

Para el propietario:
- [ ] Copiar un dato de la Ficha y pegarlo en el formulario de al lado con el panel abierto.
- [ ] Repetir a 125 % y 150 %.

## Fase 3 — Ficha de Ingredientes (`FichaIngredientes_UC_790MY`, `ValorCopiable_790MY`)

Verificado por Claude Code con la base local (solo lectura):
- [x] Datos **en vivo** (nada escrito a mano): cliente 30000000; empleados 30000001, 22222222, 30000002, 12345678 con su puesto traducido; mesas 1, 2, 4, 5, 6 (la 3 está fuera de servicio y no se muestra); turno de hoy 22:00.
- [x] Tarjetas: aprobada `4111 1111 1111 1111` (VISA) y rechazo simulado `4000 0000 0000 0002`, comprobadas en ejecución con `PagoBLL_790MY.ValidarTarjetaLuhn` y `TerminacionRechazoSimulado`; cuotas 1, 3, 6, 12 leídas de `Pago_790MY.CuotasPermitidas`. Sin contraseñas.
- [x] «Copiar» el DNI del cliente → toast «Copiado: 30000000» → pegado con Ctrl+V en el DNI de Registrar Reserva.
- [x] Portapapeles tomado por otra ventana: banner ámbar dentro del panel y valor seleccionado para Ctrl+C (sin `MessageBox`).
- [x] Base inaccesible: banner rojo «No se pudieron leer: Clientes, Empleados, Mesas. Detalle: …» y «No se pudo leer esta sección» en cada una (no queda un panel en blanco).
- [x] **G3**: rol Camarero con Modo Demostración → la Ficha no existe (solo Menú Degustación, sin pestañas). SuperAdmin con `ModoDemostracion=false` → igual.

Para el propietario:
- [ ] Base vacía (sin clientes/empleados/mesas): «Todavía no hay datos cargados.» en cada sección.
- [ ] Cambiar a inglés con la Ficha abierta: títulos, puestos y «Copy» traducidos.

## Fase 4 — Menú Degustación (`MenuDegustacion_UC_790MY`, `PasoDegustacion_790MY`)

Verificado por Claude Code:
- [x] Seis pasos con el flujo real: Cliente → Reserva → Abrir mesa → Registrar pedido → Cocina → Caja y cobro. Cada pista muestra la ruta del menú y los textos de los botones **leídos de idiomas.json** («Confirmar Reserva», «Confirmar ingreso», «Agregar», «Enviar a cocina», «Iniciar preparación», «Marcar como listo»).
- [x] Paso 2 muestra el turno vigente en vivo (22:00); paso 6 la terminación de rechazo leída de `PagoBLL_790MY`.
- [x] SuperAdmin: los 6 «Ir» abren Clientes, Registrar Reserva, Abrir Mesa, Registrar Pedido, KDS y Caja y cobro (`PerformClick` del menú); **el panel queda abierto**.
- [x] Rol con solo AbrirMesa y RegistrarPedido: pasos 1, 2, 5 y 6 con «Ir» gris y «Tu rol no tiene permiso para esta pantalla.»; 3 y 4 funcionan.
- [x] «Hecho»: título y borde en verde; se mantiene al cambiar de idioma; no se guarda.
- [x] Inglés: «Reservations › New Reservation … press «Confirm Reservation»».

Para el propietario:
- [ ] Hacer la ruta completa de punta a punta con un usuario real de cada rol.

## Fase 5 — Auditoría (ver `Informe_Etapa5.md`, nada corregido)

- [x] F1 con foco en el hijo, en inglés, con `ConfirmModal` abierto (no actúa) y con el combo de idioma desplegado (no actúa).
- [x] Hijos reemplazados siguen vivos: timers de KDS (15 s), Abrir Mesa (30 s) y Registrar Pedido (20 s) activos; +1 observador de idioma por navegación. **Pendiente de decisión del propietario** (requiere tocar `OpenChildForm`).
- [x] Portapapeles forzado a fallar: banner y valor seleccionado.

## Fase 6 — Sugerencias del Chef (`IAyudaContextual_790MY`, `AyudaSugerencias_790MY`, `GloboAyuda_790MY`)

Verificado por Claude Code (FormMDI real, teclado real):
- [x] F1 en Clientes (3 pistas), Registrar Reserva (4), Abrir Mesa (4), Registrar Pedido (3 visibles de 4: «Agregar» aparece recién al elegir un producto y se omite), Cocina (4) y Caja y cobro (5): marco dorado de 4 paneles alrededor del control y globo al lado, **sin tapar el control** y dentro de la ventana.
- [x] → avanza, ← retrocede, Esc cierra; el foco vuelve al control que lo tenía.
- [x] F1 con sugerencias activas las cierra; sin panel ni sugerencias, F1 en una pantalla sin `IAyudaContextual` (Mesas) abre el panel.
- [x] Se cierran solas al navegar, al cambiar de idioma, al maximizar/restaurar y con `BloquearNavegacion(true)`. Clic en la fila de Ayuda: cierra las sugerencias y abre el panel.
- [x] Inglés: «Find a customer · Search by DNI, surname, name, email or mobile · 1 of 3».

Para el propietario:
- [ ] Recorrer las sugerencias con datos reales (mesas abiertas, pedidos en cocina) para ver el globo con más contenido en pantalla.

## Fase 7 — Visita Guiada (`VisitaGuiada_790MY`, `VisitaOverlay_790MY`, `RegistroVisita_790MY`)

Verificado por Claude Code (los archivos de prueba en `%LocalAppData%\El Fogon del Sur` se borraron al terminar):
- [x] Primer ingreso de un usuario sin la marca: la visita arranca sola. Captura compuesta por partes con `DrawToBitmap`, atenuada, con un hueco nítido sobre cada elemento.
- [x] Pasos según lo que el usuario ve (SuperAdmin: 10). Bienvenida (nombre y rol), cada botón visible de la barra con **las opciones disponibles de su menú leídas en vivo**, Ayuda (menciona la Ficha solo si corresponde), Idioma y Cerrar Sesión.
- [x] Durante la visita: Tab no sale del globo; F1 no hace nada; clics absorbidos por la capa.
- [x] «Terminar» y «Saltar visita» (o Esc) la marcan como vista: `ayuda.json` = `{"visitasVistas": ["<usuario>"]}`, sin duplicados.
- [x] «Repetir visita» en el encabezado del panel de ayuda la vuelve a mostrar.
- [x] Maximizar/restaurar durante la visita la corta **sin** marcarla; la barra vuelve a habilitarse.
- [x] `ayuda.json` dañado → «no vista» y se reescribe al marcar. Ruta ocupada por una carpeta → sin excepción, «no vista».
- [x] Inglés: «Welcome! … You can skip it and replay it from Help.»

Para el propietario:
- [ ] Ingresar con un usuario de cada rol: la visita muestra solo sus botones.
- [ ] Usuario de Windows sin permisos de administrador (app instalada en Program Files): la marca se guarda en `%LocalAppData%`.
- [ ] Reducir animaciones: la visita no tiene animaciones (no aplica).

## Ajustes post-revisión (2026-10-21)

Verificado por Claude Code. El arnés ya **no mueve el mouse ni el teclado reales**: usa `PerformClick` y mensajes a la ventana, y captura con `DrawToBitmap`.
- [x] **1. Submenú del Chef**: la fila despliega «Sugerencias de esta pantalla [F1]», «Visita Guiada», «Manual de Recetas (PDF)» (solo SuperAdmin) y «Ficha de Ingredientes [Modo Evaluación]» (para otros roles, «Menú Degustación»). Cada opción hace lo suyo.
- [x] **2. Sugerencias en todas las pantallas**: F1 muestra el globo en las 19 pantallas del menú más el alta de rol/familia (74 pistas; claves verificadas en es y en).
- [x] **3. Manual**: botón «Receta» en cada paso del Menú Degustación con su PDF (los 6 existen en `bin\Manual`). Sin la carpeta: banner «No se encontró el Manual de Recetas en …». El instalador copia `docs\Manual\*.pdf` a `{app}\Manual`.
- [x] **4. Visita, paso 1**: el hueco y su borde quedan dentro de la barra lateral.
- [x] **5. Cambiar Contraseña**: lo escrito se ve en tinta oscura (antes blanco sobre fondo claro) y enmascarado.
- [x] **6. OpenChildForm**: el KDS anterior queda descartado con su timer detenido; los observadores de idioma se mantienen en 2 tras 12 navegaciones. Salir de Cambiar Contraseña no cierra la sesión. Roles: grilla → alta → Cancelar vuelve a la **misma** grilla, y el alta se libera.
- [x] **7. Panel a 320 px**: Ficha (Copiar como ícono con ToolTip) y Menú Degustación sin recortes.
- [x] **8. Animación**: inclinación de 0° a 10° en ~150 ms, latido y brillo en 300 ms; el timer se detiene solo; se corta al bloquear la navegación; 0,3 % de un núcleo.

Para el propietario:
- [ ] Abrir una receta y la carpeta del Manual desde la app real (el arnés no lanzó el visor de PDF para no abrir ventanas en tu escritorio).
- [ ] Ver la animación con el mouse real y con «Efectos de animación» desactivado en Windows (no debe moverse).
- [ ] Cambiar la contraseña de punta a punta: al confirmarla se cierra la sesión.
