using System;
using System.Collections.Generic;
using System.Formats.Cbor;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using cborModular.DataModels;

namespace cborModular.Services
{
    internal static class CborHandler
    {

        /// <summary>
        /// Encodes a list of DataIdentifiers as a CBOR array.
        /// </summary>     
        /// <returns>CBOR-encoded byte array</returns>
        public static byte[] EncodeRequest()
        {
            var writer = new CborWriter();

            writer.WriteStartArray(DataStorage.GetRequestedIdentifiers().Count);
            foreach (var identifier in DataStorage.GetRequestedIdentifiers())
            {
                writer.WriteInt32((int)identifier);
            }
            writer.WriteEndArray();

            // TODO: bude se muset řešit uložení requestu do přečtení dat z bluetooth.
            DataStorage.ClearRequest();
            return writer.Encode();

        }

        /// <summary>
        /// Decodes a CBOR request to get the list of requested DataIdentifiers.
        /// </summary>
        /// <param name="cborData">CBOR-encoded request data</param>
        /// <returns>List of requested DataIdentifiers</returns>
        public static List<DataIdentifier> DecodeRequest(byte[] cborData)
        {
            var requestedIdentifiers = new List<DataIdentifier>();
            var reader = new CborReader(cborData);

            reader.ReadStartArray();
            while (reader.PeekState() != CborReaderState.EndArray)
            {
                var id = reader.ReadInt32();
                requestedIdentifiers.Add((DataIdentifier)id);
            }
            reader.ReadEndArray();

            return requestedIdentifiers;
        }

        /// <summary>
        /// Encodes response data into CBOR format.
        /// </summary>
        /// <param name="responseData">Dictionary of DataIdentifiers and their values</param>
        /// <returns>CBOR-encoded byte array</returns>
        public static byte[] EncodeResponse(Dictionary<DataIdentifier, object> responseData)
        {
            var writer = new CborWriter();

            writer.WriteStartMap(responseData.Count);
            foreach (var entry in responseData)
            {
                writer.WriteInt32((int)entry.Key);

                switch (entry.Value)
                {
                    case float f:
                        writer.WriteSingle(f);
                        break;
                    case bool b:
                        writer.WriteBoolean(b);
                        break;
                    case int i:
                        writer.WriteInt32(i);
                        break;
                    case uint u:
                        writer.WriteUInt32(u);
                        break;
                    case ushort us:
                        writer.WriteUInt32(us); // Encoded as uint
                        break;
                    case sbyte sb:
                        writer.WriteInt32(sb); // Encoded as int
                        break;
                    case byte bt:
                        writer.WriteUInt32(bt); // Encoded as uint
                        break;
                    case TimeSpan ts:
                        writer.WriteInt64(ts.Ticks); // Encode TimeSpan as ticks (long)
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported data type");
                }
            }
            writer.WriteEndMap();
            return writer.Encode();

        }

        /// <summary>
        /// Decodes a CBOR response and updates the provided storage dictionary with the data.
        /// </summary>
        /// <param name="cborData">CBOR-encoded response data</param>
        public static void DecodeResponse(byte[] cborData)
        {
            var reader = new CborReader(cborData);

            reader.ReadStartMap();
            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var identifier = (DataIdentifier)reader.ReadInt32();
                var expectedType = GetExpectedType(identifier);

                // Decode the value based on the expected type
                object value = expectedType switch
                {
                    Type t when t == typeof(float) => reader.ReadSingle(),
                    Type t when t == typeof(bool) => reader.ReadBoolean(),
                    Type t when t == typeof(ushort) => (ushort)reader.ReadUInt32(),
                    Type t when t == typeof(int) => reader.ReadInt32(),
                    Type t when t == typeof(sbyte) => (sbyte)reader.ReadInt32(),
                    _ => throw new InvalidOperationException($"Unsupported data type for identifier {identifier}")
                };

                // Add value to the list in storage, creating a new list if necessary
                DataStorage.AddData(identifier, value);
            }
            reader.ReadEndMap();
        }
        /// <summary>
        /// Retrieves the expected type for a given DataIdentifier using reflection.
        /// </summary>
        /// <param name="identifier">The DataIdentifier enum value</param>
        /// <returns>Type of the expected value for the identifier</returns>
        private static Type GetExpectedType(DataIdentifier identifier)
        {
            var memberInfo = typeof(DataIdentifier).GetMember(identifier.ToString())[0];
            var attribute = memberInfo.GetCustomAttribute<DataTypeAttribute>();
            return attribute?.ExpectedType ?? throw new InvalidOperationException("Data type not specified for identifier.");
        }
    }
}
