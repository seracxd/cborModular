using System.Formats.Cbor;
using cborModular.Infrastructure;
using cborModular.Domain;
using cborModular.Application;
using static Microsoft.Maui.ApplicationModel.Permissions;


namespace cborModular
{
    public partial class MainPage : ContentPage
    {
        private readonly MotorcycleRepository _motorcycleRepository;
        private readonly MotorcycleDataService _motorcycleDataService;

        private readonly MotorcycleRepository _motorcycleRepositoryRead;
        private readonly MotorcycleDataService _motorcycleDataServiceRead;

        public MainPage()
        {
            InitializeComponent();

            _motorcycleRepository = new MotorcycleRepository();
            _motorcycleDataService = new MotorcycleDataService(_motorcycleRepository);

            _motorcycleRepositoryRead = new MotorcycleRepository();
            _motorcycleDataServiceRead = new MotorcycleDataService(_motorcycleRepositoryRead);

            // Example of using the service
            Task.Run(async () =>
            {
                await SaveSampleDataAsync();
                await RetrieveAndDisplayDataAsync();
            });
            
            
            // TODO: zjistit jak funguje
            var data = _motorcycleRepository.GetAllValuesAsync(MotorcycleDataFields.Speed);
        
        }
        private async Task SaveSampleDataAsync()
        {
            var sampleData = new MotorcycleData()
            {
                Speed = new MotorcycleDataWithTimestamp<float> { Value = 85.5f, Timestamp = DateTime.Now },
                Throttle = new MotorcycleDataWithTimestamp<float> { Value = 0.75f, Timestamp = DateTime.Now },
                XCoord = new MotorcycleDataWithTimestamp<float> { Value = 14.5f, Timestamp = DateTime.Now },
                YCoord = new MotorcycleDataWithTimestamp<float> { Value = 60.05f, Timestamp = DateTime.Now }
            };

            await _motorcycleRepository.SaveAsync(sampleData);
            Console.WriteLine("Sample data saved.");
        }

        // Method to retrieve the last saved data in CBOR format based on a request
        private async Task RetrieveAndDisplayDataAsync()
        {
            // Example CBOR request: Requesting only "Speed" and "Throttle"
            var requestParameters = new Dictionary<string, object>
            {
                { "request", new[] { "Speed", "Throttle" } }
            };
            byte[] cborRequestData = SerializeToCbor(requestParameters);

            // Get the requested data in CBOR format
            byte[] cborResponseData = await _motorcycleDataService.GetRequestedDataAsync(cborRequestData);

            // For demonstration purposes, decode and display the response data
            var parsedData = DisplayCborData(cborResponseData);
            if (parsedData != null)
            {
                await _motorcycleRepository.SaveAsync(parsedData);
            }
        }

        // Utility method to serialize a request to CBOR
        private byte[] SerializeToCbor(Dictionary<string, object> data)
        {
            // using (var stream = new MemoryStream())
            var writer = new CborWriter();

            writer.WriteStartMap(data.Count);
            foreach (var kvp in data)
            {
                writer.WriteTextString(kvp.Key);

                // Define 'arr' before conditional assignment
                string[] arr = kvp.Value as string[] ?? Array.Empty<string>();
                writer.WriteStartArray(arr.Length);

                foreach (var item in arr)
                {
                    writer.WriteTextString(item);
                }

                writer.WriteEndArray();
            }
            writer.WriteEndMap();
            return writer.Encode();

        }

        // Utility method to display CBOR response data
        private MotorcycleData? DisplayCborData(byte[] cborData)
        {
            var reader = new CborReader(cborData);
            var parsedData = new MotorcycleData();

            reader.ReadStartMap();
            while (reader.PeekState() != CborReaderState.EndMap)
            {
                string key = reader.ReadTextString();
                object value;

                switch (reader.PeekState())
                {
                    case CborReaderState.TextString:
                        value = reader.ReadTextString();
                        break;
                    case CborReaderState.DoublePrecisionFloat:
                        value = reader.ReadDouble();
                        break;
                    case CborReaderState.SinglePrecisionFloat:
                        value = reader.ReadSingle();
                        break;
                    case CborReaderState.HalfPrecisionFloat:
                        value = reader.ReadSingle();
                        break;
                    case CborReaderState.UnsignedInteger:
                        value = reader.ReadUInt64();
                        break;
                    case CborReaderState.NegativeInteger:
                        value = reader.ReadInt64();
                        break;
                    case CborReaderState.Boolean:
                        value = reader.ReadBoolean();
                        break;
                    case CborReaderState.Null:
                        reader.ReadNull();
                        value = "null";  // Explicitly set "null" as the string representation of a null value
                        break;
                    default:
                        // If an unsupported type is encountered, skip it
                        value = "Unknown Type";
                        reader.SkipValue();
                        break;
                }

                switch (key)
                {
                    case "Speed" when value is float speed:
                        parsedData.Speed = new MotorcycleDataWithTimestamp<float> { Value = speed, Timestamp = DateTime.Now };
                        break;
                    case "Throttle" when value is float throttle:
                        parsedData.Throttle = new MotorcycleDataWithTimestamp<float> { Value = throttle, Timestamp = DateTime.Now };
                        break;
                    case "XCoord" when value is float xCoord:
                        parsedData.XCoord = new MotorcycleDataWithTimestamp<float> { Value = xCoord, Timestamp = DateTime.Now };
                        break;
                    case "YCoord" when value is float yCoord:
                        parsedData.YCoord = new MotorcycleDataWithTimestamp<float> { Value = yCoord, Timestamp = DateTime.Now };
                        break;
                }
            }
            reader.ReadEndMap();
            return parsedData;
        }

    }
}
