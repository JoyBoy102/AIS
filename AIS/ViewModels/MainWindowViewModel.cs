using AIS.Models;
using AIS.Services;
using AIS.Structs;
using AIS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace AIS.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private MainWindowModel _mainWindowModel;
        private Module? _selectedModule;
        private ObservableCollection<NavigationViewItem> _navigationViews = new ObservableCollection<NavigationViewItem>();
        public MainWindowViewModel()
        {
            _mainWindowModel = new MainWindowModel();
            foreach (var item in _mainWindowModel.Modules)
            {
                _navigationViews.Add(new NavigationViewItem(item.ModuleName, item.ModuleIcon, item.Control.GetType()));
            }
            EventAggregator.UserAuthenticated += () =>
            {
                _navigationViews[3].IsEnabled = ProfileService.LicenseOwner;
                _navigationViews[3].Opacity = ProfileService.LicenseOwner? 1 : 0.5;
            };
        }
        
        public ObservableCollection<NavigationViewItem> ModuleItemsNav
        {
            get
            {
                return _navigationViews;
            }
        }

        public ObservableCollection<NavigationViewItem> FooterModuleItemsNav
        {
            get
            {
                return 
                [
                    new NavigationViewItem("Настройки", SymbolRegular.Settings24, typeof(SettingsView)),
                    new NavigationViewItem("Профиль", SymbolRegular.Person24, typeof(ProfileSettingsView))
                ];
            }
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

        public Module? SelectedModule
        {
            get => _selectedModule;
            set
            {
                _selectedModule = value;
                OnPropertyChanged();
            }
        }
    }
}
