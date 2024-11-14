using cborModular.DataModels;
using cborModular.Interfaces;
using cborModular.Services.BluetoothServices;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace cborModular.Interfaces.Controlers
{
    internal class BluetoothControler : IBluetoothService
    {
        private readonly BleConnection _connection;
        private readonly BleScanner _scanner;
        // private readonly BleGetServices _getServices;
        private readonly BleNotifications _notifications;
        private readonly BleSendData _sendData;


        // Events
        public event EventHandler<IDevice> DeviceConnected;
        public event EventHandler<IDevice> DeviceDisconnected;
        public event EventHandler<byte[]> NotificationReceived;
        public event EventHandler<IDevice> DeviceDiscovered;

        public BluetoothControler()
        {
            _scanner = new BleScanner();
            _connection = new BleConnection(_scanner);       // Předání skeneru do BleConnection
            //_getServices = new BleGetServices();             // Pokud je BleGetServices samostatný
            _notifications = new BleNotifications();         // Pokud je BleNotifications samostatný
            _sendData = new BleSendData();

            // Připojení událostí
            _connection.DeviceConnected += (s, e) => DeviceConnected?.Invoke(this, e);
            _connection.DeviceDisconnected += (s, e) => DeviceDisconnected?.Invoke(this, e);
            _notifications.NotificationReceived += (s, data) => NotificationReceived?.Invoke(this, data);
            _scanner.DeviceDiscovered += (s, device) => DeviceDiscovered?.Invoke(this, device);
        }

       
        // Connection Management
        public Task ConnectToDeviceAsync(IDevice device) => _connection.ConnectToDeviceAsync(device);
        public Task DisconnectAsync() => _connection.DisconnectAsync();

        // Scanning Management
       // public ObservableCollection<IDevice> DiscoveredDevices => _scanner.DiscoveredDevices;
        public Task StartScanningAsync() => _scanner.StartScanningAsync();
        public Task StopScanningAsync() => _scanner.StopScanningAsync();

        // Service and Characteristic Management
        public Task<IService> GetServicesAsync(IDevice device) => BleGetServices.GetServicesAsync(device);
        public Task<List<CharacteristicInfo>> GetCharacteristicsAsync(IService service) => BleGetServices.GetCharacteristicsAsync(service);

        // Notifications
        public Task SubscribeToNotificationsAsync(ICharacteristic characteristic) => _notifications.SubscribeToNotificationsAsync(characteristic);
        public Task UnsubscribeFromNotificationsAsync(ICharacteristic characteristic) => _notifications.UnsubscribeFromNotificationsAsync(characteristic);

        // Data Sending
        public Task SendRequestAsync(ICharacteristic characteristic, byte[] data) => BleSendData.SendRequestAsync(characteristic, data);
    }
}

