using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AIS.Converters
{
    public class CollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            var collection = value as IEnumerable;
            if (collection == null) return string.Empty;

            string propertyName = parameter as string ?? "Value";

            var result = new StringBuilder();
            foreach (var item in collection)
            {
                var property = item.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    var propertyValue = property.GetValue(item);
                    result.AppendLine(propertyValue?.ToString() ?? string.Empty);
                }
            }

            return result.ToString().TrimEnd();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
