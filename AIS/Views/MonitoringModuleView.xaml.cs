using AIS.ViewModels;
using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIS.Views
{
    /// <summary>
    /// Логика взаимодействия для MonitoringModuleView.xaml
    /// </summary>
    public partial class MonitoringModuleView : UserControl
    {
        public MonitoringModuleView()
        {
            InitializeComponent();
            InitializeViewModelAsync();
        }

        private async Task InitializeViewModelAsync()
        {
            var viewModel = await MonitoringModuleViewModel.CreateAsync();
            DataContext = viewModel;
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            //if (sender is ListBoxItem item)
            //{
            //    // Программно меняем цвета
            //    item.Border.Background = new SolidColorBrush(Colors.LightBlue);
            //}
            //e.Handled = true;
        }
    }
}
