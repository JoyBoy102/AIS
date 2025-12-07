using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class ExecutionDevicesPowerSingleGreenhouseInfo
    {
        [JsonPropertyName("temperature_power")]
        public double? TemperaturePower { get; set; }

        [JsonPropertyName("humidity_power")]
        public double? HumidityPower { get; set; }

        [JsonPropertyName("co2_power")]
        public double? Co2Power { get; set; }
    }
}
