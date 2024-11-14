using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BleScanner
    {
        private readonly IAdapter _adapter;
        private readonly ConcurrentBag<IDevice> _discoveredDevices = [];

        public event EventHandler<IDevice> DeviceDiscovered;

        public IAdapter Adapter => _adapter;

        public IReadOnlyCollection<IDevice> DiscoveredDevices => _discoveredDevices;

        public BleScanner()
        {
            _adapter = CrossBluetoothLE.Current.Adapter;
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
        }

        public async Task StartScanningAsync()
        {
            _discoveredDevices.Clear(); // Vymažte zařízení před novým skenováním
            await _adapter.StartScanningForDevicesAsync();
        }

        public async Task StopScanningAsync()
        {
            await _adapter.StopScanningForDevicesAsync();
        }

        private async void OnDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            try
            {
                var device = e.Device;

                if (device.AdvertisementRecords != null)
                {
                    foreach (var record in device.AdvertisementRecords)
                    {
                        if (record.Type == AdvertisementRecordType.UuidsComplete128Bit)
                        {
                            byte[] bytes = record.Data;

                            Guid id = GuidServices.ReverseGuidByteOrder(bytes);

                            if (GuidServices.ParseCustomGuid(id).isValid)
                            {
                                if (!_discoveredDevices.Contains(device))
                                {
                                    _discoveredDevices.Add(device);
                                    DeviceDiscovered?.Invoke(this, device); // Oznámení, že nové zařízení bylo nalezeno
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                await StopScanningAsync();
            }
        }
    }
}
