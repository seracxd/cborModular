using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using cborModular.DataModels;
using Plugin.BLE.Abstractions;
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
        private BleDeviceModel _connectedDeviceModel;

        public BleConnection(BleScanner scanner)
        {
            _scanner = scanner;

            _scanner.Adapter.DeviceConnected += OnDeviceConnected;
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
        private async void OnDeviceConnected(object sender, DeviceEventArgs e)
        {
            try
            {
                var device = e.Device;

                // Vytvoříme novou instanci BleDeviceModel
                _connectedDeviceModel = new BleDeviceModel
                {
                    Name = device.Name,
                    Id = device.Id,
                    IsConnected = true
                };

                // Načtení reklamních dat
                foreach (var record in device.AdvertisementRecords)
                {
                    _connectedDeviceModel.AdvertisementData.Add(record.Data);
                }

                // Načtení služeb
                var services = await device.GetServicesAsync();
                foreach (var service in services)
                {
                    var serviceModel = new BluetoothServiceModel
                    {
                        ServiceId = service.Id,
                        ServiceName = service.Name,
                        IsPrimary = service.IsPrimary
                    };

                    // Načtení charakteristik pro každou službu
                    var characteristics = await service.GetCharacteristicsAsync();
                    foreach (var characteristic in characteristics)
                    {
                        var characteristicModel = new BluetoothCharacteristicModel
                        {
                            CharacteristicId = characteristic.Id,
                            CharacteristicName = characteristic.Name
                        };

                        // Načtení descriptorů pro každou charakteristiku
                        var descriptors = await characteristic.GetDescriptorsAsync();
                        foreach (var descriptor in descriptors)
                        {
                            var descriptorModel = new BluetoothDescriptorModel
                            {
                                DescriptorId = descriptor.Id,
                                DescriptorName = descriptor.Name
                            };

                            characteristicModel.Descriptors.Add(descriptorModel);
                        }

                        serviceModel.Characteristics.Add(characteristicModel);
                    }

                    _connectedDeviceModel.Services.Add(serviceModel);
                }

                // (Volitelně) zde můžete provést další akce po načtení všech dat, například aktualizaci UI
                Console.WriteLine("Device details loaded and ready for use.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving device details: {ex.Message}");
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
