using AIS.Models.PopupModels;
using AIS.Services;
using AIS.Structs;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.ViewModels.PopupViewModels
{
    public class AddSensorRowWindowViewmodel: BaseViewModel
    {
        public IAsyncRelayCommand AddSensorRowCommand { get; set; }
        public IAsyncRelayCommand CloseWindowCommand { get; set; }
        private AddSensorRowWindowModel _addSensorRowWindowModel;
        private Window _window;

        public AddSensorRowWindowViewmodel(Window window, AddSensorRowWindowModel model)
        {
            _addSensorRowWindowModel = model;
            _window = window;
            AddSensorRowCommand = new AsyncRelayCommand(UpdateSensorRow);
            CloseWindowCommand = new AsyncRelayCommand(CloseWindow);
        }

        public static async Task<AddSensorRowWindowViewmodel> CreateAsync(Window window)
        {
            var model = await AddSensorRowWindowModel.CreateAsync();
            return new AddSensorRowWindowViewmodel(window, model);
        }

        public string SensorType
        {
            get => _addSensorRowWindowModel.SensorType;
            set
            {
                SetProperty(ref _addSensorRowWindowModel.SensorType, value);
            }
        }

        public List<Greenhouse> Greenhouses
        {
            get => _addSensorRowWindowModel.Greenhouses;
        }

        public Greenhouse SelectedGreenhouse
        {
            get => _addSensorRowWindowModel.SelectedGreenhouse;
            set
            {
                SetProperty(ref _addSensorRowWindowModel.SelectedGreenhouse, value);
            }
        }

        private async Task UpdateSensorRow()
        {
            var updateResult = await _addSensorRowWindowModel.AddSensorRow();
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
