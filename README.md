# El Fogón del Sur — Sistema de gestión del restaurante

Aplicación de escritorio en C# WinForms (.NET Framework 4.8) con base de datos SQL Server.

**Esta versión corresponde a la Entrega 2.** Abarca:

- **Reservas (RF1):** registro de reservas con selección de mesa en el plano y comprobante en PDF.
- **Atención en mesa (RF2):**
  - CUN-004 Abrir Mesa (cliente con reserva)
  - CUN-005 Registrar Walk-in (cliente sin reserva)
  - CUN-006 Registrar Pedido
  - CUN-007 Cobrar Venta (Factura B con IVA contenido)
  - CUN-008 Despachar Pedido (cocina)
- **Caja y reportes:** arqueo y cierre de caja, exportación de la factura en PDF, reporte de reservas y análisis predictivo de canasta.
- **Seguridad y auditoría:** usuarios, roles y familias de permisos, bitácora, dígitos verificadores, respaldos y bitácora de cambios de productos con rollback.

Es un trabajo académico. Los datos fiscales (CUIT, CAE, QR) son ficticios y los comprobantes llevan la leyenda «Maqueta académica · sin validez fiscal».

---

## 1. Puesta en marcha

Hay dos formas de probar el sistema:

- **A. Con el instalador** (sección 1.1). Es lo más rápido. Crea la base y deja la aplicación lista.
- **B. Desde el código fuente con Visual Studio** (sección 1.2).

### 1.1 Opción A: instalador

**Requisitos:**

- Windows 10 u 11 de 64 bits.
- .NET Framework 4.8.
- SQL Server 2016 SP1 o posterior: Express, Developer o LocalDB.
- `sqlcmd` en el PATH. Se instala con SQL Server o con las *Microsoft Command Line Utilities for SQL Server*.

**Pasos:**

1. Ejecutar `Installer\Output\Setup_ElFogonDelSur.exe` como administrador.
2. Elegir la carpeta de destino (por defecto `C:\Program Files\El Fogon del Sur`).
3. Elegir la **instancia de SQL Server**. El desplegable lista las detectadas:
   - `.` para la instancia predeterminada;
   - `.\SQLEXPRESS` u otra instancia con nombre;
   - `(localdb)\MSSQLLocalDB`.

   El botón «Probar conexión» comprueba que la instancia responda.
4. La opción «Instalar en modo demostración» viene **marcada** (ver 1.3). Desmarcala solo para un uso real: sin ella, las mesas se abren únicamente en los horarios de atención.
5. Terminar. El instalador:
   - crea la base `TP_Ing_Soft` completa (esquema, datos de prueba, los usuarios de la sección 2 y dígitos verificadores);
   - escribe la instancia elegida en la cadena de conexión;
   - crea los accesos directos. En el menú Inicio queda también «Léame», que abre este archivo.

Si la aplicación ya estaba instalada, el mismo setup la **actualiza**: hace un backup de la base, aplica solo los cambios de esquema y no toca los datos.

**Dónde quedan backups y logs:**

- Backups: `C:\ProgramData\El Fogon del Sur\Backups`.
- Logs de `sqlcmd`: `C:\ProgramData\El Fogon del Sur\Logs`.

### 1.2 Opción B: desde el código fuente

**Requisitos:**

- **Visual Studio 2022 (17.13 o posterior) o Visual Studio 2026**, con la carga de trabajo «Desarrollo de escritorio de .NET».
  - La solución está en formato `.slnx`, que versiones anteriores no abren.
- .NET Framework 4.8 (Developer Pack).
- Una instancia de SQL Server. Sirve la **LocalDB** que se instala con Visual Studio (componente «SQL Server Express LocalDB»).
- **Conexión a internet** la primera vez, para descargar los paquetes NuGet.

**Pasos:**

**0. Desbloquear el zip (si se descargó de internet).** Windows marca los archivos descargados. Si el zip se descomprime con esa marca, Visual Studio se niega a compilar los formularios y muestra errores «no se pudo procesar el archivo ….resx porque se encuentra en Internet o en la zona restringida».

- **Antes de descomprimir:** clic derecho sobre el zip › **Propiedades** › tildar **Desbloquear** › **Aceptar**. Después, descomprimir.
- **Si ya se descomprimió:** abrir PowerShell en la carpeta extraída, ejecutar el comando de abajo y volver a abrir la solución.

```powershell
Get-ChildItem -Recurse | Unblock-File
```

**1. Abrir la solución.** Abrir `TrabajoDiploma.slnx` con Visual Studio.

**2. Recargar los paquetes NuGet.** La carpeta `packages` no está en el repositorio: hay que descargar los paquetes antes de compilar. Sin ellos, la compilación falla con errores sobre `FontAwesome.Sharp`, `IconChar`, `iTextSharp` o `Newtonsoft.Json`.

| Paquete | Versión | Proyectos | Para qué |
|---|---|---|---|
| FontAwesome.Sharp | 6.6.0 | GUI_08YS | Íconos de la interfaz |
| iTextSharp | 5.5.13.3 | GUI_08YS, BLL_08YS | Comprobantes y reportes en PDF |
| Portable.BouncyCastle | 1.9.0 | GUI_08YS, BLL_08YS | Dependencia de iTextSharp |
| Newtonsoft.Json | 13.0.4 | GUI_08YS, Service_08YS | Archivo de idiomas |

1. En el **Explorador de soluciones**, clic derecho sobre la solución › **Restaurar paquetes NuGet**.
2. Si después de compilar siguen los errores, o las referencias aparecen con un triángulo amarillo, reinstalarlos desde **Herramientas › Administrador de paquetes NuGet › Consola del Administrador de paquetes**:

   ```powershell
   Update-Package -reinstall
   ```

   El comando vuelve a instalar en cada proyecto los mismos paquetes y versiones de la tabla. No los actualiza.
3. Si el problema sigue, cerrar y volver a abrir la solución, y compilar de nuevo.

**3. Elegir la instancia de SQL Server.** La cadena de conexión está en `GUI\Config\App.config` y por defecto apunta a la instancia predeterminada (`.`). Si solo tenés LocalDB, cambiá el `Data Source` por `(localdb)\MSSQLLocalDB` (ver la sección 5).

**4. Ejecutar.**

1. Definir **GUI_08YS** como proyecto de inicio.
2. Compilar y ejecutar (F5).

**No hace falta correr ningún script.** Si la base `TP_Ing_Soft` no existe en la instancia configurada, la aplicación la crea al arrancar a partir de `GUI\script.sql`, que es una copia completa del script de instalación: incluye los usuarios de la sección 2 y los datos de prueba.

> Si la base ya existía de una versión anterior, la aplicación no la actualiza sola. Para actualizarla, correr el script a mano (sección 4) o borrar la base y volver a abrir la aplicación.

### 1.3 Modo demostración

Los turnos de atención son a las **18:00, 20:00 y 22:00**, de 2 horas cada uno. Fuera de ese horario no se puede abrir una mesa.

Con el modo demostración activo:

- fuera de horario, la aplicación usa el turno de prueba de las **22:00**, así que se puede probar a cualquier hora;
- se habilita la Ficha de Ingredientes de la Ayuda;
- el cobro con tarjeta simula la demora del banco.

Viene **activo** tanto con el instalador (la casilla está marcada) como desde el código. Para cambiarlo, editá el archivo de configuración de la aplicación:

- instalada: `GUI_08YS.exe.config`, en la carpeta de instalación;
- desde el código: `GUI\Config\App.config`.

```xml
<add key="ModoDemostracion" value="true" />
<add key="TurnoPrueba_790MY" value="22:00" />
```

---

## 2. Usuarios para probar

La base nueva trae un usuario por rol. Todos usan la contraseña **`1234`**.

| Usuario | Contraseña | Rol | Qué puede probar |
|---|---|---|---|
| `Maximo` | `1234` | SuperAdmin | Todo el sistema. Además es camarero: abre mesas, toma pedidos y cobra las mesas que tiene asignadas. |
| `Pedro` | `1234` | Recepcionista | Clientes (alta), reservas, apertura de mesa con reserva y Walk-in. |
| `Lucas` | `1234` | Camarero | Registrar pedidos y cobrar **sus** mesas (las que le asignan al abrirlas). |
| `Martina` | `1234` | Chef | Cocina: comandas, iniciar preparación y marcar pedidos listos. |
| `Laura` | `1234` | Gerente | Reporte de reservas, arqueo y cierre de caja, exportación de la factura en PDF. |
| `Diego` | `1234` | Supervisor | Administrativo: usuarios (consulta y desbloqueo), bitácora, consulta de roles y familias. |
| `Ana` | `1234` | Gestor de accesos | Alta, modificación y baja de roles y familias de permisos. |

> - Tres intentos fallidos en 2 horas bloquean el usuario. Lo desbloquea `Maximo` o `Diego` (Administrativo › Gestión de Usuarios). El desbloqueo resetea la contraseña a **DNI + apellido** (por ejemplo `30000001Gomez`) y pide cambiarla en el siguiente ingreso.
> - Si el instalador **actualiza** una base que ya existía, se conservan los usuarios de esa base y estos no se agregan.

**Datos útiles:**

- **Cliente de prueba:** DNI `30000000` (Cliente Frecuente). Desde Reservas y Abrir Mesa también se puede registrar un cliente nuevo.
- **Tarjeta aprobada:** `4111 1111 1111 1111`, en 1, 3, 6 o 12 cuotas.
- **Tarjeta rechazada por el banco simulado:** cualquier número válido terminado en `0002`, por ejemplo `4000 0000 0000 0002`.
- **Ayuda:**
  - F1 muestra sugerencias de la pantalla actual.
  - **Ayuda › Menú Degustación** recorre el flujo completo y lleva a cada pantalla que el rol tiene habilitada.

---

## 3. Recorrido de la Entrega 2

Para seguir el flujo como lo vive el restaurante, cada paso se hace con el usuario de su rol. El menú lateral muestra solo lo que el rol tiene permitido: cada usuario ve solo sus pantallas.

**Para cambiar de usuario:** botón **Cerrar Sesión** (abajo a la izquierda), y entrar con el siguiente.

### 3.1 Reserva (RF1) — `Pedro`

1. **Reservas › Registrar Reserva**:
   - DNI `30000000` y **Buscar**;
   - fecha, turno (18:00, 20:00 o 22:00) y comensales;
   - **Seleccionar Mesa**: en el plano se ven las mesas libres, ocupadas y la recomendada.
2. **Confirmar Reserva**.

**Qué controlar:**

- Se emite el comprobante, que se puede guardar en PDF. El envío por email requiere la configuración de la sección 7.
- No se puede reservar en el pasado, fuera de los turnos ni una mesa ya tomada en ese turno.

### 3.2 CUN-004 / CUN-005 Abrir Mesa — `Pedro`

**Atención en Mesa › Abrir Mesa.**

- **Con reserva:** aparecen las reservas de **hoy** para el turno en curso. Elegir la reserva, el camarero **Lucas Gomez** y **Confirmar ingreso**. Para probarlo hay que haber reservado antes para ese turno.
- **Walk-in (sin reserva)**, el camino más simple para probar:
  1. DNI `30000000` y **Buscar**.
  2. Cantidad de comensales.
  3. **Seleccionar mesa en el plano**.
  4. Camarero **Lucas Gomez**.
  5. **Confirmar ingreso**.

**Qué controlar:**

- La mesa pasa a **Ocupada** y queda a cargo del camarero elegido.
- No se puede abrir una mesa ocupada ni fuera de turno (ver 1.3).

### 3.3 CUN-006 Registrar Pedido — `Lucas`

1. **Atención en Mesa › Registrar Pedido**: a la izquierda aparecen **solo las mesas de Lucas**.
2. Elegir la mesa, filtrar el catálogo por categoría y sumar productos con **Agregar**. Cada producto puede llevar cantidad y observaciones.
3. **Enviar a cocina**.

**Qué controlar:**

- **Stock:** el catálogo muestra las porciones disponibles (stock menos lo ya comprometido por pedidos sin despachar). Un producto agotado no se puede pedir.
- **Tope:** los productos sin control de stock admiten hasta 99 unidades por pedido.
- **Salir con la comanda cargada:** si hay una comanda sin enviar y se cambia de pantalla, se cierra sesión o se sale, la aplicación pide confirmación.

### 3.4 CUN-008 Despachar Pedido — `Martina`

1. **Atención en Mesa › Despachar Pedido**: comandas pendientes, ordenadas por hora.
2. **Iniciar preparación** y después **Marcar como listo**.

**Qué controlar:**

- Al marcar listo se descuenta el stock.
- El camarero recibe el aviso «Pedido listo para su entrega» en Registrar Pedido y lo cierra con **Marcar como retirado**.

### 3.5 CUN-007 Cobrar Venta — `Lucas`

1. **Atención en Mesa › Caja y Cobro**: se ven **solo las mesas de Lucas**.
2. Elegir la mesa, aplicar un descuento (opcional) e **Imprimir Pre-Ticket**.
3. Elegir **Efectivo** (calcula el vuelto) o **Tarjeta** (número, cuotas y autorización del banco simulado). Opcional: propina.
4. **Registrar Cobro y Liberar Mesa** y confirmar.

**Qué controlar:**

- **Factura B con IVA contenido:** el precio de carta es el precio final.
  - **Total** = subtotal − descuento. El IVA (21 %) no se suma: se informa como «IVA contenido» en el bloque del Régimen de Transparencia Fiscal (Ley 27.743).
  - Por ejemplo, un subtotal de $ 60.900 con 10 % de descuento da un total de $ 54.810 y un IVA contenido de $ 9.512,48.
- **El comprobante:**
  - emisor, «A consumidor final» y condición de venta;
  - detalle del pago (la propina no integra el importe facturado);
  - QR y CAE simulados.
- **Liberación:** la mesa queda **Libre** y la reserva **Finalizada**.
- **Reglas que se pueden probar:**
  - No se cobra una mesa con pedidos sin despachar.
  - No se cobra una cuenta que cambió después del pre-ticket: hay que volver a emitirlo.
  - Con la tarjeta terminada en `0002`, el banco rechaza el pago y no se registra nada.
  - No se cobra con la caja del día ya cerrada.

### 3.6 Arqueo y cierre de caja — `Laura`

1. **Reportes › Arqueo y Cierre de Caja**: totales del día por medio de pago, propinas, descuentos y recaudación neta, con el detalle de cada cobro.
2. Elegir un cobro y **Exportar factura PDF**: genera la factura en A4 con el mismo contenido que el ticket.
3. **Cerrar caja del día**.

**Qué controlar:**

- No se puede cerrar la caja con mesas abiertas.
- Después del cierre ya no se pueden registrar cobros con fecha de ese día.
- Con las flechas se consultan días anteriores, y también se pueden exportar sus facturas.

### 3.7 Otros módulos

- **Reportes › Reporte de Reservas** (`Laura` o `Maximo`): filtros por fecha, cliente y estado, con exportación a PDF.
- **Reportes › Análisis Predictivo de Canasta** (`Maximo`): reglas de asociación entre productos que se piden juntos.
- **Maestros** (`Maximo`): Clientes (con exportación e importación XML), Mesas, Empleados y Productos.
  - El maestro de Mesas no deja poner fuera de servicio una mesa ocupada.
  - El maestro de Productos no pisa el stock que descontó la cocina mientras se editaba.
- **Administrativo** (`Maximo`, `Diego` o `Ana` según el caso):
  - Gestión de Usuarios, Bitácora, Gestión de Accesos (roles y familias) y Gestión de Respaldos.
  - **Bitácora de Cambios** (auditoría de productos): versiones de cada producto y rollback a una versión anterior.

---

## 4. Crear o actualizar la base de datos a mano

Solo hace falta para actualizar una base existente o para crearla en otra instancia sin abrir la aplicación.

El script único es `Deploy_TP_Ing_Soft.sql`:

- en el repositorio: `docs\Deploy_TP_Ing_Soft.sql`;
- en la instalación: `C:\Program Files\El Fogon del Sur\Database\Deploy_TP_Ing_Soft.sql`.

El script es **idempotente**:

- sobre una instancia sin la base, crea `TP_Ing_Soft` completa con los datos de prueba;
- sobre una base existente, solo agrega lo que falte (tablas, columnas, procedimientos, triggers y restricciones) y no modifica los datos.

Todo corre en una transacción: ante un error no queda nada a medias.

**Con sqlcmd** (desde la carpeta del script):

```bat
sqlcmd -S . -E -b -f 65001 -i Deploy_TP_Ing_Soft.sql
```

| Parámetro | Significado |
|---|---|
| `-S .` | Instancia. Cambiar por `.\SQLEXPRESS`, `"(localdb)\MSSQLLocalDB"` o `NOMBRE-PC\INSTANCIA` según corresponda. |
| `-E` | Autenticación de Windows. Con usuario de SQL, reemplazar por `-U usuario -P contraseña`. |
| `-b` | Corta ante el primer error. |
| `-f 65001` | Lee el archivo en UTF-8: hace falta para las tildes y la ñ. |

El script termina con la marca `DEPLOY_OK`.

**Con SQL Server Management Studio:** conectarse a la instancia con autenticación de Windows, abrir el archivo y ejecutarlo completo (F5). No hace falta el modo SQLCMD.

**Para empezar de cero**, borrar la base y volver a crearla con el script o abriendo la aplicación:

```bat
sqlcmd -S . -E -Q "ALTER DATABASE TP_Ing_Soft SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE TP_Ing_Soft"
```

---

## 5. Cambiar la cadena de conexión

La aplicación lee la conexión `TP_Ing_Soft` de su archivo de configuración:

- **Aplicación instalada:** `C:\Program Files\El Fogon del Sur\GUI_08YS.exe.config`. Editarlo como administrador y reiniciar la aplicación.
- **Código fuente:** `GUI\Config\App.config`. Al compilar se copia como `GUI_08YS.exe.config`.

```xml
<connectionStrings>
  <add name="TP_Ing_Soft"
       connectionString="Data Source=.;Initial Catalog=TP_Ing_Soft;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Solo hay que cambiar el **`Data Source`**:

| Caso | `Data Source` |
|---|---|
| Instancia predeterminada de la PC (valor por defecto) | `.` |
| SQL Server Express | `.\SQLEXPRESS` |
| LocalDB de Visual Studio | `(localdb)\MSSQLLocalDB` |
| Instancia con nombre o en otra PC | `NOMBRE-PC\INSTANCIA` |

Con autenticación de SQL Server en lugar de Windows, reemplazar `Integrated Security=True` por `User ID=usuario;Password=contraseña`.

`Initial Catalog` tiene que seguir siendo `TP_Ing_Soft`. Si se corre el script a mano, hay que correrlo contra la misma instancia.

---

## 6. Generar el instalador

1. Compilar la solución en **Release**.
2. Desde la raíz del repositorio, con Inno Setup 6:

   ```bat
   ISCC.exe Installer_TP_Fogon.iss
   ```

El setup queda en `Installer\Output\Setup_ElFogonDelSur.exe`. Incluye el script de base de datos, los manuales PDF y este README (también como `LEAME.txt`).

---

## 7. Envío del comprobante de reserva por email (opcional)

El sistema funciona sin configurar el correo: la reserva se registra, el PDF se puede guardar y solo se avisa que el envío no está configurado.

Para habilitar el envío:

1. Copiar `App.secrets.config.template` como `App.secrets.config`, en la carpeta de instalación o en `GUI\Config`.
2. Completar el remitente y una contraseña de aplicación de Gmail.

Ese archivo nunca se sube al repositorio.

---

## 8. Problemas frecuentes

| Síntoma | Qué hacer |
|---|---|
| Errores «no se pudo procesar el archivo ….resx porque se encuentra en Internet o en la zona restringida» | El zip se descomprimió sin desbloquear. Ver el paso 0 de la sección 1.2. |
| Advertencia «No se pudo agregar el archivo 'Config\App.secrets.config' al proyecto» | Es normal: ese archivo tiene credenciales SMTP y no se distribuye. No impide compilar. Solo hace falta para enviar el comprobante por email (sección 7). |
| Errores de compilación con `FontAwesome.Sharp`, `IconChar`, `iTextSharp` o `Newtonsoft.Json` | Faltan los paquetes NuGet. Recargarlos como indica el paso 2 de la sección 1.2. Hace falta conexión a internet. |
| «No se pudo preparar la base de datos» al iniciar | La instancia del `Data Source` no existe o no responde. Revisar la sección 5 y que el servicio de SQL Server esté iniciado. |
| No se conecta a `(localdb)\MSSQLLocalDB` | Crear e iniciar la instancia con `sqllocaldb create MSSQLLocalDB -s`. Si no existe el comando, agregar «SQL Server Express LocalDB» desde el Visual Studio Installer. |
| El instalador falla al crear la base | Revisar `C:\ProgramData\El Fogon del Sur\Logs\Deploy_TP_Ing_Soft.log` y que `sqlcmd` esté en el PATH. |
| No existe el usuario `Lucas` (u otro de la sección 2) | La base es de una versión anterior. Actualizarla o recrearla (sección 4). |
| No se puede abrir mesa: «fuera de turno» | Estamos fuera de los turnos de atención. Activar el modo demostración (1.3). |
| La reserva de hoy no aparece en Abrir Mesa | Solo aparecen las reservas del turno en curso. Usar Walk-in o reservar para ese turno. |
| El usuario quedó bloqueado | Desbloquearlo con `Maximo` o `Diego` (ver la nota de la sección 2). |
| Aviso de inconsistencia de dígitos verificadores | Alguien modificó datos por fuera de la aplicación. Ingresar con `Maximo`: la pantalla de inconsistencia permite recalcular los DV o restaurar un backup. |
