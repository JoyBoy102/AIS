using AIS.Services;
using AIS.Structs;
using DocumentFormat.OpenXml.InkML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models.PopupModels
{
    public class AddExecutionDeviceRowModel
    {
        private ApiService _apiService;
        public ObservableCollection<Greenhouse> Greenhouses;
        public Greenhouse SelectedGreenhouse;
        public Sensor SelectedSensor;

        public AddExecutionDeviceRowModel()
        {

        }

        public static async Task<AddExecutionDeviceRowModel> CreateAsync(ApiService apiService)
        {
            var instance = new AddExecutionDeviceRowModel();
            await instance.InitializeAsync(apiService);
            return instance;
        }

        private async Task InitializeAsync(ApiService apiService)
        {
            _apiService = apiService;
            Greenhouses = new ObservableCollection<Greenhouse>();
            var devicesList = await _apiService.GetExecutionDevicesTableAsync();
            var sensorsList = await _apiService.GetSensorsTableAsync();
            var busySensors = devicesList.Select(x => x.SensorID).ToList();

            sensorsList = sensorsList.Where(s => !busySensors.Contains(s.ID)).ToList();
            var groupByGreenhouseId = sensorsList.GroupBy(s => new { s.GreenhouseID, s.Greenhouse.Name});

            foreach (var group in groupByGreenhouseId)
            {
                Greenhouse gr = new Greenhouse { ID = group.Key.GreenhouseID, Name = group.Key.Name, FreeSensors = new ObservableCollection<Sensor>() };
                foreach (var sensor in group)
                {
                    gr.FreeSensors.Add(sensor);
                }
                Greenhouses.Add(gr);
            }
        }

    }
}
