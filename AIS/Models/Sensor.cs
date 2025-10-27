using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class Sensor
    {
        [JsonPropertyName("reading_id")]
        public int ReadingID { get; set; }

        [JsonPropertyName("sensor_id")]
        public int ID { get; set; }
        [JsonPropertyName("sensor_type")]
        public string Type { get; set; }
        [JsonPropertyName("value")]
        public double Value { get; set; }
        [JsonPropertyName("reading_time")]
        public string ReadingTime { get; set; }
        [JsonPropertyName("greenhouse_id")]
        public int GreenhouseID { get; set; }
        [JsonPropertyName("greenhouse_name")]
        public string GreenhouseName { get; set; }
        [JsonPropertyName("greenhouse_description")]
        public string GreenhouseDescription { get; set; }
        [JsonPropertyName("greenhouse_location")]
        public string GreenhouseLocation {  get; set; }

    }

}
