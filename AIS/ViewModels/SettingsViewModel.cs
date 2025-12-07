using AIS.Models;
using AIS.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;

namespace AIS.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {

        private ApplicationTheme _currentApplicationTheme = ApplicationTheme.Unknown;
        private SettingsModel _settingsModel;

        public SettingsViewModel()
        {
            _settingsModel = new SettingsModel();
        }

        public static async Task<SettingsViewModel> CreateAsync()
        {
            var instance = new SettingsViewModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            _settingsModel = await SettingsModel.CreateAsync();
        }

        public ApplicationTheme CurrentApplicationTheme
        {
            get => _currentApplicationTheme;
            set
            {
                if (_currentApplicationTheme != value)
                {
                    _currentApplicationTheme = value;
                    OnPropertyChanged(nameof(CurrentApplicationTheme));
                    ApplicationThemeManager.Apply(value);
                }
            }
        }

        public bool AutoMode
        {
            get => _settingsModel.AutoMode;
            set
            {
                _settingsModel.AutoMode = value;
                OnPropertyChanged(nameof(AutoMode));
                EventAggregator.RaiseAutoModeChanged(value);
            }
        }
    }
}
