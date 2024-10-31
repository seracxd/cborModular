using System.Formats.Cbor;
using cborModular.DataModels;
using cborModular.Services;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace cborModular
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var bluetoothSimulator = new BluetoothSimulator();
            var dataProcessor = new DataProcessor(bluetoothSimulator);

            // Initialize storage dictionary
            var dataStorage = new Dictionary<DataIdentifier, List<object>>();

            // List of data identifiers you want to request
            var requestedIdentifiers = new List<DataIdentifier>
            {
             DataIdentifier.Rychlost,
             DataIdentifier.Pretizeni,
             DataIdentifier.OtackyMotoru,
             DataIdentifier.NouzovaSvetla,
             DataIdentifier.Pretizeni,
             DataIdentifier.RegenerativniBrzdeni
            };

            // Send request and store response
            dataProcessor.SendRequestAndStoreResponse(requestedIdentifiers, dataStorage);

            requestedIdentifiers = new List<DataIdentifier>
            {
             DataIdentifier.Rychlost,
             DataIdentifier.Pretizeni,
             DataIdentifier.OtackyMotoru,
             DataIdentifier.SvetelnaUroven
            };

            dataProcessor.SendRequestAndStoreResponse(requestedIdentifiers, dataStorage);

            if (dataStorage.TryGetValue(DataIdentifier.Rychlost, out var rychlostValues) && rychlostValues.Count > 0)
            {             
                SpeedLabel.Text = $"Speed: {rychlostValues[^1]} km/h";  // poslední prvek
            }
        }
    }
}
