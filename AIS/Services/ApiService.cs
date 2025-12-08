using AIS.Structs;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

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

        private const string _simulateReadingEndpoint = "/simulations/simulate-reading/current";
        private const string _getGreenhousesEndpoint = "/greenhouses/";
        private const string _getSensorsEndpoint = "/sensors/";
        private const string _getExecutionDevicesEndpoint = "/execution_devices/read";
        private const string _getAgronomicRulesEndpoint = "/agronomic_rules/get_agronomic_rules";
        private const string _createExecutionDeviceRowEndpoint = "/execution_devices/create";
        private const string _getReportsEndpoint = "/reports/";
        private const string _startPeriodicReportsEndpoint = "/simulations/start-periodic-reports";
        private const string _stopPeriodicReportsEndpoint = "/simulations/stop-periodic-reports";
        private const string _getReportingStatusEndpoint = "/simulations/reporting-status";
        private const string _getPowerExecutionDevicesEndpoint = "/simulations/power_execution_devices";
        private const string _getDetectionsEndpoint = "/detections/";
        private const string _createDetectionEndpoint = "/detections/";

        public async Task<List<SensorReading>> GetGreenhousesMonitoringInfoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_simulateReadingEndpoint}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // Добавьте опции сериализации
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonString, options);

                    // Добавьте проверку на null
                    return apiResponse?.Readings ?? new List<SensorReading>();
                }
                else
                {
                    MessageService.ShowError(
                        "Ошибка генерации данных",
                        $"Эндпоинт {_simulateReadingEndpoint} вернул код {(int)response.StatusCode}"
                    );
                    return new List<SensorReading>();
                }
            }
            catch (Exception ex)
            {
                return new List<SensorReading>();
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
                    MessageService.ShowError(
                        "Ошибка получения агрономических правил",
                        $"Эндпоинт {_getAgronomicRulesEndpoint} вернул код {(int)response.StatusCode}"
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
                    MessageService.ShowError(
                        "Ошибка получения теплиц",
                        $"Эндпоинт {_getGreenhousesEndpoint} вернул код {(int)response.StatusCode}"
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
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось удалить запись из БД"
                    );
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                    "Ошибка",
                    $"Ошибка при удалении: {ex.Message}"
                );
                return false;
            }
        }

        public async Task<bool> UpdateGreenhouseRow(int greenhouseID, string _name, string _location, string _description, AgronomicRuleModel selectedAgronomicRule)
        {
            try
            {
                var greenhouseData = new
                {
                    name = _name,
                    location = _location,
                    description = _description,
                    agrorule_id = selectedAgronomicRule.Id
                };
                var json = JsonSerializer.Serialize(greenhouseData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/greenhouses/{greenhouseID}", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось редактировать запись"
                    );
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                    "Ошибка",
                    $"Ошибка при изменении записи: {ex.Message}"
                );
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
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось создать запись"
                    );
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                    "Ошибка",
                    $"Ошибка при создании записи: {ex.Message}"
                );
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
                    var greenhouses = await GetGreenhousesTableAsync();
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var sensorsInfo = JsonSerializer.Deserialize<List<Sensor>>(jsonString);

                    var joinedData = sensorsInfo
                        .Join(greenhouses,
                            sensor => sensor.GreenhouseID,
                            greenhouse => greenhouse.ID,
                            (sensor, greenhouse) => new Sensor
                            {
                                ID = sensor.ID,
                                Type = sensor.Type,
                                GreenhouseID = greenhouse.ID,
                                Greenhouse = new Greenhouse
                                {
                                    ID = greenhouse.ID,
                                    Name = greenhouse.Name,
                                    Location = greenhouse.Location,
                                    Description = greenhouse.Description,
                                    AgronomicRuleId = greenhouse.AgronomicRuleId,
                                    AgronomicRule = greenhouse.AgronomicRule
                                }
                            })
                        .ToList();
                    return joinedData;
                }
                else
                {
                    MessageService.ShowError(
                        "Ошибка получения сенсоров",
                        $"Эндпоинт {_getSensorsEndpoint} вернул код {(int)response.StatusCode}"
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
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось удалить запись из БД"
                    );
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                    "Ошибка",
                    $"Ошибка при удалении: {ex.Message}"
                );
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
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось редактировать запись"
                    );
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                    "Ошибка",
                    $"Ошибка при изменении записи: {ex.Message}"
                );
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
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось создать запись"
                    );
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                    "Ошибка",
                    $"Ошибка при изменении записи: {ex.Message}"
                );
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
                    MessageService.ShowError(
                        "Ошибка получения исполнительных устройств",
                        $"Эндпоинт {_getExecutionDevicesEndpoint} вернул код {(int)response.StatusCode}"
                    );
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new List<ExecutionDevice>();
            }
        }

        public async Task<bool> DeleteExecutionDeviceByIdFromDB(int deviceID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/execution_devices/delete/{deviceID}");
                if (!response.IsSuccessStatusCode)
                {
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось удалить запись из БД"
                    );
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                    "Ошибка",
                    $"Ошибка при удалении: {ex.Message}"
                );
                return false;
            }
        }

        public async Task<bool> AddExecutionDeviceRow(int greenhouseID, int sensorID, string type)
        {
            try
            {
                var ExecutionDeviceData = new
                {
                    greenhouse_id = greenhouseID,
                    sensor_id = sensorID,
                    type = type
                };
                var json = JsonSerializer.Serialize(ExecutionDeviceData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_createExecutionDeviceRowEndpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось создать запись"
                    );
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                    "Ошибка",
                    $"Ошибка при создании записи: {ex.Message}"
                );
                return false;
            }
        }

        public async Task<Dictionary<string, ExecutionDevicesPowerSingleGreenhouseInfo>> GetGreenhousesExecutionDevicesPowerAsync()
        {
            var response = await _httpClient.GetAsync(_getPowerExecutionDevicesEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                MessageService.ShowError("Ошибка", "Не удалось получить данные теплиц");
                return new Dictionary<string, ExecutionDevicesPowerSingleGreenhouseInfo>();
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var greenhousesDict = JsonSerializer.Deserialize<Dictionary<string, ExecutionDevicesPowerSingleGreenhouseInfo>>(
                jsonString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return greenhousesDict ?? new Dictionary<string, ExecutionDevicesPowerSingleGreenhouseInfo>();
        }
        #endregion

        #region Reports
        public async Task<List<Report>> GetReportTableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_getReportsEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var reportsInfo = JsonSerializer.Deserialize<List<Report>>(jsonString);
                    return reportsInfo;
                }
                else
                {
                    MessageService.ShowError(
                        "Ошибка получения отчетов",
                        $"Эндпоинт {_getReportsEndpoint} вернул код {(int)response.StatusCode}"
                    );
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new List<Report>();
            }
        }

        public async Task StartPeriodicReportsAsync(string interval)
        {
            try
            {
                StringContent content = new StringContent(interval, Encoding.UTF8);
                var response = await _httpClient.PostAsync(_startPeriodicReportsEndpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось включить автоматический режим"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                     "Ошибка",
                     $"Ошибка при включении автоматического режима: {ex.Message}"
                 );
            }
        }

        public async Task StopPeriodicReportsAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync(_stopPeriodicReportsEndpoint, null);
                if (!response.IsSuccessStatusCode)
                {
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось выключить автоматический режим"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                     "Ошибка",
                     $"Ошибка при выключении автоматического режима: {ex.Message}"
                 );
            }
        }

        public async Task<bool> GetPeriodicReportsStatusAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_getReportingStatusEndpoint);
                if (!response.IsSuccessStatusCode)
                {
                    MessageService.ShowError(
                        "Ошибка",
                        "Не удалось получить статус создания отчетов"
                    );
                    return false;
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                var reportsStatusInfo = JsonSerializer.Deserialize<ReportingStatusResponse>(jsonString);

                return reportsStatusInfo?.ReportingActive ?? false;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(
                     "Ошибка",
                     $"Ошибка при получении статуса создания отчетов: {ex.Message}"
                 );
                return false;
            }
        }
        #endregion

        #region Detections
        public async Task CreateDetectionInDatabaseAsync(string imagePath, int greenhouseID)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(greenhouseID.ToString()), "greenhouse_id");
                var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                string mimeType = GetMimeType(imagePath);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                formData.Add(fileContent, "photo", Path.GetFileName(imagePath));
                await _httpClient.PostAsync(_createDetectionEndpoint, formData);

            }
            catch (Exception ex)
            {
                MessageService.ShowError("Не удалось создать запись с детекцией в БД");
            }
        }

        public async Task<List<DetectionImage>> GetDetectionsList()
        {
            try
            {
                var response = await _httpClient.GetAsync(_getDetectionsEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var detectionsInfo = JsonSerializer.Deserialize<List<DetectionImage>>(jsonString);
                    return detectionsInfo;
                }
                else
                {
                    MessageService.ShowError(
                        "Ошибка получения информации по детекциям",
                        $"Эндпоинт {_getDetectionsEndpoint} вернул код {(int)response.StatusCode}"
                    );
                    return new List<DetectionImage>();
                }
            }
            catch (Exception ex)
            {
                return new List<DetectionImage>();
            }
        }

        public async Task<byte[]> GetDetectionPhoto(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    MessageService.ShowError(
                        "Ошибка получения фотографии с детекцией",
                        $"Эндпоинт {url} вернул код {(int)response.StatusCode}"
                    );
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private string GetMimeType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".tiff" => "image/tiff",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
        #endregion

    }
}