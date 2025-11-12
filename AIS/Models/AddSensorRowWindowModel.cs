using AIS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class AddSensorRowWindowModel
    {
        public int SensorID;
        public string SensorType;
        public int GreenhouseID;
        private ApiService _apiService;
        public List<Greenhouse> Greenhouses;
        public Greenhouse SelectedGreenhouse;

        public AddSensorRowWindowModel(ApiService apiService, List<Greenhouse> greenhouses)
        {
            _apiService = apiService;
            Greenhouses = greenhouses;
        }

        public static async Task<AddSensorRowWindowModel> CreateAsync()
        {
            var apiService = new ApiService(new System.Net.Http.HttpClient());
            var greenhouses = await apiService.GetGreenhousesTableAsync();
            return new AddSensorRowWindowModel(apiService, greenhouses);
        }

        public async Task<bool> AddSensorRow()
        {
            var updateResult = await _apiService.AddSensorRow(SensorType, GreenhouseID);
            return updateResult;
        }
    }
}
