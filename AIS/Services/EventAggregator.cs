using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Services
{
    public static class EventAggregator
    {
        public static event Action? GreenhouseRowUpdated;
        public static event Action? SensorRowUpdated;
        public static event Action? ExecutionDeviceRowUpdated;

        public static void RaiseGreenhouseRowUpdated()
        {
            GreenhouseRowUpdated?.Invoke();
        }
        public static void RaiseSensorRowUpdated()
        {
            SensorRowUpdated?.Invoke();
        }

    }

}
