using AIS.Models.PopupModels;
using AIS.Services;
using AIS.Structs;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.ViewModels.PopupViewModels
{
    public class AddExecutionDeviceRowWindowViewmodel:BaseViewModel
    {
        private AddExecutionDeviceRowModel _model;
        public IAsyncRelayCommand AddExecutionDeviceRowCommand { get; set; }
        public IAsyncRelayCommand CloseWindowCommand { get; set; }

        private Window _window;
        public AddExecutionDeviceRowWindowViewmodel()
        {
            
        }

        public static async Task<AddExecutionDeviceRowWindowViewmodel> CreateAsync(Window window)
        {
            var instance = new AddExecutionDeviceRowWindowViewmodel();
            await instance.InitializeAsync(window);
            return instance;
        }

        private async Task InitializeAsync(Window window)
        {
            _model = await AddExecutionDeviceRowModel.CreateAsync(new ApiService(new System.Net.Http.HttpClient()));
            _window = window;
            AddExecutionDeviceRowCommand = new AsyncRelayCommand(AddExecutionDeviceRow);
            CloseWindowCommand = new AsyncRelayCommand(CloseWindow);
        }

        public ObservableCollection<Greenhouse> Greenhouses
        {
            get => _model.Greenhouses;
            set
            {
                SetProperty(ref _model.Greenhouses, value);
            }
        }

        public Greenhouse SelectedGreenhouse
        {
            get => _model.SelectedGreenhouse;
            set
            {
                SetProperty(ref _model.SelectedGreenhouse, value);
            }
        }

        public Sensor SelectedSensor
        {
            get => _model.SelectedSensor;
            set
            {
                SetProperty(ref _model.SelectedSensor, value);
            }
        }

        private async Task AddExecutionDeviceRow()
        {
            bool AddResult = await _model.AddExecutionDeviceRow();
            if (AddResult)
            {
                EventAggregator.RaiseExecutionDeviceRowUpdated();
            }
        }

        private async Task CloseWindow()
        {
            _window.Close();
        }

    }
}
