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

        public IAsyncRelayCommand DeleteRowExecutionDeviceCommand { get; set; }

        public IRelayCommand UpdateRowExecutionDeviceCommand { get; set; }

        public CRUDViewModel(CRUDModel model)
        {
            _CRUDmodel = model;
            DeleteRowGreenhouseCommand = new AsyncRelayCommand<int>(DeleteRowGreenhouse);
            UpdateRowGreenhouseCommand = new RelayCommand<Greenhouse>(UpdateRowGreenhouse);
            DeleteRowSensorCommand = new AsyncRelayCommand<int>(DeleteRowSensor);
            UpdateRowSensorCommand = new RelayCommand<Sensor>(UpdateRowSensor);
            DeleteRowExecutionDeviceCommand = new AsyncRelayCommand<int>(DeleteRowExecutionDevices);
            UpdateRowExecutionDeviceCommand = new RelayCommand<ExecutionDevice>(UpdateRowExecutionDevice);
            EventAggregator.GreenhouseRowUpdated+=(async () => await UpdateGreenhouses());
            EventAggregator.SensorRowUpdated+=(async () => await UpdateSensors());
            EventAggregator.ExecutionDeviceRowUpdated+=(async () => await UpdateExecutionDevices());
        }

        public static async Task<CRUDViewModel> CreateAsync()
        {
            var model = await CRUDModel.CreateAsync();
            return new CRUDViewModel(model);
        }

        //----------Greenhouses----------
        public ObservableCollection<Greenhouse> Greenhouses
        {
            get => _CRUDmodel.Greenhouses;
            set
            {
                _CRUDmodel.Greenhouses = value;
                OnPropertyChanged();
            }
        }

        public async Task UpdateGreenhouses()
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
                await UpdateGreenhouses();
            }
        }

        private void UpdateRowGreenhouse(Greenhouse greenhouse)
        {
            var updateWindow = new UpdateGreenhouseRowWindow();
            var updateWindowViewModel = new UpdateGreenhouseRowWindowViewmodel(greenhouse, updateWindow);
            updateWindow.DataContext = updateWindowViewModel;
            updateWindow.ShowDialog();
        }
        //----------Greenhouses----------


        //----------Sensors----------
        public ObservableCollection<Sensor> Sensors
        {
            get => _CRUDmodel.Sensors;
            set
            {
                _CRUDmodel.Sensors = value;
                OnPropertyChanged();
            }
        }


        public async Task UpdateSensors()
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
                await UpdateSensors();
            }
        }

        private void UpdateRowSensor(Sensor sensor)
        {
            var updateWindow = new UpdateSensorRowWindow();
            var updateWindowViewModel = new UpdateSensorRowWindowViewmodel(sensor, updateWindow);
            updateWindow.DataContext = updateWindowViewModel;
            updateWindow.ShowDialog();
        }
        //----------Sensors----------


        //----------ExecutionDevices----------
        public ObservableCollection<ExecutionDevice> ExecutionDevices
        {
            get => _CRUDmodel.ExecutionDevices;
            set
            {
                _CRUDmodel.ExecutionDevices = value;
                OnPropertyChanged();
            }
        }


        public async Task UpdateExecutionDevices()
        {
            await _CRUDmodel.UpdateExecutionDevices();
            ExecutionDevices = _CRUDmodel.ExecutionDevices;
            OnPropertyChanged();
        }



        private async Task DeleteRowExecutionDevices(int deviceId)
        {
            bool deleteResult = await _CRUDmodel.DeleteRowExecutionDevices(deviceId);
            if (deleteResult)
            {
                await UpdateExecutionDevices();
            }
        }

        private void UpdateRowExecutionDevice(ExecutionDevice sensor)
        {
            //
        }
        //----------ExecutionDevices----------
    }
}
