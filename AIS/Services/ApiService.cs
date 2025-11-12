using AIS.Models;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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

        private const string _simulateReadingEndpoint = "simulations/simulate-reading/?vg=0&vs=0";
        private const string _getGreenhousesEndpoint = "/greenhouses/";
        private const string _getSensorsEndpoint = "/sensors/";
        private const string _getExecutionDevicesEndpoint = "/execution_devices/read";
        private const string _getAgronomicRulesEndpoint = "/agronomic_rules/get_agronomic_rules";

        public async Task<List<Sensor>> GetGreenhousesMonitoringInfoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://127.0.0.1:8000/{_simulateReadingEndpoint}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var sensorsInfo = JsonSerializer.Deserialize<List<Sensor>>(jsonString);
                    return sensorsInfo;
                }
                else
                {
                    MessageBox.Show(
                        $"Эндпоинт {_simulateReadingEndpoint} вернул код {(int)response.StatusCode}",
                        "Ошибка генерации данных",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new List<Sensor>();
            }
        }

        #region Agronomic Rules
        public async Task<List<AgronomicRuleModel>> GetAgronomicRules()
        {
            try
            {
                var response = await _httpClient.GetAsync(_getAgronomicRulesEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var agronomicRulesModelInfo = JsonSerializer.Deserialize<List<AgronomicRuleModel>>(jsonString);
                    return agronomicRulesModelInfo;
                }
                else
                {
                    MessageBox.Show(
                        $"Эндпоинт {_getAgronomicRulesEndpoint} вернул код {(int)response.StatusCode}",
                        "Ошибка получения агрономических правил",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new List<AgronomicRuleModel>();
            }
        }
        #endregion

        #region Greenhouse
        public async Task<List<Greenhouse>> GetGreenhousesTableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_getGreenhousesEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var greenhousesInfo = JsonSerializer.Deserialize<List<Greenhouse>>(jsonString);
                    var agroRules = await GetAgronomicRules();
                    var joinedData = greenhousesInfo
                        .Join(agroRules,
                            greenhouse => greenhouse.AgronomicRuleId,
                            rule => rule.Id,
                            (greenhouse, rule) => new Greenhouse
                            {
                                ID = greenhouse.ID,
                                Name = greenhouse.Name,
                                Location = greenhouse.Location,
                                Description = greenhouse.Description,
                                AgronomicRuleId = greenhouse.AgronomicRuleId,
                                AgronomicRule = rule
                            })
                        .ToList();
                    return joinedData;
                }
                else
                {
                    MessageBox.Show(
                        $"Эндпоинт {_getGreenhousesEndpoint} вернул код {(int)response.StatusCode}",
                        "Ошибка получения теплиц",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
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

        public async Task<bool> AddGreenhouseRow(string name, string location, string description, AgronomicRuleModel selectedAgronomicRule)
        {
            try
            {
                var greenhouseData = new
                {
                    name = name,
                    location = location,
                    description = description,
                    agrorule_id = selectedAgronomicRule.Id,
                };
                var json = JsonSerializer.Serialize(greenhouseData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"/greenhouses/", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Не удалось создать запись",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании записи: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                return false;
            }
        }
        #endregion

        #region Sensors
        public async Task<List<Sensor>> GetSensorsTableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_getSensorsEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var sensorsInfo = JsonSerializer.Deserialize<List<Sensor>>(jsonString);
                    return sensorsInfo;
                }
                else
                {
                    MessageBox.Show(
                        $"Эндпоинт {_getSensorsEndpoint} вернул код {(int)response.StatusCode}",
                        "Ошибка получения сенсоров",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
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
                var sensorData = new
                {
                    type = sensorType,
                    greenhouse_id = greenhouseID
                };
                var json = JsonSerializer.Serialize(sensorData);
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
        public async Task<bool> AddSensorRow(string sensorType, int greenhouseID)
        {
            try
            {
                var sensorData = new
                {
                    type = sensorType,
                    greenhouse_id = greenhouseID
                };
                var json = JsonSerializer.Serialize(sensorData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"/sensors/", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Не удалось создать запись",
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
        #endregion

        #region Execution Devices
        public async Task<List<ExecutionDevice>> GetExecutionDevicesTableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_getExecutionDevicesEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var devicesInfo = JsonSerializer.Deserialize<List<ExecutionDevice>>(jsonString);
                    return devicesInfo;
                }
                else
                {
                    MessageBox.Show(
                        $"Эндпоинт {_getExecutionDevicesEndpoint} вернул код {(int)response.StatusCode}",
                        "Ошибка получения исполнительных устройств",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
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
        #endregion
    }
}
