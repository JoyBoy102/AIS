using AIS.Services;
using AIS.Structs;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Vml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Models
{
    public class WeedDetectionModel
    {
        public ObservableCollection<DetectionImage> DetectionImages { get; set; }
        private ApiService _apiService;
        public Visibility IsLoading = Visibility.Collapsed;
        public Visibility Loaded = Visibility.Collapsed;
        public WeedDetectionModel()
        {
            _apiService = new ApiService(new HttpClient());
        }

        public static async Task<WeedDetectionModel> CreateAsync()
        {
            var instance = new WeedDetectionModel();
            await instance.InitializeAsync();
            return instance;
        }

        private async Task InitializeAsync()
        {
            IsLoading = Visibility.Visible;
            Loaded = Visibility.Collapsed;
            DetectionImages = await GetDetectionsListAsync();
            IsLoading = Visibility.Collapsed;
            Loaded = Visibility.Visible;
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

            return greenhouseDetections.Count(g => g.DetectionCount > 20);
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

        public async Task<ObservableCollection<DetectionImage>> GetDetectionsListAsync()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(basePath).Parent.Parent.Parent.FullName;
            var result = new ObservableCollection<DetectionImage>();

            var detectionsList = await _apiService.GetDetectionsList();
                
            foreach (var detection in detectionsList)
            {
                var endpoint = $"/detections/{detection.DetectionID}/detection-photo";
                byte[] photoData = await _apiService.GetDetectionPhoto(endpoint);

                if (photoData != null && photoData.Length > 0)
                {
                    // Определяем расширение файла на основе Content-Type или данных
                    string extension = GetImageExtension(photoData) ?? ".jpg";

                    // Генерируем имя файла
                    string fileName = $"detection_{detection.DetectionID}{extension}";
                    string folderPath = System.IO.Path.Combine(projectRoot, "Detections");

                    // Создаем папку если не существует
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Полный путь к файлу
                    string fullPath = System.IO.Path.Combine(folderPath, fileName);
                    if (File.Exists(fullPath))
                    {
                        // Файл уже существует, просто используем его
                        detection.ImagePath = fullPath;
                        detection.IsWeed = detection.Confidence >= 0.42;
                        detection.GreenhouseName = $"Теплица №{detection.GreenhouseID}";
                        result.Add(detection);
                        continue; // Пропускаем загрузку
                    }
                    try
                    {
                        // Сохраняем фото на диск
                        await File.WriteAllBytesAsync(fullPath, photoData);

                        // Сохраняем путь в объекте детекции
                        detection.ImagePath = fullPath;

                    }
                    catch (Exception ex)
                    {
                        detection.ImagePath = null;
                    }
                }
                else
                {
                    detection.ImagePath = null;
                }

                // Небольшая задержка между запросами чтобы не перегружать API
                detection.IsWeed = detection.Confidence >= 0.42;
                detection.GreenhouseName = $"Теплица №{detection.GreenhouseID}";
                result.Add(detection);
                await Task.Delay(10);
            }
            
            
            return result;
        }

        private string GetImageExtension(byte[] imageData)
        {
            if (imageData.Length < 8) return ".jpg";

            // Проверяем сигнатуры форматов
            // JPEG
            if (imageData[0] == 0xFF && imageData[1] == 0xD8 && imageData[2] == 0xFF)
                return ".jpg";

            // PNG
            if (imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47)
                return ".png";

            // GIF
            if (imageData[0] == 0x47 && imageData[1] == 0x49 && imageData[2] == 0x46)
                return ".gif";

            // BMP
            if (imageData[0] == 0x42 && imageData[1] == 0x4D)
                return ".bmp";

            // По умолчанию jpg
            return ".jpg";
        }

        public async Task LoadImagesToDB()
        {
            var greenhouseList = await _apiService.GetGreenhousesTableAsync();
            var greenhouseIds = greenhouseList.Select(x => x.ID).ToList();
            Random random = new Random();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(basePath).Parent.Parent.Parent.FullName;
            string imagesPath = System.IO.Path.Combine(projectRoot, "Images");
            if (Directory.Exists(imagesPath))
            {
                var imageFiles = Directory.GetFiles(imagesPath);

                foreach (var imagePath in imageFiles)
                {
                    await _apiService.CreateDetectionInDatabaseAsync(imagePath, greenhouseIds[random.Next(greenhouseIds.Count)]);
                }
            }
        }
    }
}
