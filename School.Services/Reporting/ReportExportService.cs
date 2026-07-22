using System.Text;
using System.Text.Json;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using School.Services.Interfaces;

namespace School.Services.Reporting
{
    /// <summary>
    /// Non-RDLC export service for CSV streaming, styled Excel (ClosedXML),
    /// XML, and JSON exports. Used when RDLC is not available or for raw data exports.
    /// </summary>
    public class ReportExportService : IReportExportService
    {
        private readonly ILogger<ReportExportService> _logger;

        public ReportExportService(ILogger<ReportExportService> logger)
        {
            _logger = logger;
        }

        // ─── CSV ─────────────────────────────────────────────────────────────

        public async Task<byte[]> ExportToCsvAsync(IEnumerable<object> data, string[] columns)
        {
            return await Task.Run(() =>
            {
                var sb = new StringBuilder();
                var rows = data.ToList();

                if (!rows.Any()) return Encoding.UTF8.GetBytes(string.Empty);

                // Header
                sb.AppendLine(string.Join(",", columns.Select(c => $"\"{c}\"")));

                // Determine properties once
                var props = rows[0].GetType().GetProperties()
                    .Where(p => columns.Length == 0 || columns.Contains(p.Name))
                    .ToList();

                foreach (var row in rows)
                {
                    var values = props.Select(p =>
                    {
                        var v = p.GetValue(row)?.ToString()?.Replace("\"", "\"\"") ?? string.Empty;
                        return $"\"{v}\"";
                    });
                    sb.AppendLine(string.Join(",", values));
                }

                return Encoding.UTF8.GetBytes(sb.ToString());
            });
        }

        // ─── Excel ───────────────────────────────────────────────────────────

        public async Task<byte[]> ExportToExcelAsync(
            IEnumerable<object> data, string sheetName = "Report",
            string[]? columns = null, bool freezeHeader = true, bool autoWidth = true)
        {
            return await Task.Run(() =>
            {
                using var wb = new XLWorkbook();
                var ws = wb.AddWorksheet(sheetName);
                var rows = data.ToList();

                if (!rows.Any())
                {
                    using var emptyMs = new MemoryStream();
                    wb.SaveAs(emptyMs);
                    return emptyMs.ToArray();
                }

                var props = rows[0].GetType().GetProperties()
                    .Where(p => columns == null || columns.Length == 0 || columns.Contains(p.Name))
                    .ToList();

                // Header row
                for (int col = 0; col < props.Count; col++)
                {
                    var cell = ws.Cell(1, col + 1);
                    cell.Value = props[col].Name;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e3a8a");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Data rows
                for (int row = 0; row < rows.Count; row++)
                {
                    for (int col = 0; col < props.Count; col++)
                    {
                        var cell = ws.Cell(row + 2, col + 1);
                        var value = props[col].GetValue(rows[row]);
                        SetCellValue(cell, value);

                        // Alternating row colors
                        if (row % 2 == 1)
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#f0f4ff");
                    }
                }

                // Freeze header
                if (freezeHeader)
                    ws.SheetView.FreezeRows(1);

                // Auto-fit columns
                if (autoWidth)
                    ws.Columns().AdjustToContents();

                // Range border
                if (rows.Count > 0)
                {
                    var range = ws.Range(1, 1, rows.Count + 1, props.Count);
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Hair;
                }

                using var ms = new MemoryStream();
                wb.SaveAs(ms);
                return ms.ToArray();
            });
        }

        public async Task<byte[]> ExportMultiSheetExcelAsync(
            Dictionary<string, IEnumerable<object>> sheets)
        {
            return await Task.Run(() =>
            {
                using var wb = new XLWorkbook();

                foreach (var (sheetName, data) in sheets)
                {
                    var ws = wb.AddWorksheet(sheetName.Length > 31
                        ? sheetName.Substring(0, 31) : sheetName);
                    var rows = data.ToList();
                    if (!rows.Any()) continue;

                    var props = rows[0].GetType().GetProperties().ToList();

                    // Header
                    for (int col = 0; col < props.Count; col++)
                    {
                        var cell = ws.Cell(1, col + 1);
                        cell.Value = props[col].Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e3a8a");
                        cell.Style.Font.FontColor = XLColor.White;
                    }

                    // Data
                    for (int row = 0; row < rows.Count; row++)
                        for (int col = 0; col < props.Count; col++)
                            SetCellValue(ws.Cell(row + 2, col + 1), props[col].GetValue(rows[row]));

                    ws.SheetView.FreezeRows(1);
                    ws.Columns().AdjustToContents();
                }

                using var ms = new MemoryStream();
                wb.SaveAs(ms);
                return ms.ToArray();
            });
        }

        // ─── XML ─────────────────────────────────────────────────────────────

        public async Task<byte[]> ExportToXmlAsync(
            IEnumerable<object> data, string rootElement = "Report")
        {
            return await Task.Run(() =>
            {
                var rows = data.ToList();
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sb.AppendLine($"<{rootElement} GeneratedAt=\"{DateTime.UtcNow:O}\">");

                foreach (var row in rows)
                {
                    sb.AppendLine("  <Row>");
                    foreach (var prop in row.GetType().GetProperties())
                    {
                        var val = prop.GetValue(row)?.ToString() ?? string.Empty;
                        sb.AppendLine($"    <{prop.Name}>{System.Security.SecurityElement.Escape(val)}</{prop.Name}>");
                    }
                    sb.AppendLine("  </Row>");
                }

                sb.AppendLine($"</{rootElement}>");
                return Encoding.UTF8.GetBytes(sb.ToString());
            });
        }

        // ─── JSON ─────────────────────────────────────────────────────────────

        public async Task<byte[]> ExportToJsonAsync(IEnumerable<object> data)
        {
            return await Task.Run(() =>
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return Encoding.UTF8.GetBytes(json);
            });
        }

        // ─── Private Helpers ─────────────────────────────────────────────────

        private static void SetCellValue(IXLCell cell, object? value)
        {
            if (value == null) { cell.Value = string.Empty; return; }

            switch (value)
            {
                case int i: cell.Value = i; break;
                case long l: cell.Value = l; break;
                case decimal d: cell.Value = d; break;
                case double dbl: cell.Value = dbl; break;
                case float f: cell.Value = f; break;
                case bool b: cell.Value = b; break;
                case DateTime dt:
                    cell.Value = dt;
                    cell.Style.DateFormat.Format = "dd-MMM-yyyy HH:mm";
                    break;
                default: cell.Value = value.ToString() ?? string.Empty; break;
            }
        }
    }
}
