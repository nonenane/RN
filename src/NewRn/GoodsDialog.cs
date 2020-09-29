using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;
using Nwuram.Framework.Logging;
using Nwuram.Framework.ToExcel;
using System.IO;
using Nwuram.Framework.Settings.User;
using System.Diagnostics;

namespace NewRn
{
    //форма товаров
    public partial class GoodsDialog : Form
    {
        private DataTable pricesTable;
        private string groupName;
        string[] mainformData;
        string groupId;
        string stFilter = null;

        //товары по группе groupName с ценами prices
        public GoodsDialog(string in_groupName, string in_groupId, DataTable in_pricesTable, string[] in_mainformdata)
        {
            InitializeComponent();


            pricesTable = in_pricesTable;
            groupName = in_groupName;
            mainformData = in_mainformdata;
            this.Text += " " + groupName;
            groupId = in_groupId;
            grdPrices.AutoGenerateColumns = false;
            Logging.StartFirstLevel(487);
            Logging.Comment("Открытие формы 'Товары по группе'");
            Logging.Comment("Произошло открытие формы 'Товары по группе' для группы " + groupName + "(" + groupId + ")");
            Logging.Comment("Пользователь: " + UserSettings.User.Id);

            grdPrices.Columns[3].DefaultCellStyle.Format = "F2";
            grdPrices.Columns[4].DefaultCellStyle.Format = "F2";
            grdPrices.Columns[5].DefaultCellStyle.Format = "F2";
            grdPrices.Columns[6].DefaultCellStyle.Format = "F2";
            grdPrices.DataSource = pricesTable;
            Logging.Comment("Завершение операции 'Открытие формы 'Товары по группе''");
            Logging.StopFirstLevel();
        }

        private void textBoxEAN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
        && !char.IsDigit(e.KeyChar)
        && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
        }

        private void textBoxEAN_TextChanged(object sender, EventArgs e)
        {
            FilterDataView();
        }

        public static string EscapeLikeValue(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }


        private void FilterDataView()
        {
            DataTable dt = pricesTable;
            DataView view = new DataView();


            view = dt.DefaultView;

            StringBuilder sb = new StringBuilder();

            string textFilter1 = EscapeLikeValue(textBoxEAN.Text);
            string textFilter2 = EscapeLikeValue(textBoxGoods.Text);

            if (textBoxEAN.Text.Length > 0)
            {
                sb.Append("ean like '%" + textFilter1 + "%'");
            }

            if (textBoxGoods.Text.Length > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" and ");
                }

                sb.Append("cname like '%" + textFilter2 + "%'");
            }

            stFilter = sb.ToString();
            view.RowFilter = stFilter;
            grdPrices.DataSource = view;
        }

        private void setWidthColumn(int indexRow, int indexCol, int width, Nwuram.Framework.ToExcelNew.ExcelUnLoad report)
        {
            report.SetColumnWidth(indexRow, indexCol, indexRow, indexCol, width);
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            Nwuram.Framework.ToExcelNew.ExcelUnLoad report = new Nwuram.Framework.ToExcelNew.ExcelUnLoad();

            int indexRow = 1;

            int maxColumns = 0;

            foreach (DataGridViewColumn col in grdPrices.Columns)
                if (col.Visible )
                {
                    maxColumns++;
                    /*if (col.Name.Equals(cDeps.Name)) setWidthColumn(indexRow, maxColumns, 18, report);
                    if (col.Name.Equals(cPost.Name)) setWidthColumn(indexRow, maxColumns, 18, report);
                    if (col.Name.Equals(cFIO.Name)) setWidthColumn(indexRow, maxColumns, 20, report);
                    if (col.Name.Equals(cPass.Name)) setWidthColumn(indexRow, maxColumns, 15, report);
                    if (col.Name.Equals(cDatePrintPass.Name)) setWidthColumn(indexRow, maxColumns, 17, report);
                    if (col.Name.Equals(cPhone.Name)) setWidthColumn(indexRow, maxColumns, 17, report);*/
                }

            #region "Head"
            report.Merge(indexRow, 1, indexRow, maxColumns);
            report.AddSingleValue($"{this.Text}", indexRow, 1);
            report.SetFontBold(indexRow, 1, indexRow, 1);
            report.SetFontSize(indexRow, 1, indexRow, 1, 16);
            report.SetCellAlignmentToCenter(indexRow, 1, indexRow, 1);
            indexRow++;
            indexRow++;


            //report.Merge(indexRow, 1, indexRow, maxColumns);
            //report.AddSingleValue($"Отдел: {cmbDeps.Text}", indexRow, 1);
            //indexRow++;

            //report.Merge(indexRow, 1, indexRow, maxColumns);
            //report.AddSingleValue($"Должность: {cmbPost.Text}", indexRow, 1);
            //indexRow++;

            //report.Merge(indexRow, 1, indexRow, maxColumns);
            //report.AddSingleValue($"Место работы: {(rbOffice.Checked ? rbOffice.Text : rbUni.Text)}", indexRow, 1);
            //indexRow++;

            //report.Merge(indexRow, 1, indexRow, maxColumns);
            //report.AddSingleValue($"Статус сотрудника: {(rbWork.Checked ? rbWork.Text : rbUnemploy.Text)}", indexRow, 1);
            //indexRow++;

            //if (tbPostName.Text.Trim().Length != 0 || tbKadrName.Text.Trim().Length != 0)
            //{
            //    report.Merge(indexRow, 1, indexRow, maxColumns);
            //    report.AddSingleValue($"Фильтр: {(tbPostName.Text.Trim().Length != 0 ? $"Должность:{tbPostName.Text.Trim()} | " : "")} {(tbKadrName.Text.Trim().Length != 0 ? $"ФИО:{tbKadrName.Text.Trim()}" : "")}", indexRow, 1);
            //    indexRow++;
            //}

            report.Merge(indexRow, 1, indexRow, maxColumns);
            report.AddSingleValue("Выгрузил: " + Nwuram.Framework.Settings.User.UserSettings.User.FullUsername, indexRow, 1);
            indexRow++;

            report.Merge(indexRow, 1, indexRow, maxColumns);
            report.AddSingleValue("Дата выгрузки: " + DateTime.Now.ToString(), indexRow, 1);
            indexRow++;
            indexRow++;
            #endregion

            int indexCol = 0;
            foreach (DataGridViewColumn col in grdPrices.Columns)
                if (col.Visible )
                {
                    indexCol++;
                    report.AddSingleValue(col.HeaderText, indexRow, indexCol);
                }
            report.SetFontBold(indexRow, 1, indexRow, maxColumns);
            report.SetBorders(indexRow, 1, indexRow, maxColumns);
            report.SetCellAlignmentToCenter(indexRow, 1, indexRow, maxColumns);
            report.SetCellAlignmentToJustify(indexRow, 1, indexRow, maxColumns);
            report.SetWrapText(indexRow, 1, indexRow, maxColumns);
            indexRow++;

            foreach (DataRowView row in pricesTable.DefaultView)
            {
                indexCol = 1;
                report.SetWrapText(indexRow, indexCol, indexRow, maxColumns);
                foreach (DataGridViewColumn col in grdPrices.Columns)
                {
                    if (col.Visible )
                    {
                        if (row[col.DataPropertyName] is DateTime)
                            report.AddSingleValue(((DateTime)row[col.DataPropertyName]).ToShortDateString(), indexRow, indexCol);
                        else
                           if (row[col.DataPropertyName] is decimal || row[col.DataPropertyName] is double)
                        {
                            report.AddSingleValueObject(row[col.DataPropertyName], indexRow, indexCol);
                            report.SetFormat(indexRow, indexCol, indexRow, indexCol, "0.00");
                        }
                        else
                            report.AddSingleValue(row[col.DataPropertyName].ToString(), indexRow, indexCol);

                        indexCol++;
                    }
                }

                report.SetBorders(indexRow, 1, indexRow, maxColumns);
                report.SetCellAlignmentToCenter(indexRow, 1, indexRow, maxColumns);
                report.SetCellAlignmentToJustify(indexRow, 1, indexRow, maxColumns);

                indexRow++;
            }
            report.SetColumnAutoSize(6, 1, indexRow, maxColumns);
            report.Show();


            //if (grdPrices.DataSource != null)
            //{


            //    //Logging.StartFirstLevel(472);
            //    //Logging.Comment("Выгрузка данных по товарам группы");
            //    string[] filePath = (Application.StartupPath + "\\tmp").Split(new char[] { '\\' });

            //    grdPrices.Refresh();
            //    DataTable printTable = pricesTable.Copy();
            //    if (stFilter != null)
            //    {
            //        printTable.DefaultView.RowFilter = stFilter;
            //    }
            //    Print(printTable.DefaultView.ToTable(), filePath[filePath.Length - 1]);

            //}
        }

        private string GetColumnSum(DataTable prices, string columnName)
        {
            return Convert.ToDecimal(prices.Compute("sum(" + columnName + ")", "")).ToString();
        }

        private void Print(DataTable reportTable, string filename)
        {
            try
            {
                if (Report.OOAvailable || Report.ExcelAvailable)
                {
                    if (grdPrices.DataSource != null)
                    {
                        Logging.StartFirstLevel(486);
                        Logging.Comment("Выгрузка данных по товарам группы");
                        Logging.Comment("Имя файла: " + filename);
                        Logging.Comment("Группа: " + groupName + " (id:" + groupId + ")");
                        Logging.Comment("Фильтр 'EAN': " + textBoxEAN.Text);
                        Logging.Comment("Фильтр 'Наименование товара': " + textBoxGoods.Text);
                        Logging.Comment("Отдел: " + mainformData[0]);
                        Logging.Comment("Вывод остатков: " + mainformData[1]);
                        Logging.Comment("Период расчета с " + mainformData[2]);
                        Logging.Comment("Период расчета по " + mainformData[3]);
                        Logging.Comment("Учитывать оптовые отгрузки: " + mainformData[4]);
                        if (Convert.ToBoolean(mainformData[4]))
                        {
                            Logging.Comment("Только отгруженные: " + mainformData[5]);
                        }

                        Logging.Comment("Пользователь: " + UserSettings.User.Id);

                        Report temp = new Report();
                        temp.AddSingleValue("typeBills", groupName);
                        temp.AddSingleValue("dateStart", mainformData[2]);
                        temp.AddSingleValue("dateFinish", mainformData[3]);

                        if (reportTable != null)
                        {
                            reportTable.Columns.Remove("prihod");
                            reportTable.Columns.Remove("otgruz");
                            reportTable.Columns.Remove("vozvr");
                            reportTable.Columns.Remove("spis");
                            reportTable.Columns.Remove("spis_inv");
                            reportTable.Columns.Remove("realiz");
                            reportTable.Columns.Remove("realiz_opt");
                            reportTable.Columns.Remove("vozvkass");

                           // Report temp = new Report();
                            temp.AddSingleValue("{rn.r1sum}", (GetColumnSum((DataTable)grdPrices.DataSource, "r1")));
                            temp.AddSingleValue("{rn.r2sum}", (GetColumnSum((DataTable)grdPrices.DataSource, "r2")));
                            temp.AddSingleValue("{rn.prihod_allsum}", (GetColumnSum((DataTable)grdPrices.DataSource, "prihod_all")));
                            temp.AddSingleValue("{rn.realiz_allsum}", (GetColumnSum((DataTable)grdPrices.DataSource, "realiz_all")));
                            temp.AddSingleValue("{rn.rnsum}", (GetColumnSum((DataTable)grdPrices.DataSource, "rn")));

                            //FillExcelTemplate fet = new FillExcelTemplate();

                            //reportTable.Columns.Remove("{rn.r1sum}",(GetColumnSum((DataTable)grdPrices.DataSource, "prihod")));
                          
                            // reportTable.Columns.Remove((GetColumnSum((DataTable)grdPrices.DataSource, "prihod")));
                           // reportTable.Columns.Add(GetColumnSum((DataTable)grdPrices.DataSource, "prihod"));
                            //reportTable.Columns.Add(GetColumnSum((DataTable)grdPrices.DataSource, "otgruz"));
                            //reportTable.Columns.Add(GetColumnSum((DataTable)grdPrices.DataSource, "vozvr"));
                            //reportTable.Columns.Add(GetColumnSum((DataTable)grdPrices.DataSource, "vozvr"));
                           
                            // reportTable.Columns.Remove("r1");
                           // reportTable.Columns.Remove("r2");       

                        }
                        //MessageBox.Show(reportTable.Columns[3].ToString());
                        temp.AddMultiValues(reportTable, "rn");
                        CreateAndShowReport(temp, "GroupRN", filename);
                        Logging.Comment("Завершение операции 'Выгрузка данных по товарам группы'");
                        Logging.StopFirstLevel();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Comment(ex.Message);
            }
        }
        private void CreateAndShowReport(Report temp, string filename, string resultname)
        {
            if (temp.CreateTemplate(Directory.GetCurrentDirectory() + "\\templates\\" + filename, Directory.GetCurrentDirectory() + "\\" + resultname, null))
                temp.OpenFile(Directory.GetCurrentDirectory() + "\\" + resultname);
            else
                MessageBox.Show("Ошибка! " + temp.ErrorMessage);
        }


        private void grdPrices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GoodsDialog_Load(object sender, EventArgs e)
        {

        }

        private void GoodsDialog_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void GoodsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void grdPrices_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (grdPrices.RowCount > 0)
            {
                buttonPrint.Enabled = true;
            }
        }

        private void grdPrices_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (grdPrices.RowCount > 0)
            {
                buttonPrint.Enabled = true;
            }
            else
            {
                buttonPrint.Enabled = false;
            }
        }
    }
}


