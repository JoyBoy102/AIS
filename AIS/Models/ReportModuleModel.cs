using AIS.Structs;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Models
{
    public class ReportModuleModel
    {
        public ObservableCollection<Report> reports;

        public ReportModuleModel()
        {
            this.reports = new ObservableCollection<Report> { 
                new Report {
                    IdGreenHouse = 20,
                    Temperature = "29",
                    CO2 = "400",
                    Humidity = "40%",
                    Time = "2025-10-27 18:38",
                    Commands = new ObservableCollection<Command>{ new Command { Value = "213123" },
                                                    new Command { Value = "213123" },
                                                    new Command { Value = "213123" } }
                },
                new Report {
                    IdGreenHouse = 20,
                    Temperature = "29",
                    CO2 = "400",
                    Humidity = "40%",
                    Time = "2025-10-27 18:38",
                    Commands = new ObservableCollection<Command>{ new Command { Value = "213123" },
                                                    new Command { Value = "213123" },
                                                    new Command { Value = "213123" } }
                },
                new Report {
                    IdGreenHouse = 20,
                    Temperature = "29",
                    CO2 = "400",
                    Humidity = "40%",
                    Time = "2025-10-27 18:38",
                    Commands = new ObservableCollection<Command>{ new Command { Value = "213123" },
                                                    new Command { Value = "213123" },
                                                    new Command { Value = "213123" } }
                },
            };
        }

        public ObservableCollection<Greenhouse> greenhouses { get; internal set; }
    }
}
