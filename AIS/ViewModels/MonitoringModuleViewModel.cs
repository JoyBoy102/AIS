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
using System.Windows.Threading;

namespace AIS.ViewModels
{
    public class MonitoringModuleViewModel : INotifyPropertyChanged
    {
        public IRelayCommand<Greenhouse> ShowSensorsTableCommand { get; }
        private MonitoringModuleModel _monitoringModuleModel;
        private DispatcherTimer _refreshTimer;
        public MonitoringModuleViewModel(MonitoringModuleModel model)
        {
            _monitoringModuleModel = model;
            ShowSensorsTableCommand = new RelayCommand<Greenhouse>(ShowSensorsTable);
            InitializeDispatcherTimer();
        }

        private void InitializeDispatcherTimer()
        {
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(0.5);
            _refreshTimer.Tick += async (sender, e) => await RefreshDataAsync();
            _refreshTimer.Start();
        }

        private async Task RefreshDataAsync()
        {
            try
            {
                await _monitoringModuleModel.RefreshDataAsync();
                OnPropertyChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Refresh error: {ex.Message}");
            }
        }

        public static async Task<MonitoringModuleViewModel> CreateAsync()
        {
            var model = await MonitoringModuleModel.CreateAsync();
            return new MonitoringModuleViewModel(model);
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
