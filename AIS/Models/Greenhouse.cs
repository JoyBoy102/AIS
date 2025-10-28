using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Models
{
    public class Greenhouse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public ObservableCollection<Sensor> Sensors { get; set; }

        public bool ToggleButtonState { get; set; }

        public Visibility SensorsTableVisibility { get; set; }

    }
}
