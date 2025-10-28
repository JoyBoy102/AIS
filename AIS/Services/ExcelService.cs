using AIS.Models;
using Microsoft.Win32;
using System;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AIS.Services
{
    public class ExcelService
    {
        public async Task<string> SaveReportsToExcelAsync(IEnumerable<Report> reports)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Отчеты");

                    // Заголовки
                    worksheet.Cell(1, 1).Value = "ID Теплицы";
                    worksheet.Cell(1, 2).Value = "Время";
                    worksheet.Cell(1, 3).Value = "Температура";
                    worksheet.Cell(1, 4).Value = "CO2";
                    worksheet.Cell(1, 5).Value = "Влажность";
                    worksheet.Cell(1, 6).Value = "Команды";

                    // Стиль для заголовков
                    var headerRange = worksheet.Range(1, 1, 1, 6);
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Font.Bold = true;

                    // Данные
                    int row = 2;
                    foreach (var report in reports)
                    {
                        worksheet.Cell(row, 1).Value = report.IdGreenHouse;
                        worksheet.Cell(row, 2).Value = report.Time;
                        worksheet.Cell(row, 3).Value = report.Temperature;
                        worksheet.Cell(row, 4).Value = report.CO2;
                        worksheet.Cell(row, 5).Value = report.Humidity;

                        // Команды как строка
                        var commandsText = string.Join("; ",
                            report.Commands?.Select(c => $"{c.Value}") ?? Array.Empty<string>());
                        worksheet.Cell(row, 6).Value = commandsText;

                        row++;
                    }

                    // Авто-размер колонок
                    worksheet.Columns().AdjustToContents();

                    // Диалог сохранения файла
                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx",
                        DefaultExt = ".xlsx",
                        FileName = $"Отчет_теплицы_{DateTime.Now:yyyy-MM-dd_HH-mm}",
                        InitialDirectory = Properties.Settings.Default.LastSaveDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);

                        // Сохраняем путь для будущего использования
                        Properties.Settings.Default.LastSaveDirectory = Path.GetDirectoryName(saveFileDialog.FileName);

                        return saveFileDialog.FileName;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при сохранении в Excel: {ex.Message}", ex);
                }
            });
        }
    }
}
