using AIS.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS.ViewModels
{
    public class MonitoringModuleViewModel : INotifyPropertyChanged
    {
        public IRelayCommand<Greenhouse> ShowSensorsTableCommand { get; }
        private MonitoringModuleModel _monitoringModuleModel;
        public MonitoringModuleViewModel()
        {
            _monitoringModuleModel = new MonitoringModuleModel();
            ShowSensorsTableCommand = new RelayCommand<Greenhouse>(ShowSensorsTable);
        }

        public ObservableCollection<Greenhouse> GreenhouseItems
        {
            get => _monitoringModuleModel.greenhouses;
            set
            {
                _monitoringModuleModel.greenhouses = value;
                OnPropertyChanged();
            }
        }

        private void ShowSensorsTable(Greenhouse greenhouse)
        {
            if (greenhouse.ToggleButtonState) greenhouse.SensorsTableVisibility = System.Windows.Visibility.Visible;
            else greenhouse.SensorsTableVisibility = System.Windows.Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
