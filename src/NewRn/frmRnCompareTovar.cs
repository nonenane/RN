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
            if (!dtData.Columns.Contains("nameType"))
            {
                dtData.Columns.Add(new DataColumn("nameType", typeof(string)) { DefaultValue = "рассчитанные" });
            }

            if (!dtData.Columns.Contains("idType"))
            {
                dtData.Columns.Add(new DataColumn("idType", typeof(int)) { DefaultValue = 1 });
            }

            if (!dtData.Columns.Contains("isError"))
            {
                dtData.Columns.Add(new DataColumn("isError", typeof(bool)) { DefaultValue = false });
            }

            if (!dtData.Columns.Contains("procent"))
            {
                dtData.Columns.Add(new DataColumn("procent", typeof(decimal)));
            }

            if (dtRN != null && dtRN.Rows.Count > 0 && dtData != null && dtTovarCalculation.Rows.Count > 0)
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
                        cloneRow["nameType"] = "сохраненные";
                        cloneRow["idType"] = 2;
                        cloneRow["isError"] = false;

                        cloneRow["prihod_all"] = (decimal)rowCollect.First()["PrihodSum"] - (decimal)rowCollect.First()["OtgruzSum"] - (decimal)rowCollect.First()["VozvrSum"] - (decimal)rowCollect.First()["SpisSum"] - (decimal)rowCollect.First()["OtgruzOptSum"];

                        cloneRow["realiz_all"] = (decimal)rowCollect.First()["RealizSum"] - (decimal)rowCollect.First()["VozvrKassSum"];

                        cloneRow["rn"] = (decimal)rowCollect.First()["RealizSum"] - (decimal)rowCollect.First()["PrihodSum"] - (decimal)rowCollect.First()["RestStart"] + (decimal)rowCollect.First()["RestStop"];


                        if ((double)cloneRow["rn"] == 0 || (double)cloneRow["realiz_all"] == 0)
                            cloneRow["procent"] = (decimal)0;
                        else
                            cloneRow["procent"] = Math.Round((((decimal)rowCollect.First()["RealizSum"] - (decimal)rowCollect.First()["PrihodSum"] - (decimal)rowCollect.First()["RestStart"] + (decimal)rowCollect.First()["RestStop"]) / ((decimal)rowCollect.First()["RealizSum"] - (decimal)rowCollect.First()["VozvrKassSum"])) * 100,2);
                        //cloneRow["procent"] = ((decimal)cloneRow["rn"] / (decimal)cloneRow["realiz_all"]) * 100;


                        EnumerableRowCollection<DataRow> rowCollectType2 = dtData.AsEnumerable().Where(r => r.Field<int>("id_tovar") == (int)row["id_tovar"] && r.Field<int>("idType") == 1);
                        DataRow rowType2 = rowCollectType2.First();
                        if (
                        (double)cloneRow["r1"] != (double)rowType2["r1"] ||
                        (double)cloneRow["r2"] != (double)rowType2["r2"] ||
                        (double)cloneRow["prihod"] != (double)rowType2["prihod"] ||
                        (double)cloneRow["otgruz"] != (double)rowType2["otgruz"] ||
                        (double)cloneRow["vozvr"] != (double)rowType2["vozvr"] ||
                        (double)cloneRow["spis"] != (double)rowType2["spis"] ||
                        (double)cloneRow["spis_inv"] != (double)rowType2["spis_inv"] ||
                        (double)cloneRow["realiz"] != (double)rowType2["realiz"] ||
                        (double)cloneRow["realiz_opt"] != (double)rowType2["realiz_opt"] ||
                        (double)cloneRow["vozvkass"] != (double)rowType2["vozvkass"] ||
                        (double)cloneRow["prihod_all"] != (double)rowType2["prihod_all"] ||
                        (double)cloneRow["realiz_all"] != (double)rowType2["realiz_all"] ||
                        (double)cloneRow["rn"] != (double)rowType2["rn"] ||
                        (decimal)cloneRow["procent"] != (decimal)rowType2["procent"])
                        {
                            rowType2["isError"] = true;
                            cloneRow["isError"] = true;
                        }

                        //cloneRow[""] = rowCollect.First()[""];
                        //cloneRow[""] = rowCollect.First()[""];
                        dtData.Rows.Add(cloneRow);
                    }

                    //rowCollect = dtData.AsEnumerable().Where(r => r.Field<int>("id_tovar") == (int)row["id_tovar"] && r.Field<int>("idType")==1);
                   

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
                dtData.DefaultView.Sort = "id_tovar asc ";
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

        private void dgvData_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataTable dtData = (dgvData.DataSource as DataTable);
            if (e.RowIndex != -1 && dtData != null && dtData.DefaultView.Count != 0)
            {
                Color rColor = Color.White;
                DataRowView row = dtData.DefaultView[e.RowIndex];
                
                if ((bool)row["isError"]) rColor = panel1.BackColor;
                
                dgvData.Rows[e.RowIndex].DefaultCellStyle.BackColor = rColor;
                dgvData.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = rColor;
                dgvData.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;

                //if ((int)row["r"] != -1 && (int)row["g"] != -1 && (int)row["b"] != -1)
                //    dgvData.Rows[e.RowIndex].Cells[cColor.Index].Style.BackColor =
                //    dgvData.Rows[e.RowIndex].Cells[cColor.Index].Style.SelectionBackColor = Color.FromArgb((int)row["r"], (int)row["g"], (int)row["b"]);
            }
        }

        private void dgvData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            //Рисуем рамку для выделеной строки
            if (dgv.Rows[e.RowIndex].Selected)
            {
                int width = dgv.Width;
                Rectangle r = dgv.GetRowDisplayRectangle(e.RowIndex, false);
                Rectangle rect = new Rectangle(r.X, r.Y, width - 1, r.Height - 1);

                ControlPaint.DrawBorder(e.Graphics, rect,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid);
            }
        }
    }
}
