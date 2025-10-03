using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class InfoUsuario : Form
    {
        public InfoUsuario()
        {
            InitializeComponent();
        }

        private void BtnCerrar1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
