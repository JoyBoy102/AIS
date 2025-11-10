using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class ExecutionDevice
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("greenhouse_id")]
        public int GreenhouseID { get; set; }
        [JsonPropertyName("sensor_id")]
        public int SensorID { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
