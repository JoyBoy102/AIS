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
    public class CRUDModel
    {
        private ApiService _apiService;

        #region Properties
        public ObservableCollection<Greenhouse> Greenhouses { get; set; } = new ObservableCollection<Greenhouse>();
        public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();
        public ObservableCollection<ExecutionDevice> ExecutionDevices { get; set; } = new ObservableCollection<ExecutionDevice>();
        #endregion

        #region Constructor
        public CRUDModel()
        {
            _apiService = new ApiService(new HttpClient());
        }

        public static async Task<CRUDModel> CreateAsync()
        {
            var instance = new CRUDModel();
            await instance.InitializeAsync();
            return instance;
        }
        #endregion

        #region Initialization
        private async Task InitializeAsync()
        {
            await InitializeAsyncGreenhouses();
            await InitializeAsyncSensors();
            await InitializeAsyncExecutionDevices();
        }

        private async Task InitializeAsyncGreenhouses()
        {
            List<Greenhouse> greenhousesList = await _apiService.GetGreenhousesTableAsync();
            Greenhouses = new ObservableCollection<Greenhouse>(greenhousesList);
        }

        private async Task InitializeAsyncSensors()
        {
            List<Sensor> sensorsList = await _apiService.GetSensorsTableAsync();
            Sensors = new ObservableCollection<Sensor>(sensorsList);
        }

        private async Task InitializeAsyncExecutionDevices()
        {
            List<ExecutionDevice> executionDevices = await _apiService.GetExecutionDevicesTableAsync();
            ExecutionDevices = new ObservableCollection<ExecutionDevice>(executionDevices);
        }
        #endregion

        #region Greenhouses Methods
        public async Task<bool> DeleteRowGreenhouse(int greenhouseID)
        {
            bool deleteResult = await _apiService.DeleteGreenhouseByIdFromDB(greenhouseID);
            return deleteResult;
        }

        public async Task UpdateGreenhouses()
        {
            await InitializeAsyncGreenhouses();
        }
        #endregion

        #region Sensors Methods
        public async Task<bool> DeleteRowSensor(int sensorId)
        {
            bool deleteResult = await _apiService.DeleteSensorByIdFromDB(sensorId);
            return deleteResult;
        }

        public async Task UpdateSensors()
        {
            await InitializeAsyncSensors();
        }
        #endregion

        #region ExecutionDevices Methods
        public async Task<bool> DeleteRowExecutionDevices(int deviceId)
        {
            bool deleteResult = await _apiService.DeleteExecutionDeviceByIdFromDB(deviceId);
            return deleteResult;
        }

        public async Task UpdateExecutionDevices()
        {
            await InitializeAsyncExecutionDevices();
        }
        #endregion
    }
}