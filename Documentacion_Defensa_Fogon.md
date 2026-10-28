# Guía Técnica de Defensa de Tesis — "El Fogón del Sur"

> Documento generado a partir de la lectura del código fuente real del repositorio `TP-Ingenieria-de-Software-Beta` (solución `TrabajoDiploma.slnx`). Los nombres de clases, métodos, enums y claves de idioma son los que existen en el código. Donde algo **no se pudo verificar** o **no existe**, se indica explícitamente. Cada área incluye al final una lista de *Observaciones de auditoría / mejoras* basada en lo leído.
>
> No se incluyen credenciales: `GUI/Config/App.secrets.config` (SMTP) está fuera del repositorio y solo se documenta su plantilla.

## Índice

0. Arquitectura transversal (lo que hay que saber antes de defender cualquier módulo)
1. Casos de Uso Principales: 1.1 Reservas · 1.2 Abrir Mesa (CUN-004 / CUN-005 Walk-In) · 1.3 Registrar Pedido (CUN-006) · 1.4 Despachar Pedido / Cocina (KDS) (CUN-008) · 1.5 Caja y Cobro (CUN-007 Cobrar Venta)
2. Maestros: 2.1 Clientes · 2.2 Empleados · 2.3 Mesas · 2.4 Productos / Categorías · 2.5 Gestión de Accesos / Roles
3. Reportes: 3.1 Reporte de Reservas · 3.2 Arqueo y Cierre de Caja · 3.3 Reporte Predictivo (Inteligente)
4. Bitácora de Cambios (auditoría de productos) y Bitácora del sistema
5. Sistema de Ayuda "Sombrero del Chef"
6. Instalador y Despliegue
7. Preguntas probables del tribunal y respuestas cortas
8. Resumen consolidado de mejoras

---

## 0. Arquitectura transversal

### 0.1 Tecnología y proyectos

| Proyecto | Rol | Namespace principal |
|---|---|---|
| `GUI_08YS` | Windows Forms, .NET Framework 4.8. Formularios, UserControls, estilo | `GUI_08YS.Estilo`, `.RF1`, `.RF2`, `.Maestros`, `.Reportes`, `.Admin`, `.Usuario` |
| `BLL_08YS` | Reglas de negocio, validaciones, orquestación | `BLL_08YS` |
| `DAL_08YS` | Acceso a datos ADO.NET (`System.Data.SqlClient`), repositorios | `DAL_08YS`, `DAL_08YS.SQL`, `DAL_08YS.Interfaces_Repositories`, `DAL_08YS.Instalador` |
| `BE_08YS` | Entidades de negocio | `BE_08YS` |
| `MPP_08YS` | Mappers de seguridad y bitácora (`AccessMapper_08YS`, `BitacoraMapper_08YS`, `UserMapper_08YS`) | — |
| `Service_08YS` | Servicios transversales: sesión, traductor, bitácora (entidades), seguridad, encriptación | `Service_08YS`, `Service_08YS.Entities.Acceso`, `.Bitacora` |

Convención de sufijos: `_790MY` y `_08YS` identifican a los dos autores/etapas del código; ambos conviven y se usan tal cual en este documento.

### 0.2 Librerías externas (confirmadas en `packages.config` / csproj de la GUI)

| Librería | Versión | Uso |
|---|---|---|
| FontAwesome.Sharp | 6.6.0 | `IconButton`, `IconPictureBox`, `IconChar` (íconos vectoriales en botones y tarjetas KPI) |
| iTextSharp | 5.5.13.3 | PDF de comprobante de reserva y reporte de reservas |
| Portable.BouncyCastle | 1.9.0 | Dependencia de iTextSharp (hay *binding redirect*) |
| Newtonsoft.Json | 13.0.4 | `RegistroVisita_790MY` (`ayuda.json`) y carga de `idiomas.json` |
| System.Net.Mail / System.Configuration | del Framework | Envío SMTP del comprobante (`SmtpSection`) |

También se usa P/Invoke (`SendMessage` con `EM_SETCUEBANNER = 0x1501`) para *placeholders* en cajas de texto y `ReleaseCapture`/`SendMessage` en `FormMDI_08YS` para arrastrar la ventana sin barra de título.

### 0.3 Arranque de la aplicación (`GUI/Config/Program.cs`)

1. `BLLInstalador_08YS.AsegurarBaseDatos()` → `InstaladorBD_08YS.AsegurarBaseDatos()` (crea la BD si falta; ver §6).
2. `EventCatalog_08YS.CatalogValidation()`: lanza `NotImplementedException` si algún valor del enum `Evento` no está en el catálogo (obliga a registrar módulo y criticidad de cada evento nuevo).
3. `DVManager_08YS.Inicializar(BLLFactory_08YS.CreateDvBLL())`.
4. `Application.Run(new FormLogin_08YS())`.

Si la instalación de la BD falla, `Program.cs` muestra un `MessageBox` (error fatal: excepción justificada a la regla "sin MessageBox").

### 0.4 Inyección de dependencias manual (fábricas)

- `BLLFactory_08YS`: `CreateUserBLL`, `CreateBitacoraBLL`, `CreateFamiliaBLL`, `CreateRolBLL`, `CreateDvBLL`, `CreateBackupBLL`.
- `BLLFactory_790MY`: `CreateClienteBLL`, `CreateMesaBLL`, `CreateReservaBLL`, `CreateEmpleadoBLL`, `CreateProductoBLL`, `CreatePedidoBLL`, `CreateFacturaBLL`, `CreatePagoBLL`, `CreateReportePredictivoBLL`, `CreateProductoCBLL`.
- Los repositorios 790MY (`SqlXxxRepository_790MY`) reciben la cadena de conexión; los 08YS usan `IDbFactory_08YS` / `SqlDbFactory_08YS` (con `SqlDbFactory_08YS.ConnectionString`, leída de `App.config`, cadena `TP_Ing_Soft`).
- Todos los repositorios se consumen **por interfaz** (`IXxxRepository_790MY` / `_08YS`) → las BLL no conocen SQL (inversión de dependencias, testeable con dobles).

#### 0.4.1 Relaciones entre BLL (BLL → BLL)

Cada BLL lee solo el repositorio de **su** entidad; cuando necesita datos de otra entidad pasa por la BLL dueña. `BLLFactory_790MY` arma el grafo (sin ciclos):

```
Nivel 0: ClienteBLL, ProductoBLL, MesaBLL*, EmpleadoBLL*
ReservaBLL   -> ClienteBLL, MesaBLL, EmpleadoBLL
PedidoBLL    -> ReservaBLL, ProductoBLL, EmpleadoBLL
FacturaBLL   -> ReservaBLL, PedidoBLL, EmpleadoBLL
PagoBLL      -> FacturaBLL
ProductoCBLL -> ProductoBLL
```

- **Permisos:** la BLL destino expone métodos `internal` **sin** `ValidatePermission` (p. ej. `EmpleadoBLL_790MY.ObtenerEmpleadoDeSesion/ObtenerCamarerosActivos/ObtenerPorDni`, `ReservaBLL_790MY.ObtenerPorId/ObtenerAsistidasPorCamarero/ObtenerAsistidas`, `ProductoBLL_790MY.ObtenerActivos/ObtenerPorId/ExisteNombre`, `PedidoBLL_790MY.ContarNoDespachados`, `ClienteBLL_790MY.ExisteActivo`, `MesaBLL_790MY.ObtenerPorNumero`). El método público de origen valida **su** patente al principio; así el Chef (solo `DespacharPedido`) o el Camarero (sin `VerReservas`, `AbrirMesa` ni `GestionProductos`) no necesitan permisos ajenos. `internal` impide que la GUI los llame.
- **\* Excepción:** `MesaBLL_790MY.EliminarMesa` y `EmpleadoBLL_790MY.ValidarSinMesasAbiertas/ContarMesasAbiertas` siguen leyendo `IReservaRepository_790MY` (chequeos de integridad de solo lectura antes de la baja). Pasar por `ReservaBLL` formaría un ciclo `ReservaBLL → MesaBLL/EmpleadoBLL → ReservaBLL` y la fábrica entraría en recursión infinita.
- Sin cambios: `EmpleadoBLL → IUserRepository_08YS`, `ReportePredictivoBLL → IFacturaRepository_790MY` y las relaciones de usuarios, familias y roles (`_08YS`).

### 0.5 Patrón estándar de una operación de BLL

```
SessionManager_08YS.Instance.ValidatePermission(Permisos.X)   // 1. patente
→ validaciones de negocio (ArgumentException con texto traducido)  // 2. MensajesBLL_790MY.Regla(clave, args)
→ persistencia atómica (transacción, a veces SERIALIZABLE + UPDLOCK/HOLDLOCK) // 3. DAL
→ DVManager_08YS.Recalcular()                                   // 4. dígitos verificadores
→ BitacoraBLL_08YS.RegistrarEventoPostCommit(Evento.X, detalle) // 5. auditoría (después del COMMIT)
```

- `MensajesBLL_790MY.Regla(clave, args)` devuelve una `ArgumentException` con el texto traducido (usa `TraductorManager_08YS`). La BLL **nunca** muestra `MessageBox`: lanza excepciones y la GUI las muestra con `EstiloRF2_790MY.MostrarErrorEnBanner(banner, ex)` (banner) o toast.
- `RegistrarEventoPostCommit` reintenta una vez y, si falla, deja traza con `Trace.TraceError` **sin relanzar** (decisión de diseño: no informar un error sobre un dato que sí se guardó y evitar duplicados por reintento del usuario). `DVManager_08YS.Recalcular` sigue el mismo criterio y, si falla, se registra el evento `RecalculoDVFallido` (Criticidad.Critico).

### 0.6 Seguridad (resumen; detalle en §2.5)

- Patrón **Composite**: `AccessComponent_08YS` (abstracta) → `Permiso_08YS` (hoja) / `Familia_08YS` (compuesto, contiene permisos y otras familias). `Rol_08YS` **no hereda** de `AccessComponent_08YS`: es el contenedor raíz que agrupa familias y permisos sueltos.
- `Permisos` (enum): `VerBitacora`, `VerUsuarios`, `VerFamilias`, `VerRoles`, `VerRespaldos`, `AuditarProductos`, `VerClientes`, `VerMesas`, `CrearEmpleado`, `GestionProductos`, `RegistrarReserva`, `VerReservas`, `AbrirMesa`, `RegistrarPedido`, `DespacharPedido`, `GenerarFactura`, `RegistrarCobro`, `GestionCaja`, `GestionReportePredictivo`, etc. (se verificaron los que usa `FormMDI_08YS`).
- `SessionManager_08YS` (Singleton con `Lazy`): `Current`, `HasPermission`, `ValidatePermission`, evento `SesionInvalidada`.
- `PermissionFilter_08YS.Aplicar(this, _mapaPermisos)` pone `Visible` en cada control según el permiso (diccionario `nombre de control → Permisos`); `AplicarMenuStrip` hace lo mismo con los ítems de menú. No los deshabilita: los oculta.
- Contraseñas: `Encriptador_08YS`, PBKDF2 (`Rfc2898DeriveBytes`, SHA-256, 100 000 iteraciones, sal de 16 bytes, hash de 32 bytes, Base64). Bloqueo tras 3 intentos fallidos en 2 horas (`ContarIntentosFallidos`).
- Dígitos verificadores: `DvEncriptador_08YS` (AES-256-CBC, IV derivado por SHA-256 del nombre de tabla, clave fija en código), `DVManager_08YS`, `DvBLL_08YS`, procedimiento `sp_GetAllTablesParaDV`, tablas `DV`.

### 0.7 Internacionalización

- `TraductorManager_08YS.Instance` (Singleton): `GetTexto(clave)`, `CambiarIdioma("es"|"en")`, `IdiomaActual`, `CulturaActual` (`es-AR` / `en-US`), `Suscribir`/`Desuscribir`.
- Patrón **Observer**: `IIdiomaObserver_08YS.UpdateIdioma()`. Cada formulario se suscribe en el constructor y se desuscribe en `FormClosed`.
- Archivo `GUI/idiomas.json` (estructura `{ "es": { clave: texto }, "en": {...} }`; copiado siempre a la salida: `CopyToOutputDirectory=Always`). Los controles guardan su clave en la propiedad `Tag` y `EstiloRF2_790MY.TraducirTags(this)` los traduce. Prefijos de clave: `AM_` (Abrir Mesa), `RP_` (Registrar Pedido), `KDS_`, `CJ_` (Caja), `AC_` (Arqueo), `RR_` (Registrar Reserva), `FC_` (Clientes), `FM_` (Mesas), `FE_` (Empleados), `FP_` (Productos), `REP_`/`RI_` (reportes), `SM_`, `CR_`, `BC_` (Bitácora de Cambios), `AY_` (Ayuda; 197 claves únicas con ese prefijo) y `msg_*` (mensajes).

### 0.8 Sistema de diseño compartido (`GUI/Estilo`)

| Clase | Qué hace |
|---|---|
| `TemaFogon_790MY` (internal static) | Paleta: Accion `#6B1D2F` (bordó), Marca, Oro, Crema, Panel, Línea, Tinta; colores de estado Ok/Aviso/Ocup/Fuera/Error; tema oscuro de cocina (KDS); colores de barra lateral `Barra*`; escala de fuentes Segoe UI; espaciados múltiplos de 4; radios 6/10/12; helpers `EstilizarBoton`, `EstilizarGrilla`, `PonerMarcaDeAgua`, `EstilizarOpcion` |
| `Banner_UC_790MY` | Banner de avisos `Dock=Top`; `TipoBanner_790MY` (Error/Aviso/Info); `Mostrar(msg, tipo, cerrable)`, `Ocultar()` |
| `ConfirmModal_790MY` | `Confirmar(owner, titulo, mensaje, textoAccion)`; el foco inicial queda en *Cancelar* (acción destructiva segura) |
| `ToastHost_790MY` | `Mostrar(ancla, mensaje)`; abajo a la derecha, se cierra solo a los 4 s |
| `SlidingTab_UC_790MY` | Selector con indicador deslizante (animación de 200 ms), claves de idioma por pestaña |
| `BotonCobro_790MY` | Botón con estados `EstadoBotonCobro_790MY` (Disponible/Deshabilitado/Procesando) y *spinner* GDI+ |
| `PanelRedondeado_790MY` | Panel de esquinas redondeadas (`Radio`, `ColorBorde`, método estático `CrearRuta`) |
| `EstiloRF2_790MY` (alias `E`) | Paleta del RF2, `T()` (traducir), `TraducirTags`, `MostrarErrorEnBanner`, `MostrarExitoToast`, `CrearLabel`, `CrearChip` / `EtiquetaChip_790MY` |
| `EstiloMaestros_790MY` | `EstilizarGrilla`, `EstilizarBoton`, `PintarPildora`, `PintarEstado` (píldoras de estado en `DataGridView`) |

Regla de interfaz aplicada en todo el sistema: **ErrorProvider** para el campo con error + **Banner** para el mensaje + **Toast** para el éxito + **ConfirmModal** para acciones destructivas; `MessageBox` solo para errores fatales (decisión E2-D1). *Excepción detectada:* `FormGestionAcceso_08YS`, `FormAccesoAM_08YS` y `FormBitacora_08YS` (código 08YS, anterior a la regla) todavía usan `MessageBox` (ver mejoras).

### 0.9 Regla de oro de capas del proyecto (Project Instructions)

`FormX.Designer.cs` = solo diseño visual dentro de `InitializeComponent()`; `FormX.cs` = solo lógica de presentación, eventos y comunicación con la BLL; el constructor llama a `InitializeComponent()` y a la carga inicial. **Observación:** en varios formularios se crean controles dinámicos desde `FormX.cs` (p. ej. `CrearTarjetasKpi` en `FormArqueoCaja_790MY` instancia `PanelRedondeado_790MY`, `IconPictureBox` y `Label`; `ArmarTablaDiff` en `FormBitacoraCambios_790MY`; `FormRegistrarReserva_790MY`/`FormSeleccionarMesa_790MY` crean el mapa de mesas). Esto es justificable (el contenido depende de datos en tiempo de ejecución) pero **no cumple a la letra** la regla de "prohibido instanciar controles en `FormX.cs`"; se lista en las mejoras.

---

## 1. CASOS DE USO PRINCIPALES

Flujo completo del restaurante (es también el recorrido del *Menú Degustación* de la ayuda): **Cliente → Reserva → Abrir mesa → Registrar pedido → Cocina (KDS) → Caja y cobro**.

Estados que lo sostienen:

| Entidad | Enum | Valores |
|---|---|---|
| `Reserva_790MY` | `EstadoReserva_790MY` | Confirmada, Cancelada, Asistida, Finalizada (`EstadosQueOcupanMesa` define cuáles bloquean una mesa) |
| `Mesa_790MY` | `EstadoMesa_790MY` | Libre, Ocupada, Reservada, FueraDeServicio |
| `Pedido_790MY` | `EstadoPedido_790MY` | Pendiente, EnPreparacion, Listo (`EstadosConStockComprometido`) |
| `Factura_790MY` | `EstadoFactura_790MY` | Abierta, Pagada, Anulada |

Turnos: 18:00, 20:00 y 22:00 (2 h cada uno) en `MesaBLL_790MY.TurnosValidos`. `ModoDemostracion` y `TurnoPrueba_790MY` (App.config: `true` y `22:00`) permiten simular un turno vigente: `MesaBLL_790MY.ObtenerTurnoVigente`, `EsTurnoDePrueba`, `ObtenerTurnoVigenteObligatorio`.

---

### 1.1 Reservas

#### A. Idea y Diseño Visual

- **Objetivo:** que la recepcionista registre una reserva para un cliente existente en una fecha/turno/cantidad de comensales y elija la mesa **viendo un mapa del salón**, no una lista.
- **Formularios (namespace `GUI_08YS.RF1`):** `FormRegistrarReserva_790MY` (formulario principal, con resumen en vivo), `FormSeleccionarMesa_790MY` (modal con el mapa), `FormRegistrarCliente_790MY` (alta rápida de cliente en modal; usa `FormListaSimple_790MY` para emails/teléfonos adicionales), `FormComprobanteReserva_790MY` (comprobante tras confirmar; **no se leyó su código**, por eso no se detalla).
- **UI programada:** el mapa de mesas es un `DoubleBufferedPanel` con una grilla de 4×3 de `MesaVisualControl_790MY`. Cada `MesaVisualControl_790MY` es un `Control` propio con `BackColor` transparente simulado (`SupportsTransparentBackColor` + `OnPaintBackground` que dibuja la porción de imagen del padre) para que la mesa "flote" sobre la foto de fondo `Properties.Resources.FondoRestauran`. El logo es `Resources.LogoElFogon`. Forma según capacidad: ≤2 personas círculo 70×70; 3–4 rectángulo 85×65; 5–8 rectángulo 120×65; se dibujan sillas. El color depende de `EstadoMapa_790MY` (ver B). `MesaMapaDTO_790MY.EsCircular = Capacidad <= 2`.
- **UX:** resumen en vivo de la reserva a la derecha; `ErrorProvider` por campo + `Banner_UC_790MY` para el mensaje + `ToastHost_790MY` al confirmar; banner flotante dentro del mapa; paleta propia del formulario alineada a `TemaFogon`.

#### B. Lógica y Reglas de Negocio

- **Caso de uso central:** `ReservaBLL_790MY.RegistrarReserva` (permiso `Permisos.RegistrarReserva`).
- **Reglas:**
  - Mesa informada (`msg_rf2_falta_mesa`); cliente existente **y activo** (`ClienteBLL_790MY.ExisteActivo`, que usa `GetByDni` y filtra `Activo = 1`; si no: `msg_rf2_cliente_inexistente`); `ValidarComensales` (comensales > 0 y ≤ capacidad: `msg_rf2_comensales_invalidos` / `msg_rf2_capacidad_insuficiente`); fecha+hora **no pasada** (`msg_rf2_reserva_en_pasado`).
  - La mesa debe estar libre en esa fecha/turno. La disponibilidad se re-valida **dentro de la transacción** (`AddSiMesaDisponible(reserva, ocuparMesaFisica: false)`: la reserva no ocupa físicamente la mesa, solo la compromete en ese turno). Si falla: `msg_rf2_mesa_no_disponible`.
  - Los turnos 18/20/22 h se ofrecen desde la GUI (`OpcionCombo_790MY<TimeSpan>` + `ReservaBLL_790MY.FormatearTurno`) y `MesaBLL_790MY.TurnosValidos`.
  - Mapa (`MesaBLL_790MY.ObtenerMesasMapa(fecha, hora, comensales)`) devuelve `MesaMapaDTO_790MY` con `EstadoMapa_790MY`: **DisponibleRecomendada** (la de capacidad mínima suficiente = *best fit*), **Disponible**, **Ocupada**, **Incompatible** (capacidad insuficiente).
  - Cancelación: hoy no hay un caso de uso que cancele reservas. `CancelarReserva` se eliminó en la refactorización de limpieza (no tenía llamadores); el estado `Cancelada` y `Evento.ReservaCancelada` se conservan por los datos y la bitácora históricos.
  - Si la mesa se ocupó entre que se mostró el mapa y se confirmó → se rechaza y se registra `Evento.ReservaRechazadaDisponibilidad`.
- **Validaciones de cliente (en GUI):** DNI `^\d{7,8}$`, email `^[^@\s]+@[^@\s]+\.[^@\s]+$`, celular `^\d+$`. La BLL del cliente exige `dni > 0` y unicidad (ver §2.1). *Nota: la regla "7–8 dígitos" vive solo en la GUI; la BLL no la repite* (mejora).
- **Excepciones:** `ArgumentException` traducida desde `MensajesBLL_790MY.Regla` → banner. Nunca `MessageBox`.

#### C. Flujo de Trabajo por Capas

```
FormRegistrarReserva_790MY
  ├─ BLLFactory_790MY.CreateClienteBLL()  → ClienteBLL_790MY.GetByDni()/Exists()
  ├─ FormSeleccionarMesa_790MY → MesaBLL_790MY.ObtenerMesasMapa(fecha, hora, comensales)
  │        └─ IMesaRepository_790MY → SqlMesaRepository_790MY  (usa FiltroOcupacionMesa_790MY)
  └─ ReservaBLL_790MY.RegistrarReserva(...)
           ├─ ClienteBLL_790MY.ExisteActivo(dni)   // internal, sin ValidatePermission
           └─ IReservaRepository_790MY.AddSiMesaDisponible(reserva)   // SqlReservaRepository_790MY
                  → transacción SERIALIZABLE, UPDLOCK + HOLDLOCK, INSERT solo si la mesa sigue libre
           → DVManager_08YS.Recalcular()
           → BitacoraBLL_08YS.RegistrarEventoPostCommit(Evento.ReservaRegistrada, ...)
BE: Reserva_790MY, Mesa_790MY, Cliente_790MY
```

Métodos clave de la BLL: `RegistrarReserva`, `Buscar`, `ObtenerReservasParaApertura`, `ContarMesasACargo`, `AbrirMesa`, `AbrirMesaWalkIn`, `ValidarComensales`, `ValidarCamarero`. Para otras BLL expone consultas `internal` sin `ValidatePermission` (`ObtenerPorId`, `ObtenerAsistidasPorCamarero`, `ObtenerAsistidas`; ver §Relaciones entre BLL).

`FiltroOcupacionMesa_790MY` (DAL) construye el fragmento SQL de "mesa ocupada" a partir de `Reserva_790MY.EstadosQueOcupanMesa` y `Pedido_790MY.EstadosConStockComprometido`, de modo que el criterio de ocupación existe **en un solo lugar**.

**Concurrencia (para defender):** dos recepcionistas reservando la misma mesa a la vez: `AddSiMesaDisponible` abre una transacción `Serializable`, lee con `UPDLOCK, HOLDLOCK` y solo inserta si no hay conflicto; el segundo recibe un rechazo, no una reserva duplicada.

#### D. Clases Auxiliares e Integración

- **Comprobante PDF** (`ReservaBLL_790MY.Comprobante.cs`, `GenerarComprobantePDF`): iTextSharp, tamaño A5, banda de marca bordó, logo cacheado a 256 px desde `Resources/logo_fogon.png`, código de barras `Barcode128` con el texto `"RES-" + número`, "tiles" de datos, pie con `PieComprobante_790MY : PdfPageEventHelper` (archivo propio, `internal`), textos localizados (`TraductorManager_08YS`). Helpers: `FormatearTurno`, `FormatearNumeroReserva` (formato `"D6"`), `NombreArchivoComprobante`.
- **Email** (`EnviarComprobanteEmail`, `EmailConfigurado`): `System.Net.Mail` + sección `system.net/mailSettings/smtp` (`configSource="App.secrets.config"`). **Nunca lanza** (devuelve resultado para que la GUI avise sin perder la reserva).
- **Traducciones:** prefijo `RR_` y `msg_*`.
- **Dependencias externas:** SMTP, iTextSharp, BouncyCastle.

**Observaciones / mejoras (1.1):**
0. *(Resuelto)* `ReservaBLL_790MY.CancelarReserva` no validaba patente; primero se le agregó `ValidatePermission` y después se eliminó por no tener llamadores (refactorización de limpieza, fase 1).
1. Repetir la validación de DNI 7–8 dígitos en `ClienteBLL_790MY` (hoy solo `dni > 0`).
2. `FormComprobanteReserva_790MY` no fue auditado.
3. El tamaño del mapa (4×3 = 12) coincide con el tope de 12 mesas de `MesaBLL_790MY.RegistrarMesa`; si se sube el tope hay que rediseñar la grilla.
4. `AlterTableMesas_Mapa.sql` (carpeta `Scripts/`) borra reservas y mesas ("entorno de desarrollo / reset"): no debe correrse en producción; está superado por `Deploy_TP_Ing_Soft.sql`.

---

### 1.2 Abrir Mesa (CUN-004) y Registrar Walk-In (CUN-005)

#### A. Idea y Diseño Visual

- **Objetivo:** el recepcionista marca la llegada de los clientes: pasa una **reserva** a *Asistida* o registra un **walk-in** (cliente sin reserva), asigna **camarero** y deja la mesa *Ocupada*.
- **Formulario:** `FormAbrirMesa_790MY` (namespace `GUI_08YS.RF2`).
- **UI:** dos modos (Reserva / Walk-in) con `SlidingTab_UC_790MY sldModo_790MY`; tarjetas con `PanelRedondeado_790MY`; chips de estado con `EtiquetaChip_790MY`; reloj en vivo (`timerReloj_790MY`) que muestra el turno vigente; estilo `EstiloRF2_790MY`. Banner + toast + `ErrorProvider`.

#### B. Lógica y Reglas de Negocio

- **Caso central:** `ReservaBLL_790MY.AbrirMesa` (modo reserva) y `AbrirMesaWalkIn` (modo walk-in), permiso `Permisos.AbrirMesa`.
- **Reglas:**
  - `AbrirMesa(reservaId, camareroDni)`: la reserva debe existir (`msg_rf2_reserva_inexistente`); si ya está *Asistida* → `msg_rf2_reserva_ya_asistida`; si no es *Confirmada* → `msg_rf2_reserva_no_abrible`; debe ser **de hoy y del turno vigente** (`reserva.Fecha.Date == DateTime.Today` y `reserva.Hora == turno`; si no, `msg_rf2_reserva_fuera_de_turno`); la mesa no puede estar `FueraDeServicio` (`msg_rf2_mesa_fuera_servicio`) ni físicamente ocupada (`msg_rf2_mesa_fisicamente_ocupada`); y la apertura es atómica (`AbrirReserva`: `UPDATE` **condicional con `OUTPUT`**; si otro puesto la abrió antes → `msg_rf2_apertura_concurrente`).
  - `AbrirMesaWalkIn(clienteDni, mesa, comensales, camareroDni)`: exige turno en curso (`msg_rf2_fuera_de_turno`), cliente activo, `ValidarComensales`, `ValidarCamarero`; crea una `Reserva_790MY` ya en estado **Asistida** con `CamareroDNI` y la inserta con `AddSiMesaDisponible(reserva, ocuparMesaFisica: true)` (misma lógica de ocupación; si falla, evento `ReservaRechazadaDisponibilidad`).
  - `ObtenerReservasParaApertura(nroReserva?, clienteDni?)`: nómina de reservas Confirmadas **de hoy del turno vigente** (lista vacía fuera de horario de atención).
  - El cierre de la atención (reserva *Finalizada* y mesa *Libre*) ocurre dentro de la transacción del cobro (`PagoBLL_790MY.RegistrarCobro` → `IPagoRepository_790MY.RegistrarCobro`, §1.5). El antiguo `ReservaBLL_790MY.FinalizarReserva` se eliminó en la refactorización de limpieza: no tenía llamadores.
  - El camarero se valida con `ValidarCamarero` (usa `EmpleadoBLL_790MY.ObtenerPorDni`, `internal`) y `ContarMesasACargo`; se listan solo los activos con `EmpleadoBLL_790MY.GetCamarerosActivos`. La mesa de la reserva se busca con `MesaBLL_790MY.ObtenerPorNumero` (`internal`).
  - Eventos: `ReservaAsistida`, `MesaAbierta`, `WalkInRegistrado`, `CamareroAsignado`.
- **Excepciones:** reglas → `ArgumentException` traducida → banner; rechazo por disponibilidad → `Evento.ReservaRechazadaDisponibilidad`.

#### C. Flujo por capas

```
FormAbrirMesa_790MY
  → ReservaBLL_790MY.ObtenerReservasParaApertura()
  → EmpleadoBLL_790MY.GetCamarerosActivos()
  → ReservaBLL_790MY.AbrirMesa(reservaId, camarero)  |  AbrirMesaWalkIn(cliente, mesa, comensales, camarero)
        → EmpleadoBLL_790MY.ObtenerPorDni · MesaBLL_790MY.ObtenerPorNumero · ClienteBLL_790MY.ExisteActivo   // internal
        → IReservaRepository_790MY.AbrirReserva(...)  (UPDATE condicional + OUTPUT)  // SqlReservaRepository_790MY
        → DVManager_08YS.Recalcular() → BitacoraBLL_08YS.RegistrarEventoPostCommit(...)
BE: Reserva_790MY, Mesa_790MY, Empleado_790MY
```

#### D. Auxiliares e Integración

`EstiloRF2_790MY`, `PanelRedondeado_790MY`, `SlidingTab_UC_790MY`, `Banner_UC_790MY`, `ToastHost_790MY`; claves `AM_*`; `README_RF2_790MY.md` (resumen del RF2 y orden de migraciones). Ayuda contextual: implementa `IAyudaContextual_790MY`.

**Mejoras (1.2):** `FormAbrirMesa_790MY` valida DNI con Regex en la GUI (misma observación que 1.1); el recorrido del `timerReloj_790MY` no se midió.

---

### 1.3 Registrar Pedido (CUN-006)

#### A. Idea y Diseño Visual

- **Objetivo:** el camarero toma el pedido de **sus** mesas asignadas, desde un catálogo con stock visible, y lo envía a cocina.
- **Formulario:** `FormRegistrarPedido_790MY` (`GUI_08YS.RF2`).
- **UI:** catálogo de productos organizado por **pestañas de categoría** (enum `CategoriaProducto_790MY`: Entrada, PlatoPrincipal, Postre, Bebida); una **comanda por mesa** (diccionario `_comandas`); botón de quitar ítem con `IconButton` (`FontAwesome.Sharp`, `IconChar.TrashAlt`); indicador de stock bajo (`UmbralBajoStock = 3`) y tope de cantidad (`MaximoSinControl = 99` para productos que no controlan stock). `timerAvisos_790MY` consulta periódicamente `ObtenerPedidosDeMisMesas` para avisar al camarero cuando un pedido pasa a *Listo*. Banner + toast.

#### B. Lógica y Reglas de Negocio

- **Caso central:** `PedidoBLL_790MY.RegistrarPedido(reservaId, items)` (permiso `RegistrarPedido`).
- **Reglas (en orden de ejecución):**
  1. El usuario de sesión debe ser un camarero activo (`ObtenerCamareroDeSesion`; si no: `msg_rf2_usuario_no_camarero`). Vínculo `Empleado_790MY.UsuarioDNI` (nullable) con el usuario del sistema.
  2. Al menos un ítem (`msg_rf2_pedido_sin_items`); cantidades > 0 (`msg_rf2_pedido_cantidad_invalida`); observaciones ≤ `LargoMaximoObservaciones = 200` (`msg_rf2_pedido_observaciones_largas`).
  3. La reserva debe existir, estar **Asistida** (`msg_rf2_reserva_no_asistida`) y estar **asignada a ese camarero** (`reserva.CamareroDNI == camarero.DNI`, si no `msg_rf2_mesa_no_asignada`).
  4. **El precio lo toma el repositorio dentro de la transacción**, no la GUI: `ItemPedido_790MY.PrecioUnitario` es un *snapshot* del precio vigente al pedir.
  5. Stock: `IPedidoRepository_790MY.RegistrarSiHayStock` bloquea los productos **en orden de ID** (evita deadlocks), verifica `PorcionesDisponibles` (stock menos comprometido por otros pedidos no despachados) y, si todo alcanza, inserta; si no, *rollback* completo. Resultado: `ResultadoRegistroPedido_790MY` con `EstadoRegistroPedido_790MY` (Ok, StockInsuficiente, ProductoNoVigente, ReservaNoAsistida).
- **Excepciones:** `StockInsuficienteException_790MY` (lleva `ProductoID`, `ProductoNombre`, `PorcionesDisponibles`) → la GUI muestra banner y llama a `RecargarCatalogo` para refrescar el stock. Evento `PedidoRechazadoStock`.
- **Importante (para defender):** al registrar el pedido **no se descuenta el stock**: las porciones quedan *comprometidas* (`Pedido_790MY.EstadosConStockComprometido`); el descuento físico ocurre al despachar (§1.4).

#### C. Flujo por capas

```
FormRegistrarPedido_790MY
  → PedidoBLL_790MY.ObtenerMesasAsignadas()   → ReservaBLL_790MY.ObtenerAsistidasPorCamarero (internal)
  → PedidoBLL_790MY.ObtenerCatalogo()         → ProductoBLL_790MY.ObtenerActivos (internal)
  → PedidoBLL_790MY.RegistrarPedido(reservaId, items)
        → EmpleadoBLL_790MY.ObtenerEmpleadoDeSesion · ReservaBLL_790MY.ObtenerPorId   // internal
        → IPedidoRepository_790MY.RegistrarSiHayStock(pedido)   // SqlPedidoRepository_790MY
        → DVManager_08YS.Recalcular() → RegistrarEventoPostCommit(Evento.PedidoRegistrado, "Pedido N - Mesa M")
  → timerAvisos: PedidoBLL_790MY.ObtenerPedidosDeMisMesas()
BE: Pedido_790MY (`EstadoPedido_790MY`: Pendiente/EnPreparacion/Listo), ItemPedido_790MY, Producto_790MY (PorcionesDisponibles, ControlaStock, Admite), Reserva_790MY
```

#### D. Auxiliares e Integración

`StockInsuficienteException_790MY` (en `BLL/Exceptions`), `EstiloRF2_790MY`, `Banner_UC_790MY`, `ToastHost_790MY`; claves `RP_*` (incluye `RP_cat_<Categoria>` que reutiliza la Bitácora de Cambios) y `msg_rf2_*`. Ayuda contextual `IAyudaContextual_790MY`.

**Mejoras (1.3):** el polling del `timerAvisos_790MY` consulta la BD periódicamente (aceptable para un TP, en producción conviene un mecanismo de notificación/`SqlDependency`); `ObtenerCamareroDeSesion` depende de que el vínculo usuario-empleado esté completo.

---

### 1.4 Despachar Pedido / Cocina (KDS) (CUN-008)

#### A. Idea y Diseño Visual

- **Objetivo:** pantalla de cocina tipo *Kitchen Display System*: tres columnas **Pendientes / En preparación / Listas**, con un ticket por comanda y un semáforo de antigüedad.
- **Formulario:** `FormCocinaKDS_790MY` (menú: *Atención › Despachar pedido*, permiso `DespacharPedido`).
- **UI:** tema oscuro propio de cocina (colores KDS en `TemaFogon_790MY`/`EstiloRF2_790MY`); cada comanda es un `TicketKds_790MY` (UserControl con `IconButton`); tipografías propias `FuentesKds_790MY`; `timerKds_790MY` refresca periódicamente. Semáforo (`NivelSemaforo`): **< 10 min verde, 10–20 min ámbar, > 20 min rojo**.
- **Render incremental:** `_tickets` (diccionario `nroPedido → TicketKds_790MY`) y `SincronizarColumna` reutilizan los controles existentes en lugar de recrearlos en cada refresco (decisión de la Etapa 3, "Opción C", con medición por `Stopwatch` documentada en `docs/Etapa 3 — KDS problema, decisiones e hipótesis.pdf`). Métodos: `Recargar`, `Renderizar`, `SincronizarColumna`.

#### B. Lógica y Reglas de Negocio

- `IniciarPreparacion(nroPedido)` → pasa Pendiente → EnPreparacion (el repositorio devuelve `false` si no estaba Pendiente → `msg_rf2_pedido_no_iniciable` con el estado traducido, o `msg_rf2_pedido_inexistente`). Evento `PedidoEnPreparacion`.
- `MarcarPedidoListo(nroPedido)` → **marca Listo y descuenta stock en la misma transacción** (`MarcarListoYDescontarStock(nroPedido, DateTime.Now, out porciones)`). Resultado `ResultadoDespacho_790MY`: Ok / NoEncontrado / YaProcesado (`msg_rf2_pedido_ya_despachado`) / StockFisicoInsuficiente (`msg_rf2_stock_fisico_insuficiente`, evento `StockDescuentoFallido`, Criticidad Alto). **Idempotencia** con el flag `StockDescontado`: reprocesar no vuelve a descontar. Eventos `PedidoListo` y `StockDescontado`.
- Lectura: `ObtenerComandasCocina`, `ObtenerListosDelDia(maximo = 10)`, `ObtenerCamarerosPorDni` (para mostrar el camarero de cada ticket; usa `EmpleadoBLL_790MY.ObtenerCamarerosActivos`, `internal`, para no exigirle al Chef `CrearEmpleado`); todas con `ValidatePermission(DespacharPedido)`.

#### C. Flujo por capas

```
FormCocinaKDS_790MY (timerKds_790MY → Recargar → Renderizar → SincronizarColumna → TicketKds_790MY)
  → PedidoBLL_790MY.ObtenerComandasCocina() / ObtenerListosDelDia() / ObtenerCamarerosPorDni()
  → PedidoBLL_790MY.IniciarPreparacion(nro)         → IPedidoRepository_790MY.IniciarPreparacion
  → PedidoBLL_790MY.MarcarPedidoListo(nro)          → IPedidoRepository_790MY.MarcarListoYDescontarStock (transacción)
  → DVManager_08YS.Recalcular() → BitacoraBLL_08YS.RegistrarEventoPostCommit(...)
BE: Pedido_790MY, ItemPedido_790MY, Producto_790MY, Empleado_790MY
```

#### D. Auxiliares

`TicketKds_790MY`, `FuentesKds_790MY`, `NivelSemaforo`, `TemaFogon_790MY` (tema oscuro), claves `KDS_*`.

**Mejoras (1.4):** el refresco por `timer` y por consulta completa escala mal con muchas comandas (mitigado con el render incremental); no hay sonido/alerta al llegar una comanda nueva (no encontrado en el código leído).

---

### 1.5 Caja y Cobro — Cobrar Venta (CUN-007)

#### A. Idea y Diseño Visual

- **Objetivo:** el cajero elige una mesa con consumos, ve la cuenta, aplica descuento, genera la factura, registra el cobro (efectivo o tarjeta) y el sistema libera la mesa.
- **Formulario:** `FormFacturacionCobro_790MY` (3 paneles: mesas por cobrar / cuenta / cobro) y `FormComprobanteFactura_790MY` (vista previa e impresión).
- **UI:** `SlidingTab_UC_790MY sldMetodo_790MY` (Efectivo | Tarjeta); `BotonCobro_790MY` con *spinner* GDI+ mientras procesa; campos con formato moneda `es-AR`; recálculo en vivo de importes mientras se edita el descuento; banner de rechazo.
- **Comprobante:** `FormComprobanteFactura_790MY` dibuja con **GDI+** sobre `PrintDocument`/`PrintPreviewDialog` (no usa PDF), incluye un **QR simulado** (`DibujarQrSimulado`) y el **hash SHA-256** del comprobante (`FacturaBLL_790MY.CalcularHashComprobante` + `CodigoVerificacion`); el cursor de dibujo es `TicketFactura_790MY` (`internal`, archivo propio en `GUI/RF2`).

#### B. Lógica y Reglas de Negocio

- **Cobrar Venta (CUN-007) — emisión de la factura** — `FacturaBLL_790MY.GenerarFactura(nroReserva, valorDescuento, TipoDescuento_790MY)` (permiso `GenerarFactura`; sobrecarga con `porcentajeDescuento`):
  - Sin consumos → `msg_fc_sin_consumos`. Reserva ya pagada → `msg_fc_reserva_ya_pagada`; reserva no Asistida → `msg_rf2_reserva_no_asistida`.
  - `CalcularImportes` (función **pura**, estática, reutilizada por la GUI para recalcular en vivo): descuento por **porcentaje** (0–100, `msg_fc_descuento_porcentaje_invalido`) o por **monto fijo** (0–subtotal, `msg_fc_descuento_monto_invalido`); `neto = subtotal − descuento`; `IVA = neto × 21 %` (`Factura_790MY.AlicuotaIVA = 0.21`) y `MontoTotal = neto + IVA` (**el IVA se suma sobre el neto**, no está incluido en el precio de carta). Redondeo a 2 decimales (`FacturaBLL_790MY.Redondear`).
  - Si ya hay una factura Abierta **con el mismo contenido**, se reutiliza; si cambió, se **anula** la anterior (`FacturaAnulada`) y se genera una nueva (`FacturaGenerada`). Número de comprobante con formato `"B-0001-00000234"` (`NumeroComprobante`).
- **Cobrar Venta (CUN-007) — registro del pago** — `PagoBLL_790MY.RegistrarCobro(SolicitudCobro_790MY)` (permiso `RegistrarCobro`):
  - La factura debe estar **Abierta** (`msg_fc_factura_no_abierta`); propina ≥ 0 (`msg_fc_propina_invalida`); `MontoAbonado = MontoTotal + propina`.
  - **Efectivo:** `MontoRecibido ≥ MontoAbonado` (`msg_fc_efectivo_insuficiente`), `Cambio = CalcularCambio(abonado, recibido)`. La GUI además limita el recibido a `FactorMaximoRecibido = 5` veces el total y el vuelto a `VueltoMaximo = 100000`.
  - **Tarjeta:** `ValidarTarjetaLuhn` (13–19 dígitos; acepta espacios y guiones; `msg_fc_tarjeta_invalida`), cuotas ∈ {1, 3, 6, 12} (`Pago_790MY.CuotasPermitidas`; `msg_fc_cuotas_invalidas`), marca por prefijo IIN (`DetectarMarca`), número **enmascarado** `**** **** **** 1111` (`EnmascararTarjeta`; **nunca se guarda el número completo**).
  - **Autorización bancaria simulada** (`SolicitarAutorizacionBancaria`): rechaza toda tarjeta que termine en `PagoBLL_790MY.TerminacionRechazoSimulado = "0002"` → `PagoRechazadoException_790MY("FONDOS_INSUFICIENTES", ...)`, evento `PagoRechazado`, **sin cambios en la base**.
  - **Confirmación transaccional** (`IPagoRepository_790MY.RegistrarCobro`, SERIALIZABLE con `UPDLOCK`): en **una sola transacción** inserta el `Pago`, pasa la `Factura` a Pagada, la `Reserva` a Finalizada y la `Mesa` a Libre. Valida que no haya pedidos sin despachar (`ResultadoRegistroCobro_790MY.PedidosSinDespachar` → `msg_fc_pedidos_sin_despachar`). Eventos: `PagoRegistrado`, `ReservaFinalizada`, `MesaEstadoActualizado`.
- **Bloqueo de navegación durante el cobro (E4-D6):** `FormFacturacionCobro_790MY.IniciarProcesamiento/FinalizarProcesamiento` llaman a `FormMDI_08YS.BloquearNavegacion(true/false)`: se deshabilitan menú lateral (incluido cerrar sesión), selector de idioma y cierre de la app (si el usuario intenta cerrar, `FormMDI_FormClosing` cancela y dispara `CierreBloqueado`, que el formulario traduce en un aviso). Mientras tanto se corta la Visita Guiada, las Sugerencias y el panel de Ayuda.
- **Asincronía:** el botón "Registrar cobro" es `async`: `await Task.Run(() => GenerarFactura + RegistrarCobro)` con una latencia simulada `LatenciaSimuladaMs = 2500` para mostrar el estado *Procesando* (el cobro real a un banco tendría esa espera).

#### C. Flujo por capas

```
FormFacturacionCobro_790MY
  → FacturaBLL_790MY.ObtenerMesasParaCobrar() → MesaCobroDTO_790MY
        → EmpleadoBLL_790MY.ObtenerEmpleadoDeSesion / ObtenerCamarerosActivos      // internal
        → ReservaBLL_790MY.ObtenerAsistidasPorCamarero | ObtenerAsistidas           // internal
  → FacturaBLL_790MY.ObtenerDetalleConsumoMesa(nroReserva, ...) → DetalleCuentaDTO_790MY
        → ReservaBLL_790MY.ObtenerPorId · PedidoBLL_790MY.ContarNoDespachados       // internal
  → FacturaBLL_790MY.CalcularImportes(subtotal, valor, tipo)       // estático, sin BD
  → FacturaBLL_790MY.GenerarFactura(...)    → IFacturaRepository_790MY (SqlFacturaRepository_790MY)
  → PagoBLL_790MY.RegistrarCobro(SolicitudCobro_790MY)
        → FacturaBLL_790MY.ObtenerFactura(nro)                                     // público; RegistrarCobro ya habilita Caja
        → IPagoRepository_790MY.RegistrarCobro (SqlPagoRepository_790MY)
  → ResultadoCobroDTO_790MY { Factura, Pago } → FormComprobanteFactura_790MY
BE: Factura_790MY, ItemFactura_790MY, Pago_790MY (MetodoPago: Efectivo/Tarjeta), SolicitudCobro_790MY, CobroArqueo_790MY (un archivo por tipo)
DTOs (BLL, un archivo por clase): MesaCobroDTO_790MY, ImportesCuenta_790MY, DetalleCuentaDTO_790MY, ResultadoCobroDTO_790MY, ReporteArqueoCaja_790MY
```

#### D. Auxiliares

`PagoRechazadoException_790MY`, `BotonCobro_790MY`, `ConfirmModal_790MY`, `FormMDI_08YS.BloquearNavegacion`, `PrintDocument`/`PrintPreviewDialog`, `SHA256`. Claves `CJ_*`, `msg_fc_*`. El cierre de caja y el arqueo están en §3.2.

**Mejoras (1.5):**
1. `LatenciaSimuladaMs = 2500` es solo una demora de demostración: conviene aplicarla únicamente con `ModoDemostracion=true`.
2. La autorización bancaria es **simulada a propósito** (el sistema no tiene pasarela real; se conserva); el número de tarjeta viaja en memoria hasta la BLL (se enmascara antes de persistir, pero no hay integración PCI real).
3. El QR del comprobante es simulado (no apunta a un servicio verificador real).
4. `FormMDI_08YS.BloquearNavegacion` acopla formularios hijos con el MDI (se resuelve con un evento/interfaz de bloqueo).

---

## 2. MAESTROS

Los cuatro ABM de negocio (`FormClientes_790MY`, `FormEmpleados_790MY`, `FormMesas_790MY`, `FormProductos_790MY`) viven en `GUI/Maestros` (namespace `GUI_08YS.Maestros`) y comparten el mismo esqueleto de interfaz, que conviene defender **una sola vez**:

- **Modos de edición:** enum compartido `ModoEdicion_790MY { Reposo, Alta, Modificacion }` (`GUI/Maestros/ModoEdicion_790MY.cs`, `internal`), usado por los cuatro ABM. `EstablecerModo(...)` habilita/deshabilita campos y botones según el modo; en *Reposo* solo se puede navegar la grilla.
- **Decisión E2-D4 (alta continua):** tras un alta, la ficha queda **limpia y con foco** en el primer campo para cargar otro registro; tras una modificación, queda **seleccionado** el registro con los datos ya guardados.
- **Retroalimentación al usuario (decisión E2-D1):** error de regla de negocio → `Banner_UC_790MY` (`bnrAvisos_790MY`) + `ErrorProvider` (`errValidacion_790MY`) en el campo; éxito → `ToastHost_790MY.Mostrar(...)`; baja → `ConfirmModal_790MY.Confirmar(...)`; error inesperado (base de datos, excepción no prevista) → `MessageBox` nativo (`MostrarErrorFatal`).
- **Estilo:** `EstiloMaestros_790MY` (`GUI_08YS.Maestros`) aporta `EstilizarGrilla`, `EstilizarBoton`, `PintarPildora` y `PintarEstado` (píldoras de estado dibujadas en `CellPainting`); el color de encabezado de grilla es `EstiloMaestros_790MY.Encabezado = (110, 26, 46)`. Los formularios RF2 (Empleados, Productos) usan además el alias `E` de `EstiloRF2_790MY`.
- **Importante para la defensa:** los Designers de los cuatro Maestros usan controles **estándar** de WinForms (`Button`, `TextBox`, `DataGridView`, `Panel`, `TableLayoutPanel`, `NumericUpDown`, etc.) más `Banner_UC_790MY` y, en Mesas, `EtiquetaChip_790MY`. **No** usan `FontAwesome.Sharp` (verificado: no hay `IconChar` ni `IconButton` en `GUI/Maestros`). Lo único gráfico propio es `Properties.Resources.FogonIcon` como ícono del formulario (Clientes y Mesas).
- **Seguridad en pantalla:** los botones de acción se muestran según permiso con `PermissionFilter_08YS.Aplicar(this, _mapaPermisos)` (el filtro modifica `Visible`, no `Enabled`); la BLL vuelve a validar con `SessionManager_08YS.Instance.ValidatePermission(...)` (defensa en profundidad: ocultar un botón no es seguridad).
- **Baja lógica** en los cuatro maestros (campo `Activo`, o `Estado = FueraDeServicio` en mesas): nunca se hace `DELETE` físico desde el flujo de negocio, para preservar el historial de reservas, facturas y pedidos.

---

### 2.1 Clientes

#### A. Idea y Diseño Visual

- **Objetivo:** ABM de clientes del restaurante (datos de contacto necesarios para reservar y para enviar el comprobante), con búsqueda inmediata y exportación/importación XML.
- **Formulario:** `FormClientes_790MY` (`GUI_08YS.Maestros`), con `IIdiomaObserver_08YS` e `IAyudaContextual_790MY`. Es el más grande del grupo (≈1050 líneas).
- **UI programada:** grilla `dgvClientes_790MY` + ficha de detalle `pnlDetalle_790MY` (dibujada en `pnlDetalle_790MY_Paint`); caja de búsqueda `txtBuscar_790MY` con *debounce* por `tmrBusqueda_790MY` (`txtBuscar_790MY_TextChanged` reinicia el timer y `tmrBusqueda_790MY_Tick` aplica `AplicarFiltro`); resumen de resultados (`ActualizarResumen`); **etiquetas (chips)** para emails y teléfonos adicionales (`DibujarEtiquetas`, `LlenarEtiquetas`, `AgregarAdicional`, `FlowLayoutPanel`); panel plegable de **serialización XML** (`btnPlegarSerializacion_790MY`, `btnExaminarSerializar_790MY`, `btnExaminarDeserializar_790MY`, `btnSerializar_790MY`, `btnDeserializar_790MY`). Banner con `Banner_UC_790MY bnrAvisos_790MY`.
- **UX:** validación **en vivo y por campo tocado** (`_camposTocados`, `CampoFicha_TextChanged`, `CampoFicha_Leave`, `ActualizarValidacion`) con resumen de errores en el banner (`TextoResumen`); detección de **cambios sin guardar** (`HuellaFicha()` compara contra `_huellaInicial`; `HayCambiosSinGuardar()`); si el usuario cambia de fila con edición pendiente, `ResolverCambioDeFila` ofrece *Guardar* (`btnGuardarCambios_790MY`) o *Descartar* (`btnDescartarCambios_790MY`) y luego `IrAPendiente(dni)`.

#### B. Lógica y Reglas de Negocio

- **Casos de uso:** alta, modificación y baja lógica de clientes. Permisos: `Permisos.CrearCliente` para alta/modificación/baja; `Permisos.VerClientes` para `GetAll`, `Exists` y `GetByDni`.
- **Validaciones en la GUI** (`ValidarFicha`, tres `Regex` estáticas): DNI `^\d{7,8}$` (`FC_val_dni_formato`) y además `> 0` (`msg_fc_dni_positivo`); apellido y nombre obligatorios (`FC_val_apellido`, `FC_val_nombre`); email obligatorio (`msg_fc_email_requerido`) y `^[^@\s]+@[^@\s]+\.[^@\s]+$` (`FC_val_email_formato`); celular obligatorio (`msg_fc_celular_requerido`) y solo dígitos `^\d+$` (`FC_val_celular_numeros`). Los adicionales se validan con `ErrorFormatoAdicional` / `AdicionalesInvalidos`.
- **Validaciones en la BLL** (`ClienteBLL_790MY.ValidarDatos`, privada): `dni > 0`, nombre, apellido y teléfono no vacíos, email con la misma `Regex`. **Unicidad:** `_clienteRepository.Exists(dni)` → `ClienteDniDuplicadoException_790MY(dni)`. *La regla "7–8 dígitos" existe solo en la GUI* (ver mejoras).
- **Baja lógica:** `EliminarCliente(dni)` → `DeleteLogico(dni)` (`UPDATE Clientes SET Activo = 0`); si no existe o ya está de baja: `ArgumentException`. Un cliente dado de baja **sigue ocupando su DNI** (el comentario del repositorio y de la GUI lo aclaran: `Exists()` no filtra por `Activo` a propósito), por eso el alta con ese DNI devuelve "DNI duplicado".
- **Manejo de excepciones en `Guardar()`:** `ClienteDniDuplicadoException_790MY` → banner con `FC_err_dni_duplicado` + `ErrorProvider` en `txtDni_790MY`; `ArgumentException` → banner (`FC_err_bll_guardar`); cualquier otra → `MostrarErrorFatal(FC_fatal_guardar)`. **La BLL no muestra ningún `MessageBox`.**
- **Serialización XML (`ClienteBLL_790MY`):** `SerializarClientesXML(List<Cliente_790MY>)` / `DeserializarClientesXML(string)` / `SerializarArchivoXML(clientes, ruta)` / `DeserializarArchivoXML(ruta)`; usa `XmlSerializer` con un `StringWriter` que fuerza UTF-8 sin BOM (clase interna con `Encoding => new UTF8Encoding(false)`). Se exportan exactamente las **filas seleccionadas** de la grilla; sin selección: aviso `msg_fc_seleccionar_para_exportar`. Los errores de archivo (`ArgumentException`, `IOException`, `UnauthorizedAccessException`) van al banner (`EsErrorDeArchivo`).
- **Auditoría:** eventos `ClienteRegistrado`, `ClienteModificado`, `ClienteEliminado` (Módulo `Clientes`), con el DNI como `targetUsername`. **`ClienteBLL_790MY` usa `RegistrarEvento` (no `RegistrarEventoPostCommit`)**: si la bitácora fallara después del `INSERT`, la excepción llegaría a la GUI aunque el cliente ya se guardó (ver mejoras).

#### C. Flujo de Trabajo por Capas

```
FormClientes_790MY ── BLLFactory_790MY.CreateClienteBLL() ──► ClienteBLL_790MY
  CargarGrilla()                 → GetAll()                      [VerClientes]
  Guardar() (alta)               → RegistrarCliente(dni, nombre, apellido, email, telefono, direccion,
                                                    emailsAdicionales, telefonosAdicionales)  [CrearCliente]
  Guardar() (modificación)       → ActualizarCliente(Cliente_790MY)                          [CrearCliente]
  btnEliminar_790MY_Click        → EliminarCliente(dni)                                      [CrearCliente]
  btnSerializar / btnDeserializar→ SerializarArchivoXML / DeserializarArchivoXML
        │
        ▼  IClienteRepository_790MY  (por interfaz)
SqlClienteRepository_790MY :  Exists, Create, Update, DeleteLogico, GetAll, GetByDni
        │   Create/Update en transacción (BeginTransaction): tabla Clientes +
        │   EmailsAdicionalesCliente + TelefonosAdicionalesCliente (se borran y reinsertan)
        ▼
BE: Cliente_790MY (DNI, Nombre, Apellido, Email, Telefono, Direccion, Activo,
                   EmailsAdicionales : List<string>, TelefonosAdicionales : List<string>)
Después de cada escritura: DVManager_08YS.Recalcular() → BitacoraBLL_08YS.RegistrarEvento(...)
```

- `SqlClienteRepository_790MY.GetAll()` filtra `WHERE Activo = 1` y completa los emails/teléfonos adicionales (consulta por cliente: patrón *N+1*, ver mejoras). `GetByDni` también filtra `Activo = 1`.
- *(Resuelto)* `ClienteMapper_790MY_OBSOLETO` (clase vacía en `MPP_08YS`) se eliminó en la refactorización de limpieza.

#### D. Clases Auxiliares e Integración

`ClienteDniDuplicadoException_790MY`, `FormListaSimple_790MY` (editor de listas de emails/celulares usado por `FormRegistrarCliente_790MY`, no por este formulario: `FormClientes_790MY` replica sus reglas con chips), `XmlSerializer`, `OpenFileDialog`/`SaveFileDialog`, `Banner_UC_790MY`, `ToastHost_790MY`, `ConfirmModal_790MY`, `PermissionFilter_08YS`. Claves de idioma `FC_*` y `msg_fc_*` (55 claves `FC_`). Ayuda contextual: `ObtenerPistas()`.

**Mejoras (2.1):**
1. Llevar la regla **DNI 7–8 dígitos** a `ClienteBLL_790MY.ValidarDatos` (hoy la BLL solo exige `dni > 0`; un cliente creado por otra vía podría tener un DNI de 3 dígitos).
2. `ValidarDatos`, `ActualizarCliente` y `EliminarCliente` lanzan textos **escritos en español dentro de la BLL** (`"El DNI debe ser un número positivo."`, etc.): no pasan por `TraductorManager_08YS`/`MensajesBLL_790MY.Regla`, y en inglés se verían en español.
3. Cambiar `RegistrarEvento` por `RegistrarEventoPostCommit` en `ClienteBLL_790MY` y `MesaBLL_790MY` para ser coherentes con el resto de RF2 (criterio descrito en §0.5).
4. `SqlClienteRepository_790MY.GetAll()` resuelve los adicionales cliente por cliente (`CargarAdicionales`). Es un **N+1 deliberado y comentado en el código** ("el volumen esperado de clientes es chico"); para una defensa conviene decirlo así, y mencionar que con volumen real se reemplaza por dos consultas agregadas.
5. Semántica asimétrica: `Exists(dni)` **no** filtra `Activo` (a propósito, para no reutilizar DNIs de bajas) pero `GetByDni(dni)` y `GetAll()` **sí** filtran `Activo = 1`. Es correcto, pero obliga a que la GUI explique el mensaje de duplicado cuando el cliente "no aparece" en la grilla; considerar la opción de **reactivar** un cliente dado de baja.

---

### 2.2 Empleados

#### A. Idea y Diseño Visual

- **Objetivo:** alta, modificación y baja de empleados (`Camarero`, `Chef`, `Recepcionista`); es el dato que habilita la asignación de camareros a mesas (`FormAbrirMesa_790MY`) y el vínculo opcional con un usuario del sistema.
- **Formulario:** `FormEmpleados_790MY` (`IIdiomaObserver_08YS`, `IAyudaContextual_790MY`), con `TableLayoutPanel` de dos columnas (lista + ficha), grilla `dgvEmpleados_790MY` dibujada en `dgvEmpleados_790MY_CellPainting` (píldora de puesto y de estado con `EstiloMaestros_790MY.PintarPildora/PintarEstado`), filtros (`Filtro_Changed`), `cmbCargo_790MY` cargado con la clase genérica compartida `OpcionCombo_790MY<T>` (`GUI/Estilo`; `CargarCombosFijos`, `SeleccionarPuesto`), `chkActivo_790MY`, `lblAviso_790MY` y placeholder de búsqueda con **P/Invoke** `SendMessage(..., EM_SETCUEBANNER = 0x1501, ...)` (`UpdateIdioma`).
- **UX:** `lblAviso_790MY` muestra un aviso (`FE_aviso_mesas`) cuando el camarero seleccionado tiene mesas abiertas (`ActualizarAvisoMesas` → `ContarMesasAbiertas`); `txtDni_790MY_KeyPress` bloquea cualquier carácter que no sea dígito; `txtDni_790MY.MaxLength = 9`, nombre y apellido `MaxLength = 100`.

#### B. Lógica y Reglas de Negocio

- **Permiso:** `Permisos.CrearEmpleado` para `GetAll`, `RegistrarEmpleado`, `ModificarEmpleado`, `DarDeBaja` y `ContarMesasAbiertas`. `GetCamarerosActivos()` exige `Permisos.AbrirMesa` (lo usa la recepción, no el administrador).
- **`ValidarDatos(Empleado_790MY)`** (privada): objeto no nulo; `DNI > 0` (`msg_rf2_empleado_dni_invalido`); nombre y apellido no vacíos y ≤ `LargoMaximoNombre = 100` (`msg_rf2_empleado_nombre_invalido`); puesto válido con `Enum.IsDefined` (`msg_rf2_empleado_puesto_invalido`); recorta espacios (`Trim`).
- **Unicidad:** `Exists(dni)` → `msg_rf2_empleado_duplicado`. **Vínculo con usuario** (`ValidarUsuarioAsociado`): si `UsuarioDNI` tiene valor, el usuario debe existir (`msg_rf2_usuario_inexistente`) y no estar vinculado a otro empleado (`msg_rf2_usuario_ya_vinculado`).
- **Baja (`DarDeBaja(dni)`):** existe (`msg_rf2_empleado_inexistente`), no está ya inactivo (`msg_rf2_empleado_ya_inactivo`) y **no tiene mesas abiertas**: `ValidarSinMesasAbiertas` cuenta `_reservaRepository.GetAsistidasPorCamarero(dni)` (`msg_rf2_empleado_con_mesas_abiertas`). Luego `Activo = false` y `Update`.
- **Modificación (`ModificarEmpleado`):** además de las validaciones anteriores, si el empleado **era un camarero activo y deja de serlo** (cambia de puesto o queda inactivo) se ejecuta `ValidarSinMesasAbiertas`: sus mesas abiertas quedarían sin responsable para Registrar Pedido.
- **Detalle importante:** la GUI **no envía `UsuarioDNI`** al guardar (el alta queda sin usuario) y `SqlEmpleadoRepository_790MY.Update` **no incluye `UsuarioDNI`** en el `UPDATE` (`SET Nombre, Apellido, Puesto, Activo`): al modificar se conserva el vínculo que ya tenía el empleado (por ejemplo el del administrador).
- **Excepciones:** `catch (Exception ex)` → `E.MostrarErrorEnBanner(bnrAvisos_790MY, ex)`. Éxito: `E.MostrarExitoToast(dgvEmpleados_790MY, exito)`.
- **Auditoría:** `EmpleadoRegistrado`, `EmpleadoModificado`, `EmpleadoDadoDeBaja` (Módulo `Empleados`) con `RegistrarEventoPostCommit` y detalle `"Empleado {dni} ({puesto})"`.

#### C. Flujo de Trabajo por Capas

```
FormEmpleados_790MY ── BLLFactory_790MY.CreateEmpleadoBLL() ──► EmpleadoBLL_790MY
  Cargar()            → GetAll()                      [CrearEmpleado]
  btnGuardar (alta)   → RegistrarEmpleado(Empleado_790MY)
  btnGuardar (modif.) → ModificarEmpleado(Empleado_790MY)
  btnBaja             → DarDeBaja(dni)                (usa IReservaRepository_790MY.GetAsistidasPorCamarero)
  FormAbrirMesa/…     → GetCamarerosActivos()         [AbrirMesa] → GetActivosPorPuesto(Camarero)
        ▼  IEmpleadoRepository_790MY  (+ IUserRepository_08YS, IReservaRepository_790MY)
SqlEmpleadoRepository_790MY : GetAll, GetByDni, Exists, GetActivosPorPuesto, Add, Update, UsuarioVinculadoAOtroEmpleado
        ▼
BE: Empleado_790MY (DNI, Nombre, Apellido, Puesto : PuestoEmpleado_790MY, UsuarioDNI : int?, Activo,
                    NombreCompleto)                 enum PuestoEmpleado_790MY { Camarero, Chef, Recepcionista }
```

#### D. Clases Auxiliares e Integración

`EstiloRF2_790MY` (alias `E`: `T`, `MostrarErrorEnBanner`, `MostrarExitoToast`), `EstiloMaestros_790MY`, `MensajesBLL_790MY.Regla`, `ConfirmModal_790MY`, P/Invoke `user32.SendMessage`. Claves `FE_*` (incluye `FE_puesto_<Puesto>`, que reutiliza la Ficha de Ingredientes de la ayuda) y `msg_rf2_empleado_*`.

**Mejoras (2.2):**
1. La GUI no permite elegir/ver el **usuario vinculado** (`UsuarioDNI`). *(Resuelto B1: se eliminó `EmpleadoBLL_790MY.ObtenerUsuariosVinculables()`, que no se invocaba desde ningún formulario; si se agrega el combo "Usuario del sistema" habrá que reintroducir una consulta equivalente.)*
2. El DNI se valida solo como entero positivo (la GUI admite hasta 9 dígitos por `MaxLength`), a diferencia de Clientes (7–8 dígitos): unificar la regla.
3. `ContarMesasAbiertas` captura `Exception` y devuelve 0 en la GUI (`ActualizarAvisoMesas`): un fallo de base se vuelve silencioso.

---

### 2.3 Mesas

#### A. Idea y Diseño Visual

- **Objetivo:** administrar el salón (hasta 12 mesas, capacidad 1–8) como un **tablero de tarjetas** (orden automático por número, decisión P11 del código) y no como una lista.
- **Formulario:** `FormMesas_790MY` + control propio `MesaCard_790MY` (`GUI_08YS.Maestros`).
- **UI programada:** `flpTablero_790MY` (`FlowLayoutPanel`) con una `MesaCard_790MY` por mesa (se instancian en `CargarTablero` con `new MesaCard_790MY { Mesa = mesa }`, porque dependen de los datos); `Tarjeta_Click` / `MarcarSeleccion` resaltan la elegida; el detalle (`pnlDetalle_790MY`) muestra `txtNumero_790MY` (solo lectura, lo asigna el sistema), `nudCapacidad_790MY` (`NumericUpDown`) y dos `RadioButton` (`rdoDisponible_790MY` / `rdoFueraServicio_790MY`) más `EtiquetaChip_790MY` (RF2) como chips de estado. `TextoEstado` / `TextoCapacidad` traducen. El mapa visual de reserva (`MesaVisualControl_790MY`, §1.1) es **otro** control, no este.
- **UX:** los colores de estado salen de `TemaFogon_790MY` (`OkFondo`, `OcupFondo`, `FueraFondo`, …); al dar de alta, el número se autocompleta con `GetProximoNumero()` (máximo + 1) y el foco va a la capacidad.

#### B. Lógica y Reglas de Negocio

- **Permisos:** `Permisos.VerMesas` (`GetAll`, `GetProximoNumero`, `ObtenerMesasMapa`) y `Permisos.CrearMesa` (`RegistrarMesa`, `ModificarMesa`, `EliminarMesa`).
- **`ValidarDatos(numero, capacidad)`:** `numero > 0`; `capacidad` entre **1 y 8** ("La capacidad de la mesa debe estar entre 1 y 8 personas.").
- **Alta (`RegistrarMesa`):** tope de **12 mesas** (`todasLasMesas.Count >= 12` → `msg_limite_mesas_alcanzado`); número no repetido (`Exists`); crea `new Mesa_790MY(numero, capacidad, EstadoMesa_790MY.Libre)`. *El tope cuenta todas las filas, incluidas las `FueraDeServicio`.*
- **Modificación (`ModificarMesa`):** debe existir. La GUI protege los estados operativos: `EstadoAGuardar()` solo permite pasar a **FueraDeServicio** o volver a **Libre** una mesa que estaba fuera de servicio; si estaba `Ocupada` o `Reservada` conserva ese estado, "que pertenece a otros módulos".
- **Baja (`EliminarMesa`):** integridad referencial por **historial completo**: si **cualquier** reserva (de cualquier fecha/estado) usa la mesa (`_reservaRepository.Buscar(null, null, null, null)` + `Any(r => r.MesaNumero == numero)`) → se rechaza (`msg_error_eliminar_mesa_con_reservas`). Si no tiene historial, **no se hace `DELETE`**: `UpdateEstado(numero, FueraDeServicio)` (baja por estado). Evento `MesaEliminada`.
- **Estados:** `EstadoMesa_790MY { Libre, Ocupada, Reservada, FueraDeServicio }`. Las mesas `Activo = 0` o `FueraDeServicio` nunca aparecen disponibles en el mapa (`SqlMesaRepository_790MY`: `m.Activo = 1 AND m.Estado <> @FueraDeServicio`).
- **Turnos (en la misma BLL):** `Turnos`/`TurnosValidos` (18:00, 20:00, 22:00), `DuracionTurno = 2 h`, `ObtenerTurnoVigente(DateTime)` (estático), `EsTurnoDePrueba`, `ObtenerTurnoVigenteObligatorio` (`msg_rf2_fuera_de_turno`), `ObtenerMesasMapa(fecha, hora, comensales)` (`msg_rf2_fecha_anterior_hoy`, `msg_rf2_comensales_invalidos`, `msg_rf2_turno_invalido`).
- **Excepciones en la GUI:** `ArgumentException` → `bnrAvisos_790MY.Mostrar(...)` (`FM_err_bll_guardar` / `FM_err_bll_baja`); otras → `MostrarErrorFatal`.

#### C. Flujo de Trabajo por Capas

```
FormMesas_790MY ── BLLFactory_790MY.CreateMesaBLL() ──► MesaBLL_790MY
  CargarTablero()        → GetAll()                         [VerMesas]  → MesaCard_790MY por mesa
  PrepararAlta()         → GetProximoNumero()
  btnAplicar (alta)      → RegistrarMesa(numero, capacidad)   [CrearMesa]
  btnAplicar (modif.)    → ModificarMesa(Mesa_790MY)          [CrearMesa]
  btnEliminar            → EliminarMesa(numero)               [CrearMesa]
        ▼  IMesaRepository_790MY (+ IReservaRepository_790MY.Buscar para el historial)
SqlMesaRepository_790MY : GetAll, Exists, Add, Update, UpdateEstado
        ▼
BE: Mesa_790MY (NroMesa, Capacidad, Estado : EstadoMesa_790MY, Activo)
```

#### D. Clases Auxiliares e Integración

`MesaCard_790MY`, `EtiquetaChip_790MY`, `EstiloMaestros_790MY`, `ConfirmModal_790MY`, `ToastHost_790MY`, `FiltroOcupacionMesa_790MY` (DAL, usado por el mapa de disponibilidad), `ModoDemostracion`/`TurnoPrueba_790MY` (App.config). Claves `FM_*` (25) y `msg_fm_*`.

**Mejoras (2.3):**
1. `MesaBLL_790MY` mezcla dos responsabilidades: ABM de mesas **y** reglas de turnos/disponibilidad (`ObtenerMesasMapa`); separar en `TurnoService`/`DisponibilidadBLL`.
2. Varios mensajes están **fijos en español** en la BLL (`"Ya existe una mesa con el número …"`, `"El número de mesa debe ser mayor a cero."`); usar `MensajesBLL_790MY.Regla`. Los dos que usan `GetTexto(...) ?? "texto"` nunca llegan al texto alternativo, porque `GetTexto` devuelve `"[clave]"` (no `null`) si falta la clave (comportamiento visto en `SessionManager_08YS.ValidatePermission`).
3. Los eventos usan `RegistrarEvento` en vez de `RegistrarEventoPostCommit` (misma observación que Clientes).
4. El límite de 12 cuenta mesas `FueraDeServicio`: una mesa dada de baja sigue consumiendo cupo y, como no se puede borrar si tuvo reservas, el salón podría quedar bloqueado.
5. Hay **dos caminos para el mismo resultado**: `ModificarMesa` con el radio *Fuera de servicio* de la ficha y `EliminarMesa` (que solo marca `FueraDeServicio` si la mesa no tiene historial); y `ModificarMesa` no controla si la mesa tiene reservas futuras al pasarla a fuera de servicio.
6. *(Resuelto B1)* Se eliminó el código muerto `IMesaRepository_790MY.Delete` y `TieneReservasActivas` (y su implementación en `SqlMesaRepository_790MY`): la baja usa `UpdateEstado`.

---

### 2.4 Productos y Categorías

#### A. Idea y Diseño Visual

- **Objetivo:** ABM de la **carta**: nombre, categoría, precio, stock en porciones y si el producto **controla stock** o no (bebidas o platos sin límite).
- **Formulario:** `FormProductos_790MY` (RF2, `E = EstiloRF2_790MY`); patente `Permisos.GestionProductos`.
- **Categorías:** **no existe una tabla de categorías**; son el enum `CategoriaProducto_790MY { Entrada, PlatoPrincipal, Postre, Bebida }` guardado como texto en `Productos.Categoria` (el comentario de la entidad aclara que el orden del enum no afecta a los datos). `CargarCombosFijos` arma el combo con `OpcionCombo_790MY<CategoriaProducto_790MY>` y traduce con `RP_cat_<Categoria>`.
- **UI programada:** grilla `dgvProductos_790MY` con `CellPainting` propio: `PintarStock` dibuja el stock con semáforo; `NumericUpDown` para precio (`nudPrecio_790MY`) y stock (`nudStock_790MY`); `chkControlaStock_790MY` (`chkControlaStock_790MY_CheckedChanged` deshabilita el stock si no se controla), `chkActivo_790MY`, `cmbCategoria_790MY`; placeholder con `EM_SETCUEBANNER`; `Banner` + `Toast` + `ConfirmModal`.

#### B. Lógica y Reglas de Negocio

- **`ProductoBLL_790MY.ValidarDatos(producto, exceptoId)`:** nombre no vacío y ≤ `LargoMaximoNombre = 100` (`msg_rf2_producto_nombre_invalido`); categoría válida (`Enum.IsDefined`, `msg_rf2_producto_categoria_invalida`); `0 < precio ≤ PrecioMaximo = 99 999 999,99` (tope de `decimal(10,2)`, `msg_rf2_producto_precio_invalido`); `stock ≥ 0` (`msg_rf2_producto_stock_invalido`); **nombre único** (`ExisteNombre(nombre, exceptoId)`, `msg_rf2_producto_duplicado`).
- **Regla de stock al modificar:** si `ControlaStock` y el nuevo stock es menor que lo **comprometido** por pedidos Pendientes/EnPreparación (`actual.PorcionesComprometidas`), se rechaza (`msg_rf2_producto_stock_bajo_comprometido`): al despachar, el descuento fallaría.
- **Baja lógica (`DarDeBaja`):** `Activo = false`; deja de aparecer en la carta de Registrar Pedido (`GetActivos()` filtra `p.Activo = 1`); si ya estaba inactivo → `msg_rf2_producto_ya_inactivo`.
- **Auditoría:** `ProductoRegistrado`, `ProductoModificado`, `ProductoDadoDeBaja` (Módulo `Stock`, Criticidad `Medio`) con `RegistrarEventoPostCommit` y detalle `"Prod {id} {nombre}"`. **Además**, los triggers de la base guardan **cada versión** del producto (ver §4).
- **Entidad:** `Producto_790MY` expone `PorcionesComprometidas` (calculado en la consulta), `PorcionesDisponibles = StockPorciones − PorcionesComprometidas` y `Admite(cantidad) => !ControlaStock || PorcionesDisponibles >= cantidad`.

#### C. Flujo de Trabajo por Capas

```
FormProductos_790MY ── BLLFactory_790MY.CreateProductoBLL() ──► ProductoBLL_790MY
  Cargar()             → GetAll()                        [GestionProductos]
  btnGuardar (alta)    → RegistrarProducto(Producto_790MY)
  btnGuardar (modif.)  → ModificarProducto(Producto_790MY)
  btnBaja              → DarDeBaja(productoId)
        ▼  IProductoRepository_790MY
SqlProductoRepository_790MY : GetAll, GetActivos, GetById, ExisteNombre, Add, Update
        │   (SelectConComprometidas: une los ítems de pedidos no despachados)
        ▼
BE: Producto_790MY, CategoriaProducto_790MY
Triggers en la BD (Productos → Productos_C): ver §4 · FormBitacoraCambios_790MY
```

#### D. Clases Auxiliares e Integración

`EstiloRF2_790MY`, `EstiloMaestros_790MY.PintarPildora`, `Producto_C_790MY` (versiones), `ProductoCBLL_790MY` (auditoría), `MensajesBLL_790MY`. Migraciones: `CompletarABM_EmpleadosProductos_790MY.sql` (columnas `Categoria` y `ControlaStock`) y `AgregarCategoriaProducto_790MY.sql`. Claves `FP_*` (39) y `msg_rf2_producto_*`.

**Mejoras (2.4):**
1. Sin tabla de categorías: agregar una nueva categoría exige tocar el enum, las claves `RP_cat_*` y la migración; una tabla `Categorias` lo haría configurable (a costa de una FK más).
2. `PrecioMaximo` y los demás límites están duplicados entre BLL, `NumericUpDown.Maximum` y la columna `decimal(10,2)`; centralizar.
3. Cada descuento de stock en cocina dispara el trigger `producto_actualizado` y genera una **versión nueva** en `Productos_C`: el historial de auditoría se llena de versiones por stock (ver mejora 4.1).

---

### 2.5 Gestión de Accesos / Roles

#### A. Idea y Diseño Visual

- **Objetivo:** administrar **Familias** (agrupaciones de permisos, anidables) y **Roles** (conjuntos de familias y permisos) para controlar quién ve y puede hacer qué.
- **Formularios (`GUI_08YS.Admin`):** `FormGestionAcceso_08YS` (grilla + botones, con el enum `TipoEntidad { Familia, Rol }`: **un único formulario sirve para ambas entidades**) y `FormAccesoAM_08YS` (alta/modificación con **dos grillas**: `dgvSeleccionados` y `dgvDisponibles`). `FormGestionAcceso_08YS` recibe un `Action<Form> openChildForm` (en `FormMDI_08YS` es `AbrirSubpantalla`) para mostrar el AM dentro del panel central y volver a la misma instancia.
- **UI:** `IconPictureBox` de FontAwesome cuyo `IconChar` cambia según la entidad (en `FormGestionAcceso_08YS`); títulos y columnas traducidos (`Entidad_Familias`, `Entidad_Roles`, `TraducirGrillaComponentes`); permisos por botón con `PermissionFilter_08YS` y dos diccionarios (`_mapaFamilia`: `CrearFamilias`/`ModificarFamilias`/`EliminarFamilias`; `_mapaRol`: análogo con *Roles*).
- **Antigüedad del código:** son formularios 08YS, **anteriores** a la regla "sin `MessageBox`": informan con `MessageBox.Show` (selección vacía, éxito, conflicto, error). Ver mejoras.

#### B. Lógica y Reglas de Negocio

- **Modelo (patrón Composite, `Service_08YS.Entities.Acceso`):** `AccessComponent_08YS` (abstracta: `ID`, `Nombre`, `GetPermisos()`), hojas `Permiso_08YS` (con `PermisoID`, `Descripcion`) y compuesto `Familia_08YS` (`Hijos`, `Agregar`, `Quitar`, `ReemplazarHijos`; `GetPermisos()` hace `UnionWith` de los hijos, que ignora duplicados). **`Rol_08YS` no hereda de `AccessComponent_08YS`**: es el contenedor raíz (`Componentes`, `Agregar`, `GetPermisos()`), no un componente anidable.
- **Evaluación al agregar un componente** (`AccesoBLL_08YS.EvaluarAgregarComponente(listaActual, candidato)` → `ResultadoEvaluacion_08YS.Tipo`):
  - `Valido`: sin solapamiento de permisos.
  - `SugerenciaReemplazo`: el candidato **engloba por completo** a elementos ya elegidos (todos sus permisos están dentro del candidato) → se propone reemplazarlos (la GUI pide confirmación).
  - `ConflictoIrresoluble`: solapamiento parcial con elementos que el candidato no engloba → error con los nombres de los permisos en conflicto (`bll_composite_conflicto`).
- **Antiduplicación:** `ValidarFamiliaNoExistente` / `ValidarRolNoExistente`: nombre repetido (`NombreDuplicadoException_08YS`, comparación `OrdinalIgnoreCase`) o **conjunto de permisos idéntico** a otro rol/familia (`PermisosDuplicadosException_08YS`, `SetEquals`).
- **Prevención de ciclos:** `FamiliaBLL_08YS.GetComponentesDisponibles(familiaIdExcluir)` excluye la familia que se edita y todas sus **contenedoras directas e indirectas** (`ResolverContenedoras`, recorrido en anchura/BFS), de modo que una familia nunca pueda contenerse a sí misma.
- **Propagación segura:** `FamiliaBLL_08YS.Modificar` ejecuta `ValidarPropagacion`: reemplaza los hijos de la familia en una copia en memoria y verifica, en **todos los ancestros** (familias y roles, vía `_familiaRepo.GetAncestors`), que no aparezcan permisos duplicados entre hermanos (`DetectarConflictosEntreHijos`, comparación par a par con `HashSet.Overlaps`); si hay conflicto lanza `InvalidOperationException` con el detalle (`bll_propagacion_conflicto_*`).
- **Eliminación:** `Eliminar(id)` consulta `IsInUse(id)` y, si está en uso, lanza `ComponenteEnUsoException_08YS`; la GUI lo muestra con `msg_componente_en_uso_error`.
- **Efecto sobre la sesión:** si el rol modificado (o un rol ancestro de la familia modificada) es el del usuario actual, se llama a `SessionManager_08YS.Instance.InvalidarSesion()` → evento `SesionInvalidada` (`FormMDI_08YS.OnSesionInvalidada`) para que el usuario vuelva a iniciar sesión con los permisos nuevos.
- **Permisos de gestión:** `CrearFamilias`, `ModificarFamilias`, `EliminarFamilias`, `CrearRoles`, `ModificarRoles`, `EliminarRoles` (más `VerFamilias`, `VerRoles` para entrar al menú).
- **Auditoría:** `FamiliaCreada/Modificada/Eliminada`, `RolCreado/Modificado/Eliminado` (Módulo `Perfiles`).
- **Autorización en runtime:** `SessionManager_08YS.HasPermission(permiso)` compara `p.Nombre == permiso.ToString()`: **el nombre del valor del enum `Permisos` debe coincidir exacto con `Permisos.Nombre` en la base** (lo aclara el comentario del enum). `ValidatePermission` lanza `UnauthorizedAccessException` con el texto `msg_permiso_denegado`.

#### C. Flujo de Trabajo por Capas

```
FormMDI_08YS (menú Administrativo › Familias / Roles)
  → OpenChildForm(new FormGestionAcceso_08YS(TipoEntidad.Familia|Rol, AbrirSubpantalla))
FormGestionAcceso_08YS ── BLLFactory_08YS.CreateFamiliaBLL() / CreateRolBLL()
  CargarGrilla  → FamiliaBLL_08YS.GetAll() (= _familiaRepo.GetAllRoots())  |  RolBLL_08YS.GetAll()
  Crear / Modificar → AbrirAM(OperacionAM.Alta|Modificacion, …) → FormAccesoAM_08YS
  Eliminar          → FamiliaBLL_08YS.Eliminar(id)  |  RolBLL_08YS.Eliminar(id)
FormAccesoAM_08YS
  disponibles  → GetComponentesDisponibles(familiaIdExcluir?)
  agregar      → AccesoBLL_08YS.EvaluarAgregarComponente(_seleccionados, candidato)
  guardar      → FamiliaBLL_08YS.Crear / Modificar(id, nombre, HashSet<AccessComponent_08YS>)
                 RolBLL_08YS.Crear / Modificar(...)
        ▼  IFamiliaRepository_08YS · IRolRepository_08YS · IPermisoRepository_08YS
           (GetAllRoots, GetAllDictionary, GetAncestors, IsInUse, Create, Modify, Delete, GetAll, GetAllPlano)
BE/Service: Permiso_08YS · Familia_08YS · Rol_08YS · ResultadoEvaluacion_08YS
Después: DVManager_08YS.Recalcular() → BitacoraBLL_08YS.RegistrarEvento(Evento.RolCreado …)
```

#### D. Clases Auxiliares e Integración

`AccessComponentComparer_08YS` (igualdad por ID para los `HashSet`), `ComponenteEnUsoException_08YS`, `NombreDuplicadoException_08YS`, `PermisosDuplicadosException_08YS` (declaradas en `RolBLL_08YS.cs`), `SessionManager_08YS`, `PermissionFilter_08YS` (`Aplicar` para controles y `AplicarMenuStrip` para menús, recursivo), `TraductorManager_08YS`, `DvEncriptador_08YS` + `DVManager_08YS` (los cambios de Permisos/Familias/Roles invalidan los dígitos verificadores). Gestión de usuarios (`FormGestionUsuarios_08YS`, `UserBLL_08YS`: login, bloqueo, alta, cambio de contraseña) corresponde a seguridad base y **no se audita en esta guía**.

**Mejoras (2.5):**
1. **Orden de validaciones:** en `FamiliaBLL_08YS.Crear/Modificar` y `RolBLL_08YS.Crear/Modificar` se validan datos y duplicados **antes** de `ValidatePermission`, y `Eliminar` consulta `IsInUse` también antes. Un usuario sin permiso puede así enterarse de si existe un nombre o un conjunto de permisos. Conviene validar la patente **primero**.
2. Reemplazar los `MessageBox` de `FormGestionAcceso_08YS` y `FormAccesoAM_08YS` por `Banner_UC_790MY`/`ToastHost_790MY`/`ConfirmModal_790MY` (decisión E2-D1).
3. Los mensajes de `ValidarDatosEntrada` (`InvalidOperationException`) viajan a la GUI como texto traducido de la BLL; correcto, pero mezcla tipos de excepción (`InvalidOperationException` para validaciones de entrada) — usar `ArgumentException`.
4. `RolBLL_08YS.Modificar` y `FamiliaBLL_08YS.Modificar` leen `SessionManager_08YS.Instance.Current.Rol` sin comprobar nulos tras la validación de permiso; es seguro hoy porque la patente exige sesión, pero es frágil.
5. `Familia_08YS.Hijos` y `Rol_08YS.Componentes` se exponen como `IReadOnlyList` pero `ReemplazarHijos` permite mutarlos desde la BLL en el cálculo de propagación; aislar la simulación en una copia profunda.

---

## 3. REPORTES

Menú *Reportes* de `FormMDI_08YS` (permiso de visibilidad `Permisos.VerReportes`). Los tres formularios están en `GUI/Reportes` (namespace `GUI_08YS.Reportes`) y abren con `OpenChildForm(new FormReporteReservas_790MY())`, `FormArqueoCaja_790MY` y `FormReportePredictivo_790MY`. Los **dos reportes estándar** son *Reservas* y *Arqueo y Cierre de Caja*; el **reporte inteligente/avanzado** es el *Predictivo* (análisis de canasta).

| Reporte | Formulario | Permiso | Salida |
|---|---|---|---|
| Reservas | `FormReporteReservas_790MY` | `VerReservas` | Grilla + **PDF** (iTextSharp, A4 horizontal) |
| Arqueo y Cierre de Caja | `FormArqueoCaja_790MY` | `GestionCaja` | Tarjetas KPI + grilla + evento de cierre en bitácora |
| Predictivo (inteligente) | `FormReportePredictivo_790MY` | `GestionReportePredictivo` | Grilla de reglas + tarjetas de sugerencias |

---

### 3.1 Reporte de Reservas

#### A. Idea y Diseño Visual

- **Objetivo:** consultar las reservas por rango de fechas, cliente y estado, y **exportar** el resultado a un PDF auditable.
- **UI:** filtros en la parte superior — `dtpDesde_790MY` y `dtpHasta_790MY` (`DateTimePicker` con casilla `Checked`: sin tildar = sin filtro), `txtDniCliente_790MY`, `cboEstado_790MY` (cargado en `CargarEstados` con `OpcionCombo_790MY<EstadoReserva_790MY?>`), botones *Buscar*, *Limpiar*, *Generar PDF* y *Cerrar*; panel descriptivo del resumen de filtros aplicados (`InicializarPanelResumen`, `ActualizarResumenFiltros`) y grilla `dgvReporteReservas_790MY` con formateo de celdas (`dgvReporteReservas_790MY_CellFormatting`). `Banner_UC_790MY` para avisos, `ErrorProvider` para el DNI, `ToastHost_790MY` cuando se guarda el PDF.
- **Librerías:** `iTextSharp.text` / `iTextSharp.text.pdf` para el documento; `System.Diagnostics.Process` para abrirlo; `System.Text.RegularExpressions`.

#### B. Lógica y Reglas de Negocio

- **Caso de uso:** búsqueda filtrada de reservas y emisión de un listado en PDF.
- **Validación (GUI):** el DNI es opcional, pero si se escribe debe cumplir `^\d{7,8}$` y `int.TryParse`; si no, `errValidacion_790MY` + banner (`FC_val_dni_formato`).
- **BLL:** `ReservaBLL_790MY.Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado)` exige `Permisos.VerReservas` y delega en `IReservaRepository_790MY.Buscar(...)`. **No valida** que `desde ≤ hasta`.
- **Estados:** `EstadoReserva_790MY` (Confirmada, Cancelada, Asistida, Finalizada), traducidos por `TextoEstado`.
- **Manejo de excepciones (decisión E2-D1):** `ArgumentException` o `UnauthorizedAccessException` → banner (`msg_rep_error_buscar`); cualquier otra → `MessageBox` nativo. En el PDF: errores de archivo (`IOException`, `UnauthorizedAccessException`, `ArgumentException`) → banner (`msg_rep_pdf_error`) para que el usuario elija otra ruta; error inesperado → `MessageBox` (`REP_fatal_pdf`).
- **Botón PDF:** `btnGenerarPDF_790MY.Enabled` solo si hay resultados; sin datos, aviso `msg_rep_sin_datos_pdf`. Nombre sugerido: `ReporteReservas_yyyyMMdd_HHmm.pdf` (`SaveFileDialog`).

#### C. Flujo de Trabajo por Capas

```
FormReporteReservas_790MY ── BLLFactory_790MY.CreateReservaBLL() ──► ReservaBLL_790MY
  btnBuscar_790MY_Click → BuscarReservas()
        → Buscar(desde, hasta, dni, estado)              [VerReservas]
              ▼  IReservaRepository_790MY.Buscar(...)
        SqlReservaRepository_790MY.Buscar(...)  →  List<Reserva_790MY>  (_ultimoResultado)
  btnGenerarPDF_790MY_Click → GenerarPDF(rutaArchivo)    (usa _ultimoResultado y los filtros guardados)
BE: Reserva_790MY (+ EstadoReserva_790MY)
```

#### D. Clases Auxiliares e Integración

- **PDF (`GenerarPDF`)**: `Document(PageSize.A4.Rotate(), 30f, 30f, 40f, 30f)` (A4 horizontal). Paleta bordó `BaseColor(120, 25, 25)`, gris y crema; fuente `Helvetica` en varios tamaños.
  - *Encabezado:* tabla de 2 columnas — **logo** `Resources/logo_fogon.png` leído de `AppDomain.CurrentDomain.BaseDirectory` (si no existe, se imprime el texto "El Fogón del Sur") y bloque de **auditoría** con *Generado por* (`{Nombre} {Apellido} ({Rol})` tomado de `SessionManager_08YS.Instance.Current`) y *Fecha de emisión* (`DateTime.Now`).
  - *Filtros aplicados:* tabla de 4 columnas con los filtros usados en la búsqueda (`_filtroDesde`, `_filtroHasta`, `_filtroDni`).
  - *Cuerpo:* `PdfPTable(8)` con encabezado bordó y filas cebradas (blanco/crema): N°, DNI, Cliente, Mesa, Fecha, Hora, Comensales, Estado.
  - *Pie:* párrafo alineado a la derecha con el **total de registros** (`REP_pdf_total`).
  - Al guardar se abre con `Process.Start(new ProcessStartInfo(archivo){ UseShellExecute = true })`; si no hay visor, el archivo ya quedó guardado (el `catch` es vacío a propósito).
- **Traducciones:** claves `REP_*` (32) y `msg_rep_*`; los textos del PDF también salen de `TraductorManager_08YS` (el PDF se genera en el idioma activo).
- **Dependencias externas:** iTextSharp 5.5.13.3 + Portable.BouncyCastle (con *binding redirect* en `App.config`).

**Mejoras (3.1):**
1. Validar `desde ≤ hasta` (en la GUI y en `ReservaBLL_790MY.Buscar`); hoy un rango invertido devuelve una grilla vacía sin explicación.
2. El PDF **no tiene paginación ni encabezado repetido** (no hay `PdfPageEvent` ni número de página, a diferencia del comprobante de reserva que sí usa `PieComprobante : PdfPageEventHelper`); con muchas filas, las páginas siguientes pierden el contexto.
3. `catch (Exception) { }` vacío al abrir el PDF: acotarlo a `Win32Exception`.
4. El PDF se arma dentro del formulario (`GenerarPDF`, ≈200 líneas): conviene extraerlo a una clase de servicio `ReporteReservasPdf_790MY` para poder probarlo y reutilizarlo, igual que el comprobante vive en `ReservaBLL_790MY.Comprobante.cs`.

---

### 3.2 Arqueo y Cierre de Caja

#### A. Idea y Diseño Visual

- **Objetivo:** reporte gerencial del día: cuánto se cobró en efectivo y en tarjeta, propinas, descuentos y **recaudación neta**, con el detalle de las cuentas cobradas; permite **cerrar la caja** del día.
- **Formulario:** `FormArqueoCaja_790MY`. Barra de fecha con `dtpFecha_790MY` + botones anterior/siguiente (`btnAnterior_790MY`, `btnSiguiente_790MY`; el siguiente se deshabilita en la fecha de hoy).
- **UI programada:** 5 **tarjetas KPI** creadas en `CrearTarjetasKpi()` a partir de la lista `_kpis` (clase interna `Kpi`): cada una es un `PanelRedondeado_790MY` (`Radio = 10`) con un `IconPictureBox` de **FontAwesome.Sharp** y dos `Label` (`EstiloRF2_790MY.CrearLabel`):

  | KPI | Clave | `IconChar` |
  |---|---|---|
  | Efectivo | `AC_kpi_efectivo` | `MoneyBill` |
  | Tarjetas | `AC_kpi_tarjetas` | `CreditCard` |
  | Propinas | `AC_kpi_propinas` | `Heart` |
  | Descuentos (en rojo) | `AC_kpi_descuentos` | `Percent` |
  | Recaudación neta (destacada, fondo bordó) | `AC_kpi_neta` | `ArrowTrendUp` |

  Debajo, la grilla `dgvCobros_790MY` (hora, mesa, método, descuento, propina, importe; los descuentos en rojo `RojoImporte`), `lblResumen_790MY` ("fecha · N cuentas · ticket promedio") y un mensaje de "sin cobros" ubicado por `UbicarMensajeVacio`. Importes con cultura **es-AR** (`Moneda`).
- **UX:** al abrir un día ya cerrado, banner de aviso (`AvisarSiCajaCerrada`, `msg_ac_caja_ya_cerrada`); `ConfirmModal_790MY` antes de cerrar.

#### B. Lógica y Reglas de Negocio

- **Permiso:** `Permisos.GestionCaja` (lo valida `PagoBLL_790MY.GenerarReporteArqueoCaja` y `EstaCajaCerrada`).
- **Arqueo (`ReporteArqueoCaja_790MY`):** todo es **calculado** a partir de la lista `Cobros : List<CobroArqueo_790MY>`:
  - `TotalEfectivo` = suma de `ImporteCobrado` con `MetodoPago_790MY.Efectivo`; `TotalTarjetas` = ídem con `Tarjeta`.
  - `TotalPropinas` = Σ `MontoPropina`; `TotalDescuentos` = Σ `MontoDescuento`.
  - `RecaudacionNeta = TotalEfectivo + TotalTarjetas` ("descuentos deducidos, IVA incluido; las propinas se informan aparte").
  - `CantidadCuentas = Cobros.Count`; `TicketPromedio = Round(RecaudacionNeta / Cobros.Count, 2, AwayFromZero)` (0 si no hay cobros).
  - `ImporteCobrado` de cada cobro = total de la factura (descuento deducido, IVA incluido, **sin propina**).
- **Cierre (`PagoBLL_790MY.CerrarCaja(fecha)`):** recalcula el arqueo; si no hay cobros → `msg_ac_sin_cobros`; si ya se cerró → `msg_ac_caja_ya_cerrada` (**una sola vez por día**); si todo está bien, registra `Evento.CierreCajaRealizado` (Módulo `Facturacion`, Criticidad `Alto`) con el detalle `"Caja dd/MM/yyyy - {RecaudacionNeta:N2}"`. **El cierre no modifica pagos ni facturas.**
- **Cómo se sabe que una caja está cerrada:** el cierre **no tiene tabla propia**; `EstaCajaCerrada(fecha)` consulta la bitácora con `BitacoraFiltro_08YS { Evento = CierreCajaRealizado, TargetUsername = prefijo }` y comprueba que algún `TargetUsername` empiece con el prefijo `"Caja dd/MM/yyyy - "` (`PrefijoCierre`, fecha con cultura invariante para no depender del idioma).
- **Validación defensiva en la GUI:** `btnCerrarCaja_790MY_Click` vuelve a consultar `EstaCajaCerrada` antes de abrir el modal (otra terminal pudo haber cerrado). `btnCerrarCaja_790MY.Enabled = r.CantidadCuentas > 0 && !_cajaCerrada`.
- **Excepciones:** `E.MostrarErrorEnBanner(bnrAvisos_790MY, ex)`; éxito: `E.MostrarExitoToast(this, AC_toast_cierre ...)`.

#### C. Flujo de Trabajo por Capas

```
FormArqueoCaja_790MY ── BLLFactory_790MY.CreatePagoBLL() ──► PagoBLL_790MY
  Cargar()                 → GenerarReporteArqueoCaja(fecha)  [GestionCaja]
                           → EstaCajaCerrada(fecha)
        ▼  IPagoRepository_790MY.ObtenerCobrosDelDia(fecha)
  SqlPagoRepository_790MY  :  SELECT p.IdPago, p.NroFactura, p.FechaHoraPago, r.MesaNumero, p.MetodoPago,
                                     f.MontoDescuento, p.MontoPropina, f.MontoTotal
                              FROM Pagos ⨝ Facturas ⨝ Reservas  WHERE FechaHoraPago ∈ [fecha, fecha+1) AND f.Estado = Pagada
        ▼
  BE: CobroArqueo_790MY (IdPago, NroFactura, FechaHoraPago, MesaNumero, MetodoPago, MontoDescuento,
                         MontoPropina, ImporteCobrado)   →   DTO: ReporteArqueoCaja_790MY (BLL/ReporteArqueoCaja_790MY.cs)
  btnCerrarCaja_790MY_Click → ConfirmModal_790MY.Confirmar → PagoBLL_790MY.CerrarCaja(fecha)
        → BitacoraBLL_08YS.RegistrarEventoPostCommit(Evento.CierreCajaRealizado, "Caja … - importe")
  EstaCajaCerrada → BitacoraBLL_08YS.Filtrar(BitacoraFiltro_08YS) → SqlBitacoraRepository_08YS.Filtrar
```

#### D. Clases Auxiliares e Integración

`PanelRedondeado_790MY`, `IconPictureBox` (FontAwesome.Sharp 6.6.0), `EstiloRF2_790MY` (`CrearLabel`, `T`, `MostrarErrorEnBanner`, `MostrarExitoToast`), `ConfirmModal_790MY`, `BitacoraBLL_08YS`, `CultureInfo` es-AR para el formato `N2`. Claves `AC_*` (19) y `msg_ac_*`. Alimentado por el flujo de §1.5 (`PagoBLL_790MY.RegistrarCobro`).

**Mejoras (3.2):**
1. **Acoplamiento a texto:** el estado "caja cerrada" depende del *prefijo de un string* guardado en `Bitacora.TargetUsername` (limitado a 50 caracteres; `BitacoraBLL_08YS.RegistrarEvento` trunca). Es ingenioso porque evita una tabla nueva, pero si cambia el formato, o si se depura la bitácora, se pierde el cierre. Alternativa: tabla `CierresCaja(Fecha PK, Usuario, Neta)`.
2. `CerrarCaja` hace dos pasos no atómicos (comprobar `EstaCajaCerrada` + registrar el evento): dos terminales simultáneas podrían cerrar la misma caja dos veces. Una restricción única en una tabla propia lo resuelve.
3. `CrearTarjetasKpi()` instancia controles visuales en `FormX.cs` (ver §0.9); podría vivir en un `UserControl` `TarjetaKpi_UC_790MY` declarado en el Designer.
4. El arqueo suma por método sin separar **propinas en tarjeta vs. efectivo**: la caja física no incluye propinas de tarjeta; si se necesita el arqueo físico real, agregarlo.

---

### 3.3 Reporte Predictivo (Inteligente)

#### A. Idea y Diseño Visual

- **Objetivo:** reporte "inteligente" basado en **Market Basket Analysis** (reglas de asociación): descubre qué productos se piden juntos en la misma cuenta y propone hasta **3 acciones comerciales**.
- **Formulario:** `FormReportePredictivo_790MY` (+ control propio `SliderBordo_790MY`, namespace `GUI_08YS.Reportes`).
- **UI programada:**
  - `SliderBordo_790MY` (`Control` propio, **reemplaza al `TrackBar`** porque este no admite colores): `Minimum = 10`, `Maximum = 95`, `Step = 5`, `Value = 40` (confianza mínima %), `ColorPista`/`ColorRelleno` (bordó `107, 29, 47`), evento `ValueChanged` → `sldConfianza_790MY_ValueChanged` → `RecargarReglas()`.
  - Alternador de orden **Por confianza / Por soporte** (`btnPorConfianza_790MY`, `btnPorSoporte_790MY`; `EstilizarToggle` pinta el activo).
  - Grilla `dgvReglas_790MY` con `CellPainting` que dibuja una **barra de soporte** proporcional (`_soporteMaximo`) y un tooltip por regla con `RI_tooltip_regla` (cuentas con A y B, cuentas con A, **lift**).
  - **Tarjetas de sugerencias** (`tlpSugerencias_790MY`): `CrearTarjeta` arma un `PanelRedondeado_790MY` con título, descripción y un botón por sugerencia; `Redactar` compone el texto en el idioma activo (claves `RI_sug_*`), incluyendo `NumeroEnPalabras` y `DiaEnPlural`.
  - Subtítulo en vivo "últimos 90 días · N cuentas analizadas".

#### B. Lógica y Reglas de Negocio

- **Fuente de datos:** facturas **Pagadas** de los últimos `DiasAnalisis = 90` días. Cada factura es una *canasta* (`CanastaFactura_790MY`: `NroFactura`, `FechaEmision`, `Productos : Dictionary<int, string>` — ProductoID → descripción congelada). `SqlFacturaRepository_790MY.ObtenerCanastasPagadas(desde)`: `Facturas ⨝ ItemsFactura`, `WHERE f.Estado = 'Pagada' AND f.FechaEmision >= @Desde`, `GROUP BY NroFactura, FechaEmision, ProductoID` (productos **distintos** por cuenta). Se analizan siempre datos reales, con o sin `ModoDemostracion`.
- **Umbrales (constantes públicas de `ReportePredictivoBLL_790MY`):**

  | Constante | Valor | Significado |
  |---|---|---|
  | `DiasAnalisis` | 90 | Ventana de análisis |
  | `MinimoCuentasReales` | 10 | Menos cuentas → `DatosSuficientes = false`, sin reglas |
  | `SoporteMinimo` | 3 % | Frecuencia mínima del par |
  | `MinimoCuentasPar` (privada) | 2 | El par debe aparecer en ≥ 2 cuentas |
  | `LiftMinimo` | 1,2 | Descarta pares que coinciden solo porque B es muy pedido |
  | `ConfianzaMinimaSugerencias` | 40 % | Confianza mínima de las reglas que generan sugerencias |

- **Métricas** (`CalcularReglas`, `internal static`, recorrido de una sola pasada con `cuentasProducto` y `cuentasPar`):
  - **Soporte %** = cuentas con A y B / total de cuentas.
  - **Confianza %** = cuentas con A y B / cuentas con A; de las dos direcciones (A→B o B→A) **se conserva la de mayor confianza** (en empate, desempata por nombre con `string.CompareOrdinal`).
  - **Lift** = (cuentas con A y B · total) / (cuentas con A · cuentas con B), redondeado a 2 decimales; > 1 indica asociación real y no azar.
  - Los porcentajes se redondean a 1 decimal (`MidpointRounding.AwayFromZero`).
- **Sugerencias** (`ObtenerSugerenciasInteligentes`, `TipoSugerencia_790MY`): (1) `CrearCombo` con la regla de mayor confianza; (2) `PromocionDiaBajaDemanda` con la siguiente regla que **no comparta productos** con el combo (si no hay, cualquiera), promocionada el día de menor demanda (`DiaDeMenorDemanda`: cuentas por día de la semana **normalizadas por la cantidad de veces que ese día aparece en el período**); (3) `VentaCruzadaCamarero` con la regla restante de mayor soporte.
- **Acción simulada:** `BotonSugerencia_Click` abre `ConfirmModal_790MY` indicando de antemano que la acción es **simulada** (`RI_msg_simulada`) y, si se confirma, muestra un toast; **no modifica la carta ni las comandas**.
- **Permiso:** `Permisos.GestionReportePredictivo` (`ValidarAcceso()` en cada método público). Migración `AgregarPatenteReportePredictivo_790MY.sql`.
- **Excepciones:** `ObtenerReglasAsociacion` lanza `ArgumentOutOfRangeException` si la confianza no está en 0–100; la GUI captura `Exception` → `E.MostrarErrorEnBanner(bnrAvisos_790MY, ex)` y deja las listas vacías.

#### C. Flujo de Trabajo por Capas

```
FormReportePredictivo_790MY ── BLLFactory_790MY.CreateReportePredictivoBLL() ──► ReportePredictivoBLL_790MY
  Cargar()  (Load)
     → ObtenerResumen()                          → ResumenCanasta_790MY { CuentasAnalizadas, DiasAnalizados, Desde, DatosSuficientes }
     → ObtenerReglasAsociacion(confianzaMinima, OrdenReglas_790MY.PorConfianza|PorSoporte) → List<ReglaAsociacion_790MY>
     → ObtenerSugerenciasInteligentes()          → List<SugerenciaInteligente_790MY>
  slider / toggle → RecargarReglas() → ObtenerReglasAsociacion(...)      (las sugerencias no se recalculan)
        ▼ (primera consulta) Analizar() → IFacturaRepository_790MY.ObtenerCanastasPagadas(desde)
  SqlFacturaRepository_790MY : Facturas ⨝ ItemsFactura (Estado = Pagada)
        ▼
  BE: CanastaFactura_790MY · Factura_790MY / ItemFactura_790MY (origen de los datos)
  BLL: ReglaAsociacion_790MY · SugerenciaInteligente_790MY · TipoSugerencia_790MY · ResumenCanasta_790MY
```

La BLL **guarda en memoria** `_canastas`, `_reglas` y `_resumen` (se calculan una vez por instancia, `Analizar()`); `Actualizar()` los invalida para releer la base.

#### D. Clases Auxiliares e Integración

`SliderBordo_790MY`, `PanelRedondeado_790MY`, `EstiloRF2_790MY` (`T`, `CrearLabel`, `MostrarErrorEnBanner`, `MostrarExitoToast`), `ConfirmModal_790MY`. Implementación **propia, sin librerías externas** (el comentario de la clase lo justifica: con la carta de un restaurante, el conteo de pares por canasta es inmediato). Textos con claves `RI_*` (43). `IAyudaContextual_790MY` implementada (`ObtenerPistas`).

**Mejoras (3.3):**
1. `CalcularReglas` es `internal static` y puro: **ideal para pruebas unitarias** (pasar canastas armadas a mano y verificar soporte/confianza/lift). No se encontró ningún proyecto de pruebas en la solución.
2. Coste de memoria/CPU: con N productos por cuenta el conteo de pares es O(N²) por canasta; aceptable para una carta de decenas de productos, pero habría que acotar si el catálogo crece.
3. El estado cacheado en la instancia de la BLL (`_reglas`, `_canastas`) no es seguro para uso concurrente; hoy cada formulario crea su propia BLL, así que no hay problema real.
4. `ObtenerSugerenciasInteligentes` vuelve a llamar a `ObtenerReglasAsociacion`, que revalida el permiso y reordena; menor, pero redundante.
5. No existe un botón "Actualizar" en la pantalla: los datos se leen al abrir el formulario (hay que cerrar y volver a entrar para incorporar cobros nuevos).

---

## 4. BITÁCORA DE CAMBIOS

El sistema tiene **dos bitácoras distintas**; conviene no confundirlas en la defensa:

| | Bitácora del sistema (auditoría de eventos) | Bitácora de Cambios (versionado de productos) |
|---|---|---|
| Formulario | `FormBitacora_08YS` (`GUI/Admin`, namespace `GUI`) | `FormBitacoraCambios_790MY` (`GUI/Admin`, namespace `GUI_08YS.Admin`) |
| Qué registra | **Quién hizo qué** (login, altas, cobros, cierres…) | **Qué valores tenía cada producto** en cada momento |
| Almacenamiento | Tabla `Bitacora`, escrita desde la BLL | Tabla espejo `Productos_C`, escrita por **triggers** de SQL Server |
| Permiso | `VerBitacora` | `AuditarProductos` |
| Reversible | No (es un registro) | **Sí**: rollback a una versión anterior |

El área pedida es la *Bitácora de Cambios* (4.1). La bitácora del sistema (4.2) se documenta porque la primera **depende de ella** (cada rollback genera un evento) y porque es la que todas las BLL usan.

---

### 4.1 Bitácora de Cambios de Productos (`FormBitacoraCambios_790MY`)

#### A. Idea y Diseño Visual

- **Objetivo:** auditar el historial de **cada producto** (precio, stock, estado, categoría, nombre, control de stock), comparar una versión pasada contra la vigente y **restaurarla** (rollback) sin perder el historial.
- **Diseño (tarjetas sobre fondo crema, coherente con `TemaFogon_790MY` y los formularios RF2):**
  - Cabecera `pnlCabecera_790MY` (`PanelRedondeado_790MY`): `lblTitulo_790MY`, `lblSubtitulo_790MY`, `lblSesion_790MY` (usuario/rol de la sesión) y `lblPermiso_790MY`.
  - Filtros `pnlFiltros_790MY`: `txtCodigo_790MY`, `txtNombre_790MY`, `dtpDesde_790MY`, `dtpHasta_790MY` (con casilla `Checked`), botones `btnAplicar_790MY` y `btnLimpiar_790MY`. Enter aplica el filtro (`Filtro_KeyDown`); el código solo admite dígitos (`txtCodigo_790MY_KeyPress`).
  - Centro `tlpCentro_790MY`: a la izquierda la **grilla del historial** `dgvHistorial_790MY` (columnas `colID`, `colCodigo`, `colFecha`, `colHora`, `colNombre`, `colCategoria`, `colPrecio`, `colStock`, `colActivo`, `colVigente`), con la leyenda `lblLeyenda_790MY` y el mensaje `lblSinRegistros_790MY` (posicionado por `UbicarMensajeVacio`); a la derecha el **panel de detalle** `pnlDetalle_790MY` con la tabla comparativa `tlpDiff_790MY`.
  - **Pintado propio:** `dgvHistorial_790MY_CellPainting` dibuja "píldoras" (`PintarPildora`) para *Activo/Baja* y *Vigente*; `tlpDiff_790MY_CellPaint` resalta las filas que cambian; `RowPostPaint` marca la fila vigente.
  - `btnRestaurar_790MY`: botón con **ícono FontAwesome.Sharp** (`IconChar.RotateLeft`), color naranja fijo `(184, 84, 20)` cuando está habilitado y gris (`E.Deshabilitado`) si no; atajo **Alt+R** (`ProcessCmdKey`).
- **Feedback:** `Banner_UC_790MY` (`bnrAvisos_790MY`) para errores, `ErrorProvider` (`errValidacion_790MY`) para el código, `ConfirmModal_790MY` (foco inicial en *Cancelar*) y `ToastHost_790MY` para el éxito.

#### B. Lógica y Reglas de Negocio

- **Caso de uso:** *Auditar productos*. El auditor filtra, elige una versión, ve el diff contra la vigente y, si corresponde, restaura.
- **Filtros (`ProductoCBLL_790MY.ConsultarHistorial`):** sin fechas se usan los **últimos 30 días** (`DiasPorDefecto = 30`; `FechaDesdePorDefecto`); si falta solo una de las dos, se completa igual. `desde > hasta` → `BC_msg_rango_invalido`; código `≤ 0` → `BC_msg_codigo_invalido` (la GUI lo valida primero con `TryParse` y lo marca en el `ErrorProvider`).
- **Diff (`ObtenerDiferencias` → `DiferenciaVersion_790MY`):** calcula `DeltaPrecio`, `DeltaStock` y las banderas `CambiaPrecio`, `CambiaStock`, `CambiaEstado`, `CambiaCategoria`, `CambiaNombre`, `CambiaControlaStock`; `EsVigente` cuando la versión elegida es la vigente; `CantidadCambios` resume el total. Si ambas versiones no son del mismo producto → `ArgumentException`.
- **Rollback (`RestaurarVersion(Producto_C_790MY)`)** — reglas en orden:
  1. `ValidatePermission(Permisos.AuditarProductos)`.
  2. `versionHistorica` nula → `ArgumentNullException`; `Act = true` → `BC_msg_ya_vigente`.
  3. **Relee** la versión de la base (`ObtenerPorId`) porque la grilla puede estar vieja (otro rollback, una edición, un descuento de stock en el medio) → `BC_msg_version_inexistente` / `BC_msg_ya_vigente`.
  4. El producto debe seguir existiendo en `Productos` (`ProductoBLL_790MY.ObtenerPorId`, `internal`) → si no, `BC_msg_producto_eliminado`.
  5. **Mismas reglas que `ProductoBLL_790MY.ModificarProducto`:** nombre no duplicado (`ProductoBLL_790MY.ExisteNombre`, `internal`, `msg_rf2_producto_duplicado`) y, si controla stock, el stock restaurado no puede ser menor que las `PorcionesComprometidas` por pedidos abiertos (`msg_rf2_producto_stock_bajo_comprometido`).
  6. `ActivarVersion(version.ID)` → recalcula DV (`DVManager_08YS.Recalcular()`) → `RegistrarEventoPostCommit(Evento.ProductoVersionRestaurada, "Prod {ProductoID} v{ID} {Nombre}")` (módulo `Stock`, criticidad **Alto**).
- **El historial no se borra:** restaurar crea una **fila vigente nueva**; por eso el propio rollback se puede deshacer restaurando la versión que estaba vigente antes.
- **Excepciones:** la BLL solo lanza (`MensajesBLL_790MY.Regla` → `ArgumentException`, `UnauthorizedAccessException`); la GUI las muestra con `E.MostrarErrorEnBanner`. Tras un error de rollback se **recarga** la grilla antes de mostrar el banner (`Cargar` limpia el banner al empezar). No hay `MessageBox`.

#### C. Flujo de Trabajo por Capas

```
Alta/edición/baja/descuento de stock de un producto
   ProductoBLL_790MY / FacturaBLL … → SqlProductoRepository_790MY (INSERT/UPDATE/DELETE en Productos)
          ▼  triggers de SQL Server (Migraciones_790MY/AgregarBitacoraCambiosProductos_790MY.sql)
   producto_actualizado   AFTER INSERT, UPDATE ON Productos  : Act=0 a la versión vigente + INSERT nueva fila Act=1 (GETDATE())
   producto_eliminado     AFTER DELETE ON Productos          : Act=0 a todas las versiones del producto
   producto_c_actualizado AFTER UPDATE ON Productos_C        : si Act pasa 0→1 → MERGE en Productos con los datos de esa versión

Consulta:
FormBitacoraCambios_790MY ── BLLFactory_790MY.CreateProductoCBLL() ──► ProductoCBLL_790MY
   Cargar()                → ConsultarHistorial(codigo, nombre, desde, hasta)   [AuditarProductos]
                                 ▼ IProductoCRepository_790MY.ObtenerHistorial(...)
                           SqlProductoCRepository_790MY (WHERE dinámico: ProductoID, Nombre LIKE con EscaparLike,
                                                         Fecha >= @Desde, Fecha < @Hasta+1día; ORDER BY Fecha DESC, ID DESC)
   MostrarDetalle()        → ObtenerVigente(productoID) + ObtenerDiferencias(sel, vigente)  → ArmarTablaDiff()
Rollback:
   btnRestaurar_790MY_Click → ConfirmModal_790MY.Confirmar → ProductoCBLL_790MY.RestaurarVersion(sel)
        → ObtenerPorId · ProductoBLL_790MY.ObtenerPorId/ExisteNombre (internal)
        → SqlProductoCRepository_790MY.ActivarVersion(id):  UPDATE Productos_C SET Act = 1 WHERE ID = @ID AND Act = 0
              └─ dispara los 3 triggers en cadena (una sola sentencia, una sola transacción implícita)
        → DVManager_08YS.Recalcular() → BitacoraBLL_08YS.RegistrarEventoPostCommit(Evento.ProductoVersionRestaurada, …)
BE: Producto_C_790MY { ID, ProductoID, Nombre, Categoria (CategoriaProducto_790MY), Precio, StockPorciones,
                       ControlaStock, Activo, Fecha, Act }   ·   DTO: DiferenciaVersion_790MY
```

#### D. Clases Auxiliares e Integración

- **Tabla `Productos_C`** (`ID` identity, `ProductoID`, `Nombre`, `Categoria`, `Precio`, `StockPorciones`, `ControlaStock`, `Activo`, `Fecha`, `Act`). La **FK a `Productos` queda en `NOCHECK`** a propósito: permite conservar el historial aunque se borre físicamente el producto. Índices `IX_Productos_C_ProductoID_Act` e `IX_Productos_C_Fecha`.
- **Requisito de servidor:** el rollback necesita la opción **`nested triggers`** habilitada (valor por defecto = 1), porque el `UPDATE` de `Productos_C` dispara un trigger que modifica `Productos`, que a su vez dispara otro. La recursión termina en el segundo nivel porque `producto_c_actualizado` solo reacciona a la transición `Act 0→1`.
- **Patente** `AuditarProductos` (familia "Auditoría de productos"), asignada a `SuperAdmin`; el script limpia `DV` (`DELETE FROM DV`) para que los dígitos verificadores se recalculen. También figura en `Permisos` y en el `Deploy_TP_Ing_Soft.sql`.
- Soporte de UI: `EstiloRF2_790MY` (alias `E`: `T`, `MostrarErrorEnBanner`, `MostrarExitoToast`, `Deshabilitado`, `Gris`), `PanelRedondeado_790MY`, `ConfirmModal_790MY`, `Banner_UC_790MY`, FontAwesome.Sharp. Traducciones: claves `BC_*` (63) en `idiomas.json`; la pantalla se suscribe como observer (`UpdateIdioma`). Ayuda F1: documentada en el manual `Manual/Receta_BitacoraCambios.pdf` e implementa `IAyudaContextual_790MY`.

**Mejoras (4.1):**
1. **Ruido de versiones por stock:** como `producto_actualizado` se dispara con *cualquier* `UPDATE` de `Productos`, **cada descuento de stock por una venta crea una versión nueva** (`Evento.StockDescontado`). El historial se llena de filas que no son decisiones de un auditor y, además, **restaurar una versión vieja también restaura su stock viejo** (la regla de `PorcionesComprometidas` mitiga, no elimina). Alternativa: versionar solo cambios de Precio/Nombre/Categoría/Activo (`IF UPDATE(Precio) OR …`) o guardar el stock aparte.
2. `ArmarTablaDiff`/`AgregarFilaDiff` crean controles en tiempo de ejecución dentro de `FormBitacoraCambios_790MY.cs`, lo que contradice la regla "el `.cs` no instancia controles". Podría ser un `UserControl` `TablaDiff_UC_790MY` en el Designer.
3. El color `(184, 84, 20)` de `btnRestaurar_790MY` está **fijo en el `.cs`**; debería ser una constante de `TemaFogon_790MY` (por ejemplo `Aviso`).
4. La integridad depende de objetos de servidor (triggers + `nested triggers`): si alguien reinstala la base sin la migración, la pantalla "funciona" pero no hay historial. `Deploy_TP_Ing_Soft.sql` ya los incluye, pero convendría un chequeo de arranque (como `EventCatalog_08YS.CatalogValidation()`).
5. El rollback no registra **qué valores** cambiaron en la bitácora, solo `"Prod {ProductoID} v{ID} {Nombre}"` (50 caracteres máx. en `TargetUsername`).
6. La relectura + las validaciones + `ActivarVersion` no están en una transacción única que abarque la comprobación de stock comprometido: una venta entre medio podría invalidar la validación (ventana pequeña).

---

### 4.2 Bitácora del Sistema (auditoría de eventos) — `FormBitacora_08YS`

#### A. Idea y Diseño Visual

- **Objetivo:** consultar y **imprimir** la auditoría de todo lo que pasa en el sistema.
- **UI:** formulario clásico `FormBitacora_08YS` (namespace `GUI`), con **FontAwesome.Sharp** (`iconPictureBox1` en el título; `btnFiltrar`, `btnLimpiar`, `btnExportar` son `IconButton`s configurados por `ConfigurarBoton`) y los controles propios de `CustomControls`: `IconPlaceholderTextBox` (`txtUsername`, `txtTargetUsername`), `IconDateTimePicker` (`dtpDesde`, `dtpHasta`) e `IconComboBox` (`comboBoxModulo`, `comboBoxEvento`, `comboBoxCriticidad`). Grilla `dgvEventos`.
- **Color por criticidad:** `dgvEventos_CellFormatting` pinta la celda según `Criticidad` (Crítico/Alto/Medio/Bajo). Se activa `WS_EX_COMPOSITED` para evitar parpadeo.

#### B. Lógica y Reglas de Negocio

- **Combos en cascada:** `comboBoxModulo` → `RefreshComboEventos(Modulo?)` carga solo los eventos del módulo (`EventCatalog_08YS.GetEventsByModule`) y, si hay criticidad elegida, los filtra con `GetMetadata(ev).Criticidad`; `comboBoxEvento` y `comboBoxCriticidad` se sincronizan entre sí (al elegir un evento se fijan módulo y criticidad desde `EventCatalog_08YS.GetMetadata`; se desuscriben los handlers temporalmente para evitar cascadas infinitas).
- **Carga inicial:** `FormBitacora_Load` → `CargarGrid()` → `BitacoraBLL_08YS.GetAll()` (formato de fecha `dd/MM/yyyy HH:mm:ss`).
- **Filtro (`btnFiltrar_Click`):** arma un `BitacoraFiltro_08YS` (`Username`, `TargetUsername`, `Modulo?`, `Evento?`, `Criticidad?`, `FechaDesde?`, `FechaHasta?`). Valida con `MessageBox` (`error_validacion`): fecha de partida obligatoria (`msg_fecha_partida_req`), fecha fin obligatoria (`msg_fecha_fin_req`), ninguna en el futuro (`msg_fecha_partida_futura`, `msg_fecha_fin_futura`) y orden correcto (`msg_fechas_invertidas`).
- **Impresión (`btnExportar_Click`):** si no hay filas → `msg_no_datos_exportar`; si hay, `PrintDocument` **horizontal** (`DefaultPageSettings.Landscape = true`) + `PrintDialog`; `pd_PrintPage` dibuja el título **"REPORTE DE AUDITORÍA Y BITÁCORA"**, la línea "Sistema: GastroGest" y las filas paginadas con `_indiceFilaActual`.
- **Reglas de la bitácora (`BitacoraBLL_08YS`):**
  - `RegistrarEvento(Evento evento, string username = null, string targetUsername = null)`: toma el usuario de la sesión si no se pasa; **trunca `targetUsername` a 50 caracteres**; usa `EventCatalog_08YS.GetMetadata` para deducir Módulo/Criticidad.
  - `RegistrarEventoPostCommit(evento, targetUsername)`: se llama **después** de confirmar la operación de negocio; **reintenta una vez, nunca relanza** y deja el error en `Trace.TraceError`. Así una falla de auditoría no deshace una operación ya hecha.
  - `ContarIntentosFallidos(username, ventanaHoras = 2)`: cuenta los `LoginFallido` dentro de la ventana, con reinicio al ocurrir `LoginExitoso` o `UsuarioDesbloqueado` (alimenta el bloqueo de usuarios).
  - `GetAll()` y `Filtrar(BitacoraFiltro_08YS)`.
- **Catálogo (`EventCatalog_08YS`):** diccionario `Evento → EventoMetadata_08YS(Modulo, Criticidad)`. Enum `Evento` con valores hasta `ProductoVersionRestaurada = 53` (`CierreCajaRealizado = 52`). `CatalogValidation()` se ejecuta en `Program.cs` y falla al arrancar si algún evento no tiene metadatos. Enums `Criticidad {Critico, Alto, Medio, Bajo}` y `Modulo {Usuarios, Login, Perfiles, Clientes, Mesas, Reservas, Sistema, Empleados=7, Pedidos=8, Stock=9, Facturacion=10}`.

#### C. Flujo de Trabajo por Capas

```
Cualquier BLL ── RegistrarEvento / RegistrarEventoPostCommit ──► BitacoraBLL_08YS ──► IBitacoraRepository_08YS
                                                                  SqlBitacoraRepository_08YS.RegistrarEvento(BitacoraEvento_08YS)
FormBitacora_08YS ── BLLFactory_08YS.CreateBitacoraBLL() ──► BitacoraBLL_08YS
   CargarGrid() → GetAll()        btnFiltrar_Click → Filtrar(BitacoraFiltro_08YS)
        ▼  SqlBitacoraRepository_08YS.GetAll / Filtrar  (LIKE en Username y TargetUsername; resto con parámetros)
BE (Service_08YS/Entities/Bitacora): BitacoraEvento_08YS · BitacoraFiltro_08YS · Evento · Modulo · Criticidad
```

#### D. Clases Auxiliares e Integración

`EventCatalog_08YS`, `EventoMetadata_08YS`, `TraductorManager_08YS` (claves `Columna*`, `msg_*`, `atencion`, `error_validacion`; `TraducirColumnas` y `TraducirControles` recorren los controles al cambiar de idioma; implementa `IIdiomaObserver_08YS.UpdateIdioma`), `System.Drawing.Printing`, FontAwesome.Sharp, `CustomControls`, `SessionManager_08YS`. El permiso `VerBitacora` se valida en `FormMDI_08YS.bitacoraToolStripMenuItem_Click` (`ValidatePermission(Permisos.VerBitacora)` antes de `OpenChildForm(new FormBitacora_08YS())`); **`BitacoraBLL_08YS.GetAll/Filtrar` no vuelven a validarlo**. La tabla `Bitacora` **no** está en `DVManager_08YS.TablasMonitoreadas` (lista de 18 tablas con DVH/DVV), así que su contenido no está protegido por dígitos verificadores.

**Mejoras (4.2):**
1. `FormBitacora_08YS` usa `MessageBox` para validar fechas, avisar de "sin datos" y hasta para mostrar `ex.Message` crudo en `CargarGrid` y `btnFiltrar_Click` (el estándar E2-D1 pide `Banner_UC_790MY` + `ErrorProvider`); es de las pantallas del lote "08YS" sin migrar al estándar de feedback.
2. La "exportación" es **solo impresión**; no hay exportación a PDF/CSV (con iTextSharp ya referenciado sería inmediato, igual que en el Reporte de Reservas).
3. `GetAll()` trae **toda** la tabla al abrir la pantalla; con bitácora grande conviene paginar o limitar por fecha (como hace la Bitácora de Cambios con sus 30 días).
4. El pie del reporte dice "Sistema: GastroGest" (nombre anterior): unificar con "El Fogón del Sur".
5. Defensa en profundidad: `BitacoraBLL_08YS.GetAll/Filtrar` no validan el permiso (solo lo hace el menú del MDI); cualquier otro código con acceso a la BLL podría leer la auditoría. Además la tabla `Bitacora` no entra en `TablasMonitoreadas` de `DVManager_08YS`, por lo que una edición directa en SQL no se detecta.
6. `RegistrarEvento` (no `PostCommit`) se usa en `ClienteBLL_790MY` y `MesaBLL_790MY`: si la auditoría falla, la excepción llega al usuario aunque el alta ya se hizo.

---

## 5. SISTEMA DE AYUDA "SOMBRERO DEL CHEF"

Sistema de ayuda **integrado en `FormMDI_08YS`** (Etapa 5 del rediseño UX). Todo el código de ayuda vive en `GUI/Estilo` (namespace `GUI_08YS.Estilo`) y se activa desde la fila **"Ayuda"** del panel lateral (el botón `btnAyuda`, un `FilaAyuda_790MY`) o con **F1**. Tiene **cuatro piezas**:

| Pieza | Qué es | Clases principales |
|---|---|---|
| **Visita Guiada** | Recorrido del menú, con el fondo oscurecido y un "hueco" dorado sobre cada elemento | `VisitaGuiada_790MY`, `PasoVisita_790MY`, `VisitaOverlay_790MY`, `GloboAyuda_790MY`, `RegistroVisita_790MY` |
| **Sugerencias (F1)** | Marcos dorados + globo sobre los controles de **la pantalla abierta** | `IAyudaContextual_790MY`, `PistaAyuda_790MY`, `AyudaSugerencias_790MY`, `GloboAyuda_790MY` |
| **Menú Degustación** | Guía de 6 pasos del servicio real (cliente → reserva → mesa → pedido → cocina → caja) con atajo "Ir" y "Receta" (PDF) | `MenuDegustacion_UC_790MY`, `PasoDegustacion_790MY`, `DestinoAyuda_790MY`, `ManualRecetas_790MY` |
| **Ficha de Ingredientes** | Datos de prueba **en vivo** (clientes, empleados, mesas, turno, tarjetas) para demos | `FichaIngredientes_UC_790MY`, `ValorCopiable_790MY` |

El contenedor de las dos últimas es `PanelAyuda_UC_790MY` (panel lateral de 320 px con pestañas `SlidingTab_UC_790MY`).

Referencias de diseño del proyecto: `Claude outputs/Decisiones_Etapa5_Sistema_Ayuda.md` (decisiones A1–A7, C1–C11, G1–G5) e `Informe_Etapa5.md` (auditoría y hallazgos).

#### A. Idea y Diseño Visual

- **Concepto de marca:** el ícono de ayuda es un **sombrero de chef** (imagen PNG propia, no FontAwesome). La ayuda se plantea como "la cocina enseña a cocinar": *Menú Degustación* (recorrido por los platos del servicio), *Receta* (PDF paso a paso de cada caso de uso), *Ficha de Ingredientes* (datos de prueba) y *Visita Guiada* (el salón).
- **`FilaAyuda_790MY` (UserControl accesible):** fila del panel lateral con sombrero + texto + tecla **"F1"** (`TeclaEtiqueta_790MY`, un `Label` pintado como tecla). El PNG se elige por DPI en `ElegirSombrero(int dpi)` (32/40/48/64 px según `DeviceDpi`). Estados `AplicarEstado()` (normal/hover/presionado/deshabilitado con `ImagenAtenuada()`); **animación de hover** (`tmrAnimacion_790MY_Tick`): el sombrero se **inclina hasta 10°** en 150 ms y un halo ("glow") de 300 ms, y se omite si `SystemInformation.UIEffectsEnabled` es `false`. Soporta teclado (`OnKeyDown`, `IsInputKey`) y propaga el clic de sus hijos. Traduce texto, ToolTip y nombre accesible en `UpdateIdioma()`.
- **Paleta:** todo con `TemaFogon_790MY` — **Oro** (`#E8B964`) para marcos/bordes, **OroClaro** (`#F2D9A6`), **Marca** (`#610D10`) y **Barra*** (`BarraFondo (60,15,15)`, `BarraHover (72,18,18)`, `BarraPresionado (189,73,73)`, `BarraTinta (235,195,140)`) para la fila del lateral, Crema/Panel para el panel.
- **Transparencias (el problema típico de WinForms):** WinForms no soporta transparencia real entre controles hermanos. Por eso:
  - **Visita Guiada:** `VisitaGuiada_790MY.Capturar(...)` saca una **captura `DrawToBitmap` por partes** (`panel1`, `panelLateral`, `panel2` y el formulario hijo) mientras el fondo está habilitado (así no sale gris); `VisitaOverlay_790MY` (un `Control` con `ControlStyles.Opaque | UserPaint | OptimizedDoubleBuffer`) dibuja esa captura, encima una **sombra semitransparente** `Color.FromArgb(170, 20, 10, 10)` con un **hueco** (`Hueco`) y un borde dorado de 3 px. El resultado parece transparencia, pero es un solo control opaco.
  - **Sugerencias:** el marco de resalte son **4 paneles opacos de 3 px color Oro** (uno por lado), no un control con fondo transparente; el globo (`GloboAyuda_790MY`) es opaco con borde Oro y botones ←/→/Esc.
- **`PanelAyuda_UC_790MY`:** panel de 320 px, **hermano** de `panel2` (el contenedor de los hijos MDI) para no taparlo; pestañas con `SlidingTab_UC_790MY` (control propio: pista redondeada con **deslizamiento animado** `tmrDeslizar_790MY_Tick`, navegación con flechas, soporte de accesibilidad `SlidingTabAccesible`).
- **`PasoDegustacion_790MY`** (UserControl por paso: título, ruta de menú + pista, casilla *Hecho*, botón *Ir* y botón *Receta*); **`FichaIngredientes_UC_790MY`** con valores copiables (`ValorCopiable_790MY`: clic copia al portapapeles).

#### B. Lógica y Reglas de Negocio

**B.1 F1 y menú de ayuda (`FormMDI_08YS`, región `Ayuda`):**
- `ProcessCmdKey`: `Keys.F1` se **consume siempre** (aunque no haga nada); solo actúa si `!_navegacionBloqueada` y la visita no está activa (`_visita?.Activa != true`). No se usa el evento `HelpRequested` porque Windows lo dispara también con un `ConfirmModal` abierto.
- `AbrirAyuda(bool contextual)` — árbol de decisión: (1) si las sugerencias están activas, las cierra (y si fue F1, termina); (2) si el panel está abierto, lo cierra; (3) si es contextual y `panel2.Tag` es un `Form` que implementa **`IAyudaContextual_790MY`** y `Sugerencias.Mostrar(hijo, pantalla.ObtenerPistas(), foco)` devuelve `true`, muestra los marcos; (4) en cualquier otro caso, `MostrarAyuda()` abre el panel. La fila "Ayuda" (`btnAyuda_Click`) abre un **menú desplegable** (`AyudaDropDownMenu`): *Sugerencias* (`sugerenciasAyudaToolStripMenuItem_Click`), *Visita* (`visitaAyudaToolStripMenuItem_Click` → `IniciarVisita`), *Manual de Recetas* (`manualAyudaToolStripMenuItem`, **visible solo para SuperAdmin**) y *Ficha / Degustación* (`fichaAyudaToolStripMenuItem`, el texto cambia según `FichaPermitida`).
- `CerrarAyuda()` devuelve el foco al control anterior (`_focoAntesDeAyuda`). `BloquearNavegacion` corta visita/sugerencias/panel; `FormMDI_Resize` y `panel2_ControlRemoved` también cierran las sugerencias.

**B.2 Visita Guiada:**
- **Primer ingreso:** `FormMDI_Shown` lanza `IniciarVisita` (con `BeginInvoke`) si `!RegistroVisita_790MY.FueVista(Username)`. Al terminar (evento `Terminada`) se llama a `RegistroVisita_790MY.MarcarVista(Username)`.
- **Pasos derivados de lo que el usuario realmente ve** (`ConstruirPasosVisita`): logo (`AY_vis_inicio_*`), un paso por cada botón de la barra **visible** según permisos (`btnPerfil`, `btnAdministrativo`, `btnMaestros`, `btnReservar`, `btnAtencion_790MY`, `btnReportes_790MY`) con la lista de **opciones disponibles** de su menú (`OpcionesDisponibles` recorre `ToolStripItem.Available`, o sea lo que dejó `PermissionFilter_08YS`), `btnAyuda` (`AY_vis_ayuda_x_ficha` si hay Ficha), `panelIdioma` y `btnCerrarSesion`. **No hay un texto fijo por rol**: cambia con los permisos.
- `IniciarVisita` → `VisitaGuiada_790MY.Iniciar(pasos, partes)` y deshabilita el fondo (`PermitirInteraccionDeFondo(false)`) para que Tab no llegue a controles tapados. `VisitaGuiada_790MY.Cortar()` la cancela (resize/bloqueo).
- **Persistencia (`RegistroVisita_790MY`)** — ver §5.D.

**B.3 Sugerencias contextuales (F1):**
- Contrato `IAyudaContextual_790MY.ObtenerPistas()` → `IReadOnlyList<PistaAyuda_790MY>`. Cada pista: `PistaAyuda_790MY(nombreControl, claveTitulo, claveTexto, claveProbaCon = null)` (ej.: `new PistaAyuda_790MY(nameof(btnExportar), "AY_sug_bi4_t", "AY_sug_bi4_x")` en `FormBitacora_08YS`).
- `AyudaSugerencias_790MY.Mostrar(hijo, pistas, focoPrevio)`: busca cada control por **nombre** con `hijo.Controls.Find(nombre, true)` y **descarta los que no están visibles** o tienen tamaño 0 (por eso la ayuda se adapta a permisos y estado); si no queda ninguna pista devuelve `false` y F1 abre el panel. Navegación ←/→, Esc cierra (en `GloboAyuda_790MY.ProcessCmdKey`).
- **Pantallas con ayuda contextual: 19** (todas las del menú): `FormGestionUsuarios_08YS`, `FormGestionAcceso_08YS`, `FormAccesoAM_08YS`, `FormGestionRespaldos_08YS`, `FormBitacora_08YS`, `FormBitacoraCambios_790MY`, `FormCambiarContraseña_08YS`, `FormClientes_790MY`, `FormEmpleados_790MY`, `FormMesas_790MY`, `FormProductos_790MY`, `FormRegistrarReserva_790MY`, `FormAbrirMesa_790MY`, `FormRegistrarPedido_790MY`, `FormCocinaKDS_790MY`, `FormFacturacionCobro_790MY`, `FormReporteReservas_790MY`, `FormArqueoCaja_790MY`, `FormReportePredictivo_790MY`.

**B.4 Menú Degustación (`MenuDegustacion_UC_790MY`):** 6 pasos (`_pasos`), cada uno con `Destino` (`DestinoAyuda_790MY`), permisos requeridos (`p.Permisos.Any(sesion.HasPermission)`), `Receta` (PDF), ruta de menú y pista armada con las **etiquetas reales de los botones** (`Boton("AM_btnConfirmar")`…), de modo que el texto de ayuda nunca queda desactualizado:

| # | Destino | Permiso(s) | Receta (PDF) |
|---|---|---|---|
| 1 | `Clientes` | `VerClientes` | `Receta_Cliente.pdf` |
| 2 | `RegistrarReserva` | `RegistrarReserva` | `Receta_Reserva.pdf` |
| 3 | `AbrirMesa` | `AbrirMesa` | `Receta_AbrirMesa.pdf` |
| 4 | `RegistrarPedido` | `RegistrarPedido` | `Receta_RegistrarPedido.pdf` |
| 5 | `Cocina` | `DespacharPedido` | `Receta_DespacharPedido.pdf` |
| 6 | `CajaCobro` | `RegistrarCobro` **o** `GenerarFactura` | `Receta_CajaYCobro.pdf` |

Sin permiso, el paso se muestra deshabilitado con el motivo (`AY_paso_sin_permiso`). El botón *Ir* dispara `NavegacionSolicitada` → `FormMDI_08YS.IrAPantalla(DestinoAyuda_790MY)`, que hace **`ToolStripMenuItem.PerformClick()`** del ítem real del menú: así **hereda `ValidatePermission` y `OpenChildForm`** sin duplicar lógica (decisión G5); devuelve `false` si el ítem no está `Available`/`Enabled`.

**B.5 Ficha de Ingredientes:** solo existe si **`ModoDemostracion = true` (App.config, leído con `ConfigurationManager.AppSettings`) Y el rol del usuario es `SuperAdmin`** (`PanelAyuda_UC_790MY.Preparar()` → `_fichaPermitida`; si no, **remueve y hace `Dispose()` del control**: no queda ni la pestaña ni los datos en memoria). `Cargar()` lee **en vivo**: los primeros 5 clientes y empleados activos (`CargarClientes`, `CargarEmpleados`), 5 mesas no `FueraDeServicio` (`CargarMesas`), el turno vigente (`CargarTurno` → `MesaBLL_790MY.ObtenerTurnoVigente`/`EsTurnoDePrueba`) y dos **tarjetas de prueba** (`CargarTarjetas`): `4111 1111 1111 1111` (aprobada) y `4000 0000 0000 0002` (rechazada), validadas con `PagoBLL_790MY.ValidarTarjetaLuhn` y mostrando `Pago_790MY.CuotasPermitidas`. Las lecturas usan `BLLFactory_790MY.CreateClienteBLL()/CreateEmpleadoBLL()/CreateMesaBLL()` con `.GetAll()` y un `Take(Maximo)`; `Leer<TItem>` aísla cada sección: si una lectura falla, la registra en `_seccionesConError` y esa sección muestra "no disponible" (`AY_ficha_no_disponible`) sin romper el resto del panel. Las mesas **no se llaman "libres"** (depende de fecha y turno): se muestran las primeras en servicio con número y capacidad. `ValorCopiable_790MY` copia con `Clipboard` dentro de `try/catch` (el portapapeles puede estar bloqueado por otro proceso).

**B.6 Manejo de errores:** nada de `MessageBox`. Los errores se muestran en `Banner_UC_790MY` (`bnrDegustacion_790MY`, `MostrarAviso`); toda E/S de archivos está protegida (`EsErrorDeArchivo`) y, si falla, se asume "visita no vista" y el ingreso sigue.

#### C. Flujo de Trabajo por Capas

La ayuda es **100 % capa GUI**: no tiene entidad propia ni tabla. Solo toca la BLL/DAL a través de la Ficha (lectura en vivo) y de la seguridad.

```
F1 / btnAyuda ──► FormMDI_08YS.ProcessCmdKey / btnAyuda_Click ──► AbrirAyuda(contextual)
   ├─ pantalla activa (panel2.Tag) : IAyudaContextual_790MY.ObtenerPistas() ─► AyudaSugerencias_790MY.Mostrar(...) ─► GloboAyuda_790MY
   └─ sin pistas ─► MostrarAyuda() ─► PanelAyuda_UC_790MY.AlMostrar()
          ├─ MenuDegustacion_UC_790MY.Refrescar()  ─ SessionManager_08YS.HasPermission(Permisos.X)
          │        ├ Paso "Ir"     ─► NavegacionSolicitada ─► FormMDI_08YS.IrAPantalla(DestinoAyuda_790MY) ─► ToolStripMenuItem.PerformClick()
          │        └ Paso "Receta" ─► ManualRecetas_790MY.AbrirManual("Receta_X.pdf") ─► Process.Start(PDF)
          └─ FichaIngredientes_UC_790MY.Cargar()   (solo SuperAdmin + ModoDemostracion)
                   ├─ ClienteBLL_790MY / EmpleadoBLL_790MY / MesaBLL_790MY (lecturas)  ─► repositorios SQL ─► BE (Cliente, Empleado, Mesa)
                   ├─ MesaBLL_790MY.ObtenerTurnoVigente() / EsTurnoDePrueba()
                   └─ PagoBLL_790MY.ValidarTarjetaLuhn(...) · Pago_790MY.CuotasPermitidas

FormMDI_Shown ─► RegistroVisita_790MY.FueVista(Username) ─► (no vista) ─► IniciarVisita() ─► VisitaGuiada_790MY.Iniciar(ConstruirPasosVisita(), partes)
VisitaGuiada_790MY.Terminada ─► RegistroVisita_790MY.MarcarVista(Username)   (%LocalAppData%\El Fogon del Sur\ayuda.json)
```

#### D. Clases Auxiliares e Integración

- **`RegistroVisita_790MY` (internal static) — persistencia en `ayuda.json`:**
  - Ruta: `%LocalAppData%\El Fogon del Sur\ayuda.json` (`Environment.SpecialFolder.LocalApplicationData`), porque **`Program Files` no es escribible sin privilegios**.
  - Formato (Newtonsoft.Json): `{ "visitasVistas": [ "usuario1", "usuario2" ] }` (clase privada `Datos`, `[JsonProperty("visitasVistas")]`).
  - La **clave es el `Username` de la aplicación, no el rol** (dos camareros comparten rol) y la comparación es `OrdinalIgnoreCase`. Es **por equipo y no por base de datos** (no toca SQL).
  - `MarcarVista`: lectura → si el JSON está dañado (`JsonException`) lo reescribe → agrega el usuario → escritura **atómica**: `ayuda.json.tmp` + `File.Delete` + `File.Move`. Errores de E/S (`IOException`, `UnauthorizedAccessException`, `JsonException`, `NotSupportedException`, `SecurityException`, `ArgumentException`) se tragan con `Trace.TraceWarning`.
  - `FueVista`: ante cualquier error de archivo devuelve `false` (preferible mostrar la visita de nuevo a no iniciar sesión).
- **`ManualRecetas_790MY`:** carpeta `{AppDomain.BaseDirectory}\Manual`; `AbrirManual(receta)` devuelve `null` si abrió el PDF o el mensaje de error para el banner (`AY_manual_no_encontrado`, `AY_manual_no_abre`; `Win32Exception`/`ExternalException`/`InvalidOperationException` si no hay visor asociado). El instalador copia `docs\Manual\*.pdf` a `{app}\Manual` (ver §6). Los 7 PDFs: `Receta_AbrirMesa`, `Receta_BitacoraCambios`, `Receta_CajaYCobro`, `Receta_Cliente`, `Receta_DespacharPedido`, `Receta_RegistrarPedido`, `Receta_Reserva`.
- **Traducciones:** `GUI/idiomas.json` {es, en}, **197 claves con prefijo `AY_`** (`AY_vis_*`, `AY_sug_*`, `AY_paso*`, `AY_menu_*`, `AY_manual_*`…). Los controles de ayuda implementan `UpdateIdioma()` y el MDI los refresca al cambiar de idioma (`btnAyuda.UpdateIdioma()`, `panelAyuda.UpdateIdioma()`, `TraducirMenuFlotanteCustom(AyudaDropDownMenu)`).
- **Dependencias:** `Newtonsoft.Json` 13.0.4, `System.Configuration` (`ModoDemostracion`), `System.Diagnostics.Process`, `System.Windows.Forms.Clipboard`. Reutiliza `Banner_UC_790MY`, `TipoBanner_790MY`, `ConfirmModal_790MY` y `TemaFogon_790MY`.
- **Hallazgo de auditoría ya corregido (Informe_Etapa5):** `OpenChildForm` **no hacía `Dispose()`** de los hijos (fugas de timers y observers); ahora hace `Close()` + `Dispose()` y cancela la navegación si el hijo cancela el cierre.

**Mejoras (5):**
1. **Acoplamiento por nombre de control:** `PistaAyuda_790MY` busca por `nameof(control)`; si se renombra un control, la pista **desaparece en silencio** (el `Find` no lo halla). Una prueba automatizada (o un chequeo en `DEBUG`) que valide las 19 pantallas evitaría regresiones.
2. **Visita "una vez por equipo":** `ayuda.json` no viaja con el usuario; si inicia sesión en otra PC, la visita vuelve a aparecer. Alternativa: columna `VisitaVista` en `Users`.
3. La clave de la visita es el `Username` (texto): si se elimina un usuario y se crea otro con el mismo nombre, hereda el "ya vista" del anterior.
4. `FilaAyuda_790MY` (≈310 líneas) y `SlidingTab_UC_790MY` (≈320) dibujan con GDI+ a mano; funcionan, pero son los controles más costosos de mantener. Se podría extraer la animación a un helper común (`AnimadorHover_790MY`).
5. La Ficha depende de **dos condiciones de configuración** (`ModoDemostracion` + rol); conviene recordar en la defensa que en **producción** debe quedar `ModoDemostracion = false` (App.config).
6. `ManualRecetas_790MY` abre el PDF con la aplicación asociada; no hay fallback embebido si el visor no existe (solo el aviso en banner).
7. La captura `DrawToBitmap` de la visita se hace una vez al iniciar: si el usuario redimensiona la ventana, la visita se **corta** (`FormMDI_Resize`) en lugar de rehacerse.

---

## 6. INSTALADOR Y DESPLIEGUE

Este módulo **no tiene capa GUI/BLL/DAL/BE propia** (es empaquetado y arranque), así que el apartado C reemplaza "GUI → BLL → DAL" por la **cadena de despliegue**: *Release → ISCC → Setup → sqlcmd → script SQL → primer arranque*. Es la única excepción a la plantilla, y se señala explícitamente en cada sección.

Archivos involucrados (todos en la raíz de la solución `TP-Ingenieria-de-Software-Beta`):

| Archivo | Rol |
|---|---|
| `Installer_TP_Fogon.iss` | Script de **Inno Setup 6** (450 líneas). Genera `Installer\Output\Setup_ElFogonDelSur.exe` |
| `docs\Deploy_TP_Ing_Soft.sql` | Script **único** de base de datos (1430 líneas): esquema + datos semilla + DV. Lo ejecuta el instalador con `sqlcmd` |
| `GUI\Config\App.secrets.config.template` | Plantilla de credenciales SMTP |
| `GUI\Config\App.config` | Cadena de conexión `TP_Ing_Soft`, `ModoDemostracion`, `TurnoPrueba_790MY`, SMTP por `configSource` |
| `docs\Manual\*.pdf` | 7 recetas (ver §5) |
| `script.sql` (raíz y `GUI\script.sql`) | Script **antiguo** de esquema, usado por el *fallback* en tiempo de ejecución (`InstaladorBD_08YS`) |
| `Setup_08YS\Setup_08YS.vdproj` | Proyecto de instalación **legado** de Visual Studio (obsoleto) |
| `Migraciones_790MY\*.sql` (13) y `Scripts\AlterTableMesas_Mapa.sql` | Migraciones históricas, reemplazadas por `Deploy_TP_Ing_Soft.sql` |
| `docs\Seed_TP_Ing_Soft.sql` | Semilla de la que se generó el deploy |

---

### 6.1 Instalador (Inno Setup / ISCC)

#### A. Idea y Diseño Visual

- **Objetivo:** que una PC "limpia" quede funcionando con **un solo `Setup_ElFogonDelSur.exe`**: copia la aplicación, crea la base de datos en una instancia de SQL Server elegida por el usuario, deja la cadena de conexión apuntando a esa instancia y crea accesos directos. También soporta **reinstalar/actualizar** sin tocar los datos: hace un `BACKUP DATABASE` y aplica el mismo script idempotente, que solo agrega lo que falte del esquema *(deuda técnica, fase 4)*.
- **Casilla "Instalar en modo demostración"** (`[Tasks] mododemo`, **desmarcada** por defecto): fija `ModoDemostracion` en `GUI_08YS.exe.config` (`false` = producción; `true` solo si se marca) *(A8)*.
- **Interfaz del asistente (no usa FontAwesome ni UserControls; es el asistente de Inno Setup):**
  - `WizardStyle=modern`, idioma `Spanish.isl` (`[Languages]`), ícono `GUI\Resources\FogonIcon.ico` (`SetupIconFile`, `UninstallDisplayIcon`), compresión `lzma2` + `SolidCompression=yes`.
  - **Página de reinstalación** (`CreateOutputMsgPage(wpWelcome, 'Reinstalación detectada', …)`): solo si ya existe la clave de registro; muestra carpeta e instancia previas.
  - **Página personalizada "Base de datos"** (`CreateCustomPage(wpSelectDir, …)`): `TNewStaticText` descriptivo, `TNewComboBox` (`SqlCombo`, `csDropDown`: se puede elegir **o escribir** una instancia) y una nota que explica qué datos se crean. Se omite en reinstalación (`ShouldSkipPage`).
  - `UpdateReadyMemo` agrega a la pantalla final el **modo** ("Instalación limpia" / "Reinstalación / Actualización" / "Sin sqlcmd") y la instancia elegida.
  - Casilla de **ícono en el Escritorio** (`[Tasks] desktopicon`) y accesos en el Menú Inicio (`[Icons]`: aplicación, `Documentación` y desinstalador). Casilla final "Iniciar El Fogón del Sur" (`postinstall skipifsilent`).
- **Datos de identidad:** `AppName` "El Fogón del Sur", `MyAppVersion` **1.1.0** (comentario: "Etapa 5 (Ayuda del Chef y Manual de Recetas). Sin cambios en la base de datos"), `AppPublisher` "Maximo Yapura", ejecutable `GUI_08YS.exe`, `DefaultDirName={autopf}\El Fogon del Sur`, `AppId={{6F3B2A1C-8E4D-4F7A-9C21-5B0D7E9A4F18}` (fijo: identifica la instalación para actualizar/desinstalar).

#### B. Lógica y Reglas de Negocio

**Cómo se compila:** primero compilar la solución en **Release** y, desde la raíz del repositorio, ejecutar **`ISCC.exe Installer_TP_Fogon.iss`** (con winget queda en `%LOCALAPPDATA%\Programs\Inno Setup 6\ISCC.exe`). El instalador sale en `Installer\Output\Setup_ElFogonDelSur.exe` (`OutputDir`, `OutputBaseFilename`).

**Dos modos**, decididos en `InitializeSetup` con `IsUpgrade := RegValueExists(HKLM, 'Software\ElFogonDelSur', 'InstallPath')`:

| | Instalación limpia | Reinstalación / actualización |
|---|---|---|
| Detección | no existe la clave de registro | existe `HKLM\Software\ElFogonDelSur\InstallPath` |
| Carpeta | la elige el usuario (`DisableDirPage=auto`) | se reutiliza (`UsePreviousAppDir=yes`) |
| Instancia SQL | la elige en la página personalizada | se lee de `SqlInstance` del registro (si falta, `.`) |
| Prueba de conexión | sí: botón **Probar conexión** (`BtnProbarClick`, muestra versión y edición) y al pulsar Siguiente (`NextButtonClick`) | no |
| Backup previo | no | **`BACKUP DATABASE TP_Ing_Soft`** a `C:\ProgramData\El Fogon del Sur\Backups\TP_Ing_Soft_<fecha>_<hora>.bak` (si el servicio no puede escribir ahí, en la carpeta de backups de la instancia) (`DebeRespaldarBase` / `VerificarBackup`); si falla, aviso con la ruta del log y **el script no se ejecuta** |
| Script de BD | **se ejecuta** (crea todo) | **se ejecuta** si el backup salió bien: idempotente, agrega solo lo que falte; datos y DV sin cambios |
| `ModoDemostracion` | `false` salvo que se marque la casilla | se reescribe con la elección del asistente (propone la anterior) |
| `App.secrets.config` | se copia de la plantilla | **no se toca** (`onlyifdoesntexist`) |
| `GUI_08YS.exe.config` | se reescribe la instancia | se reescribe la instancia (se vuelve a copiar el `.config`) |

**Validaciones del asistente (`InitializeSetup` / `NextButtonClick`):**
1. **.NET Framework 4.8:** lee `HKLM\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\Release` y exige `>= 528040` (`NetFx48Release`); si no, pregunta si continúa igual.
2. **`sqlcmd` en el PATH:** `SqlCmdDisponible` ejecuta `where sqlcmd` (`RunCmd`); si falta, avisa que la base no se podrá crear desde el instalador (se puede hacer a mano con `Database\Deploy_TP_Ing_Soft.sql`) y pregunta si continúa.
3. **Instancia no vacía** y **prueba de conexión** (`ProbarConexion`, usada por el botón *Probar conexión* y por `NextButtonClick`): `sqlcmd -S "<instancia>" -E -b -l 30 -Q "SELECT SERVERPROPERTY(...)"` (30 s: LocalDB puede tardar en arrancar la primera vez); si falla, muestra el log y permite continuar.
4. **Detección de instancias (`DetectarInstanciasSql`, `LeerInstancias`):** lee `HKLM64`/`HKLM32` `SOFTWARE\Microsoft\SQL Server\Instance Names\SQL`; `MSSQLSERVER` → `"."`, nombradas → `".\NOMBRE"`, y **siempre** agrega `(localdb)\MSSQLLocalDB` al final.

**Despliegue de la base (`[Run]`, dos entradas en orden):**
```
1) cmd /C sqlcmd -S "<instancia>" -E -b -l 30 -Q "<ConsultaBackup>" > "C:\ProgramData\El Fogon del Sur\Logs\Backup_TP_Ing_Soft.log" 2>&1
      Check: DebeRespaldarBase  ( = IsUpgrade and SqlCmdEncontrado )        AfterInstall: VerificarBackup (BackupOk)
2) cmd /C sqlcmd -S "<instancia>" -E -b -l 30 -f 65001 -i "{app}\Database\Deploy_TP_Ing_Soft.sql" > "C:\ProgramData\El Fogon del Sur\Logs\Deploy_TP_Ing_Soft.log" 2>&1
      Check: DebeDesplegarBase  ( = SqlCmdEncontrado and (not IsUpgrade or BackupOk) )   AfterInstall: VerificarDeploy
```
`-E` autenticación de Windows, `-b` corta ante el primer error, `-f 65001` lee UTF-8. En la actualización la instancia sale del registro (`SqlInstance`). `ConsultaBackup` hace `BACKUP DATABASE … WITH INIT, CHECKSUM` dentro de `TRY/CATCH` + `THROW` (un `BACKUP` fallido no corta el lote: sin eso se imprimía la marca aunque el archivo no existiera) y emite `BACKUP_OK` (o `BACKUP_SIN_BASE` si la base no existe). La carpeta `C:\ProgramData\El Fogon del Sur\Backups` se crea con `Permissions: service-modify` porque el `.bak` lo escribe el servicio de SQL Server (solo sirve con instancia local): dentro de `Program Files` fallaba con *Operating system error 5 (Acceso denegado)*. Si aun así falla, el `CATCH` reintenta con el nombre sin carpeta y SQL Server lo guarda en su carpeta de backups predeterminada (`BACKUP_OK_CARPETA_SQL`). Backups y logs sobreviven a la desinstalación (`uninsneveruninstall`). `VerificarDeploy` exige **`DEPLOY_OK`** (sigue aceptando `DEPLOY_OMITIDO`, que emitían versiones anteriores del script); si falta, muestra el log, su ruta, la del backup y el comando para re-ejecutarlo a mano (la aplicación **queda instalada** igual y no se borra nada).

**Cadena de conexión:** `CurStepChanged(ssPostInstall)` → `ConfigurarConnectionString(GetSqlInstance(''))`, que genera `ConfigurarConexion.ps1` y lo corre con `powershell.exe -NoProfile -ExecutionPolicy Bypass`. Usa `System.Xml.XmlDocument` (`PreserveWhitespace`) + `SqlConnectionStringBuilder` para cambiar **solo** el `Data Source` de `/configuration/connectionStrings/add[@name='TP_Ing_Soft']` en `GUI_08YS.exe.config`, respetando UTF-8 y el resto de la cadena. Si falla, muestra el log e indica editar a mano.

**Desinstalación (`[UninstallDelete]`):** borra `{app}\Database`, `{app}\App.secrets.config` y `{app}\*`. **La base `TP_Ing_Soft` en SQL Server no se toca.** `[Registry]` usa `uninsdeletekey` para borrar `HKLM\Software\ElFogonDelSur` (`InstallPath`, `Version`, `SqlInstance`).

**Seguridad de la distribución (`[Files]`):** `GUI\bin\Release\*` se copia **excluyendo** `*.pdb`, `*.xml`, `script.sql`, `*.vshost.*`, `Manual\*` y, sobre todo, **`App.secrets.config`** (contiene credenciales SMTP reales de la PC de desarrollo). En su lugar viaja `App.secrets.config.template` (y una copia como `App.secrets.config`, solo si no existe). Además se instalan `docs\Deploy_TP_Ing_Soft.sql` → `{app}\Database`, `docs\*.pdf` → `{app}\docs` y `docs\Manual\*.pdf` → `{app}\Manual`.

**Privilegios y plataforma:** `PrivilegesRequired=admin` (necesario por `{autopf}` y `HKLM`), `ArchitecturesAllowed/InstallIn64BitMode=x64compatible`, `CloseApplications=yes`, `ChangesAssociations=yes` (refresca la caché de íconos).

#### C. Flujo de Despliegue (reemplaza al flujo por capas)

```
Visual Studio: Build → Release  ─►  GUI\bin\Release\  (GUI_08YS.exe, DLL, idiomas.json, Resources\, Manual\*.pdf, GUI_08YS.exe.config)
ISCC.exe Installer_TP_Fogon.iss ─► Installer\Output\Setup_ElFogonDelSur.exe
Setup_ElFogonDelSur.exe (administrador)
   InitializeSetup      : .NET 4.8 ✓ · IsUpgrade ✓ · SqlCmdEncontrado ✓
   InitializeWizard     : (reinstalación) página informativa · página "Base de datos" (SqlCombo)
   NextButtonClick      : instancia ≠ vacía · sqlcmd -Q "SELECT 1"
   [Files] / [Registry] / [Icons]
   [Run]                : (actualización) sqlcmd BACKUP DATABASE ─► VerificarBackup
                          sqlcmd … Deploy_TP_Ing_Soft.sql  (limpia, o actualización con backup OK) ─► VerificarDeploy (DEPLOY_OK)
   CurStepChanged       : ConfigurarConnectionString → GUI_08YS.exe.config  (Data Source = instancia)
                          ConfigurarModoDemostracion → GUI_08YS.exe.config  (ModoDemostracion = casilla mododemo)
Primer arranque: GUI_08YS.exe ─► Program.Main ─► BLLInstalador_08YS.AsegurarBaseDatos() ─► (si falta) InstaladorBD_08YS …  (ver 6.3)
```

#### D. Clases Auxiliares e Integración

- **Funciones del bloque `[Code]` (Pascal Script):** `GetSqlInstance`, `RunCmd`, `ReadLog`, `SqlCmdDisponible`, `AgregarInstancia`, `LeerInstancias`, `DetectarInstanciasSql`, `ConfigurarConnectionString`, `ConfigurarModoDemostracion`, `GetArchivoBackup`, `ConsultaBackup`, `DebeRespaldarBase`, `VerificarBackup`, `DebeDesplegarBase`, `VerificarDeploy`, `InitializeSetup`, `InitializeWizard`, `ShouldSkipPage`, `NextButtonClick`, `UpdateReadyMemo`, `CurStepChanged`.
- **Dependencias externas:** Inno Setup 6, `.NET Framework 4.8`, SQL Server (o LocalDB) con `sqlcmd` en el PATH, Windows PowerShell.
- **Config de la app que el instalador deja lista:** `App.config` del repositorio → `ModoDemostracion=true` (desarrollo / defensa; el instalador lo reescribe: `false` salvo que se marque la casilla), `TurnoPrueba_790MY=22:00` (ver §5 y §1), SMTP por `<smtp configSource="App.secrets.config" />`, *binding redirect* de `BouncyCastle.Crypto` 0.0.0.0–1.9.0.0 → 1.9.0.0 (iTextSharp).

**Mejoras (6.1):**
1. ~~El comentario de cabecera del `.iss` aún dice "SQL Server con la base TP_Ing_Soft creada"~~ **Resuelto (fase 4):** el encabezado describe la creación, el backup y la actualización.
2. En **reinstalación** `GUI_08YS.exe.config` se reemplaza y se reescribe la instancia; `ModoDemostracion` ahora también se reescribe con la elección del asistente (que propone la anterior), pero otros valores cambiados a mano (`TurnoPrueba_790MY`) se siguen perdiendo; considerar conservarlos o moverlos a un archivo externo como `App.secrets.config`.
3. ~~`ModoDemostracion=true` se distribuye por defecto~~ **Resuelto (A8):** casilla "Instalar en modo demostración" desmarcada por defecto → `false` en producción (y desaparece la Ficha, ver §5.B.5). `GUI\Config\App.config` conserva `true` para desarrollo/defensa.
4. **Parcialmente resuelto (M9):** la reinstalación ahora hace backup y ejecuta el Deploy idempotente, que agrega tablas/objetos faltantes y tiene una sección "Actualizaciones de esquema" para columnas (`IF COL_LENGTH … ADD`). Sigue sin haber una tabla `SchemaVersion`: cada cambio de esquema futuro debe escribirse con su guarda en esa sección.
5. La desinstalación borra `{app}\*` completo; funciona porque el directorio es exclusivo de la app, pero si el usuario eligió una carpeta compartida (`C:\Programas`) sería destructivo. Mitigar forzando una subcarpeta propia.
6. `PrivilegesRequired=admin` es necesario por `HKLM`/`Program Files`, pero obliga a UAC siempre; documentar la alternativa (`PrivilegesRequiredOverridesAllowed=dialog`).
7. El usuario semilla `Maximo` (SuperAdmin) viaja con su hash de contraseña en el script: forzar el cambio en el primer ingreso (existe `FormCambiarContraseña_08YS`) o pedir la clave inicial en el asistente.

---

### 6.2 Script de Base de Datos (`Deploy_TP_Ing_Soft.sql`)

#### A. Idea y Diseño

Script **único** y **re-ejecutable** que "reemplaza a script.sql + Migraciones_790MY + Seed_TP_Ing_Soft.sql". Fue *generado con SMO* a partir de una base semillada, por lo que contiene el **esquema final** (incluido RF2: pedidos, facturación y cobro) y los datos reales. Se invoca con el comando de §6.1 o a mano: `sqlcmd -S . -E -b -f 65001 -i Deploy_TP_Ing_Soft.sql`.

#### B. Lógica y Reglas

- **Preparación:** `SET XACT_ABORT ON` y opciones ANSI. **(1)** Si `DB_ID('TP_Ing_Soft') IS NULL` → `CREATE DATABASE [TP_Ing_Soft] COLLATE Modern_Spanish_CI_AS`. **(2) Idempotencia real (fase 4, M9):** ya no hay cortocircuito `SET NOEXEC ON`. Al empezar se decide si es **instalación nueva** (no existe `dbo.Users`) y se guarda en `SESSION_CONTEXT('DeployNuevo')`. Cada objeto tiene su guarda: tablas con `IF OBJECT_ID(N'dbo.X', N'U') IS NULL`; tipo con `TYPE_ID`; índices por `sys.indexes`; `CHECK` por nombre; `DEFAULT` y **claves foráneas por columna** (varias FK no tienen nombre fijo); procedimientos y triggers con **`CREATE OR ALTER`** (requiere **SQL Server 2016 SP1+**, documentado en el encabezado). Los cambios de columnas de tablas existentes van en la sección **"Actualizaciones de esquema"** (`IF COL_LENGTH(...) IS NULL ALTER TABLE … ADD`); hoy contiene la copia de los cierres históricos a `CierresCaja` (A4).
- **Atomicidad:** todo el esquema y los datos van en **una única transacción** (`BEGIN TRANSACTION` … `COMMIT TRANSACTION` al final): ante cualquier error (`XACT_ABORT`) no queda una base a medio crear ni a medio actualizar. Termina con `PRINT 'DEPLOY_OK'` en ambos casos (ya no emite `DEPLOY_OMITIDO`).
- **Contenido:**
  - **Tipo:** `IntList` (tabla de tipo, parámetros de tabla).
  - **24 tablas:** `Bitacora`, `CierresCaja` *(A4: PK `Fecha`, fuera de los DV)*, `Clientes`, `DV`, `EmailsAdicionalesCliente`, `Empleados`, `Facturas`, `FamiliaIntegrada`, `FamiliaPermiso`, `Familias`, `ItemsFactura`, `ItemsPedido`, `Mesas`, `Pagos`, `Pedidos`, `Permisos`, `Productos`, `Productos_C`, `Reservas`, `Roles`, `RolFamilia`, `RolPermiso`, `TelefonosAdicionalesCliente`, `Users` (+ restricciones, índices y claves foráneas).
  - **3 triggers** de la Bitácora de Cambios (§4.1): `producto_actualizado` *(A5: versiona solo cambios de negocio; un cambio solo de stock actualiza la vigente)*, `producto_eliminado`, `producto_c_actualizado`.
  - **Procedimientos almacenados:** `sp_CreateFamilia`, `sp_CreateRol`, `sp_DeleteFamilia`, `sp_DeleteRol`, `sp_EjecutarBackup`, `sp_GetAllFamilias`, `sp_GetAllPermisos`, `sp_GetAllRolesConEstructura`, `sp_GetAllTablesParaDV`, `sp_GetAncestrosFamilia`, `sp_GetDVTodo`, `sp_GetRolByID`, `sp_ModifyFamilia`, `sp_ModifyRol`, `sp_UpsertDVTabla` (los de alta/modificación usan `BEGIN TRY … ROLLBACK; THROW`).
  - **Datos semilla:** Seguridad (Roles, Permisos, Familias y relaciones), usuario **`Maximo` (SuperAdmin)**, 1 cliente de prueba, 4 empleados (uno por puesto), Mesas y el catálogo de Productos. **Sin** reservas, pedidos, facturas, pagos ni bitácora.
  - **Dígitos verificadores:** inserta los `DVH/DVV` correctos para esos datos, de modo que **el primer login no informa inconsistencias** (la integridad se valida contra las 18 tablas de `DVManager_08YS.TablasMonitoreadas`).
  - **Semilla y DV solo en instalación nueva** (`SESSION_CONTEXT('DeployNuevo') = 1`): en una base con datos no se inserta, pisa ni duplica nada y los DV quedan intactos (insertar aunque sea una fila semilla los dejaría inconsistentes). Prueba: `docs\Pruebas_DeployIdempotente.sql` (dos ejecuciones seguidas sobre una base con datos y sobre una base v1.1: sin errores, mismas filas y checksums, mismo hash de DV).

#### C. Flujo

`Setup_ElFogonDelSur.exe` → `sqlcmd` (cmd.exe) → `Deploy_TP_Ing_Soft.sql` → `Deploy_TP_Ing_Soft.log` → `VerificarDeploy`. Después, la app accede a esa base con `SqlDbFactory_08YS.ConnectionString` (cadena `TP_Ing_Soft` del `.config`).

#### D. Integración

- Los procedimientos `sp_GetAllTablesParaDV`, `sp_GetDVTodo` y `sp_UpsertDVTabla` los llama `SqlDvRepository_08YS` (que usa `DvBLL_08YS`/`DVManager_08YS`); los `sp_*Rol`/`sp_*Familia` los usa `SqlRolRepository_08YS` y el repositorio de familias (§2.5); `sp_EjecutarBackup`, `SqlBackupRepository_08YS` (pantalla `FormGestionRespaldos_08YS`).
- `Productos_C` y sus triggers son la base de la Bitácora de Cambios (§4.1) y exigen la opción de servidor `nested triggers` (por defecto activa).

**Mejoras (6.2):**
1. El script se **generó** con SMO desde una base semilla y desde la fase 4 se mantiene **a mano** con guardas idempotentes: un regenerado con SMO perdería esas guardas. Cada cambio de esquema futuro debe agregarse con su guarda (tabla nueva arriba; columna nueva en "Actualizaciones de esquema") y en `Migraciones_790MY\` para las bases existentes.
2. Los scripts viejos (`script.sql`, `Migraciones_790MY\*.sql`, `Scripts\AlterTableMesas_Mapa.sql` —este **borra datos de mesas**) siguen en el repositorio; conviene moverlos a una carpeta `Legacy/` o eliminarlos para evitar ejecutarlos por error.
3. El hash de contraseña del SuperAdmin `Maximo` queda en un script de texto versionado en el repositorio.

---

### 6.3 Fallback en tiempo de ejecución (`BLLInstalador_08YS` / `InstaladorBD_08YS`)

#### A–B. Idea y Reglas

Red de seguridad si la aplicación arranca **sin** que el instalador haya creado la base (por ejemplo, copiando la carpeta `Release` a mano). `Program.Main` (`GUI/Config/Program.cs`):

```csharp
try { BLLInstalador_08YS.AsegurarBaseDatos(); }
catch (Exception ex) { MessageBox.Show("No se pudo preparar la base de datos:\n\n" + ex.Message, "Error de instalación", …); return; }
EventCatalog_08YS.CatalogValidation();
DVManager_08YS.Inicializar(BLLFactory_08YS.CreateDvBLL());
Application.Run(new FormLogin_08YS());
```

- `BLLInstalador_08YS.AsegurarBaseDatos()` es una **fachada de una línea** (`=> InstaladorBD_08YS.AsegurarBaseDatos()`) y existe para que **la GUI nunca importe DAL** ("PUNTO CRÍTICO DE ARQUITECTURA" en el comentario).
- `InstaladorBD_08YS` (DAL, `System.Data.SqlClient`): `AsegurarBaseDatos()` toma el `DataSource` de `SqlDbFactory_08YS.ConnectionString` (`SqlConnectionStringBuilder`) → `ExisteBaseDatos(instancia)` (consulta `sys.databases` en `master`) → si no existe, `InstalarBaseDatos(instancia)`:
  1. Busca `script.sql` junto al `.exe` (`NombreArchivoScript`); si no está, `FileNotFoundException` con el mensaje "Verificá que el Setup Project lo incluya en Application Folder".
  2. Divide el script en lotes con `Regex.Split(script, @"^\s*GO\s*$", Multiline | IgnoreCase)` (porque `SqlCommand` no entiende `GO`) y los ejecuta **en la misma conexión** (el script hace `USE [TP_Ing_Soft]` y el contexto se conserva), `CommandTimeout = 120`.
  3. **Tolerancia a fallos:** si un lote lanza `SqlException` (p. ej. `ALTER DATABASE … QUERY_STORE` no soportado por LocalDB 2019), se ignora y se **registra** en `instalador_db.log` (junto al `.exe`) con fecha y mensaje; si ni siquiera se puede loguear, tampoco se bloquea.

#### C. Capas

`Program` (Config) → `BLLInstalador_08YS` (BLL) → `InstaladorBD_08YS` (DAL.Instalador) → `SqlDbFactory_08YS` → SQL Server. No hay BE.

#### D. Integración

`GUI.csproj` copia `script.sql` al directorio de salida (`CopyToOutputDirectory=Always`), pero el `.iss` **lo excluye** del instalador (`Excludes: … script.sql`): en una instalación con el Setup, la base la crea `Deploy_TP_Ing_Soft.sql` y el fallback queda como respaldo para ejecuciones de desarrollo.

**Mejoras (6.3):**
1. **Dos caminos de creación de BD con scripts distintos** (`script.sql` de 945 líneas, UTF-16, vs. `Deploy_TP_Ing_Soft.sql`): pueden divergir. Unificar: que el fallback use `Deploy_TP_Ing_Soft.sql` (o embebido como recurso). *(Pendiente, A9 parcial: la fase 4 se limitó al Deploy y al instalador; con el Deploy ya idempotente y transaccional, el fallback podría ejecutarlo sin el problema de lotes ignorados.)*
2. Ignorar `SqlException` por lote puede dejar una base **incompleta sin que el usuario lo sepa** (solo queda `instalador_db.log`). Al menos distinguir lotes opcionales (`ALTER DATABASE … SET`) de críticos (`CREATE TABLE`), o ejecutar el cuerpo en una transacción.
3. `Program.Main` usa `MessageBox` (es correcto: error fatal de arranque, previo a cualquier formulario) y `Regex` permite `GO` dentro de cadenas multilinea (poco probable, pero posible).
4. La cadena de conexión predefinida en `App.config` apunta a `Data Source=PC_Maximo` (nombre del equipo de desarrollo); funciona porque el instalador la reescribe, pero un *copy-paste* de la carpeta `Release` a otra PC falla al conectar.

---

### 6.4 Proyecto de instalación legado (`Setup_08YS.vdproj`)

Proyecto *Setup Project* de Visual Studio (`ProductName` **"HorizonHotel&Resort"**, versión `1.0.0`, `Manufacturer` "TPIngSoftSanchezYapura", incluye `script.sql` tomado de `..\GUI\bin\Debug` y la condición de lanzamiento `.NET Framework`). Es el instalador de una iteración anterior (el nombre del producto, "HorizonHotel&Resort", no corresponde a "El Fogón del Sur"); **está obsoleto** frente a Inno Setup: no crea la base, no pide instancia, no soporta reinstalación y apunta a `Debug`.

**Mejora:** no figura en `TrabajoDiploma.slnx`, pero la carpeta sigue en el repositorio; moverla a una carpeta `Legacy/` o eliminarla para que no se confunda con el instalador vigente.

---

## 7. PREGUNTAS PROBABLES DEL TRIBUNAL Y RESPUESTAS CORTAS

Respuestas pensadas para decirse en 30–60 segundos, con los nombres reales del código.

**Arquitectura**

1. **¿Por qué una arquitectura en capas y cómo la hacen cumplir?** — Cuatro capas (`GUI_08YS` → `BLL_08YS` → `DAL_08YS` → `BE_08YS`, más `Service_08YS` y `MPP_08YS`). La GUI solo habla con la BLL (incluso el arranque: `Program` llama a `BLLInstalador_08YS`, no a `DAL_08YS`); la BLL depende de **interfaces** (`IReservaRepository_790MY`, `IPagoRepository_790MY`, etc.), implementadas en `DAL_08YS.SQL`. Las dependencias se inyectan a mano en `BLLFactory_08YS` / `BLLFactory_790MY`. Cada BLL lee solo el repositorio de su entidad; para datos de otra entidad llama a la BLL dueña (métodos `internal` sin `ValidatePermission`, ver §0.4.1).
2. **¿Qué significan los sufijos `_790MY` y `_08YS`?** — Identifican a los dos autores del equipo (cada uno firma sus clases). Conviven en la misma solución y comparten sesión, bitácora y traductor.
3. **¿Cómo separan diseño y lógica en WinForms?** — `FormX.Designer.cs` solo diseño visual dentro de `InitializeComponent()`; `FormX.cs` solo eventos y llamadas a la BLL. *Excepción reconocida:* controles dinámicos creados en `.cs` (`CrearTarjetasKpi`, `ArmarTablaDiff`, `CrearTarjeta`, `MesaVisualControl_790MY`) porque su cantidad depende de los datos (ver mejoras §8).
4. **¿Cómo manejan los errores sin `MessageBox` en la BLL?** — La BLL **solo lanza excepciones** (`MensajesBLL_790MY.Regla(clave, args)` → `ArgumentException` con texto traducido; `UnauthorizedAccessException` para permisos). La GUI las muestra con `EstiloRF2_790MY.MostrarErrorEnBanner` (`Banner_UC_790MY`), marca el campo con `ErrorProvider` y confirma con `ToastHost_790MY`; `ConfirmModal_790MY` para acciones destructivas. `MessageBox` queda solo para errores fatales (decisión E2-D1).

**Seguridad**

5. **¿Cómo funciona el control de acceso?** — **Composite**: `AccessComponent_08YS` → `Permiso_08YS` (hoja) / `Familia_08YS` (compuesto); un `Rol_08YS` agrupa familias y permisos. `SessionManager_08YS` (singleton `Lazy`) resuelve `HasPermission` (compara `p.Nombre == permiso.ToString()`) y `ValidatePermission` lanza `UnauthorizedAccessException`. Se valida **dos veces**: la GUI oculta lo que no corresponde (`PermissionFilter_08YS` pone `Visible`) y cada método de BLL vuelve a validar (nunca se confía en la GUI).
6. **¿Cómo se guardan las contraseñas?** — `Encriptador_08YS`: PBKDF2 (`Rfc2898DeriveBytes`, SHA-256, 100 000 iteraciones, sal de 16 bytes, hash de 32 bytes, Base64). Bloqueo tras 3 intentos fallidos en 2 horas (`BitacoraBLL_08YS.ContarIntentosFallidos`).
7. **¿Qué son los dígitos verificadores (DV)?** — Control de integridad: `DVManager_08YS.Recalcular()` (tras cada operación) calcula DVH (por fila) y DVV (por tabla), los cifra con `DvEncriptador_08YS` (AES-256-CBC) y los guarda en la tabla `DV` vía `sp_UpsertDVTabla`. Cubre 18 tablas (`TablasMonitoreadas`). Si alguien edita datos directamente en SQL, el login informa inconsistencia (`FormInconsistenciaDB_08YS`). *Limitación honesta:* la clave está en el código y `Bitacora` no está en la lista.

**Casos de uso**

8. **¿Cómo evitan que dos recepcionistas reserven la misma mesa?** — `SqlReservaRepository_790MY.AddSiMesaDisponible` abre una transacción `SERIALIZABLE`, lee con `UPDLOCK, HOLDLOCK` y solo inserta si sigue libre; el criterio de "ocupada" vive en un solo lugar (`FiltroOcupacionMesa_790MY`).
9. **¿Por qué al registrar un pedido no se descuenta el stock?** — Se **compromete** (`Pedido_790MY.EstadosConStockComprometido`; `PorcionesDisponibles = stock − comprometido`). El descuento real ocurre en cocina, **atómico con el cambio a Listo** (`MarcarListoYDescontarStock`), con idempotencia por `StockDescontado`. Así un pedido cancelado no deja stock perdido y dos camareros no sobrevenden (`RegistrarSiHayStock` bloquea productos por ID para evitar deadlocks).
10. **¿Cómo validan la tarjeta?** — `PagoBLL_790MY.ValidarTarjetaLuhn` (13–19 dígitos, acepta espacios/guiones), cuotas ∈ {1,3,6,12} (`Pago_790MY.CuotasPermitidas`), marca por prefijo (`DetectarMarca`); **nunca se guarda el número completo** (`EnmascararTarjeta` → `**** **** **** 1111`). La autorización bancaria es **simulada**: toda tarjeta terminada en `0002` (`TerminacionRechazoSimulado`) lanza `PagoRechazadoException_790MY`.
11. **¿Qué pasa si falla algo a mitad del cobro?** — `IPagoRepository_790MY.RegistrarCobro` hace en **una transacción** `INSERT Pago` + `Factura→Pagada` + `Reserva→Finalizada` + `Mesa→Libre`; si algo falla, rollback total. Mientras se cobra, `BloquearNavegacion(true)` impide cerrar sesión, cambiar de idioma o cerrar la app.
12. **¿Cómo se calcula el IVA?** — `FacturaBLL_790MY.CalcularImportes` (estática, pura): `neto = subtotal − descuento`; `IVA = neto × 0,21` (`Factura_790MY.AlicuotaIVA`); `MontoTotal = neto + IVA`. El precio de carta **no incluye IVA**.

**Maestros y reportes**

13. **¿Dónde está la validación del DNI de 7–8 dígitos?** — En la GUI (`Regex ^\d{7,8}$` en `FormClientes_790MY`, `FormRegistrarCliente_790MY`, `FormRegistrarReserva_790MY`, `FormAbrirMesa_790MY` y el filtro de `FormReporteReservas_790MY`). La BLL solo exige `dni > 0` y unicidad: **reconocemos** que conviene repetir la regla en `ClienteBLL_790MY.ValidarDatos`.
14. **¿Por qué no se borran clientes/empleados/productos?** — Baja lógica (`Activo`): conserva reservas, facturas y auditoría. `Exists(dni)` no filtra `Activo` a propósito para no reutilizar DNIs de bajas.
15. **¿Cómo funciona el Reporte Predictivo?** — Market basket propio (sin librerías): sobre facturas Pagadas de 90 días calcula **soporte, confianza y lift** de pares de productos (`ReportePredictivoBLL_790MY.CalcularReglas`), con umbrales `MinimoCuentasReales = 10`, `SoporteMinimo = 3 %`, `LiftMinimo = 1,2`. Genera hasta 3 sugerencias (`TipoSugerencia_790MY`: combo, promoción en el día de menor demanda, venta cruzada). Las acciones de las sugerencias son **simuladas** (se avisa en el modal).
16. **¿Cómo se cierra la caja?** — `PagoBLL_790MY.CerrarCaja(fecha)` recalcula el arqueo, exige cobros y que no esté ya cerrada, y registra el evento `CierreCajaRealizado` con el prefijo `"Caja dd/MM/yyyy - "`; `EstaCajaCerrada` lo detecta consultando la bitácora. *Honestidad:* no hay tabla de cierres (ver mejoras).

**Bitácoras**

17. **¿Diferencia entre Bitácora y Bitácora de Cambios?** — La **Bitácora** (`FormBitacora_08YS`, tabla `Bitacora`) registra *quién hizo qué* (login, cobros, cierres…) con módulo y criticidad definidos en `EventCatalog_08YS`. La **Bitácora de Cambios** (`FormBitacoraCambios_790MY`, tabla espejo `Productos_C`) guarda *cada versión de un producto* mediante **triggers** (`producto_actualizado`, `producto_eliminado`, `producto_c_actualizado`) y permite **comparar y restaurar** (`ProductoCBLL_790MY.RestaurarVersion`).
18. **¿Cómo funciona el rollback de un producto?** — `ActivarVersion` hace `UPDATE Productos_C SET Act = 1 WHERE ID = @ID AND Act = 0`; el trigger `producto_c_actualizado` (transición 0→1) hace `MERGE` sobre `Productos` y `producto_actualizado` crea la nueva versión vigente. Antes se aplican las mismas reglas que `ModificarProducto` (nombre único, stock ≥ comprometido). Requiere `nested triggers` activo.
19. **¿Por qué `RegistrarEventoPostCommit` no relanza?** — Porque la operación de negocio ya se confirmó: informar un error de auditoría llevaría al usuario a repetirla (duplicados). Reintenta una vez y deja `Trace.TraceError`.

**Ayuda e instalador**

20. **¿Cómo se sabe que la visita guiada ya se vio?** — `RegistroVisita_790MY` guarda `{ "visitasVistas": [usernames] }` en `%LocalAppData%\El Fogon del Sur\ayuda.json` (porque `Program Files` no es escribible), con escritura atómica `.tmp` + `Move` y E/S protegida. La clave es el `Username`, no el rol.
21. **¿Cómo logran el efecto de transparencia en la visita?** — WinForms no soporta transparencia real entre hermanos: se captura la pantalla con `DrawToBitmap` (`VisitaGuiada_790MY.Capturar`) y `VisitaOverlay_790MY` (control opaco) pinta la captura con una sombra `Color.FromArgb(170, 20, 10, 10)` y un "hueco" con borde dorado.
22. **¿Cómo se despliega en una PC nueva?** — Se compila en Release y `ISCC.exe Installer_TP_Fogon.iss` genera `Setup_ElFogonDelSur.exe`. El asistente detecta .NET 4.8 y `sqlcmd`, ofrece las instancias de SQL Server del Registro, prueba la conexión, copia los archivos, corre `sqlcmd -E -b -f 65001 -i Deploy_TP_Ing_Soft.sql` (crea `TP_Ing_Soft` completa, en una transacción y re-ejecutable), verifica `DEPLOY_OK`/`DEPLOY_OMITIDO` y escribe la instancia en `GUI_08YS.exe.config`.
23. **¿Qué pasa al reinstalar?** — Si existe `HKLM\Software\ElFogonDelSur\InstallPath` el modo es *actualización*: se reutilizan carpeta e instancia, **no se toca la base** ni `App.secrets.config`.
24. **¿Dónde están las credenciales SMTP?** — En `App.secrets.config` (fuera del repositorio por `.gitignore` y **excluido del instalador**); se distribuye `App.secrets.config.template`. Sin él, la reserva igual se registra y el PDF se puede guardar.
25. **¿Qué hace `ModoDemostracion`?** — Si es `true`, cuando la hora no cae en ningún turno (18/20/22 h) la BLL usa `TurnoPrueba_790MY` (22:00) como turno vigente, y habilita la *Ficha de Ingredientes* para SuperAdmin. En producción debe ser `false`.
26. **¿Qué mejorarían primero?** — (1) Pruebas unitarias sobre BLL y `CalcularReglas`; (2) replicar en la BLL las validaciones que solo están en la GUI; (3) tabla propia de cierres de caja; (4) versionar solo cambios de negocio en `Productos_C`; (5) migraciones de esquema versionadas en el instalador. (Detalle en §8.)

---

## 8. RESUMEN CONSOLIDADO DE MEJORAS

Priorización: **A** = riesgo funcional o de seguridad (corregir antes de producción), **M** = calidad/consistencia, **B** = limpieza o mejora menor. Todas surgen de la lectura del código (los números entre paréntesis remiten a la sección).

### A — Prioridad alta

| # | Hallazgo | Dónde |
|---|---|---|
| A1 | Validaciones **solo en la GUI** (DNI 7–8 dígitos en cliente; DNI de empleado sin regla; rango `desde ≤ hasta` en reporte de reservas): replicarlas en la BLL<br>✅ **Resuelto (fase 1):** `ClienteBLL_790MY.ValidarDatos` exige `^\d{7,8}$` (`FC_val_dni_formato`) y `ReservaBLL_790MY.Buscar` rechaza `desde > hasta` (`msg_rep_rango_fechas_invalido`). *Pendiente:* DNI de empleado (no estaba en el plan). | 1.1, 2.1, 2.2, 3.1 |
| A2 | `ReservaBLL_790MY.CancelarReserva` **no valida permiso** (todas las demás operaciones sí)<br>✅ **Resuelto (fase 1):** valida `Permisos.RegistrarReserva`. Después, la refactorización de limpieza eliminó `CancelarReserva` y `FinalizarReserva` por no tener llamadores. | 1.1 |
| A3 | Orden inseguro en `FamiliaBLL_08YS`/`RolBLL_08YS`: se valida y consulta `IsInUse` **antes** de `ValidatePermission` (filtra información)<br>✅ **Resuelto (fase 1):** `ValidatePermission` es lo primero en Crear/Modificar/Eliminar de ambas BLL. | 2.5 |
| A4 | Cierre de caja sin tabla propia: depende del prefijo de un string en `Bitacora.TargetUsername` (50 caracteres) y no es atómico entre terminales<br>✅ **Resuelto (fase 2):** tabla `CierresCaja` (PK `Fecha`) con `ICierreCajaRepository_790MY`; doble cierre = violación de PK → `msg_ac_caja_ya_cerrada`; migración con copia de cierres históricos. *Pendiente:* `CierresCaja` fuera de los DV. | 3.2 |
| A5 | `Productos_C` versiona **cada descuento de stock**; restaurar una versión también restaura su stock viejo<br>✅ **Resuelto (fase 2):** `producto_actualizado` versiona solo cambios de negocio; un cambio solo de stock actualiza la vigente (siempre una sola `Act = 1`). *Pendiente:* el rollback (`producto_c_actualizado`) sigue restaurando el stock de la versión. | 2.4, 4.1 |
| A6 | `BitacoraBLL_08YS.GetAll/Filtrar` no validan permiso y la tabla `Bitacora` no entra en `TablasMonitoreadas` (DV)<br>✅ **Resuelto (fase 2):** `GetAll/Filtrar` validan `Permisos.VerBitacora` (único llamador: `FormBitacora_08YS`). *Pendiente:* `Bitacora` sigue fuera de `TablasMonitoreadas`. | 4.2 |
| A7 | `LatenciaSimuladaMs = 2500` (demora artificial del cobro): condicionarla a `ModoDemostracion`. La autorización bancaria simulada, el QR simulado y las acciones simuladas del Reporte Predictivo **se conservan**: no hay pasarela, verificador ni integración reales y son necesarias para el funcionamiento mínimo y la demostración<br>✅ **Resuelto (fase 3):** la demora solo se aplica con `ModoDemostracion=true`; sin la clave o con `false` no hay demora. | 1.5, 3.3 |
| A8 | `ModoDemostracion=true` y `TurnoPrueba_790MY` se distribuyen por defecto; usuario semilla `Maximo` (SuperAdmin) con hash en el script<br>✅ **Resuelto (fase 3) en lo pedido:** el instalador escribe `ModoDemostracion=false` salvo que se marque la casilla "Instalar en modo demostración". *Pendiente:* usuario semilla `Maximo` con hash en el script. | 6.1, 6.2 |
| A9 | Fallback `InstaladorBD_08YS` ignora lotes con error (base posible incompleta) y usa un `script.sql` distinto del `Deploy_TP_Ing_Soft.sql`<br>🟡 **Parcial (fase 4):** el instalador ejecuta siempre el `Deploy_TP_Ing_Soft.sql` idempotente y transaccional (también en reinstalación, con backup previo). *Pendiente:* el fallback `InstaladorBD_08YS` sigue usando `script.sql` e ignorando lotes con error (fuera del alcance). | 6.3 |

### M — Prioridad media

| # | Hallazgo | Dónde |
|---|---|---|
| M1 | `ClienteBLL_790MY` y `MesaBLL_790MY` usan `RegistrarEvento` en vez de `RegistrarEventoPostCommit` | 2.1, 2.3 |
| M2 | Mensajes **fijos en español** en la BLL (`ClienteBLL_790MY`, `MesaBLL_790MY`) en lugar de `MensajesBLL_790MY.Regla`; `GetTexto(..) ?? "x"` nunca usa el alternativo | 2.1, 2.3 |
| M3 | `MessageBox` en `FormBitacora_08YS`, `FormGestionAcceso_08YS`, `FormAccesoAM_08YS` (contradice E2-D1)<br>✅ **Resuelto (fase 3):** banner (`MostrarErrorEnBanner`), `ErrorProvider` en campos, `ToastHost_790MY` para éxitos y `ConfirmModal_790MY` para eliminar/reemplazar; `MessageBox` solo en el camino de error fatal. | 0.8, 2.5, 4.2 |
| M4 | Controles creados en `FormX.cs` (`CrearTarjetasKpi`, `ArmarTablaDiff`, `CrearTarjeta`, mapa de mesas): moverlos a UserControls en el Designer | 0.9, 3.2, 3.3, 4.1 |
| M5 | `MesaBLL_790MY` mezcla ABM y reglas de turno/disponibilidad; el tope de 12 mesas cuenta `FueraDeServicio` | 2.3 |
| M6 | PDF del Reporte de Reservas: sin paginación ni encabezado repetido, 200 líneas dentro del formulario (extraer a una clase servicio) | 3.1 |
| M7 | `FormBitacora_08YS`: "exportar" es solo impresión (sin PDF/CSV); `GetAll()` trae toda la tabla; pie "Sistema: GastroGest" | 4.2 |
| M8 | El Reporte Predictivo no tiene botón *Actualizar*; la BLL guarda estado cacheado en la instancia | 3.3 |
| M9 | Reinstalación sin control de versión de esquema (`SchemaVersion` + scripts incrementales); `GUI_08YS.exe.config` se pisa<br>🟡 **Parcial (fase 4):** reinstalación con `BACKUP DATABASE` previo + Deploy idempotente (sección "Actualizaciones de esquema" para columnas); `ModoDemostracion` se reescribe con la elección del asistente. *Pendiente:* tabla `SchemaVersion`. | 6.1 |
| M10 | Ayuda contextual acoplada por **nombre de control** (`nameof`); sin prueba que detecte pistas rotas; `ayuda.json` es por equipo | 5 |
| M11 | `FormComprobanteReserva_790MY` no fue auditado (no se leyó); polling de `timerAvisos_790MY`/`timerKds_790MY` | 1.1, 1.3, 1.4 |
| M12 | Dos caminos para pasar una mesa a fuera de servicio; `ModificarMesa` no revisa reservas futuras | 2.3 |

### B — Prioridad baja / limpieza

| # | Hallazgo | Dónde |
|---|---|---|
| B1 | Código muerto: `EmpleadoBLL_790MY.ObtenerUsuariosVinculables`, `IMesaRepository_790MY.Delete`, `TieneReservasActivas`<br>✅ **Resuelto (fase 3):** métodos e implementaciones eliminados; sin referencias en la solución. | 2.2, 2.3 |
| B2 | Constantes duplicadas entre BLL, `NumericUpDown.Maximum` y columnas SQL (`PrecioMaximo`…); color `(184, 84, 20)` del botón restaurar fijo en el `.cs` | 2.4, 4.1 |
| B3 | Legado: `Setup_08YS.vdproj` ("HorizonHotel&Resort"), `script.sql`, `Migraciones_790MY/*.sql` (13), `Scripts/AlterTableMesas_Mapa.sql` (borra datos): mover a `Legacy/` | 6 |
| B4 | Comentario de cabecera del `.iss` desactualizado ("base creada previamente"); `PrivilegesRequired=admin` obligatorio; desinstalación borra `{app}\*`<br>🟡 **Parcial (fase 4):** encabezado del `.iss` actualizado. *Pendiente:* `PrivilegesRequired=admin` y el borrado de `{app}\*` (incluye `Database\Backups`) al desinstalar. | 6.1 |
| B5 | `catch (Exception) { }` vacío al abrir el PDF de reservas; `ContarMesasAbiertas` devuelve 0 ante cualquier error | 2.2, 3.1 |
| B6 | `InvalidOperationException` usada para validaciones de entrada en Accesos; `Familia_08YS.Hijos` mutable vía `ReemplazarHijos` | 2.5 |
| B7 | Sin proyecto de **pruebas unitarias** en la solución (`CalcularReglas`, `CalcularImportes`, `ValidarTarjetaLuhn` son puros y fáciles de probar) | 1.5, 3.3 |

### Lo que conviene decir en la defensa como fortalezas

- Doble validación de permisos (GUI oculta, BLL valida) con Composite de roles/familias/permisos.
- Concurrencia resuelta en base (`SERIALIZABLE` + `UPDLOCK/HOLDLOCK`, `UPDATE` condicional con `OUTPUT`, bloqueo de productos por ID).
- Auditoría completa: bitácora de eventos catalogados + versionado de productos por triggers con rollback + dígitos verificadores.
- Reglas de negocio puras y reutilizables (`CalcularImportes`, `CalcularReglas`, `ValidarTarjetaLuhn`).
- Internacionalización completa (1294 claves es / 1295 en) con patrón Observer.
- Instalador reproducible (Inno Setup) con reinstalación segura, secretos fuera del paquete y script de BD idempotente y transaccional.
- Sistema de ayuda contextual que se adapta a permisos y a lo que el usuario realmente ve.

---

*Fin de la Guía Técnica de Defensa.*
