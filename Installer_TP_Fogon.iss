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
;       - el usuario elige la carpeta de destino y la instancia de SQL Server;
;       - se copian los binarios de GUI\bin\Release y el PDF de docs\;
;       - se ejecuta docs\Seed_TP_Ing_Soft.sql con sqlcmd sobre esa instancia;
;       - se crea App.secrets.config a partir de la plantilla (si no existe);
;       - se crean los accesos directos (Escritorio y Menú Inicio).
;   * Reinstalación / actualización (la clave de registro ya existe):
;       - se reutilizan la carpeta y la instancia SQL de la instalación anterior;
;       - solo se reemplazan ejecutables, DLLs y recursos;
;       - NO se ejecuta la semilla: la base de datos queda intacta;
;       - App.secrets.config (credenciales SMTP) no se toca.
;   * Desinstalación: borra toda la carpeta de instalación (incluidos
;     App.secrets.config y Database\); la base TP_Ing_Soft queda intacta.
;
;  Requisitos en la PC destino: .NET Framework 4.8, SQL Server con la base
;  TP_Ing_Soft creada (esquema + migraciones) y sqlcmd en el PATH.
; =============================================================================

#define MyAppName        "El Fogón del Sur"
#define MyAppVersion     "1.0.0"
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

[Files]
; Ejecutable, DLLs, idiomas.json, GUI_08YS.exe.config y Resources\.
; Se excluyen símbolos/documentación XML, el script de esquema viejo y, sobre todo,
; App.secrets.config: contiene credenciales SMTP reales de la PC de desarrollo.
Source: "{#ReleaseDir}\*"; DestDir: "{app}"; \
    Excludes: "*.pdb,*.xml,App.secrets.config,script.sql,*.vshost.*"; \
    Flags: ignoreversion recursesubdirs createallsubdirs

; Plantilla SMTP y su copia como App.secrets.config (solo si todavía no existe:
; una reinstalación nunca pisa credenciales ya cargadas). Se borra al desinstalar
; (ver [UninstallDelete]).
Source: "GUI\Config\App.secrets.config.template"; DestDir: "{app}"; Flags: ignoreversion
Source: "GUI\Config\App.secrets.config.template"; DestDir: "{app}"; DestName: "App.secrets.config"; \
    Flags: onlyifdoesntexist

; Semilla de la base: se instala siempre (para poder re-aplicarla a mano),
; pero el instalador solo la ejecuta en la instalación limpia.
Source: "docs\Seed_TP_Ing_Soft.sql"; DestDir: "{app}\Database"; Flags: ignoreversion

; Documentación
Source: "docs\*.pdf"; DestDir: "{app}\docs"; Flags: ignoreversion

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
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; \
    WorkingDir: "{app}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Limpieza total: también lo que se crea en tiempo de ejecución o después de
; instalar (log de la semilla, App.secrets.config con credenciales SMTP, etc.).
; La base de datos TP_Ing_Soft en SQL Server NO se toca.
Type: filesandordirs; Name: "{app}\Database"
Type: files; Name: "{app}\App.secrets.config"
Type: filesandordirs; Name: "{app}\*"

[Code]
const
  RegKey         = '{#RegKey}';
  DefaultSqlInst = '{#DefaultSqlInst}';
  NetFx48Release = 528040;

var
  IsUpgrade: Boolean;
  PreviousSqlInstance: String;
  SqlPage: TInputQueryWizardPage;
  SeedCheckBox: TNewCheckBox;

{ ---------------------------------------------------------------------------
  Utilidades
  --------------------------------------------------------------------------- }

function GetSqlInstance(Param: String): String;
begin
  if IsUpgrade then
    Result := PreviousSqlInstance
  else
    Result := Trim(SqlPage.Values[0]);
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
end;

procedure InitializeWizard;
begin
  { Reinstalación: se avisa antes de copiar nada que la base no se va a tocar. }
  if IsUpgrade then
    CreateOutputMsgPage(wpWelcome,
      'Reinstalación detectada',
      'Ya existe una instalación de ' + '{#MyAppName}' + ' en este equipo',
      'Se detectó una versión previa instalada. Se actualizarán los ejecutables conservando la base de datos existente.' + #13#10#13#10 +
      'Carpeta de instalación: ' + WizardForm.PrevAppDir + #13#10 +
      'Instancia de SQL Server: ' + PreviousSqlInstance);

  SqlPage := CreateInputQueryPage(wpSelectDir,
    'Base de datos',
    'Instancia de SQL Server donde está la base TP_Ing_Soft',
    'Ingrese el nombre de la instancia de SQL Server: "." es la instancia predeterminada del equipo ' +
    '(localhost); otros ejemplos: .\SQLEXPRESS, (localdb)\MSSQLLocalDB o NOMBRE-PC\INSTANCIA. ' +
    'Se usa autenticación integrada de Windows.');
  SqlPage.Add('Instancia de SQL Server:', False);
  SqlPage.Values[0] := DefaultSqlInst;

  SeedCheckBox := TNewCheckBox.Create(SqlPage);
  SeedCheckBox.Parent := SqlPage.Surface;
  SeedCheckBox.Top := SqlPage.Edits[0].Top + SqlPage.Edits[0].Height + ScaleY(24);
  SeedCheckBox.Width := SqlPage.SurfaceWidth;
  SeedCheckBox.Height := ScaleY(34);
  SeedCheckBox.Caption := 'Aplicar la base semilla limpia (Seed_TP_Ing_Soft.sql). ' +
    'ATENCIÓN: borra reservas, pedidos, facturas, pagos, bitácora, clientes y usuarios (salvo Maximo) de TP_Ing_Soft.';
  SeedCheckBox.Checked := True;
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  { En una reinstalación se conserva la instancia anterior y la base no se toca. }
  Result := IsUpgrade and (PageID = SqlPage.ID);
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  Instancia, LogFile: String;
begin
  Result := True;
  if CurPageID <> SqlPage.ID then
    Exit;

  Instancia := Trim(SqlPage.Values[0]);
  if Instancia = '' then
  begin
    MsgBox('Ingrese la instancia de SQL Server.', mbError, MB_OK);
    Result := False;
    Exit;
  end;

  if not SqlCmdDisponible then
  begin
    Result := MsgBox('No se encontró sqlcmd en el PATH, por lo que no se podrá aplicar la base semilla.' + #13#10 +
                     'Puede aplicarla luego a mano con Database\Seed_TP_Ing_Soft.sql (en la carpeta de instalación).' + #13#10#13#10 +
                     '¿Continuar igualmente?', mbConfirmation, MB_YESNO) = IDYES;
    Exit;
  end;

  { Prueba de conexión: la instancia responde y existe la base TP_Ing_Soft. }
  LogFile := ExpandConstant('{tmp}\sql_check.log');
  if RunCmd('sqlcmd -S "' + Instancia + '" -E -b -l 10 -Q "IF DB_ID(N''TP_Ing_Soft'') IS NULL RAISERROR(N''No existe la base TP_Ing_Soft.'', 16, 1)"',
            LogFile) <> 0 then
  begin
    Result := MsgBox('No se pudo verificar la base TP_Ing_Soft en "' + Instancia + '":' + #13#10#13#10 +
                     ReadLog(LogFile) + #13#10#13#10 +
                     '¿Continuar igualmente?', mbConfirmation, MB_YESNO) = IDYES;
  end;
end;

function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
begin
  if IsUpgrade then
    Result := 'Modo: Reinstalación / Actualización' + NewLine +
              Space + 'Se reemplazarán los ejecutables y DLLs. La base de datos NO se modifica.' + NewLine + NewLine
  else
  begin
    Result := 'Modo: Instalación limpia' + NewLine;
    if SeedCheckBox.Checked then
      Result := Result + Space + 'Se aplicará la base semilla sobre TP_Ing_Soft.' + NewLine + NewLine
    else
      Result := Result + Space + 'No se aplicará la base semilla.' + NewLine + NewLine;
  end;

  Result := Result + MemoDirInfo + NewLine + NewLine +
            'Instancia de SQL Server:' + NewLine + Space + GetSqlInstance('') + NewLine + NewLine;
  if MemoTasksInfo <> '' then
    Result := Result + MemoTasksInfo;
end;

procedure AplicarSemilla(const Instancia: String);
var
  SeedFile, LogFile: String;
begin
  SeedFile := ExpandConstant('{app}\Database\Seed_TP_Ing_Soft.sql');
  LogFile := ExpandConstant('{app}\Database\Seed_TP_Ing_Soft.log');
  WizardForm.StatusLabel.Caption := 'Aplicando la base semilla en ' + Instancia + '...';

  { -b: aborta con exit code <> 0 ante cualquier error; -f 65001: script en UTF-8. }
  if RunCmd('sqlcmd -S "' + Instancia + '" -E -b -f 65001 -i "' + SeedFile + '"', LogFile) <> 0 then
    MsgBox('No se pudo aplicar la base semilla:' + #13#10#13#10 + ReadLog(LogFile) + #13#10#13#10 +
           'La aplicación quedó instalada. Puede re-ejecutar el script a mano:' + #13#10 +
           'sqlcmd -S "' + Instancia + '" -E -b -f 65001 -i "' + SeedFile + '"', mbError, MB_OK);
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep <> ssPostInstall then
    Exit;

  { En ambos modos el .exe.config se reemplaza: se vuelve a escribir la instancia. }
  ConfigurarConnectionString(GetSqlInstance(''));

  if (not IsUpgrade) and SeedCheckBox.Checked and SqlCmdDisponible then
    AplicarSemilla(GetSqlInstance(''));
end;
