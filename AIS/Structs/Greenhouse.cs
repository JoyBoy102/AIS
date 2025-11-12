using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Structs
{
    public class Greenhouse
    {
        [JsonPropertyName("greenhouse_id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("agrorule_id")]
        public int AgronomicRuleId { get; set; }

        public AgronomicRuleModel AgronomicRule { get; set; }

        public ObservableCollection<SensorReading> SensorsReadings { get; set; }

        public bool ToggleButtonState { get; set; }

        public Visibility SensorsTableVisibility { get; set; }

        public override string ToString()
        {
            return Name.ToString();
        }

    }
}
