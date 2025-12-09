using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class SensorReading
    {
        [JsonPropertyName("sensor_id")]
        public int SensorId { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("reading_time")]
        public string ReadingTime { get; set; }

        [JsonPropertyName("greenhouse_id")]
        public int GreenhouseId { get; set; }

        [JsonPropertyName("greenhouse_name")]
        public string GreenhouseName { get; set; }

        [JsonPropertyName("greenhouse_location")]
        public string GreenhouseLocation { get; set; }

        [JsonPropertyName("greenhouse_description")]
        public string GreenhouseDescription { get; set; }
        public double? TemperaturePower { get; set; }
        public double? HumidityPower { get; set; }
        public double? Co2Power { get; set; }

        public string CurrentPower { get; set; }
    }
}
