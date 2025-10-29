using AIS.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class UpdateSensorRowWindowModel
    {
        public int SensorID;
        public string SensorType;
        public int GreenhouseID;
        private ApiService _apiService;

        public UpdateSensorRowWindowModel()
        {
            _apiService = new ApiService(new System.Net.Http.HttpClient());
        }

        public async Task<bool> UpdateSensorRow()
        {
            var updateResult = await _apiService.UpdateSensorRow(SensorID, SensorType, GreenhouseID);
            return updateResult;
        }
    }
}
