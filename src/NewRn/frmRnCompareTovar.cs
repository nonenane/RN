using NewRn.Data;
using Nwuram.Framework.Settings.Connection;
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
    public partial class frmRnCompareTovar : Form
    {
        Sql sql = null;
        Sql sqlVVO = null;
        public frmRnCompareTovar()
        {
            InitializeComponent();
            //доступ к данным на сервере                
            sql = new Sql(ConnectionSettings.GetServer(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase(), ConnectionSettings.ProgramName);
            sqlVVO = new Sql(ConnectionSettings.GetServer("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase("2"), ConnectionSettings.ProgramName);
        }

        private void frmRnCompareTovar_Load(object sender, EventArgs e)
        {
            DataTable dtDeps = sql.getDepartments(true);
            cmbDeps.DataSource = dtDeps;
            cmbDeps.ValueMember = "id";
            cmbDeps.DisplayMember = "name";
            cmbDeps_SelectionChangeCommitted(null, null);
        }

        private void cmbDeps_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataTable dtGrp1 = sql.getGroups((Int16)cmbDeps.SelectedValue);
            cmbGrp1.DataSource = dtGrp1;
            cmbGrp1.ValueMember = "id";
            cmbGrp1.DisplayMember = "name";

            DataTable dtGrp2 = sql.getGroups_inv((Int16)cmbDeps.SelectedValue);
            cmbGrp2.DataSource = dtGrp2;
            cmbGrp2.ValueMember = "id";
            cmbGrp2.DisplayMember = "name";

        }

        private void cmbGrp1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cmbGrp2.SelectedIndex = 0;
        }

        private void cmbGrp2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cmbGrp1.SelectedIndex = 0;
        }

        private void dgvData_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            int width = 0;
            foreach (DataGridViewColumn col in dgvData.Columns)
            {
                if (col.Name.Equals(cEan.Name))
                {
                    tbEan.Location = new Point(dgvData.Location.X + width+1, tbEan.Location.Y);
                    tbEan.Size = new Size(cEan.Width, tbEan.Height);
                }
                else
                if (col.Name.Equals(cName.Name))
                {
                    tbName.Location = new Point(dgvData.Location.X + width + 1, tbEan.Location.Y);
                    tbName.Size = new Size(cName.Width, tbName.Height);
                }

                width += col.Width;
            }
        }
    }
}
