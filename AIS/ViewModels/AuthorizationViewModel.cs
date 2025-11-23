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
        private string _firstTextBoxData;
        private string _password;
        public IRelayCommand SignInCommand { get; set; }
        public IRelayCommand RegistrationCommand { get; set; }
        public IRelayCommand EmailModeSelectCommand { get; set; }
        public IRelayCommand NumberModeSelectCommand { get; set; }

        private List<User> _registeredUsers;

        private string _selectedAuthMode;

        public AuthorizationViewModel()
        {
            SignInCommand = new RelayCommand(SignIn);
            EmailModeSelectCommand = new RelayCommand(EmailModeSelect);
            NumberModeSelectCommand = new RelayCommand(NumberModeSelect);
            RegistrationCommand = new RelayCommand(RegisterUser);
            _selectedAuthMode = "Email";
            _registeredUsers = new List<User>()
            {
                new User() { Email = "admin@yandex.ru", Number = "+89999999999", Password = "12345" }
            };
        }

        public string FirstTextBoxData
        {
            get
            {
                if (string.IsNullOrEmpty(_firstTextBoxData))
                {
                    return _selectedAuthMode == "Email" ? "Введите почту..." : "Введите номер...";
                }
                return _firstTextBoxData;
            }
            set
            {
                SetProperty(ref _firstTextBoxData, value);
            }
        }

        public string Password
        {
            get
            {
                return string.IsNullOrEmpty(_password) ? "Введите пароль..." : _password;
            }
            set => SetProperty(ref _password, value);
        }

        private void SignIn()
        {
            switch (_selectedAuthMode)
            {
                case "Email":
                    if (!string.IsNullOrEmpty(_firstTextBoxData) && !string.IsNullOrEmpty(_password))
                    {
                        if (IsValidEmail(_firstTextBoxData))
                        {
                            foreach (var user in _registeredUsers)
                            {
                                if (_firstTextBoxData.ToLower() == user.Email && _password == user.Password)
                                {
                                    var mainWindow = new MainWindow();
                                    mainWindow.Show();
                                    Application.Current.Windows.OfType<AuthorizationView>().FirstOrDefault()?.Close();
                                    return;
                                }
                            }
                            MessageService.ShowError("Неверные данные для входа!");
                        }
                        else
                        {
                            MessageService.ShowError("Введите почтовый адрес в корректном формате!");
                        }
                    }
                    else
                    {
                        MessageService.ShowError("Введите данные для входа!");
                    }
                    break;

                case "Number":
                    if (!string.IsNullOrEmpty(_firstTextBoxData) && !string.IsNullOrEmpty(_password))
                    {
                        if (IsValidPhoneNumber(_firstTextBoxData))
                        {
                            foreach (var user in _registeredUsers)
                            {
                                if (_firstTextBoxData.ToLower() == user.Number && _password == user.Password)
                                {
                                    var mainWindow = new MainWindow();
                                    mainWindow.Show();
                                    Application.Current.Windows.OfType<AuthorizationView>().FirstOrDefault()?.Close();
                                    return;
                                }
                            }
                            MessageService.ShowError("Неверные данные для входа!");
                        }
                        else
                        {
                            MessageService.ShowError("Введите номер телефона в корректном формате!");
                        }
                    }
                    else
                    {
                        MessageService.ShowError("Введите данные для входа!");
                    }
                    break;
            }
        }

        private void EmailModeSelect()
        {
            _selectedAuthMode = "Email";
            _firstTextBoxData = "";
            _password = "";
            OnPropertyChanged(nameof(FirstTextBoxData));
            OnPropertyChanged(nameof(Password));
        }

        private void NumberModeSelect()
        {
            _selectedAuthMode = "Number";
            _firstTextBoxData = "";
            _password = "";
            OnPropertyChanged(nameof(FirstTextBoxData));
            OnPropertyChanged(nameof(Password));
        }

        private void RegisterUser()
        {
            if (!string.IsNullOrEmpty(_firstTextBoxData) && !string.IsNullOrEmpty(_password))
            {
                if (_selectedAuthMode == "Email")
                {
                    if (IsValidEmail(_firstTextBoxData))
                    {
                        _registeredUsers.Add(new User() { Email = _firstTextBoxData, Password = _password, Number = "" });
                        MessageService.ShowInfo("Пользователь успешно зарегестрирован!");
                    }
                    else
                        MessageService.ShowError("Введите почтовый адрес в корректном формате!");
                }
                else
                {
                    if (IsValidPhoneNumber(_firstTextBoxData))
                    {
                        _registeredUsers.Add(new User() { Email = "", Password = _password, Number = _firstTextBoxData });
                        MessageService.ShowInfo("Пользователь успешно зарегестрирован!");
                    }
                    else
                        MessageService.ShowError("Введите номер телефона в корректном формате!");
                }

            }
            else
            {
                MessageService.ShowError("Введите данные для регистрации!");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            var allowedChars = new HashSet<char>("+0123456789- ()");
            if (phone.Any(c => !allowedChars.Contains(c))) return false;

            return phone.Length >= 10 &&
                   (phone.StartsWith("+7") || phone.StartsWith("7") || phone.StartsWith("8"));
        }
    }
}
