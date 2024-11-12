using System;
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


namespace cborModular
{
    public partial class MainPage : ContentPage
    {
        private readonly BleScanner _bleClient;
        private readonly BleConnection _bleConnection;

        private  BleGetServices bleGetServices;

        public MainPage()
        {
            InitializeComponent();

            // Inicializace BLE skeneru
            _bleClient = new BleScanner(Dispatcher);
            _bleConnection = new BleConnection(_bleClient);


            DevicesListView.ItemsSource = _bleClient.DiscoveredDevices;

        }

        private async void OnStartScanningClicked(object sender, EventArgs e)
        {
            await _bleClient.StartScanningAsync();
        }
        private async void OnDeviceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedDevice = e.SelectedItem as IDevice;
            if (selectedDevice != null)
            {
                await _bleConnection.ConnectToDeviceAsync(selectedDevice);

                bleGetServices = new BleGetServices(selectedDevice);
            }
        }

        private async void OnGetCharakteristicsClicked(object sender, EventArgs e)
        {         
            await bleGetServices.DisplayServicesAndCharacteristicsAsync();
        }
    }
}
