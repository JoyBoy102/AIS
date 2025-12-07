using AIS.ViewModels;
using DocumentFormat.OpenXml.Drawing;
using System.Configuration;
using System.Data;
using System.Net.Http;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ApplicationThemeManager.Changed += SetIcon;

            // Простая проверка через ping
            bool isConnected = PingServer("127.0.0.1", 8000); // Порт вашего сервера

            if (isConnected)
            {
                StartupUri = new Uri("Views/AuthorizationView.xaml", UriKind.Relative);
            }
            else
            {
                StartupUri = new Uri("Views/NoConnectionView.xaml", UriKind.Relative);
            }
        }

        private bool PingServer(string address, int port)
        {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient())
                {
                    // Пробуем подключиться с коротким таймаутом
                    var result = client.BeginConnect(address, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(2000); // 2 секунды
                    client.EndConnect(result);

                    return success;
                }
            }
            catch
            {
                return false;
            }
        }

        public void SetIcon(ApplicationTheme currentApplicationTheme, Color systemAccent)
        {
            var icon = (ImageIcon)Resources["Icon"];
            if (icon != null)
            {
                icon.Source = ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark
                    ? new BitmapImage(new Uri("pack://application:,,,/Resources/icon_white.png"))
                    : new BitmapImage(new Uri("pack://application:,,,/Resources/icon_black.png"));
            }
        }
    }

}
