using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class DetectionImage
    {
        public int GreenhouseID { get; set; }
        public string ImagePath { get; set; }
        public string GreenhouseName { get; set; }
        public string DetectionTime { get; set; }
        public string Confidence { get; set; }

        public bool IsWeed { get; set; }
    }
}
