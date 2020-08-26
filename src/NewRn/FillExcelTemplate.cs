using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace NewRn
{
    //класс для выгрузки в excel
    public class FillExcelTemplate
    {

        #region Fields

        private DataTable dtSingleValue = null;
        private DataSet dsMultiValue = null;

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string MessageError = "";
        /// <summary>
        /// Номер ошибки (0 - без ошибок, -1 - пользовательская ошибка, -2 - системная ошибка)
        /// </summary>
        public int nError = 0;

        #endregion

        #region Constructor

        public FillExcelTemplate()
        {
            dtSingleValue_Init();
        }

        #endregion

        #region Private Methods

        private Excel.Application OpenTemplateInExcel(string PathTemplate)
        {
            object missingValue = Type.Missing;

            try
            {
                Excel.Application ExcelApp = new Excel.Application();
                ExcelApp.DisplayAlerts = false;
                ExcelApp.DefaultSaveFormat = Excel.XlFileFormat.xlExcel7;
                ExcelApp.Workbooks.Open(PathTemplate, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue);

                return ExcelApp;
            }
            catch (Exception exc)
            {
                WriteError(-2, exc.Message);
                return null;
            }
        }

        private bool UnloadListValueInTemplate(Excel.Application ExcelApp, ArrayList ListSheetName)
        {
            object missingValue = Type.Missing;

            for (int i = 0; i < ExcelApp.Sheets.Count; i++)
            {
                if (ListSheetName == null || ListSheetName.Count == 0 || FindSheetInListSheetName(ListSheetName, ((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[i + 1]).Name))
                {
                    if (!UnloadSingleValue(ExcelApp, i + 1))
                    {
                        return false;
                    }

                    if (!UnloadMultiValue(ExcelApp, i + 1))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void dtSingleValue_Init()
        {
            dtSingleValue = new DataTable();

            dtSingleValue.Columns.Add("TagName", typeof(System.String));
            dtSingleValue.Columns.Add("TagValue", typeof(System.String));
            dtSingleValue.AcceptChanges();
        }

        private bool FindSheetInListSheetName(ArrayList alListSheetName, string SheetName)
        {
            bool FindSheetName = false;

            if (alListSheetName == null || alListSheetName.Count == 0 || SheetName == null)
                return FindSheetName;

            for (int i = 0; i < alListSheetName.Count; i++)
            {
                if (alListSheetName[i].ToString().Trim() == SheetName.Trim())
                {
                    FindSheetName = true;
                    break;
                }
            }

            return FindSheetName;
        }

        private bool UnloadSingleValue(Excel.Application ExcelApp, int SheetIndex)
        {
            object missingValue = Type.Missing;
            string TempMessage = "(Заполнение единичных тегов) ";

            for (int i = 0; i < dtSingleValue.Rows.Count; i++)
            {
                try
                {
                    if (((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[SheetIndex]).Cells.Find(dtSingleValue.Rows[i]["TagName"].ToString(), missingValue, Excel.XlFindLookIn.xlValues, missingValue, missingValue, Excel.XlSearchDirection.xlNext, true, missingValue, missingValue) == null)
                    {
                        WriteError(-1, TempMessage + "В листе - '" + ((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[SheetIndex]).Name + "' отсутствует информация о '" + dtSingleValue.Rows[i]["TagName"].ToString() + "'");
                        return false;
                    }

                    ((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[SheetIndex]).Cells.Replace(dtSingleValue.Rows[i]["TagName"].ToString(), dtSingleValue.Rows[i]["TagValue"].ToString(), Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, true, false, false, false);
                }
                catch (Exception exc)
                {
                    WriteError(-2, TempMessage + exc.Message);
                    return false;
                }
            }

            return true;
        }

        private bool UnloadMultiValue(Excel.Application ExcelApp, int SheetIndex)
        {
            object missingValue = Type.Missing;
            string TempMessage = "(Заполнение мульти-списка) ";

            if (dsMultiValue != null && dsMultiValue.Tables.Count > 0)
            {
                for (int i = 0; i < dsMultiValue.Tables.Count; i++)
                {
                    Excel.Range ExcelRange;
                    Int32[] Fields = new Int32[dsMultiValue.Tables[i].Columns.Count];
                    int nRow = 0;

                    for (int j = 0; j < dsMultiValue.Tables[i].Columns.Count; j++)
                    {
                        string TagName = "{" + dsMultiValue.Tables[i].Namespace.Trim() + "." + dsMultiValue.Tables[i].Columns[j].ColumnName.Trim() + "}";

                        try
                        {
                            ExcelRange = ((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[SheetIndex]).Cells.Find(TagName, missingValue, missingValue, missingValue, missingValue, Excel.XlSearchDirection.xlNext, missingValue, missingValue, missingValue);
                        }
                        catch (Exception exc)
                        {
                            WriteError(-2, TempMessage + exc.Message);
                            return false;
                        }

                        if (ExcelRange == null)
                        {
                            ExcelRange = null;
                            WriteError(-1, TempMessage + "В листе - '" + ((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[SheetIndex]).Name + "' отсутствует информация о '" + TagName + "'");
                            return false;
                        }

                        Fields[j] = (ExcelRange != null) ? ExcelRange.Column : -1;
                        nRow = (ExcelRange != null) ? ExcelRange.Row : -1;

                        ExcelRange = null;
                    }

                    try
                    {
                        ExcelRange = (Excel.Range)((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[SheetIndex]).Cells[nRow, 1];
                    }
                    catch (Exception exc)
                    {
                        WriteError(-2, TempMessage + exc.Message);
                        return false;
                    }

                    for (int j = 0; j < dsMultiValue.Tables[i].Rows.Count; j++)
                    {
                        if ((j + 1) < dsMultiValue.Tables[i].Rows.Count)
                        {
                            try
                            {
                                ExcelRange.EntireRow.Insert(true, true);
                            }
                            catch (Exception exc)
                            {
                                WriteError(-2, TempMessage + exc.Message);
                                return false;
                            }
                        }

                        for (int k = 0; k < dsMultiValue.Tables[i].Columns.Count; k++)
                        {
                            if (Fields[k] >= 0)
                            {
                                try
                                {
                                    ((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[SheetIndex]).Cells[nRow + j, Fields[k]] = dsMultiValue.Tables[i].Rows[j][k];
                                }
                                catch (Exception exc)
                                {
                                    WriteError(-2, TempMessage + exc.Message);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        private void DeleteTemplate(string PathTemplate)
        {
            try
            {
                FileInfo fiTemplate = new FileInfo(PathTemplate);

                if (fiTemplate.Exists)
                    fiTemplate.Delete();
            }
            catch (Exception) { }
        }

        private bool CopyTemplate(string PathTemplate, string PathUnloadTemplate)
        {
            string TempMessage = "(Ошибка при копировании файла шаблона) ";

            FileInfo fiTemplate = new FileInfo(PathTemplate);

            if (!fiTemplate.Exists)
            {
                WriteError(-1, TempMessage + "Файл шаблона по указанному пути " + '"' + PathTemplate + '"' + " не найден!");
                return false;
            }

            FileInfo fiUnloadTemplate = new FileInfo(PathUnloadTemplate);

            if (!Directory.Exists(fiUnloadTemplate.DirectoryName))
            {
                WriteError(-1, TempMessage + "Директория выгрузки шаблона " + '"' + fiUnloadTemplate.DirectoryName + '"' + " не найдена!");
                return false;
            }

            try
            {
                fiTemplate.CopyTo(PathUnloadTemplate, true);
            }
            catch (Exception exc)
            {
                WriteError(-2, TempMessage + exc.Message);
                return false;
            }

            return true;
        }

        private void WriteError(int Error, string Message)
        {
            this.nError = Error;
            this.MessageError = Message;
        }

        private void CloseExcel(Excel.Application ExcelApp)
        {
            if (ExcelApp.Workbooks != null)
                ExcelApp.Workbooks.Close();

            if (ExcelApp != null)
                ExcelApp.Quit();

            // выгружаем из памяти объекты Excel
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp.Workbooks);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp);

            ExcelApp = null;

            GC.Collect();
        }

        private void DisposeAll()
        {
            if (dtSingleValue != null)
            {
                dtSingleValue.Dispose();
                dtSingleValue = null;
            }

            if (dsMultiValue != null)
            {
                dsMultiValue.Dispose();
                dsMultiValue = null;
            }

            GC.Collect();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Добавить единичный тег
        /// </summary>
        /// <param name="TagName">Имя тега</param>
        /// <param name="Value">Значение тега</param>
        /// <returns></returns>
        public bool AddItemSingleList(string TagName, string Value)
        {
            #region Проверка входных параметров

            if (TagName == null || TagName.Trim().Length == 0)
            {
                WriteError(-1, "Параметр TagName должен быть заполнен!");
                return false;
            }

            if (Value == null || Value.Trim().Length == 0)
            {
                WriteError(-1, "Параметр Value должен быть заполнен!");
                return false;
            }

            if (dtSingleValue.Select("TagName = " + "'" + TagName.Trim() + "'").Length > 0)
            {
                WriteError(-1, "Тег с таким именем уже добавлен!");
                return false;
            }


            #endregion

            DataRow drNew = dtSingleValue.NewRow();
            drNew["TagName"] = TagName.Trim();
            drNew["TagValue"] = Value.Trim();

            dtSingleValue.Rows.Add(drNew);

            return true;
        }

        /// <summary>
        /// Добавить мульти-список
        /// </summary>
        /// <param name="MultiList">Мульти-список (список должен сожержать имена полей без префикса и фигурных скобок)</param>
        /// <param name="NamePrefix">Префикс тега</param>
        /// <returns></returns>
        public bool AddMultiList(DataTable MultiList, string NamePrefix)
        {
            if (NamePrefix == null || NamePrefix.Trim().Length == 0)
            {
                WriteError(-1, "Недопустимое значение передаваемого параметра NamePrefix!");
                return false;
            }

            if (dsMultiValue == null)
                dsMultiValue = new DataSet();

            for (int i = 0; i < dsMultiValue.Tables.Count; i++)
            {
                if (dsMultiValue.Tables[i].Namespace.Trim() == NamePrefix.Trim())
                {
                    WriteError(-1, "MultiList с таким NamePrefix уже добавлен!");
                    return false;
                }
            }
            DataTable dtNew = MultiList.DefaultView.ToTable();
            dtNew.TableName = "MultiList" + ((dsMultiValue == null) ? 1 : dsMultiValue.Tables.Count + 1).ToString();
            dtNew.Namespace = NamePrefix;

            dsMultiValue.Tables.Add(dtNew);

            return true;
        }

        /// <summary>
        /// Очистить все единичные теги
        /// </summary>
        public void ClearSingleList()
        {
            try
            {
                dtSingleValue.Clear();
            }
            catch (Exception exc)
            {
                WriteError(-2, exc.Message);
            }
        }

        /// <summary>
        /// Очистить все мульти-списки
        /// </summary>
        public void ClearMultiList()
        {
            try
            {
                dsMultiValue.Tables.Clear();
            }
            catch (Exception exc)
            {
                WriteError(-2, exc.Message);
            }
        }

        /// <summary>
        /// Очистить все единичные теги и мульти-списки
        /// </summary>
        public void ClearAll()
        {
            ClearSingleList();

            // если не возникало ошибок
            if (nError == 0)
            {
                ClearMultiList();
            }
        }

        /// <summary>
        /// Создать шаблон
        /// </summary>
        /// <param name="PathTemplate">Путь к файлу шаблона</param>
        /// <param name="PathUnloadTemplate">Путь для выгрузки шаблона, с именем файла</param>
        /// <param name="ListSheetName">Список листов для которых нужно производить замену. Для замены во всех листах необходимо указать null, либо пустой список.</param>
        /// <returns></returns>
        public bool CreateTemplate(string PathTemplate, string PathUnloadTemplate, ArrayList ListSheetName)
        {
            if ((dtSingleValue == null || dtSingleValue.Rows.Count == 0) && (dsMultiValue == null || dsMultiValue.Tables.Count == 0))
            {
                WriteError(-1, "Информация для выгрузки отсутствует!");
                return false;
            }

            // копируем файл фаблона в папку для выгрузки шаблона 
            if (!CopyTemplate(PathTemplate, PathUnloadTemplate))
            {
                DisposeAll();
                DeleteTemplate(PathUnloadTemplate);
                return false;
            }

            // открываем файл отчета в MS Excel и получаем объект Excel
            Excel.Application ExcelApp = OpenTemplateInExcel(PathUnloadTemplate);

            if (ExcelApp == null)
            {
                DisposeAll();
                DeleteTemplate(PathUnloadTemplate);
                return false;
            }

            // заменяем в файле шаблона теги на значения
            if (!UnloadListValueInTemplate(ExcelApp, ListSheetName))
            {
                CloseExcel(ExcelApp);
                DisposeAll();
                DeleteTemplate(PathUnloadTemplate);
                return false;
            }

            try
            {
                object missingValue = Type.Missing;
                ExcelApp.ActiveWorkbook.SaveAs(PathUnloadTemplate, missingValue, missingValue, missingValue, missingValue, missingValue, Excel.XlSaveAsAccessMode.xlNoChange, missingValue, missingValue, missingValue, missingValue, missingValue);
            }
            catch (Exception exc)
            {
                WriteError(-2, exc.Message);
                CloseExcel(ExcelApp);
                DisposeAll();
                DeleteTemplate(PathUnloadTemplate);

                return false;
            }

            CloseExcel(ExcelApp);

            return true;
        }

        #endregion
    }
}
