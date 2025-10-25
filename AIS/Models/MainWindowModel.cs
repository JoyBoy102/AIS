using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS.Views;

namespace AIS.Models
{
    public class MainWindowModel
    {
        public ObservableCollection<Module> Modules { get; set; }

        public MainWindowModel()
        {
            Modules = new ObservableCollection<Module> { new Module { ModuleName = "Модуль мониторинга", Control = new MonitoringModuleView() },
                                                         new Module { ModuleName = "Модуль отчетности"} };
        }
    }
}
