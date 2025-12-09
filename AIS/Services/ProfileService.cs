using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Services
{
    public static class ProfileService
    {
        public static bool IsSudo { get; set; }
        public static string CurrentUserPassword { get; set; }
        public static string CurrentUserEmail { get; set; }
        public static string CurrentUserPhone { get; set; }
    }
}
