using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace GUI_08YS
{
    public class DropdownMenuStrip_08YS : ContextMenuStrip
    {
        //Campos
        private bool isMainMenu;
        private int menuItemHeight = 25;
        private Color menuItemTextColor = Color.Empty; //Sin color. El predeterminado esta en la clase MenuRenderer
        private Color primaryColor = Color.Empty; //Sin color. El predeterminado esta en la clase MenuRenderer

        private Bitmap menuItemHeaderSize;

        //Constructor
        public DropdownMenuStrip_08YS(IContainer container)
            : base(container)
        {

        }

        //Propiedades
        //Opcionalmente, ocultar las propiedades en el cuadro de herramientas para evitar el problema de mostrar y/o 
        //guardar los cambios de las propiedades del control en el diseñador en tiempo de diseño en Visual Studio.
        [Browsable(false)]
        public bool IsMainMenu
        {
            get { return isMainMenu; }
            set { isMainMenu = value; }
        }

        [Browsable(false)]
        public int MenuItemHeight
        {
            get { return menuItemHeight; }
            set { menuItemHeight = value; }
        }

        [Browsable(false)]
        public Color MenuItemTextColor
        {
            get { return menuItemTextColor; }
            set { menuItemTextColor = value; }
        }

        [Browsable(false)]
        public Color PrimaryColor
        {
            get { return primaryColor; }
            set { primaryColor = value; }
        }

        //Metodos privados
        private void LoadMenuItemApparence()
        {
            if (IsMainMenu)
            {
                menuItemHeaderSize = new Bitmap(25, 45);
                menuItemTextColor = Color.Gainsboro;
            }
            else
                menuItemHeaderSize = new Bitmap(15, menuItemHeight);
            creaElementos(Items);
        }

        private void creaElementos(ToolStripItemCollection items)
        {
            foreach (ToolStripMenuItem item in items)
            {
                item.ForeColor = menuItemTextColor;
                item.ImageScaling = ToolStripItemImageScaling.None;
                if (item.Image == null) item.Image = menuItemHeaderSize;
                creaElementos(item.DropDownItems);
            }
        }

        //Overrides
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this.DesignMode == false)
            {
                this.Renderer = new MenuRenderer(isMainMenu, primaryColor, menuItemTextColor);
                LoadMenuItemApparence();
            }
        }
    }
}
