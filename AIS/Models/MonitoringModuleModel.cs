using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Models
{
    public class MonitoringModuleModel
    {
        public ObservableCollection<Greenhouse> greenhouses;

        public MonitoringModuleModel()
        {
            greenhouses = new ObservableCollection<Greenhouse> { new Greenhouse { Name = "Основная теплица",
                                                                                  Location = "Уфа",
                                                                                  Description = "Тест",
                                                                                  SensorsTableVisibility = System.Windows.Visibility.Collapsed,
                                                                                  ToggleButtonState = false,
                                                                                  Sensors = new ObservableCollection<Sensor>{ new Sensor { ID = 1, Type = "temp", Value = 25 },
                                                                                                                              new Sensor { ID = 2, Type = "CO2", Value = 800 },
                                                                                                                              new Sensor { ID = 3, Type = "Humidility", Value = 67 } } }
                                                                };
        }
    }
}
