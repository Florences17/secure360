using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Opteamix.AuthorizationFramework.Common
{
    public class ExcelUtility
    {
        #region Read Excel           
       
        public static DataSet GetDataFromExcel(string filePath)
        {
            DataSet excelData = new DataSet();
            int dataRow = 2;
            int columnRow =1;

            try
            {
                using (SpreadsheetDocument docuemnt = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = docuemnt.WorkbookPart;
                    Sheets sheetCollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    
                    foreach (Sheet sheet in sheetCollection)
                    {
                        DataTable dataTable = new DataTable();
                        Worksheet worksheet =((WorksheetPart) workbookPart.GetPartById(sheet.Id)).Worksheet;
                        SheetData sheetData =(SheetData) worksheet.GetFirstChild<SheetData>();
                        int totalHeaderCount = sheetData.Descendants<Row>().ElementAt(columnRow).Descendants<Cell>().Count();
                        List<Cell> cellList = sheetData.Descendants<Row>().ElementAt(columnRow).Elements<Cell>().ToList();
                        try
                        {
                            for (int i = 1; i <= totalHeaderCount; i++)
                            {
                                string columnName = GetCellValue(workbookPart, cellList, i);
                                if (string.IsNullOrWhiteSpace(columnName))
                                {
                                    throw new Exception("Blank Column in Excel");
                                }

                                columnName = columnName.Trim();

                                dataTable.Columns.Add(columnName);
                            }

                            foreach (Row row in sheetData.Descendants<Row>())
                            {
                                if (row.RowIndex > dataRow)
                                {
                                    DataRow tempRow = dataTable.NewRow();

                                    for (int i = 1; i <= totalHeaderCount; i++)
                                    {
                                        tempRow[i - 1] = GetCellValue(workbookPart, row.Elements<Cell>().ToList(), i);
                                    }

                                    dataTable.Rows.Add(tempRow);
                                }
                            }

                            excelData.Tables.Add(dataTable);
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }

            return excelData;
        }

        private static string GetCellValue(WorkbookPart workbookPart, List<Cell> cellList, int index)
        {
            return GetCellValue(workbookPart,cellList,GetColumnName(index));
        }
        private static string GetColumnName(int colNumber)
        {
            int dividend = colNumber;
            string colName = string.Empty;
            int modulo;
            while (dividend > 0)
            {
                modulo = (dividend-1) % 26;
                colName = Convert.ToChar(65+modulo).ToString() + colName;
                dividend = (int)((dividend - modulo)/26);
            }
            return colName;
        }

        private static string GetCellValue(WorkbookPart workbookPart , List<Cell> cellList, string columnName)
        {
             Cell theCell = null;
             string value = string.Empty;

             foreach (Cell cell in cellList)
             {
                if (cell.CellReference.Value.StartsWith(columnName))
                {
                    theCell = cell;
                    break;
                }
             }
             if (theCell.CellValue != null)
             {
                value = theCell.CellValue.InnerText;
                if(theCell.DataType != null)
                {
                    switch (theCell.DataType.Value)
                    {
                        case CellValues.SharedString:
                        var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                        if (stringTable != null)
                        {
                            value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                        }
                            break;
                        case CellValues.Boolean:
                        switch (value)
                        {
                            case "0":
                                value = "FALSE";
                                break;
                            default:
                            value = "TRUE";
                                break;
                        }
                        
                            break;
                    }
                    
                }
             }

             return value;
        }

         #endregion

         #region Write Excel

         public static void WriteExcel(string fileName, string path, DataSet excelData, string template)
         {
            string templateFile = Path.Combine(path, template);
            string exportFile =Path.Combine(path, fileName);
            if (!File.Exists(templateFile))
            {
                throw new Exception("Excel Template Error");
            }
            File.Copy(templateFile, exportFile, true);
            using(SpreadsheetDocument document = SpreadsheetDocument.Open(exportFile, true))
            {
                foreach (DataTable table in excelData.Tables)
                {
                    uint rowIndex = 2;
                    int columnIndex = 1;

                    foreach (DataColumn column in table.Columns)
                    {
                        string columnName = GetColumnName(columnIndex);
                        AddUpdateCellValue(document, table.TableName, rowIndex, columnName, column.ColumnName);
                        columnIndex++;
                    }

                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        rowIndex++;
                        columnIndex = 1;
                        for (int col = 0; col < table.Columns.Count; col++)
                        {
                            string value = table.Rows[row][col].ToString();
                            string columnName = GetColumnName(columnIndex);
                            AddUpdateCellValue(document, table.TableName, rowIndex, columnName, value);
                            columnIndex++;
                        }
                    }
                }
            }
         }

         private static void AddUpdateCellValue(SpreadsheetDocument spreadsheet, string sheetName, uint rowIndex, string columnName, string text)
         {
            WorksheetPart worksheetPart = RetrieveSheetPartByName(spreadsheet, sheetName);
            if (worksheetPart != null)
            {
                Cell cell = InsertCellInSheet(columnName, (rowIndex), worksheetPart);
                cell.CellValue = new CellValue(text);
                cell.DataType = new EnumValue<CellValues>(CellValues.String);
                worksheetPart.Worksheet.Save();
            }
         }

         private static WorksheetPart RetrieveSheetPartByName(SpreadsheetDocument document, string sheetName)
         {
            IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().Where(s=>s.Name == sheetName);
            if(sheets.Count() == 0)
            {
                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart) document.WorkbookPart.GetPartById(relationshipId);

            return worksheetPart;
         }

         private static Cell InsertCellInSheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
         {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;
            Row row;

            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r=>r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row()
                {
                    RowIndex = rowIndex
                };
                sheetData.Append(row);
            }

            if (row.Elements<Cell>().Where(c=>c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c=>c.CellReference.Value == cellReference ).First();
            }
            else  
            {
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if(string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell()
                {
                    CellReference = cellReference
                };
                row.InsertBefore(newCell, refCell);
                worksheet.Save();
                return newCell;
                
            }
         }
            
         #endregion
    }
}