using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class DetectionImage
    {
        public string ImagePath { get; set; }
        public string GreenhouseName { get; set; }
        public string DetectionTime { get; set; }
        public string Confidence { get; set; }
    }
}
