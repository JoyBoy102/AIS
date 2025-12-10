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
        public ExecutionDevice SelectedDevice = null;
        public double PowerValue = 0;
        private ApiService _apiService;
        private bool _autoMode;
        public string SelectedGreenhouse;

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

        public async Task<double?> GetPower()
        {
            var d = await MonitoringModuleModel.GetSensorsReadingsList();
            var device = d.Where(d => d.GreenhouseName == SelectedGreenhouse)
                          .Where(dd => dd.Type == SelectedDevice?.Type.Split('_')[0])
                          .FirstOrDefault();
            Double.TryParse(device.CurrentPower, out double res);
            return res;
        }

        private async Task InitializeAsync()
        {
            await InitializeAsyncExecutionDevices();
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
