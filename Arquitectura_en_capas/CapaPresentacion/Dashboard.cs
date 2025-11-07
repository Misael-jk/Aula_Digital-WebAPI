using CapaDatos.InterfacesDTO;
using CapaDatos.MappersDTO;
using CapaEntidad;
using CapaNegocio;
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
    public partial class Dashboard : UserControl
    {
        private readonly MapperHistorialElemento mapperHistorialElemento;
        private readonly IMapperPrestamosActivos mapperPrestamosActivos; 
        public Dashboard(IMapperPrestamosActivos mapperPrestamosActivos)
        {
            InitializeComponent();
            this.mapperPrestamosActivos = mapperPrestamosActivos;
        }

        public void MostrarDatos()
        {
            //var elemento = mapperHistorialElemento.GetAllDTO();
            //dataGridView1.DataSource = elemento.ToList();
            dgvPrestamosActivos.DataSource = mapperPrestamosActivos.GetAllDTO().ToList();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            MostrarDatos();
        }
    }
}
