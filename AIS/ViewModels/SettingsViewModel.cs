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
    }
}
