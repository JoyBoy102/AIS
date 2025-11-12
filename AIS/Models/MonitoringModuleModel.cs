using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AIS.Services;
using AIS.Structs;

namespace AIS.Models
{
    public class MonitoringModuleModel
    {
        public ObservableCollection<Greenhouse> greenhouses;
        private ApiService _apiService;
        public MonitoringModuleModel()
        {
            _apiService = new ApiService(new System.Net.Http.HttpClient());
            greenhouses = new ObservableCollection<Greenhouse>();
        }

        public static async Task<MonitoringModuleModel> CreateAsync()
        {
            var instance = new MonitoringModuleModel();
            await instance.InitializeAsync();
            return instance;
        }
        
        private async Task InitializeAsync()
        {
            var sensorReadingsList = await GetSensorsReadingsList();
            greenhouses = GetGreenhouseObservableCollection(sensorReadingsList);
        }

        private async Task<List<SensorReading>> GetSensorsReadingsList()
        {
            var sensorReadings = await _apiService.GetGreenhousesMonitoringInfoAsync();
            return sensorReadings;
        }

        private ObservableCollection<Greenhouse> GetGreenhouseObservableCollection(List<SensorReading> sensorReadings)
        {
            var result = new ObservableCollection<Greenhouse>();
            var groupedByGreenhouse = sensorReadings.GroupBy(reading => new { reading.GreenhouseID, reading.GreenhouseName, reading.GreenhouseLocation, reading.GreenhouseDescription });
            foreach (var group in groupedByGreenhouse)
            {
                Greenhouse greenhouse = new Greenhouse();
                greenhouse.ID = group.Key.GreenhouseID;
                greenhouse.Name = group.Key.GreenhouseName;
                greenhouse.Location = group.Key.GreenhouseLocation;
                greenhouse.Description = group.Key.GreenhouseDescription;
                greenhouse.SensorsReadings = new ObservableCollection<SensorReading>();
                foreach (var sensor in group)
                {
                    greenhouse.SensorsReadings.Add(sensor);
                }
                result.Add(greenhouse);
            }
            return result;
        }

        public async Task RefreshDataAsync()
        {
            var sensorReadingsList = await GetSensorsReadingsList();
            UpdateGreenhouseSensors(sensorReadingsList);
        }

        private void UpdateGreenhouseSensors(List<SensorReading> sensorReadingsList)
        {
            var sensorsByGreenhouse = sensorReadingsList
                .GroupBy(s => s.GreenhouseID)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var greenhouse in greenhouses)
            {
                if (sensorsByGreenhouse.TryGetValue(greenhouse.ID, out var newSensors))
                {
                    // Вместо замены коллекции обновляем существующую
                    if (greenhouse.SensorsReadings == null)
                    {
                        greenhouse.SensorsReadings = new ObservableCollection<SensorReading>(newSensors);
                    }
                    else
                    {
                        // Очищаем и добавляем в существующую коллекцию
                        // ObservableCollection автоматически уведомляет об изменениях
                        greenhouse.SensorsReadings.Clear();
                        foreach (var sensor in newSensors)
                        {
                            greenhouse.SensorsReadings.Add(sensor);
                        }
                    }
                }
            }
        }

        public async Task UpdateGreenhouses()
        {
            await InitializeAsync();
        }
    }
}
