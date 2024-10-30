using cborModular.Infrastructure;
using System;
using System.Collections.Generic;
using System.Formats.Cbor;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace cborModular.Application
{
    internal class MotorcycleDataService
    {
        private readonly MotorcycleRepository _motorcycleRepository;

        internal MotorcycleDataService(MotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<byte[]> GetRequestedDataAsync(byte[] serializedRequest)
        {
            // Step 1: Parse the CBOR request to get requested fields
            List<string> requestedFields = ParseRequest(serializedRequest);

            // Step 2: Get the last motorcycle data
            var lastData = await _motorcycleRepository.GetLastDataAsync();
            if (lastData == null)
            {
                throw new InvalidOperationException("No data available.");
            }

            // Step 3: Prepare a dictionary with requested fields
            var responseData = new Dictionary<string, object>();
            foreach (var field in requestedFields)
            {
                switch (field)
                {
                    case "Speed":
                        if (lastData.Speed != null) responseData["Speed"] = lastData.Speed.Value;
                        break;
                    case "Throttle":
                        if (lastData.Throttle != null) responseData["Throttle"] = lastData.Throttle.Value;
                        break;
                    case "XCoord":
                        if (lastData.XCoord != null) responseData["XCoord"] = lastData.XCoord.Value;
                        break;
                    case "YCoord":
                        if (lastData.YCoord != null) responseData["YCoord"] = lastData.YCoord.Value;
                        break;
                    default:
                        Console.WriteLine($"Unknown field requested: {field}");
                        break;
                }
            }

            // Step 4: Serialize the response data to CBOR
            return SerializeResponse(responseData);
        }

        private List<string> ParseRequest(byte[] serializedRequest)
        {
            var requestedFields = new List<string>();

            var reader = new CborReader(serializedRequest);

            reader.ReadStartMap();
            if (reader.ReadTextString() == "request")
            {
                reader.ReadStartArray();
                while (reader.PeekState() != CborReaderState.EndArray)
                {
                    requestedFields.Add(reader.ReadTextString());
                }
                reader.ReadEndArray();
            }
            reader.ReadEndMap();


            return requestedFields;
        }

        private byte[] SerializeResponse(Dictionary<string, object> responseData)
        {                  
                var writer = new CborWriter();
            
                writer.WriteStartMap(responseData.Count);
                foreach (var kvp in responseData)
                {
                    writer.WriteTextString(kvp.Key);
                    switch (kvp.Value)
                    {
                        case double d:
                            writer.WriteDouble(d);
                            break;
                        case float f:
                            writer.WriteSingle(f);
                            break;
                        default:
                            throw new InvalidOperationException($"Unsupported data type for field {kvp.Key}");
                    }
                }
                writer.WriteEndMap();
                return writer.Encode();
            
        }
    }
}
