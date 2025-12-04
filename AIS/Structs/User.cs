using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class User
    {
        public string Email { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }

        public bool LicenseOwner { get; set; }
    }
}
