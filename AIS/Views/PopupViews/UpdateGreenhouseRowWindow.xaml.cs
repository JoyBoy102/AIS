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
    /// Логика взаимодействия для UpdateGreenhouseRowWindow.xaml
    /// </summary>
    public partial class UpdateGreenhouseRowWindow : FluentWindow
    {
        public UpdateGreenhouseRowWindow(Greenhouse greenhouse)
        {
            InitializeComponent();
            InitializeViewModelAsync(greenhouse);
        }

        private async Task InitializeViewModelAsync(Greenhouse greenhouse)
        {
            var viewModel = await UpdateGreenhouseRowWindowViewmodel.CreateAsync(this, greenhouse);
            DataContext = viewModel;
        }
    }
}
