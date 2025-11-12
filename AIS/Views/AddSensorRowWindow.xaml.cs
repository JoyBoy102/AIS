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

namespace AIS.Views
{
    /// <summary>
    /// Логика взаимодействия для UpdateGreenhouseRowWindow.xaml
    /// </summary>
    public partial class AddSensorRowWindow : Window
    {
        public AddSensorRowWindow()
        {
            InitializeComponent();
            InitializeViewModelAsync();
        }

        private async Task InitializeViewModelAsync()
        {
            var viewModel = await AddSensorRowWindowViewmodel.CreateAsync(this);
            DataContext = viewModel;
        }
    }
}
