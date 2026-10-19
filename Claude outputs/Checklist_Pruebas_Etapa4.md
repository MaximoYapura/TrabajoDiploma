# Checklist de pruebas — Etapa 4 (Caja y cobro)

Rama: `etapa4/facturacion-y-cobro`. Compila con 0 errores. **Nada de esto se probó todavía en la app real.**
Marcá cada ítem con [x]. Si algo falla, anotá acción, escala, idioma y la salida de la categoría `Cobro`.

## 0. Antes de empezar

- [ ] Abrir la salida de `Trace` (ventana Output de Visual Studio en Debug, o DebugView) y filtrar por `Cobro:`.
- [ ] Repetir los casos clave a **100 %, 125 % y 150 %** de escala de Windows.
- [ ] Repetir en **español e inglés**. Cambiar el idioma con la pantalla abierta (sin cobro en curso): selector, botón, textos y banner se actualizan.
- [ ] Ningún texto aparece como clave cruda (ej. `CJ_btn_procesando`).

## 1. Componentes nuevos

| Componente | Qué verificar |
|---|---|
| `SlidingTab_UC_790MY` (medio de pago) | Clic o toque en Efectivo/Tarjeta: el indicador se desliza (~200 ms) y cambia el panel de abajo. Con foco (Tab): anillo dorado sobre la opción elegida; ←/→, ↑/↓, Inicio y Fin cambian la opción. Al abrir la pantalla arranca en Tarjeta **sin** deslizarse. Deshabilitado durante el cobro (gris). |
| `BotonCobro_790MY` (Registrar cobro) | Disponible: bordó con ✓. Deshabilitado: gris con texto gris. Procesando: indicador que gira y «Procesando cobro…». Al terminar o fallar vuelve al estado anterior. Foco con Tab: anillo dorado; Enter/Espacio cobran. |
| «Reducir animaciones» | Windows → Accesibilidad → Efectos visuales → desactivar «Efectos de animación». El selector cambia sin deslizarse y el indicador del botón queda quieto. |

## 2. Flujo de cobro

- [ ] **Efectivo con vuelto**: modal «¿Confirmar el cobro de $X en Efectivo?»; foco inicial en Cancelar. Confirmar → spinner → comprobante → toast «Cobro registrado…» → la mesa sale de la lista.
- [ ] **Efectivo insuficiente**: ícono en «Monto recibido», botón deshabilitado y motivo debajo.
- [ ] **Tarjeta aprobada**, cuotas **1, 3, 6 y 12**: el modal dice «Tarjeta Visa (N cuota(s))»; datos del comprobante correctos.
- [ ] **Tarjeta con Luhn inválido**: ícono en el campo, botón deshabilitado.
- [ ] **Tarjeta rechazada (termina en `0002`)**: banner rojo con el mensaje del banco, ícono en el campo, foco en el número con el texto seleccionado; el botón vuelve a Disponible. Al editar el número o pasar a Efectivo, el banner y el ícono se van. Se puede cobrar en efectivo enseguida.
- [ ] **Cancelar en el modal** (botón, Esc o X): no pasa nada.
- [ ] **Doble clic rápido** sobre el botón y sobre «Confirmar cobro» del modal: un solo cobro y una sola factura.
- [ ] **La ventana responde durante el cobro**: el indicador gira sin trabarse; se puede mover/minimizar la ventana principal.
- [ ] Durante el cobro: lista de mesas, cuenta, descuento, pre-ticket, propina, selector y datos de pago deshabilitados.
- [ ] Durante el cobro: **menú lateral** (incluido cerrar sesión) y **selector de idioma** deshabilitados; se rehabilitan al terminar, también tras un rechazo o un error.
- [ ] **Cerrar la aplicación (X)** durante el cobro: no se cierra y aparece el banner ámbar «Hay un cobro en curso…»; el aviso se va al terminar.
- [ ] **Sin la patente `GenerarFactura`** y con cuenta ya emitida: cobra sobre esa factura. Sin cuenta emitida: banner «La mesa todavía no tiene una cuenta emitida…» y no se abre el modal.
- [ ] **Sin la patente `RegistrarCobro`**: el botón no se ve (`PermissionFilter`).
- [ ] **Dos instancias cobrando la misma factura**: la segunda recibe «factura no abierta» en el banner y la lista se recarga.
- [ ] **Base caída a mitad del cobro**: `MessageBox` de error fatal (el único permitido) y la pantalla queda usable.

## 3. Pre-ticket y comprobante

- [ ] **Pre-ticket**: se abre la vista; al cerrarla, toast «Factura N.º … generada».
- [ ] **Pre-ticket con error de regla**: banner (no `MessageBox`).
- [ ] **Imprimir sin impresora instalada**: banner en el comprobante con el motivo; el botón Imprimir sigue disponible.

## 4. Datos y bitácora (mismo resultado que la versión síncrona)

- [ ] Factura en `Pagada`, reserva `Finalizada`, mesa `Libre`, fila en `Pagos` con los mismos importes.
- [ ] Bitácora en el mismo orden: `PagoRegistrado`, `ReservaFinalizada`, `MesaEstadoActualizado`; en el rechazo, solo `PagoRechazado`.
- [ ] DV recalculado sin errores de integridad al reingresar.

## 5. Mediciones (E4-H1)

Por cada cobro aparecen cinco líneas `Cobro:`, en este orden:

```
Cobro: GenerarFactura: N ms
Cobro: Transaccion RegistrarCobro: N ms (Ok)
Cobro: DVManager.Recalcular: N ms
Cobro: Bitacora (3 eventos): N ms
Cobro: RegistrarCobro (total): N ms
```

Las tres del medio salen desde `PagoBLL_790MY`, dentro del total de `RegistrarCobro`. En un rechazo solo sale `GenerarFactura` (el banco rechaza antes de la transacción). `GenerarFactura` incluye su propio `Recalcular` cuando emite una factura nueva; si reutiliza la abierta, es solo lectura.
- [ ] Registrar 5 repeticiones de efectivo, 5 de tarjeta aprobada y 3 de `0002` en la sección «Mediciones» de `Decisiones_Etapa4_Facturacion_Cobro.md`.
