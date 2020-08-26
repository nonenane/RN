using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace NewRn
{
    //класс для логирования
    class Logger
    {
        #region Fields

        /// <summary>
        /// Наименование файла лога
        /// </summary>
        static string _fileName;

        #endregion

        #region Constructors

        /// <summary>
        /// Создание файла, либо просто определение его имени
        /// </summary>
        public Logger()
        {
            _fileName = DateTime.Today.ToShortDateString() + ".log";
        }

        public Logger(string fileName)
        {
            _fileName = DateTime.Today.ToShortDateString() + fileName + ".log";
        }

        #endregion

        #region Methods

        /// <summary>
        /// запись нового сообщения в файл
        /// </summary>
        /// <param name="message">текст сообщения</param>
        public static void NewMessage(string message)
        {
            try
            {
                FileInfo file = new FileInfo(_fileName);
                StreamWriter writer = file.AppendText();
                writer.WriteLine(DateTime.Now.ToString() + @":  " + message);
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи в лог: " + e.Message);
            }
        }

        #endregion
    }
}
