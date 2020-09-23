using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewRn
{
    public partial class frmDepsVsTovarNoCorrect : Form
    {
        public frmDepsVsTovarNoCorrect()
        {
            InitializeComponent();
        }

        private void frmDepsVsTovarNoCorrect_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = Config.dtListDepsVsTovarNoCorrect;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
