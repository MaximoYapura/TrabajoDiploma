# Decisiones aprobadas — Rediseño UX/UI «El Fogón del Sur» (Etapas 1 y 2)

Fuente: auditoría y plan en cuatro etapas (artifact «Rediseño El Fogón del Sur», v2). Decisiones del propietario del proyecto.
Este documento manda sobre el artifact: si hay contradicción, vale lo que dice acá. Reemplaza a «Decisiones Etapa 1 - Rediseño UX».
Última actualización: 2026-10-18.

## Estado
- Etapa 1 (quick wins y sistema visual): TERMINADA. Existe la clase de tema `TemaFogon` (GUI).
- Etapa 2 (interacción y UX, reemplazo de MessageBox): MIGRACIÓN COMPLETA en ramas encadenadas `etapa2/*` (la última, `etapa2/reportes-y-bitacora`, contiene todo salvo `etapa2/limpieza-prueba-mapa`, que sale de `beta`). Pendiente: pruebas de integración en la app (100/125/150 %, es/en). Quedan en el camino legacy, por decisión, `FormFacturacionCobro_790MY` y `FormComprobanteFactura_790MY` (Etapa 4) y todos los `_08YS`.
- Etapa 3 (KDS): decisiones E3-D1 a E3-D6 aprobadas y Opción C implementada (B medida y descartada); ver «Decisiones_Etapa3_KDS.md». Etapa 4: sin decisiones.

## Etapa 2 — respuestas al inventario (2026-10-18, vigentes)
- **Formularios `_08YS` EXCLUIDOS** (Login, MDI, Administración, Bitácora, Respaldos, Contraseña, etc.): prohibido modificarlos en esta etapa y en las siguientes. Solo se evaluarán al final del proyecto si fuera estrictamente necesario.
- R1. Dirección del cliente: OPCIONAL en la GUI (la BLL no la exige). Resuelve la contradicción 2.
- R2. «Fuera de servicio» en el Maestro de Mesas: SE MANTIENE (el administrador deshabilita una mesa rota desde el panel de edición). **Reemplaza lo dicho en P14** sobre que el maestro no permite pasar una mesa a «Fuera de servicio».
- R3. DNI: la GUI valida 7 u 8 dígitos numéricos antes de llamar a la BLL.
- R5. `FormPruebaMapa_08YS`: ELIMINADO (rama `etapa2/limpieza-prueba-mapa`). La exclusión de los `_08YS` aplica a los formularios activos de infraestructura y producción, no a prototipos sin uso.
- R6. `FormSeleccionarMesa_790MY`: se aprueba un banner o aviso superpuesto, sin modificar estructura ni controles del plano.
- R7. `EstiloRF2_790MY`: camino legacy. `MostrarError`/`MostrarExito` quedan intactos (MessageBox) para `FormFacturacionCobro_790MY` y las pantallas no migradas; las migradas usan `MostrarErrorEnBanner(banner, ex)` y `MostrarExitoToast(ancla, mensaje)`.
- R8. Errores de campo: `ErrorProvider` en toda la app, más el banner del formulario (`Banner_UC`). En Clientes reemplaza a las etiquetas de error de la Etapa 1.
- R9. Toda confirmación (bajas, «¿registrar cliente?», «¿abrir el PDF?», etc.) usa el modal propio `ConfirmModal` con `TemaFogon`.
- Componentes creados en `GUI/Estilo`: `ConfirmModal_790MY`, `Banner_UC_790MY`, `ToastHost_790MY`.

## Reglas de trabajo (siempre vigentes)
- Arquitectura en capas GUI, BLL, DAL. En las Etapas 1 y 2 NO se toca BE, BLL, DAL, MPP ni Service_08YS.
- Cada formulario se entrega con `FormX.Designer.cs` (solo diseño visual, dentro de `InitializeComponent()` o auxiliares privados) y `FormX.cs` (solo lógica, eventos y comunicación con capas inferiores). Constructor limpio.
- Se conservan namespace, clases parciales y nombres de controles existentes.
- Una rama y una entrega por formulario. Texto nuevo en `idiomas.json` (es/en). No cambian permisos ni eventos de bitácora.
- Cada entrega se prueba a 100, 125 y 150 % de escala y en español e inglés.
- No inventar funcionalidades ni datos. Si algo no está claro en el código, se pregunta antes.

## Etapa 1 — decisiones vigentes (P01 a P18 aprobadas)
- P01 a P10: dos niveles de diseño, regla de descarte, paleta única (acción #6B1D2F, hover #561625, marca #610D10, oros #F2D9A6 y #E8B964, crema #FAF8F5, panel #F4F0E9, línea #E2D8CA, tintas #2D2A28 y #6E645F), bordó sólido = acción, Segoe UI, sin sombras, reglas de animación, catálogo de componentes.
- P11: tablero de mesas ordenado por número, solo en el Maestro de Mesas (`FormMesas_790MY`).
- P12 a P14: el Maestro de Mesas muestra estado y panel lateral de edición. Sin búsqueda ni filtros (hay máximo 10 o 12 mesas). No muestra comensales ni ocupación. Los estados del maestro se limitan a **Disponible** y **Fuera de servicio**, y el maestro no permite pasar una mesa a «Fuera de servicio» (una mesa rota se reemplaza físicamente).
- P15 a P18: Clientes con lista, ficha y búsqueda; validación en línea; «Serialización XML» plegada; aviso «Guardar o descartar».
- Clientes: el alta y la edición se hacen dentro del mismo formulario del maestro, SIN invocar el registro rápido. El registro rápido se usa solo al tomar una Reserva sin cliente existente o en Walk-in.
- Celular por WhatsApp o SMS: funcionalidad nueva, no se agrega sin evaluar costos y tiempos. El comprobante se envía por email.

### Alcance sobre `FormSeleccionarMesa_790MY` (aclaración vigente)
- NO se cambia su maquetación visual, para no alterar el flujo de recepción. P13 (selección en reserva con tarjetas) sigue suspendida. Si más adelante se quiere cambiar su aspecto, se presentan primero beneficios y contras para debatirlo.
- SÍ entra en la Etapa 2, solo para reemplazar sus `MessageBox` por avisos o validaciones en pantalla, sin tocar el maquetado.
- `MesaVisualControl_790MY` sigue sin tocarse; el arreglo del `SolidBrush` sin `Dispose` (D12) queda pendiente de decisión.

## Etapa 2 — decisiones aprobadas
**E2-D1. Confirmaciones.**
- Bajas y eliminaciones: diálogo MODAL propio, con el tema `TemaFogon`, que reemplaza al `MessageBox` nativo.
- Modificaciones y guardados exitosos: avisos o banners no bloqueantes.
- Los `MessageBox` nativos se conservan solo para errores fatales (por ejemplo, base de datos inaccesible o excepción inesperada).

**E2-D2. Alcance y orden.**
- Orden: validaciones de maestros, luego avisos de éxito, luego confirmaciones, y al final RF1 y RF2. Dentro de cada grupo se prioriza por cantidad de llamadas a `MessageBox` (E2-H2).
- Queda fuera `FormFacturacionCobro` (Etapa 4).
- `FormSeleccionarMesa` entra solo para sus `MessageBox`.
- Antes de migrar nada: inventario de las llamadas a `MessageBox`, clasificadas en validación, confirmación, éxito, error recuperable y error fatal. La tabla se presenta para aprobación.

**E2-D3. Textos.** Se reescriben en lenguaje claro (qué ocurrió y cómo solucionarlo) y se centralizan en `idiomas.json` (es/en).

**E2-D4. Comportamiento.**
- Validación al salir del campo con `ErrorProvider`, y resumen de errores en un banner al intentar guardar.
- `ToastHost` para éxitos, que se cierran solos a los 4 s. Los errores van en banners y permanecen hasta corregirse.
- Altas: se limpia el formulario y el foco va al primer campo. Modificaciones: se mantienen los datos cargados.
- Los errores que devuelve el BLL se muestran en el banner del formulario.

**E2-H3. Reglas duplicadas en la GUI.** La GUI repite validaciones simples (requerido, formato de email, solo números) antes de llamar al BLL. El BLL sigue siendo la autoridad.

## Reglas de negocio de Cliente (respuestas del propietario)
- DNI: solo caracteres numéricos, sin puntos ni guiones, de 7 u 8 dígitos.
- Obligatorios (por BLL): email, nombre, apellido y celular.
- Dirección: opcional.
- «Inactivo»: baja lógica del cliente. Solo usuarios con rol Administrador pueden cambiar ese estado (con el sistema de permisos existente; no se modifica).

### Contradicciones a resolver (no implementar hasta aclararlas)
1. **Celular.** En la Etapa 1 se respondió que era OPCIONAL (H13). En la Etapa 2 se respondió que es OBLIGATORIO por BLL. Vale la respuesta más reciente (obligatorio). Hay que verificar que lo hecho en la Etapa 1 sea coherente y corregirlo si no.
2. **Dirección.** Se indica que es opcional, pero el análisis del código indicó que `FormClientes_790MY` la exige. Si el código o el BLL la exigen, no se cambia la regla en la GUI: se consulta antes, porque cambiaría una regla de negocio.
3. **Estados de mesa.** Los estados del maestro son Disponible y Fuera de servicio; los estados Ocupada y Reservada pertenecen a otros módulos. Verificar que ninguna pantalla del maestro los ofrezca.

## Lecciones de la Etapa 1 (para respetar en las siguientes)
- Limpiar código muerto y controles en desuso al migrar cada formulario.
- Mantener delimitadas las responsabilidades de cada módulo (por ejemplo, el Maestro de Mesas no muestra datos de ocupación).

## Pendiente de decisión (no implementar todavía)
- Etapas 3 y 4: P19 a P37 y las hipótesis H01 a H09, H15 a H18 del artifact.
- Arreglo del `SolidBrush` sin `Dispose` en `MesaVisualControl_790MY`.

## Para verificar en el código antes de implementar (no asumir)
1. Inventario completo de `MessageBox` en la GUI (el análisis contó 145; Clientes 21).
2. Contradicciones 1 y 2 de arriba.
3. Cuántos `MessageBox` de `FormSeleccionarMesa` son validación, confirmación o error, sin tocar su maquetación.
4. FormEmpleados y FormProductos solo se revisaron por estructura; revisarlos antes de migrarlos.
5. Cómo se muestra hoy el diálogo modal estilizado más cercano que pueda reutilizarse como base del diálogo de baja con `TemaFogon`.
