using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;

namespace AIS.Services
{
    public class MessageService
    {
        private static Wpf.Ui.Controls.MessageBox CreateMessageBox(string? messageBoxText, string caption, System.Windows.MessageBoxButton button, MessageBoxImage icon)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox();
            messageBox.Title = caption;
            messageBox.Content = messageBoxText;
            switch (button)
            {
                case System.Windows.MessageBoxButton.OK:
                    messageBox.PrimaryButtonText = "OK";
                    messageBox.IsSecondaryButtonEnabled = false;
                    messageBox.IsCloseButtonEnabled = false;
                    break;

                case System.Windows.MessageBoxButton.OKCancel:
                    messageBox.PrimaryButtonText = "OK";
                    messageBox.SecondaryButtonText = "Cancel";
                    messageBox.IsCloseButtonEnabled = false;
                    break;

                case System.Windows.MessageBoxButton.YesNo:
                    messageBox.PrimaryButtonText = "Yes";
                    messageBox.SecondaryButtonText = "No";
                    messageBox.IsCloseButtonEnabled = false;
                    break;

                case System.Windows.MessageBoxButton.YesNoCancel:
                    messageBox.PrimaryButtonText = "Yes";
                    messageBox.SecondaryButtonText = "No";
                    messageBox.CloseButtonText = "Cancel";
                    messageBox.IsCloseButtonEnabled = true;
                    break;
            }
            return messageBox;
        }
        

        public async static void ShowError(string message)
        {
            await CreateMessageBox(message, "Ошибка", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error).ShowDialogAsync();
        }

        public async static void ShowError(string title, string? message)
        {
            await CreateMessageBox(message, title, System.Windows.MessageBoxButton.OK, MessageBoxImage.Error).ShowDialogAsync();
        }

        public async static void ShowInfo(string message)
        {
            await CreateMessageBox(message, "Информация", System.Windows.MessageBoxButton.OK, MessageBoxImage.Information).ShowDialogAsync();
        }

        public async static void ShowInfo(string title, string message)
        {
            await CreateMessageBox(message, title, System.Windows.MessageBoxButton.OK, MessageBoxImage.Information).ShowDialogAsync();
        }
    }
}
