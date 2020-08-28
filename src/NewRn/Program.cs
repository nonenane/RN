using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using Nwuram.Framework.Settings.Connection;
using Nwuram.Framework.Settings.User;
using Nwuram.Framework.Logging;
using Nwuram.Framework.Project;

namespace NewRn
{
    static class Program
    {

        //возвращает true, если исключение ex критично (соотв. типа); false - в ином случае
        public static bool IsCritical(Exception ex)
        {
            if (ex is OutOfMemoryException) return true;
            if (ex is AppDomainUnloadedException) return true;
            if (ex is BadImageFormatException) return true;
            if (ex is CannotUnloadAppDomainException) return true;
            if (ex is ExecutionEngineException) return true;
            if (ex is InvalidProgramException) return true;
            if (ex is System.Threading.ThreadAbortException)
                return true;
            return true;// false;
        }
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {      
                Project.FillSettings(args);
                InitAndRun();
            }
            else
            {
                try
                {
                    Project proj = new Project(Application.StartupPath + "\\NewRn.ini");                
                    if (proj.Authorize())
                        InitAndRun();  
                }
                catch (Exception e)
                {
                    if (IsCritical(e))
                        throw;
                    MessageBox.Show(e.Message);
                    Logger.NewMessage(e.Message);
                }
            }
        }
        private static void InitAndRun()
        {
            Logging.Init(ConnectionSettings.GetServer(), ConnectionSettings.GetDatabase(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);
            Logging.StartFirstLevel(1);
            Logging.Comment("Вход в программу");
            Logging.StopFirstLevel();



            Application.Run(new Form1());
            //Application.Run(new frmRnCompareTovar());


            Logging.StartFirstLevel(2);
            Logging.Comment("Выход из программы");
            Logging.StopFirstLevel();
        }
    }
}
