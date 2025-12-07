using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class ReportingStatusResponse
    {
        [JsonPropertyName("reporting_active")]
        public bool ReportingActive { get; set; }

        [JsonPropertyName("interval_seconds")]
        public int IntervalSeconds { get; set; }
    }
}
