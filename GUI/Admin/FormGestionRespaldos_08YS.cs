using BLL_08YS;
using Service_08YS;
using Service_08YS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS.Admin
{
    public partial class FormGestionRespaldos_08YS : Form, IIdiomaObserver_08YS
    {
        private readonly BackupBLL_08YS _bll;

        private static readonly Color ColorFondo = Color.FromArgb(5, 15, 45);
        private static readonly Color ColorOro = Color.Goldenrod;
        private static readonly Color ColorTexto = Color.White;
        private static readonly Color ColorExito = Color.LimeGreen;
        private static readonly Color ColorError = Color.OrangeRed;

        public FormGestionRespaldos_08YS()
        {
            InitializeComponent();
            _bll = BLLFactory_08YS.CreateBackupBLL();
        }

        #region Idioma
        public void UpdateIdioma()
        {
            TraducirControles(this);
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                {
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());
                }

                if (c.HasChildren)
                {
                    TraducirControles(c);
                }
            }
        }
        #endregion

        private void FormGestionRespaldos_Load(object sender, EventArgs e)
        {
            ConfigurarBotones();
            PrecargarCarpetaDefault();
            ActualizarPreviewNombre();
        }

        private void PrecargarCarpetaDefault()
        {
            string carpeta = BackupBLL_08YS.CarpetaDefault;
            txtCarpetaDestino.Text = carpeta;
            ActualizarPreviewNombre();

            // Validar la carpeta por defecto al abrir el form
            ValidarYMostrarCarpeta(carpeta);
        }

        private void ActualizarPreviewNombre()
        {
            lblNombreArchivo.Text = _bll.GenerarNombreArchivo();
        }

        private void btnSeleccionarCarpetaBackup_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = TraductorManager_08YS.Instance.GetTexto("backup_seleccionar_carpeta");
                dialog.ShowNewFolderButton = true;
                dialog.SelectedPath = txtCarpetaDestino.Text;

                if (dialog.ShowDialog() != DialogResult.OK) return;

                string carpeta = dialog.SelectedPath;

                // Validar inmediatamente al seleccionar — antes de que el usuario intente el backup
                ValidarYMostrarCarpeta(carpeta);
            }
        }

        private void ValidarYMostrarCarpeta(string carpeta)
        {
            txtCarpetaDestino.Text = carpeta;
            ActualizarPreviewNombre();
            OcultarResultadoBackup();

            try
            {
                Cursor = Cursors.WaitCursor;
                btnRealizarBackUp.Enabled = false;

                var resultado = _bll.ValidarCarpeta(carpeta);

                switch (resultado.Tipo)
                {
                    case ResultadoValidacion_08YS.TipoResultado.Ok:
                        MostrarValidacionCarpeta(true, false,
                            TraductorManager_08YS.Instance.GetTexto("backup_carpeta_valida"));
                        btnRealizarBackUp.Enabled = true;
                        break;

                    case ResultadoValidacion_08YS.TipoResultado.Advertencia:
                        // Puede continuar pero con riesgo
                        MostrarValidacionCarpeta(true, true, resultado.Mensaje);
                        btnRealizarBackUp.Enabled = true;
                        break;

                    case ResultadoValidacion_08YS.TipoResultado.Error:
                        MostrarValidacionCarpeta(false, false, resultado.Mensaje);
                        btnRealizarBackUp.Enabled = false;
                        break;
                }
            }
            finally { Cursor = Cursors.Default; }
        }

        private void MostrarValidacionCarpeta(bool valido, bool esAdvertencia, string mensaje)
        {
            //if (valido && !esAdvertencia)
            //{
            //    picEstadoBackup.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            //    picEstadoBackup.IconColor = ColorExito;
            //    lblResultadoBackup.ForeColor = ColorExito;
            //}
            //else if (esAdvertencia)
            //{
            //    picEstadoBackup.IconChar = FontAwesome.Sharp.IconChar.ExclamationTriangle;
            //    picEstadoBackup.IconColor = Color.Orange;
            //    lblResultadoBackup.ForeColor = Color.Orange;
            //}
            //else
            //{
            //    picEstadoBackup.IconChar = FontAwesome.Sharp.IconChar.TimesCircle;
            //    picEstadoBackup.IconColor = ColorError;
            //    lblResultadoBackup.ForeColor = ColorError;
            //}

            //lblResultadoBackup.Text = mensaje;
            //picEstadoBackup.Visible = true;
            //lblResultadoBackup.Visible = true;
        }

        private void btnRealizarBackUp_Click(object sender, EventArgs e)
        {
            string carpeta = txtCarpetaDestino.Text.Trim();
            if (string.IsNullOrWhiteSpace(carpeta)) return;

            // Actualizar preview con timestamp exacto del momento de backup
            ActualizarPreviewNombre();

            try
            {
                Cursor = Cursors.WaitCursor;
                btnRealizarBackUp.Enabled = false;

                string rutaCreada = _bll.RealizarBackup(carpeta);

                MostrarResultadoBackup(true,
                    $"{TraductorManager_08YS.Instance.GetTexto("backup_ok")}\n{Path.GetFileName(rutaCreada)}");
            }
            catch (InvalidOperationException ex)
            {
                MostrarResultadoBackup(false, ex.Message);
            }
            catch (Exception ex)
            {
                MostrarResultadoBackup(false,
                    TraductorManager_08YS.Instance.GetTexto("backup_error") + "\n" + ex.Message);
            }
            finally
            {
                btnRealizarBackUp.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void MostrarResultadoBackup(bool exito, string mensaje)
        {
            //picEstadoBackup.IconChar = exito
            //    ? FontAwesome.Sharp.IconChar.CheckCircle
            //    : FontAwesome.Sharp.IconChar.TimesCircle;
            //picEstadoBackup.IconColor = exito ? ColorExito : ColorError;

            //lblResultadoBackup.ForeColor = exito ? ColorExito : ColorError;
            //lblResultadoBackup.Text = mensaje;

            //picEstadoBackup.Visible = true;
            //lblResultadoBackup.Visible = true;
        }

        private void OcultarResultadoBackup()
        {
            //picEstadoBackup.Visible = false;
            //lblResultadoBackup.Visible = false;
        }

        private void btnSeleccionarArchivoRestore_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = TraductorManager_08YS.Instance.GetTexto("restore_seleccionar_archivo");
                dialog.Filter = "Archivos de backup (*.bak)|*.bak|Todos los archivos (*.*)|*.*";
                dialog.FilterIndex = 1;

                // Abrir en la misma carpeta del backup si ya hay una configurada
                if (!string.IsNullOrEmpty(txtCarpetaDestino.Text) &&
                    Directory.Exists(txtCarpetaDestino.Text))
                    dialog.InitialDirectory = txtCarpetaDestino.Text;
                else
                    dialog.InitialDirectory = BackupBLL_08YS.CarpetaDefault;

                if (dialog.ShowDialog() != DialogResult.OK) return;

                txtArchivoRestore.Text = dialog.FileName;
                btnRealizarRestore.Enabled = true;

                // Mostrar info del archivo seleccionado
                var info = new FileInfo(dialog.FileName);
                lblInfoArchivo.Text = string.Format(
                    TraductorManager_08YS.Instance.GetTexto("restore_info_archivo"),
                    info.Name,
                    (info.Length / (1024.0 * 1024)).ToString("F1"),
                    info.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss"));

                pnlInfoArchivo.Visible = true;
                OcultarResultadoRestore();
            }
        }

        private void btnRealizarRestore_Click(object sender, EventArgs e)
        {
            string rutaBak = txtArchivoRestore.Text.Trim();
            if (string.IsNullOrWhiteSpace(rutaBak)) return;

            // Confirmación doble por la gravedad de la operación
            var confirmacion = MessageBox.Show(
                TraductorManager_08YS.Instance.GetTexto("restore_confirm_mensaje"),
                TraductorManager_08YS.Instance.GetTexto("restore_confirm_titulo"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2); // "No" como opción por defecto

            if (confirmacion != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                btnRealizarRestore.Enabled = false;

                _bll.RestaurarBackup(rutaBak);

                // Restore exitoso: avisar y reiniciar
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("restore_ok"),
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Application.Restart();
            }
            catch (InvalidOperationException ex)
            {
                btnRealizarRestore.Enabled = true;
                MostrarResultadoRestore(false, ex.Message);
            }
            catch (Exception ex)
            {
                btnRealizarRestore.Enabled = true;
                MostrarResultadoRestore(false,
                    TraductorManager_08YS.Instance.GetTexto("restore_error") + "\n" + ex.Message);
            }
            finally { Cursor = Cursors.Default; }
        }

        private void MostrarResultadoRestore(bool exito, string mensaje)
        {
            //picEstadoRestore.IconChar = exito
            //    ? FontAwesome.Sharp.IconChar.CheckCircle
            //    : FontAwesome.Sharp.IconChar.TimesCircle;
            //picEstadoRestore.IconColor = exito ? ColorExito : ColorError;

            //lblResultadoRestore.ForeColor = exito ? ColorExito : ColorError;
            //lblResultadoRestore.Text = mensaje;

            //picEstadoRestore.Visible = true;
            //lblResultadoRestore.Visible = true;
        }

        private void OcultarResultadoRestore()
        {
            //picEstadoRestore.Visible = false;
            //lblResultadoRestore.Visible = false;
        }

        private void ConfigurarBotones()
        {
            foreach (var btn in new FontAwesome.Sharp.IconButton[]
                { btnSeleccionarCarpetaBackup, btnSeleccionarArchivoRestore, btnRealizarBackUp, btnRealizarRestore })
            {
                btn.BackColor = ColorFondo;
                btn.ForeColor = ColorOro;
                btn.IconColor = ColorOro;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = ColorOro;
                btn.FlatAppearance.BorderSize = 1;

                btn.MouseEnter += (s, _) => {
                    var b = (FontAwesome.Sharp.IconButton)s;
                    if (!b.Enabled) return;
                    b.BackColor = ColorOro;
                    b.ForeColor = ColorFondo;
                    b.IconColor = ColorFondo;
                };
                btn.MouseLeave += (s, _) => {
                    var b = (FontAwesome.Sharp.IconButton)s;
                    b.BackColor = ColorFondo;
                    b.ForeColor = ColorOro;
                    b.IconColor = ColorOro;
                };
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
