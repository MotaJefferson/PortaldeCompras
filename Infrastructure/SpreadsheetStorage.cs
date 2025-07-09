using ClosedXML.Excel;
using PortaldeCompras.Domain.Interfaces;
using System.Data;

namespace PortaldeCompras.Infrastructure
{
    public class SpreadsheetStorage : IDataSaver
    {
        public int Save(DataTable newData, string path)
        {
            path = path.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ? path : $"{path}.xlsx";

            XLWorkbook wb;
            IXLWorksheet ws;
            var existingKeys = new HashSet<string>();

            if (File.Exists(path))
            {
                wb = new XLWorkbook(path);
                if (wb.Worksheets.TryGetWorksheet("Licitacoes", out ws))
                {
                    if (ws.LastRowUsed() != null && ws.LastRowUsed().RowNumber() > 1)
                    {
                        var header = ws.FirstRowUsed();
                        var tempTable = new DataTable();
                        foreach(var cell in header.CellsUsed())
                        {
                            tempTable.Columns.Add(cell.Value.ToString());
                        }

                        foreach(var dataRow in ws.RowsUsed().Skip(1))
                        {
                            var newRow = tempTable.NewRow();
                            for(int i = 0; i < tempTable.Columns.Count; i++)
                            {
                                newRow[i] = dataRow.Cell(i + 1).Value;
                            }
                            tempTable.Rows.Add(newRow);
                        }
                        existingKeys = new HashSet<string>(tempTable.AsEnumerable().Select(GenerateKey));
                    }
                }
                else
                {
                    ws = wb.AddWorksheet("Licitacoes");
                }
            }
            else
            {
                wb = new XLWorkbook();
                ws = wb.AddWorksheet("Licitacoes");
            }

            var newRowsToInsert = newData.AsEnumerable()
                .Where(row => !existingKeys.Contains(GenerateKey(row)))
                .ToList();

            if (!newRowsToInsert.Any())
            {
                wb.Dispose();
                return 0; 
            }

            if (ws.LastRowUsed() == null)
            {
                for (int i = 0; i < newData.Columns.Count; i++)
                {
                    ws.Cell(1, i + 1).Value = newData.Columns[i].ColumnName;
                }
            }

            if (newRowsToInsert.Any())
            {
                ws.Cell(ws.LastRowUsed().RowNumber() + 1, 1).InsertData(newRowsToInsert);
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(path);
            wb.Dispose();

            return newRowsToInsert.Count;
        }

        private string GenerateKey(DataRow row)
        {
            return string.Join("|", row.ItemArray.Select(v => v?.ToString()?.Trim()));
        }
    }
}