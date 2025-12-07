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
        public static event Action? UserAuthenticated;
        public static event Action<bool>? AutoModeChanged;

        public static void RaiseGreenhouseRowUpdated()
        {
            GreenhouseRowUpdated?.Invoke();
        }
        public static void RaiseSensorRowUpdated()
        {
            SensorRowUpdated?.Invoke();
        }

        public static void RaiseExecutionDeviceRowUpdated()
        {
            ExecutionDeviceRowUpdated?.Invoke();
        }

        public static void RaiseUserAuthenticated()
        {
            UserAuthenticated?.Invoke();
        }

        public static void RaiseAutoModeChanged(bool isAutoMode)
        {
            AutoModeChanged?.Invoke(isAutoMode);
        }

    }

}
