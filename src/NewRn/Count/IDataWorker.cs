using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace NewRn
{
    //интерфейс для работы с данными
    interface IDataWorker
    {
        DataTable GetTovars(int id_otdel, DateTime date_start, DateTime date_finish, bool withOptOtgruz, bool shipped);
        //test2
        DataTable GetTovars_inv(int id_otdel, DateTime date_start, DateTime date_finish, bool withOptOtgruz, bool shipped);
        
        DataTable GetPrihod(DateTime date, DateTime date_fix, int id_otdel);
        ArrayList GetGroups(int id_otdel);
        //test2
        ArrayList GetGroups_inv(int id_otdel);

        ArrayList GetDepartments();
        DataTable getGroupGroup(int id_dep);
        decimal GetProcurementPrice(int id_tovar, DateTime date);

        DataTable getSaveRN(int id_tSaveRN);
    }
}
