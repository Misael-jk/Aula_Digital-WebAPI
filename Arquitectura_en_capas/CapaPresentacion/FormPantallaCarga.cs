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
    public partial class FormPantallaCarga : Form
    {
        public FormPantallaCarga()
        {
            InitializeComponent();
        }

        private void FormPantallaCarga_Load(object sender, EventArgs e)
        {

        }

        private void tmrCarga_Tick(object sender, EventArgs e)
        {
            if (pgbCarga.Value < 100)
            {
                pgbCarga.Value += 1;
                lblPorcentaje.Text = pgbCarga.Value.ToString() + "%";
            }
            else
            {
                tmrCarga.Stop();

                Task.Delay(300).ContinueWith(_ =>
                {
                    Invoke(new Action(() =>
                    {
                        this.Close();
                    }));
                });
            }
        }
    }
}
