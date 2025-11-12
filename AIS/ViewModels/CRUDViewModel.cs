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
using System.Windows.Controls;

namespace AIS.ViewModels
{
    public class CRUDViewModel: BaseViewModel
    {
        private CRUDModel _CRUDmodel;
        public IAsyncRelayCommand AddCommand { get; set; }
        public IAsyncRelayCommand DeleteRowGreenhouseCommand { get; set; }
        public IRelayCommand UpdateRowGreenhouseCommand { get; set; }
        public IAsyncRelayCommand DeleteRowSensorCommand { get; set; }
        public IRelayCommand UpdateRowSensorCommand { get; set; }
        public IAsyncRelayCommand DeleteRowExecutionDeviceCommand { get; set; }
        public IRelayCommand UpdateRowExecutionDeviceCommand { get; set; }


        public CRUDViewModel(CRUDModel model)
        {
            _CRUDmodel = model;
            AddCommand = new AsyncRelayCommand(Add);
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

        #region Add mapper
        private int selectedTableIndex;
        public int SelectedTableIndex { get => selectedTableIndex; set => SetProperty(ref selectedTableIndex, value); }

        private async Task Add()
        {
            switch (SelectedTableIndex)
            {
                case 0: // Теплицы
                    AddGreenhouse();
                    break;
                case 1: // Датчики
                    AddSensor();
                    break;
                case 2: // Исполнительные устройства
                    AddExecutionDevice();
                    break;
            }
        }

        private void AddExecutionDevice()
        {
            throw new NotImplementedException();
        }

        private void AddSensor()
        {
            var updateWindow = new AddSensorRowWindow();
            var updateWindowViewModel = updateWindow.DataContext;
            updateWindow.ShowDialog();
        }

        private async Task AddGreenhouse()
        {
            var updateWindow = new AddGreenhouseRowWindow();
            var updateWindowViewModel = updateWindow.DataContext;
            updateWindow.ShowDialog();
        }
        #endregion

        #region Greenhouses
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

        private void UpdateRowGreenhouse(Greenhouse? greenhouse)
        {
            if (greenhouse == null) return;
            var updateWindow = new UpdateGreenhouseRowWindow();
            var updateWindowViewModel = new UpdateGreenhouseRowWindowViewmodel(greenhouse, updateWindow);
            updateWindow.DataContext = updateWindowViewModel;
            updateWindow.ShowDialog();
        }
        #endregion

        #region Sensors
        public ObservableCollection<Sensor> Sensors
        {
            get => _CRUDmodel.Sensors;
            set
            {
                _CRUDmodel.Sensors = value;
                OnPropertyChanged();
            }
        }
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

        private void UpdateRowSensor(Sensor? sensor)
        {
            if (sensor == null) return;
            var updateWindow = new UpdateSensorRowWindow();
            var updateWindowViewModel = new UpdateSensorRowWindowViewmodel(sensor, updateWindow);
            updateWindow.DataContext = updateWindowViewModel;
            updateWindow.ShowDialog();
        }

        public async Task UpdateSensors()
        {
            await _CRUDmodel.UpdateSensors();
            Sensors = _CRUDmodel.Sensors;
            OnPropertyChanged();
        }
        #endregion


        #region ExecutionDevices
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

        private void UpdateRowExecutionDevice(ExecutionDevice? sensor)
        {
            //
        }
        #endregion
    }
}
