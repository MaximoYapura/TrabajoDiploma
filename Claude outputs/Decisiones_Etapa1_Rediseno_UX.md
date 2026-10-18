# Decisiones aprobadas — Rediseño UX/UI «El Fogón del Sur» (Etapa 1)

Fuente: auditoría y plan en cuatro etapas (artifact «Rediseño El Fogón del Sur», v2). Decisiones del propietario del proyecto, 2026-10-17.
Este documento manda sobre el artifact: si hay contradicción, vale lo que dice acá.

## Cómo trabajar (reglas del proyecto, siempre vigentes)
- Arquitectura en capas: GUI, BLL, DAL. En la Etapa 1 NO se toca BE, BLL, DAL, MPP ni Service_08YS.
- Cada formulario se entrega con `FormX.Designer.cs` (solo diseño visual, dentro de `InitializeComponent()` o auxiliares privados) y `FormX.cs` (solo lógica, eventos y comunicación con capas inferiores). Constructor limpio.
- Se conservan namespace, clases parciales y nombres de controles existentes.
- Una rama y una entrega por formulario. Texto nuevo en `idiomas.json` (es/en). No cambian permisos ni eventos de bitácora.
- Cada entrega se prueba a 100, 125 y 150 % de escala y en español e inglés.
- No inventar funcionalidades ni datos. Si algo no está claro en el código, se pregunta antes.

## Propuestas APROBADAS: P01 a P18
- P01 Dos niveles de diseño con los mismos tokens (identidad premium / operación).
- P02 Regla de descarte: si un recurso de marca retrasa una tarea operativa, se descarta.
- P03 Paleta única: acción #6B1D2F (hover #561625), marca #610D10 (estimado del logo), oros #F2D9A6 y #E8B964, crema #FAF8F5, panel #F4F0E9, línea #E2D8CA, tintas #2D2A28 y #6E645F.
- P04 Bordó sólido = acción. Estados con superficie suave, ícono y texto.
- P05 Tema oscuro de cocina con sus valores propios (solo definición; el KDS no se toca en Etapa 1).
- P06 Segoe UI en toda la aplicación y escala tipográfica fija.
- P07 La tipografía slab del logo queda solo en la imagen del logo. No se empaqueta ninguna fuente.
- P08 Espaciado en múltiplos de 4, radios 6/10/12, sin sombras.
- P09 Reglas de animación, respetando «reducir animaciones».
- P10 Catálogo de componentes (nombres orientativos).
- P11 Tablero ordenado por número, **solo en el formulario Maestro de Mesas (`FormMesas_790MY`)**.
- P12 Maestro con estado visible y panel lateral de edición (ver restricciones de H10 a H12).
- P13 Selección en reserva: **SUSPENDIDA** por la restricción de abajo (ver «Restricción estricta»).
- P14 «Fuera de servicio» solo como visualización de lectura (ver H11).
- P15 Clientes: lista a la izquierda, ficha a la derecha, búsqueda siempre visible.
- P16 Validación en línea con las mismas reglas actuales. Guardar explica por qué está deshabilitado.
- P17 «Serialización XML» plegada. Emails y teléfonos adicionales como etiquetas en la ficha.
- P18 Aviso en línea «Guardar o descartar» al cambiar de fila con datos sin guardar.

## Restricción estricta
**Está prohibido tocar el formulario «Seleccionar Mesa» (`FormSeleccionarMesa_790MY`) en esta etapa.**
- Tampoco se modifica `MesaVisualControl_790MY`, que usa ese formulario. La corrección del `SolidBrush` sin `Dispose` (D12) queda pendiente por esta razón.
- Si más adelante se considera que «Seleccionar Mesa» necesita cambios, primero se plantean beneficios y contras para debatirlo. No se cambia nada hasta que se apruebe.

## Respuestas a las hipótesis de la Etapa 1
- **H10 (búsqueda y filtros):** el restaurante tiene un máximo físico de 10 o 12 mesas. Buscar y filtrar en Mesas NO es prioritario: no se implementa. Para Clientes sí es prioritario, porque no hay límite de registros.
- **H11 (Fuera de servicio):** el Maestro de Mesas NO debe permitir cambiar una mesa a «Fuera de servicio». Por regla de negocio, una mesa en ese estado está rota y no se restaura por sistema; se reemplaza físicamente. Solo puede mostrarse el estado si ya existe en los datos.
- **H12 (datos en el maestro):** el Maestro de Mesas muestra solo datos estructurales de la mesa (número, capacidad, estado). No se agregan comensales ni minutos de ocupación: eso corresponde a otros módulos.
- **H13 (reglas de Cliente):** el celular es OPCIONAL. El comprobante de reserva se envía por email. Enviar un PDF por celular (WhatsApp o SMS) sería una funcionalidad nueva, que se evalúa con costos y tiempos y no se agrega ahora.
- **H14 (registro rápido):** el modo «registro rápido» se invoca solo cuando no hay cliente al tomar una Reserva (o en Walk-in). En el Maestro de Clientes, el alta y la edición se hacen dentro del mismo formulario, SIN invocar el registro rápido.

## Pendiente de decisión (no implementar todavía)
P19 a P37 y las hipótesis H01 a H09, H15 a H18 siguen sin resolver. No se trabaja en Etapas 2 a 4 hasta que se aprueben.

## Para verificar en el código antes de implementar (no asumir)
1. Si el celular hoy se exige en la validación de `FormClientes_790MY`. Si es así, hay una contradicción con H13: se consulta antes de cambiar nada.
2. Cómo se invoca hoy el modo de registro rápido y desde qué formularios. El texto de H14 nombra `FormCliente`; la clase real parece ser `FormClientes_790MY` (existe también `FormRegistrarCliente`). Confirmar nombres.
3. FormEmpleados y FormProductos solo se revisaron por estructura; no entran en la Etapa 1 hasta revisarlos.
4. Cuántos usos de `FormSeleccionarMesa` o `MesaVisualControl_790MY` dependen de estilos compartidos (`EstiloRF2_790MY`, `EstiloMaestros_790MY`): los valores actuales se conservan hasta migrar cada pantalla, para no alterar el formulario prohibido.
