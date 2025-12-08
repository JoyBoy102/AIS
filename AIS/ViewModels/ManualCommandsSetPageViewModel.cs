using AIS.Models;
using AIS.Services;
using AIS.Structs;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.ViewModels
{
    public class ManualCommandsSetPageViewModel:BaseViewModel
    {
        private ManualCommandsSetPageModel _manualCommandsSetPageModel;
        public IAsyncRelayCommand ApplyCommand { get; set; }

        public ManualCommandsSetPageViewModel()
        {
            ApplyCommand = new AsyncRelayCommand(Apply);
        }

        public static async Task<ManualCommandsSetPageViewModel> CreateAsync()
        {
            var instance = new ManualCommandsSetPageViewModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
           _manualCommandsSetPageModel = await ManualCommandsSetPageModel.CreateAsync();
        }

        public ObservableCollection<ExecutionDevice> Devices
        {
            get => _manualCommandsSetPageModel.Devices;
            set => SetProperty(ref _manualCommandsSetPageModel.Devices, value);
        }

        public ExecutionDevice SelectedDevice
        {
            get => _manualCommandsSetPageModel.SelectedDevice;
            set => SetProperty(ref _manualCommandsSetPageModel.SelectedDevice, value);
        }
        
        public int PowerValue
        {
            get => _manualCommandsSetPageModel.PowerValue;
            set => SetProperty(ref _manualCommandsSetPageModel.PowerValue, value);
        }

        private async Task Apply()
        {
            await _manualCommandsSetPageModel.Apply();
        }
    }
}
