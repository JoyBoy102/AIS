using AIS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.ViewModels
{
    public class ProfileSettingsViewModel
    {
        public string CurrentUserEmail
        {
            get => ProfileService.CurrentUserEmail;
        }
        public string CurrentUserNumber
        {
            get => ProfileService.CurrentUserPhone;
        }
        public string CurrentUserPhone
        {
            get => ProfileService.CurrentUserPhone;
        }
        public string CurrentUserLicenseStatus
        {
            get => ProfileService.LicenseOwner ? "Активна" : "Не активна";
        }
    }
}
