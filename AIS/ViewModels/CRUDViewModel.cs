using AIS.Models;
using AIS.Services;
using AIS.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.ViewModels
{
    public class CRUDViewModel: BaseViewModel
    {
        private CRUDModel _CRUDmodel;
        public IAsyncRelayCommand DeleteRowGreenhouseCommand { get; set; }
        public IRelayCommand UpdateRowGreenhouseCommand { get; set; }
        public IAsyncRelayCommand DeleteRowSensorCommand { get; set; }

        public IRelayCommand UpdateRowSensorCommand { get; set; }

        public CRUDViewModel(CRUDModel model)
        {
            _CRUDmodel = model;
            DeleteRowGreenhouseCommand = new AsyncRelayCommand<int>(DeleteRowGreenhouse);
            UpdateRowGreenhouseCommand = new RelayCommand<Greenhouse>(UpdateRowGreenhouse);
            DeleteRowSensorCommand = new AsyncRelayCommand<int>(DeleteRowSensor);
            UpdateRowSensorCommand = new RelayCommand<Sensor>(UpdateRowSensor);
            EventAggregator.GreenhouseRowUpdated+=(async () => await ReadGreenhouses());
            EventAggregator.SensorRowUpdated+=(async () => await ReadSensors());
        }

        public static async Task<CRUDViewModel> CreateAsync()
        {
            var model = await CRUDModel.CreateAsync();
            return new CRUDViewModel(model);
        }
        public ObservableCollection<Greenhouse> Greenhouses
        {
            get => _CRUDmodel.Greenhouses;
            set
            {
                _CRUDmodel.Greenhouses = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Sensor> Sensors
        {
            get => _CRUDmodel.Sensors;
            set
            {
                _CRUDmodel.Sensors = value;
                OnPropertyChanged();
            }
        }

        #region Greenhouses

        public async Task ReadGreenhouses()
        {
            await _CRUDmodel.UpdateGreenhouses();
            Greenhouses = _CRUDmodel.Greenhouses;
            OnPropertyChanged();
        }

        private async Task DeleteRowGreenhouse(int greenhouseId)
        {
            bool deleteResult = await _CRUDmodel.DeleteRowGreenhouse(greenhouseId);
            if (deleteResult)
            {
                await ReadGreenhouses();
            }
        }

        private void UpdateRowGreenhouse(Greenhouse greenhouse)
        {
            var updateWindow = new UpdateGreenhouseRowWindow();
            var updateWindowViewModel = new UpdateGreenhouseRowWindowViewmodel(greenhouse, updateWindow) { WindowName = "Редактирование данных теплицы"};
            updateWindow.DataContext = updateWindowViewModel;
            updateWindow.ShowDialog();
        }

        private void CreateRowGreenhouse(Greenhouse greenhouse)
        {
            var updateWindow = new UpdateGreenhouseRowWindow();
            var updateWindowViewModel = new UpdateGreenhouseRowWindowViewmodel(greenhouse, updateWindow);
            updateWindow.DataContext = updateWindowViewModel;
            updateWindow.ShowDialog();
        }
        #endregion

        #region Sensors
        public async Task ReadSensors()
        {
            await _CRUDmodel.UpdateSensors();
            Sensors = _CRUDmodel.Sensors;
            OnPropertyChanged();
        }

        private async Task DeleteRowSensor(int sensorId)
        {
            bool deleteResult = await _CRUDmodel.DeleteRowSensor(sensorId);
            if (deleteResult)
            {
                await ReadSensors();
            }
        }

        private void UpdateRowSensor(Sensor sensor)
        {
            var updateWindow = new UpdateSensorRowWindow();
            var updateWindowViewModel = new UpdateSensorRowWindowViewmodel(sensor, updateWindow);
            updateWindow.DataContext = updateWindowViewModel;
            updateWindow.ShowDialog();
        }
        #endregion
    }
}
