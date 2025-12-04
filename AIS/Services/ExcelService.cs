using Microsoft.Win32;
using System;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using AIS.Structs;

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
                    if (reports == null || !reports.Any())
                        throw new ArgumentException("Нет данных для сохранения");

                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Отчеты");

                    // Получаем свойства класса Report через рефлексию
                    var properties = typeof(Report).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.CanRead)
                        //.OrderBy(p => p.Name)
                        .ToArray();

                    // Заголовки через рефлексию
                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = GetDisplayName(properties[i]);
                    }

                    // Стиль для заголовков
                    var headerRange = worksheet.Range(1, 1, 1, properties.Length);
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Данные через рефлексию
                    int row = 2;
                    foreach (var report in reports)
                    {
                        for (int col = 0; col < properties.Length; col++)
                        {
                            var value = properties[col].GetValue(report);
                            SetCellValue(worksheet.Cell(row, col + 1), value, properties[col]);
                        }
                        row++;
                    }

                    // Применяем форматирование к данным
                    ApplyDataFormatting(worksheet, properties, reports.Count());

                    // Авто-размер колонок
                    worksheet.Columns().AdjustToContents();

                    // Добавляем границы ко всем данным
                    var dataRange = worksheet.Range(1, 1, row - 1, properties.Length);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

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
                        Properties.Settings.Default.Save();

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

        private void SetCellValue(IXLCell cell, object value, PropertyInfo property)
        {
            if (value == null)
            {
                cell.Value = string.Empty;
                return;
            }

            // Преобразуем значение в правильный тип для ClosedXML
            switch (value)
            {
                case DateTime dateTime:
                    cell.Value = dateTime;
                    break;
                case int intValue:
                    cell.Value = intValue;
                    break;
                case double doubleValue:
                    cell.Value = doubleValue;
                    break;
                case decimal decimalValue:
                    cell.Value = decimalValue;
                    break;
                case float floatValue:
                    cell.Value = floatValue;
                    break;
                case long longValue:
                    cell.Value = longValue;
                    break;
                case bool boolValue:
                    cell.Value = boolValue;
                    break;
                case string stringValue:
                    cell.Value = stringValue;
                    break;
                default:
                    // Для остальных типов используем строковое представление
                    cell.Value = value.ToString();
                    break;
            }
        }

        private string GetDisplayName(PropertyInfo property)
        {
            // Можно добавить атрибуты для кастомных названий, пока используем имя свойства
            return property.Name switch
            {
                "IdGreenHouse" => "ID Теплицы",
                "ReportTime" => "Время",
                "TemperatureValue" => "Температура",
                "CO2Value" => "CO2",
                "HumidityValue" => "Влажность",
                "CO2Pred" => "Прогноз уровня CO2",
                "commandCO2" => "Команда для исполнительного устройства CO2",
                "commandHumidity" => "Команда для увлажнителя",
                "commandTemperature" => "Команда для обогревателя",
                "HumidityPred" => "Прогноз уровня влажности",
                "TemperaturePred" => "Прогноз температуры",
                _ => property.Name
            };
        }

        private void ApplyDataFormatting(IXLWorksheet worksheet, PropertyInfo[] properties, int dataRowCount)
        {
            if (dataRowCount == 0) return;

            for (int col = 0; col < properties.Length; col++)
            {
                var propertyType = properties[col].PropertyType;
                var dataRange = worksheet.Range(2, col + 1, dataRowCount + 1, col + 1);

                // Применяем форматирование в зависимости от типа данных
                if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    dataRange.Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
                    dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
                else if (IsNumericType(propertyType))
                {
                    dataRange.Style.NumberFormat.Format = "0,00";
                    dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                }
                else
                {
                    dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                }
            }
        }

        private bool IsNumericType(Type type)
        {
            var numericTypes = new HashSet<Type>
            {
                typeof(int), typeof(double), typeof(decimal), typeof(float), typeof(long),
                typeof(short), typeof(byte), typeof(uint), typeof(ulong), typeof(ushort),
                typeof(sbyte), typeof(int?), typeof(double?), typeof(decimal?), typeof(float?),
                typeof(long?), typeof(short?), typeof(byte?), typeof(uint?), typeof(ulong?),
                typeof(ushort?), typeof(sbyte?)
            };

            return numericTypes.Contains(type) ||
                   numericTypes.Contains(Nullable.GetUnderlyingType(type));
        }
    }
}