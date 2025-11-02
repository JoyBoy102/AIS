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
    public class UpdateGreenhouseRowWindowViewmodel: BaseViewModel
    {
        public IAsyncRelayCommand UpdateGreenhouseRowCommand { get; set; }
        public IAsyncRelayCommand CloseWindowCommand { get; set; }
        private UpdateGreenhouseRowWindowModel _updateGreenhouseRowWindowModel;
        private Window _window;

        public UpdateGreenhouseRowWindowViewmodel(Greenhouse greenhouse, Window window)
        {
            _updateGreenhouseRowWindowModel = new UpdateGreenhouseRowWindowModel();
            UpdateGreenhouseRowCommand = new AsyncRelayCommand(UpdateGreenhouseRow);
            CloseWindowCommand = new AsyncRelayCommand(CloseWindow);
            GreenhouseId = greenhouse.ID;
            Name = greenhouse.Name;
            Description = greenhouse.Description;
            Location = greenhouse.Location;
            _window = window;
        }
        
        public string WindowName
        {
            get => _updateGreenhouseRowWindowModel.WindowName;
            set
            {
                SetProperty(ref _updateGreenhouseRowWindowModel.WindowName, value);
            }
        }

        public int GreenhouseId
        {
            get => _updateGreenhouseRowWindowModel.GreenhouseId;
            set
            {
                SetProperty(ref _updateGreenhouseRowWindowModel.GreenhouseId, value);
            }
        }

        public string Name
        {
            get => _updateGreenhouseRowWindowModel.Name;
            set
            {
                SetProperty(ref _updateGreenhouseRowWindowModel.Name, value);
            }
        }

        public string Description
        {
            get => _updateGreenhouseRowWindowModel.Description;
            set
            {
                SetProperty(ref _updateGreenhouseRowWindowModel.Description, value);
            }
        }

        public string Location
        {
            get => _updateGreenhouseRowWindowModel.Location;
            set
            {
                SetProperty(ref _updateGreenhouseRowWindowModel.Location, value);
            }
        }

        private async Task UpdateGreenhouseRow()
        {
           var updateResult = await _updateGreenhouseRowWindowModel.UpdateGreenhouseRow();
           if (updateResult)
               EventAggregator.RaiseGreenhouseRowUpdated();
        }

        private async Task CloseWindow()
        {
            _window.Close();
        }
    }
}
