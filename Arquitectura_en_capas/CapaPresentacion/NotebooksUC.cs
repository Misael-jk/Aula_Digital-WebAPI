using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaDatos.InterfacesDTO;

namespace CapaPresentacion
{
    public partial class NotebooksUC : UserControl
    {
        public readonly NotebooksCN notebooksCN;
        private readonly IMapperModelo mapperModelos;
        public NotebooksUC(NotebooksCN notebooksCN, IMapperModelo mapperModelo)
        {
            InitializeComponent();
            mapperModelos = mapperModelo;
            this.notebooksCN = notebooksCN;
        }

        private void NotebooksUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1200);
            dtgNotebook.DataSource = notebooksCN.GetAll();
        }

        private void btnAgregarNotebook_Click(object sender, EventArgs e)
        {
            var Notebook = new FormCRUDNotebook(notebooksCN, mapperModelos);
            Notebook.Show();
        }
    }
}
