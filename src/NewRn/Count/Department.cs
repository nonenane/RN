using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using NewRn.Data;
using Nwuram.Framework.Settings.Connection;

namespace NewRn
{
    class Department
    {

        #region Properties

        private int id = 0; //идентификатор отдела
        public int Id
        {
            get { return id; }
        }

        private string name = ""; //название отдела
        public string Name
        {
            get { return name; }
        }

        private DataTable rn = null;
        private Sql sql = new Sql(ConnectionSettings.GetServer(), ConnectionSettings.GetUsername(),
                    ConnectionSettings.GetPassword(), ConnectionSettings.GetDatabase(), ConnectionSettings.ProgramName);

        public decimal RN
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(rn)", ""));
                else
                    return 0;
            }
        }

        public decimal RealizAll
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(realiz_all)", ""));
                else
                    return 0;
            }
        }

        public decimal R1
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(r1)", ""));
                else
                    return 0;
            }
        }
        public decimal R2
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(r2)", ""));
                else
                    return 0;
            }
        }
        public decimal Realiz
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(realiz)", ""));
                else
                    return 0;
            }
        }

        public decimal RealizOpt
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(realiz_opt)", ""));
                else
                    return 0;
            }
        }

        public decimal Vozvkass
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(vozvkass)", ""));
                else
                    return 0;
            }
        }

        public decimal PrihodAll
        {
            get
            {


                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(prihod_all)", ""));
                else
                    return 0;
            }
        }

        public decimal Prihod
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(prihod)", ""));
                else
                    return 0;
            }
        }

        public decimal Otgruz
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(otgruz)", ""));
                else
                    return 0;
            }
        }

        public decimal Vozvr
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(vozvr)", ""));
                else
                    return 0;
            }
        }

        public decimal Spis
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(spis)", ""));
                else
                    return 0;
            }
        }

        public decimal SpisInv
        {
            get
            {
                if (rn != null && rn.Rows.Count > 0)
                    return Convert.ToDecimal(rn.Compute("Sum(spis_inv)", ""));
                else
                    return 0;
            }
        }

        public decimal Procent
        {
            get
            {
                if (RealizAll != 0)
                    return Math.Round(RN * 100 / RealizAll, 2);
                else
                    return 0;
            }
        }

        private DataTable MakeGroupsTable()
        {
            DataTable table = new DataTable();
            //table.Columns.Add("id", typeof(int));
            table.Columns.Add("id", typeof(string));
            table.Columns.Add("cname", typeof(string));
            table.Columns.Add("prihod_all", typeof(double));
            table.Columns.Add("prihod", typeof(double));
            table.Columns.Add("otgruz", typeof(double));
            table.Columns.Add("vozvr", typeof(double));
            table.Columns.Add("spis", typeof(double));
            table.Columns.Add("spis_inv", typeof(double));
            table.Columns.Add("realiz_all", typeof(double));
            table.Columns.Add("realiz", typeof(double));
            table.Columns.Add("realiz_opt", typeof(double));
            table.Columns.Add("vozvkass", typeof(double));
            table.Columns.Add("rn", typeof(double));
            table.Columns.Add("r1", typeof(double));
            table.Columns.Add("r2", typeof(double));
            table.Columns.Add("procent", typeof(double));
            table.Columns.Add("id_otdel", typeof(int));

            //table.Columns.Add("id", typeof(int));
            //table.Columns.Add("cname", typeof(string));
            //table.Columns.Add("prihod_all", typeof(decimal));
            //table.Columns.Add("prihod", typeof(decimal));
            //table.Columns.Add("otgruz", typeof(decimal));
            //table.Columns.Add("vozvr", typeof(decimal));
            //table.Columns.Add("spis", typeof(decimal));
            //table.Columns.Add("spis_inv", typeof(decimal));
            //table.Columns.Add("realiz_all", typeof(decimal));
            //table.Columns.Add("realiz", typeof(decimal));
            //table.Columns.Add("realiz_opt", typeof(decimal));
            //table.Columns.Add("vozvkass", typeof(decimal));
            //table.Columns.Add("rn", typeof(decimal));
            //table.Columns.Add("r1", typeof(decimal));
            //table.Columns.Add("r2", typeof(decimal));
            //table.Columns.Add("procent", typeof(decimal));
            return table;
        }

        private decimal GetValueForGroup(int id_group, string columnName)
        {
            decimal value = 0;
            try
            {
                value = Convert.ToDecimal(rn.Compute("Sum(" + columnName + ")", "id_grp1 = '" + id_group + "'"));
            }
            catch { }
            return value;
        }

        private decimal GetValueForGroup_inv(int id_group, string columnName)
        {
            decimal value = 0;
            try
            {
                value = Convert.ToDecimal(rn.Compute("Sum(" + columnName + ")", "id_grp2 = '" + id_group + "'"));
            }
            catch { }
            return value;
        }

        private IDataWorker dataWorker = null;
        private DataRow groupRow;

        public DataTable Groups
        {
            get
            {
                DataTable groupsRN = MakeGroupsTable();
                if (dataWorker != null)
                {
                    ArrayList groups = dataWorker.GetGroups(this.id);
                    foreach (Group group in groups)
                    {
                        groupRow = groupsRN.NewRow();
                        groupRow["id"] = group.Id;
                        groupRow["cname"] = group.Name;

                        groupRow["prihod_all"] = Math.Round(GetValueForGroup(group.Id, "prihod_all"), 2);
                        groupRow["prihod"] = Math.Round(GetValueForGroup(group.Id, "prihod"), 2);
                        groupRow["otgruz"] = Math.Round(GetValueForGroup(group.Id, "otgruz"), 2);
                        groupRow["vozvr"] = Math.Round(GetValueForGroup(group.Id, "vozvr"), 2);
                        groupRow["spis"] = Math.Round(GetValueForGroup(group.Id, "spis"), 2);
                        groupRow["spis_inv"] = Math.Round(GetValueForGroup(group.Id, "spis_inv"), 2);
                        groupRow["realiz_all"] = Math.Round(GetValueForGroup(group.Id, "realiz_all"), 2);
                        groupRow["realiz"] = Math.Round(GetValueForGroup(group.Id, "realiz"), 2);
                        groupRow["realiz_opt"] = Math.Round(GetValueForGroup(group.Id, "realiz_opt"), 2);
                        groupRow["vozvkass"] = Math.Round(GetValueForGroup(group.Id, "vozvkass"), 2);
                        groupRow["rn"] = Math.Round(GetValueForGroup(group.Id, "rn"), 2);

                        groupRow["r1"] = Math.Round(GetValueForGroup(group.Id, "r1"), 2);
                        groupRow["r2"] = Math.Round(GetValueForGroup(group.Id, "r2"), 2);

                        if (GetValueForGroup(group.Id, "realiz_all") != 0)
                            groupRow["procent"] = Math.Round(Convert.ToDecimal(GetValueForGroup(group.Id, "rn") * 100 / GetValueForGroup(group.Id, "realiz_all")), 2);
                        else
                            groupRow["procent"] = 0;
                        groupRow["id_otdel"] = this.id;
                        groupsRN.Rows.Add(groupRow);
                    }
                    //DataTable dtGroupDataTable = dataWorker.getGroupGroup(this.id);
                    DataTable dtGroupDataTable = sql.getGroupGroup(this.id);
                    if (dtGroupDataTable.Rows.Count > 0)
                    {
                        Console.WriteLine("rtrt");

                        string tmpstr = "";
                        string nameotdel = "";
                        string idotdel = "";



                        decimal prihod_all
                        , prihod
                        , otgruz
                        , vozvr
                        , spis
                        , spis_inv
                        , realiz_all
                        , realiz
                        , realiz_opt
                        , vozvkass
                        , rn
                        , r1
                        , r2
                        , procent;



                        foreach (DataRow row in dtGroupDataTable.Rows)
                        {
                            nameotdel = "";
                            idotdel = "";
                            tmpstr = Convert.ToString(row["val"]);
                            prihod_all = 0;
                            prihod = 0;
                            otgruz = 0;
                            vozvr = 0;
                            spis = 0;
                            spis_inv = 0;
                            realiz_all = 0;
                            realiz = 0;
                            realiz_opt = 0;
                            vozvkass = 0;
                            rn = 0;
                            r1 = 0;
                            r2 = 0;
                            procent = 0;


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
                                        prihod_all += Convert.ToDecimal(f["prihod_all"]);
                                        prihod += Convert.ToDecimal(f["prihod"]);
                                        otgruz += Convert.ToDecimal(f["otgruz"]);
                                        vozvr += Convert.ToDecimal(f["vozvr"]);
                                        spis += Convert.ToDecimal(f["spis"]);
                                        spis_inv += Convert.ToDecimal(f["spis_inv"]);
                                        realiz_all += Convert.ToDecimal(f["realiz_all"]);
                                        realiz += Convert.ToDecimal(f["realiz"]);
                                        realiz_opt += Convert.ToDecimal(f["realiz_opt"]);
                                        vozvkass += Convert.ToDecimal(f["vozvkass"]);
                                        rn += Convert.ToDecimal(f["rn"]);
                                        r1 += Convert.ToDecimal(f["r1"]);
                                        r2 += Convert.ToDecimal(f["r2"]);
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
                            otdelrow["id"] = idotdel;
                            //otdelrow["id"] = Convert.ToInt16(split[0]);
                            otdelrow["cname"] = nameotdel;
                            otdelrow["id_otdel"] = this.id;
                            // otdelrow["dep"] = Id;
                            //otdelrow["gcn"] = idotdel;
                            // otdelrow["chk"] = true;
                            otdelrow["prihod_all"] = prihod_all;
                            otdelrow["prihod"] = prihod;
                            otdelrow["otgruz"] = otgruz;
                            otdelrow["vozvr"] = vozvr;
                            otdelrow["spis"] = spis;
                            otdelrow["spis_inv"] = spis_inv;
                            otdelrow["realiz_all"] = realiz_all;
                            otdelrow["realiz"] = realiz;
                            otdelrow["realiz_opt"] = realiz_opt;
                            otdelrow["vozvkass"] = vozvkass;
                            otdelrow["rn"] = rn;
                            otdelrow["r1"] = r1;
                            otdelrow["r2"] = r2;

                            if (realiz_all != 0)
                                otdelrow["procent"] = Math.Round(Convert.ToDecimal(rn * 100 / realiz_all), 2);
                            else
                                otdelrow["procent"] = 0;
                            groupsRN.Rows.InsertAt(otdelrow, Convert.ToInt32(split[0]) - 1);
                            groupsRN.AcceptChanges();
                            groupsRN.DefaultView.Sort = "id asc";
                            groupsRN = groupsRN.DefaultView.ToTable(true);
                        }
                        Console.WriteLine("rtrt");
                    }
                }
                return groupsRN;
            }
        }

        //test2
        public DataTable Groups_inv
        {
            get
            {
                DataTable groupsRN = MakeGroupsTable();
                if (dataWorker != null)
                {
                    ArrayList groups = dataWorker.GetGroups_inv(this.id);
                    foreach (Group group in groups)
                    {
                        groupRow = groupsRN.NewRow();
                        groupRow["id"] = group.Id;
                        groupRow["cname"] = group.Name;

                        groupRow["prihod_all"] = Math.Round(GetValueForGroup_inv(group.Id, "prihod_all"), 2);
                        groupRow["prihod"] = Math.Round(GetValueForGroup_inv(group.Id, "prihod"), 2);
                        groupRow["otgruz"] = Math.Round(GetValueForGroup_inv(group.Id, "otgruz"), 2);
                        groupRow["vozvr"] = Math.Round(GetValueForGroup_inv(group.Id, "vozvr"), 2);
                        groupRow["spis"] = Math.Round(GetValueForGroup_inv(group.Id, "spis"), 2);
                        groupRow["spis_inv"] = Math.Round(GetValueForGroup_inv(group.Id, "spis_inv"), 2);
                        groupRow["realiz_all"] = Math.Round(GetValueForGroup_inv(group.Id, "realiz_all"), 2);
                        groupRow["realiz"] = Math.Round(GetValueForGroup_inv(group.Id, "realiz"), 2);
                        groupRow["realiz_opt"] = Math.Round(GetValueForGroup_inv(group.Id, "realiz_opt"), 2);
                        groupRow["vozvkass"] = Math.Round(GetValueForGroup_inv(group.Id, "vozvkass"), 2);
                        groupRow["rn"] = Math.Round(GetValueForGroup_inv(group.Id, "rn"), 2);

                        groupRow["r1"] = Math.Round(GetValueForGroup_inv(group.Id, "r1"), 2);
                        groupRow["r2"] = Math.Round(GetValueForGroup_inv(group.Id, "r2"), 2);

                        if (GetValueForGroup_inv(group.Id, "realiz_all") != 0)
                            groupRow["procent"] = Math.Round(Convert.ToDecimal(GetValueForGroup_inv(group.Id, "rn") * 100 / GetValueForGroup_inv(group.Id, "realiz_all")), 2);
                        else
                            groupRow["procent"] = 0;
                        groupRow["id_otdel"] = this.id;
                        groupsRN.Rows.Add(groupRow);
                    }
                    //DataTable dtGroupDataTable = dataWorker.getGroupGroup(this.id);
                    //DataTable dtGroupDataTable = sql.getGroupGroup(this.id);
                    //if (dtGroupDataTable.Rows.Count > 0)
                    //{
                    //    Console.WriteLine("rtrt");

                    //    string tmpstr = "";
                    //    string nameotdel = "";
                    //    string idotdel = "";

                    //    decimal prihod_all
                    //    , prihod
                    //    , otgruz
                    //    , vozvr
                    //    , spis
                    //    , spis_inv
                    //    , realiz_all
                    //    , realiz
                    //    , realiz_opt
                    //    , vozvkass
                    //    , rn
                    //    , r1
                    //    , r2
                    //    , procent;



                    //    foreach (DataRow row in dtGroupDataTable.Rows)
                    //    {
                    //        nameotdel = "";
                    //        idotdel = "";
                    //        tmpstr = Convert.ToString(row["val"]);
                    //        prihod_all = 0;
                    //        prihod = 0;
                    //        otgruz = 0;
                    //        vozvr = 0;
                    //        spis = 0;
                    //        spis_inv = 0;
                    //        realiz_all = 0;
                    //        realiz = 0;
                    //        realiz_opt = 0;
                    //        vozvkass = 0;
                    //        rn = 0;
                    //        r1 = 0;
                    //        r2 = 0;
                    //        procent = 0;


                    //        string[] split = tmpstr.Split(new Char[] { ',', '\t', '\n' });
                    //        for (int j = 0; j < split.Length; j++)
                    //        {
                    //            Console.WriteLine(split[j]);
                    //            foreach (DataRow f in groupsRN.Rows)
                    //            {
                    //                if (split[j] == f["id"].ToString())
                    //                {
                    //                    //f["chk"] = true;
                    //                    nameotdel += f["cname"] + "/";
                    //                    idotdel += f["id"] + "/";
                    //                    prihod_all += Convert.ToDecimal(f["prihod_all"]);
                    //                    prihod += Convert.ToDecimal(f["prihod"]);
                    //                    otgruz += Convert.ToDecimal(f["otgruz"]);
                    //                    vozvr += Convert.ToDecimal(f["vozvr"]);
                    //                    spis += Convert.ToDecimal(f["spis"]);
                    //                    spis_inv += Convert.ToDecimal(f["spis_inv"]);
                    //                    realiz_all += Convert.ToDecimal(f["realiz_all"]);
                    //                    realiz += Convert.ToDecimal(f["realiz"]);
                    //                    realiz_opt += Convert.ToDecimal(f["realiz_opt"]);
                    //                    vozvkass += Convert.ToDecimal(f["vozvkass"]);
                    //                    rn += Convert.ToDecimal(f["rn"]);
                    //                    r1 += Convert.ToDecimal(f["r1"]);
                    //                    r2 += Convert.ToDecimal(f["r2"]);
                    //                    f.Delete();
                    //                    break;
                    //                }
                    //            }
                    //            groupsRN.AcceptChanges();
                    //        }
                    //        //MessageBox.Show(nameotdel);
                    //        nameotdel = nameotdel.Remove(nameotdel.Length - 1, 1);
                    //        idotdel = idotdel.Remove(idotdel.Length - 1, 1);
                    //        DataRow otdelrow = groupsRN.NewRow();
                    //        otdelrow["id"] = idotdel;
                    //        //otdelrow["id"] = Convert.ToInt16(split[0]);
                    //        otdelrow["cname"] = nameotdel;
                    //        otdelrow["id_otdel"] = this.id;
                    //        // otdelrow["dep"] = Id;
                    //        //otdelrow["gcn"] = idotdel;
                    //        // otdelrow["chk"] = true;
                    //        otdelrow["prihod_all"] = prihod_all;
                    //        otdelrow["prihod"] = prihod;
                    //        otdelrow["otgruz"] = otgruz;
                    //        otdelrow["vozvr"] = vozvr;
                    //        otdelrow["spis"] = spis;
                    //        otdelrow["spis_inv"] = spis_inv;
                    //        otdelrow["realiz_all"] = realiz_all;
                    //        otdelrow["realiz"] = realiz;
                    //        otdelrow["realiz_opt"] = realiz_opt;
                    //        otdelrow["vozvkass"] = vozvkass;
                    //        otdelrow["rn"] = rn;
                    //        otdelrow["r1"] = r1;
                    //        otdelrow["r2"] = r2;

                    //        if (realiz_all != 0)
                    //            otdelrow["procent"] = Math.Round(Convert.ToDecimal(rn * 100 / realiz_all), 2);
                    //        else
                    //            otdelrow["procent"] = 0;
                    //        groupsRN.Rows.InsertAt(otdelrow, Convert.ToInt32(split[0]) - 1);
                    //        groupsRN.AcceptChanges();
                    //        groupsRN.DefaultView.Sort = "id asc";
                    //        groupsRN = groupsRN.DefaultView.ToTable(true);
                    //    }
                    //    Console.WriteLine("rtrt");
                    //}
                }
                return groupsRN;
            }
        }

        #endregion

        private DataTable MakeGoodsTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ean", typeof(string));
            table.Columns.Add("cname", typeof(string));
            table.Columns.Add("prihod_all", typeof(double));
            table.Columns.Add("prihod", typeof(double));
            table.Columns.Add("otgruz", typeof(double));
            table.Columns.Add("vozvr", typeof(double));
            table.Columns.Add("spis", typeof(double));
            table.Columns.Add("spis_inv", typeof(double));
            table.Columns.Add("realiz_all", typeof(double));
            table.Columns.Add("realiz", typeof(double));
            table.Columns.Add("realiz_opt", typeof(double));
            table.Columns.Add("vozvkass", typeof(double));
            table.Columns.Add("rn", typeof(double));

            table.Columns.Add("r1", typeof(double));
            table.Columns.Add("r2", typeof(double));
            // table.Columns.Add("id_otdel", typeof(int));

            //table.Columns.Add("ean", typeof(string));
            //table.Columns.Add("cname", typeof(string));
            //table.Columns.Add("prihod_all", typeof(decimal));
            //table.Columns.Add("prihod", typeof(decimal));
            //table.Columns.Add("otgruz", typeof(decimal));
            //table.Columns.Add("vozvr", typeof(decimal));
            //table.Columns.Add("spis", typeof(decimal));
            //table.Columns.Add("spis_inv", typeof(decimal));
            //table.Columns.Add("realiz_all", typeof(decimal));
            //table.Columns.Add("realiz", typeof(decimal));
            //table.Columns.Add("realiz_opt", typeof(decimal));
            //table.Columns.Add("vozvkass", typeof(decimal));
            //table.Columns.Add("rn", typeof(decimal));

            //table.Columns.Add("r1", typeof(decimal));
            //table.Columns.Add("r2", typeof(decimal));
            return table;
        }

        private DataRow goodRow;
        public DataTable GetGoods(int id_group)
        {
            DataTable goods = MakeGoodsTable();
            DataRow[] group = rn.Select("id_grp1 = '" + id_group.ToString() + "'");
            foreach (DataRow groupRow in group)
            {
                goodRow = goods.NewRow();
                goodRow["ean"] = groupRow["ean"];
                goodRow["cname"] = groupRow["cname"];
                goodRow["prihod_all"] = groupRow["prihod_all"];
                goodRow["prihod"] = groupRow["prihod"];
                goodRow["otgruz"] = groupRow["otgruz"];
                goodRow["vozvr"] = groupRow["vozvr"];
                goodRow["spis"] = groupRow["spis"];
                goodRow["spis_inv"] = groupRow["spis_inv"];
                goodRow["realiz_all"] = groupRow["realiz_all"];
                goodRow["realiz"] = groupRow["realiz"];
                goodRow["realiz_opt"] = groupRow["realiz_opt"];
                goodRow["vozvkass"] = groupRow["vozvkass"];
                goodRow["rn"] = groupRow["rn"];
                goodRow["r1"] = groupRow["r1"];
                goodRow["r2"] = groupRow["r2"];
                // goodRow["id_otdel"] = groupRow["id_otdel"];
                goods.Rows.Add(goodRow);
            }
            return goods;
        }

        public DataTable GetGoods_inv(int id_group)
        {
            DataTable goods = MakeGoodsTable();
            DataRow[] group = rn.Select("id_grp2 = '" + id_group.ToString() + "'");

            foreach (DataRow groupRow in group)
            {
                goodRow = goods.NewRow();
                goodRow["ean"] = groupRow["ean"];
                goodRow["cname"] = groupRow["cname"];
                goodRow["prihod_all"] = groupRow["prihod_all"];
                goodRow["prihod"] = groupRow["prihod"];
                goodRow["otgruz"] = groupRow["otgruz"];
                goodRow["vozvr"] = groupRow["vozvr"];
                goodRow["spis"] = groupRow["spis"];
                goodRow["spis_inv"] = groupRow["spis_inv"];
                goodRow["realiz_all"] = groupRow["realiz_all"];
                goodRow["realiz"] = groupRow["realiz"];
                goodRow["realiz_opt"] = groupRow["realiz_opt"];
                goodRow["vozvkass"] = groupRow["vozvkass"];
                goodRow["rn"] = groupRow["rn"];
                goodRow["r1"] = groupRow["r1"];
                goodRow["r2"] = groupRow["r2"];
                // goodRow["id_otdel"] = groupRow["id_otdel"];
                goods.Rows.Add(goodRow);
            }
            return goods;
        }

        public DataTable GetAllGoods()
        {
            return rn;
        }

        #region Constructor

        public Department(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        #endregion

        #region Methods

        public void CountRN(DateTime dateStart, DateTime dateFinish, bool withOptOtgruz, bool shipped, IDataWorker dataWorker)
        {
            this.dataWorker = dataWorker;
            //получить товары по отделу за период
            DataTable goods = dataWorker.GetTovars(this.id, dateStart, dateFinish, withOptOtgruz, shipped);
            //приходы на начало
            DataTable prihodStart = dataWorker.GetPrihod(dateStart.AddSeconds(-1), dateStart.AddSeconds(0), this.id);
            //приходы на конец
            DataTable prihodFinish = dataWorker.GetPrihod(dateFinish.AddDays(1).AddSeconds(-1), dateFinish.AddDays(1).AddSeconds(-1), this.id);
            //получить остатки на начало
            DataTable rems1 = Count.CountRemains(goods, prihodStart, "kol1", dataWorker, dateStart);
            //получить остатки на конец
            DataTable rems2 = Count.CountRemains(goods, prihodFinish, "kol2", dataWorker, dateFinish.AddDays(1));
            //посчитать РН по отделу
            rn = Count.CountRN(goods, rems1, rems2,this.id);
        }
        //test2
        public void CountRN_inv(DateTime dateStart, DateTime dateFinish, bool withOptOtgruz, bool shipped, IDataWorker dataWorker)
        {
            this.dataWorker = dataWorker;
            //получить товары по отделу за период
            DataTable goods = dataWorker.GetTovars_inv(this.id, dateStart, dateFinish, withOptOtgruz, shipped);
            //приходы на начало
            DataTable prihodStart = dataWorker.GetPrihod(dateStart.AddSeconds(-1), dateStart.AddSeconds(0), this.id);
            //приходы на конец
            DataTable prihodFinish = dataWorker.GetPrihod(dateFinish.AddDays(1).AddSeconds(-1), dateFinish.AddDays(1).AddSeconds(-1), this.id);
            //получить остатки на начало
            DataTable rems1 = Count.CountRemains_inv(goods, prihodStart, "kol1", dataWorker, dateStart);
            //получить остатки на конец
            DataTable rems2 = Count.CountRemains_inv(goods, prihodFinish, "kol2", dataWorker, dateFinish.AddDays(1));
            //посчитать РН по отделу
            rn = Count.CountRN_inv(goods, rems1, rems2, this.id);
        }


        public void CountRN(int id, DateTime dateStart, DateTime dateFinish, DataTable prihodStart, DataTable prihodFinish, bool withOptOtgruz, bool shipped, IDataWorker dataWorker)
        {
            this.dataWorker = dataWorker;
            //получить товары по отделу за период
            DataTable goods = dataWorker.GetTovars(this.id, dateStart, dateFinish, withOptOtgruz, shipped);
            //приходы на начало
            DataTable prihodStart1 = dataWorker.GetPrihod(dateStart.AddSeconds(-1), dateStart.AddSeconds(0), this.id);
            //приходы на конец
            DataTable prihodFinish1 = dataWorker.GetPrihod(dateFinish.AddDays(1).AddSeconds(-1), dateFinish.AddDays(1).AddSeconds(-1), this.id);
            
            //получить остатки на начало
            DataTable rems1 = Count.CountRemains(goods, prihodStart1, "kol1", dataWorker, dateStart);
            //получить остатки на конец
            DataTable rems2 = Count.CountRemains(goods, prihodFinish1, "kol2", dataWorker, dateFinish.AddDays(1));
            //посчитать РН по отделу
            rn = Count.CountRN(goods, rems1, rems2, this.id);
        }

        public void CountRN_inv(int id, DateTime dateStart, DateTime dateFinish, DataTable prihodStart, DataTable prihodFinish, bool withOptOtgruz, bool shipped, IDataWorker dataWorker)
        {
            this.dataWorker = dataWorker;
            //получить товары по отделу за период
            DataTable goods = dataWorker.GetTovars_inv(this.id, dateStart, dateFinish, withOptOtgruz, shipped);
            //приходы на начало
            DataTable prihodStart1 = dataWorker.GetPrihod(dateStart.AddSeconds(-1), dateStart.AddSeconds(0), this.id);
            //приходы на конец
            DataTable prihodFinish1 = dataWorker.GetPrihod(dateFinish.AddDays(1).AddSeconds(-1), dateFinish.AddDays(1).AddSeconds(-1), this.id);

            //получить остатки на начало
            DataTable rems1 = Count.CountRemains_inv(goods, prihodStart1, "kol1", dataWorker, dateStart);
            //получить остатки на конец
            DataTable rems2 = Count.CountRemains_inv(goods, prihodFinish1, "kol2", dataWorker, dateFinish.AddDays(1));
            //посчитать РН по отделу
            rn = Count.CountRN_inv(goods, rems1, rems2, this.id);
        }

        #endregion
    }
}
