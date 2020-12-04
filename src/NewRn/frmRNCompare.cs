using NewRn.Data;
using Nwuram.Framework.Settings.Connection;
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
    public partial class frmRNCompare : Form
    {

        private Sql sql = null;
        private Sql sqlVVO = null;
        private DataTable dtData;
        private Nwuram.Framework.UI.Service.EnableControlsServiceInProg blockers = new Nwuram.Framework.UI.Service.EnableControlsServiceInProg();

        public DateTime dateStart { set; private get; }
        public DateTime dateStop { set; private get; }


        public frmRNCompare()
        {
            InitializeComponent();
            sql = new Sql(ConnectionSettings.GetServer(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase(), ConnectionSettings.ProgramName);
            sqlVVO = new Sql(ConnectionSettings.GetServer("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase("2"), ConnectionSettings.ProgramName);
            dgvData.AutoGenerateColumns = false;
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

        Dictionary<int, Store> dicListData = new Dictionary<int, Store>();

        private async void btGetData_Click(object sender, EventArgs e)
        {
            try
            {
                dicListData.Clear();
                DateTime dateStart = dtpStart.Value.Date;
                DateTime dateEnd = dtpEnd.Value.Date;
                blockers.SaveControlsEnabledState(this);
                blockers.SetControlsEnabled(this, false);
                progressBar1.Visible = true;

                //btGetData.Enabled = false;
                var result = await Task<bool>.Factory.StartNew(() =>
                {
                    
                    Task<DataTable> task = sql.getTSaveRN(dateStart, dateEnd);
                    task.Wait();

                    if (task.Result == null || task.Result.Rows.Count == 0)
                    {
                        MessageBox.Show(Config.centralText("За указанный период отсутствуют\n ранее сохранённые данные.\nРасчёт не может быть произведён\n"), "Проверка наличия сохранённых данных.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        blockers.RestoreControlEnabledState(this);
                        progressBar1.Visible = false;
                        dtData = null;
                        return false;
                    }
                    dtData = task.Result;
                    task = null;
                    Config.isCompareData = true;

                    if (!dtData.Columns.Contains("isError"))
                    {
                        dtData.Columns.Add(new DataColumn("isError", typeof(bool)) { DefaultValue = false });
                    }

                    foreach (DataRow row in dtData.Copy().Rows)
                    {
                        int id = (int)row["id"];
                        DateTime _dateStart = (DateTime)row["DateStart"];
                        DateTime _dateEnd = (DateTime)row["DateEnd"];

                        bool isInventorySpis = (bool)row["isInventorySpis"];
                        bool isOnlyShipped = (bool)row["isOnlyShipped"];
                        bool isOptOtgruz = (bool)row["isOptOtgruz"];

                        DataTable countResult = null;
                        Config.isInventSpis = isInventorySpis;
                        Config.id_TSaveRN = id;

                        Store store = new Store();
                        countResult = store.CountRN(sql, sqlVVO, _dateStart, _dateEnd, isOptOtgruz, isOnlyShipped);

                        dicListData.Add(id, store);

                        DataRow newRow = dtData.NewRow();
                        newRow.ItemArray = row.ItemArray.Clone() as object[];

                        newRow["typeCalc"] = "Рассчитанные";
                        newRow["TotalPrihod"] = store.PrihodAll;
                        newRow["TotalRealiz"] = store.RealizAll;
                        newRow["TotalRestStart"] = store.RemainStart;
                        newRow["TotalRestStop"] = store.RemainFinish;
                        newRow["TotalRN"] = store.RN;
                        newRow["TotalPercentRN"] = store.Procent;


                        EnumerableRowCollection<DataRow> rowCollectType2 = dtData.AsEnumerable().Where(r => r.Field<int>("id") == id);
                        DataRow rowType2 = rowCollectType2.First();
                        if (
                        Math.Round((decimal)newRow["TotalPrihod"],2) != Math.Round((decimal)rowType2["TotalPrihod"], 2) ||
                        Math.Round((decimal)newRow["TotalRealiz"], 2) != Math.Round((decimal)rowType2["TotalRealiz"], 2) ||
                        Math.Round((decimal)newRow["TotalRestStart"], 2) != Math.Round((decimal)rowType2["TotalRestStart"], 2) ||
                        Math.Round((decimal)newRow["TotalRestStop"], 2) != Math.Round((decimal)rowType2["TotalRestStop"], 2) ||
                        Math.Round((decimal)newRow["TotalRN"], 2) != Math.Round((decimal)rowType2["TotalRN"], 2) ||
                        Math.Round((decimal)newRow["TotalPercentRN"], 2) != Math.Round((decimal)rowType2["TotalPercentRN"], 2))
                        {
                            rowType2["isError"] = true;
                            newRow["isError"] = true;
                        }


                        dtData.Rows.Add(newRow);


                        //}
                        //else
                        //{
                        //    countResult = store.CountRN_inv(sql, sqlVVO, Convert.ToDateTime(parameters[0]), Convert.ToDateTime(parameters[1]), Convert.ToBoolean(parameters[2]), Convert.ToBoolean(parameters[3]));
                        //}
                    }
                    dtData.DefaultView.Sort = "id asc";
                    dtData = dtData.DefaultView.ToTable().Copy();

                    Config.DoOnUIThread(() =>
                    {
                        blockers.RestoreControlEnabledState(this);
                        progressBar1.Visible = false;                        
                    }, this);

                    return true;
                });
            }
            catch (Exception ex)
            {
                blockers.RestoreControlEnabledState(this);
                progressBar1.Visible = false;
            }
            finally
            {
                dgvData.DataSource = dtData;
                //btGetData.Enabled = true;
            }
        }

        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow == null || dtData == null || dtData.Rows.Count == 0 || dtData.DefaultView.Count == 0 || dtData.DefaultView.Count <= dgvData.CurrentRow.Index)
            {
                tbDate.Text = "";
                tbFio.Text = "";
                chbInv.Checked = false;
                chbOpt.Checked = false;
                chbOtgruz.Checked = false;
                return;
            }

            DataRowView row = dtData.DefaultView[dgvData.CurrentRow.Index];

            tbDate.Text = row["DateCreate"].ToString();
            tbFio.Text = row["FIO"].ToString();
            chbInv.Checked = (bool)row["isInventorySpis"];
            chbOpt.Checked = (bool)row["isOnlyShipped"];
            chbOtgruz.Checked = (bool)row["isOptOtgruz"];
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

        private void checkBox1_Click(object sender, EventArgs e)
        {
            setFilter();
        }

        private void setFilter()
        {
            if (dtData == null || dtData.Rows.Count == 0)
            {
                //btPrint.Enabled = false;
                return;
            }

            try
            {
                string filter = "";


                if (checkBox1.Checked)
                    filter += (filter.Length == 0 ? "" : " and ") + $"isError = true";

                dtData.DefaultView.RowFilter = filter;
                dtData.DefaultView.Sort = "id asc ";
            }
            catch
            {
                dtData.DefaultView.RowFilter = "id = -1";
            }
            finally
            {
                //btPrint.Enabled =
                //dtData.DefaultView.Count != 0;
            }
        }

        private void dgvData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dtData == null || dtData.Rows.Count == 0 || dtData.DefaultView.Count == 0 || dgvData.CurrentRow == null || dgvData.CurrentRow.Index == -1) return;

            Config.id_TSaveRN = (int)dtData.DefaultView[dgvData.CurrentRow.Index]["id"];
            Config.dtDaveRN = sql.getSaveRN(Config.id_TSaveRN);
            Store store = dicListData[Config.id_TSaveRN];
            DataTable dtTovarCalculation = null;

            DataTable dtDeps = sql.getDepartments(true);

            //foreach (DataRow row in (cmbDepartments.DataSource as DataTable).Rows)
            foreach (DataRow row in dtDeps.Rows)
            {
                if ((Int16)row["id"] == -1) continue;

                Department dep = store.GetDepartment((Int16)row["id"]);
                if (dtTovarCalculation == null)
                    dtTovarCalculation = dep.GetAllGoods().Copy();
                else
                    dtTovarCalculation.Merge(dep.GetAllGoods());
            }

            new frmRnCompareTovar() { dtTovarCalculation = dtTovarCalculation, id_otdel = 0, id_group = 0 }.ShowDialog();
        }
    }
}
