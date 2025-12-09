using AIS.Models;
using AIS.Structs;
using AIS.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AIS.ViewModels
{
    public class WeedDetectionViewModel : BaseViewModel
    {
        
        private WeedDetectionModel _weedDetectionModel;
        private DetectionImage _detectionImage;
        private bool _isPopupOpen;
        public IAsyncRelayCommand LoadImagesCommand { get; set; }
        public IRelayCommand<DetectionImage> OpenImagePopupCommand { get; set; }
        public WeedDetectionViewModel()
        {
            OpenImagePopupCommand = new RelayCommand<DetectionImage>(OpenPopup);
            LoadImagesCommand = new AsyncRelayCommand(LoadImagesToDB);
        }

        public static async Task<WeedDetectionViewModel> CreateAsync()
        {
            var instance = new WeedDetectionViewModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            _weedDetectionModel = await WeedDetectionModel.CreateAsync();
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

        public Visibility IsLoading
        {
            get => _weedDetectionModel.IsLoading;
            set => SetProperty(ref  _weedDetectionModel.IsLoading, value);
        }

        public Visibility Loaded
        {
            get => _weedDetectionModel.Loaded;
            set => SetProperty(ref _weedDetectionModel.Loaded, value);
        }

        public int TotalWeedsCount
        {
            get => DetectionImages.Count(x => x.IsWeed);
        }
        public int CriticalZonesCount
        {
            get => _weedDetectionModel.GetCriticalZonesCount();
        }
        public string WeedLevel
        {
            get => _weedDetectionModel.GetWeedLevel();
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => SetProperty(ref _isPopupOpen, value);
        }

        public DetectionImage SelectedImage
        {
            get => _detectionImage;
            set => SetProperty(ref _detectionImage, value);
        }

        private void OpenPopup(DetectionImage image)
        {
            if (image == null) return;

            var modalWindow = new PhotoViewer();
            var photoViewerViewModel = new PhotoViewewViewModel();
            photoViewerViewModel.SelectedImage = image;
            modalWindow.DataContext = photoViewerViewModel;
            modalWindow.ShowDialog();
        }

        private async Task LoadImagesToDB()
        {
            IsLoading = Visibility.Visible;
            Loaded = Visibility.Collapsed;
            await _weedDetectionModel.LoadImagesToDB();
            DetectionImages = await _weedDetectionModel.GetDetectionsListAsync();
            IsLoading = Visibility.Collapsed;
            Loaded = Visibility.Visible;
        }

    }


   
}