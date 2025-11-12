using AIS.Models.PopupModels;
using AIS.Services;
using AIS.Structs;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.ViewModels.PopupViewModels
{
    public class AddGreenhouseRowWindowViewmodel: BaseViewModel
    {
        public IAsyncRelayCommand AddGreenhouseRowCommand { get; set; }
        public IAsyncRelayCommand CloseWindowCommand { get; set; }
        private AddGreenhouseRowWindowModel _addGreenhouseRowWindowModel;
        private Window _window;

        public AddGreenhouseRowWindowViewmodel(Window window, AddGreenhouseRowWindowModel model)
        {
            _addGreenhouseRowWindowModel = model;
            AddGreenhouseRowCommand = new AsyncRelayCommand(AddGreenhouseRow);
            CloseWindowCommand = new AsyncRelayCommand(CloseWindow);
            Name = string.Empty;
            Description = string.Empty;
            Location = string.Empty; 
            _window = window;
        }

        public static async Task<AddGreenhouseRowWindowViewmodel> CreateAsync(Window window)
        {
            var model = await AddGreenhouseRowWindowModel.CreateAsync();
            return new AddGreenhouseRowWindowViewmodel(window, model);
        }

        public string WindowName
        {
            get => _addGreenhouseRowWindowModel.WindowName;
            set
            {
                SetProperty(ref _addGreenhouseRowWindowModel.WindowName, value);
            }
        }

        public string Name
        {
            get => _addGreenhouseRowWindowModel.Name;
            set
            {
                SetProperty(ref _addGreenhouseRowWindowModel.Name, value);
            }
        }

        public string Description
        {
            get => _addGreenhouseRowWindowModel.Description;
            set
            {
                SetProperty(ref _addGreenhouseRowWindowModel.Description, value);
            }
        }

        public string Location
        {
            get =>  _addGreenhouseRowWindowModel.Location;
            set
            {
                SetProperty(ref _addGreenhouseRowWindowModel.Location, value);
            }
        }

        public List<AgronomicRuleModel> AgronomicRules
        {
            get => _addGreenhouseRowWindowModel.AgronomicRules;
        }

        public AgronomicRuleModel SelectedAgronomicRule
        {
            get => _addGreenhouseRowWindowModel.SelectedAgronomicRule;
            set
            {
                SetProperty(ref _addGreenhouseRowWindowModel.SelectedAgronomicRule, value);
            }
        }

        private async Task AddGreenhouseRow()
        {
           var updateResult = await _addGreenhouseRowWindowModel.AddGreenhouseRow();
           if (updateResult)
               EventAggregator.RaiseGreenhouseRowUpdated();
        }

        private async Task CloseWindow()
        {
            _window.Close();
        }
    }
}
