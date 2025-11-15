using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class Report
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("greenhouse_id")]
        public int IdGreenHouse { get; set; }
        [JsonPropertyName("co2_value")]
        public string CO2Value { get; set; }
        [JsonPropertyName("humidity_value")]
        public string HumidityValue { get; set; }
        [JsonPropertyName("temperature_value")]
        public string TemperatureValue { get; set; }
        [JsonPropertyName("co2_pred")]
        public string CO2Pred {  get; set; }
        [JsonPropertyName("humidity_pred")]
        public string HumidityPred { get; set; }
        [JsonPropertyName("temperature_pred")]
        public string TemperaturePred { get; set; }
        [JsonPropertyName("command_co2")]
        public string commandCO2 { get; set; }
        [JsonPropertyName("command_humidity")]
        public string commandHumidity { get; set; }
        [JsonPropertyName("command_temperature")]
        public string commandTemperature { get; set; }
        [JsonPropertyName("report_time")]
        public string ReportTime { get; set; }
    }
}
