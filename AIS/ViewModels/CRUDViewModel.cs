using AIS.Models;
using AIS.Services;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Bibliography;
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

        public CRUDViewModel()
        {
            _CRUDmodel = new CRUDModel();
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

        public AsyncRelayCommand DeleteCommand => new AsyncRelayCommand(Delete);
        private async Task Delete()
        {
            await _CRUDmodel.Delete();
        }
    }
}
