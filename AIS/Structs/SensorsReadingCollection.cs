using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class SensorsReadingCollection : ObservableCollection<SensorReading>
    {
        // Конструктор по умолчанию
        public SensorsReadingCollection() : base()
        {
        }

        // Конструктор, принимающий IEnumerable<SensorReading>
        public SensorsReadingCollection(IEnumerable<SensorReading> collection)
            : base(collection)
        {
        }

        // Конструктор, принимающий List<SensorReading> (опционально)
        public SensorsReadingCollection(List<SensorReading> list)
            : base(list)
        {
        }
    }
}
