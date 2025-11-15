using AIS.Models;
using AIS.Services;
using AIS.Structs;
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
        }

        public static async Task<ReportModuleViewModel> CreateAsync()
        {
            var instance = new ReportModuleViewModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            _reportModuleModel = await ReportModuleModel.CreateAsync();
        }

        public ObservableCollection<Report> Reports
        {
            get => _reportModuleModel.Reports;
            set => SetProperty(ref _reportModuleModel.Reports, value);
        }

        public AsyncRelayCommand GetExcelAsyncCommand => new AsyncRelayCommand(GetExcelAsync);
        private async Task GetExcelAsync()
        {
            await _excelService.SaveReportsToExcelAsync(Reports);
        }
    }
}
