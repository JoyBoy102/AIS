using AIS.Services;
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
        public ObservableCollection<Greenhouse> Greenhouses = new ObservableCollection<Greenhouse>();
        public ObservableCollection<Sensor> Sensors = new ObservableCollection<Sensor>();
        public ObservableCollection<ExecutionDevice> ExecutionDevices = new ObservableCollection<ExecutionDevice>();

        private ApiService _apiService;

        

        public CRUDModel()
        {
            _apiService = new ApiService(new HttpClient());
        }

        public static async Task<CRUDModel> CreateAsync()
        {
            var instance = new CRUDModel();
            await instance.InitializeAsyncGreenhouses();
            await instance.InitializeAsyncSensors();
            await instance.InitializeAsyncExecutionDevices();
            return instance;
        }

        //---------Greenhouses---------
        public async Task<bool> DeleteRowGreenhouse(int greenhouseID)
        {
           bool deleteResult = await _apiService.DeleteGreenhouseByIdFromDB(greenhouseID);
           return deleteResult;
        }

        private async Task InitializeAsyncGreenhouses()
        {
            List<Greenhouse> greenhousesList = await _apiService.GetGreenhousesTableAsync();
            Greenhouses = new ObservableCollection<Greenhouse>(greenhousesList);
        }

        public async Task UpdateGreenhouses()
        {
            await InitializeAsyncGreenhouses();
        }
        //---------Greenhouses---------

        //---------Sensors---------
        public async Task<bool> DeleteRowSensor(int sensorId)
        {
            bool deleteResult = await _apiService.DeleteSensorByIdFromDB(sensorId);
            return deleteResult;
        }

        public async Task UpdateSensors()
        {
            await InitializeAsyncSensors();
        }

        private async Task InitializeAsyncSensors()
        {
            List<Sensor> SensorsList = await _apiService.GetSensorsTableAsync();
            Sensors = new ObservableCollection<Sensor>(SensorsList);
        }
        //---------Sensors---------

        //---------ExecutionDevices---------
        public async Task<bool> DeleteRowExecutionDevices(int sensorId)
        {
            bool deleteResult = await _apiService.DeleteExecutionDeviceByIdFromDB(sensorId);
            return deleteResult;
        }

        public async Task UpdateExecutionDevices()
        {
            await InitializeAsyncExecutionDevices();
        }

        private async Task InitializeAsyncExecutionDevices()
        {
            List<ExecutionDevice> executionDevices = await _apiService.GetExecutionDevicesTableAsync();
            ExecutionDevices = new ObservableCollection<ExecutionDevice>(executionDevices);
        }
        //---------ExecutionDevices---------
    }
}
