using cborModular.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Services
{
    public class BluetoothSimulator
    {
        private readonly Dictionary<DataIdentifier, object> dataStorage = new Dictionary<DataIdentifier, object>();

        public BluetoothSimulator()
        {
            // Initialize each DataIdentifier with a sample value based on its type
            dataStorage[DataIdentifier.Rychlost] = 120.5f;        // float
            dataStorage[DataIdentifier.Akcelerace] = 2.3f;        // float
            dataStorage[DataIdentifier.PrumernaRychlost] = 90.0f; // float
            dataStorage[DataIdentifier.MaximalniRychlost] = 130.2f; // float
            dataStorage[DataIdentifier.Pretizeni] = 0.8f;         // float
            dataStorage[DataIdentifier.MaxPretizeni] = 1.5f;      // float

            // Motor-related identifiers
            dataStorage[DataIdentifier.OtackyMotoru] = (ushort)6000;  // ushort
            dataStorage[DataIdentifier.VykonMotoru] = (ushort)150;    // ushort
            dataStorage[DataIdentifier.TeplotaMotoru] = 85.0f;        // float
            dataStorage[DataIdentifier.Prevod] = (sbyte)3;            // sbyte
            dataStorage[DataIdentifier.Plyn] = 65.0f;

            // Battery-related identifiers
            dataStorage[DataIdentifier.UrovenBaterie] = (byte)75;          // byte
            dataStorage[DataIdentifier.OdhadovanyDojezd] = 120.0f;         // float
            dataStorage[DataIdentifier.TeplotaBaterie] = 45.0f;            // float
            dataStorage[DataIdentifier.CasDoNabiti] = TimeSpan.FromMinutes(30); // TimeSpan
            dataStorage[DataIdentifier.SpotrebaPerKm] = 8.2f;              // float
            dataStorage[DataIdentifier.PlynulostJizdy] = 7.8f;             // float
            dataStorage[DataIdentifier.SpotrebaPerKWh] = 0.12f;            // float
            dataStorage[DataIdentifier.RegenBaterie] = 5.6f;               // float

            // GPS-related identifiers
            dataStorage[DataIdentifier.GPS_N] = 50.087;                    // float (latitude)
            dataStorage[DataIdentifier.GPS_E] = 14.421;                    // float (longitude)
            dataStorage[DataIdentifier.NadmorskaVyska] = (ushort)320;      // ushort
            dataStorage[DataIdentifier.Smer] = 180.0f;                     // float
            dataStorage[DataIdentifier.Vzdalenost] = 50.0f;                // float
            dataStorage[DataIdentifier.Sklon] = (sbyte)-5;                 // sbyte
            dataStorage[DataIdentifier.Gyroskop] = new object();           // Placeholder

            // Environment-related identifiers
            dataStorage[DataIdentifier.TeplotaProstredi] = 22.5f;          // float
            dataStorage[DataIdentifier.Vlhkost] = 55.0f;                   // float
            dataStorage[DataIdentifier.Tlak] = 1013.0f;                    // float
            dataStorage[DataIdentifier.CasProstredi] = TimeSpan.FromHours(1); // TimeSpan
            dataStorage[DataIdentifier.SvetelnaUroven] = 300.0f;           // float

            // Control elements identifiers
            dataStorage[DataIdentifier.NastaveniVykonu] = 0.75f;           // float
            dataStorage[DataIdentifier.OchranyBaterie] = (byte)1;          // byte
            dataStorage[DataIdentifier.RegenerativniBrzdeni] = true;       // bool
            dataStorage[DataIdentifier.Svetla] = true;                     // bool
            dataStorage[DataIdentifier.NouzovaSvetla] = false;             // bool
            dataStorage[DataIdentifier.ABS] = true;                        // bool
            dataStorage[DataIdentifier.BrzdovyVykon] = false;              // bool
            dataStorage[DataIdentifier.Trakce] = 0;                        // Placeholder for enum
            dataStorage[DataIdentifier.RezimJizdy] = 1;                    // Placeholder for enum
            dataStorage[DataIdentifier.MaxRychlost] = (ushort)150;         // ushort
            dataStorage[DataIdentifier.RucniBrzda] = true;                 // bool
            dataStorage[DataIdentifier.DemoRezim] = false;                 // bool

            // Safety features identifiers
            dataStorage[DataIdentifier.Imobilizer] = true;                 // bool
            dataStorage[DataIdentifier.Alarm] = false;                     // bool
            dataStorage[DataIdentifier.Geofencing] = (ushort)1;            // ushort

            // Notifications identifier (placeholder object)
            dataStorage[DataIdentifier.Aktivni] =true;         // bool
        }

        /// <summary>
        /// Processes a CBOR-encoded Bluetooth request and returns a CBOR-encoded response.
        /// </summary>
        /// <param name="cborRequest">CBOR-encoded request specifying data identifiers</param>
        /// <returns>CBOR-encoded response with requested data</returns>
        public byte[] ProcessBluetoothRequest(byte[] cborRequest)
        {
            var requestedIdentifiers = CborHandler.DecodeRequest(cborRequest);
            var responseData = new Dictionary<DataIdentifier, object>();

            foreach (var identifier in requestedIdentifiers)
            {
                if (dataStorage.TryGetValue(identifier, out var value))
                {
                    responseData[identifier] = value;
                }
                else
                {
                    Console.WriteLine($"Warning: Data for identifier {identifier} not found.");
                }
            }

            return CborHandler.EncodeResponse(responseData);
        }

        /// <summary>
        /// Adds data to the simulator's storage.
        /// </summary>
        /// <param name="identifier">The DataIdentifier key</param>
        /// <param name="value">The value to store</param>
        public void AddData(DataIdentifier identifier, object value)
        {
            dataStorage[identifier] = value;
        }
    }
}
