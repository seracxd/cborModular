using cborModular.DataIdentifiers;
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
            // Initialize each DataIdentifier with sample values based on its ExpectedType
            InitializeData(RequestDataIdentifier.Speed, 120.5f);        // float
            InitializeData(RequestDataIdentifier.Acceleration, 2.3f);  // float
            InitializeData(RequestDataIdentifier.AverageSpeed, 90.0f); // float
            InitializeData(RequestDataIdentifier.MaxSpeed, 130.2f);    // float
            InitializeData(RequestDataIdentifier.GForce, 0.8f);        // float
            InitializeData(RequestDataIdentifier.MaxGForce, 1.5f);     // float

            // Motor-related identifiers
            InitializeData(RequestDataIdentifier.EngineRPM, (ushort)6000);  // ushort
            InitializeData(RequestDataIdentifier.EnginePower, (ushort)150); // ushort
            InitializeData(RequestDataIdentifier.EngineTemperature, 85.0f); // float
            InitializeData(RequestDataIdentifier.Gear, (sbyte)3);           // sbyte
            InitializeData(RequestDataIdentifier.Throttle, 65.0f);          // float

            // Battery-related identifiers
            InitializeData(RequestDataIdentifier.BatteryLevel, (byte)75);               // byte
            InitializeData(RequestDataIdentifier.EstimatedRange, 120.0f);               // float
            InitializeData(RequestDataIdentifier.BatteryTemperature, 45.0f);            // float
            InitializeData(RequestDataIdentifier.ChargeTime, TimeSpan.FromMinutes(30)); // TimeSpan
            InitializeData(RequestDataIdentifier.ConsumptionPerKm, 8.2f);               // float
            InitializeData(RequestDataIdentifier.RideSmoothness, 7.8f);                 // float
            InitializeData(RequestDataIdentifier.ConsumptionPerKWh, 0.12f);             // float
            InitializeData(RequestDataIdentifier.BatteryRegen, 5.6f);                   // float

            // GPS-related identifiers
            InitializeData(RequestDataIdentifier.GPS_North, 50.087f);              // float (latitude)
            InitializeData(RequestDataIdentifier.GPS_East, 14.421f);              // float (longitude)
            InitializeData(RequestDataIdentifier.Altitude, (ushort)320);      // ushort
            InitializeData(RequestDataIdentifier.Direction, 180.0f);          // float
            InitializeData(RequestDataIdentifier.Distance, 50.0f);            // float
            InitializeData(RequestDataIdentifier.Incline, (sbyte)-5);         // sbyte
            InitializeData(RequestDataIdentifier.Gyroscope, new object());    // Placeholder object

            // Environment-related identifiers
            InitializeData(RequestDataIdentifier.AmbientTemperature, 22.5f);         // float
            InitializeData(RequestDataIdentifier.Humidity, 55.0f);                   // float
            InitializeData(RequestDataIdentifier.Pressure, 1013.0f);                 // float
            InitializeData(RequestDataIdentifier.EnvironmentTime, TimeSpan.FromHours(1)); // TimeSpan
            InitializeData(RequestDataIdentifier.LightLevel, 300.0f);                // float

            // Control element identifiers
            InitializeData(SetDataIdentifier.PowerSetting, 0.75f);                   // float
            InitializeData(SetDataIdentifier.BatteryProtection, (byte)1);            // byte
            InitializeData(SetDataIdentifier.RegenerativeBraking, true);             // bool
            InitializeData(SetDataIdentifier.Lights, true);                          // bool
            InitializeData(SetDataIdentifier.HazardLights, false);                   // bool
            InitializeData(SetDataIdentifier.ABS, true);                             // bool
            InitializeData(SetDataIdentifier.BrakePower, false);                     // bool
            InitializeData(SetDataIdentifier.TractionControl, false);                    // Placeholder enum
            InitializeData(SetDataIdentifier.DrivingMode, "ECO");                          // Placeholder enum
            InitializeData(SetDataIdentifier.MaxSpeedSetting, (ushort)150);          // ushort
            InitializeData(SetDataIdentifier.HandBrake, true);                       // bool
            InitializeData(SetDataIdentifier.DemoMode, false);                       // bool

            // Safety feature identifiers
            InitializeData(SetDataIdentifier.Immobilizer, true);                     // bool
            InitializeData(SetDataIdentifier.Alarm, false);                          // bool
            InitializeData(SetDataIdentifier.Geofencing, (ushort)1);                 // ushort

            // Notifications identifier
            InitializeData(SetDataIdentifier.IsActive, true);                    // bool
        }

        /// <summary>
        /// Initializes a data identifier with a specified value, ensuring type safety.
        /// </summary>
        private void InitializeData(DataIdentifier identifier, object value)
        {
            if (value.GetType() != identifier.ExpectedType)
            {
                throw new InvalidOperationException($"Invalid type for {identifier}. Expected {identifier.ExpectedType}, got {value.GetType()}.");
            }
            dataStorage[identifier] = value;
        }

        /// <summary>
        /// Processes a CBOR-encoded Bluetooth request and returns a CBOR-encoded response.
        /// </summary>
        /// <param name="cborRequest">CBOR-encoded request specifying data identifiers</param>
        /// <returns>CBOR-encoded response with requested data</returns>
        public byte[] ProcessBluetoothRequest(byte[] cborRequest)
        {
            var (sequenceNumber, requestedIdentifiers ) = CborHandler.DecodeRequest(cborRequest);
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

            return CborHandler.EncodeResponse(sequenceNumber ,responseData);
        }
    }
}
