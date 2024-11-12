using Microsoft.Maui.Dispatching;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BleScanner
    {
        private readonly IAdapter _adapter;
        private readonly IDispatcher _dispatcher;
        internal ObservableCollection<IDevice> DiscoveredDevices { get; private set; }

        public BleScanner(IDispatcher dispatcher)
        {
            _adapter = CrossBluetoothLE.Current.Adapter;
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
            _dispatcher = dispatcher;
            DiscoveredDevices = new ObservableCollection<IDevice>();
        }


        public async Task StartScanningAsync()
        {
            DiscoveredDevices.Clear();
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
                        // Hledáme typ záznamu odpovídající `ServiceData`
                        if (record.Type == AdvertisementRecordType.UuidsComplete128Bit)
                        {
                            byte[] bytes = record.Data;

                            Guid id = GuidServices.ReverseGuidByteOrder(bytes);

                            if (GuidServices.ParseCustomGuid(id).isValid)
                            {
                                _dispatcher.Dispatch(() =>
                                {

                                    if (!DiscoveredDevices.Contains(device))
                                    {
                                        DiscoveredDevices.Add(device);
                                    }
                                });
                                // await _adapter.ConnectToDeviceAsync(device);
                                // break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            finally
            {
                await StopScanningAsync();
            }
        }
    }
}
