using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class Metadata
    {
        public string Timestamp { get; set; }
        public int Season { get; set; }
        public int TimeOfDay { get; set; }
        public string SeasonName { get; set; }
        public string TimeOfDayName { get; set; }
    }
}
