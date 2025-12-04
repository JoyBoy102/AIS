using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS.Structs;
using AIS.Views;
using Wpf.Ui.Controls;

namespace AIS.Models
{
    public class MainWindowModel
    {
        public ObservableCollection<Module> Modules { get; set; }

        public MainWindowModel()
        {
            Modules = new ObservableCollection<Module> { new Module { ModuleName = "Модуль мониторинга", Control = new MonitoringModuleView(), ModuleIcon = SymbolRegular.Gauge24},
                                                         new Module { ModuleName = "Модуль формирования отчетов", Control = new ReportModuleView(), ModuleIcon = SymbolRegular.DocumentText24 },
                                                         new Module { ModuleName = "Модуль изменения таблиц", Control = new CRUDView(), ModuleIcon = SymbolRegular.Edit24 },
                                                         new Module { ModuleName = "Модуль детекций сорняков", Control = new WeedDetectionModule(), ModuleIcon = SymbolRegular.Scan24} };
        }
            
    }
}


