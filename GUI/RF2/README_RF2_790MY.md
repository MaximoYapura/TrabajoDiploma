# RF2 / PN2 — Atención en Mesa (GUI)

Menú lateral del MDI: **Atención en Mesa** (`btnAtencion_790MY`), visible si el usuario tiene
al menos una de las patentes del módulo. Cada opción se filtra con `PermissionFilter_08YS`
y se vuelve a validar con `SessionManager_08YS.Instance.ValidatePermission(...)` al abrirla.

| Opción | Formulario | Patente | Caso de uso | BLL |
|---|---|---|---|---|
| Abrir Mesa | `FormAbrirMesa_790MY` | `AbrirMesa` | CUN-004 (con reserva y Walk-in) | `ReservaBLL_790MY.AbrirMesa` / `AbrirMesaWalkIn`, `EmpleadoBLL_790MY.GetCamarerosActivos` |
| Registrar Pedido | `FormRegistrarPedido_790MY` | `RegistrarPedido` | CUN-005 | `PedidoBLL_790MY.RegistrarPedido` |
| Despachar Pedido | `FormCocinaKDS_790MY` | `DespacharPedido` | CUN-008 | `PedidoBLL_790MY.IniciarPreparacion` / `MarcarPedidoListo` |

SuperAdmin recibe las cuatro familias nuevas desde `Migraciones_790MY/CompletarRF2_AtencionMesa_790MY.sql`.

## Servicios transversales

- **i18n**: los tres formularios implementan `IIdiomaObserver_08YS`, se suscriben a
  `TraductorManager_08YS` y se desuscriben en `FormClosed`. Los textos estáticos usan la
  propiedad `Tag` (claves `AM_*`, `RP_*`, `KDS_*` en `idiomas.json`); los dinámicos se
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

Para probar CUN-005, el usuario logueado tiene que estar vinculado a un empleado con puesto
Camarero (`Empleados.UsuarioDNI`).

## Testing

- `Migraciones_790MY/DatosPrueba_AtencionMesa_790MY.sql`: productos de ejemplo por categoría
  (con uno en bajo stock y uno agotado) y vínculo del usuario `@UsuarioCamarero` como Camarero.
  Ejecutar con `sqlcmd -S <servidor> -E -I -f 65001 -i ...` (UTF-8 y `QUOTED_IDENTIFIER ON`).
- **Modo prueba de turno** (`GUI/Config/App.config`, `<appSettings>`): con
  `ModoPruebaTurno_790MY = true`, si la hora actual no cae en un turno real,
  `MesaBLL_790MY.ObtenerTurnoVigente` devuelve `TurnoPrueba_790MY` (por defecto 22:00).
  Abrir Mesa muestra "(MODO PRUEBA)" en el encabezado. Para el flujo "Con reserva", la reserva
  tiene que ser de HOY en ese turno. **Poner la bandera en `false` antes de entregar.**

## Pendientes (próximo sprint)

- **Retiro del pedido**: "Marcar como retirado" en Registrar Pedido solo oculta el aviso
  durante la sesión de la pantalla. Persistirlo requiere un estado `Entregado` en `Pedidos`.
- **Cierre de mesa**: `ReservaBLL_790MY.FinalizarReserva` (patente `CerrarMesa`) todavía no
  tiene pantalla; corresponde al registro del pago.

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
