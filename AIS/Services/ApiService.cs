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


        //---------Greenhouses---------
        public async Task<List<Sensor>> GetGreenhousesMonitoringInfoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://127.0.0.1:8000/simulations/simulate-reading/?vg=0&vs=0");
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении записи: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                return false;
            }

        }

        //---------Greenhouses---------


        //---------Sensors---------
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
        //---------Sensors---------

        //---------ExecutionDevices---------
        public async Task<List<ExecutionDevice>> GetExecutionDevicesTableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"/execution_devices/read");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var devicesInfo = JsonSerializer.Deserialize<List<ExecutionDevice>>(jsonString);
                    return devicesInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new List<ExecutionDevice>();
            }
        }

        public async Task<bool> UpdateExecutionDevicesRow(int deviceID, int greenhouseID, int sensorID, string type)
        {
            try
            {
                var executionDeviceData = new
                {
                    greenhouse_id = greenhouseID,
                    sensorID = sensorID,
                    type = type
                };
                var json = JsonSerializer.Serialize(executionDeviceData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/execution_devices/update/{deviceID}", content);
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

        public async Task<bool> DeleteExecutionDeviceByIdFromDB(int deviceID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/execution_devices/delete/{deviceID}");
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
