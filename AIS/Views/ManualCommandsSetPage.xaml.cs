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
    /// Логика взаимодействия для ManualCommandsSetPage.xaml
    /// </summary>
    public partial class ManualCommandsSetPage : UserControl
    {
        public ManualCommandsSetPage()
        {
            _ = InitializeViewModelAsync();
            InitializeComponent();
        }

        private async Task InitializeViewModelAsync()
        {
            var viewModel = await ManualCommandsSetPageViewModel.CreateAsync();
            DataContext = viewModel;
        }
    }


}
