# El Fogón del Sur — Sistema de gestión del restaurante

Aplicación de escritorio (C# WinForms, .NET Framework 4.8) con base de datos SQL Server. Cubre reservas, atención en mesa (apertura, pedidos y cocina), facturación y cobro (Factura B con IVA contenido), arqueo y cierre de caja, maestros, reportes y seguridad (usuarios, roles, familias, bitácora y dígitos verificadores).

Trabajo académico: los datos fiscales (CUIT, CAE, QR) son ficticios y los comprobantes llevan la leyenda «Maqueta académica · sin validez fiscal».

---

## 1. Usuarios para probar

Una **instalación nueva** crea un usuario por rol. Todos usan la contraseña **`1234`**.

| Usuario | Contraseña | Rol | Qué puede probar |
|---|---|---|---|
| `Maximo` | `1234` | SuperAdmin | Todo el sistema. Además es camarero: abre mesas, toma pedidos y cobra las mesas que tiene asignadas. |
| `Pedro` | `1234` | Recepcionista | Clientes (alta), reservas, apertura de mesa con reserva y Walk-in. |
| `Lucas` | `1234` | Camarero | Registrar pedidos y cobrar **sus** mesas (las que le asignan al abrirlas). |
| `Martina` | `1234` | Chef | Cocina: comandas, iniciar preparación y marcar pedidos listos. |
| `Laura` | `1234` | Gerente | Reportes: reporte de reservas, arqueo y cierre de caja, exportación de la factura en PDF. |
| `Diego` | `1234` | Supervisor | Administrativo: usuarios (consulta y desbloqueo), bitácora, consulta de roles y familias. |
| `Ana` | `1234` | Gestor de accesos | Alta, modificación y baja de roles y familias de permisos. |

> - Tres intentos fallidos en 2 horas bloquean el usuario. Lo desbloquea `Maximo` o `Diego` (Administrativo › Usuarios). El desbloqueo resetea la contraseña a **DNI + apellido** (por ejemplo `30000001Gomez`) y pide cambiarla en el siguiente ingreso.
> - Si la instalación **actualiza** una base que ya existía, los usuarios de esa base se conservan y estos usuarios no se agregan.

### Recorrido sugerido

1. **Pedro:** Atención en Mesa › Abrir Mesa › Walk-in. DNI del cliente `30000000`, 2 comensales, una mesa del plano y el camarero **Lucas Gomez**.
2. **Lucas:** Atención en Mesa › Registrar Pedido. Cargar la comanda y «Enviar a cocina».
3. **Martina:** Atención en Mesa › Cocina. Iniciar preparación y marcar «Listo».
4. **Lucas:** Atención en Mesa › Caja y cobro. Imprimir el pre-ticket y cobrar en efectivo o con tarjeta.
5. **Laura:** Reportes › Arqueo de caja. Ver el cobro, exportar la factura en PDF y cerrar la caja del día.

**Datos útiles:**

- **Cliente de prueba:** DNI `30000000` (Cliente Frecuente).
- **Tarjeta aprobada:** `4111 1111 1111 1111`, con cualquier cantidad de cuotas permitida (1, 3, 6 o 12).
- **Tarjeta rechazada por el banco simulado:** cualquier número válido terminado en `0002`, por ejemplo `4000 0000 0000 0002`.
- **Turnos de atención:** 18:00, 20:00 y 22:00, de 2 horas cada uno. Fuera de ese horario solo se puede abrir mesas en **modo demostración** (ver 2.3).
- **Ayuda:** F1 muestra sugerencias de la pantalla actual. Ayuda › Menú Degustación recorre el flujo completo según el rol.

---

## 2. Instalación con el instalador (recomendado)

### 2.1 Requisitos

- Windows 10 u 11 de 64 bits.
- .NET Framework 4.8.
- SQL Server 2016 SP1 o posterior: Express, Developer o LocalDB.
- `sqlcmd` en el PATH. Se instala con SQL Server o con las *Microsoft Command Line Utilities for SQL Server*.

### 2.2 Pasos

1. Ejecutar `Installer\Output\Setup_ElFogonDelSur.exe` como administrador.
2. Elegir la carpeta de destino (por defecto `C:\Program Files\El Fogon del Sur`).
3. Elegir la **instancia de SQL Server**. El desplegable lista las detectadas:
   - `.` para la instancia predeterminada;
   - `.\SQLEXPRESS` u otra instancia con nombre;
   - `(localdb)\MSSQLLocalDB`.

   El botón «Probar conexión» comprueba que la instancia responda.
4. Opcional: marcar **«Instalar en modo demostración»** (ver 2.3).
5. Terminar. El instalador:
   - crea la base `TP_Ing_Soft` completa (esquema, datos iniciales, usuarios de la sección 1 y dígitos verificadores);
   - escribe la instancia elegida en la cadena de conexión;
   - crea los accesos directos.

Si la aplicación ya estaba instalada, el mismo setup la **actualiza**: hace un backup de la base, aplica solo los cambios de esquema y no toca los datos.

**Dónde quedan backups y logs:**

- Backups: `C:\ProgramData\El Fogon del Sur\Backups`.
- Logs de `sqlcmd`: `C:\ProgramData\El Fogon del Sur\Logs`.

Si algo falla, el log `Deploy_TP_Ing_Soft.log` indica el motivo.

### 2.3 Modo demostración

Con el modo demostración activo:

- fuera del horario de atención, la aplicación usa el turno de prueba de las **22:00**, así que se puede abrir mesas a cualquier hora;
- se habilita la Ficha de Ingredientes de la Ayuda;
- el cobro con tarjeta simula una demora.

Para activarlo o desactivarlo después de instalar, editar `GUI_08YS.exe.config` en la carpeta de instalación:

```xml
<add key="ModoDemostracion" value="true" />
<add key="TurnoPrueba_790MY" value="22:00" />
```

---

## 3. Crear o actualizar la base de datos a mano

El script único es `Deploy_TP_Ing_Soft.sql`:

- en la instalación: `C:\Program Files\El Fogon del Sur\Database\Deploy_TP_Ing_Soft.sql`;
- en el repositorio: `docs\Deploy_TP_Ing_Soft.sql`.

El script es **idempotente**:

- sobre una instancia sin la base, crea `TP_Ing_Soft` completa con sus datos iniciales;
- sobre una base existente, solo agrega lo que falte y no modifica los datos.

Todo corre en una transacción: ante un error no queda nada a medias.

**Con sqlcmd** (desde la carpeta del script):

```bat
sqlcmd -S . -E -b -f 65001 -i Deploy_TP_Ing_Soft.sql
```

| Parámetro | Significado |
|---|---|
| `-S .` | Instancia. Cambiar por `.\SQLEXPRESS`, `(localdb)\MSSQLLocalDB` o `NOMBRE-PC\INSTANCIA` según corresponda. |
| `-E` | Autenticación de Windows. Con usuario de SQL, reemplazar por `-U usuario -P contraseña`. |
| `-b` | Corta ante el primer error. |
| `-f 65001` | Lee el archivo en UTF-8: hace falta para las tildes y la ñ. |

El script termina con la marca `DEPLOY_OK`.

**Con SQL Server Management Studio:** abrir el archivo y ejecutarlo completo (F5) en la instancia elegida. No hace falta el modo SQLCMD.

> Al ejecutar la aplicación desde Visual Studio, si la base `TP_Ing_Soft` no existe en la instancia configurada, la aplicación la crea sola al arrancar a partir de `script.sql`, que es una copia del Deploy.

---

## 4. Cambiar la cadena de conexión

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
| Instancia predeterminada de la PC | `.` |
| SQL Server Express | `.\SQLEXPRESS` |
| LocalDB | `(localdb)\MSSQLLocalDB` |
| Instancia con nombre o en otra PC | `NOMBRE-PC\INSTANCIA` |

Con autenticación de SQL Server en lugar de Windows, reemplazar `Integrated Security=True` por `User ID=usuario;Password=contraseña`.

`Initial Catalog` tiene que seguir siendo `TP_Ing_Soft`.

---

## 5. Compilar desde el código fuente

1. Abrir `TrabajoDiploma.slnx` con Visual Studio 2022 (17.13 o posterior) o Visual Studio 2026.
2. Restaurar los paquetes NuGet (se restauran solos al compilar).
3. Definir `GUI_08YS` como proyecto de inicio y ajustar la cadena de conexión (sección 4).
4. Ejecutar (F5). Si la base no existe, se crea al arrancar.

**Generar el instalador:**

1. Compilar la solución en **Release**.
2. Desde la raíz del repositorio, con Inno Setup 6:

   ```bat
   ISCC.exe Installer_TP_Fogon.iss
   ```

El setup queda en `Installer\Output\Setup_ElFogonDelSur.exe`.

---

## 6. Envío del comprobante de reserva por email (opcional)

El sistema funciona sin configurar el correo: la reserva se registra y el PDF se puede guardar, y solo se avisa que el envío no está configurado.

Para habilitar el envío:

1. Copiar `App.secrets.config.template` como `App.secrets.config`, en la carpeta de instalación o en `GUI\Config`.
2. Completar el remitente y una contraseña de aplicación de Gmail.

Ese archivo nunca se sube al repositorio.

---

## 7. Problemas frecuentes

| Síntoma | Qué hacer |
|---|---|
| «No se pudo preparar la base de datos» al iniciar | La instancia del `Data Source` no existe o no responde. Revisar la sección 4 y que el servicio de SQL Server esté iniciado. |
| Aviso de inconsistencia de dígitos verificadores | Alguien modificó datos por fuera de la aplicación. Ingresar con `Maximo`: la pantalla de inconsistencia permite recalcular los DV o restaurar un backup. |
| No se puede abrir mesa: «fuera de turno» | Estamos fuera de los turnos de atención. Activar el modo demostración (2.3). |
| El usuario quedó bloqueado | Desbloquearlo con `Maximo` o `Diego` (ver la nota de la sección 1). |
| El instalador falla al crear la base | Revisar `C:\ProgramData\El Fogon del Sur\Logs\Deploy_TP_Ing_Soft.log` y que `sqlcmd` esté en el PATH. |
