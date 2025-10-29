using AIS.Models;
using AIS.Services;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Bibliography;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS.ViewModels
{
    public class CRUDViewModel: BaseViewModel
    {
        private CRUDModel _CRUDmodel;
        public IAsyncRelayCommand DeleteRowCommand { get; set; }
        public IAsyncRelayCommand UpdateRowCommand { get; set; }
        public CRUDViewModel(CRUDModel model)
        {
            _CRUDmodel = model;
            DeleteRowCommand = new AsyncRelayCommand<int>(DeleteRow);
            UpdateRowCommand = new AsyncRelayCommand<Greenhouse>(UpdateRow);
        }

        public static async Task<CRUDViewModel> CreateAsync()
        {
            var model = await CRUDModel.CreateAsync();
            return new CRUDViewModel(model);
        }

        public ObservableCollection<Greenhouse> Greenhouses
        {
            get => _CRUDmodel.Greenhouses;
            set => SetProperty(ref _CRUDmodel.Greenhouses, value);
        }

        public ObservableCollection<Sensor> Sensors
        {
            get => _CRUDmodel.Sensors;
            set => SetProperty(ref _CRUDmodel.Sensors, value);
        }

        public AsyncRelayCommand CreateCommand => new AsyncRelayCommand(Create);
        private async Task Create()
        {
            await _CRUDmodel.Create();
        }

        public AsyncRelayCommand ReadCommand => new AsyncRelayCommand(Read);
        private async Task Read()
        {
            await _CRUDmodel.Read();
        }

        public AsyncRelayCommand UpdateCommand => new AsyncRelayCommand(Update);
        private async Task Update()
        {
            await _CRUDmodel.Update();
        }

        public async Task UpdateGreenhouses()
        {
            await _CRUDmodel.UpdateGreenhouses();
            Greenhouses = _CRUDmodel.Greenhouses;
            OnPropertyChanged();
        }

        private async Task DeleteRow(int greenhouseId)
        {
            bool deleteResult = await _CRUDmodel.DeleteRow(greenhouseId);
            if (deleteResult)
            {
                await UpdateGreenhouses();
            }
        }

        private async Task UpdateRow(Greenhouse greenhouse)
        {
            
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
