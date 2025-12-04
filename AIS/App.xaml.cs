using DocumentFormat.OpenXml.Drawing;
using System.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace AIS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ApplicationThemeManager.Changed += SetIcon;
            InitializeComponent();
        }


        public void SetIcon(ApplicationTheme currentApplicationTheme, Color systemAccent)
        {
            var icon = (ImageIcon)Resources["Icon"];
            icon.Source = ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark
                ? new BitmapImage(new Uri("pack://application:,,,/Resources/icon_white.png"))
                : new BitmapImage(new Uri("pack://application:,,,/Resources/icon_black.png"));
        }
    }

}
