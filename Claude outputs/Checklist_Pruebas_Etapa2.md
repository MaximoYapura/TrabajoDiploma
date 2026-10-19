# Checklist de pruebas — Etapa 2 (antes del push)

Rama: `beta` (4f08437). Nada de esto se probó todavía en la app real.
Marcá cada ítem con [x]. Si algo falla, anotá formulario, acción, escala e idioma.

## 0. Condiciones generales (aplican a cada pantalla)

- [x] Repetir las pantallas clave a **100 %, 125 % y 150 %** de escala de Windows.
- [x] Repetir en **español e inglés** (cambiar idioma con la pantalla abierta: banner, títulos y textos deben actualizarse).
- [x] Ningún texto aparece como clave cruda (ej. `FC_val_dni_formato`) ni vacío. Se borraron 25 claves de `idiomas.json`: si alguna se usaba armada en tiempo de ejecución, se vería así.
- [x] Ningún `MessageBox` gris de Windows aparece salvo en errores fatales (base caída, excepción inesperada).

## 1. Componentes nuevos (se ven en todas las pantallas)

| Componente | Qué verificar |
|---|---|
| `ConfirmModal_790MY` | Se centra sobre la ventana; título, mensaje y botones legibles; **el foco arranca en Cancelar**; Enter no confirma por accidente; Esc y la X cancelan; texto largo hace salto de línea sin cortarse. |
| `Banner_UC_790MY` | Aparece arriba y **empuja** el contenido (no lo tapa); crece en alto con mensajes de varias líneas; colores rojo (error), ámbar (aviso); la ✕ lo cierra cuando corresponde; el resumen de validación **no** tiene ✕ y se va solo al corregir. |
| `ToastHost_790MY` | Aparece abajo a la derecha, no roba el foco, se cierra solo a los **4 s** o con un clic; un segundo toast reemplaza al anterior. |
| `ErrorProvider` | El ícono rojo queda **dentro** del borde derecho del campo sin tapar texto importante; el tooltip muestra el motivo; desaparece al corregir. |

## 2. Maestros

### FormClientes_790MY
- [x] **Alta vacía + Aplicar**: banner con la lista de campos faltantes, íconos en cada campo, foco en el primero.
- [x] **Validación al salir del campo**: un campo vacío se marca recién al salir de él, no al abrir la ficha.
- [x] **Corregir campos**: el ícono se va al instante y el banner desaparece cuando ya no hay errores.
- [x] **DNI**: rechaza 6 dígitos, 9 dígitos, puntos y guiones; acepta 7 y 8.
- [x] **Dirección vacía**: se guarda sin error (ahora es opcional).
- [x] **Celular vacío o con letras**: se marca como error.
- [x] **Email mal formado**: se marca como error.
- [x] **Alta correcta**: toast «Se registró a…», la ficha queda limpia y el foco en DNI (sigue en modo alta).
- [x] **DNI duplicado** (incluido uno dado de baja): banner con el mensaje que menciona que puede estar dado de baja; ícono en DNI; al editar el DNI el banner se va.
- [x] **Modificación correcta**: toast «Cambios guardados», queda seleccionado el cliente con los datos guardados.
- [x] **Eliminar**: ConfirmModal con nombre y DNI; Cancelar no hace nada; confirmar da toast y el cliente desaparece de la lista.
- [x] **Email/celular adicional repetido**: ícono en el campo adicional (ya no hay etiqueta roja).
- [x] **Serializar sin selección**: banner ámbar. **Sin ruta**: ícono en la ruta. **Correcto**: toast con la cantidad.
- [x] **Deserializar sin ruta / archivo inválido**: ícono o banner según el caso; nada de MessageBox.
- [x] **«Guardar o descartar»** (cambiar de fila con cambios): sigue funcionando; Guardar sobre un alta sale a la fila elegida.

### FormMesas_790MY
- [x] **Añadir + Aplicar**: toast «Se registró la mesa N»; la ficha queda lista con el próximo número.
- [x] **Límite de 12 mesas**: el error de la BLL aparece en el banner.
- [x] **Modificar capacidad**: toast; la mesa queda seleccionada.
- [x] **Pasar a «Fuera de servicio» y volver a «Disponible»**: funciona (decisión R2).
- [x] **Mesa Ocupada o Reservada modificada desde el maestro**: el estado real no se pisa.
- [x] **Eliminar mesa sin reservas**: ConfirmModal explica que pasa a «Fuera de servicio»; toast; la tarjeta queda fuera de servicio.
- [x] **Eliminar mesa con reservas en el historial**: banner con el rechazo de la BLL.
- [x] **Clic en otra tarjeta**: el banner anterior se oculta.

### FormEmpleados_790MY
- [x] **Guardar con DNI vacío**: ícono en DNI + banner.
- [x] **Alta correcta**: toast; ficha limpia, foco en DNI.
- [x] **Modificación**: toast; queda seleccionado el empleado.
- [x] **Dar de baja**: ConfirmModal; confirmar lo deja inactivo.
- [x] **Baja de camarero con mesas abiertas**: rechazo de la BLL en banner.
- [x] **DNI duplicado / usuario ya vinculado**: banner.

### FormProductos_790MY
- [x] **Alta correcta**: toast; ficha limpia, foco en Nombre.
- [x] **Modificación**: toast.
- [x] **Nombre vacío o precio inválido**: error de la BLL en banner.
- [x] **Dar de baja**: ConfirmModal («dejará de aparecer en la carta»); el producto queda inactivo.

### FormListaSimple_790MY (desde el alta rápida de cliente, botones «+»)
- [x] **Agregar valor repetido**: ícono en el campo; desaparece al escribir.
- [x] **Quitar sin selección**: ícono en la lista; desaparece al elegir un valor.

## 3. RF1 — Reservas

### FormRegistrarReserva_790MY
- [x] **Buscar con DNI de 6 o 9 dígitos**: ícono + banner.
- [x] **Buscar cliente dado de baja**: banner ámbar «está dado de baja».
- [x] **Buscar DNI inexistente**: ConfirmModal «No hay ningún cliente con el DNI…»; «Registrar cliente» abre el alta rápida con el DNI cargado; Cancelar no abre nada.
- [x] **Seleccionar Mesa sin turno**: ícono en el combo de turno (que no tape la flecha) + banner.
- [x] **Sin mesas disponibles**: banner ámbar; al cambiar fecha, turno o comensales se oculta.
- [x] **Confirmar con todo vacío**: banner con los 3 faltantes e íconos en DNI, turno y botón de mesa; se van a medida que se completa.
- [x] **Confirmar correcto**: se abre el comprobante; al cerrarlo, toast «Reserva N registrada para…»; el formulario queda limpio.

### FormRegistrarCliente_790MY (alta rápida)
- [x] **Aplicar vacío**: banner con resumen; **el diálogo crece** para mostrarlo sin apretar los campos; al corregir, vuelve a su alto.
- [x] **Email vacío**: ahora es obligatorio (antes no).
- [x] **Celular vacío**: obligatorio. **Dirección vacía**: se acepta.
- [x] **Alta correcta**: se cierra y el cliente aparece cargado en la reserva / walk-in.
- [x] **DNI duplicado**: banner + ícono en DNI.

### FormSeleccionarMesa_790MY
- [x] **El plano se ve exactamente igual que antes** (posiciones, tamaños, leyenda, colores).
- [x] **Error de la BLL al cargar el mapa** (si se puede forzar): banner **flotante** sobre la franja superior del plano, sin mover nada; se cierra con ✕.
- [x] **Aceptar sin mesa** (si el botón llegara a estar habilitado): banner flotante; al elegir mesa se oculta.

### FormComprobanteReserva_790MY
- [x] **Guardar PDF correcto**: ConfirmModal «Comprobante guardado» con la ruta; «Abrir PDF» lo abre, «Ahora no» no.
- [x] **Guardar sobre un PDF abierto en otro programa**: banner con el error; el diálogo crece y la vista previa no se recorta.
- [x] **Envío de email**: el indicador de estado sigue funcionando igual.

## 4. RF2 — Atención en mesa

### FormAbrirMesa_790MY
- [x] **Durante el turno, «Confirmar» habilitado aunque falten datos**; fuera de turno, deshabilitado.
- [x] **Confirmar sin reserva ni camarero**: banner con faltantes + íconos en la lista de reservas y en la de camareros (esquina superior derecha, sin tapar tarjetas).
- [x] **Modo Walk-in sin cliente ni mesa**: íconos en DNI y en «Seleccionar mesa en el plano».
- [x] **Walk-in con DNI inválido / dado de baja / inexistente**: igual que en Registrar Reserva (ícono, banner ámbar, ConfirmModal + alta rápida).
- [x] **Sin disponibilidad**: banner ámbar; al cambiar comensales se oculta.
- [x] **Apertura correcta**: toast «Se abrió la mesa N…»; el formulario se limpia.
- [x] **Apertura concurrente** (otra terminal abrió la misma reserva): banner con el error, la lista se recarga.

### FormRegistrarPedido_790MY
- [x] **Elegir producto sin mesa** (usuario sin mesas): banner ámbar; se oculta al elegir una mesa.
- [x] **Enviar comanda**: toast «Pedido N° … registrado y enviado a cocina».
- [x] **Stock insuficiente**: banner con el error; la carta se refresca.
- [x] **Usuario no vinculado a un camarero**: banner persistente al abrir la pantalla.
- [x] **Aviso de pedido listo** (panel superior verde) sigue funcionando y no choca con el banner.

### FormCocinaKDS_790MY
- [x] **Marcar pedido como listo**: toast (claro sobre el tema oscuro: verificar que se lea bien).
- [x] **Iniciar preparación**: la tarjeta cambia de columna, sin aviso (igual que antes).
- [x] **Error al cambiar estado**: banner arriba; se limpia en la siguiente acción.
- [x] **El refresco automático** no muestra errores emergentes.

### Fuera de alcance (deben seguir IGUAL que antes)
- [x] `FormFacturacionCobro_790MY`: sus mensajes siguen siendo MessageBox (camino legacy).
- [x] `FormComprobanteFactura_790MY`: igual que antes.
- [x] Formularios `_08YS` (Login, MDI, Administración, Bitácora, Respaldos, Contraseña): sin cambios.

## 5. Reportes y Bitácora

### FormReporteReservas_790MY
- [x] **Filtro DNI con 6 dígitos o letras**: ícono + banner. DNI vacío: busca sin filtro.
- [x] **Generar PDF correcto**: toast «Se generó el PDF del reporte» y se abre el archivo.
- [x] **PDF sobre un archivo abierto**: banner con el error.
- [x] **Usuario sin permiso de ver reservas**: banner (no MessageBox).

### FormArqueoCaja_790MY
- [x] **Cerrar caja**: ConfirmModal «¿Cerrar la caja del dd/mm/aaaa?»; confirmar da toast con cuentas y recaudación neta; las tarjetas muestran los totales.
- [x] **Error al cerrar** (caja ya cerrada, si aplica): banner.

### FormReportePredictivo_790MY
- [x] **Botón de una sugerencia**: ConfirmModal que **avisa que es simulada antes** de aceptar; aceptar da toast con la confirmación.
- [x] **Mover el slider de confianza**: si hay error, banner.

### FormBitacoraCambios_790MY
- [x] **Código de producto 0 o con letras**: ícono + banner; al volver a buscar con un valor válido se limpia.
- [x] **Restaurar versión**: ConfirmModal con el detalle de cambios, foco en Cancelar; confirmar da toast y la versión pasa a vigente.
- [x] **Error al restaurar**: banner (después de recargar la lista).

## 6. Compilación y entrega

- [x] **Abrir la solución en Visual Studio y compilar en Debug.** Ojo: `TrabajoDiploma.slnx` tiene MPP marcado como «no compilar» en Debug desde el primer commit; si falla con errores de `MPP_08YS` en DAL, es por eso (no por la Etapa 2). Release y la compilación del proyecto GUI dan 0 errores.
- [x] `FormPruebaMapa_08YS.cs` ya no está en `GUI/RF1`.
- [x] Recién con todo lo anterior OK: `git push origin beta` (y las ramas `etapa2/*` si las querés en GitHub).
