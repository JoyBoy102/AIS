using AIS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class SettingsModel
    {
        private ApiService _apiService;
        public bool AutoMode;

        public SettingsModel()
        {
            _apiService = new ApiService(new HttpClient());
        }

        public static async Task<SettingsModel> CreateAsync()
        {
            var instance = new SettingsModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            AutoMode = await GetAutoModeAsync();
        }

        public async Task<bool> GetAutoModeAsync()
        {
            bool res = await _apiService.GetPeriodicReportsStatusAsync();
            return res;
        }
    }
}
