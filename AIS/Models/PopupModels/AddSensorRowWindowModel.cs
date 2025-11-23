using AIS.Services;
using AIS.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models.PopupModels
{
    public class AddSensorRowWindowModel
    {
        public string SensorType;
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
            var updateResult = await _apiService.AddSensorRow(SensorType, SelectedGreenhouse.ID);
            return updateResult;
        }


    }
}
