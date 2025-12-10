using AIS.Services;
using AIS.Structs;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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
        private Dictionary<string, string> DeviceTypeDict = new Dictionary<string, string>
        {
            { "co2", "co2_controller" },
            { "temperature", "temperature_controller" },
            { "humidity", "humidity_controller" }
        };

        public AddExecutionDeviceRowModel()
        {

        }

        public static async Task<AddExecutionDeviceRowModel> CreateAsync()
        {
            var instance = new AddExecutionDeviceRowModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            _apiService = new ApiService(new System.Net.Http.HttpClient());
            var devicesList = await _apiService.GetExecutionDevicesTableAsync();
            var sensorsList = await _apiService.GetSensorsTableAsync();
            var busySensors = devicesList.Select(x => x.SensorID).ToList();
            Greenhouses = new ObservableCollection<Greenhouse>();
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

        public async Task<bool> AddExecutionDeviceRow()
        {
            var AddResult = await _apiService.AddExecutionDeviceRow(SelectedGreenhouse.ID, SelectedSensor.ID, DeviceTypeDict[SelectedSensor.Type]);
            return AddResult;
        }

        public string GetCurrentDeviceType()
        {
            if (SelectedSensor != null)
            {
                return SelectedSensor.Type;
            }
            else return "";
        }
    }
}
