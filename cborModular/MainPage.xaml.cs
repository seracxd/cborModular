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

            // funkce pro simulaci bluetooth
            var bluetoothSimulator = new BluetoothSimulator();
            var dataProcessor = new DataProcessor(bluetoothSimulator);

            // manuální přidání dat
            DataStorage.AddData(DataIdentifier.Rychlost, 15.5f);
            
            // přidání požadovaných parametrů do bluetooth requestu
            DataStorage.AddRequest([DataIdentifier.Rychlost, DataIdentifier.Plyn]);
            // můžu přidávat na několikrát, ochrana proti duplikátům
            DataStorage.AddRequest([DataIdentifier.Rychlost, DataIdentifier.PrumernaRychlost, DataIdentifier.RucniBrzda]);

            // simulace bluetooth, vynulování requestu
            dataProcessor.SendRequestAndStoreResponse();


            var requestedIdentifiers = new List<DataIdentifier>
            {
             DataIdentifier.Rychlost,
             DataIdentifier.Pretizeni,
             DataIdentifier.OtackyMotoru,
             DataIdentifier.SvetelnaUroven,
             DataIdentifier.Prevod
            };
            // můžu vložit List
            DataStorage.AddRequest(requestedIdentifiers.ToArray());

            dataProcessor.SendRequestAndStoreResponse();

            // zobrazení vybraných dat
            SpeedLabel.Text = $"Speed: {DataStorage.GetLastValue(DataIdentifier.Rychlost)} km/h";
            ThrottleLabel.Text = $"Throttle: {DataStorage.GetLastValue(DataIdentifier.Plyn)}%";
        }
    }
}
