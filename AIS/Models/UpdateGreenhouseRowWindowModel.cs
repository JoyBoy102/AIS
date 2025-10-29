using AIS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class UpdateGreenhouseRowWindowModel
    {
        private ApiService _apiService;
        public int GreenhouseId;
        public string Name;
        public string Description;
        public string Location;

        public UpdateGreenhouseRowWindowModel()
        {
            _apiService = new ApiService(new System.Net.Http.HttpClient());
        }

        public async Task<bool> UpdateGreenhouseRow()
        {
            var updateResult = await _apiService.UpdateGreenhouseRow(GreenhouseId, Name, Description, Location);
            return updateResult;
        }
    }
}
