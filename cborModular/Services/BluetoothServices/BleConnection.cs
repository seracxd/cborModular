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
        private BleDeviceModel _connectedDeviceModel;

        public event EventHandler<IDevice> DeviceConnected;
        public event EventHandler<IDevice> DeviceDisconnected;

        public BleConnection(BleScanner scanner)
        {
            _scanner = scanner;
            _scanner.Adapter.DeviceConnected += OnDeviceConnected;
            _scanner.Adapter.DeviceDisconnected += OnDeviceDisconnected;
            _scanner.DiscoveredDevices.CollectionChanged += OnDeviceDiscovered;
        }

        // Event handler volaný při připojení zařízení
        private async void OnDeviceConnected(object sender, DeviceEventArgs e)
        {
            try
            {
                var device = e.Device;
                _connectedDeviceModel = InitializeDeviceModel(device);

                // Načítání detailů o zařízení
                await LoadDeviceDetailsAsync(device);

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

        // Vytvoření a inicializace modelu zařízení
        private BleDeviceModel InitializeDeviceModel(IDevice device)
        {
            return new BleDeviceModel
            {
                Name = device.Name,
                Id = device.Id,
                IsConnected = true
            };
        }

        // Načtení všech detailů zařízení (reklamní data, služby, charakteristiky, deskriptory)
        private async Task LoadDeviceDetailsAsync(IDevice device)
        {
            LoadAdvertisementData(device);

            var services = await device.GetServicesAsync();
            foreach (var service in services)
            {
                var serviceModel = CreateServiceModel(service);

                await LoadCharacteristicsAsync(service, serviceModel);
                _connectedDeviceModel.Services.Add(serviceModel);
            }
        }

        // Načítání reklamních dat
        private void LoadAdvertisementData(IDevice device)
        {
            foreach (var record in device.AdvertisementRecords)
            {
                _connectedDeviceModel.AdvertisementData.Add(record.Data);
            }
        }

        // Vytvoření modelu služby
        private BluetoothServiceModel CreateServiceModel(IService service)
        {
            return new BluetoothServiceModel
            {
                ServiceId = service.Id,
                ServiceName = service.Name,
                IsPrimary = service.IsPrimary
            };
        }

        // Načtení charakteristik pro službu
        private async Task LoadCharacteristicsAsync(IService service, BluetoothServiceModel serviceModel)
        {
            var characteristics = await service.GetCharacteristicsAsync();
            foreach (var characteristic in characteristics)
            {
                var characteristicModel = CreateCharacteristicModel(characteristic);

                await LoadDescriptorsAsync(characteristic, characteristicModel);
                serviceModel.Characteristics.Add(characteristicModel);
            }
        }

        // Vytvoření modelu charakteristiky
        private BluetoothCharacteristicModel CreateCharacteristicModel(ICharacteristic characteristic)
        {
            return new BluetoothCharacteristicModel
            {
                CharacteristicId = characteristic.Id,
                CharacteristicName = characteristic.Name
            };
        }

        // Načtení deskriptorů pro charakteristiku
        private async Task LoadDescriptorsAsync(ICharacteristic characteristic, BluetoothCharacteristicModel characteristicModel)
        {
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
