using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace cborModular.Services.BluetoothServices
{
    internal class BleConnection
    {
        private readonly BleScanner _scanner;
        private IDevice _connectedDevice;

        public event EventHandler<IDevice> DeviceConnected;
        public event EventHandler<IDevice> DeviceDisconnected;

        public BleConnection(BleScanner scanner)
        {
            _scanner = scanner;

            // Připojíme se k zařízení, pokud se odpojí
            _scanner.DiscoveredDevices.CollectionChanged += async (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (IDevice device in e.NewItems)
                    {
                        await ConnectToDeviceAsync(device);
                    }
                }
            };
        }

        // Připojení k novému zařízení
        public async Task ConnectToDeviceAsync(IDevice device)
        {
            try
            {
                if (_connectedDevice == null || _connectedDevice.Id != device.Id)
                {
                    await _scanner.Adapter.ConnectToDeviceAsync(device);
                    _connectedDevice = device;
                    DeviceConnected?.Invoke(this, device);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to device: {ex.Message}");
            }
        }

        // Pokus o opětovné připojení, když se zařízení odpojí
        private async void OnDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            if (e.Device.Id == _connectedDevice?.Id)
            {
                Console.WriteLine("Device disconnected. Attempting to reconnect...");
                DeviceDisconnected?.Invoke(this, e.Device);

                // Pokus o opětovné připojení
                await RetryConnectionAsync(e.Device);
            }
        }

        private async Task RetryConnectionAsync(IDevice device)
        {
            bool isConnected = false;
            int retryCount = 0;
            const int maxRetries = 5;
            const int delay = 2000; // 2 sekundy

            while (!isConnected && retryCount < maxRetries)
            {
                try
                {
                    await Task.Delay(delay);
                    await _scanner.Adapter.ConnectToDeviceAsync(device);
                    isConnected = true;
                    _connectedDevice = device;
                    DeviceConnected?.Invoke(this, device);
                    Console.WriteLine("Reconnected to device.");
                }
                catch
                {
                    retryCount++;
                    Console.WriteLine($"Retry {retryCount}/{maxRetries} failed.");
                }
            }

            if (!isConnected)
            {
                Console.WriteLine("Failed to reconnect after multiple attempts.");
            }
        }

        // Odpojení od aktuálního zařízení
        public async Task DisconnectAsync()
        {
            if (_connectedDevice != null)
            {
                await _scanner.Adapter.DisconnectDeviceAsync(_connectedDevice);
                _connectedDevice = null;
            }
        }
    }
}
