using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class ApiResponse
    {
        public List<SensorReading> Readings { get; set; }
        public Metadata Metadata { get; set; }
        public bool Cached { get; set; }
    }
}
