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
        public ObservableCollection<Greenhouse> Greenhouses;
        public ObservableCollection<Sensor> Sensors;
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
            return instance;
        }

        public async Task<bool> DeleteRowGreenhouse(int greenhouseID)
        {
           bool deleteResult = await _apiService.DeleteGreenhouseByIdFromDB(greenhouseID);
           return deleteResult;
        }

        public async Task<bool> DeleteRowSensor(int sensorId)
        {
            bool deleteResult = await _apiService.DeleteSensorByIdFromDB(sensorId);
            return deleteResult;
        }

        public async Task UpdateGreenhouses()
        {
            await InitializeAsyncGreenhouses();
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

        private async Task InitializeAsyncGreenhouses()
        {
            List<Greenhouse> greenhousesList = await _apiService.GetGreenhousesTableAsync();
            Greenhouses = new ObservableCollection<Greenhouse>(greenhousesList);
        }
    }
}
