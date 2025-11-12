using AIS.Models.PopupModels;
using AIS.Services;
using AIS.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.ViewModels.PopupViewModels
{
    public class AddExecutionDeviceRowWindowViewmodel:BaseViewModel
    {
        private AddExecutionDeviceRowModel _model;
        public AddExecutionDeviceRowWindowViewmodel()
        {
            
        }

        public static async Task<AddExecutionDeviceRowWindowViewmodel> CreateAsync()
        {
            var instance = new AddExecutionDeviceRowWindowViewmodel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            _model = await AddExecutionDeviceRowModel.CreateAsync(new ApiService(new System.Net.Http.HttpClient()));
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

    }
}
