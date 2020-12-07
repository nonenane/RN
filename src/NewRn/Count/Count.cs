using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NewRn
{
    class Count
    {
        public static DataTable CountRemains(DataTable goods, DataTable prihod, string kolColumnName, IDataWorker sql, DateTime datePrice)
        {
            DataTable remains = new DataTable();
            remains.Columns.Add("id_tovar");
            remains.Columns.Add(new DataColumn("remains", typeof(decimal)));
            remains.Columns.Add("id_grp1"); //debug
            remains.Columns.Add("kol"); //debug
            remains.Columns.Add("ean"); //debug
            remains.Columns.Add("zcena"); //debug

            foreach (DataRow goodsRow in goods.Rows)
            {
                int id_tovar = Convert.ToInt32(goodsRow["id"]);
                DataRow[] prihodRows = prihod.Select("id_tovar = " + id_tovar.ToString(), "dprihod desc, id desc");
                decimal kol = Convert.ToDecimal(goodsRow[kolColumnName]);
                decimal abskol = Math.Abs(kol);

                DataRow remainsRow = remains.NewRow();

                decimal remainSum = 0;
                int i;
                for (i = 0; i < prihodRows.Length && (Convert.ToDecimal(prihodRows[i]["netto"]) < abskol); ++i)
                {
                    remainSum = Math.Round(remainSum, 2, MidpointRounding.AwayFromZero) + Convert.ToDecimal(prihodRows[i]["netto"]) * Convert.ToDecimal(prihodRows[i]["zcena"]);
                    abskol -= Convert.ToDecimal(prihodRows[i]["netto"]);
                    remainsRow["zcena"] = Convert.ToDecimal(prihodRows[i]["zcena"]); //debug
                }

                //if (id_tovar == 163081)
                //    Console.WriteLine("here");

                if (abskol > 0 && i < prihodRows.Length)
                    remainSum = Math.Round(remainSum, 2, MidpointRounding.AwayFromZero) + abskol * Convert.ToDecimal(prihodRows[i]["zcena"]);
                else
                    remainSum = Math.Round(remainSum, 2, MidpointRounding.AwayFromZero) + abskol * sql.GetProcurementPrice(id_tovar, datePrice);

                if (kol < 0)
                    remainSum = 0 - remainSum;

                //DataRow remainsRow = remains.NewRow();
                remainsRow["id_tovar"] = id_tovar;
                remainsRow["remains"] = Math.Round(remainSum, 2, MidpointRounding.AwayFromZero);
                remainsRow["id_grp1"] = goodsRow["id_grp1"]; //debug
                remainsRow["kol"] = goodsRow[kolColumnName]; //debug
                remainsRow["ean"] = goodsRow["ean"]; //debug
                remains.Rows.Add(remainsRow);
            }

            return remains;
        }

        //test3
        public static DataTable CountRemains_inv(DataTable goods, DataTable prihod, string kolColumnName, IDataWorker sql, DateTime datePrice)
        {
            DataTable remains = new DataTable();
            remains.Columns.Add("id_tovar");
            remains.Columns.Add(new DataColumn("remains", typeof(decimal)));
            remains.Columns.Add("id_grp2"); //debug
            remains.Columns.Add("kol"); //debug
            remains.Columns.Add("ean"); //debug
            remains.Columns.Add("zcena"); //debug

            foreach (DataRow goodsRow in goods.Rows)
            {
                int id_tovar = Convert.ToInt32(goodsRow["id"]);
                DataRow[] prihodRows = prihod.Select("id_tovar = " + id_tovar.ToString(), "dprihod desc, id desc");
                decimal kol = Convert.ToDecimal(goodsRow[kolColumnName]);
                decimal abskol = Math.Abs(kol);

                DataRow remainsRow = remains.NewRow();

                decimal remainSum = 0;
                int i;
                for (i = 0; i < prihodRows.Length && (Convert.ToDecimal(prihodRows[i]["netto"]) < abskol); ++i)
                {
                    remainSum = Math.Round(remainSum, 2, MidpointRounding.AwayFromZero) + Convert.ToDecimal(prihodRows[i]["netto"]) * Convert.ToDecimal(prihodRows[i]["zcena"]);
                    abskol -= Convert.ToDecimal(prihodRows[i]["netto"]);
                    remainsRow["zcena"] = Convert.ToDecimal(prihodRows[i]["zcena"]); //debug
                }

                //if (id_tovar == 163081)
                //    Console.WriteLine("here");

                if (abskol > 0 && i < prihodRows.Length)
                    remainSum = Math.Round(remainSum, 2, MidpointRounding.AwayFromZero) + abskol * Convert.ToDecimal(prihodRows[i]["zcena"]);
                else
                    remainSum = Math.Round(remainSum, 2, MidpointRounding.AwayFromZero) + abskol * sql.GetProcurementPrice(id_tovar, datePrice);

                if (kol < 0)
                    remainSum = 0 - remainSum;

                //DataRow remainsRow = remains.NewRow();
                remainsRow["id_tovar"] = id_tovar;
                remainsRow["remains"] = Math.Round(remainSum, 2, MidpointRounding.AwayFromZero);
                remainsRow["id_grp2"] = goodsRow["id_grp2"]; //debug
                remainsRow["kol"] = goodsRow[kolColumnName]; //debug
                remainsRow["ean"] = goodsRow["ean"]; //debug
                remains.Rows.Add(remainsRow);
            }

            return remains;
        }


        public static DataTable CountRN(DataTable goods, DataTable rems1, DataTable rems2, int id_otdel)
        {
            DataTable rn = new DataTable();
            //            rn.Columns.Add("id");
            rn.Columns.Add(new DataColumn("id", typeof(string)));
            rn.Columns.Add("ean");
            rn.Columns.Add("cname");
            rn.Columns.Add(new DataColumn("prihod_all", typeof(double)));
            rn.Columns.Add(new DataColumn("prihod", typeof(double)));
            rn.Columns.Add(new DataColumn("otgruz", typeof(double)));
            rn.Columns.Add(new DataColumn("vozvr", typeof(double)));
            rn.Columns.Add(new DataColumn("spis", typeof(double)));
            rn.Columns.Add(new DataColumn("spis_inv", typeof(double)));
            rn.Columns.Add(new DataColumn("realiz_all", typeof(double)));
            rn.Columns.Add(new DataColumn("realiz", typeof(double)));
            rn.Columns.Add(new DataColumn("realiz_opt", typeof(double)));
            rn.Columns.Add(new DataColumn("vozvkass", typeof(double)));
            rn.Columns.Add(new DataColumn("rn", typeof(double)));
            rn.Columns.Add("id_grp1");
            rn.Columns.Add("id_grp2");
            rn.Columns.Add(new DataColumn("r1", typeof(double)));
            rn.Columns.Add(new DataColumn("r2", typeof(double)));
            rn.Columns.Add(new DataColumn("procent", typeof(double)));
            rn.Columns.Add("id_otdel", typeof(int));
            if (Config.isCompareData)
            {
                DataColumn col = new DataColumn("notValidate", typeof(bool));
                col.DefaultValue = false;
                rn.Columns.Add(col);
            }



            //rn.Columns.Add(new DataColumn("prihod_all", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("prihod", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("otgruz", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("vozvr", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("spis", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("spis_inv", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("realiz_all", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("realiz", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("realiz_opt", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("vozvkass", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("rn", typeof(decimal)));
            //rn.Columns.Add("id_grp1");
            //rn.Columns.Add(new DataColumn("r1", typeof(decimal)));
            //rn.Columns.Add(new DataColumn("r2", typeof(decimal)));

            foreach (DataRow goodsRow in goods.Rows)
            {
                //int id_tovar = Convert.ToInt32(goodsRow["id"]);
                string id_tovar = Convert.ToString(goodsRow["id"]);
                decimal prihod = Convert.ToDecimal(goodsRow["prihod_all"]);
                decimal realiz = Convert.ToDecimal(goodsRow["realiz_all"]);
                DataRow[] remStart = rems1.Select("id_tovar = '" + id_tovar.ToString() + "'");
                DataRow[] remFinish = rems2.Select("id_tovar = '" + id_tovar.ToString() + "'");

                decimal RN = realiz - (Convert.ToDecimal(remStart[0]["remains"]) + prihod - Convert.ToDecimal(remFinish[0]["remains"]));

                if (!Config.isInventSpis)
                    prihod = prihod - Convert.ToDecimal(goodsRow["spis_inv"]);
                


                DataRow rnRow = rn.NewRow();
                rnRow["id"] = id_tovar;
                rnRow["ean"] = goodsRow["ean"];
                rnRow["cname"] = goodsRow["cname"];
                rnRow["prihod_all"] = prihod;
                rnRow["prihod"] = goodsRow["prihod"];
                rnRow["otgruz"] = goodsRow["otgruz"];
                rnRow["vozvr"] = goodsRow["vozvr"];
                rnRow["spis"] = goodsRow["spis"];
                rnRow["spis_inv"] = Config.isInventSpis ? goodsRow["spis_inv"] : (decimal)0;
                rnRow["realiz_all"] = realiz;
                rnRow["realiz"] = goodsRow["realiz"];
                rnRow["realiz_opt"] = goodsRow["realiz_opt"];
                rnRow["vozvkass"] = goodsRow["vozvkass"];
                rnRow["rn"] = RN;
                rnRow["id_grp1"] = goodsRow["id_grp1"];
                rnRow["id_grp2"] = goodsRow["id_grp2"];

                rnRow["r1"] = remStart[0]["remains"];
                rnRow["r2"] = remFinish[0]["remains"];
                rnRow["id_otdel"] = id_otdel;


                if (realiz != 0)
                    rnRow["procent"] = Math.Round(RN * 100 / realiz, 2);
                else
                    rnRow["procent"] = 0;

                if (Config.isCompareData && Config.dtDaveRN != null)
                {
                    DataRow[] remDaveRN = Config.dtDaveRN.Select($"id_tovar = {id_tovar} and id_department = {id_otdel}");
                    if (remDaveRN.Count() > 0)
                    {
                        if ((decimal)remDaveRN[0]["RestStart"] != decimal.Parse(rnRow["r1"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["RestStop"] != decimal.Parse(rnRow["r2"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["PrihodSum"] != decimal.Parse(rnRow["prihod"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["OtgruzSum"] != decimal.Parse(rnRow["otgruz"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["VozvrSum"] != decimal.Parse(rnRow["vozvr"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["SpisSum"] != decimal.Parse(rnRow["spis"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["InventSpisSum"] != decimal.Parse(rnRow["spis_inv"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["RealizSum"] != decimal.Parse(rnRow["realiz"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["OtgruzOptSum"] != decimal.Parse(rnRow["realiz_opt"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["VozvrKassSum"] != decimal.Parse(rnRow["vozvkass"].ToString())) rnRow["notValidate"] = true;

                        if ((int)remDaveRN[0]["id_department"] != id_otdel)
                        {
                            DataRow newRow = Config.dtListDepsVsTovarNoCorrect.NewRow();
                            newRow["ean"] = goodsRow["ean"];
                            newRow["cName"] = goodsRow["cname"];
                            newRow["depOrginal"] = Config.dtDeps.AsEnumerable().Where(r => r.Field<Int16>("id") == id_otdel).First()["name"];
                            newRow["depCopy"] = Config.dtDeps.AsEnumerable().Where(r => r.Field<Int16>("id") == (int)remDaveRN[0]["id_department"]).First()["name"];

                            Config.dtListDepsVsTovarNoCorrect.Rows.Add(newRow);
                        }
                    }
                    else
                    {
                        rnRow["notValidate"] = true;
                        remDaveRN = Config.dtDaveRN.Select($"id_tovar = {id_tovar}");
                        if (remDaveRN.Count() > 0)
                        {
                            if ((int)remDaveRN[0]["id_department"] != id_otdel)
                            {
                                DataRow newRow = Config.dtListDepsVsTovarNoCorrect.NewRow();
                                newRow["ean"] = goodsRow["ean"];
                                newRow["cName"] = goodsRow["cname"];
                                newRow["depOrginal"] = Config.dtDeps.AsEnumerable().Where(r => r.Field<Int16>("id") == id_otdel).First()["name"];
                                newRow["depCopy"] = Config.dtDeps.AsEnumerable().Where(r => r.Field<Int16>("id") == (int)remDaveRN[0]["id_department"]).First()["name"];

                                Config.dtListDepsVsTovarNoCorrect.Rows.Add(newRow);
                            }
                        }
                    }
                }

                rn.Rows.Add(rnRow);
            }

            return rn;
        }
        //test3
        public static DataTable CountRN_inv(DataTable goods, DataTable rems1, DataTable rems2, int id_otdel)
        {
            DataTable rn = new DataTable();
            //            rn.Columns.Add("id");
            rn.Columns.Add(new DataColumn("id", typeof(string)));
            rn.Columns.Add("ean");
            rn.Columns.Add("cname");
            rn.Columns.Add(new DataColumn("prihod_all", typeof(double)));
            rn.Columns.Add(new DataColumn("prihod", typeof(double)));
            rn.Columns.Add(new DataColumn("otgruz", typeof(double)));
            rn.Columns.Add(new DataColumn("vozvr", typeof(double)));
            rn.Columns.Add(new DataColumn("spis", typeof(double)));
            rn.Columns.Add(new DataColumn("spis_inv", typeof(double)));
            rn.Columns.Add(new DataColumn("realiz_all", typeof(double)));
            rn.Columns.Add(new DataColumn("realiz", typeof(double)));
            rn.Columns.Add(new DataColumn("realiz_opt", typeof(double)));
            rn.Columns.Add(new DataColumn("vozvkass", typeof(double)));
            rn.Columns.Add(new DataColumn("rn", typeof(double)));
            rn.Columns.Add("id_grp1");
            rn.Columns.Add("id_grp2");
            rn.Columns.Add(new DataColumn("r1", typeof(double)));
            rn.Columns.Add(new DataColumn("r2", typeof(double)));
            rn.Columns.Add("id_otdel", typeof(int));
            if (Config.isCompareData)
            {
                DataColumn col = new DataColumn("notValidate", typeof(bool));
                col.DefaultValue = false;
                rn.Columns.Add(col);
            }

            foreach (DataRow goodsRow in goods.Rows)
            {
                //int id_tovar = Convert.ToInt32(goodsRow["id"]);
                string id_tovar = Convert.ToString(goodsRow["id"]);
                decimal prihod = Convert.ToDecimal(goodsRow["prihod_all"]);
                decimal realiz = Convert.ToDecimal(goodsRow["realiz_all"]);
                DataRow[] remStart = rems1.Select("id_tovar = '" + id_tovar.ToString() + "'");
                DataRow[] remFinish = rems2.Select("id_tovar = '" + id_tovar.ToString() + "'");

                decimal RN = realiz - (Convert.ToDecimal(remStart[0]["remains"]) + prihod - Convert.ToDecimal(remFinish[0]["remains"]));

                if (!Config.isInventSpis)
                    prihod = prihod - Convert.ToDecimal(goodsRow["spis_inv"]);

                DataRow rnRow = rn.NewRow();
                rnRow["id"] = id_tovar;
                rnRow["ean"] = goodsRow["ean"];
                rnRow["cname"] = goodsRow["cname"];
                rnRow["prihod_all"] = prihod;
                rnRow["prihod"] = goodsRow["prihod"];
                rnRow["otgruz"] = goodsRow["otgruz"];
                rnRow["vozvr"] = goodsRow["vozvr"];
                rnRow["spis"] = goodsRow["spis"];
                rnRow["spis_inv"] = Config.isInventSpis ? goodsRow["spis_inv"] : (decimal)0;
                rnRow["realiz_all"] = realiz;
                rnRow["realiz"] = goodsRow["realiz"];
                rnRow["realiz_opt"] = goodsRow["realiz_opt"];
                rnRow["vozvkass"] = goodsRow["vozvkass"];
                rnRow["rn"] = RN;
                rnRow["id_grp1"] = goodsRow["id_grp1"];
                rnRow["id_grp2"] = goodsRow["id_grp2"];

                rnRow["r1"] = remStart[0]["remains"];
                rnRow["r2"] = remFinish[0]["remains"];
                rnRow["id_otdel"] = id_otdel;

                if (Config.isCompareData && Config.dtDaveRN != null)
                {
                    DataRow[] remDaveRN = Config.dtDaveRN.Select($"id_tovar = {id_tovar}");
                    if (remDaveRN.Count() > 0)
                    {
                        if ((decimal)remDaveRN[0]["RestStart"] != decimal.Parse(rnRow["r1"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["RestStop"] != decimal.Parse(rnRow["r2"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["PrihodSum"] != decimal.Parse(rnRow["prihod"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["OtgruzSum"] != decimal.Parse(rnRow["otgruz"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["VozvrSum"] != decimal.Parse(rnRow["vozvr"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["SpisSum"] != decimal.Parse(rnRow["spis"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["InventSpisSum"] != decimal.Parse(rnRow["spis_inv"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["RealizSum"] != decimal.Parse(rnRow["realiz"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["OtgruzOptSum"] != decimal.Parse(rnRow["realiz_opt"].ToString())) rnRow["notValidate"] = true;
                        if ((decimal)remDaveRN[0]["VozvrKassSum"] != decimal.Parse(rnRow["vozvkass"].ToString())) rnRow["notValidate"] = true;

                        if ((int)remDaveRN[0]["id_department"] != id_otdel)
                        {
                            DataRow newRow = Config.dtListDepsVsTovarNoCorrect.NewRow();
                            newRow["ean"] = goodsRow["ean"];
                            newRow["cName"] = goodsRow["cname"];
                            newRow["depOrginal"] = Config.dtDeps.AsEnumerable().Where(r => r.Field<Int16>("id") == id_otdel).First()["name"];
                            newRow["depCopy"] = Config.dtDeps.AsEnumerable().Where(r => r.Field<Int16>("id") == (int)remDaveRN[0]["id_department"]).First()["name"];

                            Config.dtListDepsVsTovarNoCorrect.Rows.Add(newRow);
                        }
                    }
                    else
                    {
                        rnRow["notValidate"] = true;
                    }
                }

                rn.Rows.Add(rnRow);
            }

            return rn;
        }
    }
 }

