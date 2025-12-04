using AIS.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class WeedDetectionModel
    {
        public ObservableCollection<DetectionImage> DetectionImages { get; set; }

        public WeedDetectionModel()
        {
            DetectionImages = new ObservableCollection<DetectionImage>
            {
                new DetectionImage
                {
                    GreenhouseID = 1,
                    ImagePath = "/Images/weed1.jpg",
                    GreenhouseName = "Теплица №1",
                    DetectionTime = "12.11.2024 14:30",
                    Confidence = "92% уверенности",
                    IsWeed = true
                },
                new DetectionImage
                {
                    GreenhouseID= 2,
                    ImagePath = "/Images/weed2.jpg",
                    GreenhouseName = "Теплица №2",
                    DetectionTime = "12.11.2024 14:25",
                    Confidence = "85% уверенности",
                    IsWeed = true
                }
            };
        }

        public int GetCriticalZonesCount()
        {
            var greenhouseDetections = DetectionImages
                .GroupBy(img => img.GreenhouseID)
                .Select(g => new
                {
                    GreenhouseID = g.Key,
                    DetectionCount = g.Count()
                });

            return greenhouseDetections.Count(g => g.DetectionCount > 10);
        }

        public string GetWeedLevel()
        {
            if (DetectionImages == null || !DetectionImages.Any())
                return "0%";

            int totalImages = DetectionImages.Count;
            int weedImagesCount = DetectionImages.Count(img => img.IsWeed);

            double weedPercentage = (double)weedImagesCount / totalImages * 100;

            return $"{weedPercentage:F1}%";
        }

        public string GetResourceImagePath(string imageName)
        {
            return $"pack://application:,,,/AIS;component/{imageName}";
        }
    }
}
