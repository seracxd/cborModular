using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Formats.Cbor;
using System.Linq.Expressions;
using cborModular.DataIdentifiers;
using cborModular.DataModels;
using cborModular.Services;

 
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
 

namespace cborModular
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Bluetooth simulation setup
            var bluetoothSimulator = new BluetoothSimulator();
            var dataProcessor = new DataProcessor(bluetoothSimulator);


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
