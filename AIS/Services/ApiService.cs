using AIS.Models;
using DocumentFormat.OpenXml.Spreadsheet;
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
                    var greenhousesInfo = JsonSerializer.Deserialize<List<Greenhouse>>(jsonString);
                    return greenhousesInfo;
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

        public async Task<List<Sensor>> GetSensorsTableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"/sensors/");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var sensorsInfo = JsonSerializer.Deserialize<List<Sensor>>(jsonString);
                    return sensorsInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new List<Sensor>();
            }
        }

        public async Task<bool> DeleteGreenhouseByIdFromDB(int greenhouseID)
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

        public async Task<bool> DeleteSensorByIdFromDB(int sensorId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/sensors/{sensorId}");
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

        public async Task<bool> UpdateGreenhouseRow(int greenhouseID, string _name, string _location, string _description)
        {
            try
            {
                var greenhouseData = new
                {
                    name = _name,
                    location = _location,
                    description = _description
                };
                var json = JsonSerializer.Serialize(greenhouseData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/greenhouses/{greenhouseID}", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Не удалось редактировать запись",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении записи: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                return false;
            }
           
        }

        public async Task<bool> UpdateSensorRow(int sensorID, string sensorType, int greenhouseID)
        {
            try
            {
                var greenhouseData = new
                {
                    type = sensorType,
                    greenhouse_id = greenhouseID
                };
                var json = JsonSerializer.Serialize(greenhouseData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/sensors/{sensorID}", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Не удалось редактировать запись",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении записи: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                return false;
            }
        }
    }
}
