using AIS.Models;
using AIS.Services;
using AIS.Structs;
using AIS.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.ViewModels
{
    public class AuthorizationViewModel : BaseViewModel
    {
        public IRelayCommand SignInCommand { get; set; }
        public IRelayCommand RegistrationCommand { get; set; }
        public IRelayCommand EmailModeSelectCommand { get; set; }
        public IRelayCommand NumberModeSelectCommand { get; set; }

        private AuthorizationModel _authorizationModel;

        public AuthorizationViewModel()
        {
            SignInCommand = new RelayCommand(SignIn);
            EmailModeSelectCommand = new RelayCommand(EmailModeSelect);
            NumberModeSelectCommand = new RelayCommand(NumberModeSelect);
            RegistrationCommand = new RelayCommand(RegisterUser);
            _authorizationModel = new AuthorizationModel();
        }

        public string FirstTextBoxData
        {
            get
            {
                if (string.IsNullOrEmpty(_authorizationModel.FirstTextBoxData))
                {
                    return _authorizationModel.SelectedAuthMode == "Email" ? "Введите почту..." : "Введите номер...";
                }
                return _authorizationModel.FirstTextBoxData;
            }
            set
            {
                SetProperty(ref _authorizationModel.FirstTextBoxData, value);
            }
        }

        public string Password
        {
            get
            {
                return string.IsNullOrEmpty(_authorizationModel.Password) ? "Введите пароль..." : _authorizationModel.Password;
            }
            set => SetProperty(ref _authorizationModel.Password, value);
        }

        private void SignIn()
        {
            _authorizationModel.SignIn();
        }

        private void EmailModeSelect()
        {
            _authorizationModel.EmailModeSelect();
            OnPropertyChanged(nameof(FirstTextBoxData));
            OnPropertyChanged(nameof(Password));
        }

        private void NumberModeSelect()
        {
            _authorizationModel.NumberModeSelect();
            OnPropertyChanged(nameof(FirstTextBoxData));
            OnPropertyChanged(nameof(Password));
        }

        private void RegisterUser()
        {
            _authorizationModel.RegisterUser();
        }
    }
}
