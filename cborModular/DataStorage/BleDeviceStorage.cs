using cborModular.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataStorage
{
    internal class BleDeviceStorage
    {
        private readonly List<DeviceModel> _devices;

        public void AddBleDevice(DeviceModel device) => _devices.Add(device);      
        public void RemoveBleDevice(DeviceModel device) => _devices.Remove(device);
        public DeviceModel GetConnectedModel() => _devices.FirstOrDefault(device => device.Connected);
        public void SetDeviceConnection(DeviceModel device, bool set = false) =>_devices.FirstOrDefault(device).Connected = set;

        





    }
}
