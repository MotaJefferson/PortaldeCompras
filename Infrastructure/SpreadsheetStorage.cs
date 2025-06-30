using PortaldeCompras.Domain.Interfaces;
using System.Data;
using ClosedXML.Excel;

namespace PortaldeCompras.Infrastructure
{
    public class SpreadsheetStorage : IDataSaver
    {
        public void Save(DataTable data, string path)
        {
            path = path.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ? path : $"{path}.xlsx";

            DataTable DataExists = new DataTable();

            if (File.Exists(path))
            {
                using var workbook = new XLWorkbook(path);
                var worksheet = workbook.Worksheet("Licitacoes");
                
                if (worksheet != null)
                {
                    bool firstRow = true;
                    foreach(var row in worksheet.RowsUsed())
                    {
                        if (firstRow)
                        {
                            foreach (var cell in row.Cells())
                                DataExists.Columns.Add(cell.GetString());
                            firstRow = false;
                        }
                        else
                        {
                            var dataRow = DataExists.NewRow();
                            for (int i = 0; i < DataExists.Columns.Count; i++)
                            {
                                var cell = row.Cell(i + 1);
                                dataRow[i] = cell.Value.Type == XLDataType.Blank
                                    ? DBNull.Value
                                    : cell.Value;

                                //dataRow[i] = row.Cell(i + 1).Value;
                            }                                
                                
                            DataExists.Rows.Add(dataRow);
                        }
                    }
                }
            }
            else
            {
                foreach(DataColumn col in data.Columns)
                    DataExists.Columns.Add(col.ColumnName);
            }

            Func<DataRow, string> GenerateKey = r => string.Join("|", r.ItemArray.Select(v => v?.ToString()?.Trim()));

            var ExistingKeys = DataExists.AsEnumerable()
                                         .Select(GenerateKey)
                                         .ToHashSet();

            var NewRowsToInsert = data.AsEnumerable()
                                      .Where(r => !ExistingKeys.Contains(GenerateKey(r)))
                                      .ToList();

            if(NewRowsToInsert.Count == 0)
            {
                Console.WriteLine("Nenhuma nova linha para inserir");
            }

            XLWorkbook wb;
            IXLWorksheet ws;

            if (File.Exists(path))
            {
                wb = new XLWorkbook(path);
                ws = wb.Worksheet("Licitacoes") ?? wb.AddWorksheet("Licitacoes");
            }
            else
            {
                wb = new XLWorkbook();
                ws = wb.AddWorksheet("Licitacoes");

                for(int i = 0; i < data.Columns.Count; i++)
                    ws.Cell(1, i + 1).Value = data.Columns[i].ColumnName;
            }

            int lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;

            if(lastRow == 1 && ws.Cell(1, 1).IsEmpty())
            {
                for (int i = 0; i < data.Columns.Count; i++)
                    ws.Cell(1, i + 1).Value = data.Columns[i].ColumnName;
                lastRow = 1;
            }

            int currentRow = lastRow + 1;

            foreach(var row in NewRowsToInsert)
            {
                for(int col = 0; col < data.Columns.Count; col++)
                {
                    var value = row[col];

                    //ws.Cell(currentRow, col + 1).Value = value != null ? value.ToString() : "";

                    if (Convert.IsDBNull(value))
                    {
                        ws.Cell(currentRow, col + 1).Value = "";
                    }
                    else if (value is int || value is double || value is decimal || value is float)
                    {
                        ws.Cell(currentRow, col + 1).Value = Convert.ToDouble(value);
                    }
                    else if (value is DateTime)
                    {
                        ws.Cell(currentRow, col + 1).Value = (DateTime)value;
                    }
                    else
                    {
                        ws.Cell(currentRow, col + 1).Value = value.ToString();
                    }

                }
                currentRow++;
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(path);

            Console.WriteLine($"{NewRowsToInsert.Count} novas linhas foram inseridas na planilha");
        }
    }
}
