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
