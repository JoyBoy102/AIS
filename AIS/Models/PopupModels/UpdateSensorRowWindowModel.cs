using AIS.Services;
using AIS.Structs;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models.PopupModels
{
    public class UpdateSensorRowWindowModel
    {
        public int SensorID;
        public string SensorType;
        public int GreenhouseID;
        private ApiService _apiService;
        public List<Greenhouse> Greenhouses;
        public Greenhouse SelectedGreenhouse;

        public UpdateSensorRowWindowModel(ApiService apiService, List<Greenhouse> greenhouses)
        {
            _apiService = apiService;
            Greenhouses = greenhouses;
        }

        public static async Task<UpdateSensorRowWindowModel> CreateAsync()
        {
            var apiService = new ApiService(new System.Net.Http.HttpClient());
            var greenhouses = await apiService.GetGreenhousesTableAsync();
            return new UpdateSensorRowWindowModel(apiService, greenhouses);
        }

        public async Task<bool> UpdateSensorRow()
        {
            var updateResult = await _apiService.UpdateSensorRow(SensorID, SensorType, SelectedGreenhouse.ID);
            return updateResult;
        }
    }
}
