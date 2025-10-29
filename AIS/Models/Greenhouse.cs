using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Models
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

        public ObservableCollection<Sensor> Sensors { get; set; }

        public bool ToggleButtonState { get; set; }

        public Visibility SensorsTableVisibility { get; set; }

    }
}
