using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace cborModular.Services.BluetoothServices
{
    internal class BluetoothService
    {
        private readonly IAdapter _adapter;
        private readonly IBluetoothLE _ble;

        public ObservableCollection<IDevice> DiscoveredDevices { get; private set; }

        public BluetoothService()
        {
            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            DiscoveredDevices = new ObservableCollection<IDevice>();

            _adapter.DeviceDiscovered += (s, a) => DiscoveredDevices.Add(a.Device);
        }

        public bool IsBluetoothOn => _ble.State == BluetoothState.On;



    }
}
