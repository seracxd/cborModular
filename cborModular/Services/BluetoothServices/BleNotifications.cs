using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BleNotifications
    {
        // Událost vyvolaná při přijetí nové notifikace
        public event EventHandler<byte[]> NotificationReceived;

        // Přihlášení k notifikaci pro specifickou charakteristiku
        public async Task SubscribeToNotificationsAsync(ICharacteristic characteristic)
        {
            if (characteristic == null)
            {
                throw new ArgumentNullException(nameof(characteristic), "Characteristic is not initialized.");
            }

            // Zaregistrujeme obsluhu události pro přijetí notifikace
            characteristic.ValueUpdated += OnNotificationReceived;

            // Povolit notifikace
            await characteristic.StartUpdatesAsync();         
        }

        // Odhlášení od notifikace pro specifickou charakteristiku
        public async Task UnsubscribeFromNotificationsAsync(ICharacteristic characteristic)
        {
            if (characteristic == null)
            {
                throw new ArgumentNullException(nameof(characteristic), "Characteristic is not initialized.");
            }

            // Zrušíme obsluhu události
            characteristic.ValueUpdated -= OnNotificationReceived;

            // Zakázat notifikace
            await characteristic.StopUpdatesAsync();
        }

        // Obsluha pro přijetí notifikace
        private void OnNotificationReceived(object sender, CharacteristicUpdatedEventArgs e)
        {
            byte[] data = e.Characteristic.Value;

            // Vyvolání vlastní události pro notifikaci
            NotificationReceived?.Invoke(this, data);
        }
    }
}

