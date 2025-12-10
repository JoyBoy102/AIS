using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class Sensor
    {
        [JsonPropertyName("sensor_id")]
        public int ID { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("greenhouse_id")]
        public int GreenhouseID { get; set; }

        public Greenhouse Greenhouse { get; set; }

        public override string ToString()
        {
            return Type;
        }

    }

}
