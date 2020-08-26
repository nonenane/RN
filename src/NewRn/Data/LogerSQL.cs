using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Nwuram.Framework.Settings.Connection;

namespace NewRn.Data
{
    internal class LogerSQL
    {
        #region Data

        private ArrayList ap = new ArrayList();
        private SqlConnection sql = null;
        private string connectionString = GetConnectionString();

        #endregion

        #region Constructors
        //public LogerSQL(string server, string username, string password, string dbName, string appName)
        public LogerSQL()
        {
            sql = new SqlConnection();
            sql.ConnectionString = connectionString;
            sql.Open();
            sql.Close();
        }
        #endregion

        private static string GetConnectionString()
        {
            // To avoid storing the connection string in your code,  
            // you can retrieve it from a configuration file. 
            return @"Data Source=" + ConnectionSettings.GetServer() + ";Initial Catalog=Logs;User ID=" +
                   ConnectionSettings.GetUsername() + ";Password=" + ConnectionSettings.GetPassword();
        }

        public string getStatus()
        {
            return sql.State.ToString();
        }

        public void OpenSql()
        {
            sql.Open();
        }
        public void CloseSql()
        {
            sql.Close();
        }
    }
}