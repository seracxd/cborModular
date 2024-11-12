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


        public MainPage()
        {
            InitializeComponent();

            // Inicializace BLE skeneru
            _bleClient = new BleScanner(this);
         
            DevicesListView.ItemsSource = _bleClient.DiscoveredDevices;
        }

        //internal async Task HandleDeviceConnectedAsync(IDevice device)
        //{
        //    // Zajistíme, že operace proběhne na hlavním vlákně pomocí Dispatcher
        //    await Dispatcher.DispatchAsync(() => DeviceDetails.Clear());

        //    // Získání a zobrazení služeb zařízení
        //    var services = await device.GetServicesAsync();
            
        //    foreach (var service in services)
        //    {
        //        await Dispatcher.DispatchAsync(() => DeviceDetails.Add($"Služba: {service.Id}"));

        //        // Získání a zobrazení charakteristik pro každou službu
        //        var characteristics = await service.GetCharacteristicsAsync();
        //        foreach (var characteristic in characteristics)
        //        {
        //            await Dispatcher.DispatchAsync(() => DeviceDetails.Add($"  Charakteristika: {characteristic.Id}"));
        //            await Dispatcher.DispatchAsync(() => DeviceDetails.Add($"    Vlastnosti: {characteristic.Properties}"));

        //            // Zobrazení deskriptorů (volitelné)
        //            var descriptors = await characteristic.GetDescriptorsAsync();
        //            foreach (var descriptor in descriptors)
        //            {
        //                await Dispatcher.DispatchAsync(() => DeviceDetails.Add($"    Deskriptor: {descriptor.Id}"));
        //            }
        //        }
        //    }
        //}



        private async void OnStartScanningClicked(object sender, EventArgs e)
        {
            await _bleClient.StartScanningAsync();
        }

    }
}
