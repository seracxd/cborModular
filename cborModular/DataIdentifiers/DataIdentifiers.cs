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

        /// <summary>Speed of the vehicle. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Speed = new RequestDataIdentifier(1, typeof(float));
        /// <summary>Acceleration of the vehicle. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Acceleration = new RequestDataIdentifier(2, typeof(float));
        /// <summary>Average speed of the vehicle. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier AverageSpeed = new RequestDataIdentifier(3, typeof(float));
        /// <summary>Maximum speed of the vehicle. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier MaxSpeed = new RequestDataIdentifier(4, typeof(float));
        /// <summary>G-Force experienced. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier GForce = new RequestDataIdentifier(5, typeof(float));
        /// <summary>Maximum G-Force experienced. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier MaxGForce = new RequestDataIdentifier(6, typeof(float));

        // Motor identifiers
        /// <summary>Engine revolutions per minute (RPM). Expected type: <c>ushort</c></summary>
        public static readonly RequestDataIdentifier EngineRPM = new RequestDataIdentifier(10, typeof(ushort));
        /// <summary>Engine power. Expected type: <c>ushort</c></summary>
        public static readonly RequestDataIdentifier EnginePower = new RequestDataIdentifier(11, typeof(ushort));
        /// <summary>Engine temperature. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier EngineTemperature = new RequestDataIdentifier(12, typeof(float));
        /// <summary>Throttle level. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Throttle = new RequestDataIdentifier(13, typeof(float));
        /// <summary>Gear. Expected type: <c>sbyte</c></summary>
        public static readonly RequestDataIdentifier Gear = new RequestDataIdentifier(15, typeof(sbyte));

        // Battery identifiers
        /// <summary>Battery level. Expected type: <c>byte</c></summary>
        public static readonly RequestDataIdentifier BatteryLevel = new RequestDataIdentifier(20, typeof(byte));
        /// <summary>Estimated range. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier EstimatedRange = new RequestDataIdentifier(21, typeof(float));
        /// <summary>Battery temperature. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier BatteryTemperature = new RequestDataIdentifier(22, typeof(float));
        /// <summary>Time until fully charged. Expected type: <c>TimeSpan</c></summary>
        public static readonly RequestDataIdentifier ChargeTime = new RequestDataIdentifier(23, typeof(TimeSpan));
        /// <summary>Consumption per kilometer. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier ConsumptionPerKm = new RequestDataIdentifier(24, typeof(float));
        /// <summary>Ride smoothness. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier RideSmoothness = new RequestDataIdentifier(25, typeof(float));
        /// <summary>Consumption per kilowatt-hour (kWh). Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier ConsumptionPerKWh = new RequestDataIdentifier(26, typeof(float));
        /// <summary>Battery regeneration level. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier BatteryRegen = new RequestDataIdentifier(27, typeof(float));

        // GPS identifiers
        /// <summary>GPS North coordinate. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier GPS_North = new RequestDataIdentifier(40, typeof(float));
        /// <summary>GPS East coordinate. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier GPS_East = new RequestDataIdentifier(41, typeof(float));
        /// <summary>Altitude above sea level. Expected type: <c>ushort</c></summary>
        public static readonly RequestDataIdentifier Altitude = new RequestDataIdentifier(42, typeof(ushort));
        /// <summary>Direction. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Direction = new RequestDataIdentifier(43, typeof(float));
        /// <summary>Distance traveled. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Distance = new RequestDataIdentifier(44, typeof(float));
        /// <summary>Incline angle. Expected type: <c>sbyte</c></summary>
        public static readonly RequestDataIdentifier Incline = new RequestDataIdentifier(45, typeof(sbyte));
        /// <summary>Gyroscope data (object placeholder). Expected type: <c>object</c></summary>
        public static readonly RequestDataIdentifier Gyroscope = new RequestDataIdentifier(46, typeof(object));

        // Environment identifiers
        /// <summary>Ambient temperature. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier AmbientTemperature = new RequestDataIdentifier(60, typeof(float));
        /// <summary>Humidity level. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Humidity = new RequestDataIdentifier(61, typeof(float));
        /// <summary>Atmospheric pressure. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Pressure = new RequestDataIdentifier(62, typeof(float));
        /// <summary>Environment time measurement. Expected type: <c>TimeSpan</c></summary>
        public static readonly RequestDataIdentifier EnvironmentTime = new RequestDataIdentifier(63, typeof(TimeSpan));
        /// <summary>Light level. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier LightLevel = new RequestDataIdentifier(64, typeof(float));



        private RequestDataIdentifier(int id, Type expectedType) : base(id, expectedType)
        {
            DataIdentifierRegistry.Register(this);
        }
    }

    public class SetDataIdentifier : DataIdentifier
    {
        // Control elements
        /// <summary>Power setting of the vehicle. Expected type: <c>float</c></summary>
        public static readonly SetDataIdentifier PowerSetting = new SetDataIdentifier(70, typeof(float));
        /// <summary>Battery protection level. Expected type: <c>byte</c></summary>
        public static readonly SetDataIdentifier BatteryProtection = new SetDataIdentifier(71, typeof(byte));
        /// <summary>Enables or disables regenerative braking. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier RegenerativeBraking = new SetDataIdentifier(72, typeof(bool));
        /// <summary>Controls the lights on the vehicle. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier Lights = new SetDataIdentifier(73, typeof(bool));
        /// <summary>Controls hazard lights. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier HazardLights = new SetDataIdentifier(74, typeof(bool));
        /// <summary>Activates or deactivates ABS (Anti-lock Braking System). Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier ABS = new SetDataIdentifier(75, typeof(bool));
        /// <summary>Controls brake power setting. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier BrakePower = new SetDataIdentifier(76, typeof(bool));
        /// <summary>Activates or deactivates traction control. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier TractionControl = new SetDataIdentifier(77, typeof(bool));
        /// <summary>Sets the driving mode. Expected type: <c>string</c>. Consider using an enum for defined modes.</summary>
        public static readonly SetDataIdentifier DrivingMode = new SetDataIdentifier(78, typeof(string));
        /// <summary>Sets the maximum speed limit. Expected type: <c>ushort</c></summary>
        public static readonly SetDataIdentifier MaxSpeedSetting = new SetDataIdentifier(79, typeof(ushort));
        /// <summary>Activates or deactivates the handbrake. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier HandBrake = new SetDataIdentifier(80, typeof(bool));
        /// <summary>Activates or deactivates demo mode. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier DemoMode = new SetDataIdentifier(81, typeof(bool));

        // Safety features
        /// <summary>Activates or deactivates the immobilizer. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier Immobilizer = new SetDataIdentifier(90, typeof(bool));
        /// <summary>Activates or deactivates the alarm. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier Alarm = new SetDataIdentifier(91, typeof(bool));
        /// <summary>Sets the geofencing range. Expected type: <c>ushort</c></summary>
        public static readonly SetDataIdentifier Geofencing = new SetDataIdentifier(92, typeof(ushort));
        /// <summary>Indicates if the device or feature is active. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier IsActive = new SetDataIdentifier(100, typeof(bool));


        private SetDataIdentifier(int id, Type expectedType) : base(id, expectedType)
        {
            DataIdentifierRegistry.Register(this);
        }
    }
}