using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class DetectionImage
    {
        [JsonPropertyName("greenhouse_id")]
        public int GreenhouseID { get; set; }
        [JsonPropertyName("id")]
        public int DetectionID { get; set; }
        [JsonPropertyName("created_at")]
        public string DetectionTime { get; set; }
        [JsonPropertyName("confidence_level")]
        public double Confidence { get; set; }
        public string ImagePath { get; set; }
        [JsonIgnore]
        public bool IsWeed { get; set; }
        [JsonIgnore]
        public string GreenhouseName { get; set; }
    }
}
