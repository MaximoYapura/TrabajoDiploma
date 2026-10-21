# Etapa 5 — Informe de spikes (Fase 0)

Fecha: 2026-10-20. Rama: `etapa5/sistema-ayuda` (desde `beta`, 03861f0).
Proyecto de prueba descartable (fuera del repo, no se commitea): WinForms .NET 4.8 que referencia `GUI_08YS.exe` y reproduce `FormMDI_08YS` (`IsMdiContainer = true`, `FormBorderStyle.None`, barra lateral y `panel2` con `Dock = Fill`) con la misma lógica de `OpenChildForm`. Se usaron controles reales del proyecto: `FormCocinaKDS_790MY`, `ConfirmModal_790MY`, `SlidingTab_UC_790MY`, `MesaCard_790MY`, `IconComboBox`.
Entrada de teclado **real** (`keybd_event` con la ventana en primer plano), no mensajes simulados. Equipo: 12 núcleos, pantalla al 100 % (96 dpi).

## Resultado del gate
**F1 llega a `FormMDI_08YS.ProcessCmdKey` en todos los casos con el foco dentro de un hijo embebido.** Se avanza a la Fase 1.

## B1 — F1 con el foco en un hijo embebido

| Caso | `ProcessCmdKey` de la MDI | `HelpRequested` de la MDI |
|---|---|---|
| Foco en la barra lateral | Sí (1) | Sí (1) |
| Hijo: `TextBox` | Sí (1) | Sí (1) |
| Hijo: `DataGridView` (celda seleccionada) | Sí (1) | Sí (1) |
| Hijo: `DataGridView` en edición (`DataGridViewTextBoxEditingControl`) | Sí (1) | Sí (1) |
| Hijo: `ComboBox` DropDownList **desplegado** | Sí (1), el combo sigue desplegado | Sí (1) |
| Hijo: `ComboBox` editable desplegado | Sí (1) | Sí (1) |
| Hijo real: `FormCocinaKDS_790MY` (foco en el formulario) | Sí (1) | Sí (1) |
| Hijo real: KDS, foco en `btnActualizar_790MY` | Sí (1) | Sí (1) |
| **`ConfirmModal_790MY` abierto** (`ShowDialog` con la MDI como dueña) | **No (0)** — correcto | **Sí (1)** — ver hallazgo |
| `IconComboBox` (idioma) con su desplegable abierto | No medido (ver nota) | — |

**Hallazgo importante (no previsto en el brief):** `HelpRequested` se dispara en la MDI **aunque `ProcessCmdKey` devuelva `true`**, y también **con `ConfirmModal` abierto**. Motivo: Windows envía F1 por dos caminos independientes: `WM_KEYDOWN` (lo que ve `ProcessCmdKey`) y `WM_KEYF1` → `WM_HELP`, que sube por los padres y, en una ventana de nivel superior, llega a su **dueña** (la MDI). Consumir la tecla no evita el `WM_HELP`.

**Decisión para la Fase 1:** usar **solo `ProcessCmdKey`** (como en el brief) y **no suscribir `HelpRequested`** en la MDI: si se usara, F1 actuaría dos veces y además se abriría la ayuda por detrás de un `ConfirmModal`. Ningún control del proyecto usa hoy `HelpRequested` (verificado con búsqueda), así que el `WM_HELP` sobrante no tiene efecto.

Nota `IconComboBox`: el spike no pudo abrir el desplegable por reflexión (excepción de desbordamiento en `ResizePopup` fuera de su contexto real). Con el desplegable abierto el foco está en un `ListBox` dentro de un `ToolStripDropDown` (ventana emergente aparte), así que lo esperable es que F1 no haga nada hasta cerrarlo. Es un caso menor (solo el combo de idioma de la barra superior lo usa entre las pantallas de la ruta) y queda para la auditoría F1 de la Fase 5 con la aplicación real.

Otros controles con manejo propio de teclas: solo `FormBitacoraCambios_790MY` (`KeyPreview = true` y `ProcessCmdKey` para Alt+R). Llama a `base.ProcessCmdKey` para el resto, así que F1 sigue subiendo.

## B2 — `Control.DrawToBitmap`

| Objeto | Resultado |
|---|---|
| Hijo con `Dock = Fill` (con `SlidingTab_UC_790MY`, 3 `MesaCard_790MY` y un `DataGridView`) | **Idéntico** a la captura real de pantalla de la misma zona (comparación visual de `b2_hijo_drawtobitmap.png` vs `b2_hijo_pantalla.png`). 30 ms. |
| `SlidingTab_UC_790MY` solo | Correcto (pestaña activa, textos y bordes). |
| `MesaCard_790MY` sola | Correcta (bordes redondeados, chip, puntos de capacidad). |
| **Formulario principal completo** (`IsMdiContainer = true`) | **Incorrecto**: el área de `panel2` sale gris (se dibuja el `MdiClient` que crea `IsMdiContainer`, no el contenido de `panel2`). 17 ms. |

**Conclusión para la Fase 7 (overlay):** no usar `DrawToBitmap` sobre `FormMDI_08YS` entero. Si se usa la técnica (a), componer la captura por partes (`panelLateral`, `panel1` y el hijo activo de `panel2`, cada uno con su `DrawToBitmap`) o usar `Graphics.CopyFromScreen` sobre los límites de la ventana (válido porque la ventana está en primer plano cuando arranca la visita).

## B4 — Timer de 16 ms limitado a un glifo de 40×40

Glifo con el sombrero de 32 px rotando ±10° con `HighQualityBicubic`, `Invalidate()` solo del control de 40×40, durante 5 s:
- CPU en reposo: 0,00 %. CPU animando: **0,13 % del total (≈ 1,6 % de un núcleo)**.
- Ticks reales: 177 en 5 s (**≈ 35 fps**; el `System.Windows.Forms.Timer` no baja de ~15,6 ms de resolución y además se acumula el pintado).
- `SystemInformation.UIEffectsEnabled = true` en este equipo.

**Conclusión:** costo despreciable; la Fase 8b es viable si se limita a ≤ 150 ms y se detiene el timer al asentarse.

## Otros hallazgos del spike (para la Fase 5, sin modificar nada)
1. **Ciclo de vida de los hijos (confirmado con evidencia):** tras `panel2.Controls.Clear()` (lo que hace `OpenChildForm`), el `FormCocinaKDS_790MY` anterior queda con `IsDisposed = false` y `timerKds_790MY.Enabled = true`: el timer de 15 s sigue consultando la base aunque el KDS ya no esté en pantalla. No se modifica `OpenChildForm` ni el KDS (regla 4); queda para el informe.
2. **Escala de pantalla:** el proyecto GUI tiene `NoWin32Manifest = true` y no declara conciencia de DPI (ni manifiesto ni `App.config`), así que es **DPI-unaware**: a 125 % y 150 % Windows estira la ventana como mapa de bits y `DeviceDpi` vale siempre 96. Consecuencias: (a) el maquetado en píxeles lógicos es idéntico a 100/125/150 %; (b) la elección del recurso del sombrero por escala (32/40/48/64 px) se implementa por `DeviceDpi` como pide el brief, pero hoy siempre elegirá 32 px; (c) este equipo está al 100 % y no puedo cambiar la escala de Windows (es una configuración del sistema): las pruebas a 125/150 % quedan para el propietario.
