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
            _ = InitializeViewModelAsync();
        }

        private async Task InitializeViewModelAsync()
        {
            var viewModel = await MonitoringModuleViewModel.CreateAsync();
            DataContext = viewModel;
        }
    }
}
