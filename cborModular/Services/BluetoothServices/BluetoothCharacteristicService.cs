using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BluetoothCharacteristicService
    {
        private readonly IDevice _device;

        public BluetoothCharacteristicService(IDevice device)
        {
            _device = device;
        }

        public async Task<ICharacteristic> GetCharacteristicAsync(Guid serviceGuid, Guid characteristicGuid)
        {
            var service = await _device.GetServiceAsync(serviceGuid);
            return await service.GetCharacteristicAsync(characteristicGuid);
        }

        public static async Task<byte[]> ReadCharacteristicAsync(ICharacteristic characteristic)
        {
            var (data, _) = await characteristic.ReadAsync();
            return data;
        }

        public static async Task WriteCharacteristicAsync(ICharacteristic characteristic, byte[] data)
        {
            await characteristic.WriteAsync(data);
        }

        public static async Task SubscribeToNotificationsAsync(ICharacteristic characteristic, Action<byte[]> onNotificationReceived)
        {
            characteristic.ValueUpdated += (o, args) =>
            {
                onNotificationReceived?.Invoke(args.Characteristic.Value);
            };
            await characteristic.StartUpdatesAsync();
        }

        public static async Task UnsubscribeFromNotificationsAsync(ICharacteristic characteristic)
        {
            await characteristic.StopUpdatesAsync();
        }
    }
}
