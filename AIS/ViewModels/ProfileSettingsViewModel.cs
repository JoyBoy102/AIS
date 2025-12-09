using AIS.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AIS.ViewModels
{
    public partial class ProfileSettingsViewModel : ObservableObject
    {
        public ApiService _apiService = new ApiService(new HttpClient());

        // Флаги редактирования
        [ObservableProperty]
        private bool _hasChanges = false;

        [ObservableProperty]
        private bool _isEditingEmail = false;

        [ObservableProperty]
        private bool _isEditingPhone = false;

        [ObservableProperty]
        private bool _isEditingPassword = false;

        // Свойства для хранения редактируемых значений
        [ObservableProperty]
        private string _editedEmail;

        [ObservableProperty]
        private string _editedPhone;

        [ObservableProperty]
        private string _editedPassword;

        public Visibility EmailVisibility
        {
            get
            {
                if (CurrentUserEmail != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public Visibility PhoneVisibility
        {
            get
            {
                if (CurrentUserNumber != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        // Свойство для видимости кнопки Сохранить
        public Visibility SaveButtonVisibility => HasChanges ? Visibility.Visible : Visibility.Collapsed;

        public string CurrentUserEmail
        {
            get => ProfileService.CurrentUserEmail;
            set => ProfileService.CurrentUserEmail = value;
        }

        public string CurrentUserNumber
        {
            get => ProfileService.CurrentUserPhone;
            set => ProfileService.CurrentUserPhone = value;
        }

        public string CurrentUserPassword
        {
            get => ProfileService.CurrentUserPassword; 
            set => ProfileService.CurrentUserPassword = value;
        }

        public string CurrentUserAdministratorStatus
        {
            get => ProfileService.IsSudo ? "Да" : "Нет";
        }

        // Команды
        [RelayCommand]
        private void EditEmail()
        {
            EditedEmail = CurrentUserEmail;
            IsEditingEmail = true;
            UpdateHasChanges();
        }

        [RelayCommand]
        private void EditPhone()
        {
            EditedPhone = CurrentUserNumber;
            IsEditingPhone = true;
            UpdateHasChanges();
        }

        [RelayCommand]
        private void EditPassword()
        {
            EditedPassword = CurrentUserPassword;
            IsEditingPassword = true;
            UpdateHasChanges();
        }

        [RelayCommand]
        private void CancelEmail()
        {
            IsEditingEmail = false;
            EditedEmail = string.Empty;
            UpdateHasChanges();
            OnPropertyChanged(nameof(CurrentUserEmail));
        }

        [RelayCommand]
        private void CancelPhone()
        {
            IsEditingPhone = false;
            EditedPhone = string.Empty;
            UpdateHasChanges();
            OnPropertyChanged(nameof(CurrentUserNumber));
        }

        [RelayCommand]
        private void CancelPassword()
        {
            IsEditingPassword = false;
            EditedPassword = string.Empty;
            UpdateHasChanges();
            OnPropertyChanged(nameof(CurrentUserPassword));
        }

        // Обновление флага изменений
        private void UpdateHasChanges()
        {
            HasChanges = IsEditingEmail || IsEditingPhone || IsEditingPassword;
            OnPropertyChanged(nameof(SaveButtonVisibility));
        }

        // Команда сохранения
        private AsyncRelayCommand _save;
        public ICommand Save => _save ??= new AsyncRelayCommand(PerformSave);

        private async Task PerformSave()
        {
            try
            {
                //ВИДИМОСТЬ ПАРОЛЯ СДЕЛАТЬ
                //И СДЕЛАТЬ СОХРАНЕНИЕ
                if (IsEditingEmail) CurrentUserEmail = EditedEmail;
                if (IsEditingPhone) CurrentUserNumber = EditedPhone;
                if (IsEditingPassword) CurrentUserPassword = EditedPassword;

                // Сбрасываем флаги редактирования
                IsEditingEmail = false;
                IsEditingPhone = false;
                IsEditingPassword = false;
                HasChanges = false;

                // Обновляем отображаемые значения
                OnPropertyChanged(nameof(CurrentUserEmail));
                OnPropertyChanged(nameof(CurrentUserNumber));
                OnPropertyChanged(nameof(CurrentUserPassword));
                OnPropertyChanged(nameof(SaveButtonVisibility));

                // Здесь можно добавить уведомление об успешном сохранении
                MessageBox.Show("Изменения успешно сохранены!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}