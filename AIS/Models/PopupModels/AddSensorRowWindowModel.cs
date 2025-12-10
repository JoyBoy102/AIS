using AIS.Services;
using AIS.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models.PopupModels
{
    public class AddSensorRowWindowModel
    {
        public Sensor SelectedSensor;
        private ApiService _apiService;
        public List<Greenhouse> Greenhouses;
        public Greenhouse SelectedGreenhouse;

        public AddSensorRowWindowModel()
        {
        }

        public static async Task<AddSensorRowWindowModel> CreateAsync()
        {
            var instance = new AddSensorRowWindowModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            _apiService = new ApiService(new HttpClient());
            var greenhouses = await _apiService.GetGreenhousesTableAsync();
            var sensors = await _apiService.GetSensorsTableAsync();
            var groupedSensors = sensors.GroupBy(sensor => sensor.GreenhouseID)
                                        .ToDictionary(group => group.Key, group => group.ToList());
            foreach (var gr in greenhouses)
            {
                gr.NotIncludedSensors = FindNotIncludedSensors(groupedSensors, gr.ID);
            }

            Greenhouses = greenhouses;
        }

        private ObservableCollection<Sensor> FindNotIncludedSensors(Dictionary<int, List<Sensor>> sensors_dict, int ID)
        {
            List<string> all_types = new List<string> { "temperature", "co2", "humidity" };
            if (sensors_dict.ContainsKey(ID))
            {
                var sensors = sensors_dict[ID];
                foreach (var sensor in sensors)
                {
                    all_types.Remove(sensor.Type);
                }
                var res = new ObservableCollection<Sensor>();
                foreach (var type in all_types)
                {
                    Sensor sensor = new Sensor
                    {
                        Type = type,
                    };
                    res.Add(sensor);
                }
                return res;
            }
            else
            {
                return new ObservableCollection<Sensor>(all_types.Select(x => new Sensor { Type = x}).ToList());
            }
        }

        public async Task<bool> AddSensorRow()
        {
            var updateResult = await _apiService.AddSensorRow(SelectedSensor.Type, SelectedGreenhouse.ID);
            return updateResult;
        }


    }
}
