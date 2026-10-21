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
