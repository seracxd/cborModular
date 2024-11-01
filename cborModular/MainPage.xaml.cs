﻿using System.Formats.Cbor;
using cborModular.DataIdentifiers;
using cborModular.DataModels;
using cborModular.Services;


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

            // Manual data addition with type safety
            DataStorage.AddData(RequestDataIdentifier.Speed, 15.5f);

            // Add requested parameters for Bluetooth request
            DataStorage.AddRequest(RequestDataIdentifier.Speed, RequestDataIdentifier.Throttle);

            // Multiple additions without duplication
            DataStorage.AddRequest(RequestDataIdentifier.Speed, RequestDataIdentifier.AverageSpeed, SetDataIdentifier.HandBrake);

            // Simulate Bluetooth request and reset request storage
            dataProcessor.SendRequestAndStoreResponse();

            // Adding a list of requested identifiers
            var requestedIdentifiers = new List<RequestDataIdentifier>
            {
             RequestDataIdentifier.Speed,
             RequestDataIdentifier.GForce,
             RequestDataIdentifier.EngineRPM,
             RequestDataIdentifier.LightLevel,
             RequestDataIdentifier.Gear
            };
            DataStorage.AddRequest(requestedIdentifiers.ToArray());

            // Send another simulated Bluetooth request and reset
            dataProcessor.SendRequestAndStoreResponse();

            // Display retrieved data on the UI
            SpeedLabel.Text = $"Speed: {DataStorage.GetLastValue(RequestDataIdentifier.Speed)} km/h";
            ThrottleLabel.Text = $"Throttle: {DataStorage.GetLastValue(RequestDataIdentifier.Throttle)}%";
        }
    }
}
