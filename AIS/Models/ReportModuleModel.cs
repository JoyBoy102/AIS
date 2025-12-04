using AIS.Services;
using AIS.Structs;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Models
{
    public class ReportModuleModel
    {
        public ObservableCollection<Report> Reports;

        private ApiService _apiService;

        public ObservableCollection<Greenhouse> greenhouses { get; internal set; }

        public ReportModuleModel()
        {
            _apiService = new ApiService(new HttpClient());
        }

        public static async Task<ReportModuleModel> CreateAsync()
        {
            var instance = new ReportModuleModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            var ReportList = await _apiService.GetReportTableAsync();
            Reports = new ObservableCollection<Report>(ReportList);
        }

        public async Task RefreshReportsAsync()
        {
            var reports = await _apiService.GetReportTableAsync();
            UpdateReportsTable(reports);
        }

        private void UpdateReportsTable(List<Report> newReports)
        {
            if (newReports == null) return;

            if (Reports == null)
            {
                Reports = new ObservableCollection<Report>();
            }
            Reports.Clear();

            foreach (var report in newReports)
            {
                Reports.Add(report);
            }
        }
    }
}
