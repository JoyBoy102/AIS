using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class TabItemModel
    {
        public string TabTitle { get; set; }

        public ObservableCollection<object> Collection { get; set; }
    }
}
