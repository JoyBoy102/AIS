using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class GreenhouseExecutionDevicesPowersWithID
    {
        public double? TemperaturePower { get; set; }
        public double? HumidityPower { get; set; }
        public double? Co2Power { get; set; }

        public int GreenhouseId { get; set; }
    }
}
