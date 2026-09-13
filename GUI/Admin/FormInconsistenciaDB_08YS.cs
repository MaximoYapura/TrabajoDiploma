using BLL_08YS;
using Service_08YS;
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
    public partial class FormInconsistenciaDB_08YS : Form, IIdiomaObserver_08YS
    {
        private readonly DvBLL_08YS _dvBLL;
        private readonly BackupBLL_08YS _backupBLL;

        private static readonly Color ColorFondo = Color.FromArgb(5, 15, 45);
        private static readonly Color ColorOro = Color.Goldenrod;
        private static readonly Color ColorPeligro = Color.OrangeRed;

        public FormInconsistenciaDB_08YS()  
        {
            InitializeComponent();
            _dvBLL = BLLFactory_08YS.CreateDvBLL();
            _backupBLL = BLLFactory_08YS.CreateBackupBLL();
        }

        private void FormInconsistenciaDB_08YS_Load(object sender, EventArgs e)
        {
            ConfigurarBotones();
            UpdateIdioma();
        }

        #region Idioma
        public void UpdateIdioma()
            => TraducirControles(this);

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

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
            TraductorManager_08YS.Instance.GetTexto("dv_confirm_recalcular"),
            TraductorManager_08YS.Instance.GetTexto("dv_confirm_titulo"),
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _dvBLL.Recalcular();

                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("dv_recalcular_ok"),
                    string.Empty,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Cierra el form y devuelve control a FormLogin
                // El usuario vuelve a intentar el login con datos ahora normalizados
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    TraductorManager_08YS.Instance.GetTexto("error_critico"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally { Cursor = Cursors.Default; }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            string rutaBak = SeleccionarArchivoBackup();
            if (string.IsNullOrEmpty(rutaBak)) return;

            // Mostrar info del archivo + confirmación final
            var info = new FileInfo(rutaBak);
            string mensajeConfirm = string.Format(
                TraductorManager_08YS.Instance.GetTexto("dv_restore_confirm"),
                info.Name,
                (info.Length / (1024.0 * 1024)).ToString("F1"),
                info.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss"));

            var confirm = MessageBox.Show(
                mensajeConfirm,
                TraductorManager_08YS.Instance.GetTexto("dv_confirm_titulo"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                btnRestore.Enabled = false;
                btnRecalcular.Enabled = false;

                _backupBLL.RestaurarBackup(rutaBak);

                // Restore exitoso: el repositorio ya hizo ClearAllPools
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("dv_restore_ok"),
                    string.Empty,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Application.Restart();
            }
            catch (InvalidOperationException ex)
            {
                // Error de validación (archivo no existe, formato inválido)
                MessageBox.Show(
                    ex.Message,
                    string.Empty,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                btnRestore.Enabled = true;
                btnRecalcular.Enabled = true;
            }
            catch (Exception ex)
            {
                // Error técnico de SQL Server u otro inesperado
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("dv_restore_error") + "\n\n" + ex.Message,
                    TraductorManager_08YS.Instance.GetTexto("error_critico"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                btnRestore.Enabled = true;
                btnRecalcular.Enabled = true;
            }
            finally { Cursor = Cursors.Default; }
        }

        private string SeleccionarArchivoBackup()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = TraductorManager_08YS.Instance.GetTexto("restore_seleccionar_archivo");
                dialog.Filter = "Archivos de backup (*.bak)|*.bak|Todos los archivos (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.InitialDirectory = BackupBLL_08YS.CarpetaDefault;

                return dialog.ShowDialog() == DialogResult.OK
                    ? dialog.FileName
                    : string.Empty;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ConfigurarBotones()
        {
            // Botones normales (oro)
            foreach (var btn in new FontAwesome.Sharp.IconButton[] { btnRecalcular, btnRestore })
            {
                EstilizarBoton(btn, ColorOro);
            }

            // Botón de salida (rojo — acción destructiva / sin resolución)
            EstilizarBoton(btnSalir, ColorPeligro);
        }

        private void EstilizarBoton(FontAwesome.Sharp.IconButton btn, Color color)
        {
            btn.BackColor = ColorFondo;
            btn.ForeColor = color;
            btn.IconColor = color;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = color;
            btn.FlatAppearance.BorderSize = 1;
            btn.IconSize = 22;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(12, 0, 0, 0);

            btn.MouseEnter += (s, _) =>
            {
                var b = (FontAwesome.Sharp.IconButton)s;
                b.BackColor = color;
                b.ForeColor = ColorFondo;
                b.IconColor = ColorFondo;
            };
            btn.MouseLeave += (s, _) =>
            {
                var b = (FontAwesome.Sharp.IconButton)s;
                b.BackColor = ColorFondo;
                b.ForeColor = color;
                b.IconColor = color;
            };
        }
    }
}
