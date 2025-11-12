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

namespace AIS.Views.PopupViews
{
    /// <summary>
    /// Логика взаимодействия для AddExecutionDeviceRowWindow.xaml
    /// </summary>
    public partial class AddExecutionDeviceRowWindow : Window
    {
        public AddExecutionDeviceRowWindow()
        {
            InitializeComponent();
            InitializeViewModelAsync();
        }

        private async Task InitializeViewModelAsync()
        {
            var viewModel = await AddExecutionDeviceRowWindowViewmodel.CreateAsync();
            DataContext = viewModel;
        }
    }
}
