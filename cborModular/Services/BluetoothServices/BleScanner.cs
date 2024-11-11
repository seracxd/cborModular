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
                _adapter.DeviceConnected += async (sender, e ) =>
                {
                    Console.WriteLine("d");
                };

                var device = e.Device;

                if (device.AdvertisementRecords != null)
                {
                    foreach (var record in device.AdvertisementRecords)
                    {
                        // Hledáme typ záznamu odpovídající `ServiceData`
                        if (record.Type == Plugin.BLE.Abstractions.AdvertisementRecordType.UuidsComplete128Bit)
                        {
                            byte[] bytes = record.Data;

                            Guid id = ReverseGuidByteOrder(bytes);

                            if (GuidParser.ParseCustomGuid(id).isValid)
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

        public static Guid ReverseGuidByteOrder(byte[] bytes)
        {
            Array.Reverse(bytes);
            // Obrátit první 4 bajty (int)
            Array.Reverse(bytes, 0, 4);
            // Obrátit další 2 bajty (short)
            Array.Reverse(bytes, 4, 2);
            // Obrátit další 2 bajty (short)
            Array.Reverse(bytes, 6, 2);

            return new Guid(bytes);
        }
    }

}
