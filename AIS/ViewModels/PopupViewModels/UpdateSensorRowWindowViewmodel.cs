using AIS.Models.PopupModels;
using AIS.Services;
using AIS.Structs;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.ViewModels.PopupViewModels
{
    public class UpdateSensorRowWindowViewmodel:BaseViewModel
    {
        public IAsyncRelayCommand UpdateSensorRowCommand { get; set; }
        public IAsyncRelayCommand CloseWindowCommand { get; set; }
        private UpdateSensorRowWindowModel _updateSensorRowWindowModel;
        private Window _window;
        public UpdateSensorRowWindowViewmodel(Window window, UpdateSensorRowWindowModel model, Sensor sensor)
        {
            _updateSensorRowWindowModel = model;
            _window = window;
            UpdateSensorRowCommand = new AsyncRelayCommand(UpdateSensorRow);
            CloseWindowCommand = new AsyncRelayCommand(CloseWindow);
            SensorID = sensor.ID;
            SensorType = sensor.Type;
            SelectedGreenhouse = sensor.Greenhouse;
        }

        public static async Task<UpdateSensorRowWindowViewmodel> CreateAsync(Window window, Sensor sensor)
        {
            var model = await UpdateSensorRowWindowModel.CreateAsync();
            return new UpdateSensorRowWindowViewmodel(window, model, sensor);
        }

        public int SensorID
        {
            get => _updateSensorRowWindowModel.SensorID;
            set
            {
                SetProperty(ref _updateSensorRowWindowModel.SensorID, value);
            }
        }

        public string SensorType
        {
            get => _updateSensorRowWindowModel.SensorType;
            set
            {
                SetProperty(ref _updateSensorRowWindowModel.SensorType, value);
            }
        }

        public List<Greenhouse> Greenhouses
        {
            get => _updateSensorRowWindowModel.Greenhouses;
        }

        public Greenhouse SelectedGreenhouse
        {
            get
            {
                return _updateSensorRowWindowModel.SelectedGreenhouse;
            }
            set
            {
                SetProperty(ref _updateSensorRowWindowModel.SelectedGreenhouse, value);
            }
        }

        private async Task UpdateSensorRow()
        {
            var updateResult = await _updateSensorRowWindowModel.UpdateSensorRow();
            if (updateResult)
                EventAggregator.RaiseSensorRowUpdated();
            _window.Close();
        }

        private async Task CloseWindow()
        {
            _window.Close();
        }
    }
}
