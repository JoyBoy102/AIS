using AIS.Models;
using AIS.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.ViewModels
{
    public class UpdateSensorRowWindowViewmodel:BaseViewModel
    {
        public IAsyncRelayCommand UpdateSensorRowCommand { get; set; }
        public IAsyncRelayCommand CloseWindowCommand { get; set; }
        private UpdateSensorRowWindowModel _updateSensorRowWindowModel;
        private Window _window;
        public UpdateSensorRowWindowViewmodel(Sensor sensor, Window window)
        {
            _updateSensorRowWindowModel = new UpdateSensorRowWindowModel();
            _window = window;
            UpdateSensorRowCommand = new AsyncRelayCommand(UpdateSensorRow);
            CloseWindowCommand = new AsyncRelayCommand(CloseWindow);
            SensorID = sensor.ID;
            SensorType = sensor.Type;
            GreenhouseID = sensor.GreenhouseID;
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

        public int GreenhouseID
        {
            get => _updateSensorRowWindowModel.GreenhouseID;
            set
            {
                SetProperty(ref _updateSensorRowWindowModel.GreenhouseID, value);
            }
        }

        private async Task UpdateSensorRow()
        {
            var updateResult = await _updateSensorRowWindowModel.UpdateSensorRow();
            if (updateResult)
                EventAggregator.RaiseSensorRowUpdated();
        }

        private async Task CloseWindow()
        {
            _window.Close();
        }
    }
}
