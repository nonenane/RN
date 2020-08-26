using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewRn
{
    public partial class frmRNCompare : Form
    {
        public DateTime dateStart { set; private get; }
        public DateTime dateStop { set; private get; }


        public frmRNCompare()
        {
            InitializeComponent();
        }

        private void frmRNCompare_Load(object sender, EventArgs e)
        {
            dtpStart.Value = dateStart.Date;
            dtpEnd.Value = dateStop.Date;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            if (dtpStart.Value > dtpEnd.Value)
            {
                dtpEnd.Value = dtpStart.Value;
            }
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dtpStart.Value > dtpEnd.Value)
            {
                dtpStart.Value = dtpEnd.Value;
            }
        }
    }
}
