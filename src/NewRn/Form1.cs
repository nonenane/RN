using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using NewRn.Data;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using Nwuram.Framework.Settings.Connection;
using Nwuram.Framework.Logging;
using Nwuram.Framework.Settings.User;
using System.Net;

namespace NewRn
{
    //главная форма
    public partial class Form1 : Form
    {

        #region Fields

        Sql sql = null;
        Sql sqlVVO = null;        
        ArrayList departments = new ArrayList(); //отделы, для которых уже посчитан РН

        #endregion

        private Store store = null;
        

        #region Constructor

        public Form1()
        {
            try
            {
                InitializeComponent();

                rBGpr1.Checked = true;

                this.cmPrint.Opening += this.cmPrint_Opening;
                Text += ", " + UserSettings.User.Status + ", " + UserSettings.User.FullUsername;
                //доступ к данным на сервере                
                sql = new Sql(ConnectionSettings.GetServer(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase(), ConnectionSettings.ProgramName);
                sqlVVO = new Sql(ConnectionSettings.GetServer("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase("2"), ConnectionSettings.ProgramName);

                toolStripStatusLabelServer.Text = ConnectionSettings.GetServer() + "-" + ConnectionSettings.GetDatabase() + "/" + ConnectionSettings.GetServer("2") + "-" + ConnectionSettings.GetDatabase("2");

                //устанавливаем даты

                DataTable dateInv = sql.getLastInvDate();
                dtpStart.Value = (DateTime)dateInv.Rows[0][0];
                dtpFinish.Value = DateTime.Today.AddDays(-1);

                //заполнение комбобокса отделов
                cmbDepartments.DataSource = sql.getDepartments();
                if (UserSettings.User.StatusCode == "МН")
                {
                    string[] split;
                    string tmpstr;
                    int id_otdel_tmp = -1;
                    DataTable bufferDataTable = sql.getListOtdelGroup(0);
                    foreach (DataRow row in bufferDataTable.Rows)
                    {
                        tmpstr = Convert.ToString(row["value"]);
                        split = tmpstr.Split(new Char[] { ';', '\t', '\n' });
                        id_otdel_tmp = -1;
                        for (int i = 0; i < split.Length; i++)
                        {
                            if (split[i] == UserSettings.User.IdDepartment.ToString())
                            {
                                id_otdel_tmp = Convert.ToInt32(split[0]);
                                break;
                            }
                        }
                        if (id_otdel_tmp != -1)
                            break;                        
                    }
                    if (id_otdel_tmp==-1)
                        cmbDepartments.SelectedValue = UserSettings.User.IdDepartment.ToString();
                    else
                        cmbDepartments.SelectedValue = id_otdel_tmp.ToString();
                    //cmbDepartments.SelectedIndex =
                    //    Convert.ToInt32(
                    //        ((DataTable) cmbDepartments.DataSource).DefaultView.ToTable().Select("id=1")[0]["id"]);
                    cmbDepartments.Enabled = false;
                }
                grdPrices.AutoGenerateColumns = false;

                //всплывающие подсказки
                toolTips.SetToolTip(btnCount, "Подсчёт РН");
                toolTips.SetToolTip(button1, "Выгрузка в Excel");
                toolTips.SetToolTip(SettingsButton, "Настройки");

                //инициализация countAllWorker
                countAllWorker.DoWork += new DoWorkEventHandler(countAllWorker_DoWork);
                countAllWorker.ProgressChanged += new ProgressChangedEventHandler(countAllWorker_ProgressChanged);
                countAllWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(countAllWorker_RunWorkerCompleted);

                //инициализация countOneWorker
                countOneWorker.DoWork += new DoWorkEventHandler(countOneWorker_DoWork);
                countOneWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(countOneWorker_RunWorkerCompleted);

                r1.Visible = chkRemains.Checked;
                r2.Visible = chkRemains.Checked;

                cbShipped.Enabled = optCheckBox.Checked;
                cmPrint.Visible = false;          
                
                Config.dtDeps = sql.getDepartments(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+ex.StackTrace);
            }
        }

        #endregion

        DataTable countResult = null;

        #region countAllWorker

        private void UpdateProgress()
        {
            countAllWorker.ReportProgress(1);
        }

        private void countAllWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                countAllWorker.WorkerReportsProgress = true;
                object[] parameters = (object[])e.Argument;
                currentDepartment = null;
                store = new Store();
                store.Notify += UpdateProgress;
                GC.Collect();
                if (rBGpr1.Checked)
                {
                    countResult = store.CountRN(sql, sqlVVO, Convert.ToDateTime(parameters[0]), Convert.ToDateTime(parameters[1]), Convert.ToBoolean(parameters[2]), Convert.ToBoolean(parameters[3]));
                }
                else
                {
                    countResult = store.CountRN_inv(sql, sqlVVO, Convert.ToDateTime(parameters[0]), Convert.ToDateTime(parameters[1]), Convert.ToBoolean(parameters[2]), Convert.ToBoolean(parameters[3]));
                }
                    GC.Collect();
            }
           catch (Exception exc)
            {
                if (Program.IsCritical(exc))
                    throw exc;
                e.Cancel = true;
            }
        }

        private void countAllWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.PerformStep();
        }

        private DataTable countResult_tmp;
        private bool mult_count = false;
        private decimal prihodAll_sum_cout,
           RealizAll_sum_cout,
           Rn_sum_cout,
           Procent_sum_cout,
           R1_sum_cout,
           R2_sum_cout;

        private bool isTUGrp, isOptCheckBox, isShipped, isWithInvSpis;

        private void countAllWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                tmpDateStart = dtpStart.Value.Date; tmpDateStop = dtpFinish.Value.Date;
                isTUGrp = rBGpr1.Checked;
                isOptCheckBox = optCheckBox.Checked;
                isShipped = cbShipped.Checked;
                isWithInvSpis = chbWithInvSpis.Checked;
                
                if (Config.isCompareData)
                    Console.WriteLine(Config.dtDaveRN.Rows.Count);

                //
                mult_count = false;
                prihodAll_sum = 0;
                RealizAll_sum = 0;
                Rn_sum = 0;
                Procent_sum = 0;
                R1_sum = 0;
                R2_sum = 0;
                DataTable checkerTable = sql.getListOtdelGroup();
                countResult_tmp = countResult;
                string tmpstr = "";
                string[] split = {""};
               
                int numkol = 0;
                foreach (DataRow row_1 in checkerTable.Rows)
                {
                    tmpstr = Convert.ToString(row_1["value"]);
                    split = tmpstr.Split(new Char[] {';', '\t', '\n'});
                    //if (Convert.ToInt32(split[0]) == Convert.ToInt32(parameters[0]))
                    //    break;

                    split_local = split;
                    stringArray[numkol] = split_local;
                    if (split.Count() > 1)
                    {
                        string nameotdel = "";
                        string idotdel = "";
                        mult_count = true;
                        DataRow firstRow = countResult.NewRow();
                        decimal prihod_all_count = 0;
                        decimal prihod_count = 0;
                        decimal otgruz_count = 0;
                        decimal vozvr_count = 0;
                        decimal spis_count = 0;
                        decimal spis_inv_count = 0;



                        decimal realiz_all_count = 0;
                        decimal realiz_count = 0;
                        decimal realiz_opt_count = 0;
                        decimal vozvkass_count = 0;
                        decimal rn_count = 0;
                        decimal procent_count = 0;
                        decimal r1_count = 0;
                        decimal r2_count = 0;
                        bool notValidate = false;
                        listID[numkol] = Convert.ToInt32(split[0]);
                        foreach (string s in split)
                        {
                            if (s.Trim() != "")
                            {

                                for (int i = 0; i < countResult_tmp.Rows.Count; i++)
                                {
                                    if (countResult_tmp.Rows[i]["id"].ToString() == s)
                                    {
                                        //Console.WriteLine(countResult_tmp.Rows[i]["id"]);
                                        nameotdel += countResult_tmp.Rows[i]["cName"].ToString() + "/";
                                        idotdel += countResult_tmp.Rows[i]["id"].ToString() + "/";
                                        //
                                        prihod_all_count += Convert.ToDecimal(countResult_tmp.Rows[i]["prihod_all"]);
                                        prihod_count += Convert.ToDecimal(countResult_tmp.Rows[i]["prihod"]);
                                        otgruz_count += Convert.ToDecimal(countResult_tmp.Rows[i]["otgruz"]);
                                        vozvr_count += Convert.ToDecimal(countResult_tmp.Rows[i]["vozvr"]);
                                        spis_count += Convert.ToDecimal(countResult_tmp.Rows[i]["spis"]);
                                        spis_inv_count += Convert.ToDecimal(countResult_tmp.Rows[i]["spis_inv"]);
                                        realiz_all_count += Convert.ToDecimal(countResult_tmp.Rows[i]["realiz_all"]);
                                        realiz_count += Convert.ToDecimal(countResult_tmp.Rows[i]["realiz"]);
                                        realiz_opt_count += Convert.ToDecimal(countResult_tmp.Rows[i]["realiz_opt"]);
                                        vozvkass_count += Convert.ToDecimal(countResult_tmp.Rows[i]["vozvkass"]);
                                        rn_count += Convert.ToDecimal(countResult_tmp.Rows[i]["rn"]);
                                        procent_count += Convert.ToDecimal(countResult_tmp.Rows[i]["procent"]);
                                        r1_count += Convert.ToDecimal(countResult_tmp.Rows[i]["r1"]);
                                        r2_count += Convert.ToDecimal(countResult_tmp.Rows[i]["r2"]);
                                        if ((bool)countResult_tmp.Rows[i]["notValidate"]) notValidate = true;

                                        //
                                        countResult_tmp.Rows[i].Delete();
                                        countResult_tmp.AcceptChanges();
                                        countResult.AcceptChanges();
                                        break;
                                    }
                                }
                            }
                        }
                        if (realiz_all_count == 0)
                            procent_count = 0;
                        else
                            procent_count = rn_count * 100 / realiz_all_count;

                        numkol++;
                        idotdel = idotdel.Remove(idotdel.Length - 1, 1);
                        nameotdel = nameotdel.Remove(nameotdel.Length - 1, 1);
                        firstRow["id"] = idotdel;
                        firstRow["cname"] = nameotdel;
                        firstRow["prihod_all"] = prihod_all_count;
                        firstRow["prihod"] = prihod_count;
                        firstRow["otgruz"] = otgruz_count;
                        firstRow["vozvr"] = vozvr_count;
                        firstRow["spis"] = spis_count;
                        firstRow["spis_inv"] = spis_inv_count;
                        firstRow["realiz_all"] = realiz_all_count;
                        firstRow["realiz"] = realiz_count;
                        firstRow["realiz_opt"] = realiz_opt_count;
                        firstRow["vozvkass"] = vozvkass_count;
                        firstRow["rn"] = rn_count;
                        firstRow["procent"] = procent_count;
                        firstRow["r1"] = r1_count;
                        firstRow["r2"] = r2_count;
                        firstRow["notValidate"] = notValidate;

                        countResult.Rows.InsertAt(firstRow, Convert.ToInt32(split[0]) - 1);
                        countResult_tmp.AcceptChanges();
                        countResult.AcceptChanges();
                        //grdPrices.DataSource = countResult_tmp;
                       // grdPrices.DataSource = countResult;
                       
                    }
                    else
                    {
                        grdPrices.DataSource = countResult;
                        SetResultSums(store.PrihodAll, store.RealizAll, store.RN, store.Procent, store.RemainStart,
                            store.RemainFinish);
                    }
                }
                //
                SetResultSums(store.PrihodAll, store.RealizAll, store.RN, store.Procent, store.RemainStart,
                           store.RemainFinish);
                grdPrices.DataSource = countResult;
               
                SetControlsEnable(true);
                if (UserSettings.User.StatusCode == "МН")
                {
                    cmbDepartments.Enabled = false;
                }


                isValidSave();
                MessageBox.Show("Подсчёт РН завершён!");
                GC.Collect();
                cmbDepartments_SelectedIndexChanged(sender, e);

                if (Config.dtListDepsVsTovarNoCorrect != null && Config.dtListDepsVsTovarNoCorrect.Rows.Count > 0)
                {
                    new frmDepsVsTovarNoCorrect().ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Соединение с сервером потеряно! Обратитесь в ОЭЭС!");
                SetControlsEnable(true);
                progress.Value = 0;
                Logging.Comment("Соединение с сервером потеряно! Обратитесь в ОЭЭС!");
                Logging.StopFirstLevel();

            }
        }

        #endregion

        #region countOneWorker

        private Department currentDepartment = null;
        private DataTable nnn ;
        private ArrayList addList = new ArrayList();
        private bool mult_sum = false;
        private int[] listID = new int[11];
        private string[] split_local;
        private string[][] stringArray = new string[11][];
        private decimal prihodAll_sum,
            RealizAll_sum,
            Rn_sum,
            Procent_sum,
            R1_sum,
            R2_sum;


        private void countOneWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                prihodAll_sum = 0;
                RealizAll_sum = 0;
                Rn_sum = 0;
                Procent_sum = 0;
                R1_sum = 0;
                R2_sum = 0;
                object[] parameters = (object[]) e.Argument;

                string[] split= { "" };
                string tmpstr;
                if (UserSettings.User.StatusCode == "МН")
                {                   
                    int id_otdel_tmp = -1;
                    DataTable bufferDataTable = sql.getListOtdelGroup(0);
                    foreach (DataRow row in bufferDataTable.Rows)
                    {
                        tmpstr = Convert.ToString(row["value"]);
                        split = tmpstr.Split(new Char[] { ';', '\t', '\n' });
                        id_otdel_tmp = -1;
                        for (int i = 0; i < split.Length; i++)
                        {
                            if (split[i] == UserSettings.User.IdDepartment.ToString())
                            {
                                id_otdel_tmp = Convert.ToInt32(split[0]);
                                break;
                            }
                        }
                        if (id_otdel_tmp != -1)
                            break;
                    }                  
                }
                else
                {
                    DataTable checkerTable = sql.getListOtdelGroup();
                    tmpstr = "";                    
                    foreach (DataRow row_1 in checkerTable.Rows)
                    {
                        tmpstr = Convert.ToString(row_1["value"]);
                        split = tmpstr.Split(new Char[] { ';', '\t', '\n' });
                        if (Convert.ToInt32(split[0]) == Convert.ToInt32(parameters[0]))
                            break;
                    }                    
                }

                addList.Clear();
                mult_sum = false;
                if (split.Count() > 1)
                {
                    if (Convert.ToInt32(split[0]) == Convert.ToInt32(parameters[0]) && split.Count() > 1)
                    {
                        listID[0] = Convert.ToInt32(split[0]);
                        nnn = new DataTable();
                        for (int i = 0; i < split.Length; i++)
                        {
                            Department dep = new Department(Convert.ToInt32(split[i]), parameters[1].ToString());
                            IDataWorker worker = sql;

                            if (dep.Id == 6)
                                worker = sqlVVO;
                            if (rBGpr1.Checked)
                            {
                                dep.CountRN(Convert.ToDateTime(parameters[2]), Convert.ToDateTime(parameters[3]),
                                    Convert.ToBoolean(parameters[4]), Convert.ToBoolean(parameters[5]), worker);
                            }
                            else
                            {
                           //     dep.CountRN_inv(Convert.ToDateTime(parameters[2]), Convert.ToDateTime(parameters[3]),
                            //        Convert.ToBoolean(parameters[4]), Convert.ToBoolean(parameters[5]), worker);

                                dep.CountRN_inv(Convert.ToDateTime(parameters[2]), Convert.ToDateTime(parameters[3]),
                                    Convert.ToBoolean(parameters[4]), Convert.ToBoolean(parameters[5]), worker);
                            }
                            currentDepartment = dep;
                            //foreach (DataRow row in currentDepartment.Groups.Rows)
                            //{
                            //    nnn.Rows.Add(row);
                            //}
                            if (i == 0)
                            {
                                if (rBGpr1.Checked)
                                {
                                    nnn = currentDepartment.Groups.Clone();
                                }
                                else
                                { nnn = currentDepartment.Groups_inv.Clone(); }
                            }
                            addList.Add(currentDepartment);
                            if (rBGpr1.Checked)
                            {
                                nnn.Merge(currentDepartment.Groups);
                            }
                            else
                            { nnn.Merge(currentDepartment.Groups_inv); }

                            prihodAll_sum += currentDepartment.PrihodAll;
                            RealizAll_sum += currentDepartment.RealizAll;
                            Rn_sum += currentDepartment.RN;
                            Procent_sum += currentDepartment.Procent;
                            R1_sum += currentDepartment.R1;
                            R2_sum += currentDepartment.R2;
                            store = null;
                            mult_sum = true;
                        }
                        if (RealizAll_sum == 0)
                            Procent_sum = 0;
                        else
                            Procent_sum = Rn_sum*100/RealizAll_sum;
                    }
                    else
                    {
                        listID[0] = 0;
                        nnn = new DataTable();
                        Department dep = new Department(Convert.ToInt32(parameters[0]), parameters[1].ToString());
                        IDataWorker worker = sql;

                        if (dep.Name == "ВВО")
                            worker = sqlVVO;
                        if (rBGpr1.Checked)
                        {
                            dep.CountRN(Convert.ToDateTime(parameters[2]), Convert.ToDateTime(parameters[3]),
                                Convert.ToBoolean(parameters[4]), Convert.ToBoolean(parameters[5]), worker);
                        }
                        else
                        {
                            dep.CountRN_inv(Convert.ToDateTime(parameters[2]), Convert.ToDateTime(parameters[3]),
                                                           Convert.ToBoolean(parameters[4]), Convert.ToBoolean(parameters[5]), worker);
                        }
                            currentDepartment = dep;
                        if (rBGpr1.Checked)
                        {
                            nnn = currentDepartment.Groups.Clone();
                            nnn.Merge(currentDepartment.Groups);
                        }
                        else
                        {
                            nnn = currentDepartment.Groups_inv.Clone();
                            nnn.Merge(currentDepartment.Groups_inv);
                        }
                        //currentDepartment.PrihodAll;
                        //currentDepartment.RealizAll;
                        //currentDepartment.RN;
                        //currentDepartment.Procent;
                        //currentDepartment.R1;
                        //currentDepartment.R2;
                        store = null;
                    }
                }
                else
                {
                    listID[0] = 0;
                    nnn = new DataTable();
                    Department dep = new Department(Convert.ToInt32(parameters[0]), parameters[1].ToString());
                    IDataWorker worker = sql;

                    if (dep.Name == "ВВО")
                        worker = sqlVVO;
                        if (rBGpr1.Checked)
                        {
                            dep.CountRN(Convert.ToDateTime(parameters[2]), Convert.ToDateTime(parameters[3]),
                                Convert.ToBoolean(parameters[4]), Convert.ToBoolean(parameters[5]), worker);
                        }
                        else
                        {
                            dep.CountRN_inv(Convert.ToDateTime(parameters[2]), Convert.ToDateTime(parameters[3]),
                                                       Convert.ToBoolean(parameters[4]), Convert.ToBoolean(parameters[5]), worker);
                        }
                        currentDepartment = dep;
                        if (rBGpr1.Checked)
                        {
                            nnn = currentDepartment.Groups.Clone();
                            nnn.Merge(currentDepartment.Groups);
                        }
                        else 
                        {
                            nnn = currentDepartment.Groups_inv.Clone();
                            nnn.Merge(currentDepartment.Groups_inv);
                        }
                        //currentDepartment.PrihodAll;
                    //currentDepartment.RealizAll;
                    //currentDepartment.RN;
                    //currentDepartment.Procent;
                    //currentDepartment.R1;
                    //currentDepartment.R2;
                    store = null;
                }
            }
            catch (Exception exc)
            {
                if (Program.IsCritical(exc))
                    throw exc;
                e.Cancel = true;
            }
        }

        private void countOneWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    tmpDateStart = dtpStart.Value.Date; tmpDateStop = dtpFinish.Value.Date;
                    
                    // DataTable nnn = currentDepartment.Groups;
                    /// nnn.Rows[3]["id"] = "34435";
                    ///grdPrices.DataSource = currentDepartment.Groups;   
                    //if(nnn.Rows.Count>0)                 
                    grdPrices.DataSource = nnn;
                   // else
                   //     grdPrices.DataSource = currentDepartment.Groups;   
                   // grdPrices.Columns["id"].CellTemplate.ValueType = typeof(string);
                   // grdPrices.Columns["id"].ValueType = typeof(string);

                  //  grdPrices.Rows[3].Cells["id"].ValueType = typeof(String);
                    //grdPrices.Rows[3].Cells["id"].Value = "test";
                    if (mult_sum)
                    {
                        SetResultSums(prihodAll_sum, RealizAll_sum, Rn_sum,
                            Procent_sum, R1_sum, R2_sum);
                    }
                    else
                    {
                        SetResultSums(currentDepartment.PrihodAll, currentDepartment.RealizAll, currentDepartment.RN, currentDepartment.Procent, currentDepartment.R1, currentDepartment.R2);
                    }
                    SetControlsEnable(true);
                    if (UserSettings.User.StatusCode == "МН")
                    {
                        cmbDepartments.Enabled = false;
                    }
                    isValidSave();
                    MessageBox.Show("Подсчёт РН завершён!");
                }
                else
                {
                    MessageBox.Show("Соединение с сервером потеряно! Обратитесь в ОЭЭС!");
                    SetControlsEnable(true);
                    progress.Value = 0;
                }
            }
            catch (Exception ex)
            {
                Logging.Comment(ex.Message);
            }
        }

        #endregion

        private void SetResultSums(decimal prihod_all, decimal realiz_all, decimal rn, decimal procent, decimal remainStart, decimal remainFinish)
        {
            txtPrihod.Text = GetFormattedString(prihod_all);
            txtSumReal.Text = GetFormattedString(realiz_all);
            txtSumRN.Text = GetFormattedString(rn);
            txtSumProcent.Text = GetFormattedString(procent);
            txtSumRemainStart.Text = GetFormattedString(remainStart);
            txtSumRemainFinish.Text = GetFormattedString(remainFinish);

            //MessageBox.Show(sql.GetDateGrn());
            //MessageBox.Show(sql.GetNumDate());
            Logging.Comment("Граничная дата округления: "+sql.GetDateGrn());
            Logging.Comment("Кол-во дней после инвентаризации: "+sql.GetNumDate());            
            Logging.Comment("Завершение операции 'Раcчёт РН'");
            Logging.Comment("Приход:" + txtPrihod.Text + ", " + "Реализация:" + txtSumReal.Text + ", " + "РН:" + txtSumRN.Text + ", " + "Процент:" + txtSumProcent.Text + ", " + "Остаток на начало:" + txtSumRemainStart.Text + ", " + "Остаток на конец:" + txtSumRemainFinish.Text);
            Logging.StopFirstLevel();
        }

        #region grdPrices Clicks

        //обработка двойного нажатия на строку в гриде
        private void grdPrices_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (SelectOneDepartment)
                {
                    DataRow selectedDataRow = ((DataRowView)cmbDepartments.SelectedItem).Row;
                    //int Id = Convert.ToInt32(selectedDataRow["Id"]);
                    int Id = Convert.ToInt32(cmbDepartments.SelectedValue);
                    int Id_otdel = Convert.ToInt32(grdPrices.CurrentRow.Cells["id_otdel"].Value);
                    if (addList.Count > 0)
                    {
                        for (int i = 0; i < addList.Count; i++)
                        {
                            Department currentDepartment_tmp = (Department)addList[i];
                            if (currentDepartment_tmp.Id == Id_otdel)
                            {
                                Id = Id_otdel;
                                currentDepartment = currentDepartment_tmp;
                                break;
                            }
                        }
                    }

                    Department selectedDepartment = null;
                    if (store != null)
//                        selectedDepartment = store.GetDepartment(cmbDepartments.Text.Trim());
                        selectedDepartment = store.GetDepartment(Id);
                    else if (currentDepartment != null && (currentDepartment.Id == Convert.ToInt32(cmbDepartments.SelectedValue) || currentDepartment.Id == Id_otdel))
                        selectedDepartment = currentDepartment;
                   
                    
                    
                    
                    if (selectedDepartment != null)
                    {
                        string id_group = Convert.ToString(grdPrices.Rows[e.RowIndex].Cells["id"].Value);
                        string groupName = grdPrices.Rows[e.RowIndex].Cells["cname"].Value.ToString();
                        

                        string[] out_Data =new string[6];
                        out_Data[0] = cmbDepartments.Text;
                        out_Data[1] = chkRemains.Checked.ToString();
                        out_Data[2] = dtpStart.Text;
                        out_Data[3] = dtpFinish.Text;
                        out_Data[4] = optCheckBox.Checked.ToString();
                        if (optCheckBox.Checked)
                        {
                            out_Data[5] = cbShipped.Checked.ToString();
                        }
                        GoodsDialog goods;
                        string[] split = id_group.Split(new Char[] { '/', '\t', '\n' });
                        if (split.Length > 1)
                        {
                            DataTable tmpforGoods = new DataTable();
                            for (int j = 0; j < split.Length; j++)
                            {
                                if (rBGpr1.Checked)
                                {
                                    tmpforGoods.Merge(selectedDepartment.GetGoods(Convert.ToInt32(split[j])));
                                }
                                else
                                {
                                    tmpforGoods.Merge(selectedDepartment.GetGoods_inv(Convert.ToInt32(split[j])));
                                }                            
                            }

                            if (Config.isCompareData && (grdPrices.DataSource as DataTable).Columns.Contains("notValidate") && (bool)(grdPrices.DataSource as DataTable).DefaultView[e.RowIndex]["notValidate"]) {
                                new frmRnCompareTovar() { dtTovarCalculation = tmpforGoods,id_otdel = Id_otdel,id_group = Convert.ToInt32(split[0]) }.ShowDialog();
                            }
                            else
                            {
                                goods = new GoodsDialog(groupName, id_group,tmpforGoods, out_Data); goods.ShowDialog();
                            }
                        }
                        else
                        {
                            if (rBGpr1.Checked)
                            {
                                if (Config.isCompareData && (grdPrices.DataSource as DataTable).Columns.Contains("notValidate") && (bool)(grdPrices.DataSource as DataTable).DefaultView[e.RowIndex]["notValidate"])
                                {
                                    new frmRnCompareTovar() { dtTovarCalculation = selectedDepartment.GetGoods(Convert.ToInt32(split[0])), id_otdel = Id_otdel, id_group = Convert.ToInt32(split[0]) }.ShowDialog();
                                }
                                else
                                {
                                    goods = new GoodsDialog(groupName, id_group, selectedDepartment.GetGoods(Convert.ToInt32(split[0])), out_Data); goods.ShowDialog();
                                }
                            }
                            else
                            {
                                if (Config.isCompareData && (grdPrices.DataSource as DataTable).Columns.Contains("notValidate") && (bool)(grdPrices.DataSource as DataTable).DefaultView[e.RowIndex]["notValidate"])
                                {
                                    new frmRnCompareTovar() { dtTovarCalculation = selectedDepartment.GetGoods_inv(Convert.ToInt32(split[0])), id_otdel = Id_otdel, id_group = Convert.ToInt32(split[0]) }.ShowDialog();
                                }
                                else
                                {
                                    goods = new GoodsDialog(groupName, id_group, selectedDepartment.GetGoods_inv(Convert.ToInt32(split[0])), out_Data); goods.ShowDialog();
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Check Date Values

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            //проверка на корректность
            if (dtpStart.Value > dtpFinish.Value)
                dtpStart.Value = dtpFinish.Value;
            if (dtpStart.Value > DateTime.Today.AddDays(-1))
                dtpStart.Value = DateTime.Today.AddDays(-1);
        }

        private void dtpFinish_ValueChanged(object sender, EventArgs e)
        {
            //проверка на корректность
            if (dtpFinish.Value < dtpStart.Value)
                dtpFinish.Value = dtpStart.Value;
            if (dtpFinish.Value > DateTime.Today.AddDays(-1))
                dtpFinish.Value = DateTime.Today.AddDays(-1);
        }

        #endregion

        #region cmbDepartments_SelectedIndexChanged

        private void SetEmptyGrid()
        {
            grdPrices.DataSource = null;
            txtPrihod.Text = "";
            txtSumReal.Text = "";
            txtSumRN.Text = "";
            txtSumProcent.Text = "";
            txtSumRemainStart.Text = "";
            txtSumRemainFinish.Text = "";
        }
        Department selectedDepartment = null;
        //обработка выбора отдела
        public void cmbDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectDepartment();
        }

        private void selectDepartment()
        {

            if (SelectOneDepartment)
            {
                DataRow selectedDataRow = ((DataRowView)cmbDepartments.SelectedItem).Row;
                int Id = Convert.ToInt32(selectedDataRow["Id"]);
                Console.WriteLine(Id);


                selectedDepartment = null;
                if (store != null)
                    //selectedDepartment = store.GetDepartment(cmbDepartments.Text.Trim());
                    selectedDepartment = store.GetDepartment(Id);
                else if (currentDepartment != null && currentDepartment.Id == Convert.ToInt32(cmbDepartments.SelectedValue))
                    selectedDepartment = currentDepartment;
                if (selectedDepartment != null)
                {
                    int listIDInt = -10;
                    int index = -11;
                    for (int i = 0; i < listID.Length; i++)
                    {
                        if (listID[i] == Convert.ToInt32(cmbDepartments.SelectedValue))
                        {
                            listIDInt = listID[i];
                            index = i;
                            break;
                        }
                    }
                    if (Convert.ToInt32(cmbDepartments.SelectedValue) == listIDInt)
                    {
                        nnn = null;// new DataTable();
                        addList.Clear();
                        prihodAll_sum = 0;
                        RealizAll_sum = 0;
                        Rn_sum = 0;
                        Procent_sum = 0;
                        R1_sum = 0;
                        R2_sum = 0;
                        for (int i = 0; i < stringArray[index].Length; i++)
                        {
                            selectedDepartment = store.GetDepartment(Convert.ToInt32(stringArray[index][i]));
                            addList.Add(selectedDepartment);
                            prihodAll_sum += selectedDepartment.PrihodAll;
                            RealizAll_sum += selectedDepartment.RealizAll;
                            Rn_sum += selectedDepartment.RN;
                            Procent_sum += selectedDepartment.Procent;
                            R1_sum += selectedDepartment.R1;
                            R2_sum += selectedDepartment.R2;
                            if (rBGpr1.Checked)
                            {
                                if (nnn == null)
                                    nnn = selectedDepartment.Groups.Copy();
                                else
                                    nnn.Merge(selectedDepartment.Groups);
                            }
                            else
                            {
                                if (nnn == null)
                                    nnn = selectedDepartment.Groups_inv.Copy();
                                else nnn.Merge(selectedDepartment.Groups_inv); 
                            }
                        }
                        if (RealizAll_sum == 0)
                            Procent_sum = 0;
                        else
                        {
                            Procent_sum = Rn_sum * 100 / RealizAll_sum;
                        }
                        grdPrices.DataSource = nnn;
                        SetResultSums(prihodAll_sum, RealizAll_sum, Rn_sum,
                            Procent_sum, R1_sum, R2_sum);
                        //SetEmptyGrid();
                    }
                    else
                    {
                        if (rBGpr1.Checked)
                        {
                    
                            grdPrices.DataSource = selectedDepartment.Groups;
                        }
                        else
                        {
                            grdPrices.DataSource = selectedDepartment.Groups_inv;
                        }
                        txtPrihod.Text = GetFormattedString(selectedDepartment.PrihodAll);
                        txtSumReal.Text = GetFormattedString(selectedDepartment.RealizAll);
                        txtSumRN.Text = GetFormattedString(selectedDepartment.RN);
                        txtSumProcent.Text = GetFormattedString(selectedDepartment.Procent);
                        txtSumRemainStart.Text = GetFormattedString(selectedDepartment.R1);
                        txtSumRemainFinish.Text = GetFormattedString(selectedDepartment.R2);
                    }
                }
                else
                {
                    int listIDInt = -10;
                    for (int i = 0; i < listID.Length; i++)
                    {
                        if (listID[i] == Id)
                        {
                            listIDInt = listID[i];
                            break;
                        }
                    }

                    if (Id == listIDInt)
                    {
                        if (mult_sum)
                        {
                            grdPrices.DataSource = nnn;
                            SetResultSums(prihodAll_sum, RealizAll_sum, Rn_sum,
                                Procent_sum, R1_sum, R2_sum);
                        }
                    }
                    else
                        SetEmptyGrid();
                }
            }
            else
            {

                if (store != null)
                {
                    grdPrices.DataSource = countResult;
                    txtPrihod.Text = GetFormattedString(store.PrihodAll);
                    txtSumReal.Text = GetFormattedString(store.RealizAll);
                    txtSumRN.Text = GetFormattedString(store.RN);
                    txtSumProcent.Text = GetFormattedString(store.Procent);
                    txtSumRemainStart.Text = GetFormattedString(store.RemainStart);
                    txtSumRemainFinish.Text = GetFormattedString(store.RemainFinish);
                }
                else
                    SetEmptyGrid();
            }
        }

        #endregion

        #region enable/disable controls

        private void SetControlsEnable(bool enabled)
        {
            rBGpr1.Enabled = enabled;
            rBGrp2.Enabled = enabled;

            cmbDepartments.Enabled = enabled;
            dtpStart.Enabled = enabled;
            dtpFinish.Enabled = enabled;
            grdPrices.Enabled = enabled;
            btnCount.Enabled = enabled;
            button1.Enabled = enabled;
            txtPrihod.Enabled = enabled;
            txtSumReal.Enabled = enabled;
            txtSumRN.Enabled = enabled;
            txtSumProcent.Enabled = enabled;
            txtSumRemainStart.Enabled = enabled;
            txtSumRemainFinish.Enabled = enabled;
            optCheckBox.Enabled = enabled;
            chkRemains.Enabled = enabled;
            chbWithInvSpis.Enabled = enabled;

            SettingsButton.Enabled = enabled;
            cbShipped.Enabled = enabled;

            if (enabled)
                cbShipped.Enabled = optCheckBox.Checked;

            btCalc.Enabled = enabled;
            btSave.Enabled = false;
        }

        #endregion

        #region Count

        private string GetFormattedString(decimal value)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalSeparator = ",";
            return value.ToString("n", nfi);
        }

        private bool SelectOneDepartment
        {
            get
            {                
                return Convert.ToInt32(cmbDepartments.SelectedValue) != -1;
            }
        }

        private DateTime tmpDateStart, tmpDateStop;
      

        private void btnCount_Click(object sender, EventArgs e)
        {
            Config.isCompareData = false;
            Config.id_TSaveRN = 0;
            if (!SelectOneDepartment)
            {
                DataTable dtResult = sql.validateTSaveRN(dtpStart.Value.Date, dtpFinish.Value.Date, optCheckBox.Checked, cbShipped.Checked, chbWithInvSpis.Checked);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    MyMessageBox.MyMessageBox msgBox = new MyMessageBox.MyMessageBox("В базе данных присутствуют данные за выбранный для расчёта период.\nВыберите операцию:\n", "Расчёт РН", MyMessageBox.MessageBoxButtons.YesNoCancel, new List<string>(new string[] { "Произвести расчёт", "Расчёт со сравнением", "Отмена" }));

                    //msgBox.Size = new Size(msgBox.Size.Width + 200, msgBox.Size.Height+200);

                    DialogResult dlgResult = msgBox.ShowDialog();
                    if (dlgResult == DialogResult.Cancel) return;
                    if (dlgResult == DialogResult.No)
                    {
                        Config.isCompareData = true;
                        Config.id_TSaveRN = (int)dtResult.Rows[0]["id"];
                    }
                }
                else
                {
                    if (MessageBox.Show("Посчитать РН?", "РН", MessageBoxButtons.YesNo) == DialogResult.No) return;
                }
            }
            else
            {
                if (MessageBox.Show("Посчитать РН?", "РН", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }

            Config.isInventSpis = chbWithInvSpis.Checked;

            GC.Collect();
            Logging.StartFirstLevel(485);
            Logging.Comment("Расчет РН");
            try
            {
                SetControlsEnable(false);
                progress.Minimum = 0;
                progress.Value = 0;
                if (!SelectOneDepartment)
                {
                    DataTable departments = sql.getDepartments();
                    if (departments != null)
                    {
                        progress.Maximum = departments.Rows.Count - 1;
                        progress.Step = 1;
                        countAllWorker.RunWorkerAsync(new object[] { dtpStart.Value, dtpFinish.Value, optCheckBox.Checked, cbShipped.Checked });
                    }
                    else
                    {
                        MessageBox.Show("Соединение с сервером потеряно! Обратитесь в ОЭЭС!");
                        SetControlsEnable(true);
                        Logging.Comment("Соединение с сервером потеряно! Обратитесь в ОЭЭС!");
                        Logging.StopFirstLevel();
                    }
                }
                else
                    countOneWorker.RunWorkerAsync(new object[] { cmbDepartments.SelectedValue, cmbDepartments.Text, dtpStart.Value, dtpFinish.Value, optCheckBox.Checked, cbShipped.Checked });

                Logging.Comment("Отдел: " + cmbDepartments.Text + "(id:" + cmbDepartments.SelectedIndex + ")");
                Logging.Comment("Период с: " + dtpStart.Text + ", по: " + dtpFinish.Text);
                Logging.Comment("Учитывать оптовые отгрузки:" + optCheckBox.Checked.ToString());
                if (optCheckBox.Checked)
                {
                    Logging.Comment("только отгрузки:" + cbShipped.Checked.ToString());
                }
                Logging.Comment("Пользователь: " + UserSettings.User.Id);
            }
            catch (Exception ex)
            {
                Logging.Comment(ex.Message);
            }

        }

        #endregion

        #region Print

        private DataTable GetTableForPrint(DataTable prices)
        {
            DataTable result = prices.Copy();
            result.Columns.Remove("id");
            try
            {
                result.Columns.Remove("id_otdel");
            }
            catch (Exception) { }
            if (!chkRemains.Checked)
                try
                {
                    result.Columns.Remove("r1");
                    result.Columns.Remove("r2");
                }
                catch (Exception) { }
            return result;
        }

        private void toExcel(DataTable prices, string[] valueNames, ArrayList values, string templateFilename)
        {
            if (prices != null)
            {
                FillExcelTemplate fet = new FillExcelTemplate();

                for (int i = 0; i < valueNames.Length; ++i)
                    fet.AddItemSingleList(valueNames[i], values[i].ToString());
                fet.AddMultiList(prices, "rn");

                //выгружаем в Excel
                try
                {
                    if (!fet.CreateTemplate(Application.StartupPath + "\\templates\\" + templateFilename + ".xls", Application.StartupPath + "\\rn.xls", null))
                    {
                        MessageBox.Show(fet.MessageError + " Не удалось выгрузить в Excel");
                        return;
                    }
                    else
                        MessageBox.Show("Выгрузка в Excel прошла успешно!");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo(Application.StartupPath + "\\rn.xls");
                    Process ps = Process.Start(psi);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка просмотра в Excel!");
                    Logger.NewMessage(exc.Message);
                }
            }
            else
                MessageBox.Show("Нет данных для выгрузки в Excel!");
        }

        private void toExcel3(DataTable prices, ArrayList values)
        {
            if (prices != null)
            {
                FillExcelTemplate fet = new FillExcelTemplate();

                fet.AddItemSingleList("otdel", values[0].ToString());
                fet.AddItemSingleList("datestart", values[1].ToString());
                fet.AddItemSingleList("datefinish", values[2].ToString());
                fet.AddMultiList(GetTableForPrint(prices), "rn");
                fet.AddItemSingleList("{sumprihod_all}", values[3].ToString());
                fet.AddItemSingleList("{sumrealiz_all}", values[4].ToString());
                fet.AddItemSingleList("{sumrn}", values[5].ToString());
                fet.AddItemSingleList("{sumprocent}", values[6].ToString());
                fet.AddItemSingleList("{sumprihod}", values[7].ToString());
                fet.AddItemSingleList("{sumotgruz}", values[8].ToString());
                fet.AddItemSingleList("{sumvozvr}", values[9].ToString());
                fet.AddItemSingleList("{sumspis}", values[10].ToString());
                fet.AddItemSingleList("{sumspis_inv}", values[11].ToString());
                fet.AddItemSingleList("{sumrealiz}", values[12].ToString());
                fet.AddItemSingleList("{sumrealiz_opt}", values[13].ToString());
                fet.AddItemSingleList("{sumvozvkass}", values[14].ToString());
                //выгружаем в Excel
                try
                {
                    if (!fet.CreateTemplate(Application.StartupPath + "\\templates\\rnShablon.xls", Application.StartupPath + "\\rn.xls", null))
                    {
                        MessageBox.Show(fet.MessageError + " Не удалось выгрузить в Excel");
                        return;
                    }
                    else
                        MessageBox.Show("Выгрузка в Excel прошла успешно!");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo(Application.StartupPath + "\\rn.xls");
                    Process ps = Process.Start(psi); //открываем файл bills.xls
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка просмотра в Excel!");
                    Logger.NewMessage(exc.Message);
                }
            }
            else
                MessageBox.Show("Нет данных для выгрузки в Excel!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((DataTable)grdPrices.DataSource != null)
            {
                Logging.StartFirstLevel(486);
                Logging.Comment("Выгрузка РН в Excel");
                Logging.Comment("Отдел: " + cmbDepartments.Text);
                Logging.Comment("Вывод остатков: " + chkRemains.Checked.ToString());
                Logging.Comment("Период с: " + dtpStart.Text + ", по: " + dtpFinish.Text);
                Logging.Comment("Учитывать оптовые отгрузки:" + optCheckBox.Checked.ToString());
                if (optCheckBox.Checked)
                {
                    Logging.Comment("только отгруженные:" + cbShipped.Checked.ToString());
                }
                Logging.Comment("Приход:" + txtPrihod.Text + ", " + "Реализация:" + txtSumReal.Text + ", " + "РН:" + txtSumRN.Text + ", " + "Процент:" + txtSumProcent.Text + ", " + "Остаток на начало:" + txtSumRemainStart.Text + ", " + "Остаток на конец:" + txtSumRemainFinish.Text);
                try
                {
                    ArrayList values = new ArrayList();
                    values.Add(cmbDepartments.Text);
                    values.Add(dtpStart.Value.ToShortDateString());
                    values.Add(dtpFinish.Value.ToShortDateString());
                    values.Add(DateTime.Now.ToString());
                    if (chkRemains.Checked)
                    {
                        values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "r1"));
                        values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "r2"));
                    }
                    values.Add(txtPrihod.Text);
                    values.Add(txtSumReal.Text);
                    values.Add(txtSumRN.Text);
                    values.Add(txtSumProcent.Text);
                    values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "prihod"));
                    values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "otgruz"));
                    values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "vozvr"));
                    values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "spis"));
                    values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "spis_inv"));
                    values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "realiz"));
                    values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "realiz_opt"));
                    values.Add(GetColumnSum((DataTable)grdPrices.DataSource, "vozvkass"));

                    if (chkRemains.Checked)
                        toExcel(GetTableForPrint((DataTable)grdPrices.DataSource),
                                             new string[] { "otdel", "datestart", "datefinish","today",  
                                                        "{sumr1}", "{sumr2}", 
                                                        "{sumprihod_all}", "{sumrealiz_all}",
                                                        "{sumrn}", "{sumprocent}", 
                                                        "{sumprihod}", "{sumotgruz}", 
                                                        "{sumvozvr}", "{sumspis}", 
                                                        "{sumspis_inv}", "{sumrealiz}", 
                                                        "{sumrealiz_opt}", "{sumvozvkass}" },
                                             values, "rnShablon");
                    else
                        toExcel(GetTableForPrint((DataTable)grdPrices.DataSource),
                         new string[] { "otdel", "datestart", "datefinish","today", 
                                                        "{sumprihod_all}", "{sumrealiz_all}",
                                                        "{sumrn}", "{sumprocent}", 
                                                        "{sumprihod}", "{sumotgruz}", 
                                                        "{sumvozvr}", "{sumspis}", 
                                                        "{sumspis_inv}", "{sumrealiz}", 
                                                        "{sumrealiz_opt}", "{sumvozvkass}" },
                         values, "rnShablonbo");

                }
                catch (Exception ex)
                {
                    Logging.Comment(ex.Message);
                }
                Logging.Comment("Завершение операции 'Выгрузка РН в Excel'");
                Logging.StopFirstLevel();
            }
            else
                MessageBox.Show("Нет данных для выгрузки!");
        }

        private string GetColumnSum(DataTable prices, string columnName)
        {
            return Convert.ToDecimal(prices.Compute("sum(" + columnName + ")", "")).ToString();
        }

        private void toExcel2(DataTable prices, ArrayList values)
        {
            if (prices != null)
            {
                FillExcelTemplate fet = new FillExcelTemplate();

                fet.AddItemSingleList("otdel", values[0].ToString());
                fet.AddItemSingleList("datestart", values[1].ToString());
                fet.AddItemSingleList("datefinish", values[2].ToString());
                fet.AddMultiList(prices, "rn");
                fet.AddItemSingleList("{sumprihod}", values[3].ToString());
                fet.AddItemSingleList("{sumrealiz}", values[4].ToString());
                fet.AddItemSingleList("{sumrn}", values[5].ToString());
                fet.AddItemSingleList("{sumprocent}", values[6].ToString());
                //выгружаем в Excel
                try
                {
                    if (!fet.CreateTemplate(Application.StartupPath + "\\templates\\rnGoodsShablon.xls", Application.StartupPath + "\\rn.xls", null))
                    {
                        MessageBox.Show(fet.MessageError + " Не удалось выгрузить в Excel");
                        return;
                    }
                    else
                        MessageBox.Show("Выгрузка в Excel прошла успешно!");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo(Application.StartupPath + "\\rn.xls");
                    Process ps = Process.Start(psi); //открываем файл bills.xls
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка просмотра в Excel!");
                    Logger.NewMessage(exc.Message);
                }
            }
            else
                MessageBox.Show("Нет данных для выгрузки в Excel!");
        }

        private void print_Click(object sender, EventArgs e)
        {
            if (SelectOneDepartment)
            {
                DataTable goods = new DataTable();
                //DataTable goods = selectedDepartment.GetGoods(Convert.ToInt32(grdPrices.CurrentRow.Cells["id"].Value));
                string[] split = Convert.ToString(grdPrices.CurrentRow.Cells["id"].Value).Split(new Char[] { '/', '\t', '\n' });
                if (split.Length > 1)
                {
                    goods = new DataTable();
                    for (int j = 0; j < split.Length; j++)
                    {
                        if (rBGpr1.Checked)
                        {
                            goods.Merge(selectedDepartment.GetGoods(Convert.ToInt32(split[j])));
                        }
                        else
                        { goods.Merge(selectedDepartment.GetGoods_inv(Convert.ToInt32(split[j]))); }
                    }                    
                }
                else
                {
                    if (rBGpr1.Checked)
                    {
                        goods = selectedDepartment.GetGoods(Convert.ToInt32(split[0]));
                    }
                    else
                    {
                        goods = selectedDepartment.GetGoods_inv(Convert.ToInt32(split[0]));
                    }
                }


                goods.Columns.Remove("prihod");
                goods.Columns.Remove("otgruz");
                goods.Columns.Remove("vozvr");
                goods.Columns.Remove("spis");
                goods.Columns.Remove("spis_inv");
                goods.Columns.Remove("r1");
                goods.Columns.Remove("r2");
                goods.Columns.Remove("vozvkass");
                goods.Columns.Remove("realiz");
                goods.Columns.Remove("realiz_opt");
                goods.Columns[goods.Columns.IndexOf("ean")].ColumnName = "id";
                goods.Columns[goods.Columns.IndexOf("prihod_all")].ColumnName = "prihod";
                goods.Columns[goods.Columns.IndexOf("realiz_all")].ColumnName = "realiz";
                goods.Columns.Add(new DataColumn("procent", typeof(decimal)));
                foreach (DataRow row in goods.Rows)
                {
                    if (Convert.ToDecimal(row["realiz"]) != 0)
                        row["procent"] = Convert.ToDecimal(row["rn"]) * 100 / Convert.ToDecimal(row["realiz"]);
                    else
                        row["procent"] = 0;
                }

                ArrayList values = new ArrayList();
                values.Add(grdPrices.CurrentRow.Cells["cname"].Value);
                values.Add(dtpStart.Value.ToShortDateString());
                values.Add(dtpFinish.Value.ToShortDateString());
                values.Add(grdPrices.CurrentRow.Cells["prihod_all"].Value);
                values.Add(grdPrices.CurrentRow.Cells["realiz_all"].Value);
                values.Add(grdPrices.CurrentRow.Cells["rn"].Value);
                values.Add(grdPrices.CurrentRow.Cells["proc"].Value);

                toExcel(goods, new string[] { "otdel", "datestart", "datefinish", "{sumprihod}", "{sumrealiz}", "{sumrn}", "{sumprocent}" }, values, "rnGoodsShablon");
            }
        }

        private void grdPrices_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            
            if (e.RowIndex != -1 && countResult != null && grdPrices.DataSource!=null && (grdPrices.DataSource as DataTable).DefaultView.Count != 0)
            {

                Color rColor = Color.White;
                if (Config.isCompareData && (grdPrices.DataSource as DataTable).Columns.Contains("notValidate") && (bool)(grdPrices.DataSource as DataTable).DefaultView[e.RowIndex]["notValidate"])
                    rColor = pLegend.BackColor;

                grdPrices.Rows[e.RowIndex].DefaultCellStyle.BackColor = rColor;
                grdPrices.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = rColor;

                grdPrices.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;


                grdPrices.Rows[e.RowIndex].Cells[id.Index].Style.BackColor =
                         grdPrices.Rows[e.RowIndex].Cells[id.Index].Style.SelectionBackColor = Color.FromArgb(224, 224, 224);

                grdPrices.Rows[e.RowIndex].Cells[cname.Index].Style.BackColor =
                         grdPrices.Rows[e.RowIndex].Cells[cname.Index].Style.SelectionBackColor = Color.FromArgb(224, 224, 224);
            }
        }

        private void grdPrices_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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

        #endregion

        #region Exit

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("       Выйти из программы?       ", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        #endregion

        private void btCalc_Click(object sender, EventArgs e)
        {
            new frmRNCompare() { dateStart = dtpStart.Value.Date,dateStop =  dtpFinish.Value.Date }.ShowDialog();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (store == null) { MessageBox.Show("Ну вот нечего записывать в базу!", "Информирование",MessageBoxButtons.OK,MessageBoxIcon.Information); return; }

            DataTable dtResult;
            DateTime DateStart = tmpDateStart.Date;
            DateTime DateEnd = tmpDateStop.Date;
            
            bool isOptOtgruz = isOptCheckBox;
            bool isOnlyShipped = isShipped;
            bool isInventorySpis = isWithInvSpis;

            decimal TotalPrihod = store.PrihodAll;
            decimal TotalRealiz = store.RealizAll;
            decimal TotalRestStart = store.RemainStart;
            decimal TotalRestStop = store.RemainFinish;
            decimal TotalRN = store.RN;
            decimal TotalPercentRN = store.Procent;

            //Тут сохраняем заголовок

            

           

            dtResult = sql.validateTSaveRN(DateStart, DateEnd, isOptOtgruz, isOnlyShipped, isInventorySpis);
            int id_TSaveRN = (int)dtResult.Rows[0]["id"];
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                MyMessageBox.MyMessageBox msgBox = new MyMessageBox.MyMessageBox("В базе данных присутствуют данные\nза расчитанный период.\nСохранить новый расчёт?\n", "Сохранение расчёта РН", MyMessageBox.MessageBoxButtons.YesNo, new List<string>(new string[] { "Да", "Нет" }));
             
                if (msgBox.ShowDialog() == DialogResult.No) return;

                dtResult = sql.setSaveRN(id_TSaveRN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);
            }

            dtResult = sql.setTSaveRN(DateStart, DateEnd, isOptOtgruz, isOnlyShipped, isInventorySpis, TotalPrihod, TotalRealiz, TotalRestStart, TotalRestStop, TotalRN, TotalPercentRN);

            if (dtResult == null || dtResult.Rows.Count == 0 || (int)dtResult.Rows[0]["id"] < 0)
            {
                MessageBox.Show("Ошибка сохранения заголовка!", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            id_TSaveRN = (int)dtResult.Rows[0]["id"];

            DataTable dtDeps = sql.getDepartments(true);

            //foreach (DataRow row in (cmbDepartments.DataSource as DataTable).Rows)
            foreach (DataRow row in dtDeps.Rows)
            {
                if ((Int16)row["id"] == -1) continue;

                Department dep = store.GetDepartment((Int16)row["id"]);

                foreach (DataRow rowGoods in dep.GetAllGoods().Rows)
                {
                    int id_tovar = int.Parse(rowGoods["id"].ToString());
                    int id_grp1 = int.Parse(rowGoods["id_grp1"].ToString());
                    int id_grp2 = int.Parse(rowGoods["id_grp2"].ToString());
                    int id_otdel = (int)rowGoods["id_otdel"];
                    decimal RestStart = decimal.Parse( rowGoods["r1"].ToString());
                    decimal RestStop = decimal.Parse(rowGoods["r2"].ToString());

                    decimal PrihodSum = decimal.Parse(rowGoods["prihod"].ToString());
                    decimal OtgruzSum = decimal.Parse(rowGoods["otgruz"].ToString());
                    decimal VozvrSum = decimal.Parse(rowGoods["vozvr"].ToString());
                    decimal SpisSum = decimal.Parse(rowGoods["spis"].ToString());
                    decimal InventSpisSum = decimal.Parse(rowGoods["spis_inv"].ToString());
                    decimal RealizSum = decimal.Parse(rowGoods["realiz"].ToString());
                    decimal OtgruzOptSum = decimal.Parse(rowGoods["realiz_opt"].ToString());
                    decimal VozvrKassSum = decimal.Parse(rowGoods["vozvkass"].ToString());
                    //Тут сохраняем тело

                    if (id_otdel != 6)
                        dtResult = sql.getTovarDataToSaveRN(id_tovar, DateStart, DateEnd);
                    else
                        dtResult = sqlVVO.getTovarDataToSaveRN(id_tovar, DateStart, DateEnd);
                    

                    decimal RestStartSum = 0;
                    decimal RestStopSum = 0;
                    decimal Prihod = 0;
                    decimal Otgruz = 0;
                    decimal Vozvr = 0;
                    decimal Spis = 0;
                    decimal VozvrKass = 0;
                    decimal Realiz = 0;
                    decimal InventSpis = 0;
                    decimal OtgruzOpt = 0;

                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        RestStartSum = (decimal)dtResult.Rows[0]["RestStartSum"];
                        RestStopSum = (decimal)dtResult.Rows[0]["RestStopSum"];
                        Prihod = (decimal)dtResult.Rows[0]["Prihod"];
                        Otgruz = (decimal)dtResult.Rows[0]["Otgruz"];
                        Vozvr = (decimal)dtResult.Rows[0]["Vozvr"];
                        Spis = (decimal)dtResult.Rows[0]["Spis"];
                        VozvrKass = (decimal)dtResult.Rows[0]["VozvrKass"];
                        Realiz = (decimal)dtResult.Rows[0]["Realiz"];
                        InventSpis = (decimal)dtResult.Rows[0]["InventSpis"];
                        OtgruzOpt = (decimal)dtResult.Rows[0]["OtgruzOpt"];
                    }

                    dtResult = sql.setSaveRN(id_TSaveRN, id_tovar, id_otdel, id_grp1, id_grp2, RestStart, RestStartSum, RestStop, RestStopSum, Prihod, PrihodSum, Otgruz, OtgruzSum, Vozvr, VozvrSum, Spis, SpisSum, InventSpis, InventSpisSum, Realiz, RealizSum, OtgruzOpt, OtgruzOptSum, VozvrKass, VozvrKassSum, false);

                }                
            }
            MessageBox.Show("Данные сохранены", "Сохранить данные", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chkRemains_CheckedChanged(object sender, EventArgs e)
        {
            r1.Visible = chkRemains.Checked;
            r2.Visible = chkRemains.Checked;
            //lblRemainStart.Visible = chkRemains.Checked;
            //lblRemainFinish.Visible = chkRemains.Checked;
            //txtSumRemainStart.Visible = chkRemains.Checked;
            //txtSumRemainFinish.Visible = chkRemains.Checked;
        }

        private void optCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            cbShipped.Enabled = optCheckBox.Checked;
        }

        private void grdPrices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmPrint_Opening(object sender, CancelEventArgs e)
        {
            if (!SelectOneDepartment || (grdPrices.SelectedCells.Count == 0 || grdPrices.RowCount == 0))
            {
                e.Cancel = true;
                return;
            }
            if (SelectOneDepartment)
            {
                DataRow selectedDataRow = ((DataRowView)cmbDepartments.SelectedItem).Row;
                //int Id = Convert.ToInt32(selectedDataRow["Id"]);
                int Id = Convert.ToInt32(cmbDepartments.SelectedValue);
                int Id_otdel = Convert.ToInt32(grdPrices.CurrentRow.Cells["id_otdel"].Value);
                if (addList.Count > 0)
                {
                    for (int i = 0; i < addList.Count; i++)
                    {
                        Department currentDepartment_tmp = (Department) addList[i];
                        if (currentDepartment_tmp.Id == Id_otdel)
                        {
                            Id = Id_otdel;
                            currentDepartment = currentDepartment_tmp;
                            break;
                        }
                    }
                }
                selectedDepartment = null;
                if (store != null)
                    //selectedDepartment = store.GetDepartment(cmbDepartments.Text.Trim());
                    selectedDepartment = store.GetDepartment(Id);
                else if (currentDepartment != null && (currentDepartment.Id == Convert.ToInt32(cmbDepartments.SelectedValue) || currentDepartment.Id==Id_otdel))
                    selectedDepartment = currentDepartment;
            }
            
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            if (UserSettings.User.StatusCode == "МН")
            {
                GroupOtdel gptO = new GroupOtdel();
                gptO.ShowDialog();
            }
            else
            {
                SettingsForm stF = new SettingsForm();
                stF.ShowDialog();
                cmbDepartments.DataSource = sql.getDepartments_refresh();
            }

            if (UserSettings.User.StatusCode == "МН")
            {
                string[] split;
                string tmpstr;
                int id_otdel_tmp = -1;
                DataTable bufferDataTable = sql.getListOtdelGroup(0);
                foreach (DataRow row in bufferDataTable.Rows)
                {
                    tmpstr = Convert.ToString(row["value"]);
                    split = tmpstr.Split(new Char[] { ';', '\t', '\n' });
                    id_otdel_tmp = -1;
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] == UserSettings.User.IdDepartment.ToString())
                        {
                            id_otdel_tmp = Convert.ToInt32(split[0]);
                            break;
                        }
                    }
                    if (id_otdel_tmp != -1)
                        break;
                }
                if (id_otdel_tmp == -1)
                    cmbDepartments.SelectedValue = UserSettings.User.IdDepartment.ToString();
                else
                    cmbDepartments.SelectedValue = id_otdel_tmp.ToString();
                //cmbDepartments.SelectedIndex =
                //    Convert.ToInt32(
                //        ((DataTable) cmbDepartments.DataSource).DefaultView.ToTable().Select("id=1")[0]["id"]);
                cmbDepartments.Enabled = false;
            }
        }

        bool userWantToChangeToInv = true;

        private void CheckRB(bool fromTUToInv)
        {
            if (grdPrices.DataSource != null)
            {
                if (MessageBox.Show(" Сменить группу ?", "Запрос", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    grdPrices.DataSource = null;
                    rBGrp2.Checked = fromTUToInv;
                    rBGpr1.Checked = !fromTUToInv;
                }
                else
                {
                    userWantToChangeToInv = false;
                    rBGpr1.Checked = fromTUToInv;
                    rBGrp2.Checked = !fromTUToInv;
                }
            }
        }

        private void rBGpr1_Click(object sender, EventArgs e)
        {
            CheckRB(false);
        }

        private void rBGrp2_Click(object sender, EventArgs e)
        {
            CheckRB(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = ConnectionSettings.ProgramName + ", " + Nwuram.Framework.Settings.User.UserSettings.User.FullUsername;
            btCalc.Visible = btSave.Visible = new List<string>(new string[] { "БГЛ" }).Contains(UserSettings.User.StatusCode);
        }

        private void isValidSave()
        {
            btSave.Enabled = false;

            if (tmpDateStart.Year == tmpDateStop.Year
                && tmpDateStart.Month == tmpDateStop.Month
                && tmpDateStart.Day == 1
                && (new DateTime(tmpDateStop.Year, tmpDateStop.Month, 1)).AddMonths(1).AddDays(-1).Day == tmpDateStop.Day
                && !SelectOneDepartment)
                btSave.Enabled = true;
            else
            {
                DataTable dtResult = sql.getInventPeriod(tmpDateStart.Date, tmpDateStop.Date);
                if (dtResult == null || dtResult.Rows.Count == 0) { btSave.Enabled = false; return; }
                btSave.Enabled = (int)dtResult.Rows[0]["id"] == 1 && !SelectOneDepartment;
            }

        }
    }
}
