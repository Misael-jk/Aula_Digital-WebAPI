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

namespace CapaPresentacion
{
    public partial class NotebooksUC : UserControl
    {
        public readonly NotebooksCN notebooksCN;
        public NotebooksUC(NotebooksCN notebooksCN)
        {
            InitializeComponent();

            this.notebooksCN = notebooksCN;
        }

        private void NotebooksUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1200);
            dtgNotebook.DataSource = notebooksCN.GetAll();   
        }
    }
}
