using AIS.Models;
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
    public class CRUDViewModel: INotifyPropertyChanged
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
            set
            {
                _CRUDmodel.Greenhouses = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Sensor> Sensors
        {
            get => _CRUDmodel.Sensors;
            set
            {
                _CRUDmodel.Sensors = value;
                OnPropertyChanged();
            }
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
