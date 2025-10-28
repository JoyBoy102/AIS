using AIS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Services
{
    public class ApiService
    {
        private HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://127.0.0.1:8000");
        }

        public async Task<List<Sensor>> GetGreenhousesMonitoringInfoAsync()
        {
            try
            {
                var parameters = new { Vg = 0, VS = 0 };
                var json = JsonSerializer.Serialize(parameters);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"/simulations/simulate-reading/", content );
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var sensorsInfo = JsonSerializer.Deserialize<List<Sensor>>(jsonString);
                    return sensorsInfo;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new List<Sensor>();
            }
        }

        public async Task<List<Greenhouse>> GetGreenhousesTableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"/greenhouses/");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var GreenhousesInfo = JsonSerializer.Deserialize<List<Greenhouse>>(jsonString);
                    return GreenhousesInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new List<Greenhouse>();
            }
        }

        public async Task<bool> DeleteGreenhouseByIdFromDB(int greenhouseID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/greenhouses/{greenhouseID}");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Не удалось удалить запись из БД",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                return false;
            }
        }
    }
}
