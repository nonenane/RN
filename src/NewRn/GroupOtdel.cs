using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewRn.Data;
using Nwuram.Framework.Logging;
using Nwuram.Framework.Settings.Connection;
using Nwuram.Framework.Settings.User;

namespace NewRn
{
    public partial class GroupOtdel : Form
    {
        private Sql sql = null;
        private Sql sqlVVO = null;
        private IDataWorker dataWorker = null;
        private DataTable listOtdel;
        private DataRow groupRow;
        private int indexrow;
        private string indexName;
        private bool change = false;
        private int id_to_del;
        private ArrayList name_list = new ArrayList();
        private ArrayList id_list = new ArrayList();

        public GroupOtdel()
        {
            try
            {
                InitializeComponent();
                sql = new Sql(ConnectionSettings.GetServer(), ConnectionSettings.GetUsername(),
                    ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase(), ConnectionSettings.ProgramName);
                sqlVVO = new Sql(ConnectionSettings.GetServer("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase("2"), ConnectionSettings.ProgramName);
                listOtdel = sql.getDepartments();
                listOtdel.Rows[0].Delete();
                listOtdel.AcceptChanges();
                change = false;
                comboBox1.DataSource = listOtdel;
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
                        comboBox1.SelectedValue = UserSettings.User.IdDepartment.ToString();
                    else
                        comboBox1.SelectedValue = id_otdel_tmp.ToString();

                    //comboBox1.SelectedIndex =
                    //    Convert.ToInt32(
                    //        ((DataTable)comboBox1.DataSource).DefaultView.ToTable().Select("id=1")[0]["id"])-1;
                    comboBox1.Enabled = false;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }

        }

        private DataTable MakeGroupsTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof (int));
            table.Columns.Add("cname", typeof (string));
            table.Columns.Add("dep", typeof (int));
            table.Columns.Add("gcn", typeof(string));   
            return table;
        }

        private DataTable MakeGroupsTable_chk()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("cname", typeof(string));
            table.Columns.Add("dep", typeof(int));
            table.Columns.Add("chk", typeof(bool));
            return table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (change)
            {
                if (DialogResult.Yes ==
                    MessageBox.Show("Сохранить изменения", "Запрос на сохранение?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {                    
                    change = false;
                    save();                    
                }
                else
                {
                    MessageBox.Show("Изменения не сохранены!");
                    change = false;
                }
            }
            this.DialogResult = DialogResult.OK;                    
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (change)
            {
                if (DialogResult.Yes ==
                    MessageBox.Show("Сохранить изменения", "Запрос на сохранение?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {                   
                    change = false;
                    save();        
                }
                else
                {
                    MessageBox.Show("Изменения не сохранены!");
                    change = false;
                }
            }
            update_table();
        }

        private string[] arrayOtdelGroup(int otdel)
        {
            DataTable checkerTable = new DataTable();
            if (UserSettings.User.StatusCode == "МН")
            {
                 checkerTable = sql.getListOtdelGroup(0);
            }
            else
            {
                 checkerTable = sql.getListOtdelGroup();
            }
            string tmpstr = "";
            string[] split = {""};
            foreach (DataRow row_1 in checkerTable.Rows)
            {
                tmpstr = Convert.ToString(row_1["value"]);
                split = tmpstr.Split(new Char[] {';', '\t', '\n'});
                if (Convert.ToInt32(split[0]) == otdel)
                    break;
            }
            return split;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (change)
            {
                if (DialogResult.Yes ==
                    MessageBox.Show("Сохранить изменения", "Запрос на сохранение?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {                
                    change = false;
                    save();
                    update_table();
                  
                }
                else
                {
                    MessageBox.Show("Изменения не сохранены!");
                    change = false;
                }
            }
           
            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                id_list.Clear();
                name_list.Clear();
                dataWorker = sql;
                if (Convert.ToInt32(r.Cells[2].Value)==6)
                    dataWorker = sqlVVO;              
                indexrow = Convert.ToInt32(r.Cells[0].Value);
                indexName = Convert.ToString(r.Cells["Column2"].Value);
                // Console.WriteLine(r.Cells[2].Value);
                DataTable groupsRN = MakeGroupsTable_chk();
                ArrayList groups = dataWorker.GetGroups(Convert.ToInt32(r.Cells[2].Value));
                foreach (Group group in groups)
                {
                    if (indexrow != group.Id)
                    {
                        groupRow = groupsRN.NewRow();
                        groupRow["id"] = group.Id;
                        groupRow["cname"] = group.Name;
                        groupRow["dep"] = Convert.ToInt32(r.Cells[2].Value);
                        groupRow["chk"] = false;
                        groupsRN.Rows.Add(groupRow);
                    }
                    else
                    {
                        id_list.Add(group.Id);
                        name_list.Add(group.Name);
                        indexName = group.Name;
                    }
                }
                string tmpstr = "";
                string nameotdel = "";
                string idotdel = "";
                id_to_del = 0;
                DataTable dtGroupDataTable = sql.getGroupGroup(Convert.ToInt32(r.Cells[2].Value));
                foreach (DataRow row in dtGroupDataTable.Rows)
                {
                    tmpstr = Convert.ToString(row["val"]);

                    string[] split = tmpstr.Split(new Char[] { ',', '\t', '\n' });
                    for (int j = 0; j < split.Length; j++)
                    {
               //         Console.WriteLine(split[j]);
                        foreach (DataRow f in groupsRN.Rows)
                        {
                            if (split[j] == f["id"].ToString())
                            {
                                f["chk"] = true;
                                if (indexrow!=Convert.ToInt32(split[0]))
                                    f.Delete();
                                if (indexrow == Convert.ToInt32(split[0]) && id_to_del == 0)
                                    id_to_del = Convert.ToInt32(row["id"]);
                                break;
                            }
                        }
                        groupsRN.AcceptChanges();
                    }
                    //MessageBox.Show(nameotdel);                   
                    groupsRN.AcceptChanges();
                }
                foreach (DataRow row in groupsRN.Rows)
                {
                    if (Convert.ToBoolean(row["chk"]))
                    {
                        name_list.Add(row["cname"]);
                        id_list.Add(row["id"]);
                    }
                }
                dataGridView2.DataSource = groupsRN;
                //MessageBox.Show(id_to_del.ToString());
            }
        }
        //test2
        //private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (change)
        //    {
        //        if (DialogResult.Yes ==
        //            MessageBox.Show("Сохранить изменения", "Запрос на сохранение?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
        //        {
        //            change = false;
        //            save();
        //            update_table();

        //        }
        //        else
        //        {
        //            MessageBox.Show("Изменения не сохранены!");
        //            change = false;
        //        }
        //    }

        //    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
        //    {
        //        id_list.Clear();
        //        name_list.Clear();
        //        dataWorker = sql;
        //        if (Convert.ToInt32(r.Cells[2].Value) == 6)
        //            dataWorker = sqlVVO;
        //        indexrow = Convert.ToInt32(r.Cells[0].Value);
        //        indexName = Convert.ToString(r.Cells["Column2"].Value);
        //        // Console.WriteLine(r.Cells[2].Value);
        //        DataTable groupsRN = MakeGroupsTable_chk();
        //        ArrayList groups = dataWorker.GetGroups_inv(Convert.ToInt32(r.Cells[2].Value));
        //        foreach (Group group in groups)
        //        {
        //            if (indexrow != group.Id)
        //            {
        //                groupRow = groupsRN.NewRow();
        //                groupRow["id"] = group.Id;
        //                groupRow["cname"] = group.Name;
        //                groupRow["dep"] = Convert.ToInt32(r.Cells[2].Value);
        //                groupRow["chk"] = false;
        //                groupsRN.Rows.Add(groupRow);
        //            }
        //            else
        //            {
        //                id_list.Add(group.Id);
        //                name_list.Add(group.Name);
        //                indexName = group.Name;
        //            }
        //        }
        //        string tmpstr = "";
        //        string nameotdel = "";
        //        string idotdel = "";
        //        id_to_del = 0;
        //        DataTable dtGroupDataTable = sql.getGroupGroup(Convert.ToInt32(r.Cells[2].Value));
        //        foreach (DataRow row in dtGroupDataTable.Rows)
        //        {
        //            tmpstr = Convert.ToString(row["val"]);

        //            string[] split = tmpstr.Split(new Char[] { ',', '\t', '\n' });
        //            for (int j = 0; j < split.Length; j++)
        //            {
        //                //         Console.WriteLine(split[j]);
        //                foreach (DataRow f in groupsRN.Rows)
        //                {
        //                    if (split[j] == f["id"].ToString())
        //                    {
        //                        f["chk"] = true;
        //                        if (indexrow != Convert.ToInt32(split[0]))
        //                            f.Delete();
        //                        if (indexrow == Convert.ToInt32(split[0]) && id_to_del == 0)
        //                            id_to_del = Convert.ToInt32(row["id"]);
        //                        break;
        //                    }
        //                }
        //                groupsRN.AcceptChanges();
        //            }
        //            //MessageBox.Show(nameotdel);                   
        //            groupsRN.AcceptChanges();
        //        }
        //        foreach (DataRow row in groupsRN.Rows)
        //        {
        //            if (Convert.ToBoolean(row["chk"]))
        //            {
        //                name_list.Add(row["cname"]);
        //                id_list.Add(row["id"]);
        //            }
        //        }
        //        dataGridView2.DataSource = groupsRN;
        //        //MessageBox.Show(id_to_del.ToString());
        //    }
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            string strtemp = "";
            ArrayList listGroupArrayList = new ArrayList();
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (Convert.ToBoolean(row.Cells[3].Value))
                {
                    listGroupArrayList.Add(row.Cells[0].Value.ToString());
                    //                  Console.WriteLine(row.Cells[0].Value);
                }
            }
            listGroupArrayList.Add(indexrow.ToString());
//            Console.WriteLine(indexrow);
            listGroupArrayList.Sort();

            foreach (string str in listGroupArrayList)
            {
                strtemp += str + ",";
            }
            if (strtemp != "")
            {
                strtemp = strtemp.Substring(0, strtemp.Length - 1);
            }
            Console.WriteLine("Dep:" + dataGridView2.Rows[0].Cells[2].Value);
            Console.WriteLine("Groupe: "+strtemp);
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            change = true;
        }

        private void update_table()
        {
            dataWorker = sql;            
            DataRow selectedDataRow = ((DataRowView)comboBox1.SelectedItem).Row;
            int Id = Convert.ToInt32(selectedDataRow["Id"]);
            DataTable groupsRN = MakeGroupsTable();
            string [] fff =
            arrayOtdelGroup(Id);
            if (dataWorker != null)
            {
                //if (Convert.ToInt32(arrayOtdelGroup(Id)[0]) == Id && arrayOtdelGroup(Id).Length> 1)
                if (fff[0] != "")
                {
                    if (Convert.ToInt32(fff[0]) == Id && fff.Length > 1)
                    {
                        for (int i = 0; i < fff.Length; i++)
                        {
                            dataWorker = sql;
                            if (Convert.ToInt32(fff[i]) == 6)
                                dataWorker = sqlVVO;
                            ArrayList groups = dataWorker.GetGroups(Convert.ToInt32(fff[i]));
                            foreach (Group group in groups)
                            {
                                groupRow = groupsRN.NewRow();
                                groupRow["id"] = group.Id;
                                groupRow["cname"] = group.Name;
                                groupRow["dep"] = Convert.ToInt32(fff[i]);
                                groupRow["gcn"] = group.Id;
                                groupsRN.Rows.Add(groupRow);
                            }

                            string tmpstr = "";
                            string nameotdel = "";
                            string idotdel = "";
                            DataTable dtGroupDataTable = sql.getGroupGroup(Convert.ToInt32(fff[i]));
                            foreach (DataRow row in dtGroupDataTable.Rows)
                            {
                                nameotdel = "";
                                idotdel = "";
                                tmpstr = Convert.ToString(row["val"]);

                                string[] split = tmpstr.Split(new Char[] {',', '\t', '\n'});
                                for (int j = 0; j < split.Length; j++)
                                {
                                    //  Console.WriteLine(split[i]);
                                    foreach (DataRow f in groupsRN.Rows)
                                    {
                                        if (split[j] == f["id"].ToString())
                                        {
                                            //f["chk"] = true;
                                            nameotdel += f["cname"] + "/";
                                            idotdel += f["id"] + "/";
                                            f.Delete();
                                            break;
                                        }
                                    }
                                    groupsRN.AcceptChanges();
                                }
                                //MessageBox.Show(nameotdel);
                                nameotdel = nameotdel.Remove(nameotdel.Length - 1, 1);
                                idotdel = idotdel.Remove(idotdel.Length - 1, 1);
                                DataRow otdelrow = groupsRN.NewRow();
                                otdelrow["id"] = Convert.ToInt16(split[0]);
                                otdelrow["cname"] = nameotdel;
                                otdelrow["dep"] = Convert.ToInt32(fff[i]);
                                otdelrow["gcn"] = idotdel;
                                // otdelrow["chk"] = true;
                                groupsRN.Rows.InsertAt(otdelrow, Convert.ToInt32(split[0]) - 1);
                                groupsRN.AcceptChanges();
                                groupsRN.DefaultView.Sort = "id asc";
                                groupsRN = groupsRN.DefaultView.ToTable(true);
                            }
                        }
                        //dataGridView1.DataSource = groupsRN;
                    }
                    else
                    {
                        ArrayList groups = dataWorker.GetGroups(Id);
                        foreach (Group group in groups)
                        {
                            groupRow = groupsRN.NewRow();
                            groupRow["id"] = group.Id;
                            groupRow["cname"] = group.Name;
                            groupRow["dep"] = Id;
                            groupRow["gcn"] = group.Id;
                            groupsRN.Rows.Add(groupRow);
                        }
                        string tmpstr = "";
                        string nameotdel = "";
                        string idotdel = "";
                        DataTable dtGroupDataTable = sql.getGroupGroup(Id);
                        foreach (DataRow row in dtGroupDataTable.Rows)
                        {
                            nameotdel = "";
                            idotdel = "";
                            tmpstr = Convert.ToString(row["val"]);

                            string[] split = tmpstr.Split(new Char[] { ',', '\t', '\n' });
                            for (int j = 0; j < split.Length; j++)
                            {
                                Console.WriteLine(split[j]);
                                foreach (DataRow f in groupsRN.Rows)
                                {
                                    if (split[j] == f["id"].ToString())
                                    {
                                        //f["chk"] = true;
                                        nameotdel += f["cname"] + "/";
                                        idotdel += f["id"] + "/";
                                        f.Delete();
                                        break;
                                    }
                                }
                                groupsRN.AcceptChanges();
                            }
                            //MessageBox.Show(nameotdel);
                            nameotdel = nameotdel.Remove(nameotdel.Length - 1, 1);
                            idotdel = idotdel.Remove(idotdel.Length - 1, 1);
                            DataRow otdelrow = groupsRN.NewRow();
                            otdelrow["id"] = Convert.ToInt16(split[0]);
                            otdelrow["cname"] = nameotdel;
                            otdelrow["dep"] = Id;
                            otdelrow["gcn"] = idotdel;
                            // otdelrow["chk"] = true;
                            groupsRN.Rows.InsertAt(otdelrow, Convert.ToInt32(split[0]) - 1);
                            groupsRN.AcceptChanges();
                            groupsRN.DefaultView.Sort = "id asc";
                            groupsRN = groupsRN.DefaultView.ToTable(true);
                        }
                    }
                }
                else
                {
                    ArrayList groups = dataWorker.GetGroups(Id);
                    foreach (Group group in groups)
                    {
                        groupRow = groupsRN.NewRow();
                        groupRow["id"] = group.Id;
                        groupRow["cname"] = group.Name;
                        groupRow["dep"] = Id;
                        groupRow["gcn"] = group.Id;
                        groupsRN.Rows.Add(groupRow);
                    }
                    string tmpstr = "";
                    string nameotdel = "";
                    string idotdel = "";
                    DataTable dtGroupDataTable = sql.getGroupGroup(Id);
                    foreach (DataRow row in dtGroupDataTable.Rows)
                    {
                        nameotdel = "";
                        idotdel = "";
                        tmpstr = Convert.ToString(row["val"]);

                        string[] split = tmpstr.Split(new Char[] {',', '\t', '\n'});
                        for (int j = 0; j < split.Length; j++)
                        {
                            Console.WriteLine(split[j]);
                            foreach (DataRow f in groupsRN.Rows)
                            {
                                if (split[j] == f["id"].ToString())
                                {
                                    //f["chk"] = true;
                                    nameotdel += f["cname"] + "/";
                                    idotdel += f["id"] + "/";
                                    f.Delete();
                                    break;
                                }
                            }
                            groupsRN.AcceptChanges();
                        }
                        //MessageBox.Show(nameotdel);
                        nameotdel = nameotdel.Remove(nameotdel.Length - 1, 1);
                        idotdel = idotdel.Remove(idotdel.Length - 1, 1);
                        DataRow otdelrow = groupsRN.NewRow();
                        otdelrow["id"] = Convert.ToInt16(split[0]);
                        otdelrow["cname"] = nameotdel;
                        otdelrow["dep"] = Id;
                        otdelrow["gcn"] = idotdel;
                        // otdelrow["chk"] = true;
                        groupsRN.Rows.InsertAt(otdelrow, Convert.ToInt32(split[0]) - 1);
                        groupsRN.AcceptChanges();
                        groupsRN.DefaultView.Sort = "id asc";
                        groupsRN = groupsRN.DefaultView.ToTable(true);
                    }
                }
                dataGridView1.DataSource = groupsRN;
            }
        }

        private bool save()
        {
            try
            {
                Logging.StartFirstLevel(343);
                Logging.Comment("Группировка групп. Время: "+DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString());
                Logging.Comment("ДО");
                for (int i = 0; i < id_list.Count; i++)
                {
                    Logging.Comment("ID: " + id_list[i].ToString()
                                    + " Название: " + name_list[i]);                         
                }
                Logging.Comment("После");
                string strtemp = "";
                ArrayList listGroupArrayList = new ArrayList();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["dataGridViewCheckBoxColumn1"].Value))
                    {
                        listGroupArrayList.Add(row.Cells["dataGridViewTextBoxColumn1"].Value.ToString());
                        Logging.Comment("ID: "+row.Cells["dataGridViewTextBoxColumn1"].Value.ToString()+
                        " Название: "+row.Cells["dataGridViewTextBoxColumn2"].Value.ToString());
                        //                  Console.WriteLine(row.Cells[0].Value);
                    }
                }
                Logging.Comment("ID: " + indexrow.ToString() +
                        " Название: " + indexName);
                listGroupArrayList.Add(indexrow.ToString());
                //            Console.WriteLine(indexrow);
                listGroupArrayList.Sort();

                foreach (string str in listGroupArrayList)
                {
                    strtemp += str + ",";
                }
                if (strtemp != "")
                {
                    strtemp = strtemp.Substring(0, strtemp.Length - 1);
                }
             //   MessageBox.Show(id_to_del.ToString());
                if(id_to_del!=0)
                    sql.deleteGroupGroup(id_to_del);
                if (listGroupArrayList.Count > 1)
                {
             //       MessageBox.Show(listGroupArrayList.Count.ToString());
                    sql.insertGroupeGroupe(
                        Convert.ToInt32(dataGridView2.Rows[0].Cells["dataGridViewTextBoxColumn3"].Value), strtemp);
                }
                Console.WriteLine("Dep:" + dataGridView2.Rows[0].Cells["dataGridViewTextBoxColumn3"].Value);
                Console.WriteLine("Groupe: " + strtemp);
                Logging.Comment("Пользователь: " + UserSettings.User.FullUsername + " ID: " + UserSettings.User.Id);
                Logging.Comment("Завершение группировки групп");
                Logging.StopFirstLevel();
                MessageBox.Show("Изменения сохранены");
                return true;
            }
            catch (Exception)
            {

                return false;
            }            
        }
    }
}
