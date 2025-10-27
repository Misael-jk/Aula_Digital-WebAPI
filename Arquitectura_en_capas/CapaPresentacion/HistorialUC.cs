using CapaDatos.InterfacesDTO;
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
    public partial class HistorialUC : UserControl
    {
        private readonly IMapperHistorialCarrito mapperHistorialCarrito;
        private readonly IMapperHistorialNotebook mapperHistorialNotebook;
        private readonly IMapperHistorialElemento mapperHistorialElemento;

        public HistorialUC(IMapperHistorialElemento mapperHistorialElemento, IMapperHistorialNotebook mapperHistorialNotebook, IMapperHistorialCarrito historialCarrito)
        {
            InitializeComponent();
            this.mapperHistorialNotebook = mapperHistorialNotebook;
            this.mapperHistorialElemento = mapperHistorialElemento;
            this.mapperHistorialCarrito = historialCarrito;
        }

        private void HistorialUC_Load(object sender, EventArgs e)
        {
        }

        public void RefrescarDatos()
        {
            guna2DataGridView1.DataSource = mapperHistorialNotebook.GetAllDTO();
            guna2DataGridView2.DataSource = mapperHistorialElemento.GetAllDTO();
            guna2DataGridView3.DataSource = mapperHistorialCarrito.GetAllDTO();
        }
    }
}
