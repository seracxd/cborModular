﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Formats.Cbor;
using System.Linq.Expressions;
using cborModular.DataIdentifiers;
using cborModular.DataModels;
using cborModular.Services;
using System.Collections.ObjectModel;
using cborModular.Services.BluetoothServices;
using Plugin.BLE.Abstractions.Contracts;
using cborModular.Interfaces;
using cborModular.Interfaces.Controlers;


namespace cborModular
{
    public partial class MainPage : ContentPage
    {
        private readonly IBluetoothService _bluetoothService;
        private IDevice _selectedDevice;
        //private readonly ICharacteristic _selectedCharacteristic;
        private readonly IDataService _dataService;

        public ObservableCollection<IDevice> DiscoveredDevices { get; } = [];


        public MainPage(IBluetoothService bluetoothService, IDataService dataService )
        {
            InitializeComponent();
            _bluetoothService = bluetoothService;
            _dataService = dataService;

            // Připojení událostí
            _bluetoothService.DeviceConnected += OnDeviceConnected;
            _bluetoothService.DeviceDisconnected += OnDeviceDisconnected;
            _bluetoothService.NotificationReceived += OnNotificationReceived;
            _bluetoothService.DeviceDiscovered += OnDeviceDiscovered;
            DevicesListView.ItemsSource = DiscoveredDevices;
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            // Zastaví skenování Bluetooth zařízení při opuštění stránky
            await _bluetoothService.StopScanningAsync();

            // Odpojení od událostí
            _bluetoothService.DeviceConnected -= OnDeviceConnected;
            _bluetoothService.DeviceDisconnected -= OnDeviceDisconnected;
            _bluetoothService.NotificationReceived -= OnNotificationReceived;
            _bluetoothService.DeviceDiscovered -= OnDeviceDiscovered;
        }

        private void OnDeviceDiscovered(object sender, IDevice device)
        {
            // Aktualizace UI musí probíhat ve vlákně UI
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!DiscoveredDevices.Contains(device))
                {
                    DiscoveredDevices.Add(device);                    
                }
            });
        }

        private async void OnStartScanningClicked(object sender, EventArgs e)
        {
            await _bluetoothService.StartScanningAsync();
        }

        // Obsluha výběru zařízení
        private async void OnDeviceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _selectedDevice = e.SelectedItem as IDevice;
            if (_selectedDevice != null)
            {
                await _bluetoothService.ConnectToDeviceAsync(_selectedDevice);
            }
        }
       
        private async void OnSendRequestClicked(object sender, EventArgs e)
        {
            _dataService.
            if (_selectedCharacteristic == null)
            {
                await DisplayAlert("Warning", "Please select a characteristic first.", "OK");
                return;
            }


            _dataService.AddRequest(RequestDataIdentifier.Speed, RequestDataIdentifier.Throttle);

            var cborHandler = new CborHandler(_dataService);
            byte[] cborData = cborHandler.EncodeRequest(MessageType.Request);

            await _bluetoothService.SendRequestAsync(_selectedCharacteristic, cborData);
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Zahájí skenování Bluetooth zařízení při zobrazení stránky
            await _bluetoothService.StartScanningAsync();
        }

        // Metoda pro obsluhu připojení zařízení
        private void OnDeviceConnected(object sender, IDevice device)
        {
            Console.WriteLine($"Device connected: {device.Name}");
        }

        // Metoda pro obsluhu odpojení zařízení
        private void OnDeviceDisconnected(object sender, IDevice device)
        {
            Console.WriteLine($"Device disconnected: {device.Name}");
        }

        // Metoda pro obsluhu příchozí notifikace
        private void OnNotificationReceived(object sender, byte[] data)
        {
            Console.WriteLine("Notification received: " + BitConverter.ToString(data));
        }       
    }
}
