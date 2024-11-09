using System.Formats.Cbor;
using System.Linq.Expressions;
using cborModular.DataIdentifiers;
using cborModular.DataModels;
using cborModular.Services;
using cborModular.Services.BluetoothServices;


namespace cborModular
{
    public partial class MainPage : ContentPage
    {
        private readonly BluetoothManager _bluetoothManager;

        public MainPage()
        {
            InitializeComponent();
            _bluetoothManager = new BluetoothManager();





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
            dataProcessor.ProcessBluetooth(MessageType.Request);

            // Adding a list of requested identifiers
            var requestedIdentifiers = new List<RequestDataIdentifier>
            {
             RequestDataIdentifier.Speed,
             RequestDataIdentifier.GForce,
             RequestDataIdentifier.EngineRPM,
             RequestDataIdentifier.LightLevel,
             RequestDataIdentifier.Gear
            };
            DataStorage.AddRequest([.. requestedIdentifiers]);

            // Send another simulated Bluetooth request and reset
            dataProcessor.ProcessBluetooth(MessageType.Request);

            DataStorage.AddSet(SetDataIdentifier.ABS, true);
            dataProcessor.ProcessBluetooth(MessageType.Set);

            // Display retrieved data on the UI
            SpeedLabel.Text = $"Speed: {DataStorage.GetLastValue(RequestDataIdentifier.Speed)} km/h";
            ThrottleLabel.Text = $"Throttle: {DataStorage.GetLastValue(RequestDataIdentifier.Throttle)}%";

            TimeLabel.Text = $"Time: {DataStorage.GetLastValue(RequestDataIdentifier.Speed, entry => entry.Timestamp)}";
        }
        
      
        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            await _bluetoothManager.StartScanningAsync();
        }

        private async void OnConnectButtonClicked(object sender, EventArgs e)
        {
            Guid serviceGuid = new("0000180D-0000-1000-8000-00805F9B34FB");
            Guid requestCharacteristicGuid = new("00002A37-0000-1000-8000-00805F9B34FB");
            Guid notificationCharacteristicGuid = new("00002A38-0000-1000-8000-00805F9B34FB");

           

            await _bluetoothManager.ConnectAndSubscribeToCharacteristic(deviceGuid, serviceGuid, notificationCharacteristicGuid, data =>
            {
                // Handle notification data
                Console.WriteLine($"Notification received: {BitConverter.ToString(data)}");
            });
        }
    }
}
