using cborModular.DataIdentifiers;
using cborModular.DataModels;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BleGetServices
    {
        public static async Task<IService> GetServicesAsync(IDevice device)
        {
            try
            {
                var services = await device.GetServicesAsync();
                
                if (services != null)
                {
                    foreach (var service in services)
                    {
                        // Použití ParseCustomGuid k ověření, zda je identifikátor služby typu "Service"
                        var (identifier, isValid) = GuidServices.ParseCustomGuid(service.Id);
                        if (isValid && identifier == BluetoothCharakteristicIdentifiers.Service)
                        {
                            return service;
                        }
                    }
                }              
            }
            catch{}

            return null;
        }


        public static async Task<List<CharacteristicInfo>> GetCharacteristicsAsync(IService service)
        {
            try
            {
                var characteristics = await service.GetCharacteristicsAsync();
                var characteristicInfos = new List<CharacteristicInfo>();

                if (characteristics != null)
                {
                    foreach (var characteristic in characteristics)
                    {
                        var (characteristicType, isValid) = GuidServices.ParseCustomGuid(characteristic.Id);

                        // Pokud je GUID platný a máme definovaný typ charakteristiky, použijeme jej
                        var identifier = isValid && characteristicType.HasValue
                            ? characteristicType.Value
                            : BluetoothCharakteristicIdentifiers.Unknown; 

                        characteristicInfos.Add(new CharacteristicInfo
                        {
                            Characteristic = characteristic,
                            Identifier = identifier
                        });
                    }
                }

                return characteristicInfos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching characteristics: {ex.Message}");
                return [];
            }
        }

    }
}
