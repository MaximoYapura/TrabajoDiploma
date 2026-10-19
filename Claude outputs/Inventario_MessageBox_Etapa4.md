# Inventario de `MessageBox` — Etapa 4 (Caja y cobro)

Rama: `etapa4/facturacion-y-cobro` (base `beta`, commit `30038dd`). Fecha: 2026-10-19.
Mismas clases que `Inventario_MessageBox_Etapa2.md`: **V** validación · **C** confirmación · **X** éxito · **R** error recuperable · **F** error fatal.

## Resumen

| Formulario | Directas | Vía `E.MostrarError` / `E.MostrarExito` | Total | Quedan |
|---|---:|---:|---:|---:|
| `FormFacturacionCobro_790MY` | 2 | 6 | 8 | 0 legacy (solo el F de `MostrarErrorEnBanner`) |
| `FormComprobanteFactura_790MY` | 0 | 1 | 1 | 0 |
| `FormMDI_08YS` (fuera de alcance) | 3 | 0 | 3 | 3, sin cambios |

`FormComprobanteFactura_790MY` no tiene `MessageBox` directos (confirmado, §3.4), pero sí un `E.MostrarError` legacy en la impresión: se migró.

## Detalle y destino

### FormFacturacionCobro_790MY (líneas del commit `30038dd`)

| # | Línea | Clase | Antes | Ahora |
|---|---|---|---|---|
| 1 | 106 `CargarMesas` | R/F | `E.MostrarError` (no se pudieron leer las mesas) | `E.MostrarErrorEnBanner`: regla o permiso al banner; falla inesperada sigue en el `MessageBox` de error fatal (E2-D1) |
| 2 | 214 `SeleccionarMesa` | R/F | `E.MostrarError` | `E.MostrarErrorEnBanner` |
| 3 | 353 `btnPreTicket` | X | `E.MostrarExito` «Factura N.º … generada» | Toast después de cerrar la vista del pre-ticket |
| 4 | 361 `btnPreTicket` | R/F | `E.MostrarError` | `E.MostrarErrorEnBanner` |
| 5 | 520 `btnRegistrarCobro` | C | `MessageBox` Sí/No «¿Confirma el cobro…?» | `ConfirmModal_790MY`: «¿Confirmar el cobro de $X en [medio]?», título «Cobrar mesa N», foco inicial en Cancelar |
| 6 | 548 `btnRegistrarCobro` | X | `E.MostrarExito` «Cobro registrado…» | Toast al volver del comprobante |
| 7 | 559 `btnRegistrarCobro` | R | `MessageBox` de advertencia «Pago rechazado» | Banner de error con el mensaje de `PagoRechazadoException_790MY`, ícono de `ErrorProvider` en `txtNroTarjeta_790MY`, foco y `SelectAll` |
| 8 | 567 `btnRegistrarCobro` | R/F | `E.MostrarError` | `E.MostrarErrorEnBanner` y recarga de mesas (como antes) |

Además: `ObtenerFacturaParaCobrar` lanzaba `ArgumentException(msg_fc_factura_requerida)` (caía en el #8). Ahora se valida antes de confirmar y se muestra en el banner, sin abrir el modal.

### FormComprobanteFactura_790MY

| # | Línea | Clase | Antes | Ahora |
|---|---|---|---|---|
| 1 | 140 `btnImprimir` | R | `E.MostrarError` (cualquier excepción caía como error inesperado) | Banner en el comprobante: «No se pudo preparar la impresión (…). El comprobante ya está registrado…». Se puede reintentar sin cerrar. |

### Nuevos avisos (sin equivalente anterior)

- **Cobro en curso** (banner ámbar, sin ✕): al intentar cerrar el formulario o la aplicación durante el cobro.
- **Validación en línea con `ErrorProvider`** (mismas reglas, no se agregaron): descuento inválido, propina inválida, efectivo insuficiente, tarjeta que no pasa Luhn y tarjeta rechazada.

### Lo que se conserva

- El `MessageBox` nativo de error fatal dentro de `E.MostrarErrorEnBanner` (excepción inesperada, base de datos caída), por E2-D1.
- `FormMDI_08YS`: sesión invalidada y las dos confirmaciones de cerrar sesión siguen con `MessageBox` (fuera de alcance de esta etapa). Durante un cobro, cerrar sesión no está disponible porque el menú lateral queda bloqueado.
