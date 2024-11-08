using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Formats.Cbor;
using cborModular.DataIdentifiers;
using cborModular.DataModels;
using cborModular.Services;
using Microsoft.Maui.Controls;

 
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
 

namespace cborModular
{
    public partial class MainPage : ContentPage
    {

        private BluetoothLEDevice _device;
        private GattCharacteristic _requestCharacteristic;
        private GattCharacteristic _notificationCharacteristic;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ConnectButton_Clicked(object sender, EventArgs e)
        {
            // Replace with your Bluetooth device's address or retrieve it from discovery
            string deviceAddress = "30:9C:23:AC:45:ED"; // Replace with actual MAC address
            await ConnectToServerAsync(deviceAddress);
        }

        private async Task ConnectToServerAsync(string deviceAddress)
        {

            ulong bluetoothAddress = Convert.ToUInt64(deviceAddress.Replace(":", ""), 16);
            _device = await BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothAddress);

            if (_device == null)
            {
                await DisplayAlert("Error", "Device not found.", "OK");
                return;
            }

            var services = await _device.GetGattServicesAsync();
            var service = services.Services.FirstOrDefault(s => s.Uuid == new Guid("0000180D-0000-1000-8000-00805F9B34FB"));

            if (service == null)
            {
                await DisplayAlert("Error", "Service not found.", "OK");
                return;
            }

            var characteristics = await service.GetCharacteristicsAsync();
            _requestCharacteristic = characteristics.Characteristics.FirstOrDefault(c => c.Uuid == new Guid("00002A37-0000-1000-8000-00805F9B34FB"));
            _notificationCharacteristic = characteristics.Characteristics.FirstOrDefault(c => c.Uuid == new Guid("00002A38-0000-1000-8000-00805F9B34FB"));

            if (_requestCharacteristic != null)
            {
                Console.WriteLine("Request characteristic found.");
            }
            if (_notificationCharacteristic != null)
            {
                Console.WriteLine("Notification characteristic found.");
                _notificationCharacteristic.ValueChanged += OnNotificationReceived;
                await _notificationCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            }

        }


        private async Task SendRequestAsync(Dictionary<DataIdentifier, object> data)
        {

            if (_requestCharacteristic == null)
            {
                await DisplayAlert("Error", "Request characteristic not found.", "OK");
                return;
            }

            // Encode the data to CBOR format
            byte[] cborData = CborHandler.EncodeRequest(MessageType.Request);

            var writer = new DataWriter();
            writer.WriteBytes(cborData);
            var result = await _requestCharacteristic.WriteValueAsync(writer.DetachBuffer());

            if (result == GattCommunicationStatus.Success)
            {
                Console.WriteLine("Request sent successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send request.");
            }

        }


        private void OnNotificationReceived(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            byte[] receivedData = new byte[args.CharacteristicValue.Length];
            reader.ReadBytes(receivedData);

            // Decode the CBOR response
            CborHandler.DecodeResponse(receivedData);
           

            // Optionally remove the event handler if you only want to receive a single response per request
            _notificationCharacteristic.ValueChanged -= OnNotificationReceived;
        }



        private async void DisconnectButton_Clicked(object sender, EventArgs e)
        {
            if (_notificationCharacteristic != null)
            {
                await _notificationCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.None);
                _notificationCharacteristic.ValueChanged -= OnNotificationReceived;
            }

            _device?.Dispose();
            Console.WriteLine("Disconnected.");
        }


        public async Task ProcessBluetoothAsync(MessageType messageType, Dictionary<DataIdentifier, object> data)
        {

            if (_requestCharacteristic == null || _notificationCharacteristic == null)
            {
                Console.WriteLine("Bluetooth characteristics not initialized.");
                return;
            }

            // Step 1: Encode the request into CBOR format
            byte[] cborRequest = CborHandler.EncodeRequest(messageType);

            // Step 2: Send the request to the Bluetooth server by writing to the request characteristic
            var writer = new DataWriter();
            writer.WriteBytes(cborRequest);
            var result = await _requestCharacteristic.WriteValueAsync(writer.DetachBuffer());

            if (result == GattCommunicationStatus.Success)
            {
                Console.WriteLine("Request sent successfully.");

                // Step 3: Wait for the response from the notification characteristic
                _notificationCharacteristic.ValueChanged += OnNotificationReceived;
            }
            else
            {
                Console.WriteLine("Failed to send request.");
            }

        }


    }
}
