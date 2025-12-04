using AIS.Structs;
using AIS.ViewModels.PopupViewModels;
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
using Wpf.Ui.Controls;

namespace AIS.Views
{
    /// <summary>
    /// Логика взаимодействия для UpdateSensorRowWindow.xaml
    /// </summary>
    public partial class UpdateSensorRowWindow : FluentWindow
    {
        public UpdateSensorRowWindow(Sensor sensor)
        {
            InitializeComponent();
            InitializeViewModelAsync(sensor);
        }

        private async Task InitializeViewModelAsync(Sensor sensor)
        {
            var viewModel = await UpdateSensorRowWindowViewmodel.CreateAsync(this, sensor);
            DataContext = viewModel;
        }
    }
}
