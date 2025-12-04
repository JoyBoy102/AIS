using AIS.Services;
using AIS.Structs;
using AIS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string SelectedAuthMode;
        public AuthorizationModel()
        {
            SelectedAuthMode = "Email";
            RegisteredUsers = new List<User>()
            {
                new User() { Email = "admin@yandex.ru", Number = "+89999999999", Password = "12345", LicenseOwner = true }
            };
        }

        public void SignIn()
        {
            switch (SelectedAuthMode)
            {
                case "Email":
                    if (!string.IsNullOrEmpty(FirstTextBoxData) && !string.IsNullOrEmpty(Password))
                    {
                        if (IsValidEmail(FirstTextBoxData))
                        {
                            foreach (var user in RegisteredUsers)
                            {
                                if (FirstTextBoxData.ToLower() == user.Email && Password == user.Password)
                                {
                                    var mainWindow = new MainWindow();
                                    mainWindow.Show();
                                    ProfileService.LicenseOwner = user.LicenseOwner;
                                    ProfileService.CurrentUserPhone = user.Number;
                                    ProfileService.CurrentUserEmail = user.Email;
                                    ProfileService.CurrentUserPassword = user.Password;
                                    EventAggregator.RaiseUserAuthenticated();
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
                    if (!string.IsNullOrEmpty(FirstTextBoxData) && !string.IsNullOrEmpty(Password))
                    {
                        if (IsValidPhoneNumber(FirstTextBoxData))
                        {
                            foreach (var user in RegisteredUsers)
                            {
                                if (FirstTextBoxData.ToLower() == user.Number && Password == user.Password)
                                {
                                    var mainWindow = new MainWindow();
                                    mainWindow.Show();
                                    ProfileService.LicenseOwner = user.LicenseOwner;
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

        public void RegisterUser()
        {
            if (!string.IsNullOrEmpty(FirstTextBoxData) && !string.IsNullOrEmpty(Password))
            {
                if (SelectedAuthMode == "Email")
                {
                    if (IsValidEmail(FirstTextBoxData))
                    {
                        RegisteredUsers.Add(new User() { Email = FirstTextBoxData, Password = Password, Number = "" });
                        MessageService.ShowInfo("Пользователь успешно зарегестрирован!");
                    }
                    else
                        MessageService.ShowError("Введите почтовый адрес в корректном формате!");
                }
                else
                {
                    if (IsValidPhoneNumber(FirstTextBoxData))
                    {
                        RegisteredUsers.Add(new User() { Email = "", Password = Password, Number = FirstTextBoxData });
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
