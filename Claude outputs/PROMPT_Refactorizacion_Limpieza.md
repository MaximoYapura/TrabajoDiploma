# Prompt para Claude Code: refactorización de limpieza por fases

## Contexto

Repo `TrabajoDiploma` (carpeta `TP-Ingenieria-de-Software-Beta`). C# WinForms .NET Framework 4.8, capas `GUI_08YS` / `BLL_08YS` / `DAL_08YS` / `BE_08YS` (+ `Service_08YS`, `MPP_08YS`). Rama actual: `beta`.

Vas a hacer una refactorización de **limpieza**, en fases, **sin cambiar el comportamiento**. El objetivo no es la perfección: es dejar el código más limpio y parecido a lo que pide la cátedra (separación de responsabilidades, BLL que se conectan entre sí, un tipo por archivo) sin agregar complejidad innecesaria. Si dudás entre dos soluciones, elegí la más simple.

Las listas de abajo salen de un análisis automático y de lectura de código. Pueden tener falsos positivos: **verificá cada punto antes de aplicarlo** y, si algo no coincide, no lo apliques y anotalo en el informe.

## Reglas que valen en todas las fases

1. **Sin cambios de comportamiento.** No cambiar permisos que exige cada método, eventos de bitácora, llamadas a `DVManager_08YS.Recalcular()`, consultas SQL, textos ni la API pública que usa la GUI (salvo lo que se declara muerto y se borra).
2. **No tocar ningún archivo `*_08YS*`** (ni sus `.Designer.cs` / `.resx`), ni `BLLFactory_08YS.cs`. Quedan fuera por decisión mía: son funcionales y no se evalúan ahora. Única excepción: las líneas de `.csproj` estrictamente necesarias. Si una tarea exigiera tocar un `_08YS`, no lo hagas: anotalo en el informe.
3. **No tocar** `FormSeleccionarMesa_790MY` ni `MesaVisualControl_790MY` (decisión previa: no se altera su maquetación).
4. **`.csproj` clásico:** todo archivo nuevo necesita `<Compile Include>`; todo archivo borrado o movido hay que quitarlo o actualizarlo. Al terminar cada fase comprobá que no hay `.cs` en disco fuera del csproj ni entradas del csproj sin archivo.
5. **Codificación:** respetá la de cada archivo existente (UTF-8 con BOM, CRLF). Los archivos nuevos: UTF-8 con BOM y CRLF. No reformatear ni reordenar `using` en archivos que no son parte del cambio: diffs mínimos.
6. **Arquitectura de la GUI:** `FormX.Designer.cs` solo diseño visual; `FormX.cs` solo lógica, eventos y comunicación con capas inferiores. Mantener `namespace`, `partial class` y nombres de controles.
7. **Verificación:** después de cada subpaso compilá la solución con MSBuild (Debug) sin errores ni warnings nuevos. Si no podés compilar en este entorno, decímelo y pedime que compile yo. No des por terminada una fase sin compilación.
8. **Git:**
   - Una rama por fase, creada desde `beta`: `refactor/fase1-codigo-muerto`, `refactor/fase2-mover-tipos`, `refactor/fase3-<tema o formulario>`, `refactor/fase4-<bll>`, `refactor/fase5-limpieza`.
   - Commits chicos, uno por tema, con mensaje descriptivo y **sin línea de co-autoría** (regla de este repo).
   - Podés pushear tu rama de fase. **No hagas merge a `beta` ni a `main`, ni push a `main`.**
   - Al cerrar cada fase deteneté y esperá mi OK.
9. **Antes de borrar** algo, buscá referencias en `.cs`, `.resx`, `.csproj`, `.json`, `.iss` y `.vdproj`.
10. **Informe al cerrar cada fase** (corto): qué cambió y en qué archivos, qué NO hiciste y por qué, y una checklist de pruebas manuales para que las haga yo.

## Paso 0: resguardo (antes de crear ninguna rama)

1. Corré `git status` y `git diff --stat`. Hoy hay cambios sin commitear: `GUI/GUI_08YS.csproj` y los `.Designer.cs` de `FormAbrirMesa_790MY` y `FormRegistrarPedido_790MY` (RF2), más sus `.resx` nuevos sin trackear. **No uses `git add -A`.**
2. Mostrame la lista de archivos y proponeme qué entra al commit de resguardo. Quedan **fuera**: `PROMPT_Deuda_Tecnica.md`, `Documentacion_Defensa_Fogon_borrador_previo.md`, `Claude outputs/assets/` y cualquier binario del instalador. Esperá mi OK.
3. Con mi OK: commit en `beta`, `push` a `origin/beta` y tag `backup-pre-refactor-2026-10-27` (también pusheado).
4. Yo recompilo el instalador desde ese commit y pruebo. **Esperá mi "baseline OK"** antes de crear ramas.

## Fase 1: código muerto

**1.1 Borrar archivos**
- `BLL/FormatoSerializacion_790MY.cs` (está fuera del csproj).
- `GUI/Maestros/FormRegistrarClienteModal_790MY.cs` (stub de 4 líneas, fuera del csproj).
- `MPP/ClienteMapper_790MY.cs` (clase `[Obsolete]` vacía) y quitarlo de `MPP_08YS.csproj`.
- `GUI/UserControls/UserControlMesa_790MY.cs` (sin referencias) y quitarlo de `GUI_08YS.csproj`.
- `script.sql` de la **raíz** de la solución. El válido es `GUI/script.sql` (está en el csproj): ese no se toca. Ya verifiqué que nada referencia el de la raíz (el `.vdproj` usa `GUI\bin\Debug\script.sql`).

**1.2 Reservas** (borrar por completo)
- `ReservaBLL_790MY.CancelarReserva` y `FinalizarReserva`.
- `IReservaRepository_790MY.CancelarReserva` / `FinalizarReserva` y sus implementaciones en `SqlReservaRepository_790MY`.
- `_pedidoRepository` de `ReservaBLL` solo se usaba en `FinalizarReserva`: quitá también `IPedidoRepository_790MY` del campo y del constructor, y el argumento en `BLLFactory_790MY.CreateReservaBLL`.
- **No** borrar `Evento.ReservaCancelada` (está en un archivo `_08YS` y puede figurar en la bitácora histórica).
- Si el comentario de cabecera de `ReservaBLL` menciona alguno de estos métodos, ajustalo.

**1.3 Comentario obsoleto**: en `ReservaBLL_790MY.cs` (~L230) dice `FormRegistrarClienteModal_790MY`; esa clase no existe. Cambialo por `FormRegistrarCliente_790MY` (GUI/RF1).

**1.4 Miembros sin llamadores** (verificar y borrar)
- `PedidoBLL_790MY.ObtenerPedidosListosDelCamarero`.
- `PagoBLL_790MY.ObtenerPagoDeFactura`.
- `IMesaRepository_790MY.GetDisponibles` y su implementación en `SqlMesaRepository_790MY`.
- `Empleado_790MY.TieneUsuario`.
- `Reserva_790MY.OcupaMesa` (verificá si `EstadosQueOcupanMesa` sigue usándose en otro lado antes de tocarlo).
- `TemaFogon_790MY.FuenteTitulo`, `FuenteSubtitulo`, `FuenteCuerpo`, `FuenteEtiqueta`, `FuenteAyuda`.
- `EstiloRF2_790MY.MostrarError` y `MostrarExito` (conservar `MostrarErrorEnBanner` y `MostrarExitoToast`).
- **No borrar** `FacturaBLL_790MY.ObtenerFactura`: se usa en la Fase 4.

**1.5 No tocar (son `_08YS`):** enum `UserRole`, `TraductorManager.IdiomaActual`, `ResultadoValidacion.EsValido`, `Permisos.RealizarRestore`, y el código comentado de `SqlUserRepository_08YS`, `FormGestionAcceso_08YS` y `FormGestionRespaldos_08YS`. Tampoco borres `DAL/TP_INGENIERIA_SOFTWAREDataSet.xss`: solo reportá si algo lo usa.

## Fase 2: un tipo por archivo (BLL, BE, DAL), sin cambiar namespace

- `BLL/CobroDTOs_790MY.cs` → un archivo por clase (`MesaCobroDTO_790MY`, `ImportesCuenta_790MY`, `DetalleCuentaDTO_790MY`, `ResultadoCobroDTO_790MY`, `ReporteArqueoCaja_790MY`).
- `ReportePredictivoBLL_790MY.cs` → sacar `OrdenReglas_790MY`, `ReglaAsociacion_790MY`, `TipoSugerencia_790MY`, `SugerenciaInteligente_790MY` y `ResumenCanasta_790MY` a un archivo cada uno.
- `ProductoCBLL_790MY.cs` → sacar `DiferenciaVersion_790MY`.
- `BE/Factura_790MY.cs` → dejar los enums junto a `Factura_790MY`; `ItemFactura_790MY` y `CanastaFactura_790MY` aparte.
- `BE/Pago_790MY.cs` → `SolicitudCobro_790MY` y `CobroArqueo_790MY` aparte.
- `BE/Pedido_790MY.cs` → `ItemPedido_790MY` aparte.
- `DAL/Interfaces_Repositories/IPedidoRepository_790MY.cs` → sacar `EstadoRegistroPedido_790MY`, `ResultadoRegistroPedido_790MY` y `ResultadoDespacho_790MY` (queda la interfaz).
- **No hacer:** extraer excepciones de `UserBLL_08YS` / `RolBLL_08YS`, partir `BitacoraEvento_08YS.cs`, ni separar los pares "enum + su clase" de 2 tipos ni las interfaces de Pago/Factura con su enum de resultado.

## Fase 3: clases auxiliares anidadas

Criterio: **duplicado → tipo compartido; con lógica o grande → archivo propio; pequeño (3-10 líneas, sin lógica) y de un solo formulario → se queda anidado.**

**3.1 Tipos compartidos (una rama):**
- `GUI/Maestros/ModoEdicion_790MY.cs`: enum `{ Reposo, Alta, Modificacion }`. Reemplaza los 4 `private enum ModoEdicion` de `FormClientes_790MY`, `FormEmpleados_790MY`, `FormMesas_790MY` y `FormProductos_790MY`.
- `GUI/Estilo/OpcionCombo_790MY.cs`: clase genérica `OpcionCombo_790MY<T>` con `Valor`, `Texto` y `ToString() => Texto`. Reemplaza `Opcion<T>` (`FormEmpleados`, `FormProductos`), `EstadoItem` (`FormReporteReservas_790MY`) y `TurnoItem` (`FormRegistrarReserva_790MY`). Renombrá las propiedades en los usos (`Display` pasa a `Texto`; `Valor` / `Hora` pasan a `Valor`).

**3.2 Archivo propio (una rama por formulario):**
- `ItemComanda` de `FormRegistrarPedido_790MY` → `GUI/RF2/ItemComanda_790MY.cs` (`internal`).
- `Ticket` de `FormComprobanteFactura_790MY` (~130 líneas) → archivo propio en `GUI/RF2` con sufijo `_790MY` (ej.: `TicketFactura_790MY`).
- `PieComprobante` de `ReservaBLL_790MY.Comprobante.cs` → `BLL/PieComprobante_790MY.cs` (`internal`).

**3.3 Se quedan anidadas:** `TextoSugerencia`, `Kpi`, `Consejo`, `Paso`, `Datos`, `EstadoEmail`, `NivelSemaforo`, `CalView`, `SlidingTabAccesible`, `Utf8StringWriter`, `DoubleBufferedPanel` (está en `FormSeleccionarMesa`, que no se toca), y todo lo que viva en archivos `_08YS` (`ComponentRow`, `EstadoUI`, etc.).

Estos tipos no viven en los `.Designer.cs`, así que no hay que tocarlos. Probaré cada formulario a 100/125/150 % de escala y en es/en.

## Fase 4: BLL que usan repositorios de otra entidad → pasar por la BLL correspondiente

**Idea:** hoy varias BLL consultan directo repositorios que no son de su entidad (ej.: `PedidoBLL` usa `IReservaRepository`). Pasarán por la BLL de esa entidad.

**Peligro principal: los permisos.** Los métodos públicos de la otra BLL validan permisos que el rol que llega no tiene (el Chef solo tiene `DespacharPedido`; el Camarero no tiene `VerReservas`, `AbrirMesa` ni `GestionProductos`). Por eso:
- En la BLL destino agregá métodos **`internal` que NO llaman a `ValidatePermission`** y usan **el mismo método de repositorio que usa hoy** el código que se migra (no cambiar consultas).
- El método público de origen **sigue validando su propio permiso al principio**, antes de usar el `internal`.
- Ningún método público existente cambia el permiso que exige.

**Métodos `internal` a agregar:**
- `EmpleadoBLL_790MY`: `ObtenerEmpleadoDeSesion()` (devuelve el `Empleado` o `null`; cada llamador aplica su propia regla), `ObtenerCamarerosActivos()` y `ObtenerPorDni(int)`. `GetCamarerosActivos` (público) pasa a validar `AbrirMesa` y delegar en el `internal`.
- `ReservaBLL_790MY`: `ObtenerPorId(int)`, `ObtenerAsistidasPorCamarero(int)` y `ObtenerAsistidas()`.
- `ProductoBLL_790MY`: `ObtenerActivos()`, `ObtenerPorId(int)` y `ExisteNombre(string, int)`.
- `PedidoBLL_790MY`: `ContarNoDespachados(int)`.
- `ClienteBLL_790MY`: `ExisteActivo(int dni)` (hoy `GetByDni(dni) != null`, que ya filtra `Activo = 1`).
- `MesaBLL_790MY`: `ObtenerPorNumero(int)` (misma lógica actual: `GetAll().FirstOrDefault(...)`).

**Estructura final (sin ciclos):**
```
Nivel 0: ClienteBLL, ProductoBLL, MesaBLL*, EmpleadoBLL*
ReservaBLL  -> Cliente, Mesa, Empleado      (ya sin Pedido, por 1.2)
PedidoBLL   -> Reserva, Producto, Empleado
FacturaBLL  -> Reserva, Pedido, Empleado
PagoBLL     -> Factura
ProductoCBLL-> Producto
```
\* `MesaBLL` y `EmpleadoBLL` **conservan** `IReservaRepository` solo para sus chequeos de integridad de solo lectura antes de dar de baja (`MesaBLL.EliminarMesa`, `EmpleadoBLL.ValidarSinMesasAbiertas` / `ContarMesasAbiertas`). Si lo cambiaras a BLL→BLL formarías un ciclo y la fábrica entraría en recursión infinita. Dejá un comentario `// Excepción: ...` en esos puntos.

**Sin cambios:** `EmpleadoBLL` → `IUserRepository` (`UserBLL` es `_08YS`), `ReportePredictivoBLL` → `IFacturaRepository`, y las relaciones User/Familia/Rol.

**Duplicados a unificar:** `PedidoBLL.ObtenerCamareroDeSesion` (lanza error si no es camarero) y `FacturaBLL.ObtenerCamareroDeSesion` (devuelve `null`) pasan a usar `EmpleadoBLL.ObtenerEmpleadoDeSesion()`, conservando cada una su regla. Lo mismo con `CamarerosPorDni` (en `PedidoBLL` y `FacturaBLL`).

**Orden y ramas (una por BLL de origen):**
- 4.1 `PagoBLL` → `FacturaBLL.ObtenerFactura` (método público ya compatible con `RegistrarCobro`; no hace falta agregar nada).
- 4.2 `EmpleadoBLL` internos + `ReservaBLL.ValidarCamarero`.
- 4.3 `ReservaBLL` → `ClienteBLL` y `MesaBLL`.
- 4.4 `PedidoBLL` → `EmpleadoBLL`, `ProductoBLL` y `ReservaBLL`.
- 4.5 `FacturaBLL` → `EmpleadoBLL`, `ReservaBLL` y `PedidoBLL`.
- 4.6 (opcional) `ProductoCBLL` → `ProductoBLL`.

Los constructores solo cambian a través de `BLLFactory_790MY` (ya verifiqué que no hay otros `new XBLL(...)` en el repo). La GUI no se toca. No muevas escrituras entre BLL: las transacciones multi-tabla ya viven dentro de los repositorios.

**Pruebas por rol (las hago yo al cierre de cada rama; dame la checklist):** Camarero (mesas asignadas, catálogo, registrar pedido, pedidos listos, generar factura, cobrar), Chef (abrir el KDS con los nombres de camareros, iniciar preparación, marcar listo), Recepcionista (abrir mesa con reserva y Walk-in, registrar reserva, reporte de reservas), SuperAdmin (todo) y un rol personalizado con **solo** el permiso del caso de uso.

## Fase 5: claves de idioma y `using`

- **`GUI/idiomas.json`:** hay ~100 claves candidatas sin uso. Generá la lista con una búsqueda por texto en `.cs` y `.resx` y **mostrámela antes de borrar**. Excluir: claves que se usan como nombre de permiso (`AbrirMesa`, `CerrarMesa`, `CrearEmpleado`, `DespacharPedido`, `GestionProductos`, `RegistrarPedido`, `VerMaestros`, `VerReportes`), y las de pantallas `_08YS` (roles, familias, usuarios, respaldos, login, contraseña, `msg_rol_*`, `msg_familia_*`, `msg_user_*`, `backup_ok`, `titulo_alta_*`, `titulo_mod_*`, etc.). Borrar siempre en `es` y `en` a la vez y dejar el JSON válido. Las claves de las funciones que se borraron en la 1.2 (`msg_rf2_reserva_no_cancelable`, `msg_rf2_reserva_no_finalizable`, `msg_rf2_reserva_con_pedidos_pendientes`) pueden entrar en la lista.
- **`using` sobrantes:** no lo hagas con scripts. Dame la lista de archivos **no `_08YS`** con `using` probablemente innecesarios (sobre todo `System.Linq`, `System.Text`, `System.Threading.Tasks`, `System.Collections.Generic`, `System.Data`) y lo aplico yo con Visual Studio.

## Fase 6: cierre

Actualizá `Documentacion_Defensa_Fogon.md` con lo que cambió (en especial las menciones a `CancelarReserva` / `FinalizarReserva` y las nuevas relaciones BLL→BLL para los diagramas de secuencia). Dejá `docs/Informe_Deuda_Tecnica.md` como está (es histórico). Entregame un informe final con todas las ramas, lo que quedó afuera y por qué.

---

**Empezá por el Paso 0 y deteneté.**
