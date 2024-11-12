using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BleGetServices
    {
        private readonly IDevice _device;

        public BleGetServices(IDevice device)
        {
            _device = device;
        }

        public async Task<List<IService>> GetServicesAsync()
        {
            try
            {
                var services = await _device.GetServicesAsync();
                return services != null ? new List<IService>(services) : new List<IService>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching services: {ex.Message}");
                return new List<IService>();
            }
        }

        public async Task<List<ICharacteristic>> GetCharacteristicsAsync(IService service)
        {
            try
            {
                var characteristics = await service.GetCharacteristicsAsync();
                return characteristics != null ? new List<ICharacteristic>(characteristics) : new List<ICharacteristic>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching characteristics: {ex.Message}");
                return new List<ICharacteristic>();
            }
        }

        public async Task DisplayServicesAndCharacteristicsAsync()
        {
            var services = await GetServicesAsync();
            foreach (var service in services)
            {
                Console.WriteLine($"Service: {service.Id}");

                var characteristics = await GetCharacteristicsAsync(service);
                foreach (var characteristic in characteristics)
                {
                    Console.WriteLine($"  Characteristic: {characteristic.Id}");
                    Console.WriteLine($"    Properties: {characteristic.Properties}");
                }
            }
        }
    }
}
