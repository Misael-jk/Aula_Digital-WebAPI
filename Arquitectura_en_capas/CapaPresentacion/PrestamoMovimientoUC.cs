using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.InterfacesDTO;

namespace CapaPresentacion
{
    public partial class PrestamosYDevolucionesUC : UserControl
    {
        private readonly IMapperTransaccion mapperTransaccion;
        public PrestamosYDevolucionesUC(IMapperTransaccion mapperTransaccion)
        {
            InitializeComponent();
            this.mapperTransaccion = mapperTransaccion;
        }

        private void PrestamosYDevolucionesUC_Load(object sender, EventArgs e)
        {
            dvgPrestamosYDevoluciones.DataSource = mapperTransaccion.GetAllDTO();
        }
    }
}
