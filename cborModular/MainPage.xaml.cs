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
          


            DataStorage.AddRequest([DataIdentifier.Rychlost, DataIdentifier.Plyn]);
        
            // Send request and store response
            dataProcessor.SendRequestAndStoreResponse();

            var requestedIdentifiers = new List<DataIdentifier>
            {
             DataIdentifier.Rychlost,
             DataIdentifier.Pretizeni,
             DataIdentifier.OtackyMotoru,
             DataIdentifier.SvetelnaUroven,
             DataIdentifier.Prevod
            };
            DataStorage.AddRequest(requestedIdentifiers.ToArray());

            dataProcessor.SendRequestAndStoreResponse();


            SpeedLabel.Text = $"Speed: {DataStorage.GetLastValue(DataIdentifier.Rychlost)} km/h";
            ThrottleLabel.Text = $"Throttle: {DataStorage.GetLastValue(DataIdentifier.Plyn)}%";
        }
    }
}
