using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NewRn
{
   public class Config
    {
        public static bool isInventSpis;
        public static bool isCompareData;
        public static int id_TSaveRN;
        public static DataTable dtDaveRN;
        public static DataTable dtListDepsVsTovarNoCorrect;
        public static DataTable dtDeps;

        public static void createListDepsVsTovarNoCorrect()
        {
            dtListDepsVsTovarNoCorrect = new DataTable();

            dtListDepsVsTovarNoCorrect.Columns.Add("ean", typeof(string));
            dtListDepsVsTovarNoCorrect.Columns.Add("cName", typeof(string));
            dtListDepsVsTovarNoCorrect.Columns.Add("depOrginal", typeof(string));
            dtListDepsVsTovarNoCorrect.Columns.Add("depCopy", typeof(string));
            dtListDepsVsTovarNoCorrect.AcceptChanges();
        }
    }
}
