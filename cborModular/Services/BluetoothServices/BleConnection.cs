using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using cborModular.DataModels;
using cborModular.DataStorage;
using cborModular.LocalStorageSqLite;
using Newtonsoft.Json;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace cborModular.Services.BluetoothServices
{
    internal class BleConnection
    {
        private readonly BleScanner _scanner;
        private IDevice _connectedDevice;

        private readonly DatabaseSqlite _databaseSqlite;


        public event EventHandler<IDevice> DeviceConnected;
        public event EventHandler<IDevice> DeviceDisconnected;

        public BleConnection(BleScanner scanner, DatabaseSqlite databaseSqlite)
        {
            _scanner = scanner;
            _databaseSqlite = databaseSqlite;
            _scanner.Adapter.DeviceConnected += OnDeviceConnected;
            _scanner.Adapter.DeviceDisconnected += OnDeviceDisconnected;;
        }

        // Event handler volaný při připojení zařízení
        private void OnDeviceConnected(object sender, DeviceEventArgs e)
        {
            try
            {
                var device = e.Device;
                var service = BleGetServices.GetServicesAsync(device).Result;
                var characteristics = BleGetServices.GetCharacteristicsAsync(service).Result;

                MotorcycleModel model = new()
                {
                    Name = device.Name,
                    Connected = true,
                    ServiceSerialized = JsonConvert.SerializeObject(service),
                    CharacteristicsSerialized = JsonConvert.SerializeObject(characteristics)
                };
            
                _databaseSqlite.AddMotorcycle(model);
             
               // Signalizace události připojení
                DeviceConnected?.Invoke(this, device);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving device details: {ex.Message}");
            }
        }

        // Event handler pro zjištění nových zařízení
        private async void OnDeviceDiscovered(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (IDevice device in e.NewItems)
                {
                    await ConnectToDeviceAsync(device);
                }
            }
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to device: {ex.Message}");
            }
        }

        // Pokus o opětovné připojení při odpojení zařízení
        private async void OnDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            if (e.Device.Id == _connectedDevice?.Id)
            {
                DeviceDisconnected?.Invoke(this, e.Device);
                await RetryConnectionAsync(e.Device);
            }
        }

        // Opakované připojení k zařízení
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
