using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NewRn
{
    //класс для работы с ini файлом
    class IniFile
    {
        #region Fields

        private static string path;  //путь к ini файлу

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        #endregion

        #region Constructor

        protected IniFile(string INIPath)
        {
            path = INIPath;
        }

        #endregion

        #region Methods

        //запись в ini файл
        protected static void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }

        //чтение из ini файла
        protected static string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, path);
            return temp.ToString();

        }

        #endregion
    }
}
