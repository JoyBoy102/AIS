using AIS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class CRUDModel
    {
        public ObservableCollection<Greenhouse> Greenhouses;
        public ObservableCollection<Sensor> Sensors;
        private ApiService _apiService;
        public CRUDModel()
        {
            _apiService = new ApiService(new HttpClient());
        }

        public static async Task<CRUDModel> CreateAsync()
        {
            var instance = new CRUDModel();
            await instance.InitializeAsync();
            return instance;
        }

        public async Task<bool> DeleteRow(int greenhouseID)
        {
           bool deleteResult = await _apiService.DeleteGreenhouseByIdFromDB(greenhouseID);
           return deleteResult;
        }

        public async Task UpdateGreenhouses()
        {
            List<Greenhouse> greenhousesList = await _apiService.GetGreenhousesTableAsync();
            Greenhouses = new ObservableCollection<Greenhouse>(greenhousesList);
        }

        private async Task InitializeAsync()
        {
            await UpdateGreenhouses();
        }

        internal async Task Create()
        {
            throw new NotImplementedException();
        }

        internal async Task Delete()
        {
            throw new NotImplementedException();
        }

        internal async Task Read()
        {
            throw new NotImplementedException();
        }

        internal async Task Update()
        {
            throw new NotImplementedException();
        }
    }
}
