using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;

namespace cborModular.Services.BluetoothServices
{
    internal class BluetoothConnector
    {
        private readonly IAdapter _adapter;

        public BluetoothConnector()
        {
            _adapter = CrossBluetoothLE.Current.Adapter;
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
        }

        public async Task StartScanningAsync()
        {
            await _adapter.StartScanningForDevicesAsync();
        }

        private async void OnDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            var device = e.Device;         
            try
            {
                // Připojení k zařízení - pokud je požadován PIN, systém by měl zobrazit dialog
                await _adapter.ConnectToDeviceAsync(device);
                Console.WriteLine("Zařízení úspěšně připojeno.");

                // Zpracování po připojení, například načtení služeb
                var services = await device.GetServicesAsync();
                foreach (var service in services)
                {
                    Console.WriteLine($"Služba nalezena: {service.Id}");
                    // Další zpracování služby...
                }
            }
            catch (DeviceConnectionException ex)
            {
                Console.WriteLine($"Připojení selhalo: {ex.Message}");
            }
        }
    }
}
