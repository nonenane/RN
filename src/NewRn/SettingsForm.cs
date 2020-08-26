using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;
//using Microsoft.VisualBasic.PowerPacks;
using NewRn.Data;
using NewRn.Properties;
using Nwuram.Framework.Logging;
using Nwuram.Framework.Settings.Connection;
using Nwuram.Framework.Settings.User;

namespace NewRn
{
    public partial class SettingsForm : Form
    {
        private Boolean closingForm = false;
        private string temp_date_time;
        private int temp_value_day;
        private int last_value;
        //string connectionString = GetConnectionString();
        //SqlConnection connection = new SqlConnection();
        Sql sql  = new Sql(ConnectionSettings.GetServer(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase(), ConnectionSettings.ProgramName);
        Sql sqlVVO = new Sql(ConnectionSettings.GetServer("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase("2"), ConnectionSettings.ProgramName);
        ArrayList startIDList = new ArrayList();
        ArrayList endIDList = new ArrayList();
        //private LogerSQL loger = new LogerSQL();

        public SettingsForm()
        {
            InitializeComponent();
            toolTip1.SetToolTip(SaveBtn, "Сохранить");
            toolTip1.SetToolTip(button2, "Выход");
        }
        #region Button_exit
        private void button2_Click(object sender, EventArgs e)
        {                 
            if (dateTimePicker1.Value.ToShortDateString() != temp_date_time || temp_value_day!= (int)numericUpDown1.Value || validate())
            {
                var result = MessageBox.Show(Resources.SettingsForm_button2_Click_, "       Закрытие формы       ",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    closingForm = true;
                    this.Dispose();
                }
            }
            else
            {
                closingForm = true;
                this.Dispose();
            }

        }
        #endregion

        #region Save_Button
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            string strtemp = validate_1();  
            var result_save = MessageBox.Show("Вы хотите сохранить настройки?","       Сохранение настроек       ",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result_save == DialogResult.Yes)
            {
            if (dateTimePicker1.Value.ToShortDateString() != temp_date_time ||
                temp_value_day != (int) numericUpDown1.Value || validate())
            {                
                    try
                    {
                        Logging.StartFirstLevel(343);
                        Logging.Comment("Сохранение настроек");                       
                        // connection.ConnectionString = connectionString;
                        //connection.Open();

                        //SqlCommand command = new SqlCommand("", connection);
                        if (dateTimePicker1.Value.ToShortDateString() != temp_date_time &&
                            ConnectionSettings.GetIdProgram() == 107)
                        {
                            /*command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rn.setDateValue";
                        command.Parameters.Add(new SqlParameter("@newDate", dateTimePicker1.Value.ToShortDateString()));
                        command.Parameters.Add(new SqlParameter("@id_prog", ConnectionSettings.GetIdProgram()));
                        //command.ExecuteReader();
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();*/
                            sql.SetDateGrn(dateTimePicker1.Value.ToShortDateString(), 107);
                            Logging.Comment("Изменение граничной даты округления: С " + temp_date_time + " На: " +
                                            dateTimePicker1.Value.ToShortDateString());
                        }

                        if (temp_value_day != (int) numericUpDown1.Value && ConnectionSettings.GetIdProgram() == 107)
                        {
                            sql.SetNumDate((int) numericUpDown1.Value);
                            //command.CommandType = CommandType.StoredProcedure;
                            //command.CommandText = "rn.setNumberValue";
                            //command.Parameters.Add(new SqlParameter("@newNumber", numericUpDown1.Value));                       
                            //command.ExecuteReader();
                            //command.ExecuteNonQuery();
                            //command.Parameters.Clear();
                            Logging.Comment("Изменение кол-ва дней после инвентаризации: С " + temp_value_day + " На: " +
                                            numericUpDown1.Value);
                        }

                        if (validate())
                        {
                            //sql.setGroupeOtdel(strtemp);
                           
                            ArrayList newArrayList = new ArrayList();
                            ArrayList ap = new ArrayList();
                            newArrayList.Clear();
                            bool exit = false;
                            foreach (string row1 in split_1)
                            {
                                if (row1 != null)
                                    newArrayList.Add(row1);
                            }
                            for (int i = 0; i < split_string.Length; i++)
                            {
                                //exit = false;
                                if (split_string[i] != null)
                                {
                                    for (int j = 1; j < split_string[i].Length; j++)
                                    {
                                        foreach (string row1 in split_1)
                                        {
                                            if (row1 == split_string[i][j])
                                            {
                                                // MessageBox.Show(split_string[i][0]);
                                                ap.Add(split_string[i][0]);
                                                //sql.deleteGroupeOtdel(split_string[i][0]);
                                                //for (int l = 1; l < split_string[i].Length; l++)
                                                //{                                       
                                                //    //if (split_string[i][l]!=null)
                                                //    //newArrayList.Add(split_string[i][l]);
                                                //}

                                                //j = 1;
                                                //int k = i;
                                                //if (k++ < split_string.Length)
                                                //{
                                                //    i++;
                                                //}
                                                //exit = true;
                                                break;
                                            }
                                        }
                                        //if (exit)
                                        //    break;
                                    }
                                }
                            }
                            ap.Sort();
                            for (int i = 0; i < ap.Count; i++)
                            {
                                for (int j = i + 1; j < ap.Count; j++)
                                {
                                    if (ap[i].Equals(ap[j]))
                                    {
                                        ap.RemoveAt(j--);
                                    }
                                }
                            }
                            foreach (string s in ap)
                            {
                                sql.deleteGroupeOtdel(s);
                            }
                            string new_string_to_save = "";
                           // newArrayList.Sort();
                            for (int i = 0; i < newArrayList.Count; i++)
                            {
                                for (int j = i + 1; j < newArrayList.Count; j++)
                                {
                                    if (newArrayList[i].Equals(newArrayList[j]))
                                    {
                                        newArrayList.RemoveAt(j--);
                                    }
                                }
                            }
                            foreach (string s in newArrayList)
                            {
                                Console.WriteLine(s);
                                if (s != null)
                                    new_string_to_save += s + ";";
                            }
                            new_string_to_save = new_string_to_save.Substring(0, new_string_to_save.Length - 1);
                            if (newArrayList.Count > 1)
                            {                             
                                sql.setGroupeOtdel(new_string_to_save);
                            }
                            Logging.Comment("Изменина группировка отделов: С " + first_string + " На: " +
                                           new_string_to_save);

                        }
                        //connection.Close();
                        /*loger.OpenSql();
                    MessageBox.Show(loger.getStatus());
                    loger.CloseSql();
                    MessageBox.Show(loger.getStatus());*/
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Logging.Comment(ex.Message);
                        //throw;
                    }
                    var result_save_ok = MessageBox.Show("Настройки сохранены", "Сохранение настроек",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (result_save_ok == DialogResult.OK)
                    {
                        Logging.Comment("Граничная дата округления: " + dateTimePicker1.Value.ToShortDateString());
                        Logging.Comment("Количество дней после инвентаризации: " + numericUpDown1.Value);
                        Logging.Comment("Группировка отделов: " + strtemp);
                        //Logging.Comment("Пользователь: " + UserSettings.User.Id);
                        Logging.Comment("Завершение операции 'Сохранение настроек'");
                        Logging.StopFirstLevel();
                        closingForm = true;
                        this.Dispose();
                    }
                }
            else
            {
                MessageBox.Show("Необходимо изменить данные!", "Нет данных", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            }
            
 
        }
        #endregion

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closingForm)
            {
                var result = MessageBox.Show(Resources.SettingsForm_button2_Click_, "       Закрытие формы       ",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {                    
                    this.Dispose();
                }
                else
                {
                    e.Cancel = true;
                }
            }            
        }
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = numericUpDown1.Enabled = !new List<string>(new string[] { "ПР" }).Contains(UserSettings.User.StatusCode);



            if (ConnectionSettings.GetIdProgram()==17)
            {
                dateTimePicker1.Enabled = false;
            }
            if (ConnectionSettings.GetIdProgram() == 17)
            {
                numericUpDown1.Enabled = false;
            }            
            dateTimePicker1.Value = new DateTime(2013,08,01);          
            temp_date_time = dateTimePicker1.Value.ToShortDateString();
            temp_value_day = (int)numericUpDown1.Value;
            last_value = temp_value_day;
            try
            {

            //connection.ConnectionString = connectionString;
            //connection.Open();
            //SqlCommand command = new SqlCommand("", connection);

            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandText = "rn.getNumberValue";
            //SqlDataReader reader = command.ExecuteReader();
            //command.Parameters.Clear();
               
               // if (!reader.Read())
               // {
               //     reader.Close();
               //     command.CommandType = CommandType.StoredProcedure;
                //    command.CommandText = "rn.insertNumberValue";
               //     reader = command.ExecuteReader();
               // }
               // else
              //  {
                if (sql.GetNumDate() != "")
                {
                    string weight = sql.GetNumDate();
                    numericUpDown1.Value = Convert.ToDecimal(weight);
                    temp_value_day = Convert.ToInt32(weight);
                }
                else
                {
                    sql.insertNumberValue();
                }
                //}
               // while (reader.Read())
        //    {
              //  string weight = reader.GetString(0);                
            //    numericUpDown1.Value= Convert.ToDecimal(weight);
              //  temp_value_day = Convert.ToInt32(weight);
            //}
            //command.Parameters.Clear();
            //reader.Close();

            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandText = "rn.getDateValue";           
            //reader = command.ExecuteReader();
            //command.Parameters.Clear();
              /*  if (!reader.Read())
                {
                    reader.Close();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "rn.insertDateValue";
                    reader = command.ExecuteReader();
                    command.Parameters.Clear();
                }
                else
                {*/
                if (sql.GetDateGrn() != "")
                {
                    DateTime newTime = Convert.ToDateTime(sql.GetDateGrn()); //Convert.ToDateTime(reader.GetString(0));
                    dateTimePicker1.Value = newTime;
                    temp_date_time = newTime.ToShortDateString();
                }
                else
                {
                    sql.insertDateValue();
                }
                // }
            //    while (reader.Read())
            //{                
                
            //}            
            //reader.Close();
            //connection.Close(); 

                DataTable tmp = new DataTable();
                tmp = sql.getDep();
                tmp.Merge(sqlVVO.getDep());
                DataView dv = tmp.DefaultView;
                dv.Sort = "id asc";
                DataTable sortedDT = dv.ToTable();

                cbDeps.DataSource = sortedDT;
                cbDeps.DisplayMember = "name";
                cbDeps.ValueMember = "id";
                DataTable nnn_1 = new DataTable();
                nnn_1 = sortedDT.Copy();
                dataGridView1.DataSource = nnn_1;



                //cbDeps.DataSource = sql.getDep();
                //cbDeps.DisplayMember = "name";
                //cbDeps.ValueMember = "id";
                //dataGridView1.DataSource = sql.getDep();

                //if (sql.getListOtdelGroup().Rows[0]["value"].ToString()=="")
                if (sql.getListOtdelGroup() == null || sql.getListOtdelGroup().Rows.Count==0)
                {
                  //  sql.insertGroupeOtdel();
                }
                
                frozcheck();
             //   checker();//???????????????????????/
                //MessageBox.Show(checkerTable.Rows[0]["value"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logging.Comment(ex.Message);
                //throw;
            }
        }

        private string blablabla;
        private string first_string;

        private void checker(string tmpstr)
        {
//            DataTable checkerTable = sql.getListOtdelGroup();

            string[] split = {""};
           // foreach (DataRow row in checkerTable.Rows)
          //  {
          //      tmpstr = Convert.ToString(row["value"]);
           //     blablabla = Convert.ToString(row["value"]);
            blablabla = tmpstr;
            split = tmpstr.Split(new Char[] {';', '\t', '\n'});            
            first_string = tmpstr;

            startIDList.Clear();
            foreach (string s in split)
            {
                if (s.Trim() != "")
                {
                    foreach (DataGridViewRow row1 in dataGridView1.Rows)
                    {
                        if (Convert.ToString(row1.Cells["id"].Value) == s)
                        {
                            row1.Cells["check"].Value = true;
                            startIDList.Add(Convert.ToInt32(s));
                        }
                    }
                }
            }
        //}
    }
        static private string GetConnectionString()
        {
            // To avoid storing the connection string in your code,  
            // you can retrieve it from a configuration file. 
            return @"Data Source="+ConnectionSettings.GetServer()+";Initial Catalog="+ConnectionSettings.GetDatabase()+";User ID="+ConnectionSettings.GetUsername()+";Password="+ConnectionSettings.GetPassword();
        }
        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!(char.IsDigit(e.KeyChar)) || numericUpDown1.Text.Length > 1) && e.KeyChar != (char)Keys.Back)
            {                                    
                    e.Handled = true;                
            }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           // if (dateTimePicker1.Value > DateTime.Today.AddDays(0))
        //    {
          //      dateTimePicker1.Value = DateTime.Today.AddDays(0);
          //  }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            last_value = int.Parse(numericUpDown1.Text);
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_CursorChanged(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown1_Leave_1(object sender, EventArgs e)
        {
            if (numericUpDown1.Text == "" )
            {
                numericUpDown1.Text = last_value.ToString();
            }
        }

        private void numericUpDown1_Enter(object sender, EventArgs e)
        {
            last_value = int.Parse(numericUpDown1.Text);
            // if (numericUpDown1.Text == "")
            //   {
            //      numericUpDown1.Value = 2;
            //  }
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            // if (numericUpDown1.Text == "")
            //   {
            //      numericUpDown1.Value = 2;
            //  }
        }

        private void numericUpDown1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // if (numericUpDown1.Text == "")
            //   {
            //      numericUpDown1.Value = 2;
            //  }
        }

        private void numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
           // if (numericUpDown1.Text == "")
         //   {
          //      numericUpDown1.Value = 2;
          //  }
        }
        private bool load_ggg=true;
        private bool edit = false;
        private void cbDeps_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataRow selectedDataRow = ((DataRowView)cbDeps.SelectedItem).Row;
            //int Id = Convert.ToInt32(selectedDataRow["id"]);            
            if (edit && !load_ggg)
            {
                che = false;
                edit = false;
                Logging.StartFirstLevel(343);
                var result = MessageBox.Show("Применить изменения", "       Сообщение       ",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    ArrayList newArrayList = new ArrayList();
                    ArrayList ap = new ArrayList();
                    newArrayList.Clear();
                    bool exit = false;
                    foreach (string row1 in split_1)
                    {
                        if (row1 != null)
                            newArrayList.Add(row1);
                    }
                    for (int i = 0; i < split_string.Length; i++)
                    {
                        //exit = false;
                        if (split_string[i] != null)
                        {
                            for (int j = 1; j < split_string[i].Length; j++)
                            {
                                foreach (string row1 in split_1)
                                {
                                    if (row1 == split_string[i][j])
                                    {
                                       // MessageBox.Show(split_string[i][0]);
                                        ap.Add(split_string[i][0]);
                                        //sql.deleteGroupeOtdel(split_string[i][0]);
                                        //for (int l = 1; l < split_string[i].Length; l++)
                                        //{                                       
                                        //    //if (split_string[i][l]!=null)
                                        //    //newArrayList.Add(split_string[i][l]);
                                        //}

                                        //j = 1;
                                        //int k = i;
                                        //if (k++ < split_string.Length)
                                        //{
                                        //    i++;
                                        //}
                                        //exit = true;
                                        break;
                                    }
                                }
                                //if (exit)
                                //    break;
                            }
                        }
                    }
                    ap.Sort();
                    for (int i = 0; i < ap.Count; i++)
                    {
                        for (int j = i + 1; j < ap.Count; j++)
                        {
                            if (ap[i].Equals(ap[j]))
                            {
                                ap.RemoveAt(j--);
                            }
                        }
                    }
                    foreach (string s in ap)
                    {
                        sql.deleteGroupeOtdel(s);
                    }
                    string new_string_to_save = "";
                   // newArrayList.Sort();
                    for (int i = 0; i < newArrayList.Count; i++)
                    {
                        for (int j = i + 1; j < newArrayList.Count; j++)
                        {
                            if (newArrayList[i].Equals(newArrayList[j]))
                            {
                                newArrayList.RemoveAt(j--);
                            }
                        }
                    }
                    foreach (string s in newArrayList)
                    {
                        Console.WriteLine(s);
                        if (s != null)
                            new_string_to_save += s + ";";
                    }
                    new_string_to_save = new_string_to_save.Substring(0, new_string_to_save.Length - 1);
                    if (newArrayList.Count > 1)
                    {
                        sql.setGroupeOtdel(new_string_to_save);
                    }
                    Logging.Comment("Изменена группировка отделов: С " + first_string + " На: " +
                                       new_string_to_save);
                    Logging.StopFirstLevel();
                }
            }
            frozcheck();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            frozcheck();
            //string[] split = blablabla.Split(new Char[] { ';', '\t', '\n' });

            //foreach (string s in split)
            //{
            //    if (s.Trim() != "")
            //    {
            //        foreach (DataGridViewRow row in dataGridView1.Rows)
            //        {
            //           // DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[2];
            //            if (Convert.ToString(row.Cells["id"].Value) == s)
            //            {
            //                row.Cells["check"].Value = true;
            //                //startIDList.Add(Convert.ToInt32(s));
            //            }
            //        }
            //    }
            //}
        }

        private Boolean validate()
        {
            validate_1();
            DataTable tempID = sql.getListOtdelGroup();
            string tmpstr = "";
            startIDList.Clear();
            foreach (DataRow row in tempID.Rows)
            {
                tmpstr += Convert.ToString(row["value"]);
            }
            string[] split = tmpstr.Split(new Char[] { ';', '\t', '\n' });

            foreach (string s in split)
            {
                if (s.Trim() != "")
                {
                    startIDList.Add(Convert.ToInt32(s));
                }
            }
            endIDList.Sort();
            startIDList.Sort();
            foreach (int arr in endIDList)
            {
                if (!startIDList.Contains(arr))
                {
                    return true;
                }
            }
            foreach (int arr in startIDList)
            {
                if (!endIDList.Contains(arr))
                {
                    return true;
                }
            }
            return false;
        }

        private string validate_1()
        {
            string strtemp = "";
            endIDList.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["check"].Value))
                {
                    endIDList.Add(Convert.ToInt32(row.Cells["id"].Value));
                    strtemp += row.Cells["id"].Value + ";";
                }
            }
            if (strtemp != "")
            {
                strtemp = strtemp.Substring(0, strtemp.Length - 1);
                blablabla = strtemp;
            }
            return strtemp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strtemp = validate_1();
            MessageBox.Show(strtemp);
        }

        private void frozcheck()
        {
            DataRow selectedDataRow = ((DataRowView)cbDeps.SelectedItem).Row;
            int Id = Convert.ToInt32(selectedDataRow["id"]);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {                                
                    row.Cells["check"].Value = false;
                    row.Cells["check"].ReadOnly = false;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["id"].Value) == Id)
                {
                   // Console.WriteLine(row.Cells["id"].Value);
                    row.Cells["check"].Value = true;
                    row.Cells["check"].ReadOnly = true;
                    validate_1();
                    DataTable checkerTable = sql.getListOtdelGroup();
                    string tmpstr = "";
                    string[] split={""};
                    foreach (DataRow row_1 in checkerTable.Rows)
                    {
                        tmpstr = Convert.ToString(row_1["value"]);
                        split = tmpstr.Split(new Char[] {';', '\t', '\n'});

                        //string[] split = tmpstr.Split(new Char[] { ';', '\t', '\n' });
                        foreach (string s in split)
                        {
                            if (s.Trim() != "")
                            {
                                if (Id.ToString() == s)
                                {
                                    //MessageBox.Show("Hello");
                                    checker(tmpstr);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
//            getCheckedBox();
        }
        private string[] split_1 = new string[11];
        private void getCheckedBox()
        {
            //dataGridView1.Invalidate();           
                split_1 = new string[11];
                string str_end = "";
                int k = 0;
                foreach (DataGridViewRow row1 in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row1.Cells["check"].Value))
                    {
                        split_1[k] = row1.Cells["id"].Value.ToString();
                        str_end += row1.Cells["id"].Value.ToString() + ";";
                        k++;
                    }
                }
            //for (int i = 0; i < split_string.Length; i++)
            //{
            //    if (split_string[i] != null)
            //        for (int j = 0; j < split_string[i].Length; j++)
            //        {
            //            foreach (DataGridViewRow row1 in dataGridView1.Rows)
            //            {
            //                if (Convert.ToString(row1.Cells["id"].Value) == split_string[i][j] && Convert.ToBoolean(row1.Cells["check"].Value))
            //                {

            //                    MessageBox.Show("123");
            //                        row1.Cells["check"].Value = true;
            //                    //startIDList.Add(Convert.ToInt32(s));
            //                }
            //            }
            //        }
            //}


            foreach (string s in split_1)
            {
                if(s!=null)
                Console.WriteLine(s);    
            }
            if (str_end.Length > 0)
            {
                Console.WriteLine(str_end.Substring(0, str_end.Length - 1));
                str_end = str_end.Substring(0, str_end.Length - 1);
            }

            if (!che)
            {
                DataTable checkerTable = sql.getListOtdelGroup();
                string tmpstr = "";
                string[] split = { "" };
                splitLength = 0;
                foreach (DataRow row_1 in checkerTable.Rows)
                {
                    tmpstr = Convert.ToString(row_1["id"])+";";
                    tmpstr += Convert.ToString(row_1["value"]);
                    split = tmpstr.Split(new Char[] { ';', '\t', '\n' });
                    split_string[splitLength] = split;
                    splitLength++;
                }
                che = true;
            }
            //foreach (string s in split_1)
            //{
            //    if (s != null)
            //    {
            //        for (int i = 0; i < split_string.Length; i++)
            //        {
            //            if (split_string[i] != null)
            //            {
            //                for (int j = 0; j < split_string[i].Length;j++)
            //                {
            //                    if (split_string[i][j] == s)
            //                    {
            //                       // MessageBox.Show(split_string[i][j]);
            //                        break;                                    
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
          
            /*
                DataTable checkerTable = sql.getListOtdelGroup();
                string tmpstr = "";
                string[] split = {""};
            foreach (DataRow row_1 in checkerTable.Rows)
            {
                tmpstr = Convert.ToString(row_1["value"]);
                split = tmpstr.Split(new Char[] {';', '\t', '\n'});
                for (int i = 0; i < k; i++)
                {
                    //string[] split = tmpstr.Split(new Char[] { ';', '\t', '\n' });
                    foreach (string s in split)
                    {
                        if (s.Trim() != "")
                        {
                            if (split_1[i] == s)
                            {
                                // MessageBox.Show(row_1["id"].ToString());
                                foreach (string cheked in split)
                                {
                                    if (cheked.Trim() != "")
                                    {
                                        foreach (DataGridViewRow row1 in dataGridView1.Rows)
                                        {
                                            if (Convert.ToString(row1.Cells["id"].Value) == cheked)
                                            {
                                                row1.Cells["check"].Value = true;
                                                startIDList.Add(Convert.ToInt32(s));
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }*/
        }

        private bool che = false;
        private string[][] split_string=new string[11][];
        private int splitLength;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {/*
            //che = true;
            //getCheckedBox();
            if (!che)
            {                
                DataTable checkerTable = sql.getListOtdelGroup();
                string tmpstr = "";
                string[] split = {""};
                splitLength = 0;
                foreach (DataRow row_1 in checkerTable.Rows)
                {
                    tmpstr = Convert.ToString(row_1["value"]);
                    split = tmpstr.Split(new Char[] {';', '\t', '\n'});
                    split_string[splitLength]= split;
                    splitLength++;
                }
                che = true;
            }
            if (!Convert.ToBoolean(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["check"].Value))
            {
                for (int i = 0; i < split_string.Length; i++)
                {
                    if (split_string[i]!=null)
                    for(int j =0;j<split_string[i].Length;j++)
                    {
                        if (split_string[i][j] != "null" &&
                            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString() == split_string[i][j])
                        {
                            split_string[i][j] = "-10";
                        }
                    }                    
                }
            }
            getCheckedBox();*/
            load_ggg = false;
            edit = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            getCheckedBox();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).IsCurrentCellDirty) ((DataGridView)sender).CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            bool exit;
            ArrayList ap = new ArrayList();
            for (int i = 0; i < split_string.Length; i++)
            {
                exit = false;
                if (split_string[i] != null)
                {
                    for (int j = 1; j < split_string[i].Length; j++)
                    {
                        foreach (string row1 in split_1)
                        {
                            if (row1 == split_string[i][j])
                            {
                               // Console.WriteLine(split_string[i][0]);
                                ap.Add(split_string[i][0]);
                                // MessageBox.Show(split_string[i][0]);
                               // sql.deleteGroupeOtdel(split_string[i][0]);
                                //for (int l = 1; l < split_string[i].Length; l++)
                                //{                                       
                                //    //if (split_string[i][l]!=null)
                                //    //newArrayList.Add(split_string[i][l]);
                                //}

                               // j = 1;
                              //  int k = i;
                              //  if (k++ < split_string.Length)
                               // {
                               //     i++;
                              //  }
                                //exit = true;
                                break;
                            }
                        }
                        //if (exit)
                        //    break;
                    }
                }
            }
            ap.Sort();
            for (int i = 0; i < ap.Count; i++)
            {
                for (int j = i + 1; j < ap.Count; j++)
                {
                    if (ap[i].Equals(ap[j]))
                    {
                        ap.RemoveAt(j--);
                    }
                }
            }
            foreach (string s in ap)
            {
                Console.WriteLine(s);                
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            GroupOtdel grpO = new GroupOtdel();
            grpO.ShowDialog();
        }
    }
}
