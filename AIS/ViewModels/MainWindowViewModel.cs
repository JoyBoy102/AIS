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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private MainWindowModel _mainWindowModel;
        private Module _selectedModule;
        public MainWindowViewModel()
        {
            _mainWindowModel = new MainWindowModel();
        }
        public ObservableCollection<Module> ModuleItems
        {
            get => _mainWindowModel.Modules;
            set
            {
                _mainWindowModel.Modules = value;
                OnPropertyChanged();
            }
        }

        public Module SelectedModule
        {
            get => _selectedModule;
            set
            {
                _selectedModule = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
