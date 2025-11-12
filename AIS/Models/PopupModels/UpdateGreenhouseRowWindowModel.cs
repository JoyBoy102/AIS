using AIS.Services;
using AIS.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models.PopupModels
{
    public class UpdateGreenhouseRowWindowModel
    {
        private ApiService _apiService;
        public string WindowName;
        public int GreenhouseId;
        public string Name;
        public string Description;
        public string Location;
        public List<AgronomicRuleModel> AgronomicRules;
        public AgronomicRuleModel SelectedAgronomicRule;

        public UpdateGreenhouseRowWindowModel(ApiService apiService, List<AgronomicRuleModel> agronomicRules)
        {
            _apiService = apiService;
            AgronomicRules = agronomicRules;
        }

        public static async Task<UpdateGreenhouseRowWindowModel> CreateAsync()
        {
            var apiService = new ApiService(new System.Net.Http.HttpClient());
            var agronomicRules = await apiService.GetAgronomicRules();
            return new UpdateGreenhouseRowWindowModel(apiService, agronomicRules);
        }

        public async Task<bool> UpdateGreenhouseRow()
        {
            var updateResult = await _apiService.UpdateGreenhouseRow(GreenhouseId, Name, Location, Description, SelectedAgronomicRule);
            return updateResult;
        }
    }
}
