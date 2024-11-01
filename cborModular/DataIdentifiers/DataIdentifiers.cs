using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace cborModular.DataIdentifiers
{
    public abstract class DataIdentifier
    {
        public int Id { get; }
        public Type ExpectedType { get; }

        protected DataIdentifier(int id, Type expectedType)
        {
            Id = id;
            ExpectedType = expectedType;
        }      
    }
    public class RequestDataIdentifier : DataIdentifier
    {
        // Vehicle speed identifiers
        public static readonly RequestDataIdentifier Speed = new RequestDataIdentifier(1, typeof(float));
        public static readonly RequestDataIdentifier Acceleration = new RequestDataIdentifier(2, typeof(float));
        public static readonly RequestDataIdentifier AverageSpeed = new RequestDataIdentifier(3, typeof(float));
        public static readonly RequestDataIdentifier MaxSpeed = new RequestDataIdentifier(4, typeof(float));
        public static readonly RequestDataIdentifier GForce = new RequestDataIdentifier(5, typeof(float));
        public static readonly RequestDataIdentifier MaxGForce = new RequestDataIdentifier(6, typeof(float));

        // Motor identifiers
        public static readonly RequestDataIdentifier EngineRPM = new RequestDataIdentifier(10, typeof(ushort));
        public static readonly RequestDataIdentifier EnginePower = new RequestDataIdentifier(11, typeof(ushort));
        public static readonly RequestDataIdentifier EngineTemperature = new RequestDataIdentifier(12, typeof(float));
        public static readonly RequestDataIdentifier Throttle = new RequestDataIdentifier(13, typeof(float));
        public static readonly RequestDataIdentifier Gear = new RequestDataIdentifier(15, typeof(sbyte));

        // Battery identifiers
        public static readonly RequestDataIdentifier BatteryLevel = new RequestDataIdentifier(20, typeof(byte));
        public static readonly RequestDataIdentifier EstimatedRange = new RequestDataIdentifier(21, typeof(float));
        public static readonly RequestDataIdentifier BatteryTemperature = new RequestDataIdentifier(22, typeof(float));
        public static readonly RequestDataIdentifier ChargeTime = new RequestDataIdentifier(23, typeof(TimeSpan));
        public static readonly RequestDataIdentifier ConsumptionPerKm = new RequestDataIdentifier(24, typeof(float));
        public static readonly RequestDataIdentifier RideSmoothness = new RequestDataIdentifier(25, typeof(float));
        public static readonly RequestDataIdentifier ConsumptionPerKWh = new RequestDataIdentifier(26, typeof(float));
        public static readonly RequestDataIdentifier BatteryRegen = new RequestDataIdentifier(27, typeof(float));

        // GPS identifiers
        public static readonly RequestDataIdentifier GPS_North = new RequestDataIdentifier(40, typeof(float));
        public static readonly RequestDataIdentifier GPS_East = new RequestDataIdentifier(41, typeof(float));
        public static readonly RequestDataIdentifier Altitude = new RequestDataIdentifier(42, typeof(ushort));
        public static readonly RequestDataIdentifier Direction = new RequestDataIdentifier(43, typeof(float));
        public static readonly RequestDataIdentifier Distance = new RequestDataIdentifier(44, typeof(float));
        public static readonly RequestDataIdentifier Incline = new RequestDataIdentifier(45, typeof(sbyte));
        public static readonly RequestDataIdentifier Gyroscope = new RequestDataIdentifier(46, typeof(object)); // Placeholder type

        // Environment identifiers
        public static readonly RequestDataIdentifier AmbientTemperature = new RequestDataIdentifier(60, typeof(float));
        public static readonly RequestDataIdentifier Humidity = new RequestDataIdentifier(61, typeof(float));
        public static readonly RequestDataIdentifier Pressure = new RequestDataIdentifier(62, typeof(float));
        public static readonly RequestDataIdentifier EnvironmentTime = new RequestDataIdentifier(63, typeof(TimeSpan));
        public static readonly RequestDataIdentifier LightLevel = new RequestDataIdentifier(64, typeof(float));

       
        private RequestDataIdentifier(int id, Type expectedType) : base(id, expectedType)
        {
            DataIdentifierRegistry.Register(this);
        }
    }

    public class SetDataIdentifier : DataIdentifier
    {
        // Control elements
        public static readonly SetDataIdentifier PowerSetting = new SetDataIdentifier(70, typeof(float));
        public static readonly SetDataIdentifier BatteryProtection = new SetDataIdentifier(71, typeof(byte));
        public static readonly SetDataIdentifier RegenerativeBraking = new SetDataIdentifier(72, typeof(bool));
        public static readonly SetDataIdentifier Lights = new SetDataIdentifier(73, typeof(bool));
        public static readonly SetDataIdentifier HazardLights = new SetDataIdentifier(74, typeof(bool));
        public static readonly SetDataIdentifier ABS = new SetDataIdentifier(75, typeof(bool));
        public static readonly SetDataIdentifier BrakePower = new SetDataIdentifier(76, typeof(bool));
        public static readonly SetDataIdentifier TractionControl = new SetDataIdentifier(77, typeof(bool));
        public static readonly SetDataIdentifier DrivingMode = new SetDataIdentifier(78, typeof(string)); // Enum could be used instead
        public static readonly SetDataIdentifier MaxSpeedSetting = new SetDataIdentifier(79, typeof(ushort));
        public static readonly SetDataIdentifier HandBrake = new SetDataIdentifier(80, typeof(bool));
        public static readonly SetDataIdentifier DemoMode = new SetDataIdentifier(81, typeof(bool));

        // Safety features
        public static readonly SetDataIdentifier Immobilizer = new SetDataIdentifier(90, typeof(bool));
        public static readonly SetDataIdentifier Alarm = new SetDataIdentifier(91, typeof(bool));
        public static readonly SetDataIdentifier Geofencing = new SetDataIdentifier(92, typeof(ushort));

        public static readonly SetDataIdentifier IsActive = new SetDataIdentifier(100, typeof(bool));

        private SetDataIdentifier(int id, Type expectedType) : base(id, expectedType)
        {
            DataIdentifierRegistry.Register(this);
        }
    }
}