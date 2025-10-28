using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class Report
    {
        public int IdGreenHouse { get; internal set; }
        public string Time { get; set; }
        public string Temperature { get; set; }
        public string CO2 { get; set; }
        public string Humidity { get; set; }
        public ObservableCollection<Command> Commands { get; set; }
    }
}
