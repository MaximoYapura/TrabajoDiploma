# Etapa 5 — Informe de ejecución (Sistema de Ayuda «Sombrero del Chef»)

Rama: `etapa5/sistema-ayuda` (desde `beta` 03861f0). **No se hizo push.** Un commit por fase, sin `Co-authored-by`.
Compilación del proyecto GUI: **0 errores** al cerrar cada fase (MSBuild de Visual Studio 18, Debug).

## Cómo se probó
Además de compilar, cada fase se ejecutó con un arnés descartable (fuera del repo) que abre el **`FormMDI_08YS` real** con una sesión en memoria (sin login ni escritura en la base: al salir se cierra la sesión antes de `FormClosing` para no llamar a `Logout()`), lee la base local en **solo lectura** y usa **mouse y teclado reales** (`SetCursorPos`, `mouse_event`, `keybd_event`). Capturas en `Claude outputs/capturas_etapa5/`.
Límite: el equipo está al 100 % y no puedo cambiar la escala de Windows. La app es DPI-unaware (ver `Informe_Spikes_Etapa5.md`), así que a 125/150 % Windows la estira sin cambiar el maquetado. **Las pruebas a 125/150 % quedan para el propietario.**

## Commits
| Fase | Commit | Contenido |
|---|---|---|
| 0 | — | Spikes B1/B2/B4 (descartables). `Informe_Spikes_Etapa5.md` entra con la Fase 1. |
| 1 | `0c0efd7` | `FilaAyuda_790MY`, `TeclaEtiqueta_790MY`, recursos del sombrero, F1 por `ProcessCmdKey`. |
| 2 | `add08d0` | `PanelAyuda_UC_790MY` (`panelAyuda`, hermano de `panel2`), `IrAPantalla`. |
| 3 | `2a15bcf` | `FichaIngredientes_UC_790MY`, `ValorCopiable_790MY`. |
| 4 | `3402297` | `MenuDegustacion_UC_790MY`, `PasoDegustacion_790MY`. |
| 5 | `fd9a9da` | Auditoría (solo informe). |
| 6 | `4bc6a9b` | Sugerencias del Chef: `IAyudaContextual_790MY`, `AyudaSugerencias_790MY`, `GloboAyuda_790MY`; 6 hijos implementan `ObtenerPistas`. |
| 7 | `25896c1` | Visita Guiada: `VisitaGuiada_790MY`, `VisitaOverlay_790MY`, `RegistroVisita_790MY`. |

## Decisiones tomadas
1. **F1 solo por `ProcessCmdKey`, sin `HelpRequested`** (spike B1): Windows envía `WM_HELP` aparte de la tecla y llega a la MDI incluso con un `ConfirmModal` abierto.
2. **Colores de la barra en `TemaFogon_790MY`** (`BarraFondo`, `BarraHover`, `BarraPresionado`, `BarraTinta`): medidos sobre los `IconButton` reales (hover (72,18,18), presionado (189,73,73)). Así la fila es indistinguible del resto y cumple «colores desde TemaFogon».
3. **ToolTip dentro de `FilaAyuda_790MY`**, no en `FormMDI_08YS`: un ToolTip asignado al UserControl no aparece sobre sus hijos; el componente lo asigna a cada hijo al traducir.
4. **`panelAyuda` es un `UserControl` (`PanelAyuda_UC_790MY`)**, no un `Panel` con controles sueltos en `FormMDI_08YS.Designer.cs`: el Designer de la MDI solo agrega una línea de acople; el resto vive en su propio Designer.
5. **Ficha descartada, no oculta**: si no se cumple G3, `Preparar()` quita y hace `Dispose` de la Ficha; no queda pestaña ni se consulta la BLL.
6. **Casillas fijas en el Designer** (5 clientes, 5 empleados, 5 mesas, turno, 2 tarjetas; 6 pasos) en lugar de crear controles en el `.cs` (regla 2). El `.cs` solo llena y oculta.
7. **Clientes, empleados y mesas activos**: de `GetAll()` se toman los primeros 5 con `Activo = true`; en mesas también se excluye `FueraDeServicio` (la mesa 3 de la base local no aparece). No se las llama «libres» (C4).
8. **Pistas del Menú Degustación con textos reales**: rutas y nombres de botones salen de las claves existentes de idiomas.json (`menuClientes`, `RR_btnConfirmar`, `RP_btnEnviar`, `KDS_btn_listo`, etc.), así coinciden con la pantalla en ambos idiomas.
9. **Paso 6, orden invertido respecto del brief**: «primero una tarjeta terminada en 0002 para ver el rechazo; después, una válida». El rechazo no cierra la mesa, pero el cobro aprobado sí la libera: en el orden del brief habría que abrir otra mesa para ver el rechazo. La pista no muestra el número aprobado porque el Menú Degustación lo ven todos los roles y la Ficha solo el SuperAdmin; la pantalla de cobro ya muestra los números de prueba.
10. **Banner de error de la Ficha compacto**: lista las secciones que fallaron y un solo detalle técnico (antes repetía el mismo error SQL tres veces).
11. **Puntos de parada P2 y P3 no se esperaron** porque el propietario autorizó seguir sin pausas mientras estaba ausente. Las técnicas elegidas quedan documentadas acá para su revisión.
12. **Resalte de Sugerencias (Fase 6)**: el marco son 4 `Panel` opacos de 3 px en `TemaFogon.Oro` y el globo es un `UserControl` opaco. Todos están declarados en `FormMDI_08YS.Designer.cs` sin acople (no alteran el orden de Dock) y `AyudaSugerencias_790MY` solo los ubica. Ningún formulario crea controles visuales.
13. **Hijos tocados (P4)**: los 6 formularios de la ruta solo agregan `IAyudaContextual_790MY` a su declaración y un método `ObtenerPistas()` que devuelve datos. Usa `nameof(control)`, así que un control renombrado rompe la compilación en lugar de fallar en silencio. Una pista cuyo control no está visible se omite (p. ej. «Agregar» en Registrar Pedido antes de elegir un producto).
14. **Overlay de la Visita (Fase 7)**: técnica (a) del spike B2. La captura se compone por partes con `DrawToBitmap` (`panel1`, `panelLateral`, `panel2` y el hijo) porque sobre la MDI completa falla. La alternativa `CopyFromScreen` depende de que ninguna ventana tape la app. La capa es un único control opaco que pinta la captura, la oscurece y deja el hueco; no hay transparencia real.
15. **La visita usa su propio globo** (`globoVisita`): compartir el de las Sugerencias hacía que ambos componentes reaccionaran a los mismos botones.
16. **Durante la visita se deshabilitan `panel1`, `panelLateral`, `panelIdioma` y `panel2`** (después de la captura, para que no salgan en gris), así Tab no llega a botones tapados. Se restauran al cerrar, respetando `BloquearNavegacion`.
17. **Cortes que no marcan la visita como vista**: redimensionar, cambiar de idioma o bloquear la navegación. Solo «Terminar» y «Saltar» la marcan, como pide el criterio 7.5.

## Discrepancias entre el brief y el código (P4)
1. **KDS con el panel abierto (riesgo de ancho, Fase 2.5).** Con la ventana en su tamaño normal (1350 px) y el panel de 360 px, el KDS queda en 742 px y **la columna del título se parte letra a letra** (`capturas_etapa5/f2_Cocina.png`). A 320 px queda apretado pero legible (`kds_320.png`). Maximizada a 1920 px se ve bien. Registrar Reserva, Caja y cobro y Mesas se ven bien a 742 px. **No modifiqué el KDS ni el ancho.** Opciones: (a) bajar el panel a 320 px; (b) que el panel se superponga en lugar de reducir cuando el hijo activo es el KDS; (c) dejarlo así (la cocina suele usar la ventana maximizada).
2. **DPI**: el brief pide elegir el sombrero de 32/40/48/64 px según la escala. Está implementado por `DeviceDpi`, pero como la app es DPI-unaware hoy siempre elige 32 px y Windows la estira.
3. **Regla de DNI de 7 u 8 dígitos**: está en la GUI (`FormClientes_790MY`, `FormAbrirMesa_790MY`, reportes), no en `ClienteBLL_790MY` (que solo exige DNI > 0). La pista la menciona porque es lo que valida la pantalla.
4. **`EstiloRF2_790MY.MostrarErrorEnBanner`** usa `MessageBox` para errores inesperados; por eso la Ficha no lo reutiliza y arma su propio banner.

## Fase 5 — Auditoría (solo informe, nada corregido)

### 5.1 F1 con la implementación real
| Caso | Resultado |
|---|---|
| Foco en el formulario hijo (sin control activo) | Abre el panel. |
| Foco en un TextBox, idioma inglés | Abre el panel («Chef's Help»). |
| `ConfirmModal_790MY` abierto + F1 | **No** abre el panel, ni durante ni después del modal. |
| Combo de idioma desplegado + F1 | No abre el panel (el desplegable es una ventana emergente aparte). Comportamiento aceptable. |
| Navegación bloqueada (cobro) + F1 | Consumida, no abre (Fase 2). |

### 5.2 Ciclo de vida de los hijos (`OpenChildForm` no hace `Dispose`)
Medido navegando con `IrAPantalla` (el mismo `OpenChildForm`):
- **KDS reemplazado**: `IsDisposed = false`, `Parent = null`, **`timerKds_790MY.Enabled = true`**: sigue consultando la base cada 15 s sin estar en pantalla.
- **Abrir Mesa reemplazado**: `timerReloj_790MY` (30 s) sigue activo.
- **Registrar Pedido reemplazado**: `timerAvisos_790MY` (20 s) sigue activo.
- **Suscripciones de idioma**: los hijos se suscriben a `TraductorManager_08YS` y se desuscriben en `FormClosed`, que nunca ocurre porque `OpenChildForm` solo los quita de `panel2`. **+1 observador por navegación** (2 → 11 tras 9 navegaciones); cada cambio de idioma ejecuta `UpdateIdioma` sobre todas las pantallas viejas (ida y vuelta con 11 observadores: 257 ms). Además, ningún formulario reemplazado libera su memoria.
- Arreglo sugerido (requiere aprobación, toca `OpenChildForm`): antes de `panel2.Controls.Clear()`, cerrar el hijo anterior (`(panel2.Tag as Form)?.Close()`, que dispara `FormClosed` → desuscripción y detención de timers) y luego `Dispose()`. Hay que probarlo con `FormCambiarContraseña_08YS`, que usa `FormClosed` para cerrar la sesión.

### 5.3 Portapapeles
Forzado con `OpenClipboard` desde otra ventana: `Clipboard.SetText` lanza `ExternalException`; la Ficha muestra el banner ámbar «No se pudo usar el portapapeles…» y deja el valor seleccionado para Ctrl+C. Sin `MessageBox`.

## Fase 8 (opcional): no realizada
- **8a Manual de Recetas**: el brief pide que el contenido de los PDF lo redacte el propietario a partir de las ECU y prohíbe PDF de relleno. Sin los PDF, un acceso «Manual» solo mostraría «no encontrado», así que no agregué ni el acceso ni la línea del instalador. Cuando existan `docs/Manual/Receta_*.pdf`, faltan: `AbrirManual(receta)` (con `File.Exists` y `Process.Start` con `UseShellExecute`, en `try/catch` y aviso en banner), el botón en cada paso del Menú Degustación y la línea `Source: "docs\Manual\*.pdf"; DestDir: "{app}\Manual"` en `Installer_TP_Fogon.iss`.
- **8b Animación del sombrero**: no es necesaria para el entregable. El spike B4 la deja viable (≈ 1,6 % de un núcleo) si se quiere más adelante.

## Verificación final
- Debug y Release: **0 errores**; sin warnings nuevos en los archivos de la etapa.
- Regresión con la build final: fila (hover, clics, bloqueo, inglés), Menú Degustación (6 «Ir») y Sugerencias (6 pantallas) siguen pasando. Con el panel ya funcionando, Enter en la fila abre el panel y el foco pasa a sus pestañas: es lo esperado.
- Archivos de prueba en `%LocalAppData%\El Fogon del Sur` borrados (la carpeta no existía antes).
- Rama `etapa5/sistema-ayuda` con 7 commits sobre `beta`; **sin push**. `Claude outputs/assets/` queda sin versionar: las copias usadas están en `GUI/Resources/`.

## Pendiente del propietario
1. Ver la fila y el panel en la app real (P2) y aprobar las técnicas de resalte y overlay (P3).
2. Decidir el ancho del panel con el KDS (discrepancia 1).
3. Decidir si se corrige `OpenChildForm` (5.2): timers y suscripciones vivas.
4. Pruebas a 125/150 % y con usuarios reales de cada rol (checklist).
5. Fusionar en `beta` y hacer push cuando lo apruebe.
