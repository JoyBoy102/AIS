using AIS.Structs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIS.ViewModels
{
    public class WeedDetectionViewModel : INotifyPropertyChanged
    {
        private int _totalWeedsCount = 47;
        public int TotalWeedsCount
        {
            get => _totalWeedsCount;
            set { _totalWeedsCount = value; OnPropertyChanged(); }
        }

        private int _criticalZonesCount = 3;
        public int CriticalZonesCount
        {
            get => _criticalZonesCount;
            set { _criticalZonesCount = value; OnPropertyChanged(); }
        }

        private string _weedLevel = "Средний";
        public string WeedLevel
        {
            get => _weedLevel;
            set { _weedLevel = value; OnPropertyChanged(); }
        }

        // Коллекция для галереи изображений
        public ObservableCollection<DetectionImage> DetectionImages { get; set; }

        public WeedDetectionViewModel()
        {
            // Инициализируем коллекцию с тестовыми данными
            DetectionImages = new ObservableCollection<DetectionImage>
            {
                new DetectionImage
                {
                    ImagePath = "/Images/weed1.jpg",
                    GreenhouseName = "Теплица №1",
                    DetectionTime = "12.11.2024 14:30",
                    Confidence = "92% уверенности"
                },
                new DetectionImage
                {
                    ImagePath = "/Images/weed2.jpg",
                    GreenhouseName = "Теплица №2",
                    DetectionTime = "12.11.2024 14:25",
                    Confidence = "85% уверенности"
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


   
}