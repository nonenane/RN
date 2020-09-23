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

        public DataTable dtTovarCalculation { set; private get; }
        public int id_otdel { set; private get; }
        public int id_group { set; private get; }

        private DataTable dtData;

        public frmRnCompareTovar()
        {
            InitializeComponent();
            //доступ к данным на сервере                
            sql = new Sql(ConnectionSettings.GetServer(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase(), ConnectionSettings.ProgramName);
            sqlVVO = new Sql(ConnectionSettings.GetServer("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase("2"), ConnectionSettings.ProgramName);
            dgvData.AutoGenerateColumns = false;
        }

        private void frmRnCompareTovar_Load(object sender, EventArgs e)
        {
            DataTable dtDeps = sql.getDepartments(true);
            cmbDeps.DataSource = dtDeps;
            cmbDeps.ValueMember = "id";
            cmbDeps.DisplayMember = "name";
            cmbDeps_SelectionChangeCommitted(null, null);
            DataTable dtRN = null;
            try
            {
                dtRN = Config.dtDaveRN.AsEnumerable().Where(r => r.Field<int>("id_department") == id_otdel && r.Field<int>("id_grp1") == id_group).CopyToDataTable();
            }
            catch (Exception ex) { }

            dtData = dtTovarCalculation.Copy();
            if (dtRN != null && dtRN.Rows.Count > 0 && dtTovarCalculation != null && dtTovarCalculation.Rows.Count > 0)
            {
                foreach (DataRow row in dtTovarCalculation.Rows)
                {
                    DataRow cloneRow = dtData.NewRow();
                    cloneRow.ItemArray = row.ItemArray.Clone() as object[];
                    EnumerableRowCollection<DataRow> rowCollect = dtRN.AsEnumerable().Where(r => r.Field<int>("id_tovar") == (int)row["id_tovar"]);
                    if (rowCollect.Count() > 0)
                    {
                        cloneRow["r1"] = rowCollect.First()["RestStart"];
                        cloneRow["r2"] = rowCollect.First()["RestStop"];
                        cloneRow["prihod"] = rowCollect.First()["PrihodSum"];
                        cloneRow["otgruz"] = rowCollect.First()["OtgruzSum"];
                        cloneRow["vozvr"] = rowCollect.First()["VozvrSum"];
                        cloneRow["spis"] = rowCollect.First()["SpisSum"];
                        cloneRow["spis_inv"] = rowCollect.First()["InventSpisSum"];
                        cloneRow["realiz"] = rowCollect.First()["RealizSum"];
                        cloneRow["realiz_opt"] = rowCollect.First()["OtgruzOptSum"];
                        cloneRow["vozvkass"] = rowCollect.First()["VozvrKassSum"];
                        //cloneRow[""] = rowCollect.First()[""];
                        //cloneRow[""] = rowCollect.First()[""];
                        dtData.Rows.Add(cloneRow);
                    }                    
                }
                /*
                int id_tovar = int.Parse(rowGoods["id"].ToString());
                int id_grp1 = int.Parse(rowGoods["id_grp1"].ToString());
                int id_grp2 = int.Parse(rowGoods["id_grp2"].ToString());
                int id_otdel = (int)rowGoods["id_otdel"];
                decimal RestStart = decimal.Parse(rowGoods["r1"].ToString());
                decimal RestStop = decimal.Parse(rowGoods["r2"].ToString());

                decimal PrihodSum = decimal.Parse(rowGoods["prihod"].ToString());
                decimal OtgruzSum = decimal.Parse(rowGoods["otgruz"].ToString());
                decimal VozvrSum = decimal.Parse(rowGoods["vozvr"].ToString());
                decimal SpisSum = decimal.Parse(rowGoods["spis"].ToString());
                decimal InventSpisSum = decimal.Parse(rowGoods["spis_inv"].ToString());
                decimal RealizSum = decimal.Parse(rowGoods["realiz"].ToString());
                decimal OtgruzOptSum = decimal.Parse(rowGoods["realiz_opt"].ToString());
                decimal VozvrKassSum = decimal.Parse(rowGoods["vozvkass"].ToString());
                */
            }

            

            setFilter();
            dgvData.DataSource = dtData;
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

            setFilter(); 

        }

        private void cmbGrp1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cmbGrp2.SelectedIndex = 0;
            setFilter();
        }

        private void cmbGrp2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cmbGrp1.SelectedIndex = 0;
            setFilter();
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

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void setFilter()
        {
            if (dtData == null || dtData.Rows.Count == 0)
            {
                btPrint.Enabled = false;
                return;
            }

            try
            {
                string filter = "";

                if (tbEan.Text.Trim().Length != 0)
                    filter += (filter.Length == 0 ? "" : " and ") + $"ean like '%{tbEan.Text.Trim()}%'";

                if (tbName.Text.Trim().Length != 0)
                    filter += (filter.Length == 0 ? "" : " and ") + $"cname like '%{tbName.Text.Trim()}%'";

                if ((Int16)cmbDeps.SelectedValue!=-1)
                    filter += (filter.Length == 0 ? "" : " and ") + $"id_otdel = {cmbDeps.SelectedValue}";

                if ((int)cmbGrp1.SelectedValue != -1)
                    filter += (filter.Length == 0 ? "" : " and ") + $"id_grp1 = {cmbGrp1.SelectedValue}";

                if ((int)cmbGrp2.SelectedValue != -1)
                    filter += (filter.Length == 0 ? "" : " and ") + $"id_grp2 = {cmbGrp2.SelectedValue}";

                dtData.DefaultView.RowFilter = filter;
            }
            catch
            {
                dtData.DefaultView.RowFilter = "id_otdel = -1";
            }
            finally
            {
                btPrint.Enabled =
                dtData.DefaultView.Count != 0;                
            }
        }

        private void tbEan_TextChanged(object sender, EventArgs e)
        {
            setFilter();
        }
    }
}
