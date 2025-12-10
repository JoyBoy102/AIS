using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AIS.Converters
{
    public class EngToRus : IValueConverter
    {
        // Словарь для перевода (можно вынести в отдельный файл/ресурс)
        private static readonly Dictionary<string, string> _translations = new Dictionary<string, string>
        {
            // Примеры переводов для типов
            { "temperature", "Температура" },
            { "humidity", "Влажность" },
            { "co2", "Углекислый газ" },

            { "temperature_controller", "Контроллер температуры" },
            { "humidity_controller", "Контроллер влажности" },
            { "co2_controller", "Контроллер углекислого газа" },
    
            // Переводы для свойств
            { "IdGreenHouse", "ID Теплицы" },
            { "ReportTime", "Время" },
            { "TemperatureValue", "Температура" },
            { "CO2Value", "CO2" },
            { "HumidityValue", "Влажность" },
            { "CO2Pred", "Прогноз уровня CO2" },
            { "commandCO2", "Команда для исполнительного устройства CO2" },
            { "commandHumidity", "Команда для увлажнителя" },
            { "commandTemperature", "Команда для обогревателя" },
            { "HumidityPred", "Прогноз уровня влажности" },
            { "TemperaturePred", "Прогноз температуры" },

        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "не указано";

            string strValue = value.ToString();

            // Если есть перевод в словаре - возвращаем его
            if (_translations.TryGetValue(strValue, out string translatedValue))
                return translatedValue;

            // Если нет перевода - возвращаем оригинальное значение
            // Можно добавить логику для обработки составных типов
            return strValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string strValue = value.ToString();

            // Ищем обратный перевод (русский -> английский)
            var reverseTranslation = _translations
                .FirstOrDefault(x => x.Value.Equals(strValue, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(reverseTranslation.Key))
                return reverseTranslation.Key;

            // Если не нашли - возвращаем как есть
            return strValue;
        }
    }
}