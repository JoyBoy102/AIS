using AIS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace AIS.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationView.xaml
    /// </summary>
    public partial class AuthorizationView : FluentWindow
    {
        public AuthorizationView()
        {
            ApplicationThemeManager.Apply(
                ApplicationTheme.Light,
                WindowBackdropType.Acrylic
            );
            InitializeComponent();
            this.DataContext = new AuthorizationViewModel();
        }
    }
}
