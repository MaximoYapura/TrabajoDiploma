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
Camarero (`Empleados.UsuarioDNI`); ver el ejemplo comentado en la migración RF2.

## Pendientes (próximo sprint)

- **ABM de Empleados (`Empleado_790MY`)**: la BLL ya existe (`EmpleadoBLL_790MY.RegistrarEmpleado`
  / `ModificarEmpleado`, patente `CrearEmpleado`). Falta `FormEmpleados_790MY` en *Maestros*.
  Por ahora los empleados se cargan por SQL.
- **ABM de Productos (`Producto_790MY`)**: falta repositorio de escritura, BLL y
  `FormProductos_790MY` (nombre, categoría, precio, stock, activo) en *Maestros*. Por ahora se
  cargan por SQL.
- **Retiro del pedido**: "Marcar como retirado" en Registrar Pedido solo oculta el aviso
  durante la sesión de la pantalla. Persistirlo requiere un estado `Entregado` en `Pedidos`.
- **Cierre de mesa**: `ReservaBLL_790MY.FinalizarReserva` (patente `CerrarMesa`) todavía no
  tiene pantalla; corresponde al registro del pago.
