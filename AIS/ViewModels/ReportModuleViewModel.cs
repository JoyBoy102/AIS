using AIS.Models;
using AIS.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS.ViewModels
{
    public class ReportModuleViewModel : BaseViewModel
    {
        private ExcelService _excelService;
        private ReportModuleModel _reportModuleModel;

        public ReportModuleViewModel()
        {
            _excelService = new ExcelService();
            _reportModuleModel = new ReportModuleModel();
        }

        public ObservableCollection<Report> Reports
        {
            get => _reportModuleModel.reports;
            set => SetProperty(ref _reportModuleModel.reports, value);
        }

        public AsyncRelayCommand GetExcelAsyncCommand => new AsyncRelayCommand(GetExcelAsync);
        private async Task GetExcelAsync()
        {
            await _excelService.SaveReportsToExcelAsync(Reports);
        }
    }
}
