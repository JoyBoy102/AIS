using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;
using System.Collections.ObjectModel;

namespace AIS.Structs
{
    public class Module
    {
        public string ModuleName { get; set; }
        public BitmapImage Img { get; set; }

        public UserControl Control { get; set; }

        public SymbolRegular ModuleIcon { get; set; }
    }
}
