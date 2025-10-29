using AIS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Models
{
    public class CRUDModel
    {
        public ObservableCollection<Greenhouse> Greenhouses;
        public ObservableCollection<Sensor> Sensors;
        private ApiService _apiService;
        public CRUDModel()
        {
            Greenhouses = new ObservableCollection<Greenhouse>()
            {
                new Greenhouse { ID = 0, Description = "тест", Location = "уфа", Name = "Ахуенная теплица"},
                new Greenhouse { ID = 1, Description = "тест1", Location = "уфа1", Name = "Ахуенная теплица1"}
            };
            Sensors = new ObservableCollection<Sensor>()
            {
                new Sensor { ID = 0, Type = "zaebokSensorType", Value = 99999999},
                new Sensor { ID = 1, Type = "zaebokSensorType2", Value = 99999999}
            };
        }

        internal async Task Create()
        {
            throw new NotImplementedException();
        }

        internal async Task Delete()
        {
            throw new NotImplementedException();
        }

        internal async Task Read()
        {
            throw new NotImplementedException();
        }

        internal async Task Update()
        {
            throw new NotImplementedException();
        }
    }
}
