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
        private static ApiService _apiService;
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

        public static async Task<List<SensorReading>> GetSensorsReadingsList()
        {
            var sensorReadings = await _apiService.GetGreenhousesMonitoringInfoAsync();
            var executionDevicesPowers = await GetExecutionDevicesPowersAsync();

            foreach (var reading in sensorReadings)
            {
                reading.CurrentPower = FindCurrentPower(reading.Type, reading.GreenhouseId, executionDevicesPowers);
            }

            return sensorReadings;
        }

        private string FindCurrentPower(string type, int greenhouseId, List<GreenhouseExecutionDevicesPowersWithID> executionDevicesPowers)
        {
            foreach (var device in executionDevicesPowers)
            {
                if (device.GreenhouseId == greenhouseId)
                {
                    switch (type)
                    {
                        case "temperature":
                            return device.TemperaturePower != null? device.TemperaturePower.ToString():"Отсутствует";
                        case "humidity":
                            return device.HumidityPower != null ? device.HumidityPower.ToString() : "Отсутствует";
                        case "co2":
                            return device.Co2Power != null ? device.Co2Power.ToString() : "Отсутствует";

                    }
                }
            }
            return "Отсутствует";
        }

        private static async Task<Dictionary<string, ExecutionDevicesPowerSingleGreenhouseInfo>> GetExecutionDevicePowerInfoAsync()
        {
            var info = await _apiService.GetGreenhousesExecutionDevicesPowerAsync();
            return info;
        }

        private static async Task<List<GreenhouseExecutionDevicesPowersWithID>> GetExecutionDevicesPowersAsync()
        {
            var infoDict = await GetExecutionDevicePowerInfoAsync();

            var result = new List<GreenhouseExecutionDevicesPowersWithID>();

            foreach (var kvp in infoDict)
            {
                int greenhouseID;
                bool convertRes = int.TryParse(kvp.Key.Replace("greenhouse_", ""), out greenhouseID);
                if (convertRes)
                {
                    
                }
                var info = new GreenhouseExecutionDevicesPowersWithID
                {
                    Co2Power = kvp.Value.Co2Power,
                    GreenhouseId = greenhouseID,
                    HumidityPower = kvp.Value.HumidityPower,
                    TemperaturePower = kvp.Value.TemperaturePower
                };
                result.Add(info);
            }

            // Сортируем по ID теплицы
            return result.OrderBy(g => g.GreenhouseId).ToList();
        }



        private ObservableCollection<Greenhouse> GetGreenhouseObservableCollection(List<SensorReading> sensorReadings)
        {
            var result = new ObservableCollection<Greenhouse>();
            var groupedByGreenhouse = sensorReadings.GroupBy(reading => new { reading.GreenhouseId, reading.GreenhouseName, reading.GreenhouseLocation, reading.GreenhouseDescription });
            foreach (var group in groupedByGreenhouse)
            {
                Greenhouse greenhouse = new Greenhouse();
                greenhouse.ID = group.Key.GreenhouseId;
                greenhouse.Name = group.Key.GreenhouseName;
                greenhouse.Location = group.Key.GreenhouseLocation;
                greenhouse.Description = group.Key.GreenhouseDescription;
                greenhouse.SensorsReadingCollection = new SensorsReadingCollection();
                foreach (var sensor in group)
                {
                    greenhouse.SensorsReadingCollection.Add(sensor);
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
                .GroupBy(s => s.GreenhouseId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var greenhouse in greenhouses)
            {
                if (sensorsByGreenhouse.TryGetValue(greenhouse.ID, out var newSensors))
                {
                    // Вместо замены коллекции обновляем существующую
                    if (greenhouse.SensorsReadingCollection == null)
                    {
                        greenhouse.SensorsReadingCollection = new SensorsReadingCollection(newSensors);
                    }
                    else
                    {
                        // Очищаем и добавляем в существующую коллекцию
                        // ObservableCollection автоматически уведомляет об изменениях
                        greenhouse.SensorsReadingCollection.Clear();
                        foreach (var sensor in newSensors)
                        {
                            greenhouse.SensorsReadingCollection.Add(sensor);
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
