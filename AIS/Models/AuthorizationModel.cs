using AIS.Services;
using AIS.Structs;
using AIS.Views;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Models
{
    public class AuthorizationModel
    {
        public string FirstTextBoxData;
        public string Password;
        public List<User> RegisteredUsers;
        private ApiService _apiService;
        public string SelectedAuthMode;
        public AuthorizationModel()
        {
            _apiService = new ApiService(new HttpClient());
            SelectedAuthMode = "Email";
        }

        public async void SignIn()
        {
            if (!string.IsNullOrEmpty(FirstTextBoxData) && !string.IsNullOrEmpty(Password))
            {
                if (IsValidEmail(FirstTextBoxData) || IsValidPhoneNumber(FirstTextBoxData))
                {
                    var user = await _apiService.Auth(FirstTextBoxData, Password);
                    if (user != null)
                    {
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        ProfileService.IsSudo = user.IsSudo;
                        if (IsValidEmail(FirstTextBoxData)) ProfileService.CurrentUserEmail = user.Login;
                        else ProfileService.CurrentUserPhone = user.Login;
                        EventAggregator.RaiseUserAuthenticated();
                        Application.Current.Windows.OfType<AuthorizationView>().FirstOrDefault()?.Close();
                        return;
                    }
                }
                else
                {
                    string loginHelp = string.Empty;
                    if (SelectedAuthMode == "Email") loginHelp = "почтовый адрес";
                    else loginHelp = "номер телефона";
                    MessageService.ShowError($"Введите {loginHelp} в корректном формате!");
                }
            }
            else
            {
                MessageService.ShowError("Введите данные для входа!");
            }
        }

        public async void RegisterUser()
        {
            if (!string.IsNullOrEmpty(FirstTextBoxData) && !string.IsNullOrEmpty(Password))
            {
                if (SelectedAuthMode == "Email")
                {
                    if (IsValidEmail(FirstTextBoxData))
                    {
                        var ok = await _apiService.AddUser(FirstTextBoxData, Password, false, "");
                        if (ok) MessageService.ShowInfo("Пользователь успешно зарегестрирован!");
                    }
                    else
                        MessageService.ShowError("Введите почтовый адрес в корректном формате!");
                }
                else
                {
                    if (IsValidPhoneNumber(FirstTextBoxData))
                    {
                        var ok = await _apiService.AddUser(FirstTextBoxData, Password, false, "");
                        if (ok) MessageService.ShowInfo("Пользователь успешно зарегестрирован!");
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

        public void EmailModeSelect()
        {
            SelectedAuthMode = "Email";
            FirstTextBoxData = "";
            Password = "";
        }

        public void NumberModeSelect()
        {
            SelectedAuthMode = "Number";
            FirstTextBoxData = "";
            Password = "";
        }
    }
}
