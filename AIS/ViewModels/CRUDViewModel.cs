using AIS.Models;
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
        public CRUDViewModel()
        {
            _CRUDmodel = new CRUDModel();
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

        public event PropertyChangedEventHandler? PropertyChanged; 
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
