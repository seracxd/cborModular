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


namespace cborModular
{
    public partial class MainPage : ContentPage
    {
        private readonly BleScanner _bleClient;
      

        public MainPage()
        {
            InitializeComponent();
          

            // Inicializace BLE skeneru
            _bleClient = new BleScanner();
            _bleClient.ApplicationDiscovered += OnApplicationDiscovered;
        }

        private async void OnStartScanningClicked(object sender, EventArgs e)
        {
            await _bleClient.StartScanningAsync();
        }

        private void OnApplicationDiscovered(object sender, string appName)
        {
            
        }
    }
}
