using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Eventmanager
{
    public class Export
    {
        public void Exportieren(string turniername, DataGrid Starterliste_DataGrid)
        {

            string filename = "";

            // Configure save file dialog box
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = turniername; // Default file name
            dlg.DefaultExt = ".xlsx"; // Default file extension
            dlg.Filter = "Excel documents (.xlsx)|*.xlsx"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                filename = dlg.FileName;
            }


            //instantiate excel objects (application, workbook, worksheets)
            excel.Application XlObj = new excel.Application();
            XlObj.Visible = false;
            excel._Workbook WbObj = (excel.Workbook)(XlObj.Workbooks.Add(""));
            excel._Worksheet WsObj = (excel.Worksheet)WbObj.ActiveSheet;

            
            DataTable dt = DataGridtoDataTable(Starterliste_DataGrid);

            //run through datatable and assign cells to values of datatable
            try
            {
                int row = 1; int col = 1;
                foreach (DataColumn column in dt.Columns)
                {
                    //adding columns
                    WsObj.Cells[row, col] = column.ColumnName;
                    col++;
                }
                //reset column and row variables
                col = 1;
                row++;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //adding data
                    foreach (var cell in dt.Rows[i].ItemArray)
                    {
                        WsObj.Cells[row, col] = cell;
                        col++;
                    }
                    col = 1;
                    row++;
                }
                WbObj.SaveAs(filename);
            }
            catch (COMException x)
            {
                Console.WriteLine(x);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                WbObj.Close();
            }
        }

        public static DataTable DataGridtoDataTable(DataGrid dg)
        {
            List<Teilnehmer> liste = new List<Teilnehmer>();

            //Entferne die Kommas bei den Startern
            foreach(var element in dg.Items)
            {
                var starter = (Teilnehmer)element;
                var starterString = starter.Starter.Replace(",", " ");
                starter.Starter = starterString;
                liste.Add(starter);
            }

            dg.Items.Clear();

            foreach(var element in liste)
            {
                dg.Items.Add(element);
            }

            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();
            String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);

            if (result != null)
            {
                string[] Lines = result.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                string[] Fields;
                Fields = Lines[0].Split(new char[] { ',' });
                int Cols = Fields.GetLength(0);
                DataTable dt = new DataTable();
                //1st row must be column names; force lower case to ensure matching later on.
                for (int i = 0; i < Cols; i++)
                    dt.Columns.Add(Fields[i].ToUpper(), typeof(string));
                DataRow Row;
                for (int i = 1; i < Lines.GetLength(0) - 1; i++)
                {
                    Fields = Lines[i].Split(new char[] { ',' });
                    Row = dt.NewRow();
                    for (int f = 0; f < Cols; f++)
                    {
                        Row[f] = Fields[f];
                    }
                    dt.Rows.Add(Row);
                }
                return dt;
            }

            return null;

        }


    }
}
