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
    public class ReportModuleViewModel : INotifyPropertyChanged
    {
        private ReportModuleModel _reportModuleModel;

        public ReportModuleViewModel()
        {
            _reportModuleModel = new ReportModuleModel();
        }

        public ObservableCollection<Report> Reports
        {
            get 
            {
                return _reportModuleModel.reports;
            } 
            set
            {
                _reportModuleModel.reports = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
