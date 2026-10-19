# Etapa 3 — KDS: decisiones, mediciones y resultado

Fuente: documento «Etapa 3 — KDS: problema, decisiones e hipótesis» y respuestas del propietario (2026-10-19).
Formulario: `GUI/RF2/FormCocinaKDS_790MY`. Rama: `etapa3/kds-opcion-c`.

## Decisiones (vigentes)
- **E3-D1 Alcance:** primero la Opción B (no rearmar si los datos no cambiaron). Se midió y no alcanzó en los ticks con cambio de estado (117 ms con 24 tickets).
- **E3-D6 Opción C: APROBADA (2026-10-19)** por superar los 50 ms. Lineamientos: sin `Controls.Clear()` masivo; mapa NroPedido → control; altas crean solo su ticket, bajas destruyen solo el suyo; un cambio de columna mueve el control existente (`Parent = nuevoPanel`).
- **E3-D2 Halo animado:** descartado. Alcanzan borde rojo, ícono y etiqueta.
- **E3-D3 Tiempo de espera:** minutos enteros. Sin temporizador de UI de 1 s.
- **E3-D4 Consulta sincrónica:** queda como está; pasa a la Etapa 4 (hilos).
- **E3-D5 Criterio de éxito:** redibujado < 50 ms.

## Hipótesis (respuestas)
- **H1/H2 Parpadeo y clics perdidos:** se asume que existen en hardware de gama baja; se mitigan no destruyendo controles.
- **H3 Hora pico:** 20–25 tickets simultáneos (12 mesas). Caso fuerte de prueba: 24.
- **H4 Pantallas:** una sola pantalla de KDS en la cocina.
- **H5 Latencia:** 15 s es aceptable (no cambia el intervalo del temporizador).
- **H6 Hardware:** All-in-One táctiles de gama baja/media o PCs viejas. Las mediciones de abajo son de la PC de desarrollo y son un piso: en el equipo real serán mayores.

## Qué cambió en el código (Opción C)
- **Nuevo `GUI/RF2/TicketKds_790MY.cs`**: el ticket es un control propio (deriva de `PanelRedondeado_790MY`, como las demás tarjetas de RF2; sin Designer porque se arma por código, igual que `PanelRedondeado`). Contiene también el semáforo (antes en el formulario) y `FuentesKds_790MY`, las fuentes compartidas.
  - `Aplicar(pedido, ancho)` compara la «estructura» del ticket (mesa, camarero, hora, ítems, observaciones, ancho, y si es Listo o no):
    - Igual y Pendiente ↔ En preparación: solo cambia texto, ícono y color del botón y el color de las cantidades.
    - Distinta (por ejemplo, pasa a Listo, que no tiene botón y sí barra de finalizado): rearma solo los hijos de ese ticket; el ticket en sí no se destruye.
    - Siempre: `ActualizarTiempo`, que toca el chip de minutos y el semáforo solo si cambió el minuto.
  - Botón «Iniciar» / «Marcar listo»: se deshabilita al tocarlo y la acción se ejecuta con `BeginInvoke` (no se dispone el botón dentro de su propio `Click`; un doble toque no la dispara dos veces). Si la acción falla, se vuelve a habilitar.
- **`FormCocinaKDS_790MY.cs`**: `_tickets` (NroPedido → ticket). `Renderizar` sincroniza las tres columnas:
  - Alta: crea solo el ticket del pedido nuevo.
  - Cambio de columna: `ticket.Parent = flp` (mismo control) y `SetChildIndex` para respetar el orden de la consulta.
  - Baja: `Dispose` solo de los tickets cuyo pedido ya no vino en la consulta.
  - El aviso «sin comandas» de cada columna es un único `Label` que se muestra u oculta.
  - Cambio de idioma: invalida cada ticket para que se rearme con los textos nuevos.
- `Recargar` mide con `Stopwatch` la consulta y el redibujado y lo escribe con `Trace.WriteLine` (categoría `KDS`). En el equipo de la cocina se puede ver con DebugView, sin depurador.
- Sin cambios en BE, BLL, DAL, Designer ni `idiomas.json`. Se agrega `TicketKds_790MY.cs` al `.csproj`.

## Mediciones (Stopwatch)
Arnés fuera del repositorio: abre el formulario real (sin la consulta a la base), inyecta tickets sintéticos (40 % Pendientes, 30 % En preparación, 30 % Listas; 2 a 4 ítems; 1 de cada 3 con observación), ventana 1366×768, Release, 40 iteraciones. Se mide `Renderizar` más el pintado pendiente (`Application.DoEvents`). PC de desarrollo (Ryzen 5, 12 núcleos lógicos), 2026-10-19.

Mediana (p95), en ms:

| Tickets | Original: cualquier tick | B: sin cambios | B: +1 minuto | B: cambio de estado | **C: sin cambios** | **C: +1 minuto** | **C: cambio de estado** |
|---|---|---|---|---|---|---|---|
| 12 | 84 (89) | 0,0 (0,0) | 4,8 (7,0) | 59 (64) | 0,0 (0,1) | 4,8 (5,6) | **14 (37)** |
| 24 | 169 (185) | 0,0 (0,0) | 5,5 (5,8) | 117 (163) | 0,0 (0,0) | 5,4 (6,4) | **11 (23)** |
| 40 | 260 (312) | 0,0 (0,1) | 7,8 (9,1) | 168 (228) | 0,0 (0,1) | 7,6 (8,1) | **11 (22)** |

Opción C con 24 tickets, por tipo de cambio (mediana / máx.): Pendiente → En preparación 5,1 / 22,5 ms (solo se mueve y se repinta); En preparación → Listo 16,8 / 24,7 ms (se rearma un ticket); Listo → En preparación 13,5 / 20,8 ms.

- «Sin cambios»: la consulta devuelve los mismos pedidos. «+1 minuto»: además, todos los chips de tiempo cambian de texto. «Cambio de estado»: un pedido pasa a otra columna.
- Con 12 tickets el máximo es algo mayor porque las columnas tienen poco contenido y el cambio de barra de desplazamiento redimensiona más; sigue debajo de 50 ms.
- El arnés verificó en cada tick con cambio (120 casos) que cada columna muestra exactamente sus pedidos, en el orden de la consulta, con el botón correcto y el aviso de vacío correcto.
- La consulta (`ObtenerComandasCocina` + `ObtenerListosDelDia`) no está en estas cifras: se mide con la traza de `Recargar` en la app real.
- La carga inicial (todas altas) cuesta lo mismo que el código original: ocurre una vez al abrir el KDS.

## Conclusión
- Con la Opción C todos los ticks medidos quedan debajo de 50 ms en la PC de desarrollo, con margen (p95 ≤ 37 ms; 23 ms con 24 tickets).
- Ningún tick destruye los controles de los tickets que no cambiaron, así que en ellos no hay parpadeo ni clics perdidos.

## Pendiente de verificar
- Prueba en la app con base real: 100/125/150 % y es/en; «Iniciar» y «Marcar listo» (incluido doble toque); cambio de idioma con el KDS abierto; redimensionar la ventana.
- Traza `KDS` en un equipo como el de la cocina (H6).
