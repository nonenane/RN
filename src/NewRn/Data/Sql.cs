using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Nwuram.Framework.Data;
using Nwuram.Framework.Settings.Connection;
using Nwuram.Framework.Settings.User;

namespace NewRn.Data
{
    //класс для доступа к данным - получение нужных данных в виде DataTable
    class Sql : IDataWorker
    {

        #region Data

        ArrayList ap = new ArrayList();
        SqlProvider sql = null;
        private bool load_table = false;

        #endregion

        #region Constructors

        public Sql(string server, string username, string password, string dbName, string appName)
        {
            sql = new SqlProvider(server, dbName, username, password, appName);
        }

        #endregion

        #region Public Methods

        //получение списка пользователей (для авторизации)
        public DataTable getUsers(ArrayList ap)
        {
            return sql.executeProcedure("GetListUsers", new string[2] { "@name_comp", "@id_prog" }, new DbType[] { DbType.String, DbType.Int32 }, ap);
        }

        //получение таблицы доступа пользователя (для авторизации)
        public DataTable getAccessForUser(ArrayList ap)
        {
            return sql.executeProcedure("GetAccessForUser", new string[4] { "@comp_name", "@id_user", "@id_prog", "@exe_name" }, new DbType[] { DbType.String, DbType.Int32, DbType.Int32, DbType.String }, ap);
        }

        //получение списка отделов
        public DataTable getDepartments(bool notSettings =false)
        {
            DataTable departments = sql.executeProcedure("CountRN.GetDepartments", new string[] { }, new DbType[] { }, new ArrayList());
            if (notSettings)
            {
                if (departments != null)
                {
                    DataRow firstRow = departments.NewRow();
                    firstRow["id"] = -1;
                    firstRow["name"] = "Все отделы";
                    departments.Rows.InsertAt(firstRow, 0);
                    departments.AcceptChanges();
                }
                return departments;
            }
            else if (departments != null)
            {

                DataRow vvoRow = departments.NewRow();
                vvoRow["id"] = 6;
                vvoRow["name"] = "ВВО";
                departments.Rows.InsertAt(vvoRow, 5);

                if (!load_table)
                {
                    DataTable checkerTable = new DataTable();
                    if (UserSettings.User.StatusCode == "МН")
                    {
                        checkerTable = getListOtdelGroup(0);
                    }
                    else
                    {
                        checkerTable = getListOtdelGroup();
                    }
                    if (checkerTable.Rows.Count > 0)
                    {
                        string tmpstr = "";
                        foreach (DataRow row_1 in checkerTable.Rows)
                        {
                            tmpstr = Convert.ToString(row_1["value"]);

                            string[] split = tmpstr.Split(new Char[] { ';', '\t', '\n' });
                            //                MessageBox.Show(split[0].ToString());
                            string nameotdel = "";
                            string idotdel = "";
                            foreach (string s in split)
                            {
                                if (s.Trim() != "")
                                {
                                    for (int i = 0; i < departments.Rows.Count; i++)
                                    {
                                        if (Convert.ToInt32(departments.Rows[i]["id"]).ToString() == s)
                                        {
                                            Console.WriteLine(departments.Rows[i]["id"]);
                                            nameotdel += departments.Rows[i]["name"].ToString() + "/";
                                            idotdel += departments.Rows[i]["id"].ToString() + "/";
                                            departments.Rows[i].Delete();
                                            break;
                                        }
                                    }
                                }
                            }
                            idotdel = idotdel.Remove(idotdel.Length - 1, 1);
                            nameotdel = nameotdel.Remove(nameotdel.Length - 1, 1);


                            DataRow otdelrow = departments.NewRow();
                            otdelrow["id"] = split[0];
                            otdelrow["name"] = nameotdel;
                            departments.Rows.InsertAt(otdelrow, Convert.ToInt32(split[0]) - 1);
                        }
                    }
                    load_table = true;
                }

                DataRow firstRow = departments.NewRow();
                firstRow["id"] = -1;
                firstRow["name"] = "Все отделы";
                departments.Rows.InsertAt(firstRow, 0);
                departments.AcceptChanges();
            }
            return departments;
        }

        //получение списка ТУ групп
        public DataTable getTUgroups(int id_otdel)
        {
            ap.Clear();
            ap.Add(id_otdel);
            return sql.executeProcedure("CountRN.GetTUgroups", new string[] { "@id_otdel" }, new DbType[] { DbType.Int32 }, ap);
        }

        //получение даты основной инвентаризации
        public DataTable getLastInvDate()
        {
            ap.Clear();
            return sql.executeProcedure("CountRN.GetLastInvDate", new string[] { }, new DbType[] { }, ap);
        }

        //получение результатов расчёта РН
        public DataTable getPrices2(int id_otdel, DateTime date_start, DateTime date_finish, int days)
        {
            ap.Clear();
            ap.AddRange(new object[] { id_otdel, date_start, date_finish, days });
            return sql.executeProcedure("CountRN.CountRN2Days", new string[] { "@id_otdel", "@date_start", "@date_finish", "@days" }, new DbType[] { DbType.Int32, DbType.DateTime, DbType.DateTime, DbType.Int32 }, ap);
        }

        //получение результатов расчёта РН с учётом оптовых отгрузок
        public DataTable getPricesOpt(int id_otdel, DateTime date_start, DateTime date_finish, int days)
        {
            ap.Clear();
            ap.AddRange(new object[] { id_otdel, date_start, date_finish, days });
            return sql.executeProcedure("CountRN.CountRNOpt", new string[] { "@id_otdel", "@date_start", "@date_finish" }, new DbType[] { DbType.Int32, DbType.DateTime, DbType.DateTime }, ap);
        }

        #endregion

        private string GetSqlDateString(DateTime date)
        {
            return date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString() + " " + date.Hour.ToString() + ":" + date.Minute.ToString() + ":" + date.Second.ToString();
        }

        public DataTable GetTovars(int id_otdel, DateTime date_start, DateTime date_finish, bool withOptOtgruz, bool shipped)
        {
            ap.Clear();
            ap.AddRange(new object[] { id_otdel, date_start, date_finish });
            if (!withOptOtgruz)
                return sql.executeProcedure("rn.CountPrihodRealizInv", new string[] { "@id_otdel", "@date_start", "@date_finish" }, new DbType[] { DbType.Int32, DbType.DateTime, DbType.DateTime }, ap);
            else
            {
                ap.Add(shipped);
                return sql.executeProcedure("rn.CountPrihodRealizOptInv", new string[] { "@id_otdel", "@date_start", "@date_finish", "@shipped" }, new DbType[] { DbType.Int32, DbType.DateTime, DbType.DateTime, DbType.Boolean }, ap);
            }
        }
        public DataTable GetTovars_inv(int id_otdel, DateTime date_start, DateTime date_finish, bool withOptOtgruz, bool shipped)
        {
            ap.Clear();
            ap.AddRange(new object[] { id_otdel, date_start, date_finish });
            if (!withOptOtgruz)
                return sql.executeProcedure("rn.CountPrihodRealizInv_inv", new string[] { "@id_otdel", "@date_start", "@date_finish" }, new DbType[] { DbType.Int32, DbType.DateTime, DbType.DateTime }, ap);
            else
            {
                //test1
                ap.Add(shipped);
                return sql.executeProcedure("rn.CountPrihodRealizOptInv_inv", new string[] { "@id_otdel", "@date_start", "@date_finish", "@shipped" }, new DbType[] { DbType.Int32, DbType.DateTime, DbType.DateTime, DbType.Boolean }, ap);
            }
        }


        public DataTable GetPrihod(DateTime date,DateTime date_fix, int id_otdel)
        {
            ap.Clear();
            ap.AddRange(new object[] { date, date_fix, id_otdel });

            return sql.executeProcedure("rn.GetPrihod", new string[] { "@date", "@date_fix", "@id_otdel" }, new DbType[] { DbType.DateTime, DbType.DateTime, DbType.Int32 }, ap);
        }

        public decimal GetProcurementPrice(int id_tovar, DateTime date)
        {
            ap.Clear();
            ap.AddRange(new object[] { id_tovar, date });

            DataTable prices = sql.executeProcedure("rn.GetProcurementPrice", new string[] { "@id_tovar", "@date" }, new DbType[] { DbType.Int32, DbType.DateTime }, ap);
            if (prices != null && prices.Rows.Count > 0)
                return Convert.ToDecimal(prices.Rows[0][0]);
            else
                return 0;
        }

        public ArrayList GetGroups(int id_otdel)
        {
            ap.Clear();
            ap.Add(id_otdel);
            DataTable groupsTable = sql.executeProcedure("rn.GetGroups", new string[] { "@id_otdel" }, new DbType[] { DbType.Int32 }, ap);
            ArrayList groups = new ArrayList();
            if (groupsTable != null)
            {
                foreach (DataRow groupRow in groupsTable.Rows)
                    groups.Add(new Group(Convert.ToInt32(groupRow["id"]), groupRow["name"].ToString()));
            }
            return groups;
        }

        public ArrayList GetGroups_inv(int id_otdel)
        {
            ap.Clear();
            ap.Add(id_otdel);
            DataTable groupsTable = sql.executeProcedure("rn.GetGroups_inv", new string[] { "@id_otdel" }, new DbType[] { DbType.Int32 }, ap);
            ArrayList groups = new ArrayList();
            if (groupsTable != null)
            {
                foreach (DataRow groupRow in groupsTable.Rows)
                    groups.Add(new Group(Convert.ToInt32(groupRow["id"]), groupRow["name"].ToString()));
            }
            return groups;
        }

        public ArrayList GetDepartments()
        {
            ap.Clear();
            DataTable deps = sql.executeProcedure("rn.GetDepartments", new string[] { }, new DbType[] { }, ap);
            ArrayList departments = new ArrayList();
            if (deps != null)
            {
                foreach (DataRow depRow in deps.Rows)
                    departments.Add(new Department(Convert.ToInt32(depRow["id"]), depRow["name"].ToString()));
                departments.Insert(5, new Department(6, "ВВО"));
            }
            return departments;
        }
        
        //Граничная дата округления
        public string GetDateGrn()
        {
            ap.Clear();
            DataTable getDate = sql.executeProcedure("rn.getDateValue", new string[] { }, new DbType[] { }, ap);

            if (getDate != null && getDate.Rows.Count > 0)
                return Convert.ToString(getDate.Rows[0][0]);    
            else
                return "";           
        }
        
        //Кол-то дней после инвентаризации
        public string GetNumDate()
        {
            ap.Clear();
            DataTable getTimes = sql.executeProcedure("rn.getNumberValue", new string[] { }, new DbType[] { }, ap);

            if (getTimes != null && getTimes.Rows.Count > 0)
                return Convert.ToString(getTimes.Rows[0][0]);
            else
                return "";
        }
        public void SetNumDate(int numberDay)
        {
            ap.Clear();
            ap.AddRange(new object[] { numberDay });
            DataTable setNUmber = sql.executeProcedure("rn.setNumberValue", new string[] { "@newNumber" }, new DbType[] { DbType.Int32 }, ap);                    
        }
        public void SetDateGrn(string date, int id_pr)
        {
            ap.Clear();
            ap.AddRange(new object[] { date, id_pr });
            DataTable setDate = sql.executeProcedure("rn.setDateValue", new string[] { "@newDate", "@id_prog" }, new DbType[] { DbType.String, DbType.Int16 }, ap);
        }
        public void insertDateValue()
        {
            ap.Clear();
            sql.executeProcedure("rn.insertDateValue", new string[] { }, new DbType[] { }, ap);           
        }
        public void insertNumberValue()
        {
            ap.Clear();
            sql.executeProcedure("rn.insertNumberValue", new string[] { }, new DbType[] { }, ap);
        }

        public DataTable getDep()
        {
            ap.Clear();
            return sql.executeProcedure("rn.GetDepList", new string[] {}, new DbType[] {}, ap);
        }

        public DataTable getListOtdelGroup()
        {
            ap.Clear();
            ap.Add(ConnectionSettings.GetIdProgram());
            ap.Add(UserSettings.User.Id);            
            return sql.executeProcedure("rn.getListOtdelGroup", new string[] { "@id_prog", "@id_user" }, new DbType[] {DbType.Int32,DbType.Int32 }, ap);
        }

        public void setGroupeOtdel(string GroupList)
        {
            ap.Clear();
            ap.AddRange(new object[] { GroupList, ConnectionSettings.GetIdProgram(), UserSettings.User.Id });
            DataTable setDate = sql.executeProcedure("rn.setGroupeOtdel", new string[] { "@GroupList", "@id_prog", "@id_user" }, new DbType[] { DbType.String, DbType.Int16, DbType.Int16 }, ap);
        }

        public void deleteGroupeOtdel(string id)
        {
            ap.Clear();
            ap.Add(id);
            sql.executeProcedure("rn.deleteGroupeOtdel", new string[] { "@id" }, new DbType[] { DbType.String }, ap);
        }

        public void insertGroupeOtdel()
        {
            ap.Clear();
            ap.Add(ConnectionSettings.GetIdProgram());
            ap.Add(UserSettings.User.Id);            
            sql.executeProcedure("rn.insertGroupeOtdel", new string[] { "@id_prog", "@id_user" }, new DbType[] { DbType.Int32, DbType.Int32 }, ap);
        }



        public DataTable getDepartments_refresh()
        {
            DataTable departments = sql.executeProcedure("CountRN.GetDepartments", new string[] {}, new DbType[] {},
                new ArrayList());
            if (departments != null)
            {

                DataRow vvoRow = departments.NewRow();
                vvoRow["id"] = 6;
                vvoRow["name"] = "ВВО";
                departments.Rows.InsertAt(vvoRow, 5);


                DataTable checkerTable = new DataTable();
                if (UserSettings.User.StatusCode == "МН")
                {
                    checkerTable = getListOtdelGroup(0);
                }
                else
                {
                    checkerTable = getListOtdelGroup();
                }
                string tmpstr = "";
                foreach (DataRow row_1 in checkerTable.Rows)
                {
                    tmpstr = Convert.ToString(row_1["value"]);
                    string[] split = tmpstr.Split(new Char[] {';', '\t', '\n'});
//                MessageBox.Show(split[0].ToString());
                    string nameotdel = "";
                    string idotdel = "";
                    foreach (string s in split)
                    {
                        if (s.Trim() != "")
                        {
                            for (int i = 0; i < departments.Rows.Count; i++)
                            {
                                if (Convert.ToInt32(departments.Rows[i]["id"]).ToString() == s)
                                {
                                    Console.WriteLine(departments.Rows[i]["id"]);
                                    nameotdel += departments.Rows[i]["name"].ToString() + "/";
                                    idotdel += departments.Rows[i]["id"].ToString() + "/";
                                    departments.Rows[i].Delete();
                                    departments.AcceptChanges();
                                    break;
                                }
                            }
                        }
                    }
                    // idotdel = idotdel.Remove(idotdel.Length - 1, 1);
                    nameotdel = nameotdel.Remove(nameotdel.Length - 1, 1);
                    DataRow otdelrow = departments.NewRow();
                    otdelrow["id"] = split[0];
                    otdelrow["name"] = nameotdel;
                    departments.Rows.InsertAt(otdelrow, Convert.ToInt32(split[0]) - 1);
                    departments.AcceptChanges();
                    load_table = true;
                }


                DataRow firstRow = departments.NewRow();
                firstRow["id"] = -1;
                firstRow["name"] = "Все отделы";
                departments.Rows.InsertAt(firstRow, 0);
                departments.AcceptChanges();
            }
            return departments;
        }

        public DataTable getGroupGroup(int id_dep)
        {
            ap.Clear();
            ap.Add(ConnectionSettings.GetIdProgram());
            ap.Add(UserSettings.User.Id);
            ap.Add(id_dep);
            return sql.executeProcedure("rn.getGroupGroup", new string[] { "@id_pro", "@id_user", "@id_dep" }, new DbType[] { DbType.Int32, DbType.Int32, DbType.Int32 }, ap);
        }

        public void deleteGroupGroup(int id)
        {
            ap.Clear();            
            ap.Add(id);
            sql.executeProcedure("rn.deleteGroupGroup", new string[] { "@id" }, new DbType[] { DbType.Int32 }, ap);
        }

        public void insertGroupeGroupe(int id_dep, string val)
        {
            ap.Clear();
            ap.Add(ConnectionSettings.GetIdProgram());
            ap.Add(UserSettings.User.Id);
            ap.Add(id_dep);
            ap.Add(val);
            sql.executeProcedure("rn.insertGroupeGroupe", new string[] { "@id_pro", "@id_user", "@id_dep","@val" }, new DbType[] { DbType.Int32, DbType.Int32, DbType.Int32,DbType.String }, ap);
        }

        public DataTable getListOtdelGroup(int id)
        {
            ap.Clear();
            ap.Add(ConnectionSettings.GetIdProgram());
            ap.Add(id);
            return sql.executeProcedure("rn.getListOtdelGroup", new string[] { "@id_prog", "@id_user" }, new DbType[] { DbType.Int32, DbType.Int32 }, ap);
        }

        //NEW
        public DataTable getGroups(int id_otdel)
        {
            ap.Clear();
            ap.Add(id_otdel);
            DataTable dt=  sql.executeProcedure("rn.GetGroups", new string[] { "@id_otdel" }, new DbType[] { DbType.Int32 }, ap);
            DataRow firstRow = dt.NewRow();
            firstRow["id"] = -1;
            firstRow["name"] = "Все группы";
            dt.Rows.InsertAt(firstRow, 0);
            dt.AcceptChanges();
            return dt;
        }

        public DataTable getGroups_inv(int id_otdel)
        {
            ap.Clear();
            ap.Add(id_otdel);
            DataTable dt = sql.executeProcedure("rn.GetGroups_inv", new string[] { "@id_otdel" }, new DbType[] { DbType.Int32 }, ap);
            DataRow firstRow = dt.NewRow();
            firstRow["id"] = -1;
            firstRow["name"] = "Все группы";
            dt.Rows.InsertAt(firstRow, 0);
            dt.AcceptChanges();
            return dt;
        }

        public DataTable getInventPeriod(DateTime dateStart,DateTime dateStop)
        {
            ap.Clear();
            ap.Add(dateStart);
            ap.Add(dateStop);
            return sql.executeProcedure("CountRN.spg_getInventPeriod",
                new string[2] { "@dateStart", "@dateStop" },
                new DbType[2] { DbType.Date,DbType.Date }, ap);
        }

        public DataTable getTovarDataToSaveRN(int id_tovar, DateTime dateStart, DateTime dateStop)
        {
            ap.Clear();
            
            ap.Add(id_tovar);
            ap.Add(dateStart);
            ap.Add(dateStop);
            return sql.executeProcedure("CountRN.spg_getTovarDataToSaveRN",
                new string[3] {"@id_tovar", "@dateStart", "@dateStop" },
                new DbType[3] { DbType.Int32, DbType.Date, DbType.Date }, ap);
        }

        public DataTable setTSaveRN(DateTime DateStart, DateTime DateEnd,bool isOptOtgruz,bool isOnlyShipped,bool isInventorySpis,decimal TotalPrihod, decimal TotalRealiz, decimal TotalRestStart, decimal TotalRestStop, decimal TotalRN, decimal TotalPercentRN)
        {

            ap.Clear();

            ap.Add(DateStart);
            ap.Add(DateEnd);
            ap.Add(isOptOtgruz);
            ap.Add(isOnlyShipped);
            ap.Add(isInventorySpis);
            ap.Add(TotalPrihod);
            ap.Add(TotalRealiz);
            ap.Add(TotalRestStart);
            ap.Add(TotalRestStop);
            ap.Add(TotalRN);
            ap.Add(TotalPercentRN);

            ap.Add(UserSettings.User.Id);

            return sql.executeProcedure("CountRN.spg_setTSaveRN",
               new string[12] { "@DateStart", "@DateEnd", "@isOptOtgruz", "@isOnlyShipped", "@isInventorySpis", "@TotalPrihod", "@TotalRealiz", "@TotalRestStart", "@TotalRestStop", "@TotalRN", "@TotalPercentRN", "@id_user" },
               new DbType[12] { DbType.Date, DbType.Date, DbType.Boolean, DbType.Boolean, DbType.Boolean, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Int32 }, ap);
        }

        public DataTable validateTSaveRN(DateTime DateStart, DateTime DateEnd, bool isOptOtgruz, bool isOnlyShipped, bool isInventorySpis)
        {
            ap.Clear();

            ap.Add(DateStart);
            ap.Add(DateEnd);
            ap.Add(isOptOtgruz);
            ap.Add(isOnlyShipped);
            ap.Add(isInventorySpis);

            return sql.executeProcedure("CountRN.spg_validateTSaveRN",
               new string[5] { "@DateStart", "@DateEnd", "@isOptOtgruz", "@isOnlyShipped", "@isInventorySpis", },
               new DbType[5] { DbType.Date, DbType.Date, DbType.Boolean, DbType.Boolean, DbType.Boolean }, ap);
        }


        public DataTable setSaveRN(int id_tSaveRN,int  id_tovar,int  id_department,int  id_grp1,int  id_grp2, decimal RestStart, decimal RestStartSum, decimal RestStop, decimal RestStopSum, decimal Prihod, decimal PrihodSum, decimal Otgruz, decimal OtgruzSum, decimal Vozvr, decimal VozvrSum, decimal Spis, decimal SpisSum, decimal InventSpis, decimal InventSpisSum, decimal Realiz, decimal RealizSum, decimal OtgruzOpt, decimal OtgruzOptSum, decimal VozvrKass, decimal VozvrKassSum,bool isDel)
        {

            ap.Clear();

            ap.Add(id_tSaveRN);
            ap.Add(id_tovar);
            ap.Add(id_department);
            ap.Add(id_grp1);
            ap.Add(id_grp2);
            ap.Add(RestStart);
            ap.Add(RestStartSum);
            ap.Add(RestStop);
            ap.Add(RestStopSum);
            ap.Add(Prihod);
            ap.Add(PrihodSum);
            ap.Add(Otgruz);
            ap.Add(OtgruzSum);
            ap.Add(Vozvr); 
            ap.Add(VozvrSum); 
            ap.Add(Spis);
            ap.Add(SpisSum);
            ap.Add(InventSpis);
            ap.Add(InventSpisSum); 
            ap.Add(Realiz);
            ap.Add(RealizSum);
            ap.Add(OtgruzOpt);
            ap.Add(OtgruzOptSum);
            ap.Add(VozvrKass);
            ap.Add(VozvrKassSum);
            ap.Add(isDel);

            return sql.executeProcedure("CountRN.spg_setSaveRN",
               new string[26] { "@id_tSaveRN", "@id_tovar", "@id_department", "@id_grp1", "@id_grp2", "@RestStart", "@RestStartSum", "@RestStop", "@RestStopSum", "@Prihod", "@PrihodSum", "@Otgruz", "@OtgruzSum", "@Vozvr", "@VozvrSum", "@Spis", "@SpisSum", "@InventSpis", "@InventSpisSum", "@Realiz", "@RealizSum", "@OtgruzOpt", "@OtgruzOptSum", "@VozvrKass", "@VozvrKassSum", "@isDel" },
               new DbType[26] { DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Boolean }, ap);
        }
    }
}
