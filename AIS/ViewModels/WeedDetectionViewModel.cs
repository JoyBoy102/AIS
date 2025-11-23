using AIS.Models;
using AIS.Structs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIS.ViewModels
{
    public class WeedDetectionViewModel : BaseViewModel
    {
        
        private WeedDetectionModel _weedDetectionModel;
        public WeedDetectionViewModel()
        {
            _weedDetectionModel = new WeedDetectionModel();

        }

        public ObservableCollection<DetectionImage> DetectionImages
        {
            get => _weedDetectionModel.DetectionImages;
            set
            {
                _weedDetectionModel.DetectionImages = value;
                OnPropertyChanged();
            }
        }

        public int TotalWeedsCount
        {
            get => DetectionImages.Count;
        }
        public int CriticalZonesCount
        {
            get => _weedDetectionModel.GetCriticalZonesCount();
        }
        public string WeedLevel
        {
            get => _weedDetectionModel.GetWeedLevel();
        }

        // Коллекция для галереи изображений

    }


   
}