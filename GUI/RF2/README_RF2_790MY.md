# RF2 / PN2 — Atención en Mesa (GUI)

Menú lateral del MDI: **Atención en Mesa** (`btnAtencion_790MY`), visible si el usuario tiene
al menos una de las patentes del módulo. Cada opción se filtra con `PermissionFilter_08YS`
y se vuelve a validar con `SessionManager_08YS.Instance.ValidatePermission(...)` al abrirla.

| Opción | Formulario | Patente | Caso de uso | BLL |
|---|---|---|---|---|
| Abrir Mesa | `FormAbrirMesa_790MY` | `AbrirMesa` | CUN-004 (con reserva y Walk-in) | `ReservaBLL_790MY.AbrirMesa` / `AbrirMesaWalkIn`, `EmpleadoBLL_790MY.GetCamarerosActivos` |
| Registrar Pedido | `FormRegistrarPedido_790MY` | `RegistrarPedido` | CUN-005 | `PedidoBLL_790MY.RegistrarPedido` |
| Despachar Pedido | `FormCocinaKDS_790MY` | `DespacharPedido` | CUN-008 | `PedidoBLL_790MY.IniciarPreparacion` / `MarcarPedidoListo` |
| Caja y Cobro | `FormFacturacionCobro_790MY` | `GenerarFactura` / `RegistrarCobro` | CUN-006 + CUN-007 | `FacturaBLL_790MY`, `PagoBLL_790MY` |

En **Reportes**: Arqueo y Cierre de Caja (`GUI/Reportes/FormArqueoCaja_790MY`, patente `GestionCaja`,
`PagoBLL_790MY.GenerarReporteArqueoCaja` / `CerrarCaja`).

SuperAdmin recibe las cuatro familias nuevas desde `Migraciones_790MY/CompletarRF2_AtencionMesa_790MY.sql`.

## Facturación y cobro (CUN-006 / CUN-007)

- **Consumos facturables**: ítems de pedidos `Listo` de la reserva, con el precio congelado en
  el pedido. Al generar la factura el detalle se copia a `ItemsFactura`.
- **Descuento**: porcentaje (0–100) o monto fijo (0–subtotal). El IVA (21 %) se calcula sobre
  el subtotal con descuento; importes a 2 decimales. `FacturaBLL_790MY.CalcularImportes` es
  una función pura que la pantalla usa para recalcular en tiempo real.
- **Factura Abierta única por reserva**: volver a generar con los mismos importes reutiliza la
  factura; con otros importes (más consumos u otro descuento) la anterior queda `Anulada`.
- **Cobro**: efectivo (monto recibido ≥ total + propina, calcula el cambio) o tarjeta (Luhn,
  cuotas 1/3/6/12, autorización bancaria simulada). Pago + factura `Pagada` + reserva
  `Finalizada` + mesa `Libre` en una única transacción (`SqlPagoRepository_790MY.RegistrarCobro`).
  No se cobra si quedan pedidos sin despachar.
- **Tarjetas de prueba**: `4111 1111 1111 1111` se aprueba; cualquier número válido terminado en
  `0002` (p. ej. `4000 0000 0000 0002`) lo rechaza el banco simulado (escenario 8.1: solo
  bitácora `PagoRechazado`, sin cambios de estado). El número se guarda enmascarado.
- **Visibilidad**: un camarero ve y cobra solo sus mesas; un usuario sin puesto de camarero
  (encargado / caja) ve todas las mesas Asistidas.
- **Comprobantes** (`FormComprobanteFactura_790MY`): pre-ticket "NO VÁLIDO COMO FACTURA" y
  Factura B PAGADA con detalle del pago, QR simulado, código de verificación y hash SHA-256
  (`FacturaBLL_790MY.CalcularHashComprobante`). Botón Imprimir con vista previa.
- **Bitácora** (módulo `Facturacion`): `FacturaGenerada`, `FacturaAnulada`, `PagoRegistrado`,
  `PagoRechazado`, `CierreCajaRealizado`, más `ReservaFinalizada` y `MesaEstadoActualizado`.
- **KDS**: semáforo por tiempo desde la solicitud: < 10 min verde, 10–20 min amarillo,
  > 20 min rojo (borde de la comanda, insignia y leyenda con totales en el encabezado).

## Servicios transversales

- **i18n**: los formularios implementan `IIdiomaObserver_08YS`, se suscriben a
  `TraductorManager_08YS` y se desuscriben en `FormClosed`. Los textos estáticos usan la
  propiedad `Tag` (claves `AM_*`, `RP_*`, `KDS_*`, `CJ_*` (caja), `AC_*` (arqueo) y
  `msg_fc_*` / `msg_ac_*` en `idiomas.json`); los dinámicos se
  regeneran en `UpdateIdioma`.
- **Permisos**: además del menú, cada formulario aplica `PermissionFilter_08YS` a su botón
  de acción y la BLL vuelve a validar la patente en cada operación.
- **Bitácora y DV**: los registra la BLL después del COMMIT (`RegistrarEventoPostCommit` y
  `DVManager_08YS.Recalcular()`), no la GUI, así no se duplican eventos:
  apertura → `ReservaAsistida`/`WalkInRegistrado` + `MesaAbierta` + `CamareroAsignado`;
  envío → `PedidoRegistrado` (o `PedidoRechazadoStock`); despacho → `PedidoEnPreparacion`,
  `PedidoListo` + `StockDescontado`.

## Base de datos

Ejecutar en orden, con la aplicación cerrada:

1. `Migraciones_790MY/CompletarRF2_AtencionMesa_790MY.sql`
2. `Migraciones_790MY/AgregarCategoriaProducto_790MY.sql` — agrega `Productos.Categoria`
   (Entrada, PlatoPrincipal, Postre, Bebida), que usan las pestañas del catálogo.
3. `Migraciones_790MY/CompletarABM_EmpleadosProductos_790MY.sql`
4. `Migraciones_790MY/CompletarFacturacionCobro_790MY.sql` — tablas `Facturas`, `ItemsFactura`
   y `Pagos`, RS15..RS17 de `sp_GetAllTablesParaDV`, patentes `GenerarFactura`,
   `RegistrarCobro`, `GestionCaja` (SuperAdmin; el rol Camarero recibe facturación y cobro).

Para probar CUN-005, el usuario logueado tiene que estar vinculado a un empleado con puesto
Camarero (`Empleados.UsuarioDNI`).

## Testing

- `Migraciones_790MY/DatosPrueba_AtencionMesa_790MY.sql`: productos de ejemplo por categoría
  (con uno en bajo stock y uno agotado) y vínculo del usuario `@UsuarioCamarero` como Camarero.
  Ejecutar con `sqlcmd -S <servidor> -E -I -f 65001 -i ...` (UTF-8 y `QUOTED_IDENTIFIER ON`).
- **Modo demostración** (`GUI/Config/App.config`, `<appSettings>`): con
  `ModoDemostracion = true`, si la hora actual no cae en un turno real,
  `MesaBLL_790MY.ObtenerTurnoVigente` devuelve `TurnoPrueba_790MY` (por defecto 22:00).
  Abrir Mesa muestra "(MODO DEMOSTRACIÓN)" en el encabezado. Para el flujo "Con reserva", la reserva
  tiene que ser de HOY en ese turno. Con `false` se aplica el control de turnos estricto.

## Pendientes (próximo sprint)

- **Retiro del pedido**: "Marcar como retirado" en Registrar Pedido solo oculta el aviso
  durante la sesión de la pantalla. Persistirlo requiere un estado `Entregado` en `Pedidos`.
- **Cierre de caja**: hoy deja constancia en la bitácora; no bloquea cobros posteriores del
  mismo día ni persiste un registro propio de cierre.

## Maestros: Empleados y Productos

| Opción (Maestros) | Formulario | Patente | BLL |
|---|---|---|---|
| Empleados | `GUI/Maestros/FormEmpleados_790MY` | `CrearEmpleado` | `EmpleadoBLL_790MY` (alta, modificación, baja lógica) |
| Productos | `GUI/Maestros/FormProductos_790MY` | `GestionProductos` | `ProductoBLL_790MY` (alta, modificación, baja lógica) |

- Migración: `Migraciones_790MY/CompletarABM_EmpleadosProductos_790MY.sql` (columna
  `Productos.ControlaStock`, patente `GestionProductos`, familia "Gestion de carta" para SuperAdmin).
- Un camarero con mesas abiertas no puede cambiar de cargo ni darse de baja.
- El stock físico no puede bajarse por debajo de lo comprometido por pedidos sin despachar.
- Un producto con `ControlaStock = false` no se rechaza por stock en CUN-005 ni se descuenta en CUN-008.
- Pendiente: historial de precios y stock mínimo configurable (hoy el umbral de "Bajo" es fijo).
