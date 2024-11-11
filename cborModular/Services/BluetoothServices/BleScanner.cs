using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BleScanner
    {
        private readonly IAdapter _adapter;
        private readonly string AppName = "KubergMoto";
        public event EventHandler<string> ApplicationDiscovered;

        public BleScanner()
        {
            _adapter = CrossBluetoothLE.Current.Adapter;
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
        }

        public async Task StartScanningAsync()
        {
            // Spuštění skenování
            await _adapter.StartScanningForDevicesAsync();
        }
        public async Task StopScanningAsync()
        {
            // Spuštění skenování
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
                        if (record.Type == Plugin.BLE.Abstractions.AdvertisementRecordType.UuidsComplete128Bit)
                        {
                            byte[] bytes = record.Data;
                            Array.Reverse(bytes);

                            if (GuidParser.ParseCustomGuid(new Guid(bytes)).isValid)
                            {
                                await _adapter.ConnectToDeviceAsync(device);
                                break;
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
