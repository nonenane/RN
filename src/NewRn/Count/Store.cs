using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace NewRn
{
    class Store
    {
        private DataTable MakeRNTable()
        {
            DataTable rn = new DataTable();
            //rn.Columns.Add("id", typeof(int));
            rn.Columns.Add(new DataColumn("id", typeof(string)));
            rn.Columns.Add("cname", typeof(string));
            rn.Columns.Add("prihod_all", typeof(double));
            rn.Columns.Add("prihod", typeof(double));
            rn.Columns.Add("otgruz", typeof(double));
            rn.Columns.Add("vozvr", typeof(double));
            rn.Columns.Add("spis", typeof(double));
            rn.Columns.Add("spis_inv", typeof(double));
            rn.Columns.Add("realiz_all", typeof(double));
            rn.Columns.Add("realiz", typeof(double));
            rn.Columns.Add("realiz_opt", typeof(double));
            rn.Columns.Add("vozvkass", typeof(double));
            rn.Columns.Add("rn", typeof(double));
            rn.Columns.Add("procent", typeof(double));
            rn.Columns.Add("r1", typeof(double));
            rn.Columns.Add("r2", typeof(double));

            //rn.Columns.Add("prihod_all", typeof(decimal));
            //rn.Columns.Add("prihod", typeof(decimal));
            //rn.Columns.Add("otgruz", typeof(decimal));
            //rn.Columns.Add("vozvr", typeof(decimal));
            //rn.Columns.Add("spis", typeof(decimal));
            //rn.Columns.Add("spis_inv", typeof(decimal));
            //rn.Columns.Add("realiz_all", typeof(decimal));
            //rn.Columns.Add("realiz", typeof(decimal));
            //rn.Columns.Add("realiz_opt", typeof(decimal));
            //rn.Columns.Add("vozvkass", typeof(decimal));
            //rn.Columns.Add("rn", typeof(decimal));
            //rn.Columns.Add("procent", typeof(decimal));
            //rn.Columns.Add("r1", typeof(decimal));
            //rn.Columns.Add("r2", typeof(decimal));
            return rn;
        }

        private ArrayList deps = null;

        private decimal prihod_all = 0;
        public decimal PrihodAll
        {
            get
            {
                return prihod_all;               
            }
        }

        private decimal realiz_all = 0;
        public decimal RealizAll
        {
            get { return realiz_all; }
        }

        private decimal rN = 0;
        public decimal RN
        {
            get { return rN; }
        }

        private decimal remainStart = 0;
        public decimal RemainStart
        {
            get { return remainStart; }
        }

        private decimal remainFinish = 0;
        public decimal RemainFinish
        {
            get { return remainFinish; }
        }

        public decimal Procent
        {
            get
            {
                if (realiz_all != 0)
                    return Math.Round(rN * 100 / realiz_all, 2);
                else
                    return 0;
            }
        }

        public delegate void Callback();
        public event Callback Notify;
        private DataRow rnRow;
        
        public DataTable CountRN(IDataWorker sql, IDataWorker sqlVVO, DateTime date_start, DateTime date_finish, bool withOptOtgruz, bool shipped)
        {
            DataTable rn = MakeRNTable();
            deps = sql.GetDepartments();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            DataTable prihodStart = new DataTable();// = sql.GetPrihod(date_start.AddSeconds(-1), date_start.AddSeconds(0), - 1);
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            DataTable prihodFinish = new DataTable();// = sql.GetPrihod(date_finish.AddDays(1).AddSeconds(-1), date_finish.AddDays(1).AddSeconds(-1), -1);
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            DataTable prihodStartVVO = new DataTable();// = sqlVVO.GetPrihod(date_start.AddSeconds(-1), date_start.AddSeconds(0), -1);
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            DataTable prihodFinishVVO = new DataTable();// = sqlVVO.GetPrihod(date_finish.AddDays(1).AddSeconds(-1), date_finish.AddDays(1).AddSeconds(-1), -1);
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            prihod_all = 0;
            realiz_all = 0;
            rN = 0;
            remainStart = 0;
            remainFinish = 0;
            
            foreach (Department dep in deps)
            {
                if (dep.Name.Trim() != "ВВО")
                    dep.CountRN(dep.Id, date_start, date_finish, prihodStart, prihodFinish, withOptOtgruz, shipped, sql);
                else
                    dep.CountRN(dep.Id, date_start, date_finish, prihodStartVVO, prihodFinishVVO, withOptOtgruz, shipped, sqlVVO);
                //проверка на isnull
                rnRow = rn.NewRow();
                rnRow["id"] = dep.Id;
                rnRow["cname"] = dep.Name;
                rnRow["prihod_all"] = dep.PrihodAll;
                rnRow["prihod"] = dep.Prihod;
                rnRow["otgruz"] = dep.Otgruz;
                rnRow["vozvr"] = dep.Vozvr;
                rnRow["spis"] = dep.Spis;
                rnRow["spis_inv"] = dep.SpisInv;
                rnRow["realiz_all"] = dep.RealizAll;
                rnRow["realiz"] = dep.Realiz;
                rnRow["realiz_opt"] = dep.RealizOpt;
                rnRow["vozvkass"] = dep.Vozvkass;
                rnRow["rn"] = dep.RN;
                rnRow["procent"] = dep.Procent;
                rnRow["r1"] = dep.R1;
                rnRow["r2"] = dep.R2;
                rn.Rows.Add(rnRow);

                prihod_all += dep.PrihodAll;
                realiz_all += dep.RealizAll;
                rN += dep.RN;
                remainStart += dep.R1;
                remainFinish += dep.R2;

                Notify();
            }
            prihodStart.Dispose();
            prihodFinish.Dispose();
            prihodStartVVO.Dispose();
            prihodFinishVVO.Dispose();
            return rn;

        }

        //test3
        public DataTable CountRN_inv(IDataWorker sql, IDataWorker sqlVVO, DateTime date_start, DateTime date_finish, bool withOptOtgruz, bool shipped)
        {
            DataTable rn = MakeRNTable();
            deps = sql.GetDepartments();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            DataTable prihodStart = new DataTable();// = sql.GetPrihod(date_start.AddSeconds(-1), date_start.AddSeconds(0), - 1);
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            DataTable prihodFinish = new DataTable();// = sql.GetPrihod(date_finish.AddDays(1).AddSeconds(-1), date_finish.AddDays(1).AddSeconds(-1), -1);
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            DataTable prihodStartVVO = new DataTable();// = sqlVVO.GetPrihod(date_start.AddSeconds(-1), date_start.AddSeconds(0), -1);
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            DataTable prihodFinishVVO = new DataTable();// = sqlVVO.GetPrihod(date_finish.AddDays(1).AddSeconds(-1), date_finish.AddDays(1).AddSeconds(-1), -1);
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(true).ToString());
            prihod_all = 0;
            realiz_all = 0;
            rN = 0;
            remainStart = 0;
            remainFinish = 0;

            foreach (Department dep in deps)
            {
                if (dep.Name.Trim() != "ВВО")
                    dep.CountRN_inv(dep.Id, date_start, date_finish, prihodStart, prihodFinish, withOptOtgruz, shipped, sql);
                else
                    dep.CountRN_inv(dep.Id, date_start, date_finish, prihodStartVVO, prihodFinishVVO, withOptOtgruz, shipped, sqlVVO);

                rnRow = rn.NewRow();
                rnRow["id"] = dep.Id;
                rnRow["cname"] = dep.Name;
                rnRow["prihod_all"] = dep.PrihodAll;
                rnRow["prihod"] = dep.Prihod;
                rnRow["otgruz"] = dep.Otgruz;
                rnRow["vozvr"] = dep.Vozvr;
                rnRow["spis"] = dep.Spis;
                rnRow["spis_inv"] = dep.SpisInv;
                rnRow["realiz_all"] = dep.RealizAll;
                rnRow["realiz"] = dep.Realiz;
                rnRow["realiz_opt"] = dep.RealizOpt;
                rnRow["vozvkass"] = dep.Vozvkass;
                rnRow["rn"] = dep.RN;
                rnRow["procent"] = dep.Procent;
                rnRow["r1"] = dep.R1;
                rnRow["r2"] = dep.R2;
                rn.Rows.Add(rnRow);

                prihod_all += dep.PrihodAll;
                realiz_all += dep.RealizAll;
                rN += dep.RN;
                remainStart += dep.R1;
                remainFinish += dep.R2;

                Notify();
            }
            prihodStart.Dispose();
            prihodFinish.Dispose();
            prihodStartVVO.Dispose();
            prihodFinishVVO.Dispose();
            return rn;

        }
        
        public Department GetDepartment(int id)
        {
            foreach (Department dep in deps)
            {
                if (dep.Id == id)
                    return dep;
            }
            return null;
        }
        //public Department GetDepartment(string name)
        //{
        //    foreach (Department dep in deps)
        //    {
        //        if (dep.Name == name)
        //            return dep;
        //    }
        //    return null;
        //}
    }
}
