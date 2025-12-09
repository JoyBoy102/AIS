using AIS.Services;
using AIS.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class ManualCommandsSetPageModel
    {
        public ObservableCollection<ExecutionDevice> Devices = new ObservableCollection<ExecutionDevice>();
        public ExecutionDevice SelectedDevice;
        public int PowerValue;
        private ApiService _apiService;
        private bool _autoMode;

        public ManualCommandsSetPageModel()
        {
            _apiService = new ApiService(new HttpClient());
            EventAggregator.AutoModeChanged += OnAutoModeChanged;
        }

        private void OnAutoModeChanged(bool value)
        {
            _autoMode = value;
        }

        public static async Task<ManualCommandsSetPageModel> CreateAsync()
        {
            var instance = new ManualCommandsSetPageModel();
            await instance.InitializeAsync();
            return instance;
        }

        public async Task Apply()
        {
            bool autoMode = await GetAutoModeAsync();
            if (!autoMode)
                await _apiService.ApplyExecutionDevicePower(PowerValue, $"greenhouse_{SelectedDevice.GreenhouseID}", SelectedDevice.Type);
            else
                MessageService.ShowInfo("Для изменения мощности исполнительного устройства отключите автоматический режим");
        }

        private async Task InitializeAsync()
        {
            await InitializeAsyncExecutionDevices();
            SelectedDevice = Devices[0];
        }

        private async Task InitializeAsyncExecutionDevices()
        {
            List<ExecutionDevice> executionDevices = await _apiService.GetExecutionDevicesTableAsync();
            List<Greenhouse> greenhouses = await _apiService.GetGreenhousesTableAsync();
            List<ExecutionDevice> joinedData = executionDevices.Join(
                                    greenhouses,
                                    device => device.GreenhouseID,
                                    greenhouse => greenhouse.ID,
                                    (device, greenhouse) => new ExecutionDevice
                                    {
                                        ID = device.ID,
                                        GreenhouseID = greenhouse.ID,
                                        SensorID = device.SensorID,
                                        Type = device.Type,
                                        GreenhouseName = greenhouse.Name
                                    }
                                ).ToList();
            Devices = new ObservableCollection<ExecutionDevice>(joinedData);
        }
        public async Task<bool> GetAutoModeAsync()
        {
            bool res = await _apiService.GetPeriodicReportsStatusAsync();
            return res;
        }
    }
}
