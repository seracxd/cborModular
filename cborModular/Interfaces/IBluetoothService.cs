using cborModular.DataModels;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Interfaces
{
    public interface IBluetoothService
    {
        // Events
        event EventHandler<IDevice> DeviceDiscovered;
        event EventHandler<IDevice> DeviceConnected;
        event EventHandler<IDevice> DeviceDisconnected;
        event EventHandler<byte[]> NotificationReceived;


        // Connection Management
        Task ConnectToDeviceAsync(IDevice device);
        Task DisconnectAsync();      

        // Scanning Management
       // ObservableCollection<IDevice> DiscoveredDevices { get; }
        Task StartScanningAsync();
        Task StopScanningAsync();

        // Service and Characteristic Management
        Task<IService> GetServicesAsync(IDevice device);
        Task<List<CharacteristicInfo>> GetCharacteristicsAsync(IService service);      

        // Notifications
        Task SubscribeToNotificationsAsync(ICharacteristic characteristic);
        Task UnsubscribeFromNotificationsAsync(ICharacteristic characteristic);

        // Data Sending
        Task SendRequestAsync(ICharacteristic characteristic, byte[] data);
    }
}
