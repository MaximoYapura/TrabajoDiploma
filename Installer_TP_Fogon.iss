; =============================================================================
;  Installer_TP_Fogon.iss — Instalador / reinstalador de "El Fogón del Sur"
; -----------------------------------------------------------------------------
;  Compilar con Inno Setup 6 (ISCC.exe) desde la raíz del repositorio, después
;  de compilar la solución en Release:
;      ISCC.exe Installer_TP_Fogon.iss
;  (instalado con winget queda en %LOCALAPPDATA%\Programs\Inno Setup 6\ISCC.exe)
;  El setup queda en Installer\Output\Setup_ElFogon.exe
;
;  MODOS
;   * Instalación limpia (no existe HKLM\Software\ElFogonDelSur):
;       - el usuario elige la carpeta de destino y la instancia de SQL Server
;         (desplegable con las instancias detectadas en el Registro);
;       - se copian los binarios de GUI\bin\Release y el PDF de docs\;
;       - se ejecuta docs\Deploy_TP_Ing_Soft.sql con sqlcmd sobre esa instancia:
;         crea TP_Ing_Soft completa (esquema + datos iniciales + DV) si no existe;
;       - se crea App.secrets.config a partir de la plantilla (si no existe);
;       - se crean los accesos directos (Escritorio y Menú Inicio).
;   * Reinstalación / actualización (la clave de registro ya existe):
;       - se reutilizan la carpeta y la instancia SQL de la instalación anterior
;         (valor SqlInstance del registro);
;       - se reemplazan ejecutables, DLLs y recursos;
;       - ANTES de tocar la base se hace BACKUP DATABASE TP_Ing_Soft a
;         {app}\Database\Backups\TP_Ing_Soft_<fecha>_<hora>.bak (el servicio de
;         SQL Server escribe el archivo: la carpeta da permiso de modificación a
;         las cuentas de servicio; solo sirve con una instancia local);
;       - si el backup salió bien, se ejecuta el MISMO Deploy_TP_Ing_Soft.sql: es
;         idempotente, agrega solo lo que falte del esquema (tablas, columnas,
;         procedimientos, triggers) y no modifica datos ni dígitos verificadores;
;       - si el backup o el script fallan, se avisa con la ruta del log y no se
;         borra nada (sin backup el script NO se ejecuta);
;       - App.secrets.config (credenciales SMTP) no se toca.
;   * Desinstalación: borra toda la carpeta de instalación (incluidos
;     App.secrets.config y Database\, con sus Backups\); la base TP_Ing_Soft
;     queda intacta.
;
;  MODO DEMOSTRACIÓN (A8)
;   * Tarea opcional "Instalar en modo demostración", DESMARCADA por defecto.
;   * Al terminar de copiar (también en reinstalación, porque el .exe.config se
;     reemplaza) se escribe appSettings/ModoDemostracion en GUI_08YS.exe.config:
;     "false" (producción) salvo que se marque la casilla ("true"). En una
;     reinstalación el asistente propone la elección de la instalación anterior.
;   * GUI\Config\App.config del repositorio conserva ModoDemostracion=true
;     (entorno de desarrollo / defensa): el valor de producción lo fija el instalador.
;
;  Requisitos en la PC destino: .NET Framework 4.8, SQL Server 2016 SP1 o
;  posterior (o LocalDB) y sqlcmd en el PATH. La base TP_Ing_Soft la crea o la
;  actualiza el propio instalador.
; =============================================================================

#define MyAppName        "El Fogón del Sur"
; 1.1.0: Etapa 5 (Ayuda del Chef y Manual de Recetas). Sin cambios en la base de datos.
#define MyAppVersion     "1.1.0"
#define MyAppPublisher   "Maximo Yapura"
#define MyAppExeName     "GUI_08YS.exe"
#define ReleaseDir       "GUI\bin\Release"
#define RegKey           "Software\ElFogonDelSur"
; "." = instancia predeterminada (MSSQLSERVER) del equipo local.
#define DefaultSqlInst   "."

[Setup]
AppId={{6F3B2A1C-8E4D-4F7A-9C21-5B0D7E9A4F18}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\El Fogon del Sur
DefaultGroupName={#MyAppName}
; La carpeta se elige en la instalación limpia; en una reinstalación se reutiliza la anterior.
DisableDirPage=auto
UsePreviousAppDir=yes
DisableProgramGroupPage=yes
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir=Installer\Output
OutputBaseFilename=Setup_ElFogon
SetupIconFile=GUI\Resources\FogonIcon.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
CloseApplications=yes
; Al terminar avisa al Explorador que refresque los íconos: evita que la barra de
; tareas y los accesos directos sigan mostrando un ícono viejo guardado en caché.
ChangesAssociations=yes

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
; Modo demostración (A8): desmarcado por defecto = producción. En una reinstalación
; Inno Setup vuelve a proponer la elección anterior (UsePreviousTasks).
Name: "mododemo"; Description: "Instalar en modo demostración (turno de prueba, Ficha de Ingredientes y demora simulada del cobro)"; \
    GroupDescription: "Opciones de la aplicación:"; Flags: unchecked

[Files]
; Ejecutable, DLLs, idiomas.json, GUI_08YS.exe.config y Resources\.
; Se excluyen símbolos/documentación XML, el script de esquema viejo y, sobre todo,
; App.secrets.config: contiene credenciales SMTP reales de la PC de desarrollo.
Source: "{#ReleaseDir}\*"; DestDir: "{app}"; \
    Excludes: "*.pdb,*.xml,App.secrets.config,script.sql,*.vshost.*,Manual\*"; \
    Flags: ignoreversion recursesubdirs createallsubdirs

; Plantilla SMTP y su copia como App.secrets.config (solo si todavía no existe:
; una reinstalación nunca pisa credenciales ya cargadas). Se borra al desinstalar
; (ver [UninstallDelete]).
Source: "GUI\Config\App.secrets.config.template"; DestDir: "{app}"; Flags: ignoreversion
Source: "GUI\Config\App.secrets.config.template"; DestDir: "{app}"; DestName: "App.secrets.config"; \
    Flags: onlyifdoesntexist

; Script único de base de datos (esquema + datos iniciales + DV), idempotente. Se
; instala siempre, para poder re-ejecutarlo a mano; el instalador lo corre en la
; instalación limpia y en la reinstalación (ver [Run]).
Source: "docs\Deploy_TP_Ing_Soft.sql"; DestDir: "{app}\Database"; Flags: ignoreversion

; Documentación
Source: "docs\*.pdf"; DestDir: "{app}\docs"; Flags: ignoreversion

; Manual de Recetas (Etapa 5, Fase 8a): un PDF por receta; la Ayuda del Chef los abre
; desde {app}\Manual. Arriba se excluyen de la copia de bin para no duplicarlos.
Source: "docs\Manual\*.pdf"; DestDir: "{app}\Manual"; Flags: ignoreversion

[Dirs]
; Destino del BACKUP previo a una actualización. Lo escribe el servicio de SQL Server
; (cuenta NT Service\MSSQL...), que por defecto no puede escribir en Program Files:
; se le da permiso de modificación al grupo de servicios (SID S-1-5-6) solo en esta carpeta.
Name: "{app}\Database\Backups"; Permissions: service-modify

[Icons]
Name: "{autoprograms}\{#MyAppName}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{autoprograms}\{#MyAppName}\Documentación"; Filename: "{app}\docs"
Name: "{autoprograms}\{#MyAppName}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Registry]
; Marca de instalación: su existencia activa el modo Reinstalación / Actualización.
Root: HKLM; Subkey: "{#RegKey}"; Flags: uninsdeletekey
Root: HKLM; Subkey: "{#RegKey}"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"
Root: HKLM; Subkey: "{#RegKey}"; ValueType: string; ValueName: "Version"; ValueData: "{#MyAppVersion}"
Root: HKLM; Subkey: "{#RegKey}"; ValueType: string; ValueName: "SqlInstance"; ValueData: "{code:GetSqlInstance}"

[Run]
; Las entradas se ejecutan en este orden; el Check de cada una se evalúa al llegar a ella.
; 1) Solo en actualización: BACKUP DATABASE previo. VerificarBackup revisa la salida
;    (BACKUP_OK / BACKUP_SIN_BASE) y deja el resultado en BackupOk.
Filename: "{cmd}"; \
    Parameters: "/C sqlcmd -S ""{code:GetSqlInstance}"" -E -b -l 30 -Q ""{code:ConsultaBackup}"" > ""{app}\Database\Backup_TP_Ing_Soft.log"" 2>&1"; \
    StatusMsg: "Haciendo una copia de seguridad de la base TP_Ing_Soft..."; \
    Flags: runhidden waituntilterminated; Check: DebeRespaldarBase; AfterInstall: VerificarBackup
; 2) Base de datos: instalación limpia o actualización con backup correcto (Check).
;    -b corta ante el primer error y -f 65001 lee el script en UTF-8. La salida queda en
;    Database\Deploy_TP_Ing_Soft.log y VerificarDeploy la revisa (marca DEPLOY_OK).
Filename: "{cmd}"; \
    Parameters: "/C sqlcmd -S ""{code:GetSqlInstance}"" -E -b -l 30 -f 65001 -i ""{app}\Database\Deploy_TP_Ing_Soft.sql"" > ""{app}\Database\Deploy_TP_Ing_Soft.log"" 2>&1"; \
    StatusMsg: "Creando o actualizando la base de datos TP_Ing_Soft..."; \
    Flags: runhidden waituntilterminated; Check: DebeDesplegarBase; AfterInstall: VerificarDeploy

Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; \
    WorkingDir: "{app}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Limpieza total: también lo que se crea en tiempo de ejecución o después de
; instalar (log del despliegue, App.secrets.config con credenciales SMTP, etc.).
; La base de datos TP_Ing_Soft en SQL Server NO se toca.
Type: filesandordirs; Name: "{app}\Database"
Type: files; Name: "{app}\App.secrets.config"
Type: filesandordirs; Name: "{app}\*"

[Code]
const
  RegKey         = '{#RegKey}';
  DefaultSqlInst = '{#DefaultSqlInst}';
  LocalDbInst    = '(localdb)\MSSQLLocalDB';
  SqlInstancesKey = 'SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL';
  NetFx48Release = 528040;

var
  IsUpgrade: Boolean;
  PreviousSqlInstance: String;
  SqlPage: TWizardPage;
  SqlCombo: TNewComboBox;
  SqlCmdEncontrado: Boolean;
  BackupOk: Boolean;
  ArchivoBackup: String;

{ ---------------------------------------------------------------------------
  Utilidades
  --------------------------------------------------------------------------- }

function GetSqlInstance(Param: String): String;
begin
  if IsUpgrade then
    Result := PreviousSqlInstance
  else
    Result := Trim(SqlCombo.Text);
end;

{ Ejecuta un comando de consola con salida redirigida a LogFile. Devuelve el exit code. }
function RunCmd(const CommandLine, LogFile: String): Integer;
var
  ResultCode: Integer;
begin
  if not Exec(ExpandConstant('{cmd}'), '/C ' + CommandLine + ' > "' + LogFile + '" 2>&1',
              '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
    ResultCode := -1;
  Result := ResultCode;
end;

function ReadLog(const LogFile: String): String;
var
  Contenido: AnsiString;
begin
  if LoadStringFromFile(LogFile, Contenido) then
    Result := Trim(String(Contenido))
  else
    Result := '(sin salida)';
end;

function SqlCmdDisponible: Boolean;
begin
  Result := RunCmd('where sqlcmd', ExpandConstant('{tmp}\where_sqlcmd.log')) = 0;
end;

{ Agrega una instancia al desplegable si todavía no está. }
procedure AgregarInstancia(const Instancia: String);
begin
  if SqlCombo.Items.IndexOf(Instancia) < 0 then
    SqlCombo.Items.Add(Instancia);
end;

{ Lee los nombres de instancia de una vista del Registro (64 o 32 bits).
  MSSQLSERVER (instancia predeterminada) se informa aparte, porque va primero como ".". }
procedure LeerInstancias(const RootKey: Integer; var Nombradas: TArrayOfString; var TienePredeterminada: Boolean);
var
  Nombres: TArrayOfString;
  I, N: Integer;
begin
  if not RegGetValueNames(RootKey, SqlInstancesKey, Nombres) then
    Exit;
  for I := 0 to GetArrayLength(Nombres) - 1 do
  begin
    if CompareText(Nombres[I], 'MSSQLSERVER') = 0 then
      TienePredeterminada := True
    else
    begin
      N := GetArrayLength(Nombradas);
      SetArrayLength(Nombradas, N + 1);
      Nombradas[N] := '.\' + Nombres[I];
    end;
  end;
end;

{ Llena el desplegable con las instancias instaladas:
    "." si existe MSSQLSERVER, luego ".\NOMBRE" por cada instancia nombrada
    y siempre al final (localdb)\MSSQLLocalDB. Preselecciona el primer ítem. }
procedure DetectarInstanciasSql;
var
  Nombradas: TArrayOfString;
  TienePredeterminada: Boolean;
  I: Integer;
begin
  TienePredeterminada := False;
  SetArrayLength(Nombradas, 0);
  if IsWin64 then
    LeerInstancias(HKLM64, Nombradas, TienePredeterminada);
  LeerInstancias(HKLM32, Nombradas, TienePredeterminada);

  SqlCombo.Items.Clear;
  if TienePredeterminada then
    AgregarInstancia('.');
  for I := 0 to GetArrayLength(Nombradas) - 1 do
    AgregarInstancia(Nombradas[I]);
  AgregarInstancia(LocalDbInst);
  SqlCombo.ItemIndex := 0;
end;

{ Escribe la instancia elegida como "Data Source" de la cadena de conexión TP_Ing_Soft
  en GUI_08YS.exe.config. Se hace con PowerShell + XmlDocument para respetar la
  codificación UTF-8 del archivo y el resto de la cadena (Initial Catalog, etc.). }
procedure ConfigurarConnectionString(const Instancia: String);
var
  ConfigFile, Script, LogFile: String;
begin
  ConfigFile := ExpandConstant('{app}\{#MyAppExeName}.config');
  Script := ExpandConstant('{tmp}\ConfigurarConexion.ps1');
  LogFile := ExpandConstant('{tmp}\ConfigurarConexion.log');

  SaveStringToFile(Script,
    'param([string]$Archivo, [string]$Instancia)' + #13#10 +
    '$ErrorActionPreference = ''Stop''' + #13#10 +
    '$xml = New-Object System.Xml.XmlDocument' + #13#10 +
    '$xml.PreserveWhitespace = $true' + #13#10 +
    '$xml.Load($Archivo)' + #13#10 +
    '$nodo = $xml.SelectSingleNode("/configuration/connectionStrings/add[@name=''TP_Ing_Soft'']")' + #13#10 +
    'if ($nodo -eq $null) { throw "No existe la cadena de conexion TP_Ing_Soft en $Archivo" }' + #13#10 +
    '$cs = New-Object System.Data.SqlClient.SqlConnectionStringBuilder $nodo.GetAttribute(''connectionString'')' + #13#10 +
    '$cs[''Data Source''] = $Instancia' + #13#10 +
    '$nodo.SetAttribute(''connectionString'', $cs.ConnectionString)' + #13#10 +
    '$xml.Save($Archivo)' + #13#10,
    False);

  if RunCmd('powershell.exe -NoProfile -ExecutionPolicy Bypass -File "' + Script + '" -Archivo "' +
            ConfigFile + '" -Instancia "' + Instancia + '"', LogFile) <> 0 then
    MsgBox('No se pudo configurar la instancia de SQL Server en ' + ConfigFile + ':' + #13#10#13#10 +
           ReadLog(LogFile) + #13#10#13#10 +
           'Edite a mano la cadena de conexión "TP_Ing_Soft" de ese archivo.', mbError, MB_OK);
end;

{ Escribe appSettings/ModoDemostracion en GUI_08YS.exe.config ("true" solo si se marcó
  la tarea mododemo; si no, "false" = producción). Mismo mecanismo que
  ConfigurarConnectionString (PowerShell + XmlDocument); si la clave no existe, se agrega. }
procedure ConfigurarModoDemostracion(const Activo: Boolean);
var
  ConfigFile, Script, LogFile, Valor: String;
begin
  ConfigFile := ExpandConstant('{app}\{#MyAppExeName}.config');
  Script := ExpandConstant('{tmp}\ConfigurarModoDemostracion.ps1');
  LogFile := ExpandConstant('{tmp}\ConfigurarModoDemostracion.log');
  if Activo then
    Valor := 'true'
  else
    Valor := 'false';

  SaveStringToFile(Script,
    'param([string]$Archivo, [string]$Valor)' + #13#10 +
    '$ErrorActionPreference = ''Stop''' + #13#10 +
    '$xml = New-Object System.Xml.XmlDocument' + #13#10 +
    '$xml.PreserveWhitespace = $true' + #13#10 +
    '$xml.Load($Archivo)' + #13#10 +
    '$settings = $xml.SelectSingleNode("/configuration/appSettings")' + #13#10 +
    'if ($settings -eq $null) { throw "No existe la seccion appSettings en $Archivo" }' + #13#10 +
    '$nodo = $settings.SelectSingleNode("add[@key=''ModoDemostracion'']")' + #13#10 +
    'if ($nodo -eq $null) {' + #13#10 +
    '  $nodo = $xml.CreateElement("add")' + #13#10 +
    '  $nodo.SetAttribute(''key'', ''ModoDemostracion'')' + #13#10 +
    '  [void]$settings.AppendChild($nodo)' + #13#10 +
    '}' + #13#10 +
    '$nodo.SetAttribute(''value'', $Valor)' + #13#10 +
    '$xml.Save($Archivo)' + #13#10,
    False);

  if RunCmd('powershell.exe -NoProfile -ExecutionPolicy Bypass -File "' + Script + '" -Archivo "' +
            ConfigFile + '" -Valor "' + Valor + '"', LogFile) <> 0 then
    MsgBox('No se pudo configurar el modo demostración en ' + ConfigFile + ':' + #13#10#13#10 +
           ReadLog(LogFile) + #13#10#13#10 +
           'Edite a mano la clave "ModoDemostracion" (true/false) de ese archivo.', mbError, MB_OK);
end;

{ ---------------------------------------------------------------------------
  Despliegue de la base (entrada de la sección Run)
  --------------------------------------------------------------------------- }

{ Ruta del .bak de esta actualización (se calcula una sola vez: la usan la consulta
  y los mensajes). }
function GetArchivoBackup: String;
begin
  if ArchivoBackup = '' then
    ArchivoBackup := ExpandConstant('{app}\Database\Backups\TP_Ing_Soft_') +
                     GetDateTimeString('yyyymmdd_hhnnss', #0, #0) + '.bak';
  Result := ArchivoBackup;
end;

{ Parámetro -Q de la entrada de backup de [Run]. Si la base no existe (p. ej. se
  borró a mano) no hay nada que respaldar: el Deploy la vuelve a crear.
  TRY/CATCH + THROW: un BACKUP fallido no corta el lote, y sin esto se imprimiría
  BACKUP_OK aunque el archivo no se haya escrito (probado). }
function ConsultaBackup(Param: String): String;
begin
  Result := 'IF DB_ID(N''TP_Ing_Soft'') IS NULL PRINT N''BACKUP_SIN_BASE'' ELSE BEGIN TRY ' +
            'BACKUP DATABASE [TP_Ing_Soft] TO DISK = N''' + GetArchivoBackup + ''' ' +
            'WITH INIT, CHECKSUM, NAME = N''TP_Ing_Soft antes de actualizar a {#MyAppVersion}''; ' +
            'PRINT N''BACKUP_OK''; END TRY BEGIN CATCH THROW; END CATCH';
end;

{ Check de la entrada de backup de [Run]: solo en una actualización y si hay sqlcmd. }
function DebeRespaldarBase: Boolean;
begin
  Result := IsUpgrade and SqlCmdEncontrado;
end;

{ AfterInstall de la entrada de backup: sin un backup correcto la base no se toca. }
procedure VerificarBackup;
var
  LogFile, Salida: String;
begin
  LogFile := ExpandConstant('{app}\Database\Backup_TP_Ing_Soft.log');
  Salida := ReadLog(LogFile);
  BackupOk := (Pos('BACKUP_OK', Salida) > 0) or (Pos('BACKUP_SIN_BASE', Salida) > 0);
  if BackupOk then
    Exit;

  MsgBox('No se pudo hacer la copia de seguridad de la base TP_Ing_Soft:' + #13#10#13#10 + Salida + #13#10#13#10 +
         'Por seguridad la base de datos NO se actualizó y no se borró nada.' + #13#10 +
         'Detalle en: ' + LogFile + #13#10#13#10 +
         'La aplicación quedó instalada. Haga un backup de TP_Ing_Soft y actualice la base a mano con:' + #13#10 +
         'sqlcmd -S "' + GetSqlInstance('') + '" -E -b -f 65001 -i "' +
         ExpandConstant('{app}\Database\Deploy_TP_Ing_Soft.sql') + '"', mbError, MB_OK);
end;

{ Check de la entrada de Deploy de [Run]: instalación limpia, o actualización con
  backup correcto; siempre que haya sqlcmd. }
function DebeDesplegarBase: Boolean;
begin
  Result := SqlCmdEncontrado and ((not IsUpgrade) or BackupOk);
end;

{ AfterInstall de la entrada de Deploy de [Run]: revisa la salida de sqlcmd.
  DEPLOY_OMITIDO lo emitían versiones anteriores del script; se sigue aceptando. }
procedure VerificarDeploy;
var
  LogFile, ScriptFile, Salida, Respaldo: String;
begin
  LogFile := ExpandConstant('{app}\Database\Deploy_TP_Ing_Soft.log');
  ScriptFile := ExpandConstant('{app}\Database\Deploy_TP_Ing_Soft.sql');
  Salida := ReadLog(LogFile);

  if (Pos('DEPLOY_OK', Salida) > 0) or (Pos('DEPLOY_OMITIDO', Salida) > 0) then
    Exit;

  { El script corre en una transacción: ante un error no queda nada a medio hacer. }
  Respaldo := '';
  if IsUpgrade and FileExists(GetArchivoBackup) then
    Respaldo := 'Copia de seguridad previa: ' + GetArchivoBackup + #13#10;

  MsgBox('No se pudo crear o actualizar la base de datos TP_Ing_Soft:' + #13#10#13#10 + Salida + #13#10#13#10 +
         'Los cambios del script se revirtieron y no se borró nada.' + #13#10 +
         'Detalle en: ' + LogFile + #13#10 + Respaldo + #13#10 +
         'La aplicación quedó instalada. Puede re-ejecutar el script a mano:' + #13#10 +
         'sqlcmd -S "' + GetSqlInstance('') + '" -E -b -f 65001 -i "' + ScriptFile + '"', mbError, MB_OK);
end;

{ ---------------------------------------------------------------------------
  Asistente
  --------------------------------------------------------------------------- }

function InitializeSetup: Boolean;
var
  NetRelease: Cardinal;
begin
  Result := True;

  if not (RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', NetRelease)
          and (NetRelease >= NetFx48Release)) then
  begin
    if MsgBox('No se detectó .NET Framework 4.8, necesario para ejecutar ' + '{#MyAppName}' + '.' + #13#10 +
              '¿Desea continuar con la instalación de todos modos?', mbConfirmation, MB_YESNO) = IDNO then
    begin
      Result := False;
      Exit;
    end;
  end;

  IsUpgrade := RegValueExists(HKLM, RegKey, 'InstallPath');
  if IsUpgrade then
  begin
    if not RegQueryStringValue(HKLM, RegKey, 'SqlInstance', PreviousSqlInstance)
       or (Trim(PreviousSqlInstance) = '') then
      PreviousSqlInstance := DefaultSqlInst;
  end;

  SqlCmdEncontrado := SqlCmdDisponible;
end;

procedure InitializeWizard;
var
  Descripcion, Etiqueta, Nota: TNewStaticText;
begin
  { Reinstalación: se avisa antes de copiar nada qué se va a hacer con la base. }
  if IsUpgrade then
    CreateOutputMsgPage(wpWelcome,
      'Reinstalación detectada',
      'Ya existe una instalación de ' + '{#MyAppName}' + ' en este equipo',
      'Se detectó una versión previa instalada. Se actualizarán los ejecutables conservando los datos existentes: ' +
      'se hará una copia de seguridad de la base y luego se actualizará su esquema (solo se agrega lo que falte).' + #13#10#13#10 +
      'Carpeta de instalación: ' + WizardForm.PrevAppDir + #13#10 +
      'Instancia de SQL Server: ' + PreviousSqlInstance);

  SqlPage := CreateCustomPage(wpSelectDir,
    'Base de datos',
    'Instancia de SQL Server donde se creará la base TP_Ing_Soft');

  Descripcion := TNewStaticText.Create(SqlPage);
  Descripcion.Parent := SqlPage.Surface;
  Descripcion.Width := SqlPage.SurfaceWidth;
  Descripcion.WordWrap := True;
  Descripcion.AutoSize := False;
  Descripcion.Height := ScaleY(44);
  Descripcion.Caption := 'Se detectaron las instancias de SQL Server instaladas en este equipo. ' +
    'Elija una de la lista (también puede escribir otra, por ejemplo NOMBRE-PC\INSTANCIA). ' +
    'Se usa autenticación integrada de Windows.';

  Etiqueta := TNewStaticText.Create(SqlPage);
  Etiqueta.Parent := SqlPage.Surface;
  Etiqueta.Top := Descripcion.Top + Descripcion.Height + ScaleY(10);
  Etiqueta.Caption := 'Instancia de SQL Server:';

  SqlCombo := TNewComboBox.Create(SqlPage);
  SqlCombo.Parent := SqlPage.Surface;
  SqlCombo.Style := csDropDown;
  SqlCombo.Top := Etiqueta.Top + Etiqueta.Height + ScaleY(4);
  SqlCombo.Width := SqlPage.SurfaceWidth;
  DetectarInstanciasSql;

  Nota := TNewStaticText.Create(SqlPage);
  Nota.Parent := SqlPage.Surface;
  Nota.Top := SqlCombo.Top + SqlCombo.Height + ScaleY(16);
  Nota.Width := SqlPage.SurfaceWidth;
  Nota.WordWrap := True;
  Nota.AutoSize := False;
  Nota.Height := ScaleY(60);
  Nota.Caption := 'Si la base TP_Ing_Soft no existe en esa instancia, se crea completa con los datos ' +
    'iniciales (usuario Maximo, mesas, carta, empleados y un cliente de prueba). ' +
    'Si ya existe, solo se agrega lo que falte del esquema: sus datos no se modifican.';
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  { En una reinstalación se conserva la instancia anterior (registro SqlInstance). }
  Result := IsUpgrade and (PageID = SqlPage.ID);
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  Instancia, LogFile: String;
begin
  Result := True;
  if CurPageID <> SqlPage.ID then
    Exit;

  Instancia := Trim(SqlCombo.Text);
  if Instancia = '' then
  begin
    MsgBox('Elija o escriba la instancia de SQL Server.', mbError, MB_OK);
    Result := False;
    Exit;
  end;

  if not SqlCmdEncontrado then
  begin
    Result := MsgBox('No se encontró sqlcmd en el PATH, por lo que no se podrá crear la base de datos.' + #13#10 +
                     'Puede crearla luego a mano con Database\Deploy_TP_Ing_Soft.sql (en la carpeta de instalación).' + #13#10#13#10 +
                     '¿Continuar igualmente?', mbConfirmation, MB_YESNO) = IDYES;
    Exit;
  end;

  { Prueba de conexión (30 s: LocalDB puede tardar en arrancar la primera vez). }
  LogFile := ExpandConstant('{tmp}\sql_check.log');
  if RunCmd('sqlcmd -S "' + Instancia + '" -E -b -l 30 -Q "SELECT 1"', LogFile) <> 0 then
  begin
    Result := MsgBox('No se pudo conectar a la instancia "' + Instancia + '":' + #13#10#13#10 +
                     ReadLog(LogFile) + #13#10#13#10 +
                     '¿Continuar igualmente?', mbConfirmation, MB_YESNO) = IDYES;
  end;
end;

function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
begin
  if IsUpgrade and SqlCmdEncontrado then
    Result := 'Modo: Reinstalación / Actualización' + NewLine +
              Space + 'Se reemplazarán los ejecutables y DLLs.' + NewLine +
              Space + 'Base TP_Ing_Soft: copia de seguridad en Database\Backups y actualización del esquema (los datos no se modifican).' + NewLine + NewLine
  else if IsUpgrade then
    Result := 'Modo: Reinstalación / Actualización' + NewLine +
              Space + 'Se reemplazarán los ejecutables y DLLs.' + NewLine +
              Space + 'Sin sqlcmd: la base no se actualizará; ejecute Database\Deploy_TP_Ing_Soft.sql a mano (después de un backup).' + NewLine + NewLine
  else if SqlCmdEncontrado then
    Result := 'Modo: Instalación limpia' + NewLine +
              Space + 'Se creará la base TP_Ing_Soft si no existe (si ya existe, solo se agrega lo que falte del esquema).' + NewLine + NewLine
  else
    Result := 'Modo: Instalación limpia' + NewLine +
              Space + 'Sin sqlcmd: la base de datos deberá crearse a mano.' + NewLine + NewLine;

  Result := Result + MemoDirInfo + NewLine + NewLine +
            'Instancia de SQL Server:' + NewLine + Space + GetSqlInstance('') + NewLine + NewLine;
  if MemoTasksInfo <> '' then
    Result := Result + MemoTasksInfo;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep <> ssPostInstall then
    Exit;

  { En ambos modos el .exe.config se reemplaza: se vuelve a escribir la instancia
    y el modo demostración elegido en el asistente (tarea mododemo). }
  ConfigurarConnectionString(GetSqlInstance(''));
  ConfigurarModoDemostracion(WizardIsTaskSelected('mododemo'));
end;
