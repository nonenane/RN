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
    public partial class frmRnCompareTovar : Form
    {
        private Nwuram.Framework.UI.Service.EnableControlsServiceInProg blockers = new Nwuram.Framework.UI.Service.EnableControlsServiceInProg();
        private Nwuram.Framework.ToExcelNew.ExcelUnLoad report = null;

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


            getData();
        }


        private async void getData()
        {
            blockers.SaveControlsEnabledState(this);
            blockers.SetControlsEnabled(this, false);
            progressBar1.Visible = true;
            var result = await Task<bool>.Factory.StartNew(() =>
            {
                DataTable dtRN = null;
                try
                {
                    if (id_otdel != 0 & id_group != 0)
                    {
                        Config.DoOnUIThread(() =>
                        {
                            cmbDeps.SelectedValue = id_otdel;
                            cmbDeps.Enabled = false;
                            cmbDeps_SelectionChangeCommitted(null, null);
                            cmbGrp1.SelectedValue = id_group;
                            cmbGrp1.Enabled = false;
                            cmbGrp2.Enabled = false;
                        }, this);
                        
                        dtRN = Config.dtDaveRN.AsEnumerable().Where(r => r.Field<int>("id_department") == id_otdel && r.Field<int>("id_grp1") == id_group).CopyToDataTable();
                    }
                    else
                        dtRN = Config.dtDaveRN.Copy();
                }
                catch (Exception ex) { }

                dtData = dtTovarCalculation.Clone();
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

                    #region "NEW"
                    try
                    {
                        DataTable dtTmp = dtData.Clone();

                        if (dtTovarCalculation.Columns["id"].DataType == typeof(int))
                        {
                            var query = from g in dtTovarCalculation.AsEnumerable()
                                        join k in dtRN.AsEnumerable() on new { Q = g.Field<int>("id"), Z = g.Field<int>("id_otdel") } equals new { Q = k.Field<int>("id_tovar"), Z = k.Field<int>("id_department") }
                                        select dtTmp.LoadDataRow(new object[]
                                                                       {

                                                                    g.Field<string>("ean"),
                                                                    g.Field<string>("cname"),//cname
                                                                    k.Field<decimal>("PrihodSum") - k.Field<decimal>("OtgruzSum") - k.Field<decimal>("VozvrSum") -k.Field<decimal>("SpisSum") - k.Field<decimal>("VozvrKassSum") + k.Field<decimal>("InventSpisSum"),//prihod_all
                                                                    k.Field<decimal>("PrihodSum"),//prihod
                                                                    k.Field<decimal>("OtgruzSum"),//otgruz
                                                                    k.Field<decimal>("VozvrSum"),//vozvr
                                                                    k.Field<decimal>("SpisSum"),//spis
                                                                    k.Field<decimal>("InventSpisSum"),//spis_inv
                                                                    k.Field<decimal>("RealizSum") - k.Field<decimal>("OtgruzOptSum"),//realiz_all
                                                                    k.Field<decimal>("RealizSum"),//realiz
                                                                    k.Field<decimal>("OtgruzOptSum"),//realiz_opt
                                                                    k.Field<decimal>("VozvrKassSum"),//vozvkass
                                                                    //k.Field<decimal>("RealizSum") -  k.Field<decimal>("PrihodSum") - k.Field<decimal>("RestStart") + k.Field<decimal>("RestStop"),//rn                                                                    
                                                                     k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop")),//rn
                                                                    k.Field<decimal>("RestStart"),//r1
                                                                    k.Field<decimal>("RestStop"),//r2        
                                                                    g.Field<bool>("notValidate"),//notValidate
                                                                    g.Field<string>("id_grp1"),//id_grp1
                                                                    g.Field<string>("id_grp2"),//id_grp2
                                                                    g.Field<int>("id_otdel"),//id_otdel
                                                                    g.Field<int>("id"),
                                                                     (k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop")) == 0 || k.Field<decimal>("RealizAll") == 0)?(decimal)0:Math.Round((k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop"))) / (k.Field<decimal>("RealizAll")) * 100, 2),//procent  
                                                                    //(k.Field<decimal>("RealizSum") - k.Field<decimal>("PrihodSum") - k.Field<decimal>("RestStart") + k.Field<decimal>("RestStop") == 0 || k.Field<decimal>("RealizSum") - k.Field<decimal>("OtgruzOptSum") == 0)?(decimal)0:Math.Round((k.Field<decimal>("RealizSum") - k.Field<decimal>("PrihodSum") - k.Field<decimal>("RestStart") + k.Field<decimal>("RestStop")) / (k.Field<decimal>("RealizSum") - k.Field<decimal>("VozvrKassSum")) * 100, 2),//procent                                                                    
                                                                    "сохраненные",//nameType
                                                                    2,//idType
                                                                    //false,//isError
                                                                    (
                                    decimal.ToDouble(k.Field<decimal>("RestStart")) != g.Field<double>("r1") ||
                                    decimal.ToDouble(k.Field<decimal>("RestStop")) != g.Field<double>("r2") ||
                                    decimal.ToDouble(k.Field<decimal>("PrihodSum")) != g.Field<double>("prihod") ||
                                    decimal.ToDouble(k.Field<decimal>("OtgruzSum")) != g.Field<double>("otgruz") ||
                                    decimal.ToDouble(k.Field<decimal>("VozvrSum")) != g.Field<double>("vozvr") ||
                                    decimal.ToDouble(k.Field<decimal>("SpisSum")) != g.Field<double>("spis") ||
                                    decimal.ToDouble(k.Field<decimal>("InventSpisSum")) != g.Field<double>("spis_inv") ||
                                    decimal.ToDouble(k.Field<decimal>("RealizSum")) != g.Field<double>("realiz") ||
                                    decimal.ToDouble(k.Field<decimal>("OtgruzOptSum")) != g.Field<double>("realiz_opt") ||
                                    decimal.ToDouble(k.Field<decimal>("VozvrKassSum")) != g.Field<double>("vozvkass") ||
                                    decimal.ToDouble(k.Field<decimal>("PrihodAll")) != g.Field<double>("prihod_all") ||
                                    decimal.ToDouble(k.Field<decimal>("RealizAll")) != g.Field<double>("realiz_all") ||
                                    decimal.ToDouble(k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop"))) != g.Field<double>("rn") ||
                                    ((k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop")) == 0 || k.Field<decimal>("RealizAll") == 0)
                                    ?(decimal)0 :
                                       Math.Round((k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop"))) / (k.Field<decimal>("RealizAll")) * 100, 2))
                                        != g.Field<decimal>("procent")
                                    //((k.Field<decimal>("RealizSum") - k.Field<decimal>("PrihodSum") - k.Field<decimal>("RestStart") + k.Field<decimal>("RestStop") == 0 || k.Field<decimal>("RealizSum") - k.Field<decimal>("OtgruzOptSum") == 0)
                                    //?(decimal)0 :
                                       //Math.Round((k.Field<decimal>("RealizSum") - k.Field<decimal>("PrihodSum") - k.Field<decimal>("RestStart") + k.Field<decimal>("RestStop")) / (k.Field<decimal>("RealizSum") - k.Field<decimal>("VozvrKassSum")) * 100, 2))
                                        //!= g.Field<decimal>("procent")

                                    )?true:false,//isError
                        }, false);


                            DataTable dtDataNewGoods = query.CopyToDataTable();
                            dtData.Merge(dtDataNewGoods);
                             
                            query = from g in dtTovarCalculation.AsEnumerable()
                                    join k in dtData.AsEnumerable() on new { Q = g.Field<int>("id"), Z = g.Field<int>("id_otdel") } equals new { Q = k.Field<int>("id"), Z = k.Field<int>("id_otdel") }
                                    select dtTmp.LoadDataRow(new object[]
                                                                   {

                                                                    g.Field<string>("ean"),
                                                                    g.Field<string>("cname"),//cname
                                                                    g.Field<double>("prihod_all"),//prihod_all
                                                                    g.Field<double>("prihod"),//prihod
                                                                    g.Field<double>("otgruz"),//otgruz
                                                                    g.Field<double>("vozvr"),//vozvr
                                                                    g.Field<double>("spis"),//spis
                                                                    g.Field<double>("spis_inv"),//spis_inv
                                                                    g.Field<double>("realiz_all"),//realiz_all
                                                                    g.Field<double>("realiz"),//realiz
                                                                    g.Field<double>("realiz_opt"),//realiz_opt
                                                                    g.Field<double>("vozvkass"),//vozvkass
                                                                    g.Field<double>("rn"),//rn                                                                    
                                                                    g.Field<double>("r1"),//r1
                                                                    g.Field<double>("r2"),//r2        
                                                                    g.Field<bool>("notValidate"),//notValidate
                                                                    g.Field<string>("id_grp1"),//id_grp1
                                                                    g.Field<string>("id_grp2"),//id_grp2
                                                                    g.Field<int>("id_otdel"),//id_otdel
                                                                    g.Field<int>("id"),
                                                                    g.Field<decimal>("procent"),//procent                                                                    
                                                                    "рассчитанные",//nameType
                                                                    1,//idType
                                                                    k.Field<bool>("isError"),//isError                                                                                                    
                                                                   }, false);

                            dtDataNewGoods = query.CopyToDataTable();
                            dtData.Merge(dtDataNewGoods);
                        }
                        else
                        {
                            var query = from g in dtTovarCalculation.AsEnumerable()
                                        join k in dtRN.AsEnumerable() on new { Q = int.Parse(g.Field<string>("id")), Z = g.Field<int>("id_otdel") } equals new { Q = k.Field<int>("id_tovar"), Z = k.Field<int>("id_department") }
                                        select dtTmp.LoadDataRow(new object[]
                                                                       {
                                                                    g.Field<string>("id"),//id
                                                                    g.Field<string>("ean"),//ean
                                                                    g.Field<string>("cname"),//cname
                                                                    k.Field<decimal>("PrihodSum") - k.Field<decimal>("OtgruzSum") - k.Field<decimal>("VozvrSum") -k.Field<decimal>("SpisSum") - k.Field<decimal>("VozvrKassSum") + k.Field<decimal>("InventSpisSum"),//prihod_all
                                                                    k.Field<decimal>("PrihodSum"),//prihod
                                                                    k.Field<decimal>("OtgruzSum"),//otgruz
                                                                    k.Field<decimal>("VozvrSum"),//vozvr
                                                                    k.Field<decimal>("SpisSum"),//spis
                                                                    k.Field<decimal>("InventSpisSum"),//spis_inv
                                                                    k.Field<decimal>("RealizSum") - k.Field<decimal>("OtgruzOptSum"),//realiz_all
                                                                    k.Field<decimal>("RealizSum"),//realiz
                                                                    k.Field<decimal>("OtgruzOptSum"),//realiz_opt
                                                                    k.Field<decimal>("VozvrKassSum"),//vozvkass
                                                                    k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop")),//rn
                                                                    g.Field<string>("id_grp1"),//id_grp1
                                                                    g.Field<string>("id_grp2"),//id_grp2
                                                                    k.Field<decimal>("RestStart"),//r1
                                                                    k.Field<decimal>("RestStop"),//r2                                                                  
                                                                    (k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop")) == 0 || k.Field<decimal>("RealizAll") == 0)?(decimal)0:Math.Round((k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop"))) / (k.Field<decimal>("RealizAll")) * 100, 2),//procent                                                                    
                                                                    g.Field<int>("id_otdel"),//id_otdel
                                                                    g.Field<bool>("notValidate"),//notValidate
                                                                    "сохраненные",//nameType
                                                                    2,//idType
                                                                    //false,//isError
                                                                    (
                                    decimal.ToDouble(k.Field<decimal>("RestStart")) != g.Field<double>("r1") ||
                                    decimal.ToDouble(k.Field<decimal>("RestStop")) != g.Field<double>("r2") ||
                                    decimal.ToDouble(k.Field<decimal>("PrihodSum")) != g.Field<double>("prihod") ||
                                    decimal.ToDouble(k.Field<decimal>("OtgruzSum")) != g.Field<double>("otgruz") ||
                                    decimal.ToDouble(k.Field<decimal>("VozvrSum")) != g.Field<double>("vozvr") ||
                                    decimal.ToDouble(k.Field<decimal>("SpisSum")) != g.Field<double>("spis") ||
                                    decimal.ToDouble(k.Field<decimal>("InventSpisSum")) != g.Field<double>("spis_inv") ||
                                    decimal.ToDouble(k.Field<decimal>("RealizSum")) != g.Field<double>("realiz") ||
                                    decimal.ToDouble(k.Field<decimal>("OtgruzOptSum")) != g.Field<double>("realiz_opt") ||
                                    decimal.ToDouble(k.Field<decimal>("VozvrKassSum")) != g.Field<double>("vozvkass") ||
                                    decimal.ToDouble(k.Field<decimal>("PrihodAll")) != g.Field<double>("prihod_all") ||
                                    decimal.ToDouble(k.Field<decimal>("RealizAll")) != g.Field<double>("realiz_all") ||
                                    decimal.ToDouble(k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop"))) != g.Field<double>("rn") ||
                                    ((k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop")) == 0 || k.Field<decimal>("RealizAll") == 0)
                                    ?(double)0 : 
                                        decimal.ToDouble(Math.Round((k.Field<decimal>("RealizAll") -  (k.Field<decimal>("RestStart") + k.Field<decimal>("PrihodAll") - k.Field<decimal>("RestStop"))) / (k.Field<decimal>("RealizAll")) * 100, 2)))
                                        != g.Field<double>("procent")
                                    )?true:false,//isError
                        }, false);


                            DataTable dtDataNewGoods = query.CopyToDataTable();
                            dtData.Merge(dtDataNewGoods);


                            query = from g in dtTovarCalculation.AsEnumerable()
                                    join k in dtData.AsEnumerable() on new { Q = int.Parse(g.Field<string>("id")), Z = g.Field<int>("id_otdel") } equals new { Q = int.Parse(k.Field<string>("id")), Z = k.Field<int>("id_otdel") }
                                    select dtTmp.LoadDataRow(new object[]
                                                                   {
                                                                    g.Field<string>("id"),//id
                                                                    g.Field<string>("ean"),//ean
                                                                    g.Field<string>("cname"),//cname
                                                                    g.Field<double>("prihod_all"),//prihod_all
                                                                    g.Field<double>("prihod"),//prihod
                                                                    g.Field<double>("otgruz"),//otgruz
                                                                    g.Field<double>("vozvr"),//vozvr
                                                                    g.Field<double>("spis"),//spis
                                                                    g.Field<double>("spis_inv"),//spis_inv
                                                                    g.Field<double>("realiz_all"),//realiz_all
                                                                    g.Field<double>("realiz"),//realiz
                                                                    g.Field<double>("realiz_opt"),//realiz_opt
                                                                    g.Field<double>("vozvkass"),//vozvkass
                                                                    g.Field<double>("rn"),//rn
                                                                    g.Field<string>("id_grp1"),//id_grp1
                                                                    g.Field<string>("id_grp2"),//id_grp2
                                                                    g.Field<double>("r1"),//r1
                                                                    g.Field<double>("r2"),//r2                                                                  
                                                                    g.Field<double>("procent"),//procent                                                                    
                                                                    g.Field<int>("id_otdel"),//id_otdel
                                                                    g.Field<bool>("notValidate"),//notValidate
                                                                    "рассчитанные",//nameType
                                                                    1,//idType
                                                                    k.Field<bool>("isError"),//isError                                                                   
                                                                   }, false);


                            dtDataNewGoods = query.CopyToDataTable();
                            dtData.Merge(dtDataNewGoods);

                        }

                    }catch(Exception ex)
                    { 
                    
                    }
                      


                    #endregion

                    /*
                    foreach (DataRow row in dtTovarCalculation.Rows)
                    {
                        try
                        {
                            DataRow cloneRow = dtData.NewRow();
                            cloneRow.ItemArray = row.ItemArray.Clone() as object[];
                            EnumerableRowCollection<DataRow> rowCollect;
                            int id = 0;
                            if (dtTovarCalculation.Columns["id"].DataType == typeof(int))
                            {
                                id = (int)row["id"];
                                rowCollect = dtRN.AsEnumerable().Where(r => r.Field<int>("id_tovar") == (int)row["id"] && r.Field<int>("id_department") == (int)row["id_otdel"]);
                            }
                            else
                            {
                                id = int.Parse((string)row["id"]);
                                rowCollect = dtRN.AsEnumerable().Where(r => r.Field<int>("id_tovar") == id && r.Field<int>("id_department") == (int)row["id_otdel"]);
                            }

                            //if (((string)row["ean"]).Trim().Equals("46078490"))
                            //    { }

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

                                cloneRow["prihod_all"] = (decimal)rowCollect.First()["PrihodSum"] - (decimal)rowCollect.First()["OtgruzSum"] - (decimal)rowCollect.First()["VozvrSum"] - (decimal)rowCollect.First()["SpisSum"] - (decimal)rowCollect.First()["VozvrKassSum"] + (decimal)rowCollect.First()["InventSpisSum"];

                                cloneRow["realiz_all"] = (decimal)rowCollect.First()["RealizSum"] - (decimal)rowCollect.First()["OtgruzOptSum"];

                                cloneRow["rn"] = (decimal)rowCollect.First()["RealizSum"] - (decimal)rowCollect.First()["PrihodSum"] - (decimal)rowCollect.First()["RestStart"] + (decimal)rowCollect.First()["RestStop"];


                                if ((double)cloneRow["rn"] == 0 || (double)cloneRow["realiz_all"] == 0)
                                    cloneRow["procent"] = (decimal)0;
                                else
                                    cloneRow["procent"] = Math.Round((((decimal)rowCollect.First()["RealizSum"] - (decimal)rowCollect.First()["PrihodSum"] - (decimal)rowCollect.First()["RestStart"] + (decimal)rowCollect.First()["RestStop"]) / ((decimal)rowCollect.First()["RealizSum"] - (decimal)rowCollect.First()["VozvrKassSum"])) * 100, 2);
                                //cloneRow["procent"] = ((decimal)cloneRow["rn"] / (decimal)cloneRow["realiz_all"]) * 100;


                                EnumerableRowCollection<DataRow> rowCollectType2;
                                if (dtTovarCalculation.Columns["id"].DataType == typeof(int))
                                {
                                    rowCollectType2 = dtData.AsEnumerable().Where(r => r.Field<int>("id") == id && r.Field<int>("idType") == 1 && r.Field<int>("id_otdel") == (int)row["id_otdel"]);

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
                                }
                                else
                                {
                                    rowCollectType2 = dtData.AsEnumerable().Where(r => int.Parse(r.Field<string>("id")) == id && r.Field<int>("idType") == 1 && r.Field<int>("id_otdel") == (int)row["id_otdel"]);

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
                                    (double)cloneRow["procent"] != (double)rowType2["procent"])
                                    {
                                        rowType2["isError"] = true;
                                        cloneRow["isError"] = true;
                                    }
                                }
                                //cloneRow[""] = rowCollect.First()[""];
                                //cloneRow[""] = rowCollect.First()[""];
                                dtData.Rows.Add(cloneRow);
                            }

                            //rowCollect = dtData.AsEnumerable().Where(r => r.Field<int>("id_tovar") == (int)row["id_tovar"] && r.Field<int>("idType")==1);

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    */
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

                Config.DoOnUIThread(() =>
                {
                    blockers.RestoreControlEnabledState(this);
                    setFilter();
                    dgvData.DataSource = dtData;
                    progressBar1.Visible = false;
                }, this);

                return true;
            });
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
                    tbEan.Location = new Point(dgvData.Location.X + width + 1, tbEan.Location.Y);
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

                if ((Int16)cmbDeps.SelectedValue != -1)
                    filter += (filter.Length == 0 ? "" : " and ") + $"id_otdel = {cmbDeps.SelectedValue}";

                if ((int)cmbGrp1.SelectedValue != -1)
                    filter += (filter.Length == 0 ? "" : " and ") + $"id_grp1 = {cmbGrp1.SelectedValue}";

                if ((int)cmbGrp2.SelectedValue != -1)
                    filter += (filter.Length == 0 ? "" : " and ") + $"id_grp2 = {cmbGrp2.SelectedValue}";

                dtData.DefaultView.RowFilter = filter;
                dtData.DefaultView.Sort = "id_otdel asc, id asc ";
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

        private void setWidthColumn(int indexRow, int indexCol, int width, Nwuram.Framework.ToExcelNew.ExcelUnLoad report)
        {
            report.SetColumnWidth(indexRow, indexCol, indexRow, indexCol, width);
        }

        private async void btPrint_Click(object sender, EventArgs e)
        {
            report = new Nwuram.Framework.ToExcelNew.ExcelUnLoad();

            int indexRow = 1;
            int maxColumns = 0;
            blockers.SaveControlsEnabledState(this);
            blockers.SetControlsEnabled(this, false);
            progressBar1.Visible = true;
            var result = await Task<bool>.Factory.StartNew(() =>
            {

                foreach (DataGridViewColumn col in dgvData.Columns)
                    if (col.Visible)
                    {
                        maxColumns++;
                        if (col.Name.Equals(cEan)) setWidthColumn(indexRow, maxColumns, 15, report);
                        if (col.Name.Equals(cName)) setWidthColumn(indexRow, maxColumns, 30, report);
                        if (col.Name.Equals(cTypeCalc)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cOstStart)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cOstEnd)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cPrihod)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cOtgruz)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cVozvr)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cSpis)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cSpisInv)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cPrihodGlob)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cRealiz)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cOtgruzOpt)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cVozvrKass)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cRealizGlob)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cRN)) setWidthColumn(indexRow, maxColumns, 16, report);
                        if (col.Name.Equals(cRnPrc)) setWidthColumn(indexRow, maxColumns, 16, report);

                    }


                #region "Head"
                report.Merge(indexRow, 1, indexRow, maxColumns);
                report.AddSingleValue($"{this.Text}", indexRow, 1);
                report.SetFontBold(indexRow, 1, indexRow, 1);
                report.SetFontSize(indexRow, 1, indexRow, 1, 16);
                report.SetCellAlignmentToCenter(indexRow, 1, indexRow, 1);
                indexRow++;
                indexRow++;

                Config.DoOnUIThread(() =>
                {
                    //report.Merge(indexRow, 1, indexRow, maxColumns);
                    //report.AddSingleValue($"Магазин: {cmbShop.Text}", indexRow, 1);
                    //indexRow++;

                    //report.Merge(indexRow, 1, indexRow, maxColumns);
                    //report.AddSingleValue($"Период: {}", indexRow, 1);
                    //indexRow++;

                    report.Merge(indexRow, 1, indexRow, maxColumns);
                    report.AddSingleValue($"Отдел: {cmbDeps.Text}", indexRow, 1);
                    indexRow++;

                    report.Merge(indexRow, 1, indexRow, maxColumns);
                    report.AddSingleValue($"Т/У группа: {cmbGrp1.Text}", indexRow, 1);
                    indexRow++;

                    report.Merge(indexRow, 1, indexRow, maxColumns);
                    report.AddSingleValue($"Инв. группа: {cmbGrp2.Text}", indexRow, 1);
                    indexRow++;


                    if (tbEan.Text.Trim().Length != 0 || tbName.Text.Trim().Length != 0)
                    {
                        report.Merge(indexRow, 1, indexRow, maxColumns);
                        report.AddSingleValue($"Фильтр: {(tbEan.Text.Trim().Length != 0 ? $"EAN:{tbEan.Text.Trim()} | " : "")} {(tbName.Text.Trim().Length != 0 ? $"Наименование:{tbName.Text.Trim()}" : "")}", indexRow, 1);
                        indexRow++;
                    }

                }, this);

                report.Merge(indexRow, 1, indexRow, maxColumns);
                report.AddSingleValue("Выгрузил: " + Nwuram.Framework.Settings.User.UserSettings.User.FullUsername, indexRow, 1);
                indexRow++;

                report.Merge(indexRow, 1, indexRow, maxColumns);
                report.AddSingleValue("Дата выгрузки: " + DateTime.Now.ToString(), indexRow, 1);
                indexRow++;
                indexRow++;
                #endregion

                int indexCol = 0;
                foreach (DataGridViewColumn col in dgvData.Columns)
                    if (col.Visible)
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

                foreach (DataRowView row in dtData.DefaultView)
                {
                    indexCol = 1;
                    report.SetWrapText(indexRow, indexCol, indexRow, maxColumns);
                    foreach (DataGridViewColumn col in dgvData.Columns)
                    {
                        if (col.Visible)
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

                    if ((bool)row["isError"])
                        report.SetCellColor(indexRow, 1, indexRow, maxColumns, panel1.BackColor);

                    report.SetBorders(indexRow, 1, indexRow, maxColumns);
                    report.SetCellAlignmentToCenter(indexRow, 1, indexRow, maxColumns);
                    report.SetCellAlignmentToJustify(indexRow, 1, indexRow, maxColumns);

                    indexRow++;
                }

                indexRow++;
                report.SetCellColor(indexRow, 1, indexRow, 1, panel1.BackColor);
                report.Merge(indexRow, 2, indexRow, maxColumns);
                report.AddSingleValue($"{label1.Text}", indexRow, 2);

                Config.DoOnUIThread(() =>
                {
                    blockers.RestoreControlEnabledState(this);
                    progressBar1.Visible = false;
                }, this);

                report.SetColumnAutoSize(4, 1, indexRow, maxColumns);
                report.Show();
                return true;
            });
        }
    }
}
