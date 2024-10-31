using System.Formats.Cbor;
using cborModular.DataModels;
using cborModular.Services;


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
            //  var dataStorage = new Dictionary<DataIdentifier, List<object>>();

            DataStorage.AddData(DataIdentifier.Rychlost, 15.5f);

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
            dataProcessor.SendRequestAndStoreResponse(requestedIdentifiers);

            requestedIdentifiers = new List<DataIdentifier>
            {
             DataIdentifier.Rychlost,
             DataIdentifier.Pretizeni,
             DataIdentifier.OtackyMotoru,
             DataIdentifier.SvetelnaUroven,
             DataIdentifier.Plyn
            };

            dataProcessor.SendRequestAndStoreResponse(requestedIdentifiers);


            SpeedLabel.Text = $"Speed: {DataStorage.GetLastValue(DataIdentifier.Rychlost)} km/h";
            ThrottleLabel.Text = $"Throttle: {DataStorage.GetLastValue(DataIdentifier.Plyn)}%";
        }
    }
}
